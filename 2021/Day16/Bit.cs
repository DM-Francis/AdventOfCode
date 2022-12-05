using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    public struct Bit : INumber<Bit>
    {
        private readonly bool _value;

        public Bit(bool value)
        {
            _value= value;
        }

        public static Bit One => new(true);

        public static int Radix => 2;

        public static Bit Zero => new(false);

        public static Bit AdditiveIdentity => new(false);

        public static Bit MultiplicativeIdentity => new(true);

        public static Bit Abs(Bit value) => value;
        public static bool IsCanonical(Bit value) => true;
        public static bool IsComplexNumber(Bit value) => false;
        public static bool IsEvenInteger(Bit value) => value == Zero;
        public static bool IsFinite(Bit value) => true;
        public static bool IsImaginaryNumber(Bit value) => false;
        public static bool IsInfinity(Bit value) => false;
        public static bool IsInteger(Bit value) => true;
        public static bool IsNaN(Bit value) => false;
        public static bool IsNegative(Bit value) => false;
        public static bool IsNegativeInfinity(Bit value) => false;
        public static bool IsNormal(Bit value) => value == One;
        public static bool IsOddInteger(Bit value) => value == One;
        public static bool IsPositive(Bit value) => value == One;
        public static bool IsPositiveInfinity(Bit value) => false;
        public static bool IsRealNumber(Bit value) => true;
        public static bool IsSubnormal(Bit value) => false;
        public static bool IsZero(Bit value) => value == Zero;
        public static Bit MaxMagnitude(Bit x, Bit y) => new(x._value || y._value);
        public static Bit MaxMagnitudeNumber(Bit x, Bit y) => MaxMagnitude(x, y);
        public static Bit MinMagnitude(Bit x, Bit y) => new(x._value && y._value);
        public static Bit MinMagnitudeNumber(Bit x, Bit y) => MinMagnitude(x, y);
        public static Bit Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => throw new NotImplementedException();
        public static Bit Parse(string s, NumberStyles style, IFormatProvider? provider) => throw new NotImplementedException();
        public static Bit Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => throw new NotImplementedException();
        public static Bit Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Bit result) => throw new NotImplementedException();
        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Bit result) => throw new NotImplementedException();
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Bit result) => throw new NotImplementedException();
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Bit result) => throw new NotImplementedException();
        public int CompareTo(object? obj) => throw new NotImplementedException();
        public int CompareTo(Bit other) => throw new NotImplementedException();
        public bool Equals(Bit other) => other._value == _value;
        public override int GetHashCode() => HashCode.Combine(_value);
        public string ToString(string? format, IFormatProvider? formatProvider) => throw new NotImplementedException();
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotImplementedException();

        public static Bit operator +(Bit value) => value;

        public static Bit operator +(Bit left, Bit right) => new(left._value ^ right._value);

        public static Bit operator -(Bit value) => value;

        public static Bit operator -(Bit left, Bit right) => left + right;

        public static Bit operator ++(Bit value) => value + One;

        public static Bit operator --(Bit value) => value - One;

        public static Bit operator *(Bit left, Bit right) => new(left._value && right._value);

        public static Bit operator /(Bit left, Bit right)
        {
            throw new NotImplementedException();
        }

        public static Bit operator %(Bit left, Bit right)
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(Bit left, Bit right)
        {
            throw new NotImplementedException();
        }

        public static bool operator !=(Bit left, Bit right)
        {
            throw new NotImplementedException();
        }

        public static bool operator <(Bit left, Bit right)
        {
            throw new NotImplementedException();
        }

        public static bool operator >(Bit left, Bit right)
        {
            throw new NotImplementedException();
        }

        public static bool operator <=(Bit left, Bit right)
        {
            throw new NotImplementedException();
        }

        public static bool operator >=(Bit left, Bit right)
        {
            throw new NotImplementedException();
        }
    }
}
