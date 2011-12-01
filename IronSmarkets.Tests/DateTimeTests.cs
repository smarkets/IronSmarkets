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

using Xunit;

using IronSmarkets.Data;

namespace IronSmarkets.Tests
{
    public sealed class DateTimeTests
    {
	[Fact]
	public void MicrosecondsRoundtrip()
	{
            var dt = DateTime.UtcNow;
            var dt2 = DateTime.UtcNow.AddDays(1);
            Assert.Equal(dt, SetoMap.FromMicroseconds(SetoMap.ToMicroseconds(dt)));
            Assert.NotEqual(dt2, SetoMap.FromMicroseconds(SetoMap.ToMicroseconds(dt)));
            Assert.Equal((ulong)9800, SetoMap.ToMicroseconds(SetoMap.FromMicroseconds(9800)));
            Assert.Equal((ulong)1, SetoMap.ToMicroseconds(SetoMap.FromMicroseconds(1)));
            Assert.NotEqual((ulong)1, SetoMap.ToMicroseconds(SetoMap.FromMicroseconds(2)));
	}
    }
}
