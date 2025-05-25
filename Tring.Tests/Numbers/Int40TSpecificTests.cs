namespace Tring.Tests.Numbers;

using FluentAssertions;
using Tring.Numbers;
using System.Globalization;

public class Int40TSpecificTests
{
    [Fact]
    public void Constants_ShouldHaveCorrectValues()
    {
        Int40T.MaxValue.Should().Be((Int40T)6078832729528464400L);
        Int40T.MinValue.Should().Be((Int40T)(-6078832729528464400L));
    }
    
    [Fact]
    public void BackingType_ShouldBeInt64()
    {
        // Test implicit conversions with Int64 specifically
        long value = 1000000000000L;
        Int40T converted = value;
        long backToLong = converted;
        backToLong.Should().Be(value);
        
        // Test with large values specific to Int64
        long largeValue = 3000000000000L;
        Int40T convertedLarge = largeValue;
        ((long)convertedLarge).Should().Be(largeValue);
    }
    
    [Fact]
    public void MaxValue_ShouldExceedUIntMaxValue()
    {
        // Verify that MaxValueConstant is indeed larger than uint.MaxValue
        long maxValueConstant = Int40T.MaxValue;
        uint uintMaxValue = uint.MaxValue;
        
        ((ulong)maxValueConstant).Should().BeGreaterThan(uintMaxValue);
    }
    
    [Fact]
    public void CompareTo_UInt_HandlesLargeMaxValue()
    {
        // For Int40T, the CompareTo method should handle uint differently than Int20T does
        // since the MaxValueConstant exceeds uint.MaxValue
        
        // Test with a uint value (will use direct CompareTo)
        uint uintValue = 2500000000; // Large value but still within uint range
        Int40T int40Value = 3000000000L;
        
        // The comparison should work correctly without the "greater than MaxValueConstant" check
        (int40Value > uintValue).Should().BeTrue();
        (int40Value.CompareTo(uintValue) > 0).Should().BeTrue();
        
        int40Value = 2000000000L;
        (int40Value < uintValue).Should().BeTrue();
        (int40Value.CompareTo(uintValue) < 0).Should().BeTrue();
        
        int40Value = uintValue;
        (int40Value == uintValue).Should().BeTrue();
        (int40Value.CompareTo(uintValue) == 0).Should().BeTrue();
    }
    
    [Fact]
    public void CompareTo_Object_HandlesUIntDifferently()
    {
        // Test the object-based CompareTo method with uint values
        object uintObject = (uint)3000000000;
        Int40T int40Value = 4000000000L;
        
        // The comparison should use direct comparison, not the "if (uint32 > MaxValueConstant)" check
        int40Value.CompareTo(uintObject).Should().BeGreaterThan(0);
        
        int40Value = 2000000000L;
        int40Value.CompareTo(uintObject).Should().BeLessThan(0);
        
        int40Value = 3000000000L;
        int40Value.CompareTo(uintObject).Should().Be(0);
    }
    
    [Fact]
    public void CompareTo_IConvertible_HandlesUIntCorrectly()
    {
        // Test with an IConvertible that returns a uint through GetTypeCode
        // Create a custom IConvertible that returns UInt32 through GetTypeCode
        UInt32Convertible convertible = new UInt32Convertible(3000000000); // Large uint value
        
        Int40T int40Value = 4000000000L;
        int40Value.CompareTo(convertible).Should().BeGreaterThan(0);
        
        int40Value = 2000000000L;
        int40Value.CompareTo(convertible).Should().BeLessThan(0);
        
        int40Value = 3000000000L;
        int40Value.CompareTo(convertible).Should().Be(0);
    }
}

// Helper class for testing IConvertible comparison behavior
public class UInt32Convertible : IConvertible
{
    private readonly uint _value;
    
    public UInt32Convertible(uint value)
    {
        _value = value;
    }
    
    public TypeCode GetTypeCode() => TypeCode.UInt32;
    
    public uint ToUInt32(IFormatProvider? provider) => _value;
    
    // Implement the required IConvertible methods
    public bool ToBoolean(IFormatProvider? provider) => _value != 0;
    public byte ToByte(IFormatProvider? provider) => (byte)_value;
    public char ToChar(IFormatProvider? provider) => (char)_value;
    public DateTime ToDateTime(IFormatProvider? provider) => throw new InvalidCastException();
    public decimal ToDecimal(IFormatProvider? provider) => _value;
    public double ToDouble(IFormatProvider? provider) => _value;
    public short ToInt16(IFormatProvider? provider) => (short)_value;
    public int ToInt32(IFormatProvider? provider) => (int)_value;
    public long ToInt64(IFormatProvider? provider) => _value;
    public sbyte ToSByte(IFormatProvider? provider) => (sbyte)_value;
    public float ToSingle(IFormatProvider? provider) => _value;
    public string ToString(IFormatProvider? provider) => _value.ToString(provider);
    public object ToType(Type conversionType, IFormatProvider? provider) => Convert.ChangeType(_value, conversionType, provider);
    public ushort ToUInt16(IFormatProvider? provider) => (ushort)_value;
    public ulong ToUInt64(IFormatProvider? provider) => _value;
}
