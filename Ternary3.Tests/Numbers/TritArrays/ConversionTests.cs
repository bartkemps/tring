namespace Ternary3.Tests.Numbers.TernaryArrays;

using FluentAssertions;
using Ternary3.TernaryArrays;

public class ConversionTests
{
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 0, 1)]
    [InlineData(-1, 1, 0)]
    [InlineData(8, 0b1, 0b100)]
    [InlineData(-8, 0b100, 0b1)]
    [InlineData(1743392200, 0, 0b11111111111111111111)]
    [InlineData(-1743392200, 0b11111111111111111111, 0)]
    [InlineData(int.MinValue,1049046, 738857)]
    [InlineData(int.MaxValue,738856, 1049045)]
    public void ConvertToTrits_ShouldProduceCorrectTrits(int value, uint expectedNegative, uint expectedPositive)
    {
        TritConverter.To32Trits(value, out var negative, out var positive);
        negative.Should().Be(expectedNegative);
        positive.Should().Be(expectedPositive);
        TritConverter.ToInt32(negative, positive).Should().Be(value);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 0, 1)]
    [InlineData(-1, 1, 0)]
    [InlineData(8, 0b1, 0b100)]
    [InlineData(-8, 0b100, 0b1)]
    [InlineData(1743392200, 0, 0b11111111111111111111)]
    [InlineData(-1743392200, 0b11111111111111111111, 0)]
    [InlineData(int.MinValue,1049046, 738857)]
    [InlineData(int.MaxValue,738856, 1049045)]
    [InlineData(-6078832729528464400,1099511627775UL, 0)]
    [InlineData(6078832729528464400,0, 1099511627775UL)]
    [InlineData(long.MaxValue,687268435026UL, 1498450313609UL)]
    [InlineData(long.MinValue,1498450313608UL, 687268435025UL)]
    public void ConvertTo64Trits_ShouldProduceCorrectTrits(long value, ulong expectedNegative, ulong expectedPositive)
    {
        TritConverter.To64Trits(value, out var negative, out var positive);
        negative.Should().Be(expectedNegative);
        positive.Should().Be(expectedPositive);
        TritConverter.ToInt64(negative, positive).Should().Be(value);
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 0, 1)]
    [InlineData(-1, 1, 0)]
    [InlineData(8, 0b1, 0b100)]
    [InlineData(-8, 0b100, 0b1)]
    [InlineData(1743392200, 0, 0b11111111111111111111)]
    [InlineData(-1743392200, 0b11111111111111111111, 0)]
    [InlineData(int.MinValue,1049046, 738857)]
    [InlineData(int.MaxValue,738856, 1049045)]
    public void ConvertToInt32_ShouldPreserveValue_ForUint32Input(int expected, uint negative, uint positive)
    {
        var actual = TritConverter.ToInt32(negative, positive);
        actual.Should().Be(expected);
        TritConverter.To64Trits(actual, out var negative2, out var positive2);
        negative2.Should().Be(negative);
        positive2.Should().Be(positive);
    }
    
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 0, 1)]
    [InlineData(-1, 1, 0)]
    [InlineData(8, 0b1, 0b100)]
    [InlineData(-8, 0b100, 0b1)]
    [InlineData(1743392200, 0, 0b11111111111111111111)]
    [InlineData(-1743392200, 0b11111111111111111111, 0)]
    [InlineData(-926510094425920L,uint.MaxValue, 0)]
    [InlineData(926510094425920L,0, uint.MaxValue)]
    public void ConvertToInt64_ShouldPreserveValue_ForUint32Input(long expectedValue, uint negative, uint positive)
    {
        var actual = TritConverter.ToInt64(negative, positive);
        actual.Should().Be(expectedValue);
        TritConverter.To64Trits(actual, out var negative2, out var positive2);
        negative2.Should().Be(negative);
        positive2.Should().Be(positive);
    }
    
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 0, 1)]
    [InlineData(-1, 1, 0)]
    [InlineData(8, 0b1, 0b100)]
    [InlineData(-8, 0b100, 0b1)]
    [InlineData(1743392200, 0, 0b11111111111111111111)]
    [InlineData(-1743392200, 0b11111111111111111111, 0)]
    [InlineData(long.MaxValue,687268435026UL, 1498450313609UL)]
    [InlineData(long.MinValue,1498450313608UL, 687268435025UL)]
    public void ConvertToInt64_ShouldPreserveValue_ForUint64Input(long expectedValue, ulong negative, ulong positive)
    {
        var actual = TritConverter.ToInt64(negative, positive);
        actual.Should().Be(expectedValue);
        TritConverter.To64Trits(actual, out var negative2, out var positive2);
        negative2.Should().Be(negative);
        positive2.Should().Be(positive);
    }
}

