// Copyright (c) 2011-2012 Smarkets Limited
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
    internal abstract class UidQueueRpcHandler<TPayload, TResponse>
    {
        private static readonly ILog Log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISmarketsClient _client;

        private readonly IDictionary<Uid, Queue<SyncRequest<TPayload>>> _requests =
            new Dictionary<Uid, Queue<SyncRequest<TPayload>>>();
        private readonly object _lock = new object();

        public UidQueueRpcHandler(ISmarketsClient client)
        {
            _client = client;
        }

        public Response<TResponse> Request(Uid uid, Proto.Seto.Payload payload)
        {
            var req = new SyncRequest<TPayload>();
            lock (_lock)
            {

                // XXX: At the moment, SendPayload needs to be inside
                // the lock because the receiver thread could
                // theoretically receive the payload before the
                // SyncRequest is added to the dictionary. Although
                // very unlikely, I am opting for safer in this case
                // (as well as other cases which exhibit the same
                // pattern). It may make sense instead to track the
                // last sequence number for these types of requests in
                // a volatile long variable and spin-wait in the
                // receiver where necessary.
                _client.SendPayload(payload);
                if (!_requests.ContainsKey(uid))
                {
                    _requests[uid] =
                        new Queue<SyncRequest<TPayload>>();
                }
                _requests[uid].Enqueue(req);
            }
            return new Response<TResponse>(
                payload.EtoPayload.Seq,
                Map(_client, req.Response));
        }

        public void Handle(Uid uid, Proto.Seto.Payload payload)
        {
            SyncRequest<TPayload> req = null;
            lock (_lock)
            {
                Queue<SyncRequest<TPayload>> queue;
                if (_requests.TryGetValue(uid, out queue))
                {
                    Debug.Assert(queue.Count > 0);
                    req = queue.Dequeue();
                    if (queue.Count == 0)
                    {
                        _requests.Remove(uid);
                    }
                }
            }
            if (req != null)
                Extract(req, payload);
        }

        protected abstract TResponse Map(ISmarketsClient client, TPayload payload);
        protected abstract void Extract(SyncRequest<TPayload> request, Proto.Seto.Payload payload);
    }
}
