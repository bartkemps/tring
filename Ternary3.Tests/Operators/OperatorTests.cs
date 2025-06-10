namespace Ternary3.Tests.Operators;

using FluentAssertions;
using Ternary3.Operators;

public class OperatorTests
{

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, -1)]
    [InlineData(1, -1)]
    public void Negative_ExecutesCorrectly(sbyte input, sbyte expectedValue)

    {
        OperationExecutesCorrectly(UnaryTritOperator.Negative, input, expectedValue);
    }
    
    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, -1)]
    [InlineData(1, 0)]
    public void Decrement_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.Decrement, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, -1)]
    [InlineData(1, 1)]
    public void IsPositive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.IsPositive, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 0)]
    [InlineData(1, -1)]
    public void NegateAbsoluteValue_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.NegateAbsoluteValue, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    public void Ceil_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.Ceil, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    public void Identity_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.Identity, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 1)]
    [InlineData(1, -1)]
    public void IsZero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.IsZero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void KeepNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.KeepNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    public void IsNotNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.IsNotNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(1, -1)]
    public void CeilIsNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.CeilIsNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(1, 0)]
    public void CeilIsNotZero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.CeilIsNotZero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(1, 1)]
    public void KeepPositive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.KeepPositive, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 0)]
    [InlineData(1, -1)]
    public void CeilIsNotPositive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.CeilIsNotPositive, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    public void Zero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.Zero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    public void Floor_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.Floor, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 1)]
    [InlineData(1, -1)]
    public void CyclicIncrement_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.CyclicIncrement, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void FloorIsZero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.FloorIsZero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    public void Increment_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.Increment, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, -1)]
    [InlineData(1, -1)]
    public void IsNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.IsNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, -1)]
    [InlineData(1, 0)]
    public void CyclicDecrement_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.CyclicDecrement, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, -1)]
    [InlineData(1, 1)]
    public void IsNotZero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.IsNotZero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 0)]
    [InlineData(1, -1)]
    public void Negate_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.Negate, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    public void FloorIsNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.FloorIsNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    public void AbsoluteValue_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.AbsoluteValue, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 1)]
    [InlineData(1, -1)]
    public void IsNotPositive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.IsNotPositive, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void FloorIsNotPositive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.FloorIsNotPositive, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    public void Positive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryTritOperator.Positive, input, expectedValue);
    }

    [Fact]
    public void Notation()
    {
        Trit NotNegative(Trit t) => t != Trit.Negative;
        var isNotNegativeResult1 = Trit.Positive | NotNegative;
        var isNotNegativeResult2 = Trit.Positive | UnaryTritOperator.IsNotNegative; // or use ```static Import Ternary3.Operators.Operator;````
        var isNotNegativeResult3 = Trit.Positive | [Trit.Negative, Trit.Positive, Trit.Positive];

        Trit IsNotEqualTo(Trit t1, Trit t2) => t1 != t2; 
        var isNotEqualTo = new Trit[3, 3]
        {
            { false, true, true }, 
            { true, false, true }, 
            { true, true, false } 
        };
        var value1 = Trit.Negative;
        var value2 = Trit.Positive;
        var isEqualResult1 = Trit.Positive | IsNotEqualTo | Trit.Negative;
        var isEqualResult2 = Trit.Positive | isNotEqualTo | Trit.Negative;
        var isEqualResult3 = -(Trit.Positive|BinaryTritOperator.Is | Trit.Negative); // or use ```static Import Ternary3.Operators.Operator;```.
        
        isNotNegativeResult1.Should().Be(Trit.Positive);
        isNotNegativeResult2.Should().Be(Trit.Positive);
        isNotNegativeResult3.Should().Be(Trit.Positive);
        isEqualResult1.Should().Be(Trit.Positive);
        isEqualResult2.Should().Be(Trit.Positive);
        isEqualResult3.Should().Be(Trit.Positive);
    }

    private void OperationExecutesCorrectly(UnaryTritOperator operation, sbyte input, sbyte expectedValue)
    {
        var array = new TritArray27();
        var trit = (Trit)input;
        var expected = (Trit)expectedValue;


        var actual = trit | operation;
        actual.Should().Be(expected);

        array[3] = trit;
        var result = (array | operation)[3];
        result.Should().Be(expected);
    }
}

