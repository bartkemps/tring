using FluentAssertions;

namespace Ternary3.Tests.Numbers;

public class TritArray9ShiftAndConversionTests
{
    [Theory]
    [InlineData(6)]
    [InlineData(-6)]
    public void LeftShift_WithMaxValue_ConvertsToCorrectInt(int shiftAmount)
    {
        // Arrange
        TritArray9 input = Int9T.MaxValue; // 111111111
            
        // Act
        var shiftedLeft = input << shiftAmount;
        var convertedValue = (int)shiftedLeft;
            
        // Assert
        if (shiftAmount == 6)
        {
            // Expected: 111000000 = 3^8 + 3^7 + 6561 + 2187 + 729 = 9477
            shiftedLeft.ToString().Should().Be("111 000 000");
            convertedValue.Should().Be(9477, $"because 111000000 in balanced ternary converts to 9477 in decimal");
        }
        else if (shiftAmount == -6)
        {
            // Expected: 000000111 = 3^2 + 3^1 + 3^0 = 9 + 3 + 1 = 13
            shiftedLeft.ToString().Should().Be("000 000 111");
            convertedValue.Should().Be(13, $"because 000000111 in balanced ternary converts to 13 in decimal");
        }
    }

    [Theory]
    [InlineData(6)]
    [InlineData(-6)]
    public void RightShift_WithMaxValue_ConvertsToCorrectInt(int shiftAmount)
    {
        // Arrange
        TritArray9 input = Int9T.MaxValue; // 111111111
            
        // Act
        var shiftedRight = input >> shiftAmount;
        var convertedValue = (int)shiftedRight;
            
        // Assert
        if (shiftAmount == 6)
        {
            // Expected: 000000111 = 3^2 + 3^1 + 3^0 = 9 + 3 + 1 = 13
            shiftedRight.ToString().Should().Be("000 000 111");
            convertedValue.Should().Be(13, $"because 000000111 in balanced ternary converts to 13 in decimal");
        }
        else if (shiftAmount == -6)
        {
            // Expected: 111000000 = 3^8 + 3^7 + 6561 + 2187 + 729 = 9477
            shiftedRight.ToString().Should().Be("111 000 000");
            convertedValue.Should().Be(9477, $"because 111000000 in balanced ternary converts to 9477 in decimal");
        }
    }

    [Fact]
    public void MaxValue_ConvertsToCorrectDecimal()
    {
        // Arrange
        TritArray9 maxValue = Int9T.MaxValue; // 111111111
            
        // Act
        var converted = (int)maxValue;
            
        // Assert
        // 3^8 + 3^7 + 3^6 + 3^5 + 3^4 + 3^3 + 3^2 + 3^1 + 3^0
        // = 6561 + 2187 + 729 + 243 + 81 + 27 + 9 + 3 + 1 
        // = 9841
        converted.Should().Be(9841, "because 111111111 in balanced ternary equals 9841 in decimal");
    }

    [Fact]
    public void VerifyShiftedValues()
    {
        // Arrange
        TritArray9 input = Int9T.MaxValue; // 111111111 in ternary

        // Act & Assert
        var leftShift6 = input << 6;     // 111000000
        var rightShiftNeg6 = input >> -6; // 111000000
        var leftShiftNeg6 = input << -6;  // 000000111
        var rightShift6 = input >> 6;     // 000000111

        // Assert that shift operations work correctly
        leftShift6.ToString().Should().Be("111 000 000");
        rightShiftNeg6.ToString().Should().Be("111 000 000");
        leftShiftNeg6.ToString().Should().Be("000 000 111");
        rightShift6.ToString().Should().Be("000 000 111");
            
        // Assert conversions to int
        ((int)leftShift6).Should().Be(9477);
        ((int)rightShiftNeg6).Should().Be(9477);
        ((int)leftShiftNeg6).Should().Be(13);
        ((int)rightShift6).Should().Be(13);
    }
}