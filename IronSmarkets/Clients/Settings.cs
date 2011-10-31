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

using IronSmarkets.Sessions;
using IronSmarkets.Sockets;

namespace IronSmarkets.Clients
{
    public interface IClientSettings
    {
        ISocketSettings SocketSettings { get; }
        ISessionSettings SessionSettings { get; }
        int HttpRequestTimeout { get; }
    }

    public struct ClientSettings : IClientSettings
    {
        private readonly int _httpRequestTimeout;
        private readonly ISocketSettings _socketSettings;
        private readonly ISessionSettings _sessionSettings;

        public int HttpRequestTimeout { get { return _httpRequestTimeout; } }
        public ISocketSettings SocketSettings { get { return _socketSettings; } }
        public ISessionSettings SessionSettings { get { return _sessionSettings; } }

        public ClientSettings(
            ISocketSettings socketSettings,
            ISessionSettings sessionSettings,
            int httpRequestTimeout = 60000)
        {
            _socketSettings = socketSettings;
            _sessionSettings = sessionSettings;
            _httpRequestTimeout = httpRequestTimeout;
        }
    }
}
