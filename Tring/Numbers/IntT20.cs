// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable IntVariableOverflowInUncheckedContext
// ReSharper disable SuspiciousTypeConversion.Global
namespace Tring.Numbers;

using System.Globalization;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents a 20-trit signed integer, modeled after the <see cref="int"/> (Int32) type.
/// </summary>
public readonly struct IntT20 : 
    IConvertible,
    IBinaryInteger<IntT20>,
    ISignedNumber<IntT20>
{
    private readonly int value;
    /// <summary>
    ///  Represents the maximum value of an <see cref="IntT20"/>, expressed as an <see cref="int"/> This field is constant.
    /// </summary>
    public const int MaxValueConstant = 1743392200;
    /// <summary>
    /// Represents the minimum value of an <see cref="IntT20"/>, expressed as an <see cref="int"/> This field is constant.
    /// </summary>
    public const int MinValueConstant = -1743392200;
    
    private const int Wrap = (int.MaxValue - MaxValueConstant) - (int.MinValue - MinValueConstant);

    /// <summary>
    /// Represents the largest possible value of an <see cref="IntT20"/>. This field is constant.
    /// </summary>
    public static IntT20 MaxValue => new(MaxValueConstant);

    /// <summary>
    /// Represents the smallest possible value of an <see cref="IntT20"/>. This field is constant.
    /// </summary>
    public static IntT20 MinValue => new(MinValueConstant);

    private IntT20(int value) =>
        this.value = value switch
        {
            > MaxValueConstant => value + Wrap,
            < MinValueConstant => value - Wrap,
            _ => value
        };

    /// <summary>
    /// Defines an implicit conversion of a 32-bit signed integer to an <see cref="IntT20"/>.
    /// </summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <returns>An <see cref="IntT20"/> that represents the converted 32-bit signed integer.</returns>
    public static implicit operator IntT20(int value) => new(value);

    /// <summary>
    /// Defines an implicit conversion of an <see cref="IntT20"/> to a 32-bit signed integer.
    /// </summary>
    /// <param name="value">The <see cref="IntT20"/> to convert.</param>
    /// <returns>A 32-bit signed integer that represents the converted <see cref="IntT20"/>.</returns>
    public static implicit operator int(IntT20 value) => value.value;

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified object.
    /// </summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="obj"/> is an instance of <see cref="IntT20"/> or a compatible numeric type
    /// and equals the value of this instance; otherwise, <see langword="false"/>.
    /// </returns>
    public override bool Equals(object? obj)
    {
        switch (obj)
        {
            case null: return false;
            case IntT20 other: return value == other.value;
            case int int32: return value == int32;
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
    /// Returns a value indicating whether this instance is equal to a specified <see cref="IntT20"/> value.
    /// </summary>
    /// <param name="other">An <see cref="IntT20"/> value to compare to this instance.</param>
    /// <returns><see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.</returns>
    public bool Equals(IntT20 other) => value == other.value;

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified <see cref="int"/> value.
    /// </summary>
    /// <param name="other">An <see cref="int"/> value to compare to this instance.</param>
    /// <returns><see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.</returns>
    public bool Equals(int other) => value == other;

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => value.GetHashCode();

    /// <summary>
    /// Returns a value indicating whether two <see cref="IntT20"/> instances are equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(IConvertible left, IntT20 right) => right.Equals(left);

    /// <summary>
    /// Returns a value indicating whether two <see cref="IntT20"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(IConvertible left, IntT20 right) => !right.Equals(left);

    /// <summary>
    /// Returns a value indicating whether two <see cref="IntT20"/> instances are equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(IntT20 left, IConvertible right) => left.Equals(right);

    /// <summary>
    /// Returns a value indicating whether two <see cref="IntT20"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(IntT20 left, IConvertible right) => !left.Equals(right);
    
    // Arithmetic operators
    public static IntT20 operator +(IntT20 left, IntT20 right) => new(left.value + right.value);
    public static IntT20 operator -(IntT20 left, IntT20 right) => new(left.value - right.value);
    public static IntT20 operator *(IntT20 left, IntT20 right) => new(left.value * right.value);
    public static IntT20 operator /(IntT20 left, IntT20 right) => new(left.value / right.value);
    public static IntT20 operator %(IntT20 left, IntT20 right) => new(left.value % right.value);
    public static IntT20 operator -(IntT20 value) => new(-value.value);
    public static IntT20 operator +(IntT20 value) => value;

    // Mixed-type arithmetic operators with int
    public static IntT20 operator +(IntT20 left, int right) => new(left.value + right);
    public static IntT20 operator +(int left, IntT20 right) => new(left + right.value);
    
    public static IntT20 operator -(IntT20 left, int right) => new(left.value - right);
    public static IntT20 operator -(int left, IntT20 right) => new(left - right.value);
    
    public static IntT20 operator *(IntT20 left, int right) => new(left.value * right);
    public static IntT20 operator *(int left, IntT20 right) => new(left * right.value);
    
    public static IntT20 operator /(IntT20 left, int right) => new(left.value / right);
    public static IntT20 operator /(int left, IntT20 right) => new(left / right.value);
    
    public static IntT20 operator %(IntT20 left, int right) => new(left.value % right);
    public static IntT20 operator %(int left, IntT20 right) => new(left % right.value);

    // Comparison operators with IntT20
    public static bool operator >(IntT20 left, IntT20 right) => left.value > right.value;
    public static bool operator <(IntT20 left, IntT20 right) => left.value < right.value;
    public static bool operator >=(IntT20 left, IntT20 right) => left.value >= right.value;
    public static bool operator <=(IntT20 left, IntT20 right) => left.value <= right.value;

    // Comparison operators with int
    public static bool operator >(IntT20 left, int right) => left.value > right;
    public static bool operator <(IntT20 left, int right) => left.value < right;
    public static bool operator >=(IntT20 left, int right) => left.value >= right;
    public static bool operator <=(IntT20 left, int right) => left.value <= right;
    
    public static bool operator >(int left, IntT20 right) => left > right.value;
    public static bool operator <(int left, IntT20 right) => left < right.value;
    public static bool operator >=(int left, IntT20 right) => left >= right.value;
    public static bool operator <=(int left, IntT20 right) => left <= right.value;

    // Comparison operators with uint
    public static bool operator >(IntT20 left, uint right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT20 left, uint right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT20 left, uint right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT20 left, uint right) => left.CompareTo(right) <= 0;
    
    public static bool operator >(uint left, IntT20 right) => right.CompareTo(left) < 0;
    public static bool operator <(uint left, IntT20 right) => right.CompareTo(left) > 0;
    public static bool operator >=(uint left, IntT20 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(uint left, IntT20 right) => right.CompareTo(left) >= 0;

    // Comparison operators with long
    public static bool operator >(IntT20 left, long right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT20 left, long right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT20 left, long right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT20 left, long right) => left.CompareTo(right) <= 0;
    
    public static bool operator >(long left, IntT20 right) => right.CompareTo(left) < 0;
    public static bool operator <(long left, IntT20 right) => right.CompareTo(left) > 0;
    public static bool operator >=(long left, IntT20 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(long left, IntT20 right) => right.CompareTo(left) >= 0;

    // Comparison operators with ulong
    public static bool operator >(IntT20 left, ulong right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT20 left, ulong right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT20 left, ulong right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT20 left, ulong right) => left.CompareTo(right) <= 0;
    
    public static bool operator >(ulong left, IntT20 right) => right.CompareTo(left) < 0;
    public static bool operator <(ulong left, IntT20 right) => right.CompareTo(left) > 0;
    public static bool operator >=(ulong left, IntT20 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(ulong left, IntT20 right) => right.CompareTo(left) >= 0;

    // Comparison operators with short
    public static bool operator >(IntT20 left, short right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT20 left, short right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT20 left, short right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT20 left, short right) => left.CompareTo(right) <= 0;
    
    public static bool operator >(short left, IntT20 right) => right.CompareTo(left) < 0;
    public static bool operator <(short left, IntT20 right) => right.CompareTo(left) > 0;
    public static bool operator >=(short left, IntT20 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(short left, IntT20 right) => right.CompareTo(left) >= 0;

    // Comparison operators with ushort
    public static bool operator >(IntT20 left, ushort right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT20 left, ushort right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT20 left, ushort right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT20 left, ushort right) => left.CompareTo(right) <= 0;
    
    public static bool operator >(ushort left, IntT20 right) => right.CompareTo(left) < 0;
    public static bool operator <(ushort left, IntT20 right) => right.CompareTo(left) > 0;
    public static bool operator >=(ushort left, IntT20 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(ushort left, IntT20 right) => right.CompareTo(left) >= 0;

    // Comparison operators with byte
    public static bool operator >(IntT20 left, byte right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT20 left, byte right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT20 left, byte right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT20 left, byte right) => left.CompareTo(right) <= 0;
    
    public static bool operator >(byte left, IntT20 right) => right.CompareTo(left) < 0;
    public static bool operator <(byte left, IntT20 right) => right.CompareTo(left) > 0;
    public static bool operator >=(byte left, IntT20 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(byte left, IntT20 right) => right.CompareTo(left) >= 0;

    // Comparison operators with sbyte
    public static bool operator >(IntT20 left, sbyte right) => left.CompareTo(right) > 0;
    public static bool operator <(IntT20 left, sbyte right) => left.CompareTo(right) < 0;
    public static bool operator >=(IntT20 left, sbyte right) => left.CompareTo(right) >= 0;
    public static bool operator <=(IntT20 left, sbyte right) => left.CompareTo(right) <= 0;
    
    public static bool operator >(sbyte left, IntT20 right) => right.CompareTo(left) < 0;
    public static bool operator <(sbyte left, IntT20 right) => right.CompareTo(left) > 0;
    public static bool operator >=(sbyte left, IntT20 right) => right.CompareTo(left) <= 0;
    public static bool operator <=(sbyte left, IntT20 right) => right.CompareTo(left) >= 0;


    // ToString implementation
    public override string ToString() => value.ToString();
    public string ToString(string? format) => value.ToString(format);
    public string ToString(IFormatProvider? provider) => value.ToString(provider);
    public string ToString(string? format, IFormatProvider? provider) => value.ToString(format, provider);

    // Parsing methods

    /// <summary>
    /// Converts the string representation of a number to its <see cref="IntT20"/> equivalent.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <returns>An <see cref="IntT20"/> equivalent to the number contained in <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not in the correct format.</exception>
    /// <exception cref="OverflowException"><paramref name="s"/> represents a number less than <see cref="IntT20.MinValue"/> or greater than <see cref="IntT20.MaxValue"/>.</exception>
    public static IntT20 Parse(string s) => new(int.Parse(s));

    /// <summary>
    /// Converts the string representation of a number in a specified style to its <see cref="IntT20"/> equivalent.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicate the style elements that can be present in <paramref name="s"/>.</param>
    /// <returns>An <see cref="IntT20"/> equivalent to the number contained in <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="style"/> is not a <see cref="NumberStyles"/> value or <paramref name="style"/> includes the <see cref="NumberStyles.AllowHexSpecifier"/> value.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a format compliant with <paramref name="style"/>.</exception>
    /// <exception cref="OverflowException"><paramref name="s"/> represents a number less than <see cref="IntT20.MinValue"/> or greater than <see cref="IntT20.MaxValue"/>.</exception>
    public static IntT20 Parse(string s, NumberStyles style) => new(int.Parse(s, style));

    /// <summary>
    /// Tries to convert the string representation of a number to its <see cref="IntT20"/> equivalent, and returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="IntT20"/> value equivalent to the number contained in <paramref name="s"/> if the conversion succeeded, or zero if the conversion failed. This parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, out IntT20 result)
    {
        bool success = int.TryParse(s, out int value);
        result = new IntT20(value);
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
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not an <see cref="IntT20"/> or a type that can be converted to an integer.</exception>
    public int CompareTo(object? obj)
    {
        if (obj == null) return 1;
        if (obj is IntT20 other) return CompareTo(other);
        
        // For large numeric types that exceed IntT20's range, return -1 if greater than MaxValue, 1 if less than MinValue
        try
        {
            if (obj is int int32) return value.CompareTo(int32);
            if (obj is long int64)
            {
                if (int64 > MaxValueConstant) return -1;
                if (int64 < MinValueConstant) return 1;
                return value.CompareTo((int)int64);
            }
            if (obj is uint uint32)
            {
                if (uint32 > MaxValueConstant) return -1;
                return value.CompareTo((int)uint32);
            }
            if (obj is ulong uint64)
            {
                if (uint64 > MaxValueConstant) return -1;
                return value.CompareTo((int)uint64);
            }
            if (obj is short int16) return value.CompareTo(int16);
            if (obj is ushort uint16) return value.CompareTo(uint16);
            if (obj is byte byteVal) return value.CompareTo(byteVal);
            if (obj is sbyte sbyteVal) return value.CompareTo(sbyteVal);
            if (obj is float singleValue)
            {
                if (singleValue > MaxValueConstant) return -1;
                if (singleValue < MinValueConstant) return 1;
                return value.CompareTo((int)singleValue);
            }
            if (obj is double doubleValue)
            {
                if (doubleValue > MaxValueConstant) return -1;
                if (doubleValue < MinValueConstant) return 1;
                return value.CompareTo((int)doubleValue);
            }
            if (obj is decimal decimalValue)
            {
                if (decimalValue > MaxValueConstant) return -1;
                if (decimalValue < MinValueConstant) return 1;
                return value.CompareTo((int)decimalValue);
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
                        return value.CompareTo((int)uint32Value);
                    case TypeCode.Int64:
                        var int64Value = convertible.ToInt64(null);
                        if (int64Value > MaxValueConstant) return -1;
                        if (int64Value < MinValueConstant) return 1;
                        return value.CompareTo((int)int64Value);
                    case TypeCode.UInt64:
                        var uint64Value = convertible.ToUInt64(null);
                        if (uint64Value > MaxValueConstant) return -1;
                        return value.CompareTo((int)uint64Value);
                    case TypeCode.Single:
                        var singleVal = convertible.ToSingle(null);
                        if (singleVal > MaxValueConstant) return -1;
                        if (singleVal < MinValueConstant) return 1;
                        return value.CompareTo((int)singleVal);
                    case TypeCode.Double:
                        var doubleVal = convertible.ToDouble(null);
                        if (doubleVal > MaxValueConstant) return -1;
                        if (doubleVal < MinValueConstant) return 1;
                        return value.CompareTo((int)doubleVal);
                    case TypeCode.Decimal:
                        var decimalVal = convertible.ToDecimal(null);
                        if (decimalVal > MaxValueConstant) return -1;
                        if (decimalVal < MinValueConstant) return 1;
                        return value.CompareTo((int)decimalVal);
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
    /// Compares this instance to a specified <see cref="IntT20"/> object and returns an indication of their relative values.
    /// </summary>
    /// <param name="other">An <see cref="IntT20"/> object to compare.</param>
    /// <returns>
    /// A signed number indicating the relative values of this instance and <paramref name="other"/>.
    /// Return Value Description:
    /// Less than zero: This instance is less than <paramref name="other"/>.
    /// Zero: This instance is equal to <paramref name="other"/>.
    /// Greater than zero: This instance is greater than <paramref name="other"/>.
    /// </returns>
    public int CompareTo(IntT20 other) => value.CompareTo(other.value);

    #region IConvertible Implementation
    public TypeCode GetTypeCode() => TypeCode.Int32;
    
    bool IConvertible.ToBoolean(IFormatProvider? provider) => value != 0;
    char IConvertible.ToChar(IFormatProvider? provider) => Convert.ToChar(value);
    sbyte IConvertible.ToSByte(IFormatProvider? provider) => Convert.ToSByte(value);
    byte IConvertible.ToByte(IFormatProvider? provider) => Convert.ToByte(value);
    short IConvertible.ToInt16(IFormatProvider? provider) => Convert.ToInt16(value);
    ushort IConvertible.ToUInt16(IFormatProvider? provider) => Convert.ToUInt16(value);
    int IConvertible.ToInt32(IFormatProvider? provider) => value;
    uint IConvertible.ToUInt32(IFormatProvider? provider) => Convert.ToUInt32(value);
    long IConvertible.ToInt64(IFormatProvider? provider) => value;
    ulong IConvertible.ToUInt64(IFormatProvider? provider) => Convert.ToUInt64(value);
    float IConvertible.ToSingle(IFormatProvider? provider) => value;
    double IConvertible.ToDouble(IFormatProvider? provider) => value;
    decimal IConvertible.ToDecimal(IFormatProvider? provider) => value;
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
    public static IntT20 operator ++(IntT20 value) => new(value.value + 1);
    public static IntT20 operator --(IntT20 value) => new(value.value - 1);
    #endregion

    static bool IEqualityOperators<IntT20, IntT20, bool>.operator ==(IntT20 left, IntT20 right) => left.value == right.value;
    static bool IEqualityOperators<IntT20, IntT20, bool>.operator !=(IntT20 left, IntT20 right) => left.value != right.value;

    public static bool operator ==(IntT20 left, IntT20 right) => left.value == right.value;
    public static bool operator !=(IntT20 left, IntT20 right) => left.value != right.value;

    #region Binary Operations
    public static IntT20 RotateLeft(IntT20 value, int rotateAmount) =>
        new((int)BitOperations.RotateLeft((ulong)value.value, rotateAmount));
    #endregion

    #region Interface Static Members
    static IntT20 ISignedNumber<IntT20>.NegativeOne => new(-1);
    static IntT20 INumberBase<IntT20>.One => new(1);
    static IntT20 INumberBase<IntT20>.Zero => new(0);
    static IntT20 IAdditiveIdentity<IntT20, IntT20>.AdditiveIdentity => new(0);
    static IntT20 IMultiplicativeIdentity<IntT20, IntT20>.MultiplicativeIdentity => new(1);

    static bool INumberBase<IntT20>.IsCanonical(IntT20 value) => true;
    static bool INumberBase<IntT20>.IsComplexNumber(IntT20 value) => false;
    static bool INumberBase<IntT20>.IsEvenInteger(IntT20 value) => value.value % 2 == 0;
    static bool INumberBase<IntT20>.IsFinite(IntT20 value) => true;
    static bool INumberBase<IntT20>.IsImaginaryNumber(IntT20 value) => false;
    static bool INumberBase<IntT20>.IsInfinity(IntT20 value) => false;
    static bool INumberBase<IntT20>.IsInteger(IntT20 value) => true;
    static bool INumberBase<IntT20>.IsNaN(IntT20 value) => false;
    static bool INumberBase<IntT20>.IsNegative(IntT20 value) => value.value < 0;
    static bool INumberBase<IntT20>.IsNegativeInfinity(IntT20 value) => false;
    static bool INumberBase<IntT20>.IsNormal(IntT20 value) => value.value != 0;
    static bool INumberBase<IntT20>.IsOddInteger(IntT20 value) => value.value % 2 != 0;
    static bool INumberBase<IntT20>.IsPositive(IntT20 value) => value.value > 0;
    static bool INumberBase<IntT20>.IsPositiveInfinity(IntT20 value) => false;
    static bool INumberBase<IntT20>.IsRealNumber(IntT20 value) => true;
    static bool INumberBase<IntT20>.IsSubnormal(IntT20 value) => false;
    static bool INumberBase<IntT20>.IsZero(IntT20 value) => value.value == 0;

    static IntT20 INumberBase<IntT20>.MaxMagnitude(IntT20 x, IntT20 y) => 
        Math.Abs(x.value) > Math.Abs(y.value) ? x : y;
    static IntT20 INumberBase<IntT20>.MaxMagnitudeNumber(IntT20 x, IntT20 y) => 
        Math.Abs(x.value) > Math.Abs(y.value) ? x : y;
    static IntT20 INumberBase<IntT20>.MinMagnitude(IntT20 x, IntT20 y) => 
        Math.Abs(x.value) < Math.Abs(y.value) ? x : y;
    static IntT20 INumberBase<IntT20>.MinMagnitudeNumber(IntT20 x, IntT20 y) => 
        Math.Abs(x.value) < Math.Abs(y.value) ? x : y;

    static int INumberBase<IntT20>.Radix => 2;
    
    static IntT20 INumberBase<IntT20>.Abs(IntT20 value) => 
        value.value < 0 ? new(-value.value) : value;

    static IntT20 INumberBase<IntT20>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => 
        new(int.Parse(s, style, provider));

    static IntT20 INumberBase<IntT20>.Parse(string s, NumberStyles style, IFormatProvider? provider) => 
        new(int.Parse(s, style, provider));

    static bool INumberBase<IntT20>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out IntT20 result)
    {
        if (int.TryParse(s, style, provider, out var parsed))
        {
            result = new(parsed);
            return true;
        }
        result = default;
        return false;
    }

    static bool INumberBase<IntT20>.TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out IntT20 result)
    {
        if (int.TryParse(s, style, provider, out var parsed))
        {
            result = new(parsed);
            return true;
        }
        result = default;
        return false;
    }

    static bool IBinaryNumber<IntT20>.IsPow2(IntT20 value) => 
        value.value > 0 && (value.value & (value.value - 1)) == 0;
        
    static IntT20 IBinaryNumber<IntT20>.Log2(IntT20 value) => 
        new(BitOperations.Log2((uint)value.value));

    int IBinaryInteger<IntT20>.GetByteCount() => sizeof(int);
    
    int IBinaryInteger<IntT20>.GetShortestBitLength() => 
        value == 0 ? 1 : BitOperations.Log2((uint)Math.Abs(value)) + 1;

    static IntT20 IBinaryInteger<IntT20>.PopCount(IntT20 value) => 
        new(BitOperations.PopCount((uint)value.value));

    static IntT20 IBinaryInteger<IntT20>.TrailingZeroCount(IntT20 value) => 
        new(BitOperations.TrailingZeroCount((uint)value.value));

    bool IBinaryInteger<IntT20>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten)
    {
        if (destination.Length < sizeof(int))
        {
            bytesWritten = 0;
            return false;
        }

        var bytes = BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        bytes.CopyTo(destination);
        bytesWritten = sizeof(int);
        return true;
    }

    bool IBinaryInteger<IntT20>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten)
    {
        if (destination.Length < sizeof(int))
        {
            bytesWritten = 0;
            return false;
        }

        BitConverter.GetBytes(value).CopyTo(destination);
        bytesWritten = sizeof(int);
        return true;
    }

    static bool IBinaryInteger<IntT20>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out IntT20 result)
    {
        if (source.Length < sizeof(int))
        {
            result = default;
            return false;
        }

        var bytes = source.Slice(0, sizeof(int)).ToArray();
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);
        
        result = new(BitConverter.ToInt32(bytes));
        return true;
    }

    static bool IBinaryInteger<IntT20>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out IntT20 result)
    {
        if (source.Length < sizeof(int))
        {
            result = default;
            return false;
        }

        result = new(BitConverter.ToInt32(source));
        return true;
    }

    #region ISpanFormattable/ISpanParsable Implementation
    static IntT20 ISpanParsable<IntT20>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => 
        new(int.Parse(s, NumberStyles.Integer, provider));

    static bool ISpanParsable<IntT20>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out IntT20 result)
    {
        if (int.TryParse(s, NumberStyles.Integer, provider, out var value))
        {
            result = new(value);
            return true;
        }
        result = default;
        return false;
    }

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) =>
        value.TryFormat(destination, out charsWritten, format.ToString(), provider);

    static IntT20 IParsable<IntT20>.Parse(string s, IFormatProvider? provider) => 
        new(int.Parse(s, NumberStyles.Integer, provider));

    static bool IParsable<IntT20>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out IntT20 result)
    {
        if (int.TryParse(s, NumberStyles.Integer, provider, out var value))
        {
            result = new(value);
            return true;
        }
        result = default;
        return false;
    }
    #endregion

    #region Bit Operators
    static IntT20 IBitwiseOperators<IntT20, IntT20, IntT20>.operator &(IntT20 left, IntT20 right) => 
        new(left.value & right.value);
    static IntT20 IBitwiseOperators<IntT20, IntT20, IntT20>.operator |(IntT20 left, IntT20 right) => 
        new(left.value | right.value);
    static IntT20 IBitwiseOperators<IntT20, IntT20, IntT20>.operator ^(IntT20 left, IntT20 right) => 
        new(left.value ^ right.value);
    static IntT20 IBitwiseOperators<IntT20, IntT20, IntT20>.operator ~(IntT20 value) => 
        new(~value.value);

    static IntT20 IShiftOperators<IntT20, int, IntT20>.operator <<(IntT20 value, int shiftAmount) => 
        new(value.value << shiftAmount);
    static IntT20 IShiftOperators<IntT20, int, IntT20>.operator >>(IntT20 value, int shiftAmount) => 
        new(value.value >> shiftAmount);
    static IntT20 IShiftOperators<IntT20, int, IntT20>.operator >>>(IntT20 value, int shiftAmount) => 
        new(int.CreateChecked(((uint)value.value) >> shiftAmount));
    #endregion
    #endregion

    #region Generic Conversions
    static bool INumberBase<IntT20>.TryConvertFromChecked<TOther>(TOther value, out IntT20 result)
    {
        if (value is IConvertible conv)
        {
            try
            {
                var intValue = conv.ToInt32(null);
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

    static bool INumberBase<IntT20>.TryConvertFromSaturating<TOther>(TOther value, out IntT20 result)
    {
        if (value is IConvertible conv)
        {
            try
            {
                var intValue = conv.ToInt32(null);
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

    static bool INumberBase<IntT20>.TryConvertFromTruncating<TOther>(TOther value, out IntT20 result)
    {
        if (value is IConvertible conv)
        {
            try
            {
                var intValue = conv.ToInt32(null);
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

    static bool INumberBase<IntT20>.TryConvertToChecked<TOther>(IntT20 value, [MaybeNullWhen(false)] out TOther result)
        where TOther : default
    {
        if (typeof(TOther) == typeof(int))
        {
            result = (TOther)(object)value.value;
            return true;
        }
        result = default;
        return false;
    }

    static bool INumberBase<IntT20>.TryConvertToSaturating<TOther>(IntT20 value, [MaybeNullWhen(false)] out TOther result)
        where TOther : default
    {
        if (typeof(TOther) == typeof(int))
        {
            result = (TOther)(object)value.value;
            return true;
        }
        result = default;
        return false;
    }

    static bool INumberBase<IntT20>.TryConvertToTruncating<TOther>(IntT20 value, [MaybeNullWhen(false)] out TOther result)
        where TOther : default
    {
        if (typeof(TOther) == typeof(int))
        {
            result = (TOther)(object)value.value;
            return true;
        }
        result = default;
        return false;
    }
    #endregion
}
