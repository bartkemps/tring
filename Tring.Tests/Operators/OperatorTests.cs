namespace Tring.Tests.Operators;

using FluentAssertions;
using Tring.Numbers;
using Tring.Operators;

public class OperatorTests
{

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, -1)]
    [InlineData(1, -1)]
    public void Negative_ExecutesCorrectly(sbyte input, sbyte expectedValue)

    {
        OperationExecutesCorrectly(Operator.Negative, UnaryOperation.Negative, input, expectedValue);
    }
    
    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, -1)]
    [InlineData(1, 0)]
    public void Decrement_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.Decrement, UnaryOperation.Decrement, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, -1)]
    [InlineData(1, 1)]
    public void IsPositive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.IsPositive, UnaryOperation.IsPositive, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 0)]
    [InlineData(1, -1)]
    public void NegateAbsoluteValue_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.NegateAbsoluteValue, UnaryOperation.NegateAbsoluteValue, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    public void Ceil_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.Ceil, UnaryOperation.Ceil, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    public void Identity_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.Identity, UnaryOperation.Identity, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 1)]
    [InlineData(1, -1)]
    public void IsZero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.IsZero, UnaryOperation.IsZero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void KeepNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.KeepNegative, UnaryOperation.KeepNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    public void IsNotNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.IsNotNegative, UnaryOperation.IsNotNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(1, -1)]
    public void CeilIsNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.CeilIsNegative, UnaryOperation.CeilIsNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(1, 0)]
    public void IsNotZeroCeil_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.IsNotZeroCeil, UnaryOperation.IsNotZeroCeil, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, -1)]
    [InlineData(1, 1)]
    public void KeepPositive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.KeepPositive, UnaryOperation.KeepPositive, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 0)]
    [InlineData(1, -1)]
    public void IsNotPositiveCeil_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.IsNotPositiveCeil, UnaryOperation.IsNotPositiveCeil, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    public void Zero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.Zero, UnaryOperation.Zero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    public void Floor_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.Floor, UnaryOperation.Floor, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    public void CyclicIncrement_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.CyclicIncrement, UnaryOperation.CyclicIncrement, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    public void FloorIsZero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.FloorIsZero, UnaryOperation.FloorIsZero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    public void Increment_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.Increment, UnaryOperation.Increment, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    public void IsNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.IsNegative, UnaryOperation.IsNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 0)]
    [InlineData(1, -1)]
    public void CyclicDecrement_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.CyclicDecrement, UnaryOperation.CyclicDecrement, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, -1)]
    [InlineData(1, 1)]
    public void IsNotZero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.IsNotZero, UnaryOperation.IsNotZero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 0)]
    [InlineData(1, -1)]
    public void Negate_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.Negate, UnaryOperation.Negate, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    public void FloorIsNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.FloorIsNegative, UnaryOperation.FloorIsNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    public void AbsoluteValue_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.AbsoluteValue, UnaryOperation.AbsoluteValue, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 1)]
    [InlineData(1, -1)]
    public void IsNotPositive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.IsNotPositive, UnaryOperation.IsNotPositive, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void FloorIsNotPositive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.FloorIsNotPositive, UnaryOperation.FloorIsNotPositive, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    public void Positive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.Positive, UnaryOperation.Positive, input, expectedValue);
    }

    private void OperationExecutesCorrectly(Trit[] table, Func<Trit, Trit> operation, sbyte input, sbyte expectedValue)
    {
        var array = new TritArray27();
        var trit = (Trit)input;
        var expected = (Trit)expectedValue;

        var actual = trit | table;
        actual.Should().Be(expected);

        actual = trit | operation;
        actual.Should().Be(expected);

        array[1] = trit;
        var result = (array | table)[1];
        result.Should().Be(expected);

        array[1] = trit;
        result = (array | operation)[1];
        result.Should().Be(expected);
    }
}

