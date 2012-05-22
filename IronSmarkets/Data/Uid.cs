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
#if NET40
using System.Diagnostics.Contracts;
#endif
using System.Globalization;

using IronSmarkets.Proto.Seto;

namespace IronSmarkets.Data
{
    public struct Uid : IEquatable<Uid>
    {
        private readonly ulong _high;
        private readonly ulong _low;

        public ulong High { get { return _high; } }
        public ulong Low { get { return _low; } }

        public Uid(ulong low) : this(0, low) {}
        public Uid(ulong high, ulong low)
        {
            _high = high;
            _low = low;
        }

        public override int GetHashCode()
        {
            return _high.GetHashCode() ^ _low.GetHashCode();
        }

        public override bool Equals(object right)
        {
            if (ReferenceEquals(right, null))
                return false;

            if (GetType() != right.GetType())
                return false;

            return Equals((Uid)right);
        }

        public bool Equals(Uid other)
        {
            return _high == other._high && _low == other._low;
        }

        public override string ToString()
        {
            if (_high > 0)
            {
                return _high.ToString("X").ToLower() +
                    _low.ToString("X").ToLower().PadLeft(16, '0');
            }

            return _low.ToString("X").ToLower();
        }

#if NET40
        [Pure]
#endif
        public Uuid128 ToUuid128()
        {
            if (_high > 0)
            {
                return new Uuid128 {
                    High = _high,
                    Low = _low
                };
            }

            return new Uuid128 {
                Low = _low
            };
        }

        public static bool operator==(Uid left, Uid right)
        {
            return left.Equals(right);
        }

        public static bool operator!=(Uid left, Uid right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///   Parses a hex representation of a 128-bit UUID.
        /// </summary>
        public static Uid Parse(string str)
        {
            if (str.Length > 16)
            {
                string highStr = str.Substring(0, str.Length - 16);
                string lowStr = str.Substring(str.Length - 16);
                return new Uid(
                    ulong.Parse(highStr, NumberStyles.HexNumber),
                    ulong.Parse(lowStr, NumberStyles.HexNumber));
            }

            // We have a number which can be represented without the
            // high long.
            return new Uid(
                ulong.Parse(str, NumberStyles.HexNumber));
        }

        public static Uid FromUuid128(Uuid128 uuid)
        {
            return new Uid(uuid.High, uuid.Low);
        }

        public static Uid? MaybeFromUuid128(Uuid128 uuid)
        {
            if (uuid == null)
                return null;

            return FromUuid128(uuid);
        }
    }
}
