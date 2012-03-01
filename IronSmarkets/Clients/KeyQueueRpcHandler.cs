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
    internal abstract class KeyQueueRpcHandler<TKey, TPayload, TResponse> : RpcHandler<TPayload, TResponse>
    {
        private static readonly ILog Log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDictionary<TKey, Queue<SyncRequest<TPayload>>> _requests =
            new Dictionary<TKey, Queue<SyncRequest<TPayload>>>();

        public KeyQueueRpcHandler(ISmarketsClient client) : base(client)
        {
        }

        protected override void AddRequest(Proto.Seto.Payload payload, SyncRequest<TPayload> request)
        {
            var key = ExtractRequestKey(payload);
            if (!_requests.ContainsKey(key))
            {
                _requests[key] = new Queue<SyncRequest<TPayload>>();
            }
            _requests[key].Enqueue(request);
        }

        protected override SyncRequest<TPayload> GetRequest(Proto.Seto.Payload payload)
        {
            TKey key = ExtractResponseKey(payload);
            SyncRequest<TPayload> req = null;
            Queue<SyncRequest<TPayload>> queue;
            if (_requests.TryGetValue(key, out queue))
            {
                Debug.Assert(queue.Count > 0);
                req = queue.Dequeue();
                if (queue.Count == 0)
                {
                    _requests.Remove(key);
                }
            }
            return req;
        }

        protected abstract TKey ExtractResponseKey(Proto.Seto.Payload payload);
        protected abstract TKey ExtractRequestKey(Proto.Seto.Payload payload);
    }
}
