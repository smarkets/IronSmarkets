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
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

using log4net;

using IronSmarkets.Events;
using IronSmarkets.Exceptions;
using IronSmarkets.Proto.Seto;
using IronSmarkets.Sockets;

using Eto = IronSmarkets.Proto.Eto;

namespace IronSmarkets.Sessions
{
    internal sealed class SeqSession : IDisposable, ISession<Payload>
    {
        private static readonly ILog Log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISocket<Payload> _socket;
        private readonly ISessionSettings _settings;
        private readonly ConcurrentQueue<Payload> _sendBuffer =
            new ConcurrentQueue<Payload>();
        private readonly ManualResetEvent _logoutReceived =
            new ManualResetEvent(false);

        private readonly object _reader = new object();
        private readonly object _writer = new object();

        private int _disposed;

        private ulong _inSequence;
        private ulong _outSequence;
        private string _sessionId;
        private bool _loginSent;
        private Predicate<Payload> _payloadHandler;

        public ulong InSequence { get { return _inSequence; } }
        public ulong OutSequence { get { return _outSequence; } }
        public string SessionId { get { return _sessionId; } }

        public event EventHandler<PayloadReceivedEventArgs<Payload>> PayloadReceived;

        public SeqSession(
            ISocketSettings socketSettings,
            ISessionSettings sessionSettings)
        {
            _socket = new SessionSocket(socketSettings);
            _settings = sessionSettings;
            _inSequence = _settings.InSequence;
            _outSequence = _settings.OutSequence;
        }

        public bool IsDisposed
        {
            get
            {
                return Thread.VolatileRead(ref _disposed) == 1;
            }
        }

        ~SeqSession()
        {
            Dispose(false);
        }

        public ulong Login()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SeqSession",
                    "Called Login on disposed object");

            if (_loginSent)
            {
                throw new InvalidOperationException(
                    "Login payload has already been sent, " +
                    "ignoring Login() call.");
            }

            if (Log.IsDebugEnabled) Log.Debug("Opening socket connection");
            _socket.Connect();

            // Construct the login payload protobfuinter
            // TODO: Create some message factories so this is less ugly
            var loginPayload = new Payload {
                Type = PayloadType.PAYLOADLOGIN,
                EtoPayload = new Eto.Payload {
                    Type = Eto.PayloadType.PAYLOADLOGIN
                },
                Login = new Login {
                    Username = _settings.Username,
                    Password = _settings.Password
                }
            };

            if (_settings.SessionId != null)
            {
                if (Log.IsDebugEnabled) Log.Debug("Adding session id to payload");
                // We are resuming a session
                loginPayload.EtoPayload.Login = new Eto.Login {
                    Session = _settings.SessionId
                };
            }

            if (Log.IsDebugEnabled) Log.Debug("Sending login payload");
            var loginSeq = Send(loginPayload).Last();
            if (Log.IsDebugEnabled) Log.Debug("Receiving login response payload");
            var response = Receive();
            _loginSent = true;
            _logoutReceived.Reset();
            if (response.EtoPayload.Type == Eto.PayloadType.PAYLOADLOGOUT)
            {
                string message;
                switch (response.EtoPayload.Logout.Reason) {
                case Eto.LogoutReason.LOGOUTLOGINTIMEOUT:
                    message = "Login timeout";
                    break;
                case Eto.LogoutReason.LOGOUTUNKNOWNSESSION:
                    message = "Unknown session ID";
                    break;
                case Eto.LogoutReason.LOGOUTUNAUTHORISED:
                    message = "Bad username/password";
                    break;
                case Eto.LogoutReason.LOGOUTSERVICETEMPORARILYUNAVAILABLE:
                    message = "Service temporarily unavailable";
                    break;
                default:
                    message = "Login failed (see reason enum)";
                    break;
                }

                Log.Warn(
                    string.Format(
                        "Received a logout payload after login sent: {0}",
                        response.EtoPayload.Logout.Reason));

                throw new LoginFailedException(message, response.EtoPayload.Logout.Reason);
            }

