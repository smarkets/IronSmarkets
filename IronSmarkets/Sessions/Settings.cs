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

namespace IronSmarkets.Sessions
{
    public interface ISessionSettings
    {
        string Username { get; }
        string Password { get; }
        ulong InSequence { get; }
        ulong OutSequence { get; }

        // Can be null to create a new session
        string SessionId { get; }
    }

    public struct SessionSettings : ISessionSettings
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string _sessionId;

        private readonly ulong _inSequence;
        private readonly ulong _outSequence;

        public string Username { get { return _username; } }
        public string Password { get { return _password; } }
        public ulong InSequence { get { return _inSequence; } }
        public ulong OutSequence { get { return _outSequence; } }
        public string SessionId { get { return _sessionId; } }

        public SessionSettings(string username, string password) : this(username, password, 1, 1, null)
        {
        }

        public SessionSettings(
            string username, string password,
            ulong inSequence, ulong outSequence,
            string sessionId)
        {
            _username = username;
            _password = password;
            _inSequence = inSequence;
            _outSequence = outSequence;
            _sessionId = sessionId;
        }
    }
}
