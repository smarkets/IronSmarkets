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

using System.Collections.Generic;

using IronSmarkets.Events;
using IronSmarkets.Proto.Seto;

namespace IronSmarkets.Sessions
{
    public interface ISession<T> : IPayloadEvents<T>, IPayloadEndpoint<T> where T : IPayload
    {
        ulong InSequence { get; }
        ulong OutSequence { get; }
        string SessionId { get; }
        bool IsDisposed { get; }

        ulong Login();
        ulong Logout();

        IEnumerable<ulong> Send(T payload);
        IEnumerable<ulong> Send(T payload, bool flush);
        IEnumerable<ulong> Flush();
        T Receive();
        void Disconnect();
    }
}
