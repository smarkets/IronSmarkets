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
    public struct Quote : IEquatable<Quote>
    {
        private readonly Price _price;
        private readonly Quantity _quantity;

        public Price Price { get { return _price; } }
        public Quantity Quantity { get { return _quantity; } }

        private Quote(Price price, Quantity quantity)
        {
            _price = price;
            _quantity = quantity;
        }

        public override int GetHashCode()
        {
            return _price.GetHashCode() ^ _quantity.GetHashCode();
        }

        public override bool Equals(object right)
        {
            if (ReferenceEquals(right, null))
                return false;

            if (GetType() != right.GetType())
                return false;

            return Equals((Quote)right);
        }

        public bool Equals(Quote other)
        {
            return _price == other._price
                && _quantity == other._quantity;
        }

        public static bool operator==(Quote left, Quote right)
        {
            return left.Equals(right);
        }

        public static bool operator!=(Quote left, Quote right)
        {
            return !left.Equals(right);
        }

        internal static Quote FromSeto(Proto.Seto.Quote setoQuote)
        {
            return new Quote(
                new Price(setoQuote.Price),
                new Quantity(setoQuote.Quantity));
        }
    }
}
