// Copyright (c) 2012 Smarkets Limited
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

namespace IronSmarkets.Clients
{
    internal abstract class QueueRpcHandler<TPayload, TResponse, TState> : RpcHandler<TPayload, TResponse, TState>
    {
        private readonly Queue<SyncRequest<TPayload, TResponse, TState>> _requests =
            new Queue<SyncRequest<TPayload, TResponse, TState>>();

        protected QueueRpcHandler(ISmarketsClient client) : base(client)
        {
        }

        protected override void AddRequest(Proto.Seto.Payload payload, SyncRequest<TPayload, TResponse, TState> request)
        {
            _requests.Enqueue(request);
        }

        protected override SyncRequest<TPayload, TResponse, TState> GetRequest(Proto.Seto.Payload payload)
        {
            try
            {
                return _requests.Dequeue();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
