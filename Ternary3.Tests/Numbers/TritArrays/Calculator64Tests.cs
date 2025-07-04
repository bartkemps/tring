namespace Ternary3.Tests.Numbers.TritArrays;

using FluentAssertions;
using Ternary3.TritArrays;

public class Calculator64Tests
{
    [Theory]
    [InlineData(0UL, 0UL, 0UL, 0UL, 0UL, 0UL)]  // 0 + 0 = 0
    [InlineData(0UL, 1UL, 0UL, 1UL, 1UL, 2UL)]  // 1 + 1 = 2
    [InlineData(1UL, 0UL, 0UL, 1UL, 0UL, 0UL)]  // -1 + 1 = 0
    [InlineData(1UL, 0UL, 1UL, 0UL, 2UL, 1UL)]  // -1 + (-1) = -2
    [InlineData(ulong.MaxValue, 0, 0, ulong.MaxValue, 0UL, 0UL)]  // 4 + (-1) = 2
    [InlineData(0,ulong.MaxValue,  ulong.MaxValue,0, 0UL, 0UL)]  // 4 + (-1) = 2
    public void AddBalancedTernary_ShouldAddCorrectly(
        ulong neg1, ulong pos1, 
        ulong neg2, ulong pos2, 
        ulong expectedNeg, ulong expectedPos)
    {
        Calculator.AddBalancedTernary(neg1, pos1, neg2, pos2, out var actualNeg, out var actualPos);
        
        actualNeg.Should().Be(expectedNeg);
        actualPos.Should().Be(expectedPos);
    }

    [Theory]
    [InlineData(0UL, 0UL, 0UL, 0UL, 0UL, 0UL)]  // 0 * 0 = 0
    [InlineData(0UL, 1UL, 0UL, 1UL, 0UL, 1UL)]  // 1 * 1 = 1
    [InlineData(1UL, 0UL, 1UL, 0UL, 0UL, 1UL)]  // -1 * -1 = 1
    [InlineData(1UL, 0UL, 0UL, 1UL, 1UL, 0UL)]  // -1 * 1 = -1
    [InlineData(0UL, 0b100UL, 0UL, 0b100UL, 0UL, 0b10000UL)]  // 3 * 3 = 9
    public void MultiplyBalancedTernary_ShouldMultiplyCorrectly(
        ulong neg1, ulong pos1,
        ulong neg2, ulong pos2,
        ulong expectedNeg, ulong expectedPos)
    {
        Calculator.MultiplyBalancedTernary(neg1, pos1, neg2, pos2, out var actualNeg, out var actualPos);

        actualNeg.Should().Be(expectedNeg);
        actualPos.Should().Be(expectedPos);
    }

    [Theory]
    [InlineData(1L)]
    [InlineData(-1L)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    [InlineData(100L)]
    [InlineData(-100L)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void MultiplyBalancedTernary_LargeNumbers_ShouldPreserveOriginalValueInRoundTrip(long value)
    {
        // Convert the value to balanced ternary
        TritConverter.To64Trits(value, out var neg1, out var pos1);
        
        // Multiply by 1
        Calculator.MultiplyBalancedTernary(neg1, pos1, 0UL, 1UL, out var resultNeg, out var resultPos);
        
        // Convert back and verify
        var result = TritConverter.ToInt64(resultNeg, resultPos);
        result.Should().Be(value);
    }
}
