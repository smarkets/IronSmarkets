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
using System.Collections;
using System.Net.Security;
using System.Text;
using System.IO;

using ProtoBuf;

using IronSmarkets.Sockets;
using IronSmarkets.Exceptions;

using Seto = IronSmarkets.Proto.Seto;
using Eto = IronSmarkets.Proto.Eto;

namespace IronSmarkets.Sessions
{
    public class SeqSession : IDisposable
    {
        private readonly SessionSocket _socket;
        private readonly ISessionSettings _settings;

        private bool disposed = false;

        private ulong _inSequence;
        private ulong _outSequence;
        private string _sessionId = null;

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

        ~SeqSession()
        {
            var disp = this as IDisposable;
            if (disp != null)
                disp.Dispose();
        }

        public void Login()
        {
            if (disposed)
                throw new ObjectDisposedException(
                    "SeqSession",
                    "Called Login on disposed object");

            _socket.Connect();

            // Construct the login payload protobuf
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
            else if (response.EtoPayload.Type == Eto.PayloadType.PAYLOADLOGINRESPONSE)
            {
                _sessionId = response.EtoPayload.LoginResponse.Session;
                _outSequence = response.EtoPayload.LoginResponse.Reset;
            }
        }

        public void Logout()
        {
            Logout(true);
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

        public ulong Send(Seto.Payload payload)
        {
            if (disposed)
                throw new ObjectDisposedException(
                    "SeqSession",
                    "Called Send on disposed object");

            payload.EtoPayload.Seq = _outSequence;
            _socket.Write(payload);
            return _outSequence++;
        }

        public Seto.Payload Receive()
        {
            if (disposed)
                throw new ObjectDisposedException(
                    "SeqSession",
                    "Called Receive on disposed object");

            var payload = _socket.Read();
            if (payload.EtoPayload.Seq == _inSequence)
            {
                _inSequence++;
                return payload;
            }
            else if (payload.EtoPayload.Type == Eto.PayloadType.PAYLOADREPLAY)
            {
                // Replay message
                return null;
            }
            else if (payload.EtoPayload.Seq > _inSequence)
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

                Send(replayPayload);
            }

            return null;
        }

        public void Disconnect()
        {
            var disp = _socket as IDisposable;
            if (disp != null)
                disp.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            try
            {
                if (disposing)
                {
                    Logout();
                }
            }
            finally
            {
                this.disposed = true;
            }
        }

        void IDisposable.Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
