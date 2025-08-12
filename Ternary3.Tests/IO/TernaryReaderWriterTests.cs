namespace Ternary3.Tests.IO;

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ternary3.IO;
using TernaryArrays;
using Xunit;

public class TernaryReaderWriterTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    public async Task Int3T_WriteAndReadBack_ShouldPreserveValue(sbyte value)
    {
        // Arrange
        var stream = new MemoryInt3TStream();
        var writer = new TernaryWriter(stream);
        var reader = new TernaryReader(stream);
        var int3TValue = (Int3T)value;

        // Act - Write Int3T
        await writer.WriteAsync(int3TValue);
        
        // Reset stream position
        stream.Position = 0;
        
        // Act - Read Int3T
        var readInt3T = await reader.ReadInt3TAsync();
        
        // Reset stream position
        stream.Position = 0;
        
        // Act - Read as TernaryArray3 and convert
        var readArray = await reader.ReadTernaryArray3Async();
        var convertedInt3T = (Int3T)readArray;

        // Assert
        readInt3T.Should().Be(int3TValue, $"because the Int3T value {int3TValue} should be preserved when read back");
        convertedInt3T.Should().Be(int3TValue, $"because the Int3T value {int3TValue} should be preserved when read as TernaryArray3 and converted");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    public async Task TernaryArray3_WriteAndReadBack_ShouldPreserveValue(sbyte value)
    {
        // Arrange
        var stream = new MemoryInt3TStream();
        var writer = new TernaryWriter(stream);
        var reader = new TernaryReader(stream);
        var array3Value = (TernaryArray3)value;

        // Act - Write TernaryArray3
        await writer.WriteAsync(array3Value);
        
        // Reset stream position
        stream.Position = 0;
        
        // Act - Read Int3T and convert to TernaryArray3
        var readInt3T = await reader.ReadInt3TAsync();
        var convertedArray = (TernaryArray3)readInt3T;
        
        // Reset stream position
        stream.Position = 0;
        
        // Act - Read TernaryArray3 directly
        var readArray = await reader.ReadTernaryArray3Async();

        // Assert
        convertedArray.Should().Be(array3Value, $"because the TernaryArray3 value {array3Value} should be preserved when read as Int3T and converted");
        readArray.Should().Be(array3Value, $"because the TernaryArray3 value {array3Value} should be preserved when read back");
    }

    [Theory]
    [InlineData(-9840)]
    [InlineData(0)]
    [InlineData(9840)]
    public async Task Int9T_WriteAndReadBack_ShouldPreserveValue(int value)
    {
        // Arrange
        await using var stream = new MemoryInt3TStream();
        await using var writer = new TernaryWriter(stream);
        await using var reader = new TernaryReader(stream);
        var int9TValue = (Int9T)value;

        // Act - Write Int9T
        await writer.WriteAsync(int9TValue);
        
        // Reset stream position
        stream.Position = 0;
        
        // Act - Read Int9T
        var readArray = await reader.ReadTernaryArray9Async();
        var convertedArray = (Int9T)readArray;
        
        // Reset stream position
        stream.Position = 0;
        
        // Act - Read Int9T
        var readInt9T = await reader.ReadInt9TAsync();

        // Assert
        convertedArray.Should().Be(int9TValue, $"because the Int9T value {int9TValue} should be preserved when read back");
        readInt9T.Should().Be(int9TValue, $"because the Int9T value {int9TValue} should be preserved when read back");
    }

    [Theory]
    [InlineData(-9840)]
    [InlineData(0)]
    [InlineData(9840)]
    public async Task TernaryArray9_WriteAndReadBack_ShouldPreserveValue(int value)
    {
        // Arrange
        var stream = new MemoryInt3TStream();
        var writer = new TernaryWriter(stream);
        var reader = new TernaryReader(stream);
        // Create an array with some trits set
        var array9Value = (TernaryArray9)value;

        // Act - Write TernaryArray9
        await writer.WriteAsync(array9Value);
        
        // Reset stream position
        stream.Position = 0;
        
        // Act - Read TernaryArray9
        var readArray = await reader.ReadTernaryArray9Async();

        // Assert
        readArray.Should().Be(array9Value, $"because the TernaryArray9 value should be preserved when read back");
    }

    [Theory]
    [InlineData(-3812798742492)]
    [InlineData(0)]
    [InlineData(3812798742492)]
    public async Task Int27T_WriteAndReadBack_ShouldPreserveValue(long value)
    {
        // Arrange
        var stream = new MemoryInt3TStream();
        var writer = new TernaryWriter(stream);
        var reader = new TernaryReader(stream);
        var int27TValue = (Int27T)value;

        // Act - Write Int27T
        await writer.WriteAsync(int27TValue);
        
        // Reset stream position
        stream.Position = 0;
        
        // Act - Read Int27T
        var readInt27T = await reader.ReadInt27TAsync();

        // Assert
        readInt27T.Should().Be(int27TValue, $"because the Int27T value {int27TValue} should be preserved when read back");
    }

    [Theory]
    [InlineData(-3812798742492)]
    [InlineData(0)]
    [InlineData(3812798742492)]
    public async Task TernaryArray27_WriteAndReadBack_ShouldPreserveValue(long value)
    {
        // Arrange
        var stream = new MemoryInt3TStream();
        var writer = new TernaryWriter(stream);
        var reader = new TernaryReader(stream);
        var array27Value = (TernaryArray27)value;

        // Act - Write TernaryArray27
        await writer.WriteAsync(array27Value);
        
        // Reset stream position
        stream.Position = 0;
        
        // Act - Read TernaryArray27
        var readArray = await reader.ReadTernaryArray27Async();

        // Assert
        readArray.Should().Be(array27Value, $"because the TernaryArray27 value should be preserved when read back");
    }

    [Theory]
    [InlineData(null)]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Trit_WriteAndReadBack_ShouldPreserveValue(bool? value)
    {
        // Arrange
        var stream = new MemoryInt3TStream();
        var writer = new TernaryWriter(stream);
        var reader = new TernaryReader(stream);
        var tritValue = (Trit)value;

        // Act - Write Trit
        await writer.WriteAsync(tritValue);
        
        // Reset stream position
        stream.Position = 0;
        
        // Act - Read Trit
        var readTrit = await reader.ReadTritAsync();
        
        // Reset stream position
        stream.Position = 0;
        
        // Act - Read as nullable boolean
        var readNullableBoolean = await reader.ReadNullableBooleanAsync();

        // Assert
        readTrit.Should().Be(tritValue, $"because the Trit value {tritValue} should be preserved when read back");
        readNullableBoolean.Should().Be((bool?)tritValue, $"because the Trit value {tritValue} should be preserved when read as nullable boolean");
    }

    [Theory]
    [InlineData(null)]
    [InlineData(false)]
    [InlineData(true)]
    public async Task NullableBoolean_WriteAndReadBack_ShouldPreserveValue(bool? value)
    {
        // Arrange
        var stream = new MemoryInt3TStream();
        var writer = new TernaryWriter(stream);
        var reader = new TernaryReader(stream);

        // Act - Write nullable boolean
        await writer.WriteAsync(value);
        
        // Reset stream position
        stream.Position = 0;
        
        // Act - Read as nullable boolean
        var readNullableBoolean = await reader.ReadNullableBooleanAsync();
        
        // Reset stream position
        stream.Position = 0;
        
        // Act - Read as Trit
        var readTrit = await reader.ReadTritAsync();

        // Assert
        readNullableBoolean.Should().Be(value, $"because the nullable boolean value {value} should be preserved when read back");
        ((bool?)readTrit).Should().Be(value, $"because the nullable boolean value {value} should be preserved when read as Trit");
    }

    [Fact]
    public async Task MixedTypes_WriteAndReadBack_ShouldPreserveValuesAndOrder()
    {
        // Arrange
        var stream = new MemoryInt3TStream();
        var writer = new TernaryWriter(stream);
        var reader = new TernaryReader(stream);
        var tritValue = (Trit)1;
        var int3TValue = (Int3T)(-1);
        var int9TValue = (Int9T)42;
        var array3Value = (TernaryArray3)(-1);

        // Act - Write different types in sequence
        await writer.WriteAsync(tritValue);
        await writer.WriteAsync(int3TValue);
        await writer.WriteAsync(int9TValue);
        await writer.WriteAsync(array3Value);
        
        // Reset stream position
        stream.Position = 0;
        
        // Act - Read the values back in the same order
        var readTrit = await reader.ReadTritAsync();
        var readInt3T = await reader.ReadInt3TAsync();
        var readInt9T = await reader.ReadInt9TAsync();
        var readArray3 = await reader.ReadTernaryArray3Async();

        // Assert
        readTrit.Should().Be(tritValue, "because the Trit value should be preserved in sequence");
        readInt3T.Should().Be(int3TValue, "because the Int3T value should be preserved in sequence");
        readInt9T.Should().Be(int9TValue, "because the Int9T value should be preserved in sequence");
        readArray3.Should().Be(array3Value, "because the TernaryArray3 value should be preserved in sequence");
    }
}
