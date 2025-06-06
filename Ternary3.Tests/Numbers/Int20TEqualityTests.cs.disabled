// ReSharper disable SuspiciousTypeConversion.Global

namespace Tring.Tests.Numbers;

using FluentAssertions;
using System.Numerics;
using Tring.Numbers;

public class Int20TEqualityTests
{
    [Theory]
    [InlineData(42)]
    [InlineData(0)]
    [InlineData(-42)]
    [InlineData(1743392200)] // MaxValue
    [InlineData(-1743392200)] // MinValue
    public void Equals_SameValue_ShouldBeEqual(int value)
    {
        Int20T a = value;
        Int20T b = value;

        // All equality checks
        a.Equals((object)b).Should().BeTrue();
        a.Equals(b).Should().BeTrue();
        a.Equals((object)value).Should().BeTrue();
        a.Equals(value).Should().BeTrue();

        // Operators
        (a == b).Should().BeTrue();
        (a != b).Should().BeFalse();
        (a == value).Should().BeTrue();
        (value == a).Should().BeTrue();
        (a != value).Should().BeFalse();
        (value != a).Should().BeFalse();

        // Hash code consistency
        a.GetHashCode().Should().Be(b.GetHashCode());
        a.GetHashCode().Should().Be(value.GetHashCode());
    }

    [Fact]
    public void Equals_DifferentValues_ShouldNotBeEqual()
    {
        Int20T a = 42;
        Int20T b = 43;
        var intValue = 43;

        // All inequality checks
        a.Equals((object)b).Should().BeFalse();
        a.Equals(b).Should().BeFalse();
        a.Equals(intValue).Should().BeFalse();
        a.Equals((object)intValue).Should().BeFalse();

        // Operators
        (a == b).Should().BeFalse();
        (a != b).Should().BeTrue();
        (a == intValue).Should().BeFalse();
        (intValue == a).Should().BeFalse();
        (a != intValue).Should().BeTrue();
        (intValue != a).Should().BeTrue();
    }

    [Fact]
    public void Equals_ReturnsTrue_ForNumericTypesWithSameValues()
    {
        Int20T value = 42;
        value.Equals((object)(sbyte)42).Should().BeTrue();
        value.Equals((object)(byte)42).Should().BeTrue();
        value.Equals((object)(short)42).Should().BeTrue();
        value.Equals((object)(ushort)42).Should().BeTrue();
        value.Equals((object)(uint)42).Should().BeTrue();
        value.Equals((object)(long)42).Should().BeTrue();
        value.Equals((object)(ulong)42).Should().BeTrue();
        value.Equals((object)(float)42.0).Should().BeTrue();
        value.Equals((object)(double)42.0).Should().BeTrue();
        value.Equals((object)'*').Should().BeTrue(); // ASCII value of '*' is 42
        value.Equals((object)(decimal)42.0).Should().BeTrue();
        value.Equals((object)(decimal)42.0m).Should().BeTrue();
    }

