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

namespace IronSmarkets.Data
{
    public enum OrderCreateType
    {
        Limit
    }

    public enum OrderStatus
    {
        Live,
        PartiallyFilled,
        Filled,
        PartiallyCancelled,
        Cancelled
    }

    public class OrderState
    {
        private static readonly IDictionary<Proto.Seto.OrderCreateType, OrderCreateType> OrderCreateTypes =
            new Dictionary<Proto.Seto.OrderCreateType, OrderCreateType>
            {
                { Proto.Seto.OrderCreateType.ORDERCREATELIMIT, OrderCreateType.Limit }
            };
        private static readonly IDictionary<Proto.Seto.OrderStatus, OrderStatus> OrderStatuses =
            new Dictionary<Proto.Seto.OrderStatus, OrderStatus>
            {
                { Proto.Seto.OrderStatus.ORDERSTATUSLIVE, OrderStatus.Live },
                { Proto.Seto.OrderStatus.ORDERSTATUSPARTIALLYFILLED, OrderStatus.PartiallyFilled },
                { Proto.Seto.OrderStatus.ORDERSTATUSFILLED, OrderStatus.Filled },
                { Proto.Seto.OrderStatus.ORDERSTATUSPARTIALLYCANCELLED, OrderStatus.PartiallyCancelled },
                { Proto.Seto.OrderStatus.ORDERSTATUSCANCELLED, OrderStatus.Cancelled }
            };

        private readonly Uid _uid;
        private readonly OrderCreateType _type;
        private readonly OrderStatus _status;
        private readonly Quantity _quantity;
        private readonly ulong _created;
        private readonly Quantity _quantityFilled;

        public Uid Uid { get { return _uid; } }
        public OrderCreateType Type { get { return _type; } }
        public OrderStatus Status { get { return _status; } }
        public Quantity Quantity { get { return _quantity; } }
        public DateTime Created { get { return SetoMap.FromMicroseconds(_created); } }
        public Quantity QuantityFilled { get { return _quantityFilled; } }

        private OrderState(
            Uid uid,
            OrderCreateType type,
            OrderStatus status,
            Quantity quantity,
            ulong created,
            Quantity quantityFilled)
        {
            _uid = uid;
            _type = type;
            _status = status;
            _quantity = quantity;
            _created = created;
            _quantityFilled = quantityFilled;
        }

        internal OrderState FromSeto(Proto.Seto.OrderState state)
        {
            var quantityType = Quantity.QuantityTypeFromSeto(state.QuantityType);
            return new OrderState(
                Uid.FromUuid128(state.Order),
                OrderCreateTypes[state.Type],
                OrderStatuses[state.Status],
                new Quantity(quantityType, state.Quantity),
                state.CreatedMicroseconds,
                new Quantity(quantityType, state.QuantityFilled));
        }
    }
}
