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

using Xunit;

using IronSmarkets.Data;
using IronSmarkets.Extensions;

namespace IronSmarkets.Tests
{
    public class UidTests
    {
        [Fact]
        public void EqualityTest()
        {
            Assert.Equal(new Uid(93, 2981), new Uid(93, 2981));
            Assert.Equal(
                new Uid(93, 2981).GetHashCode(),
                new Uid(93, 2981).GetHashCode());
            Assert.Equal(new Uid(93), new Uid(0, 93));
            Assert.Equal(
                new Uid(93).GetHashCode(),
                new Uid(0, 93).GetHashCode());
        }

        [Fact]
        public void ParseTest()
        {
            Assert.Equal(new Uid(1), Uid.Parse("1"));
            Assert.Equal(new Uid(9238109283), Uid.Parse("226a25c63"));
            Assert.Equal(
                new Uid(18446744073709551615, 18446744073709551615),
                Uid.Parse("ffffffffffffffffffffffffffffffff"));
            Assert.Equal(
                new Uid(18446744073709551615, 0),
                Uid.Parse("ffffffffffffffff0000000000000000"));
        }

        [Fact]
        public void ParseRoundtripTest()
        {
            var tests = new List<string>{
                "1",
                "226a25c63",
                "ffffffffffffffffffffffffffffffff",
                "ffffffffffffffff0000000000000000"
            };
            tests.ForAll(x => Assert.Equal(Uid.Parse(x).ToString(), x));
        }

        [Fact]
        public void SlugTest()
        {
            var uid1 = new Uid(340, 2034);
            var uid2 = new Uid(3408723498, 20347893249);
            Assert.Equal(uid1.ToSlug(UidTag.Account), "a-1fn9i4uf0qcq7sjlq9");
            Assert.Equal(uid2.ToSlug(UidTag.Account), "a-8k9z998uxzp3343bc0ongh");
            UidTag tag;
            var uid3 = Uid.ParseSlug("1fn9i4uf0qcq7sjlq9", out tag);
            Assert.Equal(uid3, uid1);
            Assert.Equal(UidTag.Account, tag);
            var uid4 = Uid.ParseSlug("a-8k9z998uxzp3343bc0ongh", out tag);
            Assert.Equal(uid4, uid2);
            Assert.Equal(UidTag.Account, tag);
            Assert.Throws<ArgumentException>(() => Uid.ParseSlug("c-8k9z998uxzp3343bc0ongh", out tag));
            Assert.Throws<ArgumentException>(() => Uid.ParseSlug("a", out tag));
            Assert.Throws<ArgumentException>(() => Uid.ParseSlug("", out tag));
            Assert.Throws<ArgumentNullException>(() => Uid.ParseSlug(null, out tag));
        }

        [Fact]
        public void HexTest()
        {
            var uid1 = new Uid(3408723498, 20347893249);
            const string uid1Hex = "0000cb2cfe2a00000004bcd43601acc1";
            var uid2 = new Uid(1);
            const string uid2Hex = "0000000000000000000000000001acc1";
            Assert.Equal(uid1.ToHex(UidTag.Account), uid1Hex);
            Assert.Equal(uid2.ToHex(UidTag.Account), uid2Hex);
            UidTag tag;
            var uid3 = Uid.ParseHex(uid1Hex, out tag);
            Assert.Equal(uid1, uid3);
            Assert.Equal(tag, UidTag.Account);
            var uid4 = Uid.ParseHex(uid2Hex, out tag);
            Assert.Equal(uid2, uid4);
            Assert.Equal(tag, UidTag.Account);
            Assert.Throws<ArgumentException>(() => Uid.ParseHex("ffacc2", out tag));
            Assert.Throws<ArgumentException>(() => Uid.ParseHex("ggacc1", out tag));
            Assert.Throws<ArgumentException>(() => Uid.ParseHex("", out tag));
            Assert.Throws<ArgumentNullException>(() => Uid.ParseHex(null, out tag));
        }

        [Fact]
        public void HexTagsAreZeroPadded()
        {
            new[] {
                UidTag.Account, UidTag.ContractGroup, UidTag.Contract, UidTag.Order,
                UidTag.Comment, UidTag.Entity, UidTag.Event, UidTag.Session, UidTag.User,
                UidTag.Referrer
            }.ForAll(tag => Assert.Equal(4, tag.HexString.Length));
        }
    }
}
