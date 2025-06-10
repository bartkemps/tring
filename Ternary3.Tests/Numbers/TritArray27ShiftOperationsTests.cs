using FluentAssertions;

namespace Ternary3.Tests.Numbers;

public class TritArray27ShiftOperationsTests
{
    [Theory]
    [InlineData(0)]   // No shift
    [InlineData(1)]   // Shift by 1
    [InlineData(5)]   // Medium shift
    [InlineData(26)]  // Maximum valid shift (27-1)
    [InlineData(27)]  // Shift that results in all zeros
    [InlineData(100)] // Large shift beyond array length
    public void LeftShift_ShiftsTritsLeft_AndClearsRightSide(int shiftAmount)
    {
        // Create a test array with alternating trits
        var array = new TritArray27();
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = new Trit((sbyte)((i % 3) - 1)); // Values -1, 0, 1 repeating
        }

        var result = array << shiftAmount;

        // Test expectations
        if (shiftAmount >= 27)
        {
            // All trits should be zero when shift >= length
            for (var i = 0; i < result.Length; i++)
            {
                result[i].Should().Be(Trit.Zero, $"because a shift of {shiftAmount} should zero out all trits");
            }
        }
        else
        {
            // Check that values shifted correctly
            for (var i = 0; i < result.Length; i++)
            {
                if (i < shiftAmount)
                {
                    // Positions less than shift amount should be zero (filled from right)
                    result[i].Should().Be(Trit.Zero, $"because position {i} should be zero after left shift of {shiftAmount}");
                }
                else
                {
                    // Other positions should contain the original values, shifted
                    var originalIndex = i - shiftAmount;
                    var expectedValue = new Trit((sbyte)((originalIndex % 3) - 1));
                    result[i].Should().Be(expectedValue, 
                        $"because position {i} should contain the value from position {originalIndex} after left shift of {shiftAmount}");
                }
            }
        }
    }

    [Theory]
    [InlineData(0)]   // No shift
    [InlineData(1)]   // Shift by 1
    [InlineData(5)]   // Medium shift
    [InlineData(26)]  // Maximum valid shift (27-1)
    [InlineData(27)]  // Shift that results in all zeros
    [InlineData(100)] // Large shift beyond array length
    public void RightShift_ShiftsTritsRight_AndClearsLeftSide(int shiftAmount)
    {
        // Create a test array with alternating trits
        var array = new TritArray27();
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = new Trit((sbyte)((i % 3) - 1)); // Values -1, 0, 1 repeating
        }

        var result = array >> shiftAmount;

        // Test expectations
        if (shiftAmount >= 27)
        {
            // All trits should be zero when shift >= length
            for (var i = 0; i < result.Length; i++)
            {
                result[i].Should().Be(Trit.Zero, $"because a shift of {shiftAmount} should zero out all trits");
            }
        }
        else
        {
            // Check that values shifted correctly
            for (var i = 0; i < result.Length; i++)
            {
                if (i >= array.Length - shiftAmount)
                {
                    // Positions beyond (length - shift) should be zero (filled from left)
                    result[i].Should().Be(Trit.Zero, $"because position {i} should be zero after right shift of {shiftAmount}");
                }
                else
                {
                    // Other positions should contain the original values, shifted
                    var originalIndex = i + shiftAmount;
                    var expectedValue = new Trit((sbyte)((originalIndex % 3) - 1));
                    result[i].Should().Be(expectedValue, 
                        $"because position {i} should contain the value from position {originalIndex} after right shift of {shiftAmount}");
                }
            }
        }
    }

    [Theory]
    [InlineData(-1)]    // Small negative becomes positive
    [InlineData(-5)]    // Medium negative becomes positive
    [InlineData(-26)]   // Large negative becomes positive
    [InlineData(-100)]  // Very large negative
    public void LeftShift_WithNegativeAmount_ConvertsToRightShift(int negativeShiftAmount)
    {
        // Create a test array
        var array = new TritArray27();
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = new Trit((sbyte)((i % 3) - 1)); // Values -1, 0, 1 repeating
        }

        var resultWithNegativeShift = array << negativeShiftAmount;
        var resultWithPositiveRightShift = array >> -negativeShiftAmount;

        // The results should be identical
        for (var i = 0; i < array.Length; i++)
        {
            resultWithNegativeShift[i].Should().Be(resultWithPositiveRightShift[i], 
                $"because left shift with negative amount {negativeShiftAmount} should be equivalent to right shift with positive amount {-negativeShiftAmount}");
        }
    }

    [Theory]
    [InlineData(-1)]    // Small negative becomes positive
    [InlineData(-5)]    // Medium negative becomes positive
    [InlineData(-26)]   // Large negative becomes positive
    [InlineData(-100)]  // Very large negative
    public void RightShift_WithNegativeAmount_ConvertsToLeftShift(int negativeShiftAmount)
    {
        // Create a test array
        var array = new TritArray27();
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = new Trit((sbyte)((i % 3) - 1)); // Values -1, 0, 1 repeating
        }

        var resultWithNegativeShift = array >> negativeShiftAmount;
        var resultWithPositiveLeftShift = array << -negativeShiftAmount;

        // The results should be identical
        for (var i = 0; i < array.Length; i++)
        {
            resultWithNegativeShift[i].Should().Be(resultWithPositiveLeftShift[i], 
                $"because right shift with negative amount {negativeShiftAmount} should be equivalent to left shift with positive amount {-negativeShiftAmount}");
        }
    }
    
    [Theory]
    [InlineData(-1, 1)]  // Negative trit
    [InlineData(0, 1)]   // Zero trit
    [InlineData(1, 1)]   // Positive trit
    public void LeftShift_PreservesTrits_WhenShiftingSimpleTrit(sbyte tritValue, int shiftAmount)
    {
        var array = new TritArray27();
        array[0] = new Trit(tritValue);
        
        var result = array << shiftAmount;
        
        result[shiftAmount].Should().Be(new Trit(tritValue), 
            $"because a single trit shifted left by {shiftAmount} should preserve its value");
        
        // Check that all other positions are zero
        for (var i = 0; i < result.Length; i++)
        {
            if (i != shiftAmount)
            {
                result[i].Should().Be(Trit.Zero, 
                    $"because position {i} should be zero when only position {shiftAmount} should have a value");
            }
        }
    }
    
    [Theory]
    [InlineData(-1, 1)]  // Negative trit
    [InlineData(0, 1)]   // Zero trit
    [InlineData(1, 1)]   // Positive trit
    public void RightShift_PreservesTrits_WhenShiftingSimpleTrit(sbyte tritValue, int shiftAmount)
    {
        var array = new TritArray27();
        var initialPosition = shiftAmount + 1; // Position to set the initial trit
        array[initialPosition] = new Trit(tritValue);
        
        var result = array >> shiftAmount;
        
        result[1].Should().Be(new Trit(tritValue), 
            $"because a trit at position {initialPosition} shifted right by {shiftAmount} should be at position 1");
        
        // Check that all other positions are zero
        for (var i = 0; i < result.Length; i++)
        {
            if (i != 1)
            {
                result[i].Should().Be(Trit.Zero, 
                    $"because position {i} should be zero when only position 1 should have a value");
            }
        }
    }
}
