// Copyright (c) 2011 Smarkets Limited
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
// BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Threading;

using log4net;
using ProtoBuf;

using IronSmarkets.Data;
using IronSmarkets.Events;
using IronSmarkets.Exceptions;
using IronSmarkets.Sessions;

namespace IronSmarkets.Clients
{
    public interface ISmarketsClient :
        IDisposable,
        IPayloadEvents<Proto.Seto.Payload>,
        IPayloadEndpoint<Proto.Seto.Payload>
    {
        bool IsDisposed { get; }

        ulong Login();
        ulong Logout();

        ulong Ping();

        ulong SubscribeMarket(Uid market);
        ulong UnsubscribeMarket(Uid market);

        ulong RequestOrdersForAccount();
        ulong RequestOrdersForMarket(Uid market);

        Response<IEventMap> RequestEvents(EventQuery query);
        Response<AccountState> GetAccountState();
        Response<AccountState> GetAccountState(Uid account);

        Response<MarketQuotes> GetMarketQuotes(Uid market);
    }

    public sealed class SmarketsClient : ISmarketsClient
    {
        private static readonly ILog Log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IClientSettings _settings;
        private readonly ISession<Proto.Seto.Payload> _session;
        private readonly Receiver<Proto.Seto.Payload> _receiver;

        private readonly IDictionary<ulong, SyncRequest<Proto.Seto.Events>> _eventsRequests =
            new Dictionary<ulong, SyncRequest<Proto.Seto.Events>>();
        private readonly object _eventsReqLock = new object();

        private readonly IDictionary<ulong, SyncRequest<Proto.Seto.AccountState>> _accountRequests =
            new Dictionary<ulong, SyncRequest<Proto.Seto.AccountState>>();
        private readonly object _accountReqLock = new object();

        private readonly IDictionary<Uid, Queue<SyncRequest<Proto.Seto.MarketQuotes>>> _marketQuotesRequests =
            new Dictionary<Uid, Queue<SyncRequest<Proto.Seto.MarketQuotes>>>();
        private readonly object _marketQuotesReqLock = new object();

        private readonly IDictionary<Uid, EventHandler<QuotesReceivedEventArgs<Proto.Seto.MarketQuotes>>> _marketQuotesHandlers =
            new Dictionary<Uid, EventHandler<QuotesReceivedEventArgs<Proto.Seto.MarketQuotes>>>();
        private readonly object _marketQuotesHandlerLock = new object();
        private readonly IDictionary<Uid, EventHandler<QuotesReceivedEventArgs<Proto.Seto.ContractQuotes>>> _contractQuotesHandlers =
            new Dictionary<Uid, EventHandler<QuotesReceivedEventArgs<Proto.Seto.ContractQuotes>>>();
        private readonly object _contractQuotesHandlerLock = new object();

        private int _disposed;

        private SmarketsClient(IClientSettings settings)
        {
            _settings = settings;
            _session = new SeqSession(
                _settings.SocketSettings,
                _settings.SessionSettings);
            _session.PayloadReceived += (sender, args) =>
                OnPayloadReceived(args.Payload);
            _session.PayloadSent += (sender, args) =>
                OnPayloadSent(args.Payload);
            AddPayloadHandler(HandlePayload);
            _receiver = new Receiver<Proto.Seto.Payload>(_session);
        }

        public static ISmarketsClient Create(IClientSettings settings)
        {
            return new SmarketsClient(settings);
        }

        public bool IsDisposed
        {
            get
            {
                return Thread.VolatileRead(ref _disposed) == 1;
            }
        }

        public event EventHandler<PayloadReceivedEventArgs<Proto.Seto.Payload>> PayloadReceived;
        public event EventHandler<PayloadReceivedEventArgs<Proto.Seto.Payload>> PayloadSent;

        ~SmarketsClient()
        {
            Dispose(false);
        }

        public void AddPayloadHandler(Predicate<Proto.Seto.Payload> predicate)
        {
            _session.AddPayloadHandler(predicate);
        }

        public void RemovePayloadHandler(Predicate<Proto.Seto.Payload> predicate)
        {
            _session.RemovePayloadHandler(predicate);
        }

        public void SendPayload(Proto.Seto.Payload payload)
        {
            _session.SendPayload(payload);
        }

        public ulong Login()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called Logout on disposed object");

            ulong seq = _session.Login();
            _receiver.Start();
            return seq;
        }

        public ulong Logout()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called Logout on disposed object");

