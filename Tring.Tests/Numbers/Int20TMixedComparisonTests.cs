using FluentAssertions;
using Tring.Numbers;
using Xunit;

namespace Tring.Tests.Numbers;

public class Int20TMixedComparisonTests
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(-1, 0)]
    [InlineData(1743392200, int.MaxValue)] // MaxValue
    public void LessThan_WithInt32_ShouldReturnCorrectResult(Int20T intT20, int int32)
    {
        (intT20 < int32).Should().BeTrue();
        (int32 > intT20).Should().BeTrue();
    }

    [Theory]
    [InlineData(0, 1)]  // 1 > 0
    [InlineData(-1, 0)] // 0 > -1
    [InlineData(-2147483648, -1743392200)] // -1743392200 > int.MinValue
    public void GreaterThan_WithInt32_ShouldReturnCorrectResult(int int32, Int20T intT20)
    {
        (intT20 > int32).Should().BeTrue();
        (int32 < intT20).Should().BeTrue();
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(-1, -1)]
    [InlineData(1743392200, 1743392200)] // MaxValue
    [InlineData(-1743392200, -1743392200)] // MinValue
    public void LessThanOrEqual_WithInt32_ShouldReturnCorrectResultWhenEqual(Int20T intT20, int int32)
    {
        (intT20 <= int32).Should().BeTrue();
        (int32 >= intT20).Should().BeTrue();
    }

    [Theory]
    [InlineData(byte.MaxValue)]
    [InlineData(0)]
    [InlineData(1)]
    public void Comparison_WithByte_ShouldWorkCorrectly(byte value)
    {
        Int20T intT20 = value - 1;
        (intT20 < value).Should().BeTrue();
        (intT20 <= value).Should().BeTrue();
        (value > intT20).Should().BeTrue();
        (value >= intT20).Should().BeTrue();

        intT20 = value;
        (intT20 <= value).Should().BeTrue();
        (intT20 >= value).Should().BeTrue();
        (value <= intT20).Should().BeTrue();
        (value >= intT20).Should().BeTrue();

        intT20 = value + 1;
        (intT20 > value).Should().BeTrue();
        (intT20 >= value).Should().BeTrue();
        (value < intT20).Should().BeTrue();
        (value <= intT20).Should().BeTrue();
    }

    [Theory]
    [InlineData(short.MaxValue)]
    [InlineData(short.MinValue)]
    [InlineData(0)]
    public void Comparison_WithInt16_ShouldWorkCorrectly(short value)
    {
        Int20T intT20 = value - 1;
        (intT20 < value).Should().BeTrue();
        (intT20 <= value).Should().BeTrue();
        (value > intT20).Should().BeTrue();
        (value >= intT20).Should().BeTrue();

        intT20 = value;
        (intT20 <= value).Should().BeTrue();
        (intT20 >= value).Should().BeTrue();
        (value <= intT20).Should().BeTrue();
        (value >= intT20).Should().BeTrue();

        intT20 = value + 1;
        (intT20 > value).Should().BeTrue();
        (intT20 >= value).Should().BeTrue();
        (value < intT20).Should().BeTrue();
        (value <= intT20).Should().BeTrue();
    }

    [Theory]
    [InlineData(ushort.MaxValue - 1)]
    [InlineData(0)]
    [InlineData(1)]
    public void Comparison_WithUInt16_ShouldWorkCorrectly(ushort value)
    {
        Int20T intT20 = value - 1;
        (intT20 < value).Should().BeTrue();
        (intT20 <= value).Should().BeTrue();
        (value > intT20).Should().BeTrue();
        (value >= intT20).Should().BeTrue();

        intT20 = value;
        (intT20 <= value).Should().BeTrue();
        (intT20 >= value).Should().BeTrue();
        (value <= intT20).Should().BeTrue();
        (value >= intT20).Should().BeTrue();

        intT20 = value + 1;
        (intT20 > value).Should().BeTrue();
        (intT20 >= value).Should().BeTrue();
        (value < intT20).Should().BeTrue();
        (value <= intT20).Should().BeTrue();
    }

    [Theory]
    [InlineData(0U)]
    [InlineData(1U)]
    [InlineData(Int20T.MaxValueConstant)]
    public void Comparison_WithUInt32_WithinBounds_ShouldWorkCorrectly(uint value)
    {
        Int20T valueAsIntT20 = (int)value;
        
        (valueAsIntT20 <= value).Should().BeTrue();
        (value >= valueAsIntT20).Should().BeTrue();
        
        Int20T smallerValue = valueAsIntT20 - 1;
        (smallerValue < value).Should().BeTrue();
        (value > smallerValue).Should().BeTrue();
        
        (valueAsIntT20 >= value).Should().BeTrue();
    }

    [Theory]
    [InlineData(uint.MaxValue)]
    public void Comparison_WithUInt32_AboveBounds_ShouldWorkCorrectly(uint value)
    {
        Int20T maxValue = Int20T.MaxValue;
        
        (maxValue <= value).Should().BeTrue();
        (value >= maxValue).Should().BeTrue();
        
        Int20T smallerValue = maxValue - 1;
        (smallerValue < value).Should().BeTrue();
        (value > smallerValue).Should().BeTrue();
    }

    [Theory]
    [InlineData(long.MaxValue)]
    [InlineData(Int20T.MaxValueConstant + 1)]
    public void Comparison_WithInt64_AboveMaxValue_ShouldWorkCorrectly(long value)
    {
        Int20T bounded = Int20T.MaxValue;
        
        (bounded <= value).Should().BeTrue();
        (value >= bounded).Should().BeTrue();
        
        Int20T smaller = bounded - 1;
        (smaller < value).Should().BeTrue();
        (value > smaller).Should().BeTrue();
    }

    [Theory]
    [InlineData(long.MinValue)]
    [InlineData(Int20T.MinValueConstant - 1)]
    public void Comparison_WithInt64_BelowMinValue_ShouldWorkCorrectly(long value)
    {
        Int20T bounded = Int20T.MinValue;
        
        (bounded >= value).Should().BeTrue();
        (value <= bounded).Should().BeTrue();
        
        Int20T larger = bounded + 1;
        (larger > value).Should().BeTrue();
        (value < larger).Should().BeTrue();
    }

    [Theory]
    [InlineData(0L)]
    public void Comparison_WithInt64_WithinNormalBounds_ShouldWorkCorrectly(long value)
    {
        Int20T bounded = (int)value;
        
        Int20T smaller = bounded - 1;
        (smaller < value).Should().BeTrue();
        (value > smaller).Should().BeTrue();

        (bounded.CompareTo(value)).Should().Be(0);

        Int20T larger = bounded + 1;
        (larger > value).Should().BeTrue();
        (value < larger).Should().BeTrue();
    }

    [Theory]
    [InlineData(Int20T.MaxValueConstant)]
    public void Comparison_WithInt64_AtMaxValue_ShouldWorkCorrectly(long value)
    {
        Int20T bounded = (int)value;
        
        Int20T smaller = bounded - 1;
        (smaller < value).Should().BeTrue();
        (value > smaller).Should().BeTrue();

        (bounded.CompareTo(value)).Should().Be(0);
    }

    [Theory]
    [InlineData(Int20T.MinValueConstant)]
    public void Comparison_WithInt64_AtMinValue_ShouldWorkCorrectly(long value)
    {
        Int20T bounded = (int)value;
        
        (bounded.CompareTo(value)).Should().Be(0);

        Int20T larger = bounded + 1;
        (larger > value).Should().BeTrue();
        (value < larger).Should().BeTrue();
    }

    [Theory]
    [InlineData(ulong.MaxValue)]
    [InlineData((ulong)Int20T.MaxValueConstant + 1)]
    public void Comparison_WithUInt64_AboveMaxValue_ShouldWorkCorrectly(ulong value)
    {
        Int20T bounded = Int20T.MaxValue;
        
        (bounded <= value).Should().BeTrue();
        (value >= bounded).Should().BeTrue();
        
        Int20T smaller = bounded - 1;
        (smaller < value).Should().BeTrue();
        (value > smaller).Should().BeTrue();
    }

    [Theory]
    [InlineData(0UL)]
    [InlineData(1UL)]
    public void Comparison_WithUInt64_WellBelowMaxValue_ShouldWorkCorrectly(ulong value)
    {
        Int20T bounded = (int)value;
        
        Int20T smaller = bounded - 1;
        (smaller < value).Should().BeTrue();
        (value > smaller).Should().BeTrue();

        (bounded.CompareTo(value)).Should().Be(0);

        Int20T larger = bounded + 1;
        (larger > value).Should().BeTrue();
        (value < larger).Should().BeTrue();
    }

    [Theory]
    [InlineData((ulong)Int20T.MaxValueConstant)]
    public void Comparison_WithUInt64_AtMaxValue_ShouldWorkCorrectly(ulong value)
    {
        Int20T bounded = (int)value;
        
        Int20T smaller = bounded - 1;
        (smaller < value).Should().BeTrue();
        (value > smaller).Should().BeTrue();

        (bounded.CompareTo(value)).Should().Be(0);
    }
}
