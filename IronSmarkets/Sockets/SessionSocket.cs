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

using ProtoBuf;

using IronSmarkets.Exceptions;
using IronSmarkets.Proto.Seto;

namespace IronSmarkets.Sockets
{
    internal sealed class SessionSocket : IDisposable
    {
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

            _client.Connect(_settings.Host, _settings.Port);
            _client.Client.NoDelay = true;

            if (_settings.Ssl)
            {
                var sslStream = new SslStream(
                    _client.GetStream(), false,
                    _settings.RemoteSslCallback, null);
                sslStream.AuthenticateAsClient(_settings.Hostname);
                _tcpStream = sslStream;
            }
            else
            {
                _tcpStream = _client.GetStream();
            }
        }

        public void Disconnect()
        {
            if (_disposed)
                throw new ObjectDisposedException(
                    "SessionSocket",
                    "Called Disconnect on disposed object");

            if (!IsConnected)
            {
                _tcpStream.Close();
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

            Serializer.SerializeWithLengthPrefix(
                TcpStream, payload, PrefixStyle.Base128);
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

            return Serializer.DeserializeWithLengthPrefix<Payload>(
                TcpStream, PrefixStyle.Base128);
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
