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
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace IronSmarkets.Sockets
{
    public interface ISocketSettings
    {
        string Host { get; }

        // Used by SSL certificate validation. Usually the same as
        // Host, but can be different if Host is an IP address.
        string Hostname { get; }

        int Port { get; }
        bool Ssl { get; }

        // Can be null if the default validation should be used
        RemoteCertificateValidationCallback RemoteSslCallback { get; }
    }

    public struct SocketSettings : ISocketSettings
    {
        private readonly string host;
        private readonly string hostname;
        private readonly int port;
        private readonly bool ssl;
        private readonly RemoteCertificateValidationCallback remoteSslCallback;

        public string Host { get { return host; } }
        public string Hostname { get { return hostname; } }
        public int Port { get { return port; } }
        public bool Ssl { get { return ssl; } }
        public RemoteCertificateValidationCallback RemoteSslCallback { get { return remoteSslCallback; } }

        public SocketSettings(string host, int port) : this(
            host, host, port, false, null)
        {
        }

        public SocketSettings(
            string host, string hostname, int port,
            bool ssl, RemoteCertificateValidationCallback remoteSslCallback)
        {
            this.host = host;
            this.hostname = hostname;
            this.port = port;
            this.ssl = ssl;
            this.remoteSslCallback = remoteSslCallback;
        }
        
        internal static bool ValidateServerCertificate(
            object sender,
            X509Certificate certficiate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None;
        }
    }
}
