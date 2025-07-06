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

        [Fact]
        public void Int9T_IndexFromEnd1_ReturnsCorrectTrit()
        {
            // Arrange
            var value = -9841;
            var intValue = (Int9T)value;
            var tritArray = (TritArray9)value;
            var index = ^1; // Last trit

            // Act
            var tritFromInt = intValue[index];
            var tritFromArray = tritArray[index];

            // Assert
            tritFromInt.Should().Be(tritFromArray, $"because Int9T({value})[{index}] should equal TritArray9({value})[{index}]");
        }

        [Fact]
        public void Int9T_IndexFromEnd5_ReturnsCorrectTrit()
        {
            // Arrange
            var value = 4000;
            var intValue = (Int9T)value;
            var tritArray = (TritArray9)value;
            var index = ^5; // Fifth trit from end

            // Act
            var tritFromInt = intValue[index];
            var tritFromArray = tritArray[index];

            // Assert
            tritFromInt.Should().Be(tritFromArray, $"because Int9T({value})[{index}] should equal TritArray9({value})[{index}]");
        }

        [Fact]
        public void Int9T_IndexFromEnd9_ReturnsCorrectTrit()
        {
            // Arrange
            var value = 9841; // Max value
            var intValue = (Int9T)value;
            var tritArray = (TritArray9)value;
            var index = ^9; // First trit (from end)

            // Act
            var tritFromInt = intValue[index];
            var tritFromArray = tritArray[index];

            // Assert
            tritFromInt.Should().Be(tritFromArray, $"because Int9T({value})[{index}] should equal TritArray9({value})[{index}]");
        }

        [Theory]
        [InlineData(-3812798742289, 0)]  // Testing with values within Int27T range
        [InlineData(-1000000000, 0)]
        [InlineData(-1, 0)]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(1000000000, 0)]
        [InlineData(3812798742289, 0)]
        [InlineData(-3812798742289, 13)]
        [InlineData(-1, 13)]
        [InlineData(0, 13)]
        [InlineData(1, 13)]
        [InlineData(3812798742289, 13)]
        [InlineData(-3812798742289, 26)]
        [InlineData(-1, 26)]
        [InlineData(0, 26)]
        [InlineData(1, 26)]
        [InlineData(3812798742289, 26)]
        public void Int27T_IndexerReturnsCorrectTrit_WhenComparedToTritArray27(long value, int index)
        {
            // Arrange
            var intValue = (Int27T)value;
            var tritArray = (TritArray27)value;

            // Act
            var tritFromInt = intValue[index];
            var tritFromArray = tritArray[index];

            // Assert
            tritFromInt.Should().Be(tritFromArray, $"because Int27T({value})[{index}] should equal TritArray27({value})[{index}]");
        }

        [Fact]
        public void Int27T_IndexFromEnd1_ReturnsCorrectTrit()
        {
            // Arrange
            var value = -3812798742289;
            var intValue = (Int27T)value;
            var tritArray = (TritArray27)value;
            var index = ^1; // Last trit

            // Act
            var tritFromInt = intValue[index];
            var tritFromArray = tritArray[index];

            // Assert
            tritFromInt.Should().Be(tritFromArray, $"because Int27T({value})[{index}] should equal TritArray27({value})[{index}]");
        }

        [Fact]
        public void Int27T_IndexFromEnd14_ReturnsCorrectTrit()
        {
            // Arrange
            long value = 1000000000;
            var intValue = (Int27T)value;
            var tritArray = (TritArray27)value;
            var index = ^14; // 14th trit from end

            // Act
            var tritFromInt = intValue[index];
            var tritFromArray = tritArray[index];

            // Assert
            tritFromInt.Should().Be(tritFromArray, $"because Int27T({value})[{index}] should equal TritArray27({value})[{index}]");
        }

        [Fact]
        public void Int27T_IndexFromEnd27_ReturnsCorrectTrit()
        {
            // Arrange
            var value = 3812798742289; // Max value
            var intValue = (Int27T)value;
            var tritArray = (TritArray27)value;
            var index = ^27; // First trit (from end)

            // Act
            var tritFromInt = intValue[index];
            var tritFromArray = tritArray[index];

            // Assert
            tritFromInt.Should().Be(tritFromArray, $"because Int27T({value})[{index}] should equal TritArray27({value})[{index}]");
        }

        [Theory]
        [InlineData(5, 0)]    // Out of range index (Int3T has 3 trits)
        [InlineData(-1, 0)]   // Negative index
        public void Int3T_IndexerThrowsException_WhenIndexIsOutOfRange(int index, sbyte value)
        {
            // Arrange
            var intValue = (Int3T)value;

            // Act & Assert
            var act = () => { var trit = intValue[index]; };
            act.Should().Throw<ArgumentOutOfRangeException>($"because index {index} is outside the valid range for Int3T");
        }

        [Theory]
        [InlineData(10, 0)]   // Out of range index (Int9T has 9 trits)
        [InlineData(-1, 0)]   // Negative index
        public void Int9T_IndexerThrowsException_WhenIndexIsOutOfRange(int index, int value)
        {
            // Arrange
            var intValue = (Int9T)value;

            // Act & Assert
            var act = () => { var trit = intValue[index]; };
            act.Should().Throw<ArgumentOutOfRangeException>($"because index {index} is outside the valid range for Int9T");
        }

        [Theory]
        [InlineData(28, 0)]   // Out of range index (Int27T has 27 trits)
        [InlineData(-1, 0)]   // Negative index
        public void Int27T_IndexerThrowsException_WhenIndexIsOutOfRange(int index, long value)
        {
            // Arrange
            var intValue = (Int27T)value;

            // Act & Assert
            var act = () => { var trit = intValue[index]; };
            act.Should().Throw<ArgumentOutOfRangeException>($"because index {index} is outside the valid range for Int27T");
        }
    }
}
