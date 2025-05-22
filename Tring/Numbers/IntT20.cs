// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable IntVariableOverflowInUncheckedContext
namespace Tring.Numbers;

using System.Globalization;

/// <summary>
/// Represents a 20-trit signed integer, modeled after the <see cref="int"/> (Int32) type.
/// </summary>
public readonly struct IntT20 : IEquatable<IntT20>, IFormattable, IComparable, IComparable<IntT20>
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
    public static bool operator ==(IntT20 left, IntT20 right) => left.value == right.value;

    /// <summary>
    /// Returns a value indicating whether two <see cref="IntT20"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(IntT20 left, IntT20 right) => left.value != right.value;

    public static bool operator ==(IntT20 left, int right) => left.value == right;
    public static bool operator !=(IntT20 left, int right) => left.value != right;
    public static bool operator ==(int left, IntT20 right) => left == right.value;
    public static bool operator !=(int left, IntT20 right) => left != right.value;

    // ReSharper disable SuspiciousTypeConversion.Global
    public static bool operator ==(IConvertible left, IntT20 right) => right.Equals(left);
    public static bool operator !=(IConvertible left, IntT20 right) => !right.Equals(left);
    public static bool operator ==(IntT20 left, IConvertible right) => left.Equals(right);
    public static bool operator !=(IntT20 left, IConvertible right) => !left.Equals(right);
    // ReSharper restore SuspiciousTypeConversion.Global

    // Arithmetic operators
    public static IntT20 operator +(IntT20 left, IntT20 right) => new(left.value + right.value);
    public static IntT20 operator -(IntT20 left, IntT20 right) => new(left.value - right.value);
    public static IntT20 operator *(IntT20 left, IntT20 right) => new(left.value * right.value);
    public static IntT20 operator /(IntT20 left, IntT20 right) => 
        right.value == 0 
            ? throw new DivideByZeroException() 
            : new(left.value / right.value);
    public static IntT20 operator %(IntT20 left, IntT20 right) => 
        right.value == 0 
            ? throw new DivideByZeroException() 
            : new(left.value % right.value);
    public static IntT20 operator -(IntT20 value) => new(-value.value);
    public static IntT20 operator +(IntT20 value) => value;

    // Comparison operators
    public static bool operator >(IntT20 left, IntT20 right) => left.value > right.value;
    public static bool operator <(IntT20 left, IntT20 right) => left.value < right.value;
    public static bool operator >=(IntT20 left, IntT20 right) => left.value >= right.value;
    public static bool operator <=(IntT20 left, IntT20 right) => left.value <= right.value;

    // Bitwise operators
    public static IntT20 operator &(IntT20 left, IntT20 right) => new(left.value & right.value);
    public static IntT20 operator |(IntT20 left, IntT20 right) => new(left.value | right.value);
    public static IntT20 operator ^(IntT20 left, IntT20 right) => new(left.value ^ right.value);
    public static IntT20 operator ~(IntT20 value) => new(~value.value);
    public static IntT20 operator <<(IntT20 value, int shift) => new(value.value << shift);
    public static IntT20 operator >> (IntT20 value, int shift) => new(value.value >> shift);

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
    /// Converts the string representation of a number in a specified culture-specific format to its <see cref="IntT20"/> equivalent.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s"/>.</param>
    /// <returns>An <see cref="IntT20"/> equivalent to the number contained in <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not in the correct format.</exception>
    /// <exception cref="OverflowException"><paramref name="s"/> represents a number less than <see cref="IntT20.MinValue"/> or greater than <see cref="IntT20.MaxValue"/>.</exception>
    public static IntT20 Parse(string s, IFormatProvider? provider) => new(int.Parse(s, provider));

    /// <summary>
    /// Converts the string representation of a number in a specified style and culture-specific format to its <see cref="IntT20"/> equivalent.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicate the style elements that can be present in <paramref name="s"/>.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s"/>.</param>
    /// <returns>An <see cref="IntT20"/> equivalent to the number contained in <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="style"/> is not a <see cref="NumberStyles"/> value or <paramref name="style"/> includes the <see cref="NumberStyles.AllowHexSpecifier"/> value.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a format compliant with <paramref name="style"/>.</exception>
    /// <exception cref="OverflowException"><paramref name="s"/> represents a number less than <see cref="IntT20.MinValue"/> or greater than <see cref="IntT20.MaxValue"/>.</exception>
    public static IntT20 Parse(string s, NumberStyles style, IFormatProvider? provider) => new(int.Parse(s, style, provider));

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
    /// Tries to convert the string representation of a number in a specified style and culture-specific format to its <see cref="IntT20"/> equivalent, and returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicate the style elements that can be present in <paramref name="s"/>.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s"/>.</param>
    /// <param name="result">When this method returns, contains the <see cref="IntT20"/> value equivalent to the number contained in <paramref name="s"/> if the conversion succeeded, or zero if the conversion failed. This parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="style"/> is not a <see cref="NumberStyles"/> value or <paramref name="style"/> includes the <see cref="NumberStyles.AllowHexSpecifier"/> value.</exception>
    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out IntT20 result)
    {
        bool success = int.TryParse(s, style, provider, out int value);
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
        
        // Cast to primitive types and compare directly with the integer value
        if (obj is int int32) return value.CompareTo(int32);
        if (obj is long int64) return value.CompareTo((int)int64);
        if (obj is short int16) return value.CompareTo(int16);
        if (obj is byte byteVal) return value.CompareTo(byteVal);
        if (obj is sbyte sbyteVal) return value.CompareTo(sbyteVal);
        if (obj is uint uint32) return value.CompareTo((int)uint32);
        if (obj is ulong uint64)
        {
            if (uint64 > int.MaxValue)
                return -1; // This IntT20 is less than the ulong
            return value.CompareTo((int)uint64);
        }
        if (obj is ushort uint16) return value.CompareTo(uint16);
        if (obj is float singleValue) return value.CompareTo((int)singleValue);
        if (obj is double doubleValue) return value.CompareTo((int)doubleValue);
        if (obj is decimal decimalValue) return value.CompareTo((int)decimalValue);
        if (obj is char charVal) return value.CompareTo(charVal);

        if (obj is IConvertible convertible)
        {
            try
            {
                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (convertible.GetTypeCode())
                {
                    case TypeCode.Int16: return value.CompareTo(convertible.ToInt16(null));
                    case TypeCode.UInt16: return value.CompareTo(convertible.ToUInt16(null));
                    case TypeCode.Int32: return value.CompareTo(convertible.ToInt32(null));
                    case TypeCode.UInt32:
                        var uint32Value = convertible.ToUInt32(null);
                        if (uint32Value > int.MaxValue)
                            return -1; // This IntT20 is less than the uint32
                        return value.CompareTo((int)uint32Value);
                    case TypeCode.Int64:
                        var int64Value = convertible.ToInt64(null);
                        if (int64Value > int.MaxValue)
                            return -1; // This IntT20 is less than the int64
                        if (int64Value < int.MinValue)
                            return 1; // This IntT20 is greater than the int64
                        return value.CompareTo((int)int64Value);
                    case TypeCode.UInt64:
                        var uint64Value = convertible.ToUInt64(null);
                        if (uint64Value > int.MaxValue)
                            return -1; // This IntT20 is less than the uint64
                        return value.CompareTo((int)uint64Value);
                    case TypeCode.Byte: return value.CompareTo(convertible.ToByte(null));
                    case TypeCode.SByte: return value.CompareTo(convertible.ToSByte(null));
                    case TypeCode.Single: return value.CompareTo((int)convertible.ToSingle(null));
                    case TypeCode.Double: return value.CompareTo((int)convertible.ToDouble(null));
                    case TypeCode.Decimal: return value.CompareTo((int)convertible.ToDecimal(null));
                    default: throw new ArgumentException("Object is not a valid type", nameof(obj));
                }
            }
            catch
            {
                throw new ArgumentException("Object is not a valid type", nameof(obj));
            }
        }

        throw new ArgumentException("Object is not a valid type", nameof(obj));
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
}
