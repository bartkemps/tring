namespace Tring.Tests.Numbers.TritArrays;

using FluentAssertions;
using Tring.Numbers.TritArrays;

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
    public void ConvertToTrits_ShouldProduceCorrectTrits(int value, uint expectedNegative, uint expectedPositive)
    {
        TritConverter.ConvertTo32Trits(value, out var negative, out var positive);
        negative.Should().Be(expectedNegative);
        positive.Should().Be(expectedPositive);
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

    public void ConvertTo64Trits_ShouldProduceCorrectTrits(long value, ulong expectedNegative, ulong expectedPositive)
    {
        TritConverter.ConvertTo64Trits(value, out var negative, out var positive);
        negative.Should().Be(expectedNegative);
        positive.Should().Be(expectedPositive);
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
    public void ConvertToInt32_ShouldPreserveValue_ForUint32Input(int expectedvalue, uint negative, uint positive)
    {
        var actual = TritConverter.TritsToInt32(negative, positive);
        actual.Should().Be(expectedvalue);
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
    public void ConvertToInt64_ShouldPreserveValue_ForUint32Input(long expectedValue, uint negative, uint positive)
    {
        var actual = TritConverter.TritsToInt64(negative, positive);
        actual.Should().Be(expectedValue);
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
    public void ConvertToInt64_ShouldPreserveValue_ForUint64Input(long expectedValue, ulong negative, ulong positive)
    {
        var actual = TritConverter.TritsToInt64(negative, positive);
        actual.Should().Be(expectedValue);
    }
}