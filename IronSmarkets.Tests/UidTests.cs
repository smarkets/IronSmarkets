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
    }
}
