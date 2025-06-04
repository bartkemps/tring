namespace Tring.Tests.Numbers.TritArrays;

using FluentAssertions;
using Tring.TritArray;
using Tring.Numbers.TritArrays;

public class Calculator64Tests
{
    [Theory]
    [InlineData(0UL, 0UL, 0UL, 0UL, 0UL, 0UL)]  // 0 + 0 = 0
    [InlineData(0UL, 1UL, 0UL, 1UL, 1UL, 2UL)]  // 1 + 1 = 2
    [InlineData(1UL, 0UL, 0UL, 1UL, 0UL, 0UL)]  // -1 + 1 = 0
    [InlineData(1UL, 0UL, 1UL, 0UL, 2UL, 1UL)]  // -1 + (-1) = -2
    [InlineData(0UL, 3UL, 1UL, 0UL, 0UL, 2UL)]  // 4 + (-1) = 2
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
    public void MultiplyBalancedTernary_LargeNumbers_ShouldPreserveOriginalValueInRoundTrip(long value)
    {
        // Convert the value to balanced ternary
        TritConverter.ConvertTo64Trits(value, out var neg1, out var pos1);
        
        // Multiply by 1
        Calculator.MultiplyBalancedTernary(neg1, pos1, 0UL, 1UL, out var resultNeg, out var resultPos);
        
        // Convert back and verify
        var result = TritConverter.TritsToInt64(resultNeg, resultPos);
        result.Should().Be(value);
    }
}