    [Fact]
    public void Equals_ReturnsFalse_ForNumericTypesWithDifferentValues()
    {
        Int20T value = 42;
        value.Equals((object)(sbyte)43).Should().BeFalse();
        value.Equals((object)(byte)43).Should().BeFalse();
        value.Equals((object)(short)43).Should().BeFalse();
        value.Equals((object)(ushort)43).Should().BeFalse();
        value.Equals((object)(uint)43).Should().BeFalse();
        value.Equals((object)(long)43).Should().BeFalse();
        value.Equals((object)(ulong)43).Should().BeFalse();
        value.Equals((object)(float)43.0).Should().BeFalse();
        value.Equals((object)(double)43.0).Should().BeFalse();
        value.Equals((object)'#').Should().BeFalse();
        value.Equals((object)(decimal)43.0).Should().BeFalse();
        value.Equals((object)(decimal)43.0m).Should().BeFalse();
        value.Equals((object)new BigInteger(43)).Should().BeFalse();
        // non-integer values
        value.Equals((object)(decimal)42.01).Should().BeFalse();
        value.Equals((object)(decimal)42.01m).Should().BeFalse();
        value.Equals((object)(float)42.01).Should().BeFalse();
        value.Equals((object)(double)42.01).Should().BeFalse();
        // non-numeric values
        value.Equals((object)float.NaN).Should().BeFalse();
        value.Equals((object)double.NaN).Should().BeFalse();
        value.Equals((object)float.PositiveInfinity).Should().BeFalse();
        value.Equals((object)float.NegativeInfinity).Should().BeFalse();
        value.Equals((object)double.PositiveInfinity).Should().BeFalse();
        value.Equals((object)double.NegativeInfinity).Should().BeFalse();
        value.Equals((object)BigInteger.One).Should().BeFalse();
        value.Equals((object)BigInteger.Zero).Should().BeFalse();
        value.Equals((object)BigInteger.MinusOne).Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WithNonNumericTypes()
    {
        Int20T intT20 = 1;
        intT20.Equals("1").Should().BeFalse();
        intT20.Equals(new object()).Should().BeFalse();
        intT20.Equals(new List<int> { 1 }).Should().BeFalse();
        intT20.Equals(new[] { 1 }).Should().BeFalse();
        intT20.Equals(true).Should().BeFalse();

    }

    [Theory]
    [InlineData((long)2000000000)] // Above MaxValue
    [InlineData((long)-2000000000)] // Below MinValue
    [InlineData(uint.MaxValue)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData((long)2147483648)] // Above Int32.MaxValue
    [InlineData((long)-2147483649)] // Below Int32.MinValue
    public void Equals_WithOutOfRangeValues_ShouldReturnFalse(object value)
    {
        Int20T intT20 = 42;
        intT20.Equals((object)value).Should().BeFalse();
    }

    [Theory]
    [InlineData((sbyte)sbyte.MaxValue)]
    [InlineData((sbyte)sbyte.MinValue)]
    [InlineData((byte)byte.MaxValue)]
    [InlineData((byte)byte.MinValue)]
    [InlineData((short)short.MaxValue)]
    [InlineData((short)short.MinValue)]
    [InlineData((ushort)ushort.MaxValue)]
    [InlineData((ushort)ushort.MinValue)]
    [InlineData((int)1743392200)] // Max allowed value
    [InlineData((int)-1743392200)] // Min allowed value
    public void Equals_WithValidBoundaryValues_ShouldReturnTrue(object value)
    {
        Int20T intT20 = Convert.ToInt32(value);
        intT20.Equals((object)value).Should().BeTrue();
    }

    [Theory]
    [InlineData(TypeCode.Empty)]
    [InlineData(TypeCode.DBNull)]
    [InlineData(TypeCode.Char)]
    [InlineData(TypeCode.DateTime)]
    [InlineData(TypeCode.String)]
    [InlineData(TypeCode.Boolean)]
    public void Equals_WithNonNumericTypeCode_ShouldReturnFalse(TypeCode typeCode)
    {
        Int20T value = 42;
        var convertible = new ConvertibleWithTypeCode(typeCode);
        value.Equals(convertible).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithEqualityOperatorOnIConvertible_ShouldCompareCorrectly()
    {
        Int20T value = 42;
        IConvertible convertible = new ConvertibleWithTypeCode(TypeCode.Int32, 42);
        (convertible == value).Should().BeTrue();
        (convertible != value).Should().BeFalse();
        convertible = new ConvertibleWithTypeCode(TypeCode.Int32, 43);
        (convertible == value).Should().BeFalse();
        (convertible != value).Should().BeTrue();
    }

#pragma warning disable CS8605 // Unboxing a possibly null value.
#pragma warning disable CS8603 // Possible null reference return.
    /// <summary>
    /// Test class.
    /// </summary>
    /// <param name="typeCode">The IConvertible type code.</param>
    /// <param name="value">
    /// The boxed value to return.
    /// Leave empty to have the class throw an exception.
    /// </param>
    private class ConvertibleWithTypeCode(TypeCode typeCode, object? value = null)
        : IConvertible
    {
        public TypeCode GetTypeCode() => typeCode;
        public bool ToBoolean(IFormatProvider? provider) => (bool)value;
        public byte ToByte(IFormatProvider? provider) => (byte)value;
        public char ToChar(IFormatProvider? provider) => (char)value;
        public DateTime ToDateTime(IFormatProvider? provider) => (DateTime)value;
        public decimal ToDecimal(IFormatProvider? provider) => (decimal)value;
        public double ToDouble(IFormatProvider? provider) => (double)value;
        public short ToInt16(IFormatProvider? provider) => (short)value;
        public int ToInt32(IFormatProvider? provider) => (int)value;
        public long ToInt64(IFormatProvider? provider) => (long)value;
        public sbyte ToSByte(IFormatProvider? provider) => (sbyte)value;
        public float ToSingle(IFormatProvider? provider) => (float)value;
        public string ToString(IFormatProvider? provider) => value?.ToString() ?? string.Empty;
        public object ToType(Type conversionType, IFormatProvider? provider) => value;
        public ushort ToUInt16(IFormatProvider? provider) => (ushort)value;
        public uint ToUInt32(IFormatProvider? provider) => (uint)value;
        public ulong ToUInt64(IFormatProvider? provider) => (ulong)value;
    }
#pragma warning restore CS8605 // Unboxing a possibly null value.
#pragma warning restore CS8603 // Possible null reference return.
}