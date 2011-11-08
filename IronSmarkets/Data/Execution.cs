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

namespace IronSmarkets.Data
{
    public struct Execution : IEquatable<Execution>
    {
        private readonly Price _price;
        private readonly Quantity _quantity;
        private readonly Side _liquidity;
        private readonly ulong _microseconds;

        public Price Price { get { return _price; } }
        public Quantity Quantity { get { return _quantity; } }
        public Side Liquidity { get { return _liquidity; } }
        public ulong Microseconds { get { return _microseconds; } }
        public DateTime DateTime { get { return SetoMap.FromMicroseconds(_microseconds); } }

        private Execution(
            Price price,
            Quantity quantity,
            Side liquidity,
            ulong microseconds)
        {
            _price = price;
            _quantity = quantity;
            _liquidity = liquidity;
            _microseconds = microseconds;
        }

        public override int GetHashCode()
        {
            return _price.GetHashCode()
                ^ _quantity.GetHashCode()
                ^ _liquidity.GetHashCode()
                ^ _microseconds.GetHashCode();
        }

        public override bool Equals(object right)
        {
            if (ReferenceEquals(right, null))
                return false;

            if (GetType() != right.GetType())
                return false;

            return Equals((Execution)right);
        }

        public bool Equals(Execution other)
        {
            return _price == other._price
                && _quantity == other._quantity
                && _liquidity == other._liquidity
                && _microseconds == other._microseconds;
        }

        public static bool operator==(Execution left, Execution right)
        {
            return left.Equals(right);
        }

        public static bool operator!=(Execution left, Execution right)
        {
            return !left.Equals(right);
        }

        internal static Execution FromSeto(Proto.Seto.Execution setoExecution, QuantityType quantityType)
        {
            return new Execution(
                new Price(setoExecution.Price),
                new Quantity(quantityType, setoExecution.Quantity),
                setoExecution.Liquidity == Proto.Seto.Side.SIDEBUY ? Side.Buy : Side.Sell,
                setoExecution.Microseconds);
        }

        internal static Execution? MaybeFromSeto(Proto.Seto.Execution setoExecution, QuantityType? quantityType)
        {
            if (setoExecution == null || !quantityType.HasValue)
                return null;
            return FromSeto(setoExecution, quantityType.Value);
        }
    }
}
