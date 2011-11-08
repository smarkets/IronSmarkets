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
using System.Diagnostics;

namespace IronSmarkets.Data
{
    public enum PriceType
    {
        PercentOdds
    }

    public struct Price : IEquatable<Price>
    {
        private static readonly IDictionary<Proto.Seto.PriceType, PriceType> PriceTypes =
            new Dictionary<Proto.Seto.PriceType, PriceType> {
            { Proto.Seto.PriceType.PRICEPERCENTODDS, PriceType.PercentOdds }
        };

        private const decimal Divisor = 10000m;
        private readonly uint _raw;
        private readonly PriceType _type;

        public uint Raw { get { return _raw; } }
        public decimal Percent { get { return _raw / Divisor; } }

        public Price(PriceType type, uint raw)
        {
            Debug.Assert(type == PriceType.PercentOdds);
            _type = type;
            _raw = raw;
        }

        public override int GetHashCode()
        {
            return _type.GetHashCode() ^ _raw.GetHashCode();
        }

        public override bool Equals(object right)
        {
            if (ReferenceEquals(right, null))
                return false;

            if (GetType() != right.GetType())
                return false;

            return Equals((Price)right);
        }

        public override string ToString()
        {
            return Percent.ToString("#,#.00#%");
        }

        public bool Equals(Price other)
        {
            return _type == other._type && _raw == other._raw;
        }

        public static bool operator==(Price left, Price right)
        {
            return left.Equals(right);
        }

        public static bool operator!=(Price left, Price right)
        {
            return !left.Equals(right);
        }

        internal static PriceType PriceTypeFromSeto(Proto.Seto.PriceType type)
        {
            return PriceTypes[type];
        }
    }
}
