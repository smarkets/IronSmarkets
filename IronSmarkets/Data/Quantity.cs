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
    public struct Quantity : IEquatable<Quantity>
    {
        private const decimal DIVISOR = 10000.0000m;
        private readonly uint _raw;

        public uint Raw { get { return _raw; } }
        public decimal Currency { get { return (decimal)_raw / DIVISOR; } }

        public Quantity(uint raw)
        {
            _raw = raw;
        }

        public Quantity(decimal currency)
        {
            _raw = (uint)(currency * DIVISOR);
        }

        public override int GetHashCode()
        {
            return _raw.GetHashCode();
        }

        public override string ToString()
        {
            return Currency.ToString("#,#.00#");
        }

        public override bool Equals(object right)
        {
            if (ReferenceEquals(right, null))
                return false;

            if (GetType() != right.GetType())
                return false;

            return Equals((Quantity)right);
        }

        public bool Equals(Quantity other)
        {
            return _raw == other._raw;
        }

        public static bool operator==(Quantity left, Quantity right)
        {
            return left.Equals(right);
        }

        public static bool operator!=(Quantity left, Quantity right)
        {
            return !left.Equals(right);
        }
    }
}
