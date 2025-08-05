using FluentAssertions;

namespace Ternary3.Tests.Numbers;

public class TernaryArray9ShiftAndConversionTests
{
    [Theory]
    [InlineData(6, "111 000 000", 9477)]
    [InlineData(-6, "000 000 111", 13)]
    public void LeftShift_WithMaxValue_ConvertsToCorrectInt(int shiftAmount, string expectedTernary, int expectedInt)
    {
        // Arrange
        TernaryArray9 input = Int9T.MaxValue; // 111111111
            
        // Act
        var shiftedLeft = input << shiftAmount;
        var convertedValue = (int)shiftedLeft;
            
        // Assert
        shiftedLeft.ToString("ter").Should().Be(expectedTernary);
        convertedValue.Should().Be(expectedInt, $"because {expectedTernary.Replace(" ", "")} in balanced ternary converts to {expectedInt} in decimal");
    }

    [Theory]
    [InlineData(6, "000 000 111", 13)]
    [InlineData(-6, "111 000 000", 9477)]
    public void RightShift_WithMaxValue_ConvertsToCorrectInt(int shiftAmount, string expectedTernary, int expectedInt)
    {
        // Arrange
        TernaryArray9 input = Int9T.MaxValue; // 111111111
            
        // Act
        var shiftedRight = input >> shiftAmount;
        var convertedValue = (int)shiftedRight;
            
        // Assert
        shiftedRight.ToString("ter").Should().Be(expectedTernary);
        convertedValue.Should().Be(expectedInt, $"because {expectedTernary.Replace(" ", "")} in balanced ternary converts to {expectedInt} in decimal");
    }

    [Fact]
    public void MaxValue_ConvertsToCorrectDecimal()
    {
        // Arrange
        TernaryArray9 maxValue = Int9T.MaxValue; // 111111111
            
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
        TernaryArray9 input = Int9T.MaxValue; // 111111111 in ternary

        // Act & Assert
        var leftShift6 = input << 6;     // 111000000
        var rightShiftNeg6 = input >> -6; // 111000000
        var leftShiftNeg6 = input << -6;  // 000000111
        var rightShift6 = input >> 6;     // 000000111

        // Assert that shift operations work correctly
        leftShift6.ToString("ter").Should().Be("111 000 000");
        rightShiftNeg6.ToString("ter").Should().Be("111 000 000");
        leftShiftNeg6.ToString("ter").Should().Be("000 000 111");
        rightShift6.ToString("ter").Should().Be("000 000 111");
            
        // Assert conversions to int
        ((int)leftShift6).Should().Be(9477);
        ((int)rightShiftNeg6).Should().Be(9477);
        ((int)leftShiftNeg6).Should().Be(13);
        ((int)rightShift6).Should().Be(13);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void Indexer_IndexFromEnd_GetsCorrectTrit(int fromEnd)
    {
        var arr = new TernaryArray9();
        for (var i = 0; i < arr.Length; i++)
            arr[i] = new((sbyte)((i % 3) - 1));
        var expected = arr[8 - fromEnd];
        var actual = arr[^ (fromEnd + 1)];
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, -1)]
    [InlineData(1, 0)]
    [InlineData(2, 1)]
    [InlineData(3, -1)]
    [InlineData(4, 0)]
    [InlineData(5, 1)]
    [InlineData(6, -1)]
    [InlineData(7, 0)]
    [InlineData(8, 1)]
    public void Indexer_IndexFromEnd_SetsCorrectTrit(int fromEnd, sbyte tritValue)
    {
        var arr = new TernaryArray9();
        arr[^ (fromEnd + 1)] = new(tritValue);
        arr[^ (fromEnd + 1)].Value.Should().Be(tritValue);
    }
}