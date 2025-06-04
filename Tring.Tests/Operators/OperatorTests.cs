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
    public void CeilIsNotZero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.CeilIsNotZero, UnaryOperation.CeilIsNotZero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
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
    public void CeilIsNotPositive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.CeilIsNotPositive, UnaryOperation.CeilIsNotPositive, input, expectedValue);
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
    [InlineData(-1, 0)]
    [InlineData(0, 1)]
    [InlineData(1, -1)]
    public void CyclicIncrement_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.CyclicIncrement, UnaryOperation.CyclicIncrement, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void FloorIsZero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.FloorIsZero, UnaryOperation.FloorIsZero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    public void Increment_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.Increment, UnaryOperation.Increment, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, -1)]
    [InlineData(1, -1)]
    public void IsNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.IsNegative, UnaryOperation.IsNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, -1)]
    [InlineData(1, 0)]
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
    [InlineData(1, 0)]
    public void FloorIsNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(Operator.FloorIsNegative, UnaryOperation.FloorIsNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 0)]
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

    [Fact]
    public void Notation()
    {
        Trit NotNegative(Trit t) => t != Trit.Negative;
        var isNotNegativeResult1 = Trit.Positive | NotNegative;
        var isNotNegativeResult2 = Trit.Positive | Operator.IsNotNegative; // or use ```static Import Tring.Operators.Operator;````
        var isNotNegativeResult3 = Trit.Positive | [Trit.Negative, Trit.Positive, Trit.Positive];

        Trit IsNotEqualTo(Trit t1, Trit t2) => t1 != t2; 
        Trit[,] isNotEqualTo = new Trit[3, 3]
        {
            { false, true, true }, 
            { true, false, true }, 
            { true, true, false } 
        };
        var value1 = Trit.Negative;
        var value2 = Trit.Positive;
        var isEqualResult1 = Trit.Positive | IsNotEqualTo | Trit.Negative;
        var isEqualResult2 = Trit.Positive | isNotEqualTo | Trit.Negative;
        var isEqualResult3 = -(Trit.Positive|Operator.IsEqualTo | Trit.Negative); // or use ```static Import Tring.Operators.Operator;```.
        
        isNotNegativeResult1.Should().Be(Trit.Positive);
        isNotNegativeResult2.Should().Be(Trit.Positive);
        isNotNegativeResult3.Should().Be(Trit.Positive);
        isEqualResult1.Should().Be(Trit.Positive);
        isEqualResult2.Should().Be(Trit.Positive);
        isEqualResult3.Should().Be(Trit.Positive);
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

