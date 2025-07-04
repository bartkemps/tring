using FluentAssertions;

// ReSharper disable EqualExpressionComparison
#pragma warning disable CS8629  
#pragma warning disable CS1718  

namespace Ternary3.Tests.Numbers;

using Ternary3.Operators;

public unsafe class TritTests
{
    private const sbyte T = -1;
    
    [Fact]
    public void StaticValues_ShouldHaveCorrectUnderlyingValues()
    {
        // Verify the correct underlying values
        ((sbyte)Trit.Positive).Should().Be(1);
        ((sbyte)Trit.Negative).Should().Be(-1);
        ((sbyte)Trit.Zero).Should().Be(0);

        // Verify int conversions also work
        ((int)Trit.Positive).Should().Be(1);
        ((int)Trit.Negative).Should().Be(-1);
        ((int)Trit.Zero).Should().Be(0);
    }

    [Fact]
    public void BooleanConversions_ShouldWorkCorrectly()
    {
        // Implicit conversions from bool? to Trit
        Trit positive = true;
        Trit negative = false;
        Trit zero = null as bool?;

        positive.Should().Be(Trit.Positive);
        negative.Should().Be(Trit.Negative);
        zero.Should().Be(Trit.Zero);

        // Implicit conversions from Trit to bool?
        bool? positiveNullable = Trit.Positive;
        bool? negativeNullable = Trit.Negative;
        bool? zeroNullable = Trit.Zero;

        positiveNullable.Should().BeTrue();
        negativeNullable.Should().BeFalse();
        zeroNullable.Should().BeNull();

        // Explicit conversions to bool (cannot cast Zero)
        ((bool)Trit.Positive).Should().BeTrue();
        ((bool)Trit.Negative).Should().BeFalse();
        Action zeroToBool = () => _ = (bool)Trit.Zero;
        zeroToBool.Should().Throw<InvalidOperationException>();

        // Testing the direct comparison with boolean values
        (Trit.Positive == true).Should().BeTrue();
        (Trit.Negative == false).Should().BeTrue();
        (Trit.Positive == false).Should().BeFalse();
        (Trit.Negative == true).Should().BeFalse();
    }

    [Fact]
    public void ExplicitConversions_ShouldWorkCorrectly()
    {
        // sbyte to Trit
        ((Trit)(sbyte)1).Should().Be(Trit.Positive);
        ((Trit)(sbyte)-1).Should().Be(Trit.Negative);
        ((Trit)(sbyte)0).Should().Be(Trit.Zero);

        // int to Trit
        ((Trit)1).Should().Be(Trit.Positive);
        ((Trit)(-1)).Should().Be(Trit.Negative);
        ((Trit)0).Should().Be(Trit.Zero);

        // Out of range conversions should throw
        Action outOfRangeSByte = () => _ = (Trit)(sbyte)2;
        outOfRangeSByte.Should().Throw<ArgumentOutOfRangeException>();

        Action outOfRangeInt = () => _ = (Trit)2;
        outOfRangeInt.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ToString_ShouldReturnCorrectStrings()
    {
        Trit.Positive.ToString().Should().Be("Positive");
        Trit.Negative.ToString().Should().Be("Negative");
        Trit.Zero.ToString().Should().Be("Zero");
    }

    [Fact]
    public void Equals_ShouldWorkCorrectly()
    {
        // Using equality operator
        (Trit.Positive == Trit.Positive).Should().BeTrue();
        (Trit.Negative == Trit.Negative).Should().BeTrue();
        (Trit.Zero == Trit.Zero).Should().BeTrue();
        (Trit.Positive == Trit.Negative).Should().BeFalse();
        (Trit.Positive == Trit.Zero).Should().BeFalse();
        (Trit.Negative == Trit.Zero).Should().BeFalse();

        // Using inequality operator
        (Trit.Positive != Trit.Negative).Should().BeTrue();
        (Trit.Positive != Trit.Zero).Should().BeTrue();
        (Trit.Negative != Trit.Zero).Should().BeTrue();
        (Trit.Positive != Trit.Positive).Should().BeFalse();
        (Trit.Negative != Trit.Negative).Should().BeFalse();
        (Trit.Zero != Trit.Zero).Should().BeFalse();

        // Using Equals method
        Trit.Positive.Equals(Trit.Positive).Should().BeTrue();
        Trit.Negative.Equals(Trit.Negative).Should().BeTrue();
        Trit.Zero.Equals(Trit.Zero).Should().BeTrue();
        Trit.Positive.Equals(Trit.Negative).Should().BeFalse();
        Trit.Positive.Equals(Trit.Zero).Should().BeFalse();
        Trit.Negative.Equals(Trit.Zero).Should().BeFalse();

        // Equals with non-Trit objects
        Trit.Positive.Equals(1).Should().BeFalse();
        Trit.Positive.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ShouldBeConsistent()
    {
        // Same values should have same hash codes
        var positiveClone = (Trit)1;
        Trit.Positive.GetHashCode().Should().Be(positiveClone.GetHashCode());

        // Different values should have different hash codes
        Trit.Positive.GetHashCode().Should().NotBe(Trit.Negative.GetHashCode());
        Trit.Positive.GetHashCode().Should().NotBe(Trit.Zero.GetHashCode());
        Trit.Negative.GetHashCode().Should().NotBe(Trit.Zero.GetHashCode());
    }

    [Fact]
    public void LogicalOperators_True_False_ShouldWorkCorrectly()
    {
        // Testing true operator (returns true if value is Positive/1)
        var isPositiveTrue = Trit.Positive ? true : false;
        var isNegativeTrue = Trit.Negative ? true : false;
        var isZeroTrue = Trit.Zero ? true : false;

        isPositiveTrue.Should().BeTrue();
        isNegativeTrue.Should().BeFalse();
        isZeroTrue.Should().BeFalse();

        // Testing false operator (returns true if value is Negative/-1)
        if (Trit.Positive)
        {
            // This branch should be taken for Positive
            true.Should().BeTrue();
        }
        else
        {
            // This branch should not be taken for Positive
            false.Should().BeTrue();
        }

        if (Trit.Negative)
        {
            // This branch should not be taken for Negative
            false.Should().BeTrue();
        }
        else
        {
            // This branch should be taken for Negative
            true.Should().BeTrue();
        }

        if (Trit.Zero)
        {
            // This branch should not be taken for Zero
            false.Should().BeTrue();
        }
        else
        {
            // This branch should be taken for Zero, as it's neither true nor false
            true.Should().BeTrue();
        }
    }

    [Fact]
    public void LogicalNot_ShouldInvertTrit()
    {
        (!Trit.Positive).Should().Be(Trit.Negative);
        (!Trit.Negative).Should().Be(Trit.Positive);
        (!Trit.Zero).Should().Be(Trit.Zero);
    }

    private static Trit And(Trit left, Trit right)
    {
        return left |BinaryTritOperator.And| right;
    }

    [Theory]
    [InlineData(T, T, T)]
    [InlineData(T, 0, T)]
    [InlineData(T, 1, T)]
    [InlineData(0, T, T)]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 0)]
    [InlineData(1, T, T)]
    [InlineData(1, 0, 0)]
    [InlineData(1, 1, 1)]
    public void PipeOperator_WithDelegateCall_ReturnsExpectedTrit(int left, int right, int expected)
    {
        var trit1 = (Trit)left;
        var trit2 = (Trit)right;
        var expectedResult = (Trit)expected;

        var result = trit1 | And | trit2;

        result.Should().Be(expectedResult);
    }

    [Fact]
    public void PipeOperator_WithArrayLookupTritOperator_WorksCorrectly()
    {
        // Table for min(value)
        // ---+----------
        //  T | T  T  T
        //  0 | T  0  0
        //  1 | T  0  1
        var min = new[,]
        {
            { Trit.Negative, Trit.Negative, Trit.Negative },
            { Trit.Negative, Trit.Zero, Trit.Zero },
            { Trit.Negative, Trit.Zero, Trit.Positive }
        };

        (Trit.Negative | min | Trit.Negative).Should().Be(Trit.Negative);
        (Trit.Negative | min | Trit.Zero).Should().Be(Trit.Negative);
        (Trit.Negative | min | Trit.Positive).Should().Be(Trit.Negative);
        (Trit.Zero | min | Trit.Negative).Should().Be(Trit.Negative);
        (Trit.Zero | min | Trit.Zero).Should().Be(Trit.Zero);
        (Trit.Zero | min | Trit.Positive).Should().Be(Trit.Zero);
        (Trit.Positive | min | Trit.Negative).Should().Be(Trit.Negative);
        (Trit.Positive | min | Trit.Zero).Should().Be(Trit.Zero);
        (Trit.Positive | min | Trit.Positive).Should().Be(Trit.Positive);
    }

    [Fact]
    public void PipeOperator_WithLookupTritOperator_WorksCorrectly()
    {
        // Table for min(value)
        // ---+----------
        //  T | T  T  T
        //  0 | T  0  0
        //  1 | T  0  1
        var min = new BinaryTritOperator(
            Trit.Negative, Trit.Negative, Trit.Negative,
            Trit.Negative, Trit.Zero, Trit.Zero,
            Trit.Negative, Trit.Zero, Trit.Positive);

        (Trit.Negative | min | Trit.Negative).Should().Be(Trit.Negative);
        (Trit.Negative | min | Trit.Zero).Should().Be(Trit.Negative);
        (Trit.Negative | min | Trit.Positive).Should().Be(Trit.Negative);
        (Trit.Zero | min | Trit.Negative).Should().Be(Trit.Negative);
        (Trit.Zero | min | Trit.Zero).Should().Be(Trit.Zero);
        (Trit.Zero | min | Trit.Positive).Should().Be(Trit.Zero);
        (Trit.Positive | min | Trit.Negative).Should().Be(Trit.Negative);
        (Trit.Positive | min | Trit.Zero).Should().Be(Trit.Zero);
        (Trit.Positive | min | Trit.Positive).Should().Be(Trit.Positive);
    }
}