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
            var dt = MicrosecondNow();
            var dt2 = MicrosecondNow().AddDays(1);
            var dtRoundtrip = SetoMap.FromMicroseconds(SetoMap.ToMicroseconds(dt));
            var dt2Roundtrip = SetoMap.FromMicroseconds(SetoMap.ToMicroseconds(dt));
            Assert.Equal(dt, dtRoundtrip);
            Assert.Equal(dt.Kind, dtRoundtrip.Kind);
            Assert.NotEqual(dt2, dt2Roundtrip);
            Assert.Equal(dt2.Kind, dt2Roundtrip.Kind);
            Assert.Equal((ulong)9800, SetoMap.ToMicroseconds(SetoMap.FromMicroseconds(9800)));
            Assert.Equal((ulong)1, SetoMap.ToMicroseconds(SetoMap.FromMicroseconds(1)));
            Assert.NotEqual((ulong)1, SetoMap.ToMicroseconds(SetoMap.FromMicroseconds(2)));
        }

        [Fact]
        public void SetoDateRoundtrip()
        {
            var date = new Proto.Seto.Date();
            date.Year = 2012;
            date.Month = 1;
            date.Day = 1;
            var dateTime = SetoMap.FromDateTime(date, null);
            Assert.NotNull(dateTime);
            Assert.Equal(dateTime.Value.Year, (int)date.Year);
            Assert.Equal(dateTime.Value.Month, (int)date.Month);
            Assert.Equal(dateTime.Value.Day, (int)date.Day);
            Assert.Equal(dateTime.Value.Hour, 0);
            Assert.Equal(dateTime.Value.Minute, 0);
            Assert.Equal(dateTime.Value.Second, 0);
            Assert.Equal(dateTime.Value.Kind, DateTimeKind.Utc);
        }

        [Fact]
        public void SetoDateTimeRoundtrip()
        {
            var date = new Proto.Seto.Date();
            date.Year = 2012;
            date.Month = 1;
            date.Day = 1;
            var time = new Proto.Seto.Time();
            time.Hour = 5;
            time.Minute = 10;
            var dateTime = SetoMap.FromDateTime(date, time);
            Assert.NotNull(dateTime);
            Assert.Equal(dateTime.Value.Year, (int)date.Year);
            Assert.Equal(dateTime.Value.Month, (int)date.Month);
            Assert.Equal(dateTime.Value.Day, (int)date.Day);
            Assert.Equal(dateTime.Value.Hour, (int)time.Hour);
            Assert.Equal(dateTime.Value.Minute, (int)time.Minute);
            Assert.Equal(dateTime.Value.Second, 0);
            Assert.Equal(dateTime.Value.Kind, DateTimeKind.Utc);
        }

        /// <summary>
        ///   Provides a DateTime with microsecond resolution (instead
        ///   of 100-nanoseconds by default)
        /// </summary>
        private DateTime MicrosecondNow()
        {
            var dt = DateTime.UtcNow;
            return new DateTime(dt.Ticks - (dt.Ticks % 10), DateTimeKind.Utc);
        }
    }
}
