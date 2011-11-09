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
using System.Diagnostics;
using System.Linq;

namespace IronSmarkets.Data
{
    public enum PriceType
    {
        PercentOdds
    }

    internal class BidOrder : IComparer<Price>
    {
        public int Compare(Price left, Price right)
        {
            return right.Raw.CompareTo(left.Raw);
        }
    }

    internal class OfferOrder : IComparer<Price>
    {
        public int Compare(Price left, Price right)
        {
            return left.Raw.CompareTo(right.Raw);
        }
    }

    public struct Price : IEquatable<Price>
    {
        private static readonly IDictionary<Proto.Seto.PriceType, PriceType> PriceTypes =
            new Dictionary<Proto.Seto.PriceType, PriceType> {
            { Proto.Seto.PriceType.PRICEPERCENTODDS, PriceType.PercentOdds }
        };
        internal static readonly IComparer<Price> BidOrder = new BidOrder();
        internal static readonly IComparer<Price> OfferOrder = new OfferOrder();

        private const decimal Divisor = 10000m;
        private readonly uint _raw;
        private readonly PriceType _type;

        public uint Raw { get { return _raw; } }
        public decimal Percent { get { return _raw / Divisor; } }
        public decimal Decimal { get { return EuropeanTable.RawToDecimal(_raw); } }

        public Price(PriceType type, uint raw)
        {
            Debug.Assert(type == PriceType.PercentOdds);
            _type = type;
            _raw = raw;
        }

        internal Price(Proto.Seto.PriceType type, uint raw)
            : this(PriceTypes[type], raw)
        {
        }

        public override int GetHashCode()
        {
            return _type.GetHashCode() ^ _raw.GetHashCode();
        }

        public override bool Equals(object right)
        {
            if (ReferenceEquals(right, null))
                return false;

            if (GetType() != right.GetType())
                return false;

            return Equals((Price)right);
        }

        public override string ToString()
        {
            return Percent.ToString("#,0.00#%");
        }

        public bool Equals(Price other)
        {
            return _type == other._type && _raw == other._raw;
        }

        public static bool operator==(Price left, Price right)
        {
            return left.Equals(right);
        }

        public static bool operator!=(Price left, Price right)
        {
            return !left.Equals(right);
        }

        public static Price FromDecimal(decimal odds)
        {
            return new Price(
                PriceType.PercentOdds,
                EuropeanTable.DecimalToRaw(odds));
        }

        public static IEnumerable<decimal> ValidDecimals
        {
            get
            {
                return EuropeanTable.Odds.Select(x => x.Key);
            }
        }

        internal static PriceType PriceTypeFromSeto(Proto.Seto.PriceType type)
        {
            return PriceTypes[type];
        }
    }

    public static class EuropeanTable
    {
        public static BiDictionary<decimal, uint> Odds =
            new BiDictionary<decimal, uint> {
            { 1.0001m, 9999 }, { 1.01m, 9901 }, { 1.02m, 9804 },
            { 1.03m, 9709 }, { 1.04m, 9615 }, { 1.05m, 9524 }, { 1.06m, 9434 },
            { 1.07m, 9346 }, { 1.08m, 9259 }, { 1.09m, 9174 }, { 1.10m, 9091 },
            { 1.11m, 9009 }, { 1.12m, 8929 }, { 1.13m, 8850 }, { 1.14m, 8772 },
            { 1.15m, 8696 }, { 1.16m, 8621 }, { 1.17m, 8547 }, { 1.18m, 8475 },
            { 1.19m, 8403 }, { 1.20m, 8333 }, { 1.21m, 8264 }, { 1.22m, 8197 },
            { 1.23m, 8130 }, { 1.24m, 8065 }, { 1.25m, 8000 }, { 1.26m, 7937 },
            { 1.27m, 7874 }, { 1.28m, 7812 }, { 1.29m, 7752 }, { 1.30m, 7692 },
            { 1.31m, 7634 }, { 1.32m, 7576 }, { 1.33m, 7519 }, { 1.34m, 7463 },
            { 1.35m, 7407 }, { 1.36m, 7353 }, { 1.37m, 7299 }, { 1.38m, 7246 },
            { 1.39m, 7194 }, { 1.40m, 7143 }, { 1.41m, 7092 }, { 1.42m, 7042 },
            { 1.43m, 6993 }, { 1.44m, 6944 }, { 1.45m, 6897 }, { 1.46m, 6849 },
            { 1.47m, 6803 }, { 1.48m, 6757 }, { 1.49m, 6711 }, { 1.50m, 6667 },
            { 1.51m, 6623 }, { 1.52m, 6579 }, { 1.53m, 6536 }, { 1.54m, 6494 },
            { 1.55m, 6452 }, { 1.56m, 6410 }, { 1.57m, 6369 }, { 1.58m, 6329 },
            { 1.59m, 6289 }, { 1.60m, 6250 }, { 1.61m, 6211 }, { 1.62m, 6173 },
            { 1.63m, 6135 }, { 1.64m, 6098 }, { 1.65m, 6061 }, { 1.66m, 6024 },
            { 1.67m, 5988 }, { 1.68m, 5952 }, { 1.69m, 5917 }, { 1.70m, 5882 },
            { 1.71m, 5848 }, { 1.72m, 5814 }, { 1.73m, 5780 }, { 1.74m, 5747 },
            { 1.75m, 5714 }, { 1.76m, 5682 }, { 1.77m, 5650 }, { 1.78m, 5618 },
            { 1.79m, 5587 }, { 1.80m, 5556 }, { 1.81m, 5525 }, { 1.82m, 5495 },
            { 1.83m, 5464 }, { 1.84m, 5435 }, { 1.85m, 5405 }, { 1.86m, 5376 },
            { 1.87m, 5348 }, { 1.88m, 5319 }, { 1.89m, 5291 }, { 1.90m, 5263 },
            { 1.91m, 5236 }, { 1.92m, 5208 }, { 1.93m, 5181 }, { 1.94m, 5155 },
            { 1.95m, 5128 }, { 1.96m, 5102 }, { 1.97m, 5076 }, { 1.98m, 5051 },
            { 1.99m, 5025 }, { 2.00m, 5000 }, { 2.02m, 4950 }, { 2.04m, 4902 },
            { 2.06m, 4854 }, { 2.08m, 4808 }, { 2.10m, 4762 }, { 2.12m, 4717 },
            { 2.14m, 4673 }, { 2.16m, 4630 }, { 2.18m, 4587 }, { 2.20m, 4545 },
            { 2.22m, 4505 }, { 2.24m, 4464 }, { 2.26m, 4425 }, { 2.28m, 4386 },
            { 2.30m, 4348 }, { 2.32m, 4310 }, { 2.34m, 4274 }, { 2.36m, 4237 },
            { 2.38m, 4202 }, { 2.40m, 4167 }, { 2.42m, 4132 }, { 2.44m, 4098 },
            { 2.46m, 4065 }, { 2.48m, 4032 }, { 2.50m, 4000 }, { 2.52m, 3968 },
            { 2.54m, 3937 }, { 2.56m, 3906 }, { 2.58m, 3876 }, { 2.60m, 3846 },
            { 2.62m, 3817 }, { 2.64m, 3788 }, { 2.66m, 3759 }, { 2.68m, 3731 },
            { 2.70m, 3704 }, { 2.72m, 3676 }, { 2.74m, 3650 }, { 2.76m, 3623 },
            { 2.78m, 3597 }, { 2.80m, 3571 }, { 2.82m, 3546 }, { 2.84m, 3521 },
            { 2.86m, 3497 }, { 2.88m, 3472 }, { 2.90m, 3448 }, { 2.92m, 3425 },
            { 2.94m, 3401 }, { 2.96m, 3378 }, { 2.98m, 3356 }, { 3.00m, 3333 },
            { 3.05m, 3279 }, { 3.10m, 3226 }, { 3.15m, 3175 }, { 3.20m, 3125 },
            { 3.25m, 3077 }, { 3.30m, 3030 }, { 3.35m, 2985 }, { 3.40m, 2941 },
            { 3.45m, 2899 }, { 3.50m, 2857 }, { 3.55m, 2817 }, { 3.60m, 2778 },
            { 3.65m, 2740 }, { 3.70m, 2703 }, { 3.75m, 2667 }, { 3.80m, 2632 },
            { 3.85m, 2597 }, { 3.90m, 2564 }, { 3.95m, 2532 }, { 4.00m, 2500 },
            { 4.1m, 2439 }, { 4.2m, 2381 }, { 4.3m, 2326 }, { 4.4m, 2273 },
            { 4.5m, 2222 }, { 4.6m, 2174 }, { 4.7m, 2128 }, { 4.8m, 2083 },
            { 4.9m, 2041 }, { 5.0m, 2000 }, { 5.1m, 1961 }, { 5.2m, 1923 },
            { 5.3m, 1887 }, { 5.4m, 1852 }, { 5.5m, 1818 }, { 5.6m, 1786 },
            { 5.7m, 1754 }, { 5.8m, 1724 }, { 5.9m, 1695 }, { 6.0m, 1667 },
            { 6.2m, 1613 }, { 6.4m, 1562 }, { 6.6m, 1515 }, { 6.8m, 1471 },
            { 7.0m, 1429 }, { 7.2m, 1389 }, { 7.4m, 1351 }, { 7.6m, 1316 },
            { 7.8m, 1282 }, { 8.0m, 1250 }, { 8.2m, 1220 }, { 8.4m, 1190 },
            { 8.6m, 1163 }, { 8.8m, 1136 }, { 9.0m, 1111 }, { 9.2m, 1087 },
            { 9.4m, 1064 }, { 9.6m, 1042 }, { 9.8m, 1020 }, { 10.0m, 1000 },
            { 10.5m, 952 }, { 11.0m, 909 }, { 11.5m, 870 }, { 12.0m, 833 },
            { 12.5m, 800 }, { 13.0m, 769 }, { 13.5m, 741 }, { 14.0m, 714 },
            { 14.5m, 690 }, { 15.0m, 667 }, { 15.5m, 645 }, { 16.0m, 625 },
            { 16.5m, 606 }, { 17.0m, 588 }, { 17.5m, 571 }, { 18.0m, 556 },
            { 18.5m, 541 }, { 19.0m, 526 }, { 19.5m, 513 }, { 20.0m, 500 },
            { 21m, 476 }, { 22m, 455 }, { 23m, 435 }, { 24m, 417 },
            { 25m, 400 }, { 26m, 385 }, { 27m, 370 }, { 28m, 357 },
            { 29m, 345 }, { 30m, 333 }, { 32m, 312 }, { 34m, 294 },
            { 36m, 278 }, { 38m, 263 }, { 40m, 250 }, { 42m, 238 },
            { 44m, 227 }, { 46m, 217 }, { 48m, 208 }, { 50m, 200 },
            { 55m, 182 }, { 60m, 167 }, { 65m, 154 }, { 70m, 143 },
            { 75m, 133 }, { 80m, 125 }, { 85m, 118 }, { 90m, 111 },
            { 95m, 105 }, { 100m, 100 }, { 110m, 91 }, { 120m, 83 },
            { 130m, 77 }, { 140m, 71 }, { 150m, 67 }, { 160m, 62 },
            { 170m, 59 }, { 180m, 56 }, { 190m, 53 }, { 200m, 50 },
            { 210m, 48 }, { 220m, 45 }, { 230m, 43 }, { 240m, 42 },
            { 250m, 40 }, { 260m, 38 }, { 270m, 37 }, { 280m, 36 },
            { 290m, 34 }, { 300m, 33 }, { 310m, 32 }, { 320m, 31 },
            { 330m, 30 },

            // Gaps in range due to lack of percentage precision
            // below:
            { 340m, 29 },
            { 360m, 28 },
            { 370m, 27 },
            { 380m, 26 },
            { 400m, 25 },
            { 420m, 24 },
            { 430m, 23 },
            { 450m, 22 },
            { 480m, 21 },
            { 500m, 20 },
            { 530m, 19 },
            { 560m, 18 },
            { 590m, 17 },
            { 620m, 16 },
            { 670m, 15 },
            { 710m, 14 },
            { 770m, 13 },
            { 830m, 12 },
            { 910m, 11 },
            { 1000m, 10 },
            { 10000m, 1 }
        };

        public static decimal RawToDecimal(uint raw)
        {
            decimal odds;
            if (Odds.TryGetValue(raw, out odds))
                return odds;
            return Math.Round(10000m / raw, 4);
        }

        public static uint DecimalToRaw(decimal odds)
        {
            uint raw;
            if (Odds.TryGetValue(odds, out raw))
                return raw;

            // This is an annoying precision issue within the exchange
            // which matches orders with an implied percentage value
            // instead of directly using European decimal odds. It
            // only rears its ugly head at the "high" end of the odds
            // spectrum.
            throw new ArgumentException("Unsupported value", "odds");
        }
    }
}
