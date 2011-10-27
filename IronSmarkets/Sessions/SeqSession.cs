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

using IronSmarkets.Sockets;
using IronSmarkets.Exceptions;

using Seto = IronSmarkets.Proto.Seto;
using Eto = IronSmarkets.Proto.Eto;

namespace IronSmarkets.Sessions
{
    public sealed class SeqSession : IDisposable
    {
        private readonly SessionSocket _socket;
        private readonly ISessionSettings _settings;
        private readonly ConcurrentQueue<Seto.Payload> _sendBuffer =
            new ConcurrentQueue<Seto.Payload>();

        private readonly object _reader = new object();
        private readonly object _writer = new object();

        private int _disposed;

        private ulong _inSequence;
        private ulong _outSequence;
        private string _sessionId;
        private bool _loginSent;

        public ulong InSequence { get { return _inSequence; } }
        public ulong OutSequence { get { return _outSequence; } }
        public string SessionId { get { return _sessionId; } }

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
            var disp = this as IDisposable;
            disp.Dispose();
        }

        public void Login()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SeqSession",
                    "Called Login on disposed object");

            // TODO: Should we throw an exception here?
            if (_loginSent)
                return;

            _socket.Connect();

            // Construct the login payload protobfuinter
            // TODO: Create some message factories so this is less ugly
            var loginPayload = new Seto.Payload {
                Type = Seto.PayloadType.PAYLOADLOGIN,
                EtoPayload = new Eto.Payload {
                    Type = Eto.PayloadType.PAYLOADLOGIN
                },
                Login = new Seto.Login {
                    Username = _settings.Username,
                    Password = _settings.Password
                }
            };

            if (_settings.SessionId != null)
            {
                // We are resuming a session
                loginPayload.EtoPayload.Login = new Eto.Login {
                    Session = _settings.SessionId
                };
            }

            Send(loginPayload);
            var response = Receive();
            _loginSent = true;
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

                throw new LoginFailedException(message, response.EtoPayload.Logout.Reason);
            }
            
            if (response.EtoPayload.Type == Eto.PayloadType.PAYLOADLOGINRESPONSE)
            {
                _sessionId = response.EtoPayload.LoginResponse.Session;
                _outSequence = response.EtoPayload.LoginResponse.Reset;
            }
        }

        public void Logout()
        {
            if (_loginSent)
            {
                Logout(true);
            }
        }

        public void Logout(bool disconnect)
        {
            try
            {
                var logoutPayload = new Seto.Payload {
                    Type = Seto.PayloadType.PAYLOADETO,
                    EtoPayload = new Eto.Payload {
                        Type = Eto.PayloadType.PAYLOADLOGOUT,
                        Logout = new Eto.Logout {
                            Reason = Eto.LogoutReason.LOGOUTNONE
                        }
                    }
                };

                Send(logoutPayload);
            }
            finally
            {
                if (disconnect)
                {
                    Disconnect();
                }
            }
        }

        public IEnumerable<ulong> Send(Seto.Payload payload)
        {
            return Send(payload, true);
        }

        public IEnumerable<ulong> Send(Seto.Payload payload, bool flush)
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SeqSession",
                    "Called Send on disposed object");

            Console.WriteLine("Enqueueing payload {0}", payload);
            _sendBuffer.Enqueue(payload);
            if (flush)
            {
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

            Console.WriteLine("Flushing!");
            Seto.Payload outPayload;
            var seqs = new List<ulong>(_sendBuffer.Count);
            while (_sendBuffer.TryDequeue(out outPayload))
            {
                Console.WriteLine("Flushing payload {0}", outPayload);
                lock (_writer)
                {
                    outPayload.EtoPayload.Seq = _outSequence;
                    Console.WriteLine("Writing payload to socket {0}", outPayload);
                    _socket.Write(outPayload);
                    seqs.Add(_outSequence++);
                }
            }
            return seqs;
        }

        public Seto.Payload Receive()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(
                    "SeqSession",
                    "Called Receive on disposed object");

            lock (_reader)
            {
                var payload = _socket.Read();
                if (payload.EtoPayload.Seq == _inSequence)
                {
                    _inSequence++;
                    return payload;
                }
                
                if (payload.EtoPayload.Type == Eto.PayloadType.PAYLOADREPLAY)
                {
                    // Replay message
                    return null;
                }
                
                if (payload.EtoPayload.Seq > _inSequence)
                {
                    var replayPayload = new Seto.Payload {
                        Type = Seto.PayloadType.PAYLOADETO,
                        EtoPayload = new Eto.Payload {
                            Type = Eto.PayloadType.PAYLOADREPLAY,
                            Replay = new Eto.Replay {
                                Seq = _inSequence
                            }
                        }
                    };

                    Send(replayPayload, false);
                }
            }

            return null;
        }

        public void Disconnect()
        {
            var disp = _socket as IDisposable;
            if (disp != null)
                disp.Dispose();
        }

        public void Dispose(bool disposing)
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
