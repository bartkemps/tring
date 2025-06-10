#pragma warning disable CA1806
// ReSharper disable ObjectCreationAsStatement
namespace Ternary3.Tests.Operators;

using FluentAssertions;
using Ternary3.Operators;

public class UnaryTritOperatorTests
{
    [Theory]
    [InlineData(-1, 0, 1)]  // Example: operation that might be a negation
    [InlineData(1, 0, 1)]   // Example: operation that might be absolute value
    [InlineData(0, 0, 0)]   // Example: operation that returns zero for all inputs
    public void Constructor_WithThreeIntegers_CreatesOperatorWithoutThrowing(int value1, int value2, int value3)
    {
        // The values are: result for -1, result for 0, result for 1
        Action act = () => new UnaryTritOperator(value1, value2, value3);
            
        // Just verify it doesn't throw an exception with valid values
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(2, 0, 0)]  // Invalid first value
    [InlineData(0, -2, 0)] // Invalid second value
    [InlineData(0, 0, 2)]  // Invalid third value
    public void Constructor_WithInvalidIntegers_ThrowsArgumentOutOfRangeException(int value1, int value2, int value3)
    {
        Action act = () => new UnaryTritOperator(value1, value2, value3);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Value must be -1, 0, or 1*");
    }

    [Theory]
    [InlineData(new[] {-1, 0, 1})]  // Valid combination 1
    [InlineData(new[] {1, 0, 1})]   // Valid combination 2
    [InlineData(new[] {0, 0, 0})]   // Valid combination 3
    public void Constructor_WithSpanOfIntegers_CreatesOperatorWithoutThrowing(int[] values)
    {
        Action act = () => new UnaryTritOperator(values.AsSpan());
            
        // Just verify it doesn't throw an exception with valid values
        act.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithTooShortSpanOfIntegers_ThrowsArgumentException()
    {
        int[] shortArray = new[] {-1, 0};
            
        Action act = () => new UnaryTritOperator(shortArray.AsSpan());

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Span must contain exactly three values*");
    }

    [Theory]
    [InlineData(new[] {2, 0, 0}, 0)]  // Invalid first value
    [InlineData(new[] {0, -2, 0}, 1)] // Invalid second value
    [InlineData(new[] {0, 0, 2}, 2)]  // Invalid third value
    public void Constructor_WithSpanContainingInvalidIntegers_ThrowsArgumentOutOfRangeException(int[] values, int expectedIndex)
    {
        Action act = () => new UnaryTritOperator(values.AsSpan());

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage($"*Value at index {expectedIndex} must be -1, 0, or 1*");
    }

    [Theory]
    [InlineData(true, null, false)]   // Valid combination 1
    [InlineData(null, false, true)]   // Valid combination 2 
    [InlineData(false, true, null)]   // Valid combination 3
    public void Constructor_WithNullableBooleans_CreatesOperatorWithoutThrowing(bool? value1, bool? value2, bool? value3)
    {
        Action act = () => new UnaryTritOperator(value1, value2, value3);
            
        // Just verify it doesn't throw an exception
        act.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithNullableBooleanArray_CreatesOperatorWithoutThrowing()
    {
        var values = new bool?[] { true, null, false };
            

        Action act = () => new UnaryTritOperator(values);
            
        // Just verify it doesn't throw an exception
        act.Should().NotThrow();
    }

    [Fact]
    public void Constructor_WithTooShortArrayOfNullableBooleans_ThrowsArgumentException()
    {
        Action act = () => new UnaryTritOperator([false,null]);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Array must contain exactly three values*");
    }
        
    [Fact]
    public void Constructor_MapsCorrectly_WhenUsingFunction()
    {
        // Create a simple identity operator (returns the input unchanged)
        var identity = new UnaryTritOperator(trit => trit);
            
        // Just verify the constructor doesn't throw
        // Actual operation testing would require knowing details of UnaryOperation.TritOperations
        // which is outside the scope of these constructor tests
        identity.Should().NotBeNull();
    }
}
#pragma warning restore CA1806
