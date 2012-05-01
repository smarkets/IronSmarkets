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
//
// Partially adapted from: http://moneytype.codeplex.com/
// See LICENSE-MSPL in the root of this project for details.

using System.Globalization;

using Xunit;

using IronSmarkets.Data;

namespace IronSmarkets.Tests
{
    public sealed class CurrencyTests
    {
        [Fact]
        public void CurrencyFromCurrentCultureEqualsCurrentCultureCurrency()
        {
            var currency1 = Currency.FromIso3LetterCode(
                new RegionInfo(CultureInfo.CurrentCulture.LCID).ISOCurrencySymbol);
            var currency2 = Currency.FromCurrentCulture();

            Assert.Equal(currency1.Description, currency2.Description);
        }

        [Fact]
        public void CurrencyFromSpecificCultureInfoIsCorrect()
        {
            // Use CultureInfo for es-ES
            var currency = Currency.FromCultureInfo(new CultureInfo(3082));

            // Ensure that we end up with EUR
            Assert.Equal(978, currency.Description.IsoNumericCode);
            Assert.Equal("EUR", currency.Description.Iso3LetterCode);
        }

        [Fact]
        public void CurrencyFromSpecificIsoCodeIsCorrect()
        {
            var currency = Currency.FromIso3LetterCode("EUR");

            Assert.Equal(978, currency.Description.IsoNumericCode);
        }

        [Fact]
        public void CurrencyHasValueEquality()
        {
            var currency1 = Currency.FromIso3LetterCode("GBP");
            var currency2 = Currency.FromIso3LetterCode("GBP");
            object boxedCurrency2 = currency2;

            Assert.True(currency1.Equals(currency2));
            Assert.True(currency1.Equals(boxedCurrency2));
        }
    }
}
