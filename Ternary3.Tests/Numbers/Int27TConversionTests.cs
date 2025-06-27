using FluentAssertions;

namespace Ternary3.Tests.Numbers;

public class Int27TConversionTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(42)]
    [InlineData(-42)]
    [InlineData(3812798742492)]  // Near max value
    [InlineData(-3812798742492)] // Near min value
    public void ImplicitConversion_PreservesValue_WhenRoundtripped(long value)
    {
        Int27T original = value;
            
        TritArray27 array = original;
        Int27T roundTrip = array;
            
        roundTrip.Should().Be(original, $"because conversion of {value} to TritArray27 and back should preserve the original value");
    }

    [Theory]
    [InlineData(new sbyte[] { 1, 0, -1 })]  // 1, 0, T
    [InlineData(new sbyte[] { 1, 1, 1 })]   // All positives
    [InlineData(new sbyte[] { -1, -1, -1 })] // All negatives
    [InlineData(new sbyte[] { 0, 0, 0 })]   // All zeros
    [InlineData(new sbyte[] { 1, -1, 0, 1, -1 })] // Mixed values
    public void ImplicitConversion_CalculatesCorrectValue_FromTritArray27(sbyte[] tritValues)
    {
        var array = new TritArray27();
        long expectedValue = 0;
        long power = 1;
            
        for (var i = 0; i < tritValues.Length; i++)
        {
            array[i] = new(tritValues[i]);
            expectedValue += tritValues[i] * power;
            power *= 3;
        }
            
        Int27T result = array;
            
        ((long)result).Should().Be(expectedValue, $"because conversion from TritArray27 with specified trits should yield {expectedValue}");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(42)]
    [InlineData(-42)]
    [InlineData(123456)]
    [InlineData(-123456)]
    public void ShiftOperations_PreserveValue_WhenShiftingLeftThenRight(long value)
    {
        Int27T original = value;
        TritArray27 array = original;
            
        var shiftedArray = array << 1;
        var shiftedBack = shiftedArray >> 1;
        Int27T result = shiftedBack;
            
        result.Should().Be(original, $"because shifting {value} left by 1 then right by 1 should preserve the original value");
    }

    [Theory]
    [InlineData(10, 20)]
    [InlineData(-15, 7)]
    [InlineData(42, -42)]
    public void Addition_ProducesEquivalentResults_ForBothTypes(long value1, long value2)
    {
        Int27T int1 = value1;
        Int27T int2 = value2;
        var expectedSum = int1 + int2;
            
        TritArray27 array1 = int1;
        TritArray27 array2 = int2;
        var sumArray = array1 + array2;
        Int27T actualSum = sumArray;
            
        actualSum.Should().Be(expectedSum, $"because adding {value1} and {value2} should produce the same result regardless of the type used");
    }

    [Theory]
    [InlineData(30, 10)]
    [InlineData(-15, 7)]
    [InlineData(42, -42)]
    public void Subtraction_ProducesEquivalentResults_ForBothTypes(long value1, long value2)
    {
        Int27T int1 = value1;
        Int27T int2 = value2;
        var expectedDifference = int1 - int2;
            
        TritArray27 array1 = int1;
        TritArray27 array2 = int2;
        var differenceArray = array1 - array2;
        Int27T actualDifference = differenceArray;
            
        actualDifference.Should().Be(expectedDifference, $"because subtracting {value2} from {value1} should produce the same result regardless of the type used");
    }
}