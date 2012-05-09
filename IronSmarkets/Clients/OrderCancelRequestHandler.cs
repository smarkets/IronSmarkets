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

using IronSmarkets.Data;
using IronSmarkets.Exceptions;
#if NET35
using IronSmarkets.System;
#endif

namespace IronSmarkets.Clients
{
    internal sealed class OrderCancelRequestHandler
        : RpcHandler<Proto.Seto.OrderCancelled, OrderCancelledReason, OrderMap>
    {
        private class OrderCancelSyncRequest : SyncRequest<Proto.Seto.OrderCancelled, OrderCancelledReason, OrderMap>
        {
            public OrderCancelSyncRequest(ulong sequence, OrderMap state) : base(sequence, state)
            {
            }

            protected override OrderCancelledReason Map(ISmarketsClient client, Proto.Seto.OrderCancelled message)
            {
                return OrderState.OrderCancelledReasons[message.Reason];
            }
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(OrderCancelRequestHandler));

        private readonly IDictionary<ulong, Tuple<ulong, Uid, SyncRequest<Proto.Seto.OrderCancelled, OrderCancelledReason, OrderMap>>> _requestsBySeq =
            new Dictionary<ulong, Tuple<ulong, Uid, SyncRequest<Proto.Seto.OrderCancelled, OrderCancelledReason, OrderMap>>>();
        private readonly IDictionary<Uid, Tuple<ulong, Uid, SyncRequest<Proto.Seto.OrderCancelled, OrderCancelledReason, OrderMap>>> _requestsByOrder =
            new Dictionary<Uid, Tuple<ulong, Uid, SyncRequest<Proto.Seto.OrderCancelled, OrderCancelledReason, OrderMap>>>();

        public OrderCancelRequestHandler(ISmarketsClient client) : base(client)
        {
        }

        protected override void AddRequest(Proto.Seto.Payload payload, SyncRequest<Proto.Seto.OrderCancelled, OrderCancelledReason, OrderMap> request)
        {
            var seq = payload.EtoPayload.Seq;
            var uid = Uid.FromUuid128(payload.OrderCancel.Order);
            var holder = new Tuple<ulong, Uid, SyncRequest<Proto.Seto.OrderCancelled, OrderCancelledReason, OrderMap>>(seq, uid, request);
            _requestsBySeq[seq] = holder;
            _requestsByOrder[uid] = holder;
        }

        protected override SyncRequest<Proto.Seto.OrderCancelled, OrderCancelledReason, OrderMap> GetRequest(Proto.Seto.Payload payload)
        {
            Tuple<ulong, Uid, SyncRequest<Proto.Seto.OrderCancelled, OrderCancelledReason, OrderMap>> holder;
            switch (payload.Type)
            {
                case Proto.Seto.PayloadType.PAYLOADORDERCANCELLED:
                    // Use order ID to find request
                    var uid = Uid.FromUuid128(payload.OrderCancelled.Order);
                    if (_requestsByOrder.TryGetValue(uid, out holder))
                    {
                        _requestsByOrder.Remove(uid);
                        _requestsBySeq.Remove(holder.Item1);
                    }
                    return holder.Item3;
                case Proto.Seto.PayloadType.PAYLOADORDERCANCELREJECTED:
                    // Use seq to find request
                    if (_requestsBySeq.TryGetValue(payload.OrderCancelRejected.Seq, out holder))
                    {
                        _requestsBySeq.Remove(payload.OrderCancelRejected.Seq);
                        _requestsByOrder.Remove(holder.Item2);
                    }
                    return holder.Item3;
            }

            return null;
        }

        protected override void Extract(
            SyncRequest<Proto.Seto.OrderCancelled, OrderCancelledReason, OrderMap> request,
            Proto.Seto.Payload payload)
        {
            switch (payload.Type)
            {
                case Proto.Seto.PayloadType.PAYLOADORDERCANCELLED:
                    // Normal case
                    request.SetResponse(Client, payload.OrderCancelled);
                    break;
                case Proto.Seto.PayloadType.PAYLOADORDERCANCELREJECTED:
                    request.SetException(OrderCancelRejectedException.FromSeto(payload.OrderCancelRejected));
                    break;
                default:
                    Log.Error(
                        string.Format(
                            "Somehow a payload of type {0}" +
                            " was dispatched to an order cancel" +
                            " handler.", payload.Type));
                    break;
            }
        }

        protected override SyncRequest<Proto.Seto.OrderCancelled, OrderCancelledReason, OrderMap> NewRequest(ulong sequence, OrderMap state)
        {
            return new OrderCancelSyncRequest(sequence, state);
        }
    }
}
