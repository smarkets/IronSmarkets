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
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROm, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
// Partially adapted from: http://moneytype.codeplex.com/
// See LICENSE-MSPL in the root of this project for details.

using System;
using System.Globalization;
using Xunit;

using IronSmarkets.Data;

namespace IronSmarkets.Tests
{
    public sealed class MoneyTests
    {
        [Fact]
        public void MoneyHasValueEquality()
        {
            var money1 = new Money(101.5m);
            var money2 = new Money(101.5m);
            Assert.Equal(money1, money2);
        }

        [Fact]
        public void MoneyWholeAmountAdditionIsCorrect()
        {
            var money1 = new Money(101m);
            var money2 = new Money(99m);

            Assert.Equal(new Money(200m), money1 + money2);
        }

        [Fact]
        public void MoneyFractionalAmountAdditionIsCorrect()
        {
            var money1 = new Money(100.00m);
            var money2 = new Money(0.01m);

            Assert.Equal(new Money(100.01m), money1 + money2);

        }

        [Fact]
        public void MoneyFractionalAmountWithOverflowAdditionIsCorrect()
        {
            var money1 = new Money(100.999M);
            var money2 = new Money(0.9M);

            Assert.Equal(new Money(101.899m), money1 + money2);
        }

        [Fact]
        public void MoneyNegativeAmountAdditionIsCorrect()
        {
            var money1 = new Money(100.999M);
            var money2 = new Money(-0.9M);

            Assert.Equal(new Money(100.099m), money1 + money2);
        }

        [Fact]
        public void MoneyNegativeAmountWithOverflowAdditionIsCorrect()
        {
            var money1 = new Money(-100.999M);
            var money2 = new Money(-0.9M);

            Assert.Equal(new Money(-101.899m), money1 + money2);
        }

        [Fact]
        public void MoneyWholeAmountSubtractionIsCorrect()
        {
            var money1 = new Money(101m);
            var money2 = new Money(99m);

            Assert.Equal(new Money(2), money1 - money2);
        }

        [Fact]
        public void MoneyFractionalAmountSubtractionIsCorrect()
        {
            var money1 = new Money(100.00m);
            var money2 = new Money(0.01m);

            Assert.Equal(new Money(99.99m), money1 - money2);
        }

        [Fact]
        public void MoneyFractionalAmountWithOverflowSubtractionIsCorrect()
        {
            var money1 = new Money(100.5m);
            var money2 = new Money(0.9m);

            Assert.Equal(new Money(99.6m), money1 - money2);
        }

        [Fact]
        public void MoneyNegativeAmountSubtractionIsCorrect()
        {
            var money1 = new Money(100.999m);
            var money2 = -new Money(0.9m);

            Assert.Equal(new Money(101.899m), money1 - money2);
        }

        [Fact]
        public void MoneyNegativeAmountWithOverflowSubtractionIsCorrect()
        {
            var money1 = -new Money(100.999m);
            var money2 = -new Money(0.9m);

            Assert.Equal(new Money(-100.099m), money1 - money2);
        }

        [Fact]
        public void MoneyScalarWholeMultiplicationIsCorrect()
        {
            var money = new Money(100.125m);

            Assert.Equal(500.625m, money * 5);
        }

        [Fact]
        public void MoneyScalarFractionalMultiplicationIsCorrect()
        {
            var money = new Money(100.125m);

            Assert.Equal(50.0625m, money * 0.5m);
        }

        [Fact]
        public void MoneyScalarNegativeWholeMultiplicationIsCorrect()
        {
            var money = -new Money(100.125m);

            Assert.Equal(-500.625m, money * 5);
        }

        [Fact]
        public void MoneyScalarNegativeFractionalMultiplicationIsCorrect()
        {
            var money = -new Money(100.125m);

            Assert.Equal(-50.0625m, money * 0.5m);
        }

        [Fact]
        public void MoneyScalarWholeDivisionIsCorrect()
        {
            var money = new Money(100.125m);

            Assert.Equal(50.0625m, money / 2m);
        }

        [Fact]
        public void MoneyScalarFractionalDivisionIsCorrect()
        {
            var money = new Money(100.125m);

            Assert.Equal(200.25m, money / 0.5m);
        }

        [Fact]
        public void MoneyScalarNegativeWholeDivisionIsCorrect()
        {
            var money = -new Money(100.125m);

            Assert.Equal(-50.0625m, money / 2m);
        }

        [Fact]
        public void MoneyScalarNegativeFractionalDivisionIsCorrect()
        {
            var money = -new Money(100.125m);

            Assert.Equal(-200.25m, money / 0.5m);
        }

        [Fact]
        public void MoneyEqualOperatorIsCorrect()
        {
            var money1 = new Money(100.125m);
            var money2 = new Money(100.125m);

            Assert.True(money1 == money2);
        }

        [Fact]
        public void MoneyNotEqualOperatorIsCorrect()
        {
            var money1 = new Money(100.125m);
            var money2 = new Money(100.125m);

            Assert.False(money1 != money2);
        }

        [Fact]
        public void MoneyGreaterThanEqualOperatorIsCorrect()
        {
            var money1 = new Money(100.125m);
            var money2 = new Money(100.125m);

            Assert.True(money1 >= money2);
        }

        [Fact]
        public void MoneyLessThanEqualOperatorIsCorrect()
        {
            var money1 = new Money(100.125m);
            var money2 = new Money(100.125m);

            Assert.True(money1 <= money2);
        }

        [Fact]
        public void MoneyGreaterThanOperatorIsCorrect()
        {
            var money1 = new Money(100.125m);
            var money2 = new Money(100.125m);

            Assert.False(money1 > money2);
        }

        [Fact]
        public void MoneyLessThanOperatorIsCorrect()
        {
            var money1 = new Money(100.125m);
            var money2 = new Money(100.125m);

            Assert.False(money1 < money2);
        }

        [Fact]
        public void MoneyPrintsCorrectly()
        {
            var money = new Money(100.125m, Currency.Gbp);
            var formattedMoney = money.ToString(CultureInfo.InvariantCulture);
            Assert.Equal("£100.13", formattedMoney);
        }

        [Fact]
        public void MoneyOperationsInvolvingDifferentCurrencyAllFail()
        {
            var money1 = new Money(101.5m, Currency.Gbp);
            var money2 = new Money(98.5m, Currency.Eur);

            Assert.Throws<InvalidOperationException>(() => money1 + money2);
            Assert.Throws<InvalidOperationException>(() => money1 - money2);
            Assert.Throws<InvalidOperationException>(() => money1 > money2);
            Assert.Throws<InvalidOperationException>(() => money1 < money2);
            Assert.Throws<InvalidOperationException>(() => money1 >= money2);
            Assert.Throws<InvalidOperationException>(() => money1 <= money2);
            Assert.DoesNotThrow(() => Assert.False(money1 == money2));
            Assert.DoesNotThrow(() => Assert.True(money1 != money2));
        }
    }
}
