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
using System.Threading;

using log4net;

using IronSmarkets.Data;
using IronSmarkets.Events;
using IronSmarkets.Sessions;

namespace IronSmarkets.Clients
{
    public interface ISmarketsClient :
        IDisposable,
        IPayloadEvents<Proto.Seto.Payload>,
        IPayloadEndpoint<Proto.Seto.Payload>,
        IQuoteSink
    {
        bool IsDisposed { get; }

        ulong Login();
        ulong Logout();

        ulong Ping();

        Response<IEventMap> GetEvents(EventQuery query);
        Response<AccountState> GetAccountState();
        Response<AccountState> GetAccountState(Uid account);

        Response<MarketQuotes> GetQuotesByMarket(Uid market);

        Response<IOrderMap> GetOrders();
        Response<IOrderMap> GetOrdersByMarket(Uid market);

        void CancelOrder(Order order);
    }

    public sealed class SmarketsClient : ISmarketsClient
    {
        private static readonly ILog Log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IClientSettings _settings;
        private readonly ISession<Proto.Seto.Payload> _session;
        private readonly Receiver<Proto.Seto.Payload> _receiver;

        private readonly SeqRpcHandler<Proto.Seto.Events, IEventMap> _eventsRequestHandler;
        private readonly SeqRpcHandler<Proto.Seto.AccountState, AccountState> _accountStateRequestHandler;
        private readonly UidQueueRpcHandler<Proto.Seto.MarketQuotes, MarketQuotes> _marketQuotesRequestHandler;
        private readonly SeqRpcHandler<Proto.Seto.OrdersForAccount, IOrderMap> _ordersByAccountRequestHandler;
        private readonly UidQueueRpcHandler<Proto.Seto.OrdersForMarket, IOrderMap> _ordersByMarketRequestHandler;
        private readonly HttpFoundHandler<Proto.Seto.Events> _httpHandler;

        private readonly QuoteHandler<Proto.Seto.MarketQuotes> _marketQuotesHandler =
            new QuoteHandler<Proto.Seto.MarketQuotes>(
                payload => new UidPair<Proto.Seto.MarketQuotes>(
                    Uid.FromUuid128(payload.MarketQuotes.Market), payload.MarketQuotes));

        private readonly QuoteHandler<Proto.Seto.ContractQuotes> _contractQuotesHandler =
            new QuoteHandler<Proto.Seto.ContractQuotes>(
                payload => new UidPair<Proto.Seto.ContractQuotes>(
                    Uid.FromUuid128(payload.ContractQuotes.Contract), payload.ContractQuotes));

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
            _httpHandler = new HttpFoundHandler<Proto.Seto.Events>(_settings.HttpRequestTimeout);

            _eventsRequestHandler = new SeqRpcHandler<Proto.Seto.Events, IEventMap>(
                this, EventMap.FromSeto, _httpHandler.BeginFetchHttpFound);
            _accountStateRequestHandler = new SeqRpcHandler<Proto.Seto.AccountState, AccountState>(
                this, AccountState.FromSeto, (req, payload) => { req.Response = payload.AccountState; });
            _marketQuotesRequestHandler = new UidQueueRpcHandler<Proto.Seto.MarketQuotes, MarketQuotes>(
                this, MarketQuotes.FromSeto, (req, payload) => { req.Response = payload.MarketQuotes; });
            _ordersByAccountRequestHandler = new SeqRpcHandler<Proto.Seto.OrdersForAccount, IOrderMap>(
                this, OrderMap.FromSeto, (req, payload) => { req.Response = payload.OrdersForAccount; });
            _ordersByMarketRequestHandler = new UidQueueRpcHandler<Proto.Seto.OrdersForMarket, IOrderMap>(
                this, OrderMap.FromSeto, (req, payload) => { req.Response = payload.OrdersForMarket; });
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
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called AddPayloadHandler on disposed object");

            _session.AddPayloadHandler(predicate);
        }

        public void RemovePayloadHandler(Predicate<Proto.Seto.Payload> predicate)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called RemovePayloadHandler on disposed object");

