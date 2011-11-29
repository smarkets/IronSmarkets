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

namespace IronSmarkets.Data
{
    public struct Money : IEquatable<Money>, IComparable<Money>, IFormattable, IConvertible
    {
	private readonly Currency _currency;
	private readonly decimal _units;

	public Currency Currency { get { return _currency; } }
	public decimal Units { get { return _units; } }

	public Money(decimal units) : this(units, Currency.FromCurrentCulture())
	{
	}

	public Money(decimal units, Currency currency)
	{
	    AssertValidValue(units);
	    _units = units;
	    _currency = currency;
	}

	public int CompareTo(Money other)
	{
	    AssertSameCurrencies(this, other);
	    return _units.CompareTo(other._units);
	}

	public override string ToString()
	{
	    return ToString("C");
	}

	private string ToString(string format)
	{
	    return ToString(format, _currency);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
	    return _units.ToString(format, formatProvider);
	}

	public static Money operator -(Money value)
	{
	    return new Money(-value._units, value._currency);
	}

	public static Money operator +(Money left, Money right)
	{
	    AssertSameCurrencies(left, right);
	    return new Money(left._units + right._units, left._currency);
	}

	public static Money operator -(Money left, Money right)
	{
	    AssertSameCurrencies(left, right);
	    return new Money(left._units - right._units, left._currency);
	}

	public static decimal operator *(Money left, decimal right)
	{
	    return left._units * right;
	}

	public static decimal operator /(Money left, decimal right)
	{
	    return left._units / right;
	}

	public static bool operator >(Money left, Money right)
	{
	    return left.CompareTo(right) > 0;
	}

	public static bool operator <(Money left, Money right)
	{
	    return left.CompareTo(right) < 0;
	}

	public static bool operator >=(Money left, Money right)
	{
	    return left.CompareTo(right) >= 0;
	}

	public static bool operator <=(Money left, Money right)
	{
	    return left.CompareTo(right) <= 0;
	}

	public override int GetHashCode()
	{
	    return _units.GetHashCode()
		^ _currency.GetHashCode();
	}

	public override bool Equals(object right)
	{
	    if (ReferenceEquals(right, null))
		return false;

	    if (GetType() != right.GetType())
		return false;

	    return Equals((Money)right);
	}

	public bool Equals(Money other)
	{
	    return _units == other._units
		&& _currency == other._currency;
	}

	public static bool operator==(Money left, Money right)
	{
	    return left.Equals(right);
	}

	public static bool operator!=(Money left, Money right)
	{
	    return !left.Equals(right);
	}

	private static void AssertValidValue(decimal value)
	{
	    if (value < Int64.MinValue || value > Int64.MaxValue)
		throw new ArgumentOutOfRangeException(
		    "value", value,
		    string.Format(
			"Money value must be between {0} and {1}",
			Int64.MinValue, Int64.MaxValue));
	}

	private static void AssertSameCurrencies(Money left, Money right)
	{
	    if (left._currency != right._currency)
		throw new InvalidOperationException(
		    "Only Money values with the same currency " +
		    "may be compared or otherwise operated on.");
	}

	public TypeCode GetTypeCode()
	{
	    return TypeCode.Object;
	}

	public bool ToBoolean(IFormatProvider provider)
	{
	    return _units != 0;
	}

	public char ToChar(IFormatProvider provider)
	{
	    throw new NotSupportedException();
	}

	public sbyte ToSByte(IFormatProvider provider)
	{
	    return (sbyte)_units;
	}

	public byte ToByte(IFormatProvider provider)
	{
	    return (byte)_units;
	}

	public short ToInt16(IFormatProvider provider)
	{
	    return (short)_units;
	}

	public ushort ToUInt16(IFormatProvider provider)
	{
	    return (ushort)_units;
	}

	public int ToInt32(IFormatProvider provider)
	{
	    return (int)_units;
	}

	public uint ToUInt32(IFormatProvider provider)
	{
	    return (uint)_units;
	}

	public long ToInt64(IFormatProvider provider)
	{
	    return (long)_units;
	}

	public ulong ToUInt64(IFormatProvider provider)
	{
	    return (ulong)_units;
	}

	public float ToSingle(IFormatProvider provider)
	{
	    return (float)_units;
	}

	public double ToDouble(IFormatProvider provider)
	{
	    return (double)_units;
	}

	public decimal ToDecimal(IFormatProvider provider)
	{
	    return _units;
	}

	public DateTime ToDateTime(IFormatProvider provider)
	{
	    throw new NotSupportedException();
	}

	string IConvertible.ToString(IFormatProvider provider)
	{
	    return ToString("C", provider);
	}

	public object ToType(Type conversionType, IFormatProvider provider)
	{
	    throw new NotSupportedException();
	}
    }
}
