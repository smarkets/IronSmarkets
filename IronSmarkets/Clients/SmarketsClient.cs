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
using System.Net;
using System.IO;
using System.Threading;

using log4net;
using ProtoBuf;

using IronSmarkets.Data;
using IronSmarkets.Events;
using IronSmarkets.Proto.Seto;
using IronSmarkets.Sessions;
using IronSmarkets.Sockets;

using Eto = IronSmarkets.Proto.Eto;

namespace IronSmarkets.Clients
{
    public interface ISmarketsClient :
        IDisposable,
        IPayloadEvents<Payload>,
        IPayloadEndpoint<Payload>
    {
        bool IsDisposed { get; }

        ulong Login();
        ulong Logout();

        ulong Ping();

        ulong SubscribeMarket(Uuid market);
        ulong UnsubscribeMarket(Uuid market);

        ulong RequestOrdersForAccount();
        ulong RequestOrdersForMarket(Uuid market);
    }

    public sealed class SmarketsClient : ISmarketsClient
    {
        private static readonly ILog Log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISession<Payload> _session;

        private int _disposed;
        private readonly Receiver<Payload> _receiver;

        private SmarketsClient(
            ISocketSettings socketSettings,
            ISessionSettings sessionSettings)
        {
            _session = new SeqSession(socketSettings, sessionSettings);
            _session.PayloadReceived += (sender, args) => OnPayloadReceived(args.Payload);
            _receiver = new Receiver<Payload>(_session);
        }

        public static ISmarketsClient Create(
            ISocketSettings socketSettings,
            ISessionSettings sessionSettings)
        {
            return new SmarketsClient(socketSettings, sessionSettings);
        }

        public bool IsDisposed
        {
            get
            {
                return Thread.VolatileRead(ref _disposed) == 1;
            }
        }

        public event EventHandler<PayloadReceivedEventArgs<Payload>> PayloadReceived;

        ~SmarketsClient()
        {
            Dispose(false);
        }

        public void AddPayloadHandler(Predicate<Payload> predicate)
        {
            _session.AddPayloadHandler(predicate);
        }

        public void RemovePayloadHandler(Predicate<Payload> predicate)
        {
            _session.RemovePayloadHandler(predicate);
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

            var payload = new Payload {
                Type = PayloadType.PAYLOADETO,
                EtoPayload = new Eto.Payload {
                    Type = Eto.PayloadType.PAYLOADPING
                }
            };

            _session.Send(payload);
            return payload.EtoPayload.Seq;
        }

        public ulong SubscribeMarket(Uuid market)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called SubscribeMarket on disposed object");

            var payload = new Payload {
                Type = PayloadType.PAYLOADMARKETSUBSCRIBE,
                MarketSubscribe = new MarketSubscribe {
                    Market = market.ToUuid128()
                }
            };

            _session.Send(payload);
            return payload.EtoPayload.Seq;
        }

        public ulong UnsubscribeMarket(Uuid market)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called UnsubscribeMarket on disposed object");

            var payload = new Payload {
                Type = PayloadType.PAYLOADMARKETUNSUBSCRIBE,
                MarketUnsubscribe = new MarketUnsubscribe {
                    Market = market.ToUuid128()
                }
            };

            _session.Send(payload);
            return payload.EtoPayload.Seq;
        }

        public ulong RequestOrdersForMarket(Uuid market)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called RequestOrdersForMarket on disposed object");

            var payload = new Payload {
                Type = PayloadType.PAYLOADORDERSFORMARKETREQUEST,
                OrdersForMarketRequest = new OrdersForMarketRequest {
                    Market = market.ToUuid128()
                }
            };

            _session.Send(payload);
            return payload.EtoPayload.Seq;
        }

        public ulong RequestOrdersForAccount()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SmarketsClient",
                    "Called RequestOrdersForAccount on disposed object");

            var payload = new Payload {
                Type = PayloadType.PAYLOADORDERSFORACCOUNTREQUEST,
                OrdersForAccountRequest = new OrdersForAccountRequest()
            };

            _session.Send(payload);
            return payload.EtoPayload.Seq;
        }

        private void OnPayloadReceived(Payload payload)
        {
            EventHandler<PayloadReceivedEventArgs<Payload>> ev = PayloadReceived;
            if (ev != null)
                ev(this, new PayloadReceivedEventArgs<Payload>(
                       payload.EtoPayload.Seq,
                       payload));
        }

        private static Payload FetchHttpFound(Payload payload)
        {
            var req = (HttpWebRequest)WebRequest.Create(payload.HttpFound.Url);
            using (var resp = (HttpWebResponse)req.GetResponse())
            {
                Stream receiveStream = resp.GetResponseStream();
                return Serializer.Deserialize<Payload>(receiveStream);
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

    internal sealed class Receiver<T> where T : IPayload
    {
        private static readonly ILog Log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Thread _loop;
        private readonly ISession<T> _session;

        private volatile bool _complete;

        public Receiver(ISession<T> session)
        {
            _session = session;
            _loop = new Thread(Loop) {
                Name = "receiver"
            };
        }

        public void Start()
        {
            _complete = false;
            _loop.Start();
        }

        public void Stop()
        {
            _complete = true;
            _loop.Join();
        }

        private void Loop()
        {
            while (!_complete)
            {
                try
                {
                    var payload = _session.Receive();
                    if (payload.IsLogoutConfirmation())
                    {
                        Log.Info(
                            "Received a logout confirmation; " +
                            "assuming socket will close");
                        _complete = true;
                    }
                }
                catch (IOException ex)
                {
                    Log.Warn("Receiving socket timed out", ex);
                }
            }
        }
    }
}
