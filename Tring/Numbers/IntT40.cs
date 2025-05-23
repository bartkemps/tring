// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable IntVariableOverflowInUncheckedContext
// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Tring.Numbers;

using System.Globalization;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents a ternary number, modeled after the <see cref="long"/> type.
/// </summary>
public readonly partial struct IntT40 :
    IConvertible,
    IBinaryInteger<IntT40>,
    ISignedNumber<IntT40>,
    ITernaryNumber<IntT40, long>
{
    private readonly long value;

    /// <summary>
    ///  Represents the maximum value of a <see cref="IntT40"/>, expressed as a <see cref="long"/> This field is constant.
    /// </summary>
    public const long MaxValueConstant = 6078832729528464400;

    /// <summary>
    /// Represents the minimum value of a <see cref="IntT40"/>, expressed as a <see cref="long"/> This field is constant.
    /// </summary>
    public const long MinValueConstant = -6078832729528464400;

    private const long Wrap = 6289078614652622813;

    /// <summary>
    /// Represents the largest possible value of a <see cref="IntT40"/>. This field is constant.
    /// </summary>
    public static IntT40 MaxValue => new(MaxValueConstant);

    /// <summary>
    /// Represents the smallest possible value of a <see cref="IntT40"/>. This field is constant.
    /// </summary>
    public static IntT40 MinValue => new(MinValueConstant);

    private IntT40(long value) =>
        this.value = value switch
        {
            > MaxValueConstant =>(long)(value + Wrap),
            < MinValueConstant => (long)(value - Wrap),
            _ => value
        };

                      

    /// <summary>
    /// Defines an implicit conversion of a long to a <see cref="IntT40"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A <see cref="IntT40"/> that represents the converted value.</returns>
    public static implicit operator IntT40(long value) => new(value);

    /// <summary>
    /// Defines an implicit conversion of a <see cref="IntT40"/> to a long.
    /// </summary>
    /// <param name="value">The <see cref="IntT40"/> to convert.</param>
    /// <returns>A long that represents the converted <see cref="IntT40"/>.</returns>
    public static implicit operator long(IntT40 value) => value.value;

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified object.
    /// </summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="obj"/> is an instance of <see cref="IntT40"/> or a compatible numeric type
    /// and equals the value of this instance; otherwise, <see langword="false"/>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        switch (obj)
        {
            case null: return false;
            case IntT40 other: return value == other.value;
            case int typed: return value == typed;
            case long int64: return value == int64;
            case short int16: return value == int16;
            case byte byteVal: return value == byteVal;
            case sbyte sbyteVal: return value == sbyteVal;
            case uint uint32: return value == uint32;
            case ulong uint64: return value == (long)uint64;
            case ushort uint16: return value == uint16;
            case float singleValue: return singleValue.Equals(value);
            case double doubleValue: return doubleValue.Equals(value);
            case decimal decimalValue: return decimalValue.Equals(value);
            case char charVal: return value == charVal;
            case IConvertible conv:
                try
                {
                    // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                    switch (conv.GetTypeCode())
                    {
                        case TypeCode.Int16: return value == conv.ToInt16(null);
                        case TypeCode.UInt16: return value == conv.ToUInt16(null);
                        case TypeCode.Int32: return value == conv.ToInt32(null);
                        case TypeCode.UInt32: return value == conv.ToUInt32(null);
                        case TypeCode.Int64: return value == conv.ToInt64(null);
                        case TypeCode.UInt64: return value == (long)conv.ToUInt64(null);
                        case TypeCode.Byte: return value == conv.ToByte(null);
                        case TypeCode.SByte: return value == conv.ToSByte(null);
                        case TypeCode.Single: return conv.ToSingle(null).Equals(value);
                        case TypeCode.Double: return conv.ToDouble(null).Equals(value);
                        case TypeCode.Decimal: return conv.ToDecimal(null).Equals(value);
                    }
                }
                catch
                {
                    return false;
                }

                break;
        }

        return obj.Equals(value);
    }

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified <see cref="IntT40"/> value.
    /// </summary>
    /// <param name="other">A <see cref="IntT40"/> value to compare to this instance.</param>
    /// <returns><see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.</returns>
    public bool Equals(IntT40 other) => value == other.value;

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified <see cref="long"/> value.
    /// </summary>
    /// <param name="other">A <see cref="long"/> value to compare to this instance.</param>
    /// <returns><see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.</returns>
    public bool Equals(long other) => value == other;

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => value.GetHashCode();

    #region Equality Operators

    /// <summary>
    /// Returns a value indicating whether two <see cref="IntT40"/> instances are equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(IConvertible left, IntT40 right) => right.Equals(left);

    /// <summary>
    /// Returns a value indicating whether two <see cref="IntT40"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(IConvertible left, IntT40 right) => !right.Equals(left);

    /// <summary>
    /// Returns a value indicating whether two <see cref="IntT40"/> instances are equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(IntT40 left, IConvertible right) => left.Equals(right);

    /// <summary>
    /// Returns a value indicating whether two <see cref="IntT40"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(IntT40 left, IConvertible right) => !left.Equals(right);

    /// <summary>
    /// Returns a value indicating whether two <see cref="IntT40"/> instances are equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(IntT40 left, IntT40 right) => left.value == right.value;

    /// <summary>
    /// Returns a value indicating whether two <see cref="IntT40"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(IntT40 left, IntT40 right) => left.value != right.value;

    #endregion

    #region Arithmetic Operators

    /// <summary>
    /// Adds two <see cref="IntT40"/> values and returns the result.
    /// </summary>
    /// <param name="left">The first value to add.</param>
    /// <param name="right">The second value to add.</param>
    /// <returns>The sum of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static IntT40 operator +(IntT40 left, IntT40 right) => new(left.value + right.value);

    /// <summary>
    /// Subtracts one <see cref="IntT40"/> value from another and returns the result.
    /// </summary>
    /// <param name="left">The value to subtract from (the minuend).</param>
    /// <param name="right">The value to subtract (the subtrahend).</param>
    /// <returns>The result of subtracting <paramref name="right"/> from <paramref name="left"/>.</returns>
    public static IntT40 operator -(IntT40 left, IntT40 right) => new(left.value - right.value);

    /// <summary>
    /// Multiplies two <see cref="IntT40"/> values and returns the result.
    /// </summary>
    /// <param name="left">The first value to multiply.</param>
    /// <param name="right">The second value to multiply.</param>
    /// <returns>The product of <paramref name="left"/> and <paramref name="right"/>.</returns>
    public static IntT40 operator *(IntT40 left, IntT40 right) => new(left.value * right.value);

    /// <summary>
    /// Divides one <see cref="IntT40"/> value by another and returns the result.
    /// </summary>
    /// <param name="left">The value to be divided (the dividend).</param>
    /// <param name="right">The value to divide by (the divisor).</param>
    /// <returns>The result of dividing <paramref name="left"/> by <paramref name="right"/>.</returns>
    /// <exception cref="DivideByZeroException"><paramref name="right"/> is zero.</exception>
    public static IntT40 operator /(IntT40 left, IntT40 right) => new(left.value / right.value);

    /// <summary>
    /// Returns the remainder that results from dividing one <see cref="IntT40"/> value by another.
    /// </summary>
    /// <param name="left">The value to be divided (the dividend).</param>
    /// <param name="right">The value to divide by (the divisor).</param>
    /// <returns>The remainder that results from dividing <paramref name="left"/> by <paramref name="right"/>.</returns>
    /// <exception cref="DivideByZeroException"><paramref name="right"/> is zero.</exception>
    public static IntT40 operator %(IntT40 left, IntT40 right) => new(left.value % right.value);

    /// <summary>
    /// Negates the specified <see cref="IntT40"/> value.
    /// </summary>
    /// <param name="value">The value to negate.</param>
    /// <returns>The result of the value multiplied by negative one (-1).</returns>
    public static IntT40 operator -(IntT40 value) => new(-value.value);

    /// <summary>
    /// Returns the specified <see cref="IntT40"/> value; the sign of the value is unchanged.
    /// </summary>
    /// <param name="value">The value to return.</param>
    /// <returns>The value of the <paramref name="value"/> parameter.</returns>
    public static IntT40 operator +(IntT40 value) => value;

    // Mixed-type arithmetic operators with long
    public static IntT40 operator +(IntT40 left, long right) => new(left.value + right);
    public static IntT40 operator +(long left, IntT40 right) => new(left + right.value);

    public static IntT40 operator -(IntT40 left, long right) => new(left.value - right);
    public static IntT40 operator -(long left, IntT40 right) => new(left - right.value);

    public static IntT40 operator *(IntT40 left, long right) => new(left.value * right);
    public static IntT40 operator *(long left, IntT40 right) => new(left * right.value);

    public static IntT40 operator /(IntT40 left, long right) => new(left.value / right);
    public static IntT40 operator /(long left, IntT40 right) => new(left / right.value);

    public static IntT40 operator %(IntT40 left, long right) => new(left.value % right);
    public static IntT40 operator %(long left, IntT40 right) => new(left % right.value);
    #endregion
    
    #region Comparison Operators

    // Comparison operators with IntT40
    public static bool operator >(IntT40 left, IntT40 right) => left.value > right.value;
    public static bool operator <(IntT40 left, IntT40 right) => left.value < right.value;
    public static bool operator >=(IntT40 left, IntT40 right) => left.value >= right.value;
    public static bool operator <=(IntT40 left, IntT40 right) => left.value <= right.value;
    
    // Comparison operators with sbyte
    public static bool operator >(IntT40 left, sbyte right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT40 left, sbyte right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT40 left, sbyte right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT40 left, sbyte right) => left.CompareTo(right) <= 0;

    public static bool operator >(sbyte left, IntT40 right) => right.CompareTo(left) < 0;
    public static bool operator <(sbyte left, IntT40 right) => right.CompareTo(left) > 0;
    public static bool operator >=(sbyte left, IntT40 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(sbyte left, IntT40 right) => right.CompareTo(left) >= 0;
    
    // Comparison operators with byte
    public static bool operator >(IntT40 left, byte right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT40 left, byte right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT40 left, byte right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT40 left, byte right) => left.CompareTo(right) <= 0;

    public static bool operator >(byte left, IntT40 right) => right.CompareTo(left) < 0;
    public static bool operator <(byte left, IntT40 right) => right.CompareTo(left) > 0;
    public static bool operator >=(byte left, IntT40 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(byte left, IntT40 right) => right.CompareTo(left) >= 0;
    
    // Comparison operators with int
    public static bool operator >(IntT40 left, int right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT40 left, int right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT40 left, int right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT40 left, int right) => left.CompareTo(right) <= 0;

    public static bool operator >(int left, IntT40 right) => right.CompareTo(left) < 0;
    public static bool operator <(int left, IntT40 right) => right.CompareTo(left) > 0;
    public static bool operator >=(int left, IntT40 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(int left, IntT40 right) => right.CompareTo(left) >= 0;
    
    // Comparison operators with uint
    public static bool operator >(IntT40 left, uint right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT40 left, uint right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT40 left, uint right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT40 left, uint right) => left.CompareTo(right) <= 0;

    public static bool operator >(uint left, IntT40 right) => right.CompareTo(left) < 0;
    public static bool operator <(uint left, IntT40 right) => right.CompareTo(left) > 0;
    public static bool operator >=(uint left, IntT40 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(uint left, IntT40 right) => right.CompareTo(left) >= 0;
    
    // Comparison operators with short
    public static bool operator >(IntT40 left, short right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT40 left, short right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT40 left, short right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT40 left, short right) => left.CompareTo(right) <= 0;

    public static bool operator >(short left, IntT40 right) => right.CompareTo(left) < 0;
    public static bool operator <(short left, IntT40 right) => right.CompareTo(left) > 0;
    public static bool operator >=(short left, IntT40 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(short left, IntT40 right) => right.CompareTo(left) >= 0;
    
    // Comparison operators with ushort
    public static bool operator >(IntT40 left, ushort right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT40 left, ushort right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT40 left, ushort right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT40 left, ushort right) => left.CompareTo(right) <= 0;

    public static bool operator >(ushort left, IntT40 right) => right.CompareTo(left) < 0;
    public static bool operator <(ushort left, IntT40 right) => right.CompareTo(left) > 0;
    public static bool operator >=(ushort left, IntT40 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(ushort left, IntT40 right) => right.CompareTo(left) >= 0;
    
    // Comparison operators with long
    public static bool operator >(IntT40 left, long right) => left.value > right;
    public static bool operator <(IntT40 left, long right) => left.value < right;
    public static bool operator >=(IntT40 left, long right) => left.value >= right;
    public static bool operator <=(IntT40 left, long right) => left.value <= right;

    public static bool operator >(long left, IntT40 right) => left > right.value;
    public static bool operator <(long left, IntT40 right) => left < right.value;
    public static bool operator >=(long left, IntT40 right) => left >= right.value;
    public static bool operator <=(long left, IntT40 right) => left <= right.value;
    
    // Comparison operators with ulong
    public static bool operator >(IntT40 left, ulong right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT40 left, ulong right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT40 left, ulong right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT40 left, ulong right) => left.CompareTo(right) <= 0;

    public static bool operator >(ulong left, IntT40 right) => right.CompareTo(left) < 0;
    public static bool operator <(ulong left, IntT40 right) => right.CompareTo(left) > 0;
    public static bool operator >=(ulong left, IntT40 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(ulong left, IntT40 right) => right.CompareTo(left) >= 0;
     
    // Comparison operators with float
    public static bool operator >(IntT40 left, float right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT40 left, float right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT40 left, float right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT40 left, float right) => left.CompareTo(right) <= 0;

    public static bool operator >(float left, IntT40 right) => right.CompareTo(left) < 0;
    public static bool operator <(float left, IntT40 right) => right.CompareTo(left) > 0;
    public static bool operator >=(float left, IntT40 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(float left, IntT40 right) => right.CompareTo(left) >= 0;
     
    // Comparison operators with double
    public static bool operator >(IntT40 left, double right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT40 left, double right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT40 left, double right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT40 left, double right) => left.CompareTo(right) <= 0;

    public static bool operator >(double left, IntT40 right) => right.CompareTo(left) < 0;
    public static bool operator <(double left, IntT40 right) => right.CompareTo(left) > 0;
    public static bool operator >=(double left, IntT40 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(double left, IntT40 right) => right.CompareTo(left) >= 0;
     
    // Comparison operators with decimal
    public static bool operator >(IntT40 left, decimal right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT40 left, decimal right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT40 left, decimal right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT40 left, decimal right) => left.CompareTo(right) <= 0;

    public static bool operator >(decimal left, IntT40 right) => right.CompareTo(left) < 0;
    public static bool operator <(decimal left, IntT40 right) => right.CompareTo(left) > 0;
    public static bool operator >=(decimal left, IntT40 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(decimal left, IntT40 right) => right.CompareTo(left) >= 0;
     
    // Comparison operators with IComparable
    public static bool operator >(IntT40 left, IComparable right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT40 left, IComparable right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT40 left, IComparable right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT40 left, IComparable right) => left.CompareTo(right) <= 0;

    public static bool operator >(IComparable left, IntT40 right) => right.CompareTo(left) < 0;
    public static bool operator <(IComparable left, IntT40 right) => right.CompareTo(left) > 0;
    public static bool operator >=(IComparable left, IntT40 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(IComparable left, IntT40 right) => right.CompareTo(left) >= 0;

    #endregion

    // ToString implementation
    public override string ToString() => value.ToString();
    public string ToString(string? format) => value.ToString(format);
    public string ToString(IFormatProvider? provider) => value.ToString(provider);
    public string ToString(string? format, IFormatProvider? provider) => value.ToString(format, provider);

    // Parsing methods

    /// <summary>
    /// Converts the string representation of a number to its <see cref="IntT40"/> equivalent.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <returns>A <see cref="IntT40"/> equivalent to the number contained in <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not in the correct format.</exception>
    /// <exception cref="OverflowException"><paramref name="s"/> represents a number less than <see cref="IntT40.MinValue"/> or greater than <see cref="IntT40.MaxValue"/>.</exception>
    public static IntT40 Parse(string s) => new(long.Parse(s));

    /// <summary>
    /// Converts the string representation of a number in a specified style to its <see cref="IntT40"/> equivalent.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicate the style elements that can be present in <paramref name="s"/>.</param>
    /// <returns>A <see cref="IntT40"/> equivalent to the number contained in <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="style"/> is not a <see cref="NumberStyles"/> value or <paramref name="style"/> includes the <see cref="NumberStyles.AllowHexSpecifier"/> value.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a format compliant with <paramref name="style"/>.</exception>
    /// <exception cref="OverflowException"><paramref name="s"/> represents a number less than <see cref="IntT40.MinValue"/> or greater than <see cref="IntT40.MaxValue"/>.</exception>
    public static IntT40 Parse(string s, NumberStyles style) => new(long.Parse(s, style));

    /// <summary>
    /// Tries to convert the string representation of a number to its <see cref="IntT40"/> equivalent, and returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="IntT40"/> value equivalent to the number contained in <paramref name="s"/> if the conversion succeeded, or zero if the conversion failed. This parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, out IntT40 result)
    {
        bool success = long.TryParse(s, out long value);
        result = new IntT40(value);
        return success;
    }

    /// <summary>
    /// Compares this instance to a specified object and returns an indication of their relative values.
    /// </summary>
    /// <param name="obj">An object to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// A signed number indicating the relative values of this instance and <paramref name="obj"/>.
    /// Return Value Description:
    /// Less than zero: This instance is less than <paramref name="obj"/>.
    /// Zero: This instance is equal to <paramref name="obj"/>.
    /// Greater than zero: This instance is greater than <paramref name="obj"/> or <paramref name="obj"/> is <see langword="null"/>.
    /// </returns>
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not a <see cref="IntT40"/> or a type that can be converted to an integer.</exception>
    public int CompareTo(object? obj)
    {
        if (obj == null) return 1;
        if (obj is IntT40 other) return CompareTo(other);

        // For large numeric types that exceed IntT40's range, return -1 if greater than MaxValue, 1 if less than MinValue
        try
        {
            if (obj is long typed) return value.CompareTo(typed);
            if (obj is long int64)
            {
                return int64 switch
                {
                    > MaxValueConstant => -1,
                    < MinValueConstant => 1,
                    _ => value.CompareTo((long)int64)
                };
            }

            if (obj is uint uint32)
            {
                return uint32 > MaxValueConstant ? -1 : value.CompareTo((long)uint32);
            }

            if (obj is ulong uint64)
            {
                return uint64 > MaxValueConstant ? -1 : value.CompareTo((long)uint64);
            }

            if (obj is short int16) return value.CompareTo(int16);
            if (obj is ushort uint16) return value.CompareTo(uint16);
            if (obj is byte byteVal) return value.CompareTo(byteVal);
            if (obj is sbyte sbyteVal) return value.CompareTo(sbyteVal);
            if (obj is float singleValue)
            {
                return singleValue switch
                {
                    > MaxValueConstant => -1,
                    < MinValueConstant => 1,
                    _ => value.CompareTo((long)singleValue)
                };
            }

            if (obj is double doubleValue)
            {
                return doubleValue switch
                {
                    > MaxValueConstant => -1,
                    < MinValueConstant => 1,
                    _ => value.CompareTo((long)doubleValue)
                };
            }

            if (obj is decimal decimalValue)
            {
                return decimalValue switch
                {
                    > MaxValueConstant => -1,
                    < MinValueConstant => 1,
                    _ => value.CompareTo((long)decimalValue)
                };
            }

            if (obj is IConvertible convertible)
            {
                switch (convertible.GetTypeCode())
                {
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Byte:
                    case TypeCode.SByte:
                        return value.CompareTo(convertible.ToInt32(null));
                    case TypeCode.Int32:
                        return value.CompareTo(convertible.ToInt32(null));
                    case TypeCode.UInt32:
                        var uint32Value = convertible.ToUInt32(null);
                        if (uint32Value > MaxValueConstant) return -1;
                        return value.CompareTo((long)uint32Value);
                    case TypeCode.Int64:
                        var int64Value = convertible.ToInt64(null);
                        if (int64Value > MaxValueConstant) return -1;
                        if (int64Value < MinValueConstant) return 1;
                        return value.CompareTo((long)(int64Value));
                    case TypeCode.UInt64:
                        var uint64Value = convertible.ToUInt64(null);
                        if (uint64Value > MaxValueConstant) return -1;
                        return value.CompareTo((long)uint64Value);
                    case TypeCode.Single:
                        var singleVal = convertible.ToSingle(null);
                        if (singleVal > MaxValueConstant) return -1;
                        if (singleVal < MinValueConstant) return 1;
                        return value.CompareTo((long)singleVal);
                    case TypeCode.Double:
                        var doubleVal = convertible.ToDouble(null);
                        if (doubleVal > MaxValueConstant) return -1;
                        if (doubleVal < MinValueConstant) return 1;
                        return value.CompareTo((long)doubleVal);
                    case TypeCode.Decimal:
                        var decimalVal = convertible.ToDecimal(null);
                        if (decimalVal > MaxValueConstant) return -1;
                        if (decimalVal < MinValueConstant) return 1;
                        return value.CompareTo((long)decimalVal);
                }
            }
        }
        catch (OverflowException)
        {
            // If conversion fails due to overflow, we can assume the value is outside our range
            return -1;
        }

        throw new ArgumentException("Object is not a valid type for comparison", nameof(obj));
    }

    /// <summary>
    /// Compares this instance to a specified <see cref="IntT40"/> object and returns an indication of their relative values.
    /// </summary>
    /// <param name="other">An <see cref="IntT40"/> object to compare.</param>
    /// <returns>
    /// A signed number indicating the relative values of this instance and <paramref name="other"/>.
    /// Return Value Description:
    /// Less than zero: This instance is less than <paramref name="other"/>.
    /// Zero: This instance is equal to <paramref name="other"/>.
    /// Greater than zero: This instance is greater than <paramref name="other"/>.
    /// </returns>
    public int CompareTo(IntT40 other) => value.CompareTo(other.value);

    #region IConvertible Implementation

    public TypeCode GetTypeCode() => TypeCode.Int32;

    bool IConvertible.ToBoolean(IFormatProvider? provider) => value != 0;
    char IConvertible.ToChar(IFormatProvider? provider) => Convert.ToChar(value);
    sbyte IConvertible.ToSByte(IFormatProvider? provider) => Convert.ToSByte(value);
    byte IConvertible.ToByte(IFormatProvider? provider) => Convert.ToByte(value);
    short IConvertible.ToInt16(IFormatProvider? provider) => Convert.ToInt16(value);
    ushort IConvertible.ToUInt16(IFormatProvider? provider) => Convert.ToUInt16(value);
    int IConvertible.ToInt32(IFormatProvider? provider) => Convert.ToInt32(value);
    uint IConvertible.ToUInt32(IFormatProvider? provider) => Convert.ToUInt32(value);
    long IConvertible.ToInt64(IFormatProvider? provider) => Convert.ToInt64(value);
    ulong IConvertible.ToUInt64(IFormatProvider? provider) => Convert.ToUInt64(value);
    float IConvertible.ToSingle(IFormatProvider? provider) => Convert.ToSingle(value);
    double IConvertible.ToDouble(IFormatProvider? provider) => Convert.ToDouble(value);
    decimal IConvertible.ToDecimal(IFormatProvider? provider) => Convert.ToDecimal(value);
    DateTime IConvertible.ToDateTime(IFormatProvider? provider) => throw new InvalidCastException();
    string IConvertible.ToString(IFormatProvider? provider) => value.ToString(provider);

    object IConvertible.ToType(Type conversionType, IFormatProvider? provider) =>
        Convert.ChangeType(value, conversionType, provider);

    #endregion

    #region IUtf8SpanFormattable Implementation

    bool IUtf8SpanFormattable.TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (!value.TryFormat(stackalloc char[32], out int charsWritten, format, provider))
        {
            bytesWritten = 0;
            return false;
        }

        // Get UTF8 byte count for the character length
        var byteCount = System.Text.Encoding.UTF8.GetMaxByteCount(charsWritten);

        if (utf8Destination.Length < byteCount)
        {
            bytesWritten = 0;
            return false;
        }

        // Convert to string since we can't go directly from ReadOnlySpan<char> to UTF8
        var str = value.ToString(format.ToString(), provider);
        bytesWritten = System.Text.Encoding.UTF8.GetBytes(str, utf8Destination);
        return true;
    }

    #endregion

    #region IIncrementOperators/IDecrementOperators Implementation

    public static IntT40 operator ++(IntT40 value) => new(value.value + 1);
    public static IntT40 operator --(IntT40 value) => new(value.value - 1);

    #endregion

    static bool IEqualityOperators<IntT40, IntT40, bool>.operator ==(IntT40 left, IntT40 right) => left.value == right.value;
    static bool IEqualityOperators<IntT40, IntT40, bool>.operator !=(IntT40 left, IntT40 right) => left.value != right.value;

    #region Binary Operations

    public static IntT40 RotateLeft(IntT40 value, int rotateAmount) =>
        new((long)BitOperations.RotateLeft((ulong)value.value, rotateAmount));

    #endregion

    #region Interface Static Members

    static IntT40 ISignedNumber<IntT40>.NegativeOne => new(-1);
    static IntT40 INumberBase<IntT40>.One => new(1);
    static IntT40 INumberBase<IntT40>.Zero => new(0);
    static IntT40 IAdditiveIdentity<IntT40, IntT40>.AdditiveIdentity => new(0);
    static IntT40 IMultiplicativeIdentity<IntT40, IntT40>.MultiplicativeIdentity => new(1);

    static bool INumberBase<IntT40>.IsCanonical(IntT40 value) => true;
    static bool INumberBase<IntT40>.IsComplexNumber(IntT40 value) => false;
    static bool INumberBase<IntT40>.IsEvenInteger(IntT40 value) => value.value % 2 == 0;
    static bool INumberBase<IntT40>.IsFinite(IntT40 value) => true;
    static bool INumberBase<IntT40>.IsImaginaryNumber(IntT40 value) => false;
    static bool INumberBase<IntT40>.IsInfinity(IntT40 value) => false;
    static bool INumberBase<IntT40>.IsInteger(IntT40 value) => true;
    static bool INumberBase<IntT40>.IsNaN(IntT40 value) => false;
    static bool INumberBase<IntT40>.IsNegative(IntT40 value) => value.value < 0;
    static bool INumberBase<IntT40>.IsNegativeInfinity(IntT40 value) => false;
    static bool INumberBase<IntT40>.IsNormal(IntT40 value) => value.value != 0;
    static bool INumberBase<IntT40>.IsOddInteger(IntT40 value) => value.value % 2 != 0;
    static bool INumberBase<IntT40>.IsPositive(IntT40 value) => value.value > 0;
    static bool INumberBase<IntT40>.IsPositiveInfinity(IntT40 value) => false;
    static bool INumberBase<IntT40>.IsRealNumber(IntT40 value) => true;
    static bool INumberBase<IntT40>.IsSubnormal(IntT40 value) => false;
    static bool INumberBase<IntT40>.IsZero(IntT40 value) => value.value == 0;

    static IntT40 INumberBase<IntT40>.MaxMagnitude(IntT40 x, IntT40 y) =>
        Math.Abs(x.value) > Math.Abs(y.value) ? x : y;

    static IntT40 INumberBase<IntT40>.MaxMagnitudeNumber(IntT40 x, IntT40 y) =>
        Math.Abs(x.value) > Math.Abs(y.value) ? x : y;

    static IntT40 INumberBase<IntT40>.MinMagnitude(IntT40 x, IntT40 y) =>
        Math.Abs(x.value) < Math.Abs(y.value) ? x : y;

    static IntT40 INumberBase<IntT40>.MinMagnitudeNumber(IntT40 x, IntT40 y) =>
        Math.Abs(x.value) < Math.Abs(y.value) ? x : y;

    static int INumberBase<IntT40>.Radix => 2;

    static IntT40 INumberBase<IntT40>.Abs(IntT40 value) =>
        value.value < 0 ? new(-value.value) : value;

    static IntT40 INumberBase<IntT40>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) =>
        new(long.Parse(s, style, provider));

    static IntT40 INumberBase<IntT40>.Parse(string s, NumberStyles style, IFormatProvider? provider) =>
        new(long.Parse(s, style, provider));

    static bool INumberBase<IntT40>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out IntT40 result)
    {
        if (long.TryParse(s, style, provider, out var parsed))
        {
            result = new(parsed);
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<IntT40>.TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out IntT40 result)
    {
        if (long.TryParse(s, style, provider, out var parsed))
        {
            result = new(parsed);
            return true;
        }

        result = default;
        return false;
    }

    static bool IBinaryNumber<IntT40>.IsPow2(IntT40 value) =>
        value.value > 0 && (value.value & (value.value - 1)) == 0;

    static IntT40 IBinaryNumber<IntT40>.Log2(IntT40 value) =>
        new(BitOperations.Log2((uint)value.value));

    int IBinaryInteger<IntT40>.GetByteCount() => sizeof(long);

    int IBinaryInteger<IntT40>.GetShortestBitLength() =>
        value == 0 ? 1 : BitOperations.Log2((uint)Math.Abs(value)) + 1;

    static IntT40 IBinaryInteger<IntT40>.PopCount(IntT40 value) =>
        new(BitOperations.PopCount((uint)value.value));

    static IntT40 IBinaryInteger<IntT40>.TrailingZeroCount(IntT40 value) =>
        new(BitOperations.TrailingZeroCount((uint)value.value));

    bool IBinaryInteger<IntT40>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
    {
        if (destination.Length < sizeof(long))
        {
            bytesWritten = 0;
            return false;
        }

        var bytes = BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        bytes.CopyTo(destination);
        bytesWritten = sizeof(long);
        return true;
    }

    bool IBinaryInteger<IntT40>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
    {
        if (destination.Length < sizeof(long))
        {
            bytesWritten = 0;
            return false;
        }

        BitConverter.GetBytes(value).CopyTo(destination);
        bytesWritten = sizeof(long);
        return true;
    }

    static bool IBinaryInteger<IntT40>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out IntT40 result)
    {
        if (source.Length < sizeof(long))
        {
            result = default;
            return false;
        }

        var bytes = source.Slice(0, sizeof(long)).ToArray();
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);

        result = new(BitConverter.ToInt64(bytes));
        return true;
    }

    static bool IBinaryInteger<IntT40>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out IntT40 result)
    {
        if (source.Length < sizeof(long))
        {
            result = default;
            return false;
        }

        result = new(BitConverter.ToInt64(source));
        return true;
    }

    #region ISpanFormattable/ISpanParsable Implementation

    static IntT40 ISpanParsable<IntT40>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
        new(long.Parse(s, NumberStyles.Integer, provider));

    static bool ISpanParsable<IntT40>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out IntT40 result)
    {
        if (long.TryParse(s, NumberStyles.Integer, provider, out var value))
        {
            result = new(value);
            return true;
        }

        result = default;
        return false;
    }

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) =>
        value.TryFormat(destination, out charsWritten, format.ToString(), provider);

    static IntT40 IParsable<IntT40>.Parse(string s, IFormatProvider? provider) =>
        new(long.Parse(s, NumberStyles.Integer, provider));

    static bool IParsable<IntT40>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out IntT40 result)
    {
        if (long.TryParse(s, NumberStyles.Integer, provider, out var value))
        {
            result = new(value);
            return true;
        }

        result = default;
        return false;
    }

    #endregion

    #region Bit Operators

    static IntT40 IBitwiseOperators<IntT40, IntT40, IntT40>.operator &(IntT40 left, IntT40 right) =>
        new(left.value & right.value);

    static IntT40 IBitwiseOperators<IntT40, IntT40, IntT40>.operator |(IntT40 left, IntT40 right) =>
        new(left.value | right.value);

    static IntT40 IBitwiseOperators<IntT40, IntT40, IntT40>.operator ^(IntT40 left, IntT40 right) =>
        new(left.value ^ right.value);

    static IntT40 IBitwiseOperators<IntT40, IntT40, IntT40>.operator ~(IntT40 value) =>
        new(~value.value);

    static IntT40 IShiftOperators<IntT40, int, IntT40>.operator <<(IntT40 value, int shiftAmount) =>
        new(value.value << shiftAmount);

    static IntT40 IShiftOperators<IntT40, int, IntT40>.operator >> (IntT40 value, int shiftAmount) =>
        new(value.value >> shiftAmount);

    static IntT40 IShiftOperators<IntT40, int, IntT40>.operator >>> (IntT40 value, int shiftAmount) =>
        new(long.CreateChecked(((uint)value.value) >> shiftAmount));

    #endregion

    #endregion

    #region Generic Conversions

    static bool INumberBase<IntT40>.TryConvertFromChecked<TOther>(TOther value, out IntT40 result)
    {
        if (value is IConvertible conv)
        {
            try
            {
                var intValue = conv.ToInt64(null);
                if (intValue >= MinValueConstant && intValue <= MaxValueConstant)
                {
                    result = new(intValue);
                    return true;
                }
            }
            catch
            {
                // Fall through to default
            }
        }

        result = default;
        return false;
    }

    static bool INumberBase<IntT40>.TryConvertFromSaturating<TOther>(TOther value, out IntT40 result)
    {
        if (value is IConvertible conv)
        {
            try
            {
                var intValue = conv.ToInt64(null);
                result = new(intValue);
                return true;
            }
            catch
            {
                // Fall through to default
            }
        }

        result = default;
        return false;
    }

    static bool INumberBase<IntT40>.TryConvertFromTruncating<TOther>(TOther value, out IntT40 result)
    {
        if (value is IConvertible conv)
        {
            try
            {
                var intValue = conv.ToInt64(null);
                result = new(intValue);
                return true;
            }
            catch
            {
                // Fall through to default
            }
        }

        result = default;
        return false;
    }

    static bool INumberBase<IntT40>.TryConvertToChecked<TOther>(IntT40 value, [MaybeNullWhen(false)] out TOther result)
        where TOther : default
    {
        if (typeof(TOther) == typeof(long))
        {
            result = (TOther)(object)value.value;
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<IntT40>.TryConvertToSaturating<TOther>(IntT40 value, [MaybeNullWhen(false)] out TOther result)
        where TOther : default
    {
        if (typeof(TOther) == typeof(long))
        {
            result = (TOther)(object)value.value;
            return true;
        }

        result = default;
        return false;
    }

    static bool INumberBase<IntT40>.TryConvertToTruncating<TOther>(IntT40 value, [MaybeNullWhen(false)] out TOther result)
        where TOther : default
    {
        if (typeof(TOther) == typeof(long))
        {
            result = (TOther)(object)value.value;
            return true;
        }

        result = default;
        return false;
    }

    #endregion
}