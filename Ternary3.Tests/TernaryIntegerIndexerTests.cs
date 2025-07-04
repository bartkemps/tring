// filepath: c:\Users\kempsb\source\repos\Ternary\Ternary3.Tests\TernaryIntegerIndexerTests.cs
using System;
using FluentAssertions;
using Ternary3;
using Xunit;

namespace Ternary3.Tests
{
    public partial class TernaryIntegerIndexerTests
    {
        [Theory]
        [InlineData(-13, 0)]  // Min value
        [InlineData(-9, 0)]
        [InlineData(-3, 0)]   // -3 = [-1, 0, 0]
        [InlineData(-1, 0)]   // -1 = [-1, 0, 1]
        [InlineData(0, 0)]    // 0 = [0, 0, 0]
        [InlineData(1, 0)]    // 1 = [1, 0, 0]
        [InlineData(4, 0)]    // 4 = [1, 1, 1]
        [InlineData(13, 0)]   // Max value
        [InlineData(-13, 1)]
        [InlineData(-1, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(13, 1)]
        [InlineData(-13, 2)]
        [InlineData(-1, 2)]
        [InlineData(0, 2)]
        [InlineData(1, 2)]
        [InlineData(13, 2)]
        public void Int3T_IndexerReturnsCorrectTrit_WhenComparedToTritArray3(sbyte value, int index)
        {
            // Arrange
            var intValue = (Int3T)value;
            var tritArray = (TritArray3)value;

            // Act
            var tritFromInt = intValue[index];
            var tritFromArray = tritArray[index];

            // Assert
            tritFromInt.Should().Be(tritFromArray, $"because Int3T({value})[{index}] should equal TritArray3({value})[{index}]");
        }

        [Fact]
        public void Int3T_IndexFromEnd1_ReturnsCorrectTrit()
        {
            // Arrange
            sbyte value = -13;
            var intValue = (Int3T)value;
            var tritArray = (TritArray3)value;
            var index = ^1; // Last trit

            // Act
            var tritFromInt = intValue[index];
            var tritFromArray = tritArray[index];

            // Assert
            tritFromInt.Should().Be(tritFromArray, $"because Int3T({value})[{index}] should equal TritArray3({value})[{index}]");
        }

        [Fact]
        public void Int3T_IndexFromEnd2_ReturnsCorrectTrit()
        {
            // Arrange
            sbyte value = -1;
            var intValue = (Int3T)value;
            var tritArray = (TritArray3)value;
            var index = ^2; // Second-to-last trit

            // Act
            var tritFromInt = intValue[index];
            var tritFromArray = tritArray[index];

            // Assert
            tritFromInt.Should().Be(tritFromArray, $"because Int3T({value})[{index}] should equal TritArray3({value})[{index}]");
        }

        [Fact]
        public void Int3T_IndexFromEnd3_ReturnsCorrectTrit()
        {
            // Arrange
            sbyte value = 0;
            var intValue = (Int3T)value;
            var tritArray = (TritArray3)value;
            var index = ^3; // First trit (from end)

            // Act
            var tritFromInt = intValue[index];
            var tritFromArray = tritArray[index];

            // Assert
            tritFromInt.Should().Be(tritFromArray, $"because Int3T({value})[{index}] should equal TritArray3({value})[{index}]");
        }

        [Fact]
        public void Int3T_IndexFromEnd1_WithMaxValue_ReturnsCorrectTrit()
        {
            // Arrange
            sbyte value = 13;
            var intValue = (Int3T)value;
            var tritArray = (TritArray3)value;
            var index = ^1; // Last trit

            // Act
            var tritFromInt = intValue[index];
            var tritFromArray = tritArray[index];

            // Assert
            tritFromInt.Should().Be(tritFromArray, $"because Int3T({value})[{index}] should equal TritArray3({value})[{index}]");
        }

        [Theory]
        [InlineData(-9841, 0)]  // Testing with values within Int9T range
        [InlineData(-4000, 0)]
        [InlineData(-1, 0)]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(4000, 0)]
        [InlineData(9841, 0)]
        [InlineData(-9841, 4)]
        [InlineData(-1, 4)]
        [InlineData(0, 4)]
        [InlineData(1, 4)]
        [InlineData(9841, 4)]
        [InlineData(-9841, 8)]
        [InlineData(-1, 8)]
        [InlineData(0, 8)]
        [InlineData(1, 8)]
        [InlineData(9841, 8)]
        public void Int9T_IndexerReturnsCorrectTrit_WhenComparedToTritArray9(int value, int index)
        {
            // Arrange
            var intValue = (Int9T)value;
            var tritArray = (TritArray9)value;

            // Act
            var tritFromInt = intValue[index];
            var tritFromArray = tritArray[index];

            // Assert
            tritFromInt.Should().Be(tritFromArray, $"because Int9T({value})[{index}] should equal TritArray9({value})[{index}]");
        }

        [Theory]
        [InlineData(ter111_0T1_001, "PPPZNPZZP")]  // Testing with values within Int27T range
        [InlineData(terTTT_01T_00T, "NNNZPNZZN")]  // Testing with values within Int27T range
        public void Int9T_IndexerReturnsCorrectTrit(int value, string expected)
        {
            var intValue = (Int9T)value;
            var digits = "";
            for(var i=0; i < 9; i++)
            {
                digits = intValue[i].ToString()[0] + digits;
            }

            digits.Should().Be(expected);
        }

        [Theory]
        [InlineData(5, 0)]    // Out of range index (Int3T has 3 trits)
        [InlineData(-1, 0)]   // Negative index
        public void Int3T_IndexerThrowsException_WhenIndexIsOutOfRange(int index, sbyte value)
        {
            // Arrange
            var intValue = (Int3T)value;

            // Act & Assert
            Action act = () => { var trit = intValue[index]; };
            act.Should().Throw<Exception>($"because index {index} is outside the valid range for Int3T");
        }

        [Theory]
        [InlineData(10, 0)]   // Out of range index (Int9T has 9 trits)
        [InlineData(-1, 0)]   // Negative index
        public void Int9T_IndexerThrowsException_WhenIndexIsOutOfRange(int index, int value)
        {
            // Arrange
            var intValue = (Int9T)value;

            // Act & Assert
            Action act = () => { var trit = intValue[index]; };
            act.Should().Throw<Exception>($"because index {index} is outside the valid range for Int9T");
        }

        [Theory]
        [InlineData(28, 0)]   // Out of range index (Int27T has 27 trits)
        [InlineData(-1, 0)]   // Negative index
        public void Int27T_IndexerThrowsException_WhenIndexIsOutOfRange(int index, long value)
        {
            // Arrange
            var intValue = (Int27T)value;

            // Act & Assert
            Action act = () => { var trit = intValue[index]; };
            act.Should().Throw<Exception>($"because index {index} is outside the valid range for Int27T");
        }
    }
}
