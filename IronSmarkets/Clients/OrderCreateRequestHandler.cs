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
using System.Collections.Generic;
using System.Threading;

using log4net;

using IronSmarkets.Data;
using IronSmarkets.Exceptions;

namespace IronSmarkets.Clients
{
    internal sealed class OrderCreateRequestHandler
        : SeqRpcHandler<Proto.Seto.OrderAccepted, Order>
    {
	private static readonly ILog Log = LogManager.GetLogger(
	    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

	public OrderCreateRequestHandler(ISmarketsClient client)
            : base(client, Map, Extract)
	{
	}

	public Response<Order> Request(NewOrder request)
	{
            var payload = new Proto.Seto.Payload {
                Type = Proto.Seto.PayloadType.PAYLOADORDERCREATE,
                OrderCreate = request.ToOrderCreate()
            };
	    var req = BeginRequest(payload);
	    return new Response<Order>(
                payload.EtoPayload.Seq,
                request.ToOrder(Uid.FromUuid128(req.Response.Order)));
	}

        public static Order Map(ISmarketsClient client, Proto.Seto.OrderAccepted message)
        {
            // Ok, this class hierarchy needs some refactoring...
            throw new NotImplementedException();
        }

        public static void Extract(
            SyncRequest<Proto.Seto.OrderAccepted> request,
            Proto.Seto.Payload payload)
        {
            switch (payload.Type)
            {
                case Proto.Seto.PayloadType.PAYLOADORDERACCEPTED:
                    // Normal case
                    request.Response = payload.OrderAccepted;
                    break;
                case Proto.Seto.PayloadType.PAYLOADORDERREJECTED:
                    request.SetException(OrderRejectedException.FromSeto(payload.OrderRejected));
                    break;
                case Proto.Seto.PayloadType.PAYLOADORDERINVALID:
                    request.SetException(OrderInvalidException.FromSeto(payload.OrderInvalid));
                    break;
                default:
                    Log.Error(
                        string.Format(
                            "Somehow a payload of type {0}" +
                            " was dispatched to an order create" +
                            " handler.", payload.Type));
                    break;
            }
        }
    }
}
