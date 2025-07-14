using FluentAssertions;
using Xunit;

namespace Ternary3.Tests.Numbers;

public class TritArray27Int27TShiftConsistencyTests
{
    [Theory]
    [InlineData(3812798742493L, 0)] // Max value, no shift
    [InlineData(3812798742493L, 1)] // Max value, shift by 1
    [InlineData(3812798742493L, 13)] // Max value, shift by 13
    [InlineData(3812798742493L, 26)] // Max value, shift by 26
    [InlineData(3812798742493L, 27)] // Max value, shift by 27 (should be zero)
    [InlineData(-3812798742493L, 5)] // Min value, shift by 5
    [InlineData(1234567890L, 7)] // Arbitrary big number, shift by 7
    [InlineData(-1234567890L, 7)] // Negative big number, shift by 7
    public void RightShift_YieldsSameValue_ForTritArray27AndInt27T(long value, int shift)
    {
        Int27T intValue = value;
        TernaryArray27 ternaryArray = intValue;

        var shiftedArray = ternaryArray >> shift;
        var shiftedInt = intValue >> shift;
        TernaryArray27 shiftedArrayFromInt = shiftedInt;

        for (var i = 0; i < 27; i++)
        {
            shiftedArray[i].Should().Be(shiftedArrayFromInt[i], $"because right shift of {value} by {shift} should yield the same ternaries for TernaryArray27 and Int27T");
        }
    }
}

