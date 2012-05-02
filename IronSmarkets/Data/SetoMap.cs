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

using IronSmarkets.Proto.Seto;

namespace IronSmarkets.Data
{
    internal abstract class SetoMap
    {
        private const long SinceGregorian = 621355968000000000;

        public static DateTime? FromDateTime(Date date, Time time)
        {
            if (date == null)
            {
                return null;
            }

            if (time == null)
            {
                return DateTime.SpecifyKind(
                    new DateTime((int)date.Year, (int)date.Month, (int)date.Day),
                    DateTimeKind.Utc);
            }

            return new DateTime(
                (int)date.Year, (int)date.Month, (int)date.Day,
                (int)time.Hour, (int)time.Minute, 0, DateTimeKind.Utc);
        }

        public static Date FromDateTime(DateTime datetime)
        {
            DateTime utcDateTime = datetime.ToUniversalTime();
            return new Date {
                Year = (uint)utcDateTime.Year,
                Month = (uint)utcDateTime.Month,
                Day = (uint)utcDateTime.Day
            };
        }

        public static decimal FromDecimal(Proto.Seto.Decimal sDecimal)
        {
            var val = new decimal(sDecimal.Value);
            // Annoyingly, there is no integer exponentiation
            var divisor = new decimal((long)Math.Pow(10, sDecimal.Exponent));
            return Math.Round(val / divisor, (int)sDecimal.Exponent);
        }

        public static DateTime FromMicroseconds(ulong erlangTicks)
        {
            return new DateTime((long)erlangTicks * 10 + SinceGregorian, DateTimeKind.Utc);
        }

        public static ulong ToMicroseconds(DateTime dateTime)
        {
            var utc = dateTime.ToUniversalTime();
            return (ulong)((utc.Ticks - SinceGregorian) / 10);
        }
    }
}
