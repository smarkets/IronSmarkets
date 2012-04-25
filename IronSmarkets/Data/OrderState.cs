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
        Pending,
        Live,
        PartiallyFilled,
        Filled,
        PartiallyCancelled,
        Cancelled
    }

    public enum OrderCancelledReason
    {
        None,
        MemberRequested,
        MarketHalted,
        InsufficientLiquidity
    }

    public class OrderState
    {
        private static readonly BiDictionary<Proto.Seto.OrderCreateType, OrderCreateType> OrderCreateTypes =
            new BiDictionary<Proto.Seto.OrderCreateType, OrderCreateType>
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
        private static readonly IDictionary<Proto.Seto.OrderCancelledReason, OrderCancelledReason> OrderCancelledReasons =
            new Dictionary<Proto.Seto.OrderCancelledReason, OrderCancelledReason>
            {
                { Proto.Seto.OrderCancelledReason.ORDERCANCELLEDMEMBERREQUESTED, OrderCancelledReason.MemberRequested },
                { Proto.Seto.OrderCancelledReason.ORDERCANCELLEDMARKETHALTED, OrderCancelledReason.MarketHalted },
                { Proto.Seto.OrderCancelledReason.ORDERCANCELLEDINSUFFICIENTLIQUIDITY, OrderCancelledReason.InsufficientLiquidity }
            };

        private readonly Uid _uid;
        private readonly OrderCreateType _type;
        private readonly ulong _created;
        private readonly Quantity _quantity;
        private OrderStatus _status;
        private Quantity _quantityFilled;
        private OrderCancelledReason _cancelReason;

        public Uid Uid { get { return _uid; } }
        public OrderCreateType Type { get { return _type; } }
        public OrderStatus Status { get { return _status; } }
        public Quantity Quantity { get { return _quantity; } }
        public DateTime Created { get { return SetoMap.FromMicroseconds(_created); } }
        public Quantity QuantityFilled { get { return _quantityFilled; } }
        public OrderCancelledReason CancelReason { get { return _cancelReason; } }

        internal OrderState(
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

        internal void Update(Proto.Seto.OrderExecuted message)
        {
            var quantityType = Quantity.QuantityTypeFromSeto(message.QuantityType);
            _quantityFilled = new Quantity(
                quantityType,
                message.Quantity + _quantityFilled.Raw);
            if (_quantityFilled == _quantity)
                _status = OrderStatus.Filled;
            else
                _status = OrderStatus.PartiallyFilled;
        }

        internal void Update(Proto.Seto.OrderCancelled message)
        {
            _cancelReason = OrderCancelledReasons[message.Reason];
            if (_quantityFilled.Raw == 0)
                _status = OrderStatus.Cancelled;
            else
                _status = OrderStatus.PartiallyCancelled;
        }

        internal static OrderState FromSeto(Proto.Seto.OrderState state)
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

        internal static Proto.Seto.OrderCreateType FromOrderCreateType(OrderCreateType type)
        {
            return OrderCreateTypes[type];
        }
    }
}
