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

namespace IronSmarkets.Tests
{
    public class UuidTests
    {
        [Fact]
        public void EqualityTest()
        {
            Assert.Equal(new Uuid(93, 2981), new Uuid(93, 2981));
            Assert.Equal(
                new Uuid(93, 2981).GetHashCode(),
                new Uuid(93, 2981).GetHashCode());
            Assert.Equal(new Uuid(93), new Uuid(0, 93));
            Assert.Equal(
                new Uuid(93).GetHashCode(),
                new Uuid(0, 93).GetHashCode());
        }

        [Fact]
        public void ParseTest()
        {
            Assert.Equal(new Uuid(1), Uuid.Parse("1"));
            Assert.Equal(new Uuid(9238109283), Uuid.Parse("226a25c63"));
            Assert.Equal(
                new Uuid(18446744073709551615, 18446744073709551615),
                Uuid.Parse("ffffffffffffffffffffffffffffffff"));
            Assert.Equal(
                new Uuid(18446744073709551615, 0),
                Uuid.Parse("ffffffffffffffff0000000000000000"));
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
            tests.ForAll(x => Assert.Equal(Uuid.Parse(x).ToString(), x));
        }
    }
}
