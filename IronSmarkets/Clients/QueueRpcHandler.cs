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
using System.Diagnostics;
using log4net;

using IronSmarkets.Data;

namespace IronSmarkets.Clients
{
    internal abstract class QueueRpcHandler<TPayload, TResponse> : RpcHandler<TPayload, TResponse>
    {
        private static readonly ILog Log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Queue<SyncRequest<TPayload>> _requests =
            new Queue<SyncRequest<TPayload>>();

        public QueueRpcHandler(ISmarketsClient client) : base(client)
        {
        }

        protected override void AddRequest(Proto.Seto.Payload payload, SyncRequest<TPayload> request)
        {
            _requests.Enqueue(request);
        }

        protected override SyncRequest<TPayload> GetRequest(Proto.Seto.Payload payload)
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
