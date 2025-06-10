namespace Ternary3.Tests.Numbers.TritArrays;

using Xunit;
using FluentAssertions;
using Ternary3.TritArrays;

public class MultiplicationTests
{
    [Theory]
    [InlineData(0u, 0u, 0u, 0u, 0u, 0u)] // 0 * 0 = 0
    [InlineData(1u, 0u, 1u, 0u, 0u, 1u)] // -1 * -1 = 1
    [InlineData(0u, 1u, 0u, 1u, 0u, 1u)] // 1 * 1 = 1
    public void MultiplyBalancedTernary_ShouldCalculateCorrectResults(uint negative1,
        uint positive1, uint negative2,
        uint positive2, uint expectedNegative,
        uint expectedPositive)
    {
        Calculator.MultiplyBalancedTernary(negative1,
            positive1, negative2,
            positive2, out var actualNegative, out var actualPositive);

        actualPositive.Should().Be(expectedPositive);
        actualNegative.Should().Be(expectedNegative);
    }

    [Theory]
    [InlineData(10000, 11000)] // Large number multiplication
    [InlineData(-20000, 21000)] // Mixed bits multiplication
    public void MultiplyBalancedTernary_ShouldHandleLargeNumbers(int value1, int value2)
    {
        var expectedValue = value1 * value2;
        TritConverter.ConvertTo32Trits(value1, out var negative1, out var positive1);
        TritConverter.ConvertTo32Trits(value2, out var negative2, out var positive2);

        Calculator.MultiplyBalancedTernary(
            negative1, positive1,
            negative2, positive2,
            out var actualNegative, out var actualPositive);

        var actualValue = TritConverter.TritsToInt32(actualNegative, actualPositive);
        actualValue.Should().Be(expectedValue);
    }

    [Fact]
    public void MultiplyBalancedTernary_ZeroTimesAnyNumber_ShouldReturnZero()
    {
        // Test multiplying by zero
        Calculator.MultiplyBalancedTernary(0u, // zero
            0u, 0xFFFFFFFFu, // any number
            0xFFFFFFFFu, out var actualNegative, out var actualPositive);

        actualPositive.Should().Be(0);
        actualNegative.Should().Be(0);
    }

    [Theory]
    [InlineData(0b101010, 0b010101)]
    [InlineData(0b110011, 0b001100)]
    public void MultiplyBalancedTernary_OneTimesNumber_ShouldReturnSameNumber(uint negative, uint positive)
    {
        var onePositive = 1u;
        var oneNegative = 0u;

        Calculator.MultiplyBalancedTernary(
            oneNegative, onePositive,
            negative, positive,
            out var actualNegative, out var actualPositive);

        actualPositive.Should().Be(positive);
        actualNegative.Should().Be(negative);
        
        Calculator.MultiplyBalancedTernary(
            negative, positive,
            oneNegative, onePositive,
            out actualNegative, out actualPositive);

        actualPositive.Should().Be(positive);
        actualNegative.Should().Be(negative);
    }
}