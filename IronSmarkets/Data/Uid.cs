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
using System.Globalization;
using System.Linq;
#if NET40
using System.Diagnostics.Contracts;
using System.Numerics;
#endif

using IronSmarkets.Proto.Seto;
#if NET35
using IronSmarkets.System;
using IronSmarkets.System.Numerics;
#endif

namespace IronSmarkets.Data
{
    public struct UidTag : IEquatable<UidTag>
    {
        public static readonly UidTag Account =
            new UidTag("Account",       ushort.Parse("acc1", NumberStyles.HexNumber), 'a');
        public static readonly UidTag ContractGroup =
            new UidTag("ContractGroup", ushort.Parse("c024", NumberStyles.HexNumber), 'm');
        public static readonly UidTag Contract =
            new UidTag("Contract",      ushort.Parse("cccc", NumberStyles.HexNumber), 'c');
        public static readonly UidTag Order =
            new UidTag("Order",         ushort.Parse("fff0", NumberStyles.HexNumber), 'o');
        public static readonly UidTag Comment =
            new UidTag("Comment",       ushort.Parse("b1a4", NumberStyles.HexNumber), 'b');
        public static readonly UidTag Entity =
            new UidTag("Entity",        ushort.Parse("0444", NumberStyles.HexNumber), 'n');
        public static readonly UidTag Event =
            new UidTag("Event",         ushort.Parse("1100", NumberStyles.HexNumber), 'e');
        public static readonly UidTag Session =
            new UidTag("Session",       ushort.Parse("9999", NumberStyles.HexNumber), 's');
        public static readonly UidTag User =
            new UidTag("User",          ushort.Parse("0f00", NumberStyles.HexNumber), 'u');
        public static readonly UidTag Referrer =
            new UidTag("Referrer",      ushort.Parse("4e4e", NumberStyles.HexNumber), 'r');

        internal static readonly IList<UidTag> TagList = new List<UidTag> {
            Account, ContractGroup, Contract, Order, Comment, Entity, Event, Session, User, Referrer
        };
        internal static readonly IDictionary<string, UidTag> Tags = TagList.ToDictionary(x => x.Name);
        internal static readonly IDictionary<string, UidTag> TagsByHexString = TagList.ToDictionary(x => x.HexString);
        internal static readonly IDictionary<char, UidTag> TagsByPrefix = TagList.ToDictionary(x => x.Prefix);
        internal static readonly IDictionary<ushort, UidTag> TagsByIntTag = TagList.ToDictionary(x => x.IntTag);

        private readonly string _name;
        private readonly ushort _intTag;
        private readonly char _prefix;

        public string Name { get { return _name; } }
        public ushort IntTag { get { return _intTag; } }
        public char Prefix { get { return _prefix; } }
        public string HexString { get { return _intTag.ToString("x4").ToLower(); } }

        private UidTag(string name, ushort intTag, char prefix)
        {
            _name = name;
            _intTag = intTag;
            _prefix = prefix;
        }

        public Uid Tag(Uid original)
        {
            var high = original.High << 16;
            var low = (original.Low << 16) + _intTag;
            return new Uid(high, low);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode()
                ^ _intTag.GetHashCode()
                ^ _prefix.GetHashCode();
        }

        public override bool Equals(object right)
        {
            if (ReferenceEquals(right, null))
                return false;

            if (GetType() != right.GetType())
                return false;

            return Equals((UidTag)right);
        }

        public bool Equals(UidTag other)
        {
            return _name == other._name
                && _intTag == other._intTag
                && _prefix == other._prefix;
        }

        internal static Uid Split(Uid uid, out UidTag tag)
        {
            var high = uid.High >> 16;
            var low = uid.Low >> 16;
            var intTag = (ushort)(uid.Low & UInt16.MaxValue);
            if (!TagsByIntTag.TryGetValue(intTag, out tag))
                throw new ArgumentException("Uid does not have a valid tag", "uid");
            return new Uid(high, low);
        }

        internal static Uid Split(BigInteger uid, out UidTag tag)
        {
            var low = (ulong)(uid & ulong.MaxValue);
            var high = (ulong)((uid >> 64) & ulong.MaxValue);
            return Split(new Uid(high, low), out tag);
        }

        public static bool operator==(UidTag left, UidTag right)
        {
            return left.Equals(right);
        }