            ulong seq = _session.Logout();
            _receiver.Stop();
            return seq;
        }

        public ulong Ping()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called Ping on disposed object");

            var payload = new Proto.Seto.Payload {
                Type = Proto.Seto.PayloadType.PAYLOADETO,
                EtoPayload = new Proto.Eto.Payload {
                    Type = Proto.Eto.PayloadType.PAYLOADPING
                }
            };

            SendPayload(payload);
            return payload.EtoPayload.Seq;
        }

        public ulong SubscribeMarket(Uid market)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called SubscribeMarket on disposed object");

            var payload = new Proto.Seto.Payload {
                Type = Proto.Seto.PayloadType.PAYLOADMARKETSUBSCRIBE,
                MarketSubscribe = new Proto.Seto.MarketSubscribe {
                    Market = market.ToUuid128()
                }
            };

            SendPayload(payload);
            return payload.EtoPayload.Seq;
        }

        public ulong UnsubscribeMarket(Uid market)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called UnsubscribeMarket on disposed object");

            var payload = new Proto.Seto.Payload {
                Type = Proto.Seto.PayloadType.PAYLOADMARKETUNSUBSCRIBE,
                MarketUnsubscribe = new Proto.Seto.MarketUnsubscribe {
                    Market = market.ToUuid128()
                }
            };

            SendPayload(payload);
            return payload.EtoPayload.Seq;
        }

        public ulong RequestOrdersForMarket(Uid market)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called RequestOrdersForMarket on disposed object");

            var payload = new Proto.Seto.Payload {
                Type = Proto.Seto.PayloadType.PAYLOADORDERSFORMARKETREQUEST,
                OrdersForMarketRequest = new Proto.Seto.OrdersForMarketRequest {
                    Market = market.ToUuid128()
                }
            };

            SendPayload(payload);
            return payload.EtoPayload.Seq;
        }

        public ulong RequestOrdersForAccount()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called RequestOrdersForAccount on disposed object");

            var payload = new Proto.Seto.Payload {
                Type = Proto.Seto.PayloadType.PAYLOADORDERSFORACCOUNTREQUEST,
                OrdersForAccountRequest = new Proto.Seto.OrdersForAccountRequest()
            };

            SendPayload(payload);
            return payload.EtoPayload.Seq;
        }

        public Response<IEventMap> RequestEvents(EventQuery query)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called RequestEvents on disposed object");

            var payload = new Proto.Seto.Payload {
                Type = Proto.Seto.PayloadType.PAYLOADEVENTSREQUEST,
                EventsRequest = query.ToEventsRequest()
            };

            var req = new SyncRequest<Proto.Seto.Events>();
            lock (_eventsReqLock)
            {

                // XXX: At the moment, SendPayload needs to be inside
                // the lock because the receiver thread could
                // theoretically receive the payload before the
                // SyncRequest is added to the dictionary. Although
                // very unlikely, I am opting for safer in this case
                // (as well as other cases which exhibit the same
                // pattern). It may make sense instead to track the
                // last sequence number for these types of requests in
                // a volatile long variable and spin-wait in the
                // receiver where necessary.
                SendPayload(payload);
                _eventsRequests[payload.EtoPayload.Seq] = req;
            }
            return new Response<IEventMap>(
                payload.EtoPayload.Seq,
                EventMap.FromSeto(req.Response));
        }

        public Response<AccountState> GetAccountState()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetAccount on disposed object");

            return GetAccountState(new Proto.Seto.AccountStateRequest());
        }

        public Response<AccountState> GetAccountState(Uid account)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetAccount on disposed object");

            return GetAccountState(
                new Proto.Seto.AccountStateRequest {
                    Account = account.ToUuid128()
                });
        }

        private Response<AccountState> GetAccountState(
            Proto.Seto.AccountStateRequest request)
        {
            var payload = new Proto.Seto.Payload {
                Type = Proto.Seto.PayloadType.PAYLOADACCOUNTSTATEREQUEST,
                AccountStateRequest = request
            };
            var req = new SyncRequest<Proto.Seto.AccountState>();
            lock (_accountReqLock)
            {
                SendPayload(payload);
                _accountRequests[payload.EtoPayload.Seq] = req;
            }
            return new Response<AccountState>(
                payload.EtoPayload.Seq,
                AccountState.FromSeto(req.Response));
        }

        public Response<MarketQuotes> GetMarketQuotes(Uid market)
        {
            var payload = new Proto.Seto.Payload {
                Type = Proto.Seto.PayloadType.PAYLOADMARKETQUOTESREQUEST,
                MarketQuotesRequest = new Proto.Seto.MarketQuotesRequest {
                    Market = market.ToUuid128()
                }
            };
            var req = new SyncRequest<Proto.Seto.MarketQuotes>();
            lock (_marketQuotesReqLock)
            {
                SendPayload(payload);
                if (!_marketQuotesRequests.ContainsKey(market))
                {
                    _marketQuotesRequests[market] =
                        new Queue<SyncRequest<Proto.Seto.MarketQuotes>>();
                }
                _marketQuotesRequests[market].Enqueue(req);
            }
            return new Response<MarketQuotes>(
                payload.EtoPayload.Seq,
                MarketQuotes.FromSeto(req.Response));
        }

        public void AddMarketQuotesHandler(Uid uid, EventHandler<QuotesReceivedEventArgs<Proto.Seto.MarketQuotes>> handler)
        {
            lock (_marketQuotesHandlerLock)
            {
                EventHandler<QuotesReceivedEventArgs<Proto.Seto.MarketQuotes>> md;
                if (!_marketQuotesHandlers.TryGetValue(uid, out md))
                {
                    _marketQuotesHandlers[uid] = handler;
                }
                else
                {
                    _marketQuotesHandlers[uid] = md + handler;
                }
            }
        }

        public void RemoveMarketQuotesHandler(Uid uid, EventHandler<QuotesReceivedEventArgs<Proto.Seto.MarketQuotes>> handler)
        {
            lock (_marketQuotesHandlerLock)
            {
                EventHandler<QuotesReceivedEventArgs<Proto.Seto.MarketQuotes>> md = _marketQuotesHandlers[uid];
                md -= handler;
                if (md == null)
                {
                    _marketQuotesHandlers.Remove(uid);
                }
                else
                {
                    _marketQuotesHandlers[uid] = md;
                }
            }
        }

        public void AddContractQuotesHandler(Uid uid, EventHandler<QuotesReceivedEventArgs<Proto.Seto.ContractQuotes>> handler)
        {
            lock (_contractQuotesHandlerLock)
            {
                EventHandler<QuotesReceivedEventArgs<Proto.Seto.ContractQuotes>> md;
                if (!_contractQuotesHandlers.TryGetValue(uid, out md))
                {
                    _contractQuotesHandlers[uid] = handler;
                }
                else
                {
                    _contractQuotesHandlers[uid] = md + handler;
                }
            }
        }

        public void RemoveContractQuotesHandler(Uid uid, EventHandler<QuotesReceivedEventArgs<Proto.Seto.ContractQuotes>> handler)
        {
            lock (_contractQuotesHandlerLock)
            {
                EventHandler<QuotesReceivedEventArgs<Proto.Seto.ContractQuotes>> md = _contractQuotesHandlers[uid];
                md -= handler;
                if (md == null)
                {
                    _contractQuotesHandlers.Remove(uid);
                }
                else
                {
                    _contractQuotesHandlers[uid] = md;
                }
            }
        }

        private void OnPayloadReceived(Proto.Seto.Payload payload)
        {
            EventHandler<PayloadReceivedEventArgs<Proto.Seto.Payload>> ev = PayloadReceived;
            if (ev != null)
                ev(this, new PayloadReceivedEventArgs<Proto.Seto.Payload>(
                       payload.EtoPayload.Seq,
                       payload));
        }

        private bool HandlePayload(Proto.Seto.Payload payload)
        {
            switch (payload.Type)
            {
                case Proto.Seto.PayloadType.PAYLOADHTTPFOUND:
                    HandleEventsHttpFound(payload);
                    break;
                case Proto.Seto.PayloadType.PAYLOADACCOUNTSTATE:
                    HandleAccountState(payload);
                    break;
                case Proto.Seto.PayloadType.PAYLOADMARKETQUOTES:
                    HandleMarketQuotes(payload);
                    break;
                case Proto.Seto.PayloadType.PAYLOADCONTRACTQUOTES:
                    HandleContractQuotes(payload);
                    break;
            }

            return true;
        }

        private void OnPayloadSent(Proto.Seto.Payload payload)
        {
            EventHandler<PayloadReceivedEventArgs<Proto.Seto.Payload>> ev = PayloadSent;
            if (ev != null)
                ev(this, new PayloadReceivedEventArgs<Proto.Seto.Payload>(
                       payload.EtoPayload.Seq,
                       payload));
        }

        private void HandleAccountState(Proto.Seto.Payload payload)
        {
            SyncRequest<Proto.Seto.AccountState> req;
            lock (_accountReqLock)
            {
                if (_accountRequests.TryGetValue(
                        payload.EtoPayload.Seq,
                        out req)) {
                    _accountRequests.Remove(payload.EtoPayload.Seq);
                }
            }
            if (req != null) {
                req.Response = payload.AccountState;
            }
            else
            {
                Log.Warn(
                    "Received ACCOUNT_STATE payload " +
                    "but could not find original request");
            }
        }

        private void HandleEventsHttpFound(Proto.Seto.Payload payload)
        {
            SyncRequest<Proto.Seto.Events> req;
            lock (_eventsReqLock)
            {
                if (_eventsRequests.TryGetValue(
                        payload.EtoPayload.Seq,
                        out req)) {
                    _eventsRequests.Remove(payload.EtoPayload.Seq);
                }
            }
            if (req != null)
            {
                BeginFetchHttpFound(req, payload);
            }
            else
            {
                Log.Warn(
                    "Received HTTP_FOUND payload " +
                    "but could not find original request");
            }
        }

        private void HandleMarketQuotes(Proto.Seto.Payload payload)
        {
            Uid market = Uid.FromUuid128(payload.MarketQuotes.Market);
            // First, respond to a possible synchronous request
            SyncRequest<Proto.Seto.MarketQuotes> req = null;
            lock (_marketQuotesReqLock)
            {
                Queue<SyncRequest<Proto.Seto.MarketQuotes>> queue;
                if (_marketQuotesRequests.TryGetValue(market, out queue))
                {
                    Debug.Assert(queue.Count > 0);
                    req = queue.Dequeue();
                    if (queue.Count == 0)
                    {
                        _marketQuotesRequests.Remove(market);
                    }
                }
            }
            if (req != null)
                req.Response = payload.MarketQuotes;

            // Dispatch updates to all listeners
            EventHandler<QuotesReceivedEventArgs<Proto.Seto.MarketQuotes>> handler = null;
            lock (_marketQuotesHandlerLock)
            {
                _marketQuotesHandlers.TryGetValue(market, out handler);
            }
            if (handler != null)
                handler(this, new QuotesReceivedEventArgs<Proto.Seto.MarketQuotes>(
                       payload.EtoPayload.Seq,
                       payload.MarketQuotes));
        }

        private void HandleContractQuotes(Proto.Seto.Payload payload)
        {
            Uid contract = Uid.FromUuid128(payload.ContractQuotes.Contract);
            // Dispatch updates to all listeners
            EventHandler<QuotesReceivedEventArgs<Proto.Seto.ContractQuotes>> handler = null;
            lock (_contractQuotesHandlerLock)
            {
                _contractQuotesHandlers.TryGetValue(contract, out handler);
            }
            if (handler != null)
                handler(this, new QuotesReceivedEventArgs<Proto.Seto.ContractQuotes>(
                       payload.EtoPayload.Seq,
                       payload.ContractQuotes));
        }

        private IAsyncResult BeginFetchHttpFound<T>(
            SyncRequest<T> syncRequest, Proto.Seto.Payload payload)
        {
            var url = payload.HttpFound.Url;
            if (Log.IsDebugEnabled) Log.Debug(
                string.Format("Fetching payload from URL {0}", url));
            var req = (HttpWebRequest)WebRequest.Create(url);
            var state = new Tuple<HttpWebRequest, SyncRequest<T>>(
                req, syncRequest);
            var result = req.BeginGetResponse(
                FetchHttpFoundCallback<T>, state);
            ThreadPool.RegisterWaitForSingleObject(
                result.AsyncWaitHandle,
                HttpFoundTimeoutCallback<T>,
                state, _settings.HttpRequestTimeout, true);
            return result;
        }

        private void FetchHttpFoundCallback<T>(IAsyncResult result)
        {
            var stateTuple = (Tuple<HttpWebRequest, SyncRequest<T>>)result.AsyncState;
            if (Log.IsDebugEnabled) Log.Debug(
                string.Format("Web request callback for URL {0}", stateTuple.Item1.RequestUri));
            try
            {
                using (var resp = stateTuple.Item1.EndGetResponse(result))
                {
                    if (Log.IsDebugEnabled) Log.Debug("Received a response, deserializing");
                    Stream receiveStream = resp.GetResponseStream();
                    stateTuple.Item2.Response = Serializer.Deserialize<T>(receiveStream);
                }
            }
            catch (WebException wex)
            {
                if (wex.Message == "Aborted.")
                {
                    stateTuple.Item2.SetException(
                        new RequestTimedOutException(_settings.HttpRequestTimeout));
                }
                else
                {
                    stateTuple.Item2.SetException(wex);
                }
            }
            catch (Exception ex)
            {
                stateTuple.Item2.SetException(ex);
            }
        }

        private static void HttpFoundTimeoutCallback<T>(object state, bool timedOut)
        {
            if (timedOut)
            {
                var stateTuple = (Tuple<HttpWebRequest, SyncRequest<T>>)state;
                stateTuple.Item1.Abort();
            }
        }

        private void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                if (disposing)
                {
                    var dispSession = _session as IDisposable;
                    if (dispSession != null)
                        dispSession.Dispose();
                }
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
