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

using IronSmarkets.Data;
using IronSmarkets.Events;
using IronSmarkets.Exceptions;
using IronSmarkets.Messages;
using IronSmarkets.Sessions;
using IronSmarkets.Sockets;
#if NET35
using IronSmarkets.System;
#endif

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
        IMarketMap MarketMap { get; }
        IContractMap ContractMap { get; }

        IResponse<IEventMap> GetEvents(EventQuery query);
        IResponse<AccountState> GetAccountState();
        IResponse<AccountState> GetAccountState(Uid account);

        IResponse<MarketQuotes> GetQuotesByMarket(Uid market);

        IResponse<IOrderMap> GetOrders();
        IResponse<IOrderMap> GetOrdersByMarket(Uid market);

        IResponse<OrderCancelledReason> CancelOrder(Order order);
        IResponse<Order> CreateOrder(NewOrder order);
    }

    public sealed class SmarketsClient : ISmarketsClient
    {
        private readonly ISession<PS.Payload> _session;
        private readonly Receiver<PS.Payload> _receiver;

        private readonly EventMap _eventMap = new EventMap();
        private readonly OrderMap _orderMap = new OrderMap();
        private readonly MarketMap _marketMap = new MarketMap();
        private readonly ContractMap _contractMap = new ContractMap();

        private readonly IRpcHandler<IEventMap, EventMap> _eventsRequestHandler;
        private readonly IRpcHandler<AccountState, object> _accountStateRequestHandler;
        private readonly IRpcHandler<MarketQuotes, object> _marketQuotesRequestHandler;
        private readonly IRpcHandler<IOrderMap, OrderMap> _ordersForAccountRequestHandler;
        private readonly IRpcHandler<IOrderMap, OrderMap> _ordersForMarketRequestHandler;
        private readonly IRpcHandler<Order, Tuple<NewOrder, OrderMap>> _orderCreateRequestHandler;
        private readonly IRpcHandler<OrderCancelledReason, OrderMap> _orderCancelRequestHandler;
        private readonly IAsyncHttpFoundHandler<PS.Events> _httpHandler;

        private readonly QuoteHandler<PS.MarketQuotes> _marketQuotesHandler = new MarketQuoteHandler();

        private readonly QuoteHandler<PS.ContractQuotes> _contractQuotesHandler = new ContractQuoteHandler();

        private int _disposed;

        private SmarketsClient(
            ISession<PS.Payload> session,
            IAsyncHttpFoundHandler<PS.Events> httpHandler)
        {
            _session = session;
            _session.PayloadReceived += (sender, args) =>
                OnPayloadReceived(args.Payload);
            _session.PayloadSent += (sender, args) =>
                OnPayloadSent(args.Payload);
            AddPayloadHandler(HandlePayload);
            _receiver = new Receiver<PS.Payload>(_session);
            _httpHandler = httpHandler;

            _eventsRequestHandler = new EventsRequestHandler(this, _httpHandler);
            _accountStateRequestHandler = new AccountStateRequestHandler(this);
            _marketQuotesRequestHandler = new MarketQuotesRequestHandler(this);
            _ordersForAccountRequestHandler = new OrdersForAccountRequestHandler(this);
            _ordersForMarketRequestHandler = new OrdersForMarketRequestHandler(this);
            _orderCreateRequestHandler = new OrderCreateRequestHandler(this);
            _orderCancelRequestHandler = new OrderCancelRequestHandler(this);
        }

        public static ISmarketsClient Create(IClientSettings settings)
        {
            return Create(settings, null, null);
        }

        public static ISmarketsClient Create(
            IClientSettings settings,
            ISession<PS.Payload> session)
        {
            return Create(settings, session, null);
        }

        public static ISmarketsClient Create(
            IClientSettings settings,
            IAsyncHttpFoundHandler<PS.Events> httpHandler)
        {
            return Create(settings, null, httpHandler);
        }

        public static ISmarketsClient Create(
            IClientSettings settings,
            ISession<PS.Payload> session,
            IAsyncHttpFoundHandler<PS.Events> httpHandler)
        {
            if (session == null)
                session = new SeqSession(
                    new SessionSocket(settings.SocketSettings),
                    settings.SessionSettings);

            if (httpHandler == null)
                httpHandler = new HttpFoundHandler<PS.Events>(
                    settings.HttpRequestTimeout);
            var client = new SmarketsClient(session, httpHandler);
            httpHandler.SetClient(client);
            return client;
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

        public IMarketMap MarketMap
        {
            get
            {
                return _marketMap;
            }
        }

        public IContractMap ContractMap
        {
            get
            {
                return _contractMap;
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

            if (_receiver.IsCurrentThread)
                throw new ReceiverDeadlockException(
                    "SendPayload called from receiver thread "
                    + "which would result in a deadlock");

            _session.SendPayload(payload);
        }

        public ulong Login()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called Login on disposed object");

            if (_receiver.IsCurrentThread)
                throw new ReceiverDeadlockException(
                    "Login called from receiver thread "
                    + "which would result in a deadlock");

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

            if (_receiver.IsCurrentThread)
                throw new ReceiverDeadlockException(
                    "Logout called from receiver thread "
                    + "which would result in a deadlock");

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

            if (_receiver.IsCurrentThread)
                throw new ReceiverDeadlockException(
                    "Ping called from receiver thread "
                    + "which would result in a deadlock");

            var payload = Payloads.Ping();
            SendPayload(payload);
            return payload.EtoPayload.Seq;
        }

        public ulong SubscribeMarket(Uid market)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called SubscribeMarket on disposed object");

            if (_receiver.IsCurrentThread)
                throw new ReceiverDeadlockException(
                    "SubscribeMarket called from receiver thread "
                    + "which would result in a deadlock");

            var payload = Payloads.MarketSubscribe(market);
            SendPayload(payload);
            return payload.EtoPayload.Seq;
        }

        public ulong UnsubscribeMarket(Uid market)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called UnsubscribeMarket on disposed object");

            if (_receiver.IsCurrentThread)
                throw new ReceiverDeadlockException(
                    "UnsubscribeMarket called from receiver thread "
                    + "which would result in a deadlock");

            var payload = Payloads.MarketUnsubscribe(market);
            SendPayload(payload);
            return payload.EtoPayload.Seq;
        }

        public IResponse<IEventMap> GetEvents(EventQuery query)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetEvents on disposed object");

            if (_receiver.IsCurrentThread)
                throw new ReceiverDeadlockException(
                    "GetEvents called from receiver thread "
                    + "which would result in a deadlock");

            return _eventsRequestHandler.BeginRequest(
                Payloads.EventsRequest(query), _eventMap);
        }

        public IResponse<AccountState> GetAccountState()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetAccountState on disposed object");

            return GetAccountStateInternal(null);
        }

        public IResponse<AccountState> GetAccountState(Uid account)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetAccountState on disposed object");

            return GetAccountStateInternal(account);
        }

        private IResponse<AccountState> GetAccountStateInternal(Uid? account)
        {
            if (_receiver.IsCurrentThread)
                throw new ReceiverDeadlockException(
                    "GetAccountState called from receiver thread "
                    + "which would result in a deadlock");

            return _accountStateRequestHandler.BeginRequest(
                Payloads.AccountStateRequest(account), null);
        }

        public IResponse<MarketQuotes> GetQuotesByMarket(Uid market)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetQuotesByMarket on disposed object");

            if (_receiver.IsCurrentThread)
                throw new ReceiverDeadlockException(
                    "GetQuotesByMarket called from receiver thread "
                    + "which would result in a deadlock");

            return _marketQuotesRequestHandler.BeginRequest(
                Payloads.MarketQuotesRequest(market), null);
        }

        public IResponse<IOrderMap> GetOrders()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetOrdersByAccount on disposed object");

            if (_receiver.IsCurrentThread)
                throw new ReceiverDeadlockException(
                    "GetOrders called from receiver thread "
                    + "which would result in a deadlock");

            return _ordersForAccountRequestHandler.BeginRequest(
                Payloads.OrdersForAccount(), _orderMap);
        }

        public IResponse<IOrderMap> GetOrdersByMarket(Uid market)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called GetOrdersByMarket on disposed object");

            if (_receiver.IsCurrentThread)
                throw new ReceiverDeadlockException(
                    "GetOrdersByMarket called from receiver thread "
                    + "which would result in a deadlock");

            return _ordersForMarketRequestHandler.BeginRequest(
                Payloads.OrdersForMarket(market), _orderMap);
        }

        public IResponse<OrderCancelledReason> CancelOrder(Order order)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called CancelOrder on disposed object");

            if (!order.Cancellable)
                throw new InvalidOperationException(
                    string.Format(
                        "Order cannot be cancelled: {0}", order.State.Status));

            if (_receiver.IsCurrentThread)
                throw new ReceiverDeadlockException(
                    "CancelOrder called from receiver thread "
                    + "which would result in a deadlock");

            return _orderCancelRequestHandler.BeginRequest(
                Payloads.OrderCancel(order.Uid), _orderMap);
        }

        public IResponse<Order> CreateOrder(NewOrder order)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called CreateOrder on disposed object");

            if (_receiver.IsCurrentThread)
                throw new ReceiverDeadlockException(
                    "CreateOrder called from receiver thread "
                    + "which would result in a deadlock");

            return _orderCreateRequestHandler.BeginRequest(
                Payloads.OrderCreate(order),
                new Tuple<NewOrder, OrderMap>(order, _orderMap));
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
                    var marketUid = Uid.FromUuid128(payload.MarketQuotes.Market);
                    _marketQuotesRequestHandler.Handle(payload);
                    // Dispatch updates to markets in identity map
                    var marketPair = _marketQuotesHandler.Extract(payload);
                    Market market;
                    if (_marketMap.TryGetValue(marketUid, out market))
                    {
                        var args = QuoteHandler<PS.MarketQuotes>.PayloadArgs(marketPair, payload);
                        market.OnMarketQuotesReceived(this, args);
                    }
                    // Dispatch updates to all listeners
                    _marketQuotesHandler.Handle(marketPair, payload);
                    break;
                case PS.PayloadType.PAYLOADCONTRACTQUOTES:
                    var contractUid = Uid.FromUuid128(payload.ContractQuotes.Contract);
                    // Dispatch updates to markets in identity map
                    var contractPair = _contractQuotesHandler.Extract(payload);
                    Contract contract;
                    if (_contractMap.TryGetValue(contractUid, out contract))
                    {
                        var args = QuoteHandler<PS.ContractQuotes>.PayloadArgs(contractPair, payload);
                        contract.OnContractQuotesReceived(this, args);
                    }
                    // Dispatch to all other listeners
                    _contractQuotesHandler.Handle(contractPair, payload);
                    break;
                case PS.PayloadType.PAYLOADORDERSFORMARKET:
                    _ordersForMarketRequestHandler.Handle(payload);
                    break;
                case PS.PayloadType.PAYLOADORDERSFORACCOUNT:
                    _ordersForAccountRequestHandler.Handle(payload);
                    break;
                case PS.PayloadType.PAYLOADORDERACCEPTED:
                case PS.PayloadType.PAYLOADORDERREJECTED:
                case PS.PayloadType.PAYLOADORDERINVALID:
                    _orderCreateRequestHandler.Handle(payload);
                    break;
                case PS.PayloadType.PAYLOADORDEREXECUTED:
                    var orderUid = Uid.FromUuid128(payload.OrderExecuted.Order);
                    Order order;
                    if (_orderMap.TryGetValue(orderUid, out order))
                    {
                        order.Update(payload.OrderExecuted);
                    }
                    break;
                case PS.PayloadType.PAYLOADORDERCANCELLED:
                    orderUid = Uid.FromUuid128(payload.OrderCancelled.Order);
                    if (_orderMap.TryGetValue(orderUid, out order))
                    {
                        order.Update(payload.OrderCancelled);
                    }
                    _orderCancelRequestHandler.Handle(payload);
                    break;
                case PS.PayloadType.PAYLOADORDERCANCELREJECTED:
                    _orderCancelRequestHandler.Handle(payload);
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
