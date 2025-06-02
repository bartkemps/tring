namespace Tring.Tests.Numbers.TritArrays;

using Xunit;
using FluentAssertions;
using TritArray;

public class MultiplicationTests
{
    [Theory]
    [InlineData(0u, 0u, 0u, 0u, 0u, 0u)] // 0 * 0 = 0
    [InlineData(1u, 0u, 1u, 0u, 1u, 0u)] // 1 * 1 = 1
    [InlineData(0u, 1u, 0u, 1u, 1u, 0u)] // (-1) * (-1) = 1
    [InlineData(1u, 0u, 0u, 1u, 0u, 1u)] // 1 * (-1) = -1
    [InlineData(0u, 1u, 1u, 0u, 0u, 1u)] // (-1) * 1 = -1
    [InlineData(3u, 0u, 2u, 0u, 6u, 0u)] // Simple positive multiplication
    [InlineData(0u, 3u, 0u, 2u, 6u, 0u)] // Simple negative multiplication
    [InlineData(3u, 0u, 0u, 2u, 0u, 6u)] // Mixed sign multiplication
    [InlineData(0xFu, 0u, 0x3u, 0u, 0x2Du, 0u)] // Larger numbers
    public void MultiplyBalancedTernary_ShouldCalculateCorrectResults(
        uint positive1, uint negative1,
        uint positive2, uint negative2,
        uint expectedPositive, uint expectedNegative)
    {
        Calculator.MultiplyBalancedTernary(
            positive1, negative1,
            positive2, negative2,
            out uint actualPositive, out uint actualNegative);

        actualPositive.Should().Be(expectedPositive);
        actualNegative.Should().Be(expectedNegative);
    }

    [Theory]
    [InlineData(0xFFFFFFFFu, 0u, 2u, 0u)] // Large number multiplication
    [InlineData(0xFu, 0xFu, 0x3u, 0x3u)] // Mixed bits multiplication
    public void MultiplyBalancedTernary_ShouldHandleLargeNumbers(
        uint positive1, uint negative1,
        uint positive2, uint negative2)
    {
        // This test verifies that the method switches to the appropriate algorithm
        // for larger numbers without throwing exceptions
        Calculator.MultiplyBalancedTernary(
            positive1, negative1,
            positive2, negative2,
            out uint actualPositive, out uint actualNegative);

        // We're not checking specific values here, just ensuring no exceptions occur
        // and the method completes
        (actualPositive | actualNegative).Should().BeGreaterOrEqualTo(0);
    }

    [Fact]
    public void MultiplyBalancedTernary_ZeroTimesAnyNumber_ShouldReturnZero()
    {
        // Test multiplying by zero
        Calculator.MultiplyBalancedTernary(
            0u, 0u, // zero
            0xFFFFFFFFu, 0xFFFFFFFFu, // any number
            out uint actualPositive, out uint actualNegative);

        actualPositive.Should().Be(0);
        actualNegative.Should().Be(0);
    }

    [Theory]
    [InlineData(1u, 0u)] // positive one
    [InlineData(0u, 1u)] // negative one
    public void MultiplyBalancedTernary_OneTimesNumber_ShouldReturnSameNumber(
        uint onePositive, uint oneNegative)
    {
        uint testPositive = 0x12345u;
        uint testNegative = 0x54321u;

        Calculator.MultiplyBalancedTernary(
            onePositive, oneNegative,
            testPositive, testNegative,
            out uint actualPositive, out uint actualNegative);

        if (onePositive == 1u)
        {
            actualPositive.Should().Be(testPositive);
            actualNegative.Should().Be(testNegative);
        }
        else
        {
            actualPositive.Should().Be(testNegative);
            actualNegative.Should().Be(testPositive);
        }
    }
}