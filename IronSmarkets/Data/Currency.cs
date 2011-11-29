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

using System;
using System.Collections.Generic;
using System.Globalization;

namespace IronSmarkets.Data
{
    internal struct CurrencyDescription : IEquatable<CurrencyDescription>
    {
	public readonly string Name;
	public readonly string Iso3LetterCode;
	public readonly int IsoNumericCode;
	public readonly string Symbol;

	internal CurrencyDescription(
	    string name, string iso3LetterCode,
	    int isoNumberCode, string symbol)
	{
	    Name = name;
	    Iso3LetterCode = iso3LetterCode;
	    IsoNumericCode = isoNumberCode;
	    Symbol = symbol;
	}

	public override int GetHashCode()
	{
	    return Name.GetHashCode()
		^ Iso3LetterCode.GetHashCode()
		^ IsoNumericCode.GetHashCode()
		^ Symbol.GetHashCode();
	}

	public override bool Equals(object right)
	{
	    if (ReferenceEquals(right, null))
		return false;

	    if (GetType() != right.GetType())
		return false;

	    return Equals((CurrencyDescription)right);
	}

	public bool Equals(CurrencyDescription other)
	{
	    return IsoNumericCode == other.IsoNumericCode
		&& Name == other.Name
		&& Iso3LetterCode == other.Iso3LetterCode
		&& Symbol == other.Symbol;
	}

	public static bool operator==(
	    CurrencyDescription left, CurrencyDescription right)
	{
	    return left.Equals(right);
	}

	public static bool operator!=(
	    CurrencyDescription left, CurrencyDescription right)
	{
	    return !left.Equals(right);
	}
    }

    public struct Currency : IEquatable<Currency>, IFormatProvider
    {
	public static readonly Currency Gbp;
	public static readonly Currency Eur;

	private static readonly IDictionary<int, CurrencyDescription> CurrencyDescriptions =
	    new Dictionary<int, CurrencyDescription>();
	private static readonly IDictionary<int, int> CodesByCultureId =
	    new Dictionary<int, int>();
	private static readonly IDictionary<int, int> CultureIdsByCode =
	    new Dictionary<int, int>();
	private static readonly IDictionary<string, int> CodesByIso3Letters =
	    new Dictionary<string, int>();

	private static readonly IDictionary<Proto.Seto.Currency, Currency> Currencies;

	private readonly int _code;

	public int Code { get { return _code; } }

	internal CurrencyDescription Description
	{
	    get
	    {
		CurrencyDescription description;
		if (!CurrencyDescriptions.TryGetValue(_code, out description))
		    throw new InvalidOperationException(
			String.Format("Unknown currency code: {0}", _code));
		return description;
	    }
	}

	private static TValue GetValueOrNull<TKey, TValue>(TKey key, IDictionary<TKey, TValue> table) where TValue : class
	{
	    TValue value;
	    return !table.TryGetValue(key, out value) ? null : value;
	}

	static Currency()
	{
	    var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

	    var cultureIdLookup = new Dictionary<string, List<int>>();
	    var symbolLookup = new Dictionary<string, string>();

	    foreach (var culture in cultures)
	    {
		var lcid = culture.LCID;
		var regionInfo = new RegionInfo(lcid);
		var isoSymbol = regionInfo.ISOCurrencySymbol;

		if (!cultureIdLookup.ContainsKey(isoSymbol))
		    cultureIdLookup[isoSymbol] = new List<int>();

		cultureIdLookup[isoSymbol].Add(lcid);
		symbolLookup[isoSymbol] = regionInfo.CurrencySymbol;
	    }

	    CurrencyDescriptions[826] = new CurrencyDescription(
		"Pound Sterling", "GBP", 826, GetValueOrNull("GBP", symbolLookup));
	    CurrencyDescriptions[978] = new CurrencyDescription(
		"Euro", "EUR", 978, GetValueOrNull("EUR", symbolLookup));

	    foreach (var currency in CurrencyDescriptions.Values)
	    {
		var iso3LetterCode = currency.Iso3LetterCode;
		List<int> lcids;

		if (cultureIdLookup.TryGetValue(iso3LetterCode, out lcids))
		{
		    foreach (var lcid in lcids)
		    {
			CodesByCultureId[lcid] = currency.IsoNumericCode;
			CultureIdsByCode[currency.IsoNumericCode] = lcid;
		    }
		}

		CodesByIso3Letters[iso3LetterCode] = currency.IsoNumericCode;
	    }

	    Gbp = new Currency(826);
	    Eur = new Currency(978);

	    Currencies = new Dictionary<Proto.Seto.Currency, Currency>
		{
		    { Proto.Seto.Currency.CURRENCYGBP, Gbp },
		    { Proto.Seto.Currency.CURRENCYEUR, Eur }
		};
	}

	private Currency(int code)
	{
	    if (!CurrencyDescriptions.ContainsKey(code))
		throw new ArgumentOutOfRangeException(
		    "code", code,
		    "This value is not a valid ISO 4217 currency code.");
	    _code = code;
	}

	internal static Currency FromIso3LetterCode(string iso3LetterCode)
	{
	    int code;
	    if (!CodesByIso3Letters.TryGetValue(iso3LetterCode, out code))
		throw new ArgumentOutOfRangeException(
		    "iso3LetterCode", iso3LetterCode,
		    "Invalid ISO 3 letter code");
	    return new Currency(code);
	}

	public static Currency FromCurrentCulture()
	{
	    return FromCultureInfo(CultureInfo.CurrentCulture);
	}

	public static Currency FromCultureInfo(CultureInfo cultureInfo)
	{
	    int code;
	    if (!CodesByCultureId.TryGetValue(cultureInfo.LCID, out code))
		throw new ArgumentOutOfRangeException(
		    "cultureInfo", cultureInfo,
		    "Invalid CultureInfo value");
	    return new Currency(code);
	}

	public override int GetHashCode()
	{
	    return _code.GetHashCode();
	}

	public override bool Equals(object right)
	{
	    if (ReferenceEquals(right, null))
		return false;

	    if (GetType() != right.GetType())
		return false;

	    return Equals((Currency)right);
	}

	public bool Equals(Currency other)
	{
	    return _code == other._code;
	}

	public static bool operator==(Currency left, Currency right)
	{
	    return left.Equals(right);
	}

	public static bool operator!=(Currency left, Currency right)
	{
	    return !left.Equals(right);
	}

	public override string ToString()
	{
	    var d = Description;
	    return String.Format(
		"{0} ({1})",
		d.Name,
		d.Iso3LetterCode);
	}

	public static Currency FromSeto(Proto.Seto.Currency currency)
	{
	    Currency cur;
	    if (!Currencies.TryGetValue(currency, out cur))
		throw new ArgumentException("Invalid currency.");
	    return cur;
	}

	public object GetFormat(Type formatType)
	{
	    int lcid;
	    if (!CultureIdsByCode.TryGetValue(_code, out lcid))
		return null;
	    return new CultureInfo(lcid).NumberFormat;
	}
    }
}
