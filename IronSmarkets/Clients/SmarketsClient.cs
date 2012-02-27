// Copyright (c) 2011-2012 Smarkets Limited
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
using IronSmarkets.Exceptions;
using IronSmarkets.Sessions;
using IronSmarkets.Sockets;

using PS = IronSmarkets.Proto.Seto;
using PE = IronSmarkets.Proto.Eto;

namespace IronSmarkets.Clients
{
    public interface ISmarketsClient :
        IDisposable,
        IPayloadEvents<PS.Payload>,
        IPayloadEndpoint<PS.Payload>,
        IQuoteSink
    {
        bool IsDisposed { get; }

        ulong Login();
        ulong Logout();

        ulong Ping();

        IEventMap EventMap { get; }
        IOrderMap OrderMap { get; }

        Response<IEventMap> GetEvents(EventQuery query);
        Response<AccountState> GetAccountState();
        Response<AccountState> GetAccountState(Uid account);

        Response<MarketQuotes> GetQuotesByMarket(Uid market);

        Response<IOrderMap> GetOrders();
        Response<IOrderMap> GetOrdersByMarket(Uid market);

        void CancelOrder(Order order);
        Response<Order> CreateOrder(NewOrder order);
    }

    public sealed class SmarketsClient : ISmarketsClient
    {
        private static readonly ILog Log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IClientSettings _settings;
        private readonly ISession<PS.Payload> _session;
        private readonly Receiver<PS.Payload> _receiver;

        private readonly IEventMap _eventMap = new EventMap();
        private readonly IOrderMap _orderMap = new OrderMap();

        private readonly SeqRpcHandler<PS.Events, IEventMap> _eventsRequestHandler;
        private readonly SeqRpcHandler<PS.AccountState, AccountState> _accountStateRequestHandler;
        private readonly UidQueueRpcHandler<PS.MarketQuotes, MarketQuotes> _marketQuotesRequestHandler;
        private readonly SeqRpcHandler<PS.OrdersForAccount, IOrderMap> _ordersByAccountRequestHandler;
        private readonly UidQueueRpcHandler<PS.OrdersForMarket, IOrderMap> _ordersByMarketRequestHandler;
        private readonly OrderCreateRequestHandler _orderCreateRequestHandler;
        private readonly IAsyncHttpFoundHandler<PS.Events> _httpHandler;

        private readonly QuoteHandler<PS.MarketQuotes> _marketQuotesHandler =
            new QuoteHandler<PS.MarketQuotes>(
                payload => new UidPair<PS.MarketQuotes>(
                    Uid.FromUuid128(payload.MarketQuotes.Market), payload.MarketQuotes));

        private readonly QuoteHandler<PS.ContractQuotes> _contractQuotesHandler =
            new QuoteHandler<PS.ContractQuotes>(
                payload => new UidPair<PS.ContractQuotes>(
                    Uid.FromUuid128(payload.ContractQuotes.Contract), payload.ContractQuotes));

        private int _disposed;

        private SmarketsClient(
            IClientSettings settings,
            ISession<PS.Payload> session,
            IAsyncHttpFoundHandler<PS.Events> httpHandler)
        {
            _settings = settings;
            _session = session;
            _session.PayloadReceived += (sender, args) =>
                OnPayloadReceived(args.Payload);
            _session.PayloadSent += (sender, args) =>
                OnPayloadSent(args.Payload);
            AddPayloadHandler(HandlePayload);
            _receiver = new Receiver<PS.Payload>(_session);
            _httpHandler = httpHandler;

            _eventsRequestHandler = new SeqRpcHandler<PS.Events, IEventMap>(
                this, Data.EventMap.FromSeto, ExtractEventResponse);
            _accountStateRequestHandler = new SeqRpcHandler<PS.AccountState, AccountState>(
                this, AccountState.FromSeto, (req, payload) => { req.Response = payload.AccountState; });
            _marketQuotesRequestHandler = new UidQueueRpcHandler<PS.MarketQuotes, MarketQuotes>(
                this, MarketQuotes.FromSeto, (req, payload) => { req.Response = payload.MarketQuotes; });
            _ordersByAccountRequestHandler = new SeqRpcHandler<PS.OrdersForAccount, IOrderMap>(
                this, Data.OrderMap.FromSeto, (req, payload) => { req.Response = payload.OrdersForAccount; });
            _ordersByMarketRequestHandler = new UidQueueRpcHandler<PS.OrdersForMarket, IOrderMap>(
                this, Data.OrderMap.FromSeto, (req, payload) => { req.Response = payload.OrdersForMarket; });
            _orderCreateRequestHandler = new OrderCreateRequestHandler(this);
        }