            _session.RemovePayloadHandler(predicate);
        }

        public void AddMarketQuotesHandler(Uid uid, EventHandler<QuotesReceivedEventArgs<Proto.Seto.MarketQuotes>> handler)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called AddMarketQuotesHandler on disposed object");

            _marketQuotesHandler.AddHandler(uid, handler);
        }

        public void RemoveMarketQuotesHandler(Uid uid, EventHandler<QuotesReceivedEventArgs<Proto.Seto.MarketQuotes>> handler)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called RemoveMarketQuotesHandler on disposed object");

            _marketQuotesHandler.RemoveHandler(uid, handler);
        }

        public void AddContractQuotesHandler(Uid uid, EventHandler<QuotesReceivedEventArgs<Proto.Seto.ContractQuotes>> handler)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called AddContractQuotesHandler on disposed object");

            _contractQuotesHandler.AddHandler(uid, handler);
        }

        public void RemoveContractQuotesHandler(Uid uid, EventHandler<QuotesReceivedEventArgs<Proto.Seto.ContractQuotes>> handler)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called RemoveContractQuotesHandler on disposed object");

            _contractQuotesHandler.RemoveHandler(uid, handler);
        }

        public void SendPayload(Proto.Seto.Payload payload)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called SendPayload on disposed object");

            _session.SendPayload(payload);
        }

        public ulong Login()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called Login on disposed object");

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

        public Response<IEventMap> GetEvents(EventQuery query)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetEvents on disposed object");

            return _eventsRequestHandler.Request(
                new Proto.Seto.Payload {
                    Type = Proto.Seto.PayloadType.PAYLOADEVENTSREQUEST,
                        EventsRequest = query.ToEventsRequest()
                        });
        }

        public Response<AccountState> GetAccountState()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetAccountState on disposed object");

            return GetAccountState(new Proto.Seto.AccountStateRequest());
        }

        public Response<AccountState> GetAccountState(Uid account)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetAccountState on disposed object");

            return GetAccountState(
                new Proto.Seto.AccountStateRequest {
                    Account = account.ToUuid128()
                });
        }

        private Response<AccountState> GetAccountState(
            Proto.Seto.AccountStateRequest request)
        {
            return _accountStateRequestHandler.Request(
                new Proto.Seto.Payload {
                    Type = Proto.Seto.PayloadType.PAYLOADACCOUNTSTATEREQUEST,
                        AccountStateRequest = request
                        });
        }

        public Response<MarketQuotes> GetQuotesByMarket(Uid market)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetQuotesByMarket on disposed object");

            return _marketQuotesRequestHandler.Request(
                market,
                new Proto.Seto.Payload {
                    Type = Proto.Seto.PayloadType.PAYLOADMARKETQUOTESREQUEST,
                        MarketQuotesRequest = new Proto.Seto.MarketQuotesRequest {
                        Market = market.ToUuid128()
                    }
                });
        }

        public Response<IOrderMap> GetOrders()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetOrdersByAccount on disposed object");

            return _ordersByAccountRequestHandler.Request(
                new Proto.Seto.Payload {
                    Type = Proto.Seto.PayloadType.PAYLOADORDERSFORACCOUNTREQUEST,
                    OrdersForAccountRequest = new Proto.Seto.OrdersForAccountRequest()
                });
        }

        public Response<IOrderMap> GetOrdersByMarket(Uid market)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetOrdersByMarket on disposed object");

            return _ordersByMarketRequestHandler.Request(
                market,
                new Proto.Seto.Payload {
                    Type = Proto.Seto.PayloadType.PAYLOADORDERSFORMARKETREQUEST,
                        OrdersForMarketRequest = new Proto.Seto.OrdersForMarketRequest {
                        Market = market.ToUuid128()
                    }
                });
        }

        public void CancelOrder(Order order)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called CancelOrder on disposed object");

            if (!order.Cancellable)
                throw new InvalidOperationException(
                    string.Format(
                        "Order cannot be cancelled: {0}", order.State.Status));

            SendPayload(
                new Proto.Seto.Payload {
                    Type = Proto.Seto.PayloadType.PAYLOADORDERCANCEL,
                        OrderCancel = new Proto.Seto.OrderCancel {
                        Order = order.Uid.ToUuid128()
                    }
                });
        }

        private void OnPayloadReceived(Proto.Seto.Payload payload)
        {
            EventHandler<PayloadReceivedEventArgs<Proto.Seto.Payload>> ev = PayloadReceived;
            if (ev != null)
                ev(this, new PayloadReceivedEventArgs<Proto.Seto.Payload>(
                       payload.EtoPayload.Seq,
                       payload));
        }

        private void OnPayloadSent(Proto.Seto.Payload payload)
        {
            EventHandler<PayloadReceivedEventArgs<Proto.Seto.Payload>> ev = PayloadSent;
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
                    _eventsRequestHandler.Handle(payload);
                    break;
                case Proto.Seto.PayloadType.PAYLOADACCOUNTSTATE:
                    _accountStateRequestHandler.Handle(payload);
                    break;
                case Proto.Seto.PayloadType.PAYLOADMARKETQUOTES:
                    // First, respond to a possible synchronous request
                    _marketQuotesRequestHandler.Handle(
                        Uid.FromUuid128(payload.MarketQuotes.Market),
                        payload);
                    // Dispatch updates to all listeners
                    _marketQuotesHandler.Handle(payload);
                    break;
                case Proto.Seto.PayloadType.PAYLOADCONTRACTQUOTES:
                    _contractQuotesHandler.Handle(payload);
                    break;
                case Proto.Seto.PayloadType.PAYLOADORDERSFORMARKET:
                    _ordersByMarketRequestHandler.Handle(
                        Uid.FromUuid128(payload.OrdersForMarket.Market),
                        payload);
                    break;
                case Proto.Seto.PayloadType.PAYLOADORDERSFORACCOUNT:
                    _ordersByAccountRequestHandler.Handle(payload);
                    break;
            }

            return true;
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
