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

using System.Globalization;

using IronSmarkets.Proto.Seto;

namespace IronSmarkets.Data
{
    public struct Uuid
    {
        private readonly ulong _high;
        private readonly ulong _low;

        public ulong High { get { return _high; } }
        public ulong Low { get { return _low; } }

        private Uuid(ulong low) : this(0, low) {}
        private Uuid(ulong high, ulong low)
        {
            _high = high;
            _low = low;
        }

        public override string ToString()
        {
            if (_high > 0)
            {
                return _high.ToString("X").ToLower() +
                    _low.ToString("X").ToLower();
            }

            return _low.ToString("X").ToLower();
        }

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

        /// <summary>
        ///   Parses a hex representation of a 128-bit UUID.
        /// </summary>
        public static Uuid Parse(string str)
        {
            if (str.Length > 8)
            {
                string highStr = str.Substring(0, str.Length - 8);
                string lowStr = str.Substring(str.Length - 8);
                return new Uuid(
                    ulong.Parse(highStr, NumberStyles.HexNumber),
                    ulong.Parse(lowStr, NumberStyles.HexNumber));
            }

            // We have a number which can be represented without the
            // high long.
            return new Uuid(
                ulong.Parse(str, NumberStyles.HexNumber));
        }

        public static Uuid FromUuid128(Uuid128 uuid)
        {
            return new Uuid(uuid.High, uuid.Low);
        }
    }
}
