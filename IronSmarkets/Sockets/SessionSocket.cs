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
using System.Net.Security;
using System.Net.Sockets;
using System.IO;

using log4net;
using ProtoBuf;

using IronSmarkets.Exceptions;
using IronSmarkets.Proto.Seto;

namespace IronSmarkets.Sockets
{
    internal sealed class SessionSocket : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISocketSettings _settings;
        private readonly TcpClient _client = new TcpClient();

        private bool _disposed;
        private Stream _tcpStream;

        public Stream TcpStream { get { return _tcpStream; } }

        public bool IsConnected
        {
            get
            {
                return _client.Connected;
            }
        }

        public SessionSocket(ISocketSettings settings)
        {
            _settings = settings;
        }

        ~SessionSocket()
        {
            var disp = this as IDisposable;
            disp.Dispose();
        }

        public void Connect()
        {
            if (_disposed)
                throw new ObjectDisposedException(
                    "SessionSocket",
                    "Called Connect on disposed object");

            if (IsConnected)
                throw new ConnectionException(
                    "Socket already connected");

            if (Log.IsDebugEnabled) Log.Debug(
                string.Format(
                    "Connecting to host {0} on port {1}",
                    _settings.Host, _settings.Port));
            _client.Connect(_settings.Host, _settings.Port);
            _client.Client.NoDelay = true;

            if (_settings.Ssl)
            {
                if (Log.IsDebugEnabled) Log.Debug(
                    "Wrapping stream with SSL");
                var sslStream = new SslStream(
                    _client.GetStream(), false,
                    _settings.RemoteSslCallback, null);
                if (Log.IsDebugEnabled) Log.Debug(
                    string.Format(
                        "Validating hostname {0}", _settings.Hostname));
                sslStream.AuthenticateAsClient(_settings.Hostname);
                _tcpStream = sslStream;
            }
            else
            {
                _tcpStream = _client.GetStream();
            }

            if (Log.IsDebugEnabled) Log.Debug(
                "Connection established; stream is ready");
        }

        public void Disconnect()
        {
            if (_disposed)
                throw new ObjectDisposedException(
                    "SessionSocket",
                    "Called Disconnect on disposed object");

            if (IsConnected)
            {
                if (Log.IsDebugEnabled) Log.Debug("Closing TCP stream");
                _tcpStream.Close();
                if (Log.IsDebugEnabled) Log.Debug("Closing TCP client");
                _client.Close();
            }
        }

        public void Write(Payload payload)
        {
            if (_disposed)
                throw new ObjectDisposedException(
                    "SessionSocket",
                    "Called Write on disposed object");

            if (!IsConnected)
                throw new ConnectionException(
                    "Socket not connected");

            if (Log.IsDebugEnabled) Log.Debug(
                string.Format(
                    "Serializing payload [out:{0}] {1} / {2} with length prefix",
                    payload.EtoPayload.Seq, payload.Type,
                    payload.EtoPayload.Type));

            Serializer.SerializeWithLengthPrefix(
                TcpStream, payload, PrefixStyle.Base128);
        }

        public void Flush()
        {
            if (_disposed)
                throw new ObjectDisposedException(
                    "SessionSocket",
                    "Called Flush on disposed object");

            if (!IsConnected)
                throw new ConnectionException(
                    "Socket not connected");

            // This may be a useless. From MSDN:
            // The Flush method implements the Stream.Flush method;
            // however, because NetworkStream is not buffered, it has
            // no affect on network streams. Calling the Flush method
            // does not throw an exception.
            if (Log.IsDebugEnabled) Log.Debug("Flushing network stream");
            TcpStream.Flush();
        }

        public Payload Read()
        {
            if (_disposed)
                throw new ObjectDisposedException(
                    "SessionSocket",
                    "Called Read on disposed object");

            if (!IsConnected)
                throw new ConnectionException(
                    "Socket not connected");

            var payload = Serializer.DeserializeWithLengthPrefix<Payload>(
                TcpStream, PrefixStyle.Base128);
            if (Log.IsDebugEnabled)
            {
                if (payload != null)
                {
                    Log.Debug(
                        string.Format(
                            "Deserialized payload [in:{0}] {1} / {2}",
                            payload.EtoPayload.Seq, payload.Type,
                            payload.EtoPayload.Type));
                }
                else
                {
                    Log.Debug("Deserialized null payload");
                }
            }
            return payload;
        }

        public void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            try
            {
                if (disposing)
                {
                    Disconnect();
                }
            }
            finally
            {
                _disposed = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