        private void ExtractEventResponse(
            SyncRequest<PS.Events> request, PS.Payload payload)
        {
            switch (payload.Type)
            {
                case PS.PayloadType.PAYLOADINVALIDREQUEST:
                    request.SetException(InvalidRequestException.FromSeto(payload.InvalidRequest));
                    break;
                case PS.PayloadType.PAYLOADHTTPFOUND:
                    _httpHandler.BeginFetchHttpFound(request, payload);
                    break;
            }
        }

        public static ISmarketsClient Create(
            IClientSettings settings,
            ISession<PS.Payload> session = null,
            IAsyncHttpFoundHandler<PS.Events> httpHandler = null)
        {
            if (session == null)
                session = new SeqSession(
                    new SessionSocket(settings.SocketSettings),
                    settings.SessionSettings);

            if (httpHandler == null)
                httpHandler = new HttpFoundHandler<PS.Events>(
                    settings.HttpRequestTimeout);
            return new SmarketsClient(settings, session, httpHandler);
        }

        public bool IsDisposed
        {
            get
            {
                return Thread.VolatileRead(ref _disposed) == 1;
            }
        }

        public IEventMap EventMap
        {
            get
            {
                return _eventMap;
            }
        }

        public IOrderMap OrderMap
        {
            get
            {
                return _orderMap;
            }
        }

        public event EventHandler<PayloadReceivedEventArgs<PS.Payload>> PayloadReceived;
        public event EventHandler<PayloadReceivedEventArgs<PS.Payload>> PayloadSent;

        ~SmarketsClient()
        {
            Dispose(false);
        }

        public void AddPayloadHandler(Predicate<PS.Payload> predicate)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called AddPayloadHandler on disposed object");

            _session.AddPayloadHandler(predicate);
        }

        public void RemovePayloadHandler(Predicate<PS.Payload> predicate)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called RemovePayloadHandler on disposed object");

