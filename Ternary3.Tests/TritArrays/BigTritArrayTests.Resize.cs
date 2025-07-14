namespace Ternary3.Tests;

using FluentAssertions;

public partial class TernaryArrayTests
{
    [Fact]
    public void Resize_ToSameLength_MakesNoChanges()
    {
        // Arrange
        var array = new BigTernaryArray(10);
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = new((sbyte)((i % 3) - 1)); // Sets -1, 0, 1 pattern
        }
        var expectedValues = new Trit[10];
        for (var i = 0; i < 10; i++)
        {
            expectedValues[i] = array[i];
        }

        // Act
        array.Resize(10);

        // Assert
        array.Length.Should().Be(10);
        for (var i = 0; i < array.Length; i++)
        {
            array[i].Should().Be(expectedValues[i], $"because trit at index {i} should remain unchanged");
        }
    }

    [Theory]
    [InlineData(10, 5)]  // Shrink within same ulong
    [InlineData(100, 50)] // Shrink across multiple ulongs
    [InlineData(65, 64)]  // Shrink at ulong boundary
    [InlineData(128, 64)] // Shrink removing entire ulong
    public void Resize_ToSmallerLength_TruncatesData(int initialLength, int newLength)
    {
        // Arrange
        var array = new BigTernaryArray(initialLength);
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = new((sbyte)((i % 3) - 1)); // Sets -1, 0, 1 pattern
        }

        // Act
        array.Resize(newLength);

        // Assert
        array.Length.Should().Be(newLength);
        for (var i = 0; i < newLength; i++)
        {
            var expected = new Trit((sbyte)((i % 3) - 1));
            array[i].Should().Be(expected, $"because trit at index {i} should remain unchanged");
        }
            
        // Verify that accessing beyond new length throws
        var accessBeyondLength = () => { var _ = array[newLength]; };
        accessBeyondLength.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(5, 10)]    // Grow within same ulong
    [InlineData(50, 100)]  // Grow across multiple ulongs
    [InlineData(64, 65)]   // Grow at ulong boundary
    [InlineData(64, 128)]  // Grow adding entire ulong
    public void Resize_ToLargerLength_ExtendsWithZeros(int initialLength, int newLength)
    {
        // Arrange
        var array = new BigTernaryArray(initialLength);
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = new((sbyte)((i % 3) - 1)); // Sets -1, 0, 1 pattern
        }

        // Act
        array.Resize(newLength);

        // Assert
        array.Length.Should().Be(newLength);
            
        // Original values should remain unchanged
        for (var i = 0; i < initialLength; i++)
        {
            var expected = new Trit((sbyte)((i % 3) - 1));
            array[i].Should().Be(expected, $"because trit at index {i} should remain unchanged");
        }
            
        // New positions should be initialized to zero
        for (var i = initialLength; i < newLength; i++)
        {
            array[i].Should().Be(Trit.Zero, $"because new trit at index {i} should be zero");
        }
    }

    [Fact]
    public void Resize_ToZero_ClearsArray()
    {
        // Arrange
        var array = new BigTernaryArray(100);
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = Trit.Positive;
        }

        // Act
        array.Resize(0);

        // Assert
        array.Length.Should().Be(0);
        var accessElement = () => { var _ = array[0]; };
        accessElement.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Resize_WithNegativeLength_Throws()
    {
        // Arrange
        var array = new BigTernaryArray(10);

        // Act
        var resizeAction = () => array.Resize(-1);

        // Assert
        resizeAction.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("length");
    }
        
    [Fact]
    public void Resize_PreservesDataWhenResizedUpAndBackDown()
    {
        // Arrange
        var array = new BigTernaryArray(10);
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = new((sbyte)((i % 3) - 1));
        }
        var expectedValues = new Trit[10];
        for (var i = 0; i < 10; i++)
        {
            expectedValues[i] = array[i];
        }

        // Act
        array.Resize(100);  // Resize up
        array.Resize(10);   // Resize back down

        // Assert
        array.Length.Should().Be(10);
        for (var i = 0; i < array.Length; i++)
        {
            array[i].Should().Be(expectedValues[i], $"because trit at index {i} should be preserved after resizing up and down");
        }
    }
}