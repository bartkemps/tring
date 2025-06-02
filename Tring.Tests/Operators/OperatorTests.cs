namespace Tring.Tests.Operators;

using FluentAssertions;
using Tring.Numbers;
using Tring.Operators;

public class OperatorTests
{

    [Theory]
    [InlineData(-1, -1)]
    public void Negative_ExecutesCorrectly(sbyte value, sbyte expectedValue)

    {
        var array = new TritArray27();
        var trit = (Trit)value;
        var expected = (Trit)expectedValue;

        var actual = trit | Operator.Negative;
        actual.Should().Be(expected);

        actual = trit | UnaryOperation.Negative;
        actual.Should().Be(expected);

        array[1] = trit;
        var result = array | Operator.Negative;
        result[1].Should().Be(expected);

        array[1] = trit;
        result = array | UnaryOperation.Negative;
        result[1].Should().Be(expected);
    }
}