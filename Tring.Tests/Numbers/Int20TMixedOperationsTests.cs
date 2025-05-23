using FluentAssertions;
using Tring.Numbers;
using Xunit;

namespace Tring.Tests.Numbers;

public class Int20TMixedOperationsTests
{
    [Theory]
    [InlineData(5, 3, 8)]
    [InlineData(0, 7, 7)]
    [InlineData(-5, 10, 5)]
    [InlineData(1743392190, 5, 1743392195)]  // Near max value
    [InlineData(-1743392190, -5, -1743392195)] // Near min value
    public void Addition_WithInt_ShouldReturnCorrectResult(int a, int b, int expected)
    {
        // IntT20 + int
        Int20T intT20 = a;
        int intValue = b;
        Int20T result1 = intT20 + intValue;
        result1.Should().Be((Int20T)expected);

        // int + IntT20
        result1 = intValue + intT20;
        result1.Should().Be((Int20T)expected);
    }

    [Theory]
    [InlineData(5, 3, 2)]
    [InlineData(0, 7, -7)]
    [InlineData(-5, 10, -15)]
    [InlineData(10, 5, 5)]
    [InlineData(-10, -5, -5)]
    [InlineData(1743392190, 5, 1743392185)]  // Near max value
    [InlineData(-1743392190, -5, -1743392185)] // Near min value
    public void Subtraction_WithInt_ShouldReturnCorrectResult(int a, int b, int expected)
    {
        // IntT20 - int
        Int20T intT20 = a;
        int intValue = b;
        Int20T result1 = intT20 - intValue;
        result1.Should().Be((Int20T)expected);

        // int - IntT20
        result1 = intValue - intT20;
        result1.Should().Be((Int20T)(-expected)); // Reversed order inverts the sign
    }

    [Theory]
    [InlineData(5, 3, 15)]
    [InlineData(0, 7, 0)]
    [InlineData(-5, 10, -50)]
    [InlineData(10, -5, -50)]
    [InlineData(-10, -5, 50)]
    [InlineData(173439, 10, 1734390)]  // Large value
    [InlineData(-173439, 10, -1734390)] // Large negative value
    public void Multiplication_WithInt_ShouldReturnCorrectResult(int a, int b, int expected)
    {
        // IntT20 * int
        Int20T intT20 = a;
        int intValue = b;
        Int20T result1 = intT20 * intValue;
        result1.Should().Be((Int20T)expected);

        // int * IntT20
        result1 = intValue * intT20;
        result1.Should().Be((Int20T)expected);
    }

    [Theory]
    [InlineData(15, 3, 5)]
    [InlineData(0, 7, 0)]
    [InlineData(-15, 3, -5)]
    [InlineData(15, -3, -5)]
    [InlineData(-15, -3, 5)]
    [InlineData(173439, 13, 13341)]  // Large value
    public void Division_WithInt_ShouldReturnCorrectResult(int a, int b, int expected)
    {
        // IntT20 / int
        Int20T intT20 = a;
        int intValue = b;
        Int20T result1 = intT20 / intValue;
        result1.Should().Be((Int20T)expected);

        // int / IntT20
        if (a != 0) // Avoid divide by zero
        {
            result1 = intValue / intT20;
            result1.Should().Be((Int20T)(intValue / a));
        }
    }

    [Theory]
    [InlineData(17, 5, 2)]
    [InlineData(0, 7, 0)]
    [InlineData(-17, 5, -2)]
    [InlineData(17, -5, 2)]
    [InlineData(-17, -5, -2)]
    [InlineData(173438, 10000, 3438)]  // Large value - 173438 % 10000 = 3438
    public void Modulo_WithInt_ShouldReturnCorrectResult(int a, int b, int expected)
    {
        // IntT20 % int
        Int20T intT20 = a;
        int intValue = b;
        Int20T result1 = intT20 % intValue;
        result1.Should().Be((Int20T)expected);

        // int % IntT20
        if (a != 0) // Avoid divide by zero
        {
            result1 = intValue % intT20;
            result1.Should().Be((Int20T)(intValue % a));
        }
    }

    [Fact]
    public void Division_WithZero_ShouldThrowDivideByZeroException()
    {
        // IntT20 / 0
        Int20T intT20 = 10;
        Action action = () => _ = intT20 / 0;
        action.Should().Throw<DivideByZeroException>();

        // int / IntT20(0)
        Int20T zero = 0;
        action = () => _ = 10 / zero;
        action.Should().Throw<DivideByZeroException>();
    }

    [Fact]
    public void Modulo_WithZero_ShouldThrowDivideByZeroException()
    {
        // IntT20 % 0
        Int20T intT20 = 10;
        Action action = () => _ = intT20 % 0;
        action.Should().Throw<DivideByZeroException>();

        // int % IntT20(0)
        Int20T zero = 0;
        action = () => _ = 10 % zero;
        action.Should().Throw<DivideByZeroException>();
    }
}