            _session.RemovePayloadHandler(predicate);
        }

        public void AddMarketQuotesHandler(Uid uid, EventHandler<QuotesReceivedEventArgs<PS.MarketQuotes>> handler)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called AddMarketQuotesHandler on disposed object");

            _marketQuotesHandler.AddHandler(uid, handler);
        }

        public void RemoveMarketQuotesHandler(Uid uid, EventHandler<QuotesReceivedEventArgs<PS.MarketQuotes>> handler)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called RemoveMarketQuotesHandler on disposed object");

            _marketQuotesHandler.RemoveHandler(uid, handler);
        }

        public void AddContractQuotesHandler(Uid uid, EventHandler<QuotesReceivedEventArgs<PS.ContractQuotes>> handler)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called AddContractQuotesHandler on disposed object");

            _contractQuotesHandler.AddHandler(uid, handler);
        }

        public void RemoveContractQuotesHandler(Uid uid, EventHandler<QuotesReceivedEventArgs<PS.ContractQuotes>> handler)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called RemoveContractQuotesHandler on disposed object");

            _contractQuotesHandler.RemoveHandler(uid, handler);
        }

        public void SendPayload(PS.Payload payload)
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

            var payload = new PS.Payload {
                Type = PS.PayloadType.PAYLOADETO,
                EtoPayload = new PE.Payload {
                    Type = PE.PayloadType.PAYLOADPING
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

            var payload = new PS.Payload {
                Type = PS.PayloadType.PAYLOADMARKETSUBSCRIBE,
                MarketSubscribe = new PS.MarketSubscribe {
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

            var payload = new PS.Payload {
                Type = PS.PayloadType.PAYLOADMARKETUNSUBSCRIBE,
                MarketUnsubscribe = new PS.MarketUnsubscribe {
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
                new PS.Payload {
                    Type = PS.PayloadType.PAYLOADEVENTSREQUEST,
                        EventsRequest = query.ToEventsRequest()
                        });
        }

        public Response<AccountState> GetAccountState()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetAccountState on disposed object");

            return GetAccountState(new PS.AccountStateRequest());
        }

        public Response<AccountState> GetAccountState(Uid account)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetAccountState on disposed object");

            return GetAccountState(
                new PS.AccountStateRequest {
                    Account = account.ToUuid128()
                });
        }

        private Response<AccountState> GetAccountState(
            PS.AccountStateRequest request)
        {
            return _accountStateRequestHandler.Request(
                new PS.Payload {
                    Type = PS.PayloadType.PAYLOADACCOUNTSTATEREQUEST,
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
                new PS.Payload {
                    Type = PS.PayloadType.PAYLOADMARKETQUOTESREQUEST,
                        MarketQuotesRequest = new PS.MarketQuotesRequest {
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
                new PS.Payload {
                    Type = PS.PayloadType.PAYLOADORDERSFORACCOUNTREQUEST,
                    OrdersForAccountRequest = new PS.OrdersForAccountRequest()
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
                new PS.Payload {
                    Type = PS.PayloadType.PAYLOADORDERSFORMARKETREQUEST,
                        OrdersForMarketRequest = new PS.OrdersForMarketRequest {
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
                new PS.Payload {
                    Type = PS.PayloadType.PAYLOADORDERCANCEL,
                        OrderCancel = new PS.OrderCancel {
                        Order = order.Uid.ToUuid128()
                    }
                });
        }

        public Response<Order> CreateOrder(NewOrder order)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called CreateOrder on disposed object");

            return _orderCreateRequestHandler.Request(order);
        }

        private void OnPayloadReceived(PS.Payload payload)
        {
            EventHandler<PayloadReceivedEventArgs<PS.Payload>> ev = PayloadReceived;
            if (ev != null)
                ev(this, new PayloadReceivedEventArgs<PS.Payload>(
                       payload.EtoPayload.Seq,
                       payload));
        }

        private void OnPayloadSent(PS.Payload payload)
        {
            EventHandler<PayloadReceivedEventArgs<PS.Payload>> ev = PayloadSent;
            if (ev != null)
                ev(this, new PayloadReceivedEventArgs<PS.Payload>(
                       payload.EtoPayload.Seq,
                       payload));
        }

        private bool HandlePayload(PS.Payload payload)
        {
            switch (payload.Type)
            {
                case PS.PayloadType.PAYLOADINVALIDREQUEST:
                case PS.PayloadType.PAYLOADHTTPFOUND:
                    _eventsRequestHandler.Handle(payload);
                    break;
                case PS.PayloadType.PAYLOADACCOUNTSTATE:
                    _accountStateRequestHandler.Handle(payload);
                    break;
                case PS.PayloadType.PAYLOADMARKETQUOTES:
                    // First, respond to a possible synchronous request
                    _marketQuotesRequestHandler.Handle(
                        Uid.FromUuid128(payload.MarketQuotes.Market),
                        payload);
                    // Dispatch updates to all listeners
                    _marketQuotesHandler.Handle(payload);
                    break;
                case PS.PayloadType.PAYLOADCONTRACTQUOTES:
                    _contractQuotesHandler.Handle(payload);
                    break;
                case PS.PayloadType.PAYLOADORDERSFORMARKET:
                    _ordersByMarketRequestHandler.Handle(
                        Uid.FromUuid128(payload.OrdersForMarket.Market),
                        payload);
                    break;
                case PS.PayloadType.PAYLOADORDERSFORACCOUNT:
                    _ordersByAccountRequestHandler.Handle(payload);
                    break;
                case PS.PayloadType.PAYLOADORDERACCEPTED:
                case PS.PayloadType.PAYLOADORDERREJECTED:
                case PS.PayloadType.PAYLOADORDERINVALID:
                    _orderCreateRequestHandler.Handle(payload);
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
