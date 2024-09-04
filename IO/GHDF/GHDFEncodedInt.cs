using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHEngine.IO.GHDF;

public readonly struct GHDFEncodedInt
{
    // Fields.
    public readonly long Value { get; }


    // Constructors.
    public GHDFEncodedInt(long value)
    {
        Value = value;
    }


    // Inherited methods.
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is GHDFEncodedInt EncodedInt)
        {
            return EncodedInt.Value == Value;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode() * 938 + ((int)Value & 0xDEAFBEF);
    }

    public override string ToString()
    {
        return Value.ToString();
    }


    // Static methods.
    public static implicit operator GHDFEncodedInt(long value) => new(value);

    public static implicit operator GHDFEncodedInt(ulong value) => new((long)value);

    public static implicit operator GHDFEncodedInt(int value) => new(value);

    public static implicit operator GHDFEncodedInt(uint value) => new(value);

    public static implicit operator GHDFEncodedInt(short value) => new(value);

    public static implicit operator GHDFEncodedInt(ushort value) => new(value);

    public static implicit operator GHDFEncodedInt(byte value) => new(value);

    public static implicit operator GHDFEncodedInt(sbyte value) => new(value);

    public static explicit operator long(GHDFEncodedInt value) => value.Value;

    public static explicit operator ulong(GHDFEncodedInt value) => (ulong)value.Value;

    public static explicit operator int(GHDFEncodedInt value) => (int)value.Value;

    public static explicit operator uint(GHDFEncodedInt value) => (uint)value.Value;

    public static explicit operator short(GHDFEncodedInt value) => (short)value.Value;

    public static explicit operator ushort(GHDFEncodedInt value) => (ushort)value.Value;

    public static explicit operator byte(GHDFEncodedInt value) => (byte)value.Value;

    public static explicit operator sbyte(GHDFEncodedInt value) => (sbyte)value.Value;

    public static explicit operator float(GHDFEncodedInt value) => (float)value.Value;

    public static explicit operator double(GHDFEncodedInt value) => (double)value.Value;

    public static GHDFEncodedInt operator +(GHDFEncodedInt a, GHDFEncodedInt b) => new(a.Value + b.Value);

    public static GHDFEncodedInt operator -(GHDFEncodedInt a, GHDFEncodedInt b) => new(a.Value - b.Value);
    
    public static GHDFEncodedInt operator *(GHDFEncodedInt a, GHDFEncodedInt b) => new(a.Value * b.Value);

    public static GHDFEncodedInt operator /(GHDFEncodedInt a, GHDFEncodedInt b) => new(a.Value / b.Value);

    public static GHDFEncodedInt operator ++(GHDFEncodedInt value) => new(value.Value + 1);

    public static GHDFEncodedInt operator --(GHDFEncodedInt value) => new(value.Value - 1);

    public static GHDFEncodedInt operator %(GHDFEncodedInt a, GHDFEncodedInt b) => new(a.Value % b.Value);

    public static GHDFEncodedInt operator &(GHDFEncodedInt a, GHDFEncodedInt b) => new(a.Value & b.Value);

    public static GHDFEncodedInt operator |(GHDFEncodedInt a, GHDFEncodedInt b) => new(a.Value | b.Value);
    
    public static GHDFEncodedInt operator ^(GHDFEncodedInt a, GHDFEncodedInt b) => new(a.Value ^ b.Value);

    public static GHDFEncodedInt operator >>(GHDFEncodedInt value, int amount) => new(value.Value >> amount);

    public static GHDFEncodedInt operator <<(GHDFEncodedInt value, int amount) => new(value.Value << amount);

    public static GHDFEncodedInt operator >>>(GHDFEncodedInt value, int amount) => new(value.Value >>> amount);

    public static bool operator ==(GHDFEncodedInt a, GHDFEncodedInt b) => a.Value == (int)b.Value;

    public static bool operator !=(GHDFEncodedInt a, GHDFEncodedInt b) => a.Value != (int)b.Value;

    public static bool operator >(GHDFEncodedInt a, GHDFEncodedInt b) => a.Value > b.Value;

    public static bool operator <(GHDFEncodedInt a, GHDFEncodedInt b) => a.Value < b.Value;

    public static bool operator >=(GHDFEncodedInt a, GHDFEncodedInt b) => a.Value >= b.Value;

    public static bool operator <=(GHDFEncodedInt a, GHDFEncodedInt b) => a.Value <= b.Value;
}