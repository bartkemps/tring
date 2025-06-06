namespace Ternary3.Tests.Operators;

using FluentAssertions;
using Ternary3.Numbers;
using Ternary3.Operators;

public class OperatorTests
{

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, -1)]
    [InlineData(1, -1)]
    public void Negative_ExecutesCorrectly(sbyte input, sbyte expectedValue)

    {
        OperationExecutesCorrectly(UnaryLookup.Negative, Unary.Negative, input, expectedValue);
    }
    
    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, -1)]
    [InlineData(1, 0)]
    public void Decrement_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.Decrement, Unary.Decrement, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, -1)]
    [InlineData(1, 1)]
    public void IsPositive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.IsPositive, Unary.IsPositive, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 0)]
    [InlineData(1, -1)]
    public void NegateAbsoluteValue_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.NegateAbsoluteValue, Unary.NegateAbsoluteValue, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    public void Ceil_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.Ceil, Unary.Ceil, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    public void Identity_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.Identity, Unary.Identity, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 1)]
    [InlineData(1, -1)]
    public void IsZero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.IsZero, Unary.IsZero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void KeepNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.KeepNegative, Unary.KeepNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, -1)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    public void IsNotNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.IsNotNegative, Unary.IsNotNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(1, -1)]
    public void CeilIsNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.CeilIsNegative, Unary.CeilIsNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(1, 0)]
    public void CeilIsNotZero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.CeilIsNotZero, Unary.CeilIsNotZero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(1, 1)]
    public void KeepPositive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.KeepPositive, Unary.KeepPositive, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 0)]
    [InlineData(1, -1)]
    public void CeilIsNotPositive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.CeilIsNotPositive, Unary.CeilIsNotPositive, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    public void Zero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.Zero, Unary.Zero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    public void Floor_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.Floor, Unary.Floor, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 1)]
    [InlineData(1, -1)]
    public void CyclicIncrement_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.CyclicIncrement, Unary.CyclicIncrement, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void FloorIsZero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.FloorIsZero, Unary.FloorIsZero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    public void Increment_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.Increment, Unary.Increment, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, -1)]
    [InlineData(1, -1)]
    public void IsNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.IsNegative, Unary.IsNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, -1)]
    [InlineData(1, 0)]
    public void CyclicDecrement_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.CyclicDecrement, Unary.CyclicDecrement, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, -1)]
    [InlineData(1, 1)]
    public void IsNotZero_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.IsNotZero, Unary.IsNotZero, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 0)]
    [InlineData(1, -1)]
    public void Negate_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.Negate, Unary.Negate, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 0)]
    [InlineData(1, 0)]
    public void FloorIsNegative_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.FloorIsNegative, Unary.FloorIsNegative, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    public void AbsoluteValue_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.AbsoluteValue, Unary.AbsoluteValue, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 1)]
    [InlineData(1, -1)]
    public void IsNotPositive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.IsNotPositive, Unary.IsNotPositive, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void FloorIsNotPositive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.FloorIsNotPositive, Unary.FloorIsNotPositive, input, expectedValue);
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    public void Positive_ExecutesCorrectly(sbyte input, sbyte expectedValue)
    {
        OperationExecutesCorrectly(UnaryLookup.Positive, Unary.Positive, input, expectedValue);
    }

    [Fact]
    public void Notation()
    {
        Trit NotNegative(Trit t) => t != Trit.Negative;
        var isNotNegativeResult1 = Trit.Positive | NotNegative;
        var isNotNegativeResult2 = Trit.Positive | UnaryLookup.IsNotNegative; // or use ```static Import Ternary3.Operators.Operator;````
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
        var isEqualResult3 = -(Trit.Positive|BinaryLookup.Is | Trit.Negative); // or use ```static Import Ternary3.Operators.Operator;```.
        
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

