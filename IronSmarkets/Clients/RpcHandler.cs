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

using log4net;

namespace IronSmarkets.Clients
{
    internal interface IRpcHandler<TPayload, TResponse>
    {
        SyncRequest<TPayload> BeginRequest(Proto.Seto.Payload payload);
        Response<TResponse> Request(Proto.Seto.Payload payload);
        void Handle(Proto.Seto.Payload payload);
    }
    
    internal abstract class RpcHandler<TPayload, TResponse> : IRpcHandler<TPayload, TResponse>
    {
        private static readonly ILog Log = LogManager.GetLogger(
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISmarketsClient _client;

        private readonly object _lock = new object();

        public RpcHandler(ISmarketsClient client)
        {
            _client = client;
        }

        public SyncRequest<TPayload> BeginRequest(Proto.Seto.Payload payload)
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
                AddRequest(payload, req);
            }
            return req;
        }

        public Response<TResponse> Request(Proto.Seto.Payload payload)
        {
            var req = BeginRequest(payload);
            return new Response<TResponse>(
                payload.EtoPayload.Seq,
                Map(_client, req.Response));
        }

        public void Handle(Proto.Seto.Payload payload)
        {
            SyncRequest<TPayload> req;
            lock (_lock)
            {
                req = GetRequest(payload);
            }
            if (req != null)
                Extract(req, payload);
            else
            {
                Log.Warn(
                    "Received payload " +
                    "but could not find original request");
            }
        }

        protected abstract TResponse Map(ISmarketsClient client, TPayload payload);
        protected abstract void Extract(SyncRequest<TPayload> request, Proto.Seto.Payload payload);
        protected abstract void AddRequest(Proto.Seto.Payload payload, SyncRequest<TPayload> request);
        protected abstract SyncRequest<TPayload> GetRequest(Proto.Seto.Payload payload);
    }
}