            if (response.EtoPayload.Type == Eto.PayloadType.PAYLOADLOGINRESPONSE)
            {
                _sessionId = response.EtoPayload.LoginResponse.Session;
                _outSequence = response.EtoPayload.LoginResponse.Reset;
                if (Log.IsInfoEnabled) Log.Info(
                    string.Format(
                        "Login response received with session {0}, reset {1}",
                        _sessionId, _outSequence));
            }
            return loginSeq;
        }

        public ulong Logout()
        {
            if (!_loginSent)
                throw new NotLoggedInException();

            var logoutPayload = new Payload {
                Type = PayloadType.PAYLOADETO,
                EtoPayload = new Eto.Payload {
                    Type = Eto.PayloadType.PAYLOADLOGOUT,
                    Logout = new Eto.Logout {
                        Reason = Eto.LogoutReason.LOGOUTNONE
                    }
                }
            };
            if (Log.IsDebugEnabled) Log.Debug(
                "Sending logout payload with 'none' reason");

            Send(logoutPayload);

            // Block
            _logoutReceived.WaitOne();

            return logoutPayload.EtoPayload.Seq;
        }

        public IEnumerable<ulong> Send(Payload payload)
        {
            return Send(payload, true);
        }

        public IEnumerable<ulong> Send(Payload payload, bool flush)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SeqSession",
                    "Called Send on disposed object");

            if (payload.EtoPayload == null)
                payload.EtoPayload = new Eto.Payload();

            if (Log.IsDebugEnabled) Log.Debug(
                string.Format(
                    "Buffering payload {0} / {1}",
                    payload.Type, payload.EtoPayload.Type));
            _sendBuffer.Enqueue(payload);

            if (flush)
            {
                if (Log.IsDebugEnabled) Log.Debug(
                    "Flushing payloads after buffering");
                return Flush();
            }

            return Enumerable.Empty<ulong>();
        }

        public IEnumerable<ulong> Flush()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SeqSession",
                    "Called Send on disposed object");

            bool writerAcquired = false;
            try
            {
                if (Log.IsDebugEnabled) Log.Debug("Acquiring writer lock");
                Monitor.Enter(_writer, ref writerAcquired);
                if (Log.IsDebugEnabled) Log.Debug("Writer lock acquired");
                var seqs = new List<ulong>(_sendBuffer.Count);
                if (Log.IsDebugEnabled) Log.Debug(
                    string.Format(
                        "Flushing send buffer of size {0}",
                        _sendBuffer.Count));

                Payload outPayload;
                while (_sendBuffer.TryDequeue(out outPayload))
                {
                    outPayload.EtoPayload.Seq = _outSequence;
                    if (Log.IsDebugEnabled) Log.Debug(
                        string.Format(
                            "Writing payload out:{0}",
                            _outSequence));
                    _socket.Write(outPayload);
                    seqs.Add(_outSequence++);
                }
                _socket.Flush();
                return seqs;
            }
            finally
            {
                if (writerAcquired)
                {
                    if (Log.IsDebugEnabled) Log.Debug(
                        "Releasing writer lock");
                    Monitor.Exit(_writer);
                }
            }
        }

        public Payload Receive()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SeqSession",
                    "Called Receive on disposed object");

            bool readerAcquired = false;
            try
            {
                if (Log.IsDebugEnabled) Log.Debug("Acquiring reader lock");
                Monitor.Enter(_reader, ref readerAcquired);
                if (Log.IsDebugEnabled) Log.Debug(
                    "Reading next payload from socket...");
                var payload = _socket.Read();
                if (payload == null)
                {
                    throw new MessageStreamException(
                        "Empty payload was received. " +
                        "Socket was probably closed prematurely");
                }

                if (!OnPayloadReceived(payload))
                {
                    throw new MessageStreamException(
                        "Payload pipeline aborted by handler");
                }

                if (Log.IsDebugEnabled) Log.Debug(
                    string.Format(
                        "Received payload {0} / {1}", payload.Type,
                        payload.EtoPayload.Type));

                if (payload.EtoPayload.Seq == _inSequence)
                {
                    _inSequence++;
                    if (MessageTranslator<Payload>.IsLogoutConfirmation(
                            payload))
                        _logoutReceived.Set();
                    return payload;
                }

                if (payload.EtoPayload.Type == Eto.PayloadType.PAYLOADREPLAY)
                {
                    // Replay message
                    return payload;
                }

                if (payload.EtoPayload.Seq > _inSequence)
                {
                    var replayPayload = new Payload {
                        Type = PayloadType.PAYLOADETO,
                        EtoPayload = new Eto.Payload {
                            Type = Eto.PayloadType.PAYLOADREPLAY,
                            Replay = new Eto.Replay {
                                Seq = _inSequence
                            }
                        }
                    };

                    // Do not flush the send because we do not want a
                    // deadlock.
                    Log.Warn(
                        string.Format(
                            "Received sequence {0} but expected {1}; " +
                            "buffering a replay payload",
                            payload.EtoPayload.Seq, _inSequence));
                    Send(replayPayload, false);
                }
            }
            finally
            {
                if (readerAcquired)
                {
                    if (Log.IsDebugEnabled) Log.Debug(
                        "Releasing reader lock");
                    Monitor.Exit(_reader);
                }
            }

            // TODO: Provide some more information here
            throw new MessageStreamException(
                "Unable to handle payload received.");
        }

        public void Disconnect()
        {
            if  (Log.IsInfoEnabled) Log.Info("Disconnecting socket");
            var disp = _socket as IDisposable;
            if (disp != null)
                disp.Dispose();
        }

        public void AddPayloadHandler(Predicate<Payload> predicate)
        {
            if (null == _payloadHandler)
            {
                _payloadHandler = predicate;
            }
            else
            {
                _payloadHandler += predicate;
            }
        }

        public void RemovePayloadHandler(Predicate<Payload> predicate)
        {
            _payloadHandler -= predicate;
        }

        private bool OnPayloadReceived(Payload payload)
        {
            EventHandler<PayloadReceivedEventArgs<Payload>> ev = PayloadReceived;
            if (ev != null)
                ev(this, new PayloadReceivedEventArgs<Payload>(
                       payload.EtoPayload.Seq,
                       payload));

            if (_payloadHandler == null)
                throw new NoHandlerException();

            return _payloadHandler.GetInvocationList().Cast<Predicate<Payload>>().All(handler => handler(payload));
        }

        private void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                if (disposing)
                {
                    Disconnect();
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