        public static bool operator!=(UidTag left, UidTag right)
        {
            return !left.Equals(right);
        }
    }

    public struct Uid : IEquatable<Uid>
    {
        private static readonly char[] CharList = "0123456789abcdefghijklmnopqrstuvwxyz".ToCharArray();

        private readonly ulong _high;
        private readonly ulong _low;

        public ulong High { get { return _high; } }
        public ulong Low { get { return _low; } }

        public BigInteger BigInteger
        {
            get
            {
                return new BigInteger(new[] {
                        (byte)(Low & 255),
                        (byte)((Low >> 8) & 255),
                        (byte)((Low >> 16) & 255),
                        (byte)((Low >> 24) & 255),
                        (byte)((Low >> 32) & 255),
                        (byte)((Low >> 40) & 255),
                        (byte)((Low >> 48) & 255),
                        (byte)((Low >> 56) & 255),
                        (byte)(High & 255),
                        (byte)((High >> 8) & 255),
                        (byte)((High >> 16) & 255),
                        (byte)((High >> 24) & 255),
                        (byte)((High >> 32) & 255),
                        (byte)((High >> 40) & 255),
                        (byte)((High >> 48) & 255),
                        (byte)((High >> 56) & 255)
                    });
            }
        }

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
                return _high.ToString("x") + _low.ToString("x16");
            return _low.ToString("x");
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

        public string ToSlug(UidTag tag)
        {
            return ToSlug(tag, true);
        }

        public string ToSlug(UidTag tag, bool withPrefix)
        {
            var base36 = Encode36(tag.Tag(this));
            return withPrefix ? string.Format("{0}-{1}", tag.Prefix, base36) : base36;
        }

        public string ToHex(UidTag tag)
        {
            if (_high > 0)
                return _high.ToString("x12") + _low.ToString("x16") + tag.HexString;
            return _low.ToString("x28") + tag.HexString;
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

        /// <summary>
        ///   Parses a slug representation (verifying prefix) into a Uid
        /// </summary>
        public static Uid ParseSlug(string slug, out UidTag tag)
        {
            if (slug == "")
                throw new ArgumentException("string cannot be empty", "slug");
            if (slug == null)
                throw new ArgumentNullException("slug", "string cannot be empty");
            if (slug.Length > 3 && slug[1] == '-')
            {
                var uid = UidTag.Split(Decode(slug.Substring(2), 36), out tag);
                if (slug[0] != tag.Prefix)
                    throw new ArgumentException(
                        string.Format("slug with prefix {0} should have prefix {1}",
                                      slug[0], tag.Prefix), "slug");
                return uid;
            }

            return UidTag.Split(Decode(slug, 36), out tag);
        }

        /// <summary>
        ///   Parses a tagged hex representation into a Uid and UidTag
        /// </summary>
        public static Uid ParseHex(string hex, out UidTag tag)
        {
            if (hex == "")
                throw new ArgumentException("string cannot be empty", "hex");
            if (hex == null)
                throw new ArgumentNullException("hex", "string cannot be null");
            return UidTag.Split(Decode(hex, 16), out tag);
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

        private static string Encode36(Uid uid)
        {
            var big = uid.BigInteger;
            var result = new Stack<char>();
            var divisor = new BigInteger(36);
            while (big != 0)
            {
                BigInteger remainder;
                big = BigInteger.DivRem(big, divisor, out remainder);
                result.Push(CharList[(int)remainder]);
            }
            return new string(result.ToArray());
        }

        private static BigInteger CharIndex(char c, int numBase)
        {
            var converted = Convert.ToInt32(c);
            if (converted >= 48 && converted <= 57 && converted - 48 < numBase)
                return new BigInteger(converted - 48);
            if (converted >= 97 && converted <= 122 && converted - 87 < numBase)
                return new BigInteger(converted - 87);
            if (converted >= 65 && converted <= 90 && converted - 55 < numBase)
                return new BigInteger(converted - 55);
            throw new ArgumentException("character out of range", "c");
        }

        private static BigInteger Decode(string encoded, int numBase)
        {
            var result = new BigInteger();
            int pos = 0;
            return encoded.ToLower().Reverse().Aggregate(result, (current, c) => current + CharIndex(c, numBase) * BigInteger.Pow(numBase, pos++));
        }
    }
}
