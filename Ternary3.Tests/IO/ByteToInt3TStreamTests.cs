using FluentAssertions;
using Ternary3.IO;

namespace Ternary3.Tests.IO;

public class ByteToInt3TStreamTests
{
    [Fact]
    public void Constructor_WithByteStream_CreatesReadableStream()
    {
        var memoryStream = new MemoryStream();
        var stream = new ByteToInt3TStream(memoryStream);

        stream.CanRead.Should().BeTrue();
        stream.CanWrite.Should().BeTrue(); 
        stream.CanSeek.Should().BeFalse();
    }

    [Fact]
    public async Task ReadAsync_WithInvalidParameters_ThrowsArgumentExceptions()
    {
        // Arrange
        var memoryStream = new MemoryStream();
        var stream = new ByteToInt3TStream(memoryStream);
        var buffer = new Int3T[10];
            
        // Act & Assert - Null buffer
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            stream.ReadAsync(null!, 0, 10));
                
        // Act & Assert - Negative offset
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => 
            stream.ReadAsync(buffer, -1, 5));
                
        // Act & Assert - Negative count
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => 
            stream.ReadAsync(buffer, 0, -1));
                
        // Act & Assert - Offset + count > buffer length
        await Assert.ThrowsAsync<ArgumentException>(() => 
            stream.ReadAsync(buffer, 5, 6));
    }

    [Theory]
    [InlineData(new [] { 1, 10, 0, 0 })]
    [InlineData(new [] { 1, 10, 1, 0 })]
    [InlineData(new [] { 1, 10, 1, 11 })]
    [InlineData(new [] { 1, 10, 1, 11, 0 })]
    [InlineData(new [] { 1, 10, 1, 11, 0, 0 })]
    public async Task WriteAsync_WithTrits_ConvertsTwoWays(int[] trits)
    {
        // Arrange
        var memoryStream = new MemoryStream();
        var buffer = trits.Select(t => (Int3T)t).ToArray();
        await using var stream1 = new ByteToInt3TStream(memoryStream);
        await stream1.WriteAsync(buffer, 0, buffer.Length);
        await stream1.FlushAsync();
        memoryStream.Position = 0;
        var outputBuffer = new Int3T[buffer.Length + 2];
        await using var stream2 = new ByteToInt3TStream(memoryStream);
        await stream2.ReadAsync(outputBuffer, 1, buffer.Length);
        outputBuffer[0].Should().Be(0);
        outputBuffer[^1].Should().Be(0);
        outputBuffer[1..^1].Should().BeEquivalentTo(buffer);
    }

    [Fact]
    public async Task SeekAsync_ThrowsNotSupportedException()
    {
        // Arrange
        var memoryStream = new MemoryStream();
        var stream = new ByteToInt3TStream(memoryStream);
            
        // Act & Assert
        await Assert.ThrowsAsync<NotSupportedException>(() => 
            stream.SeekAsync(0, SeekOrigin.Begin));
    }

    [Fact]
    public async Task SetLengthAsync_ThrowsNotSupportedException()
    {
        // Arrange
        var memoryStream = new MemoryStream();
        var stream = new ByteToInt3TStream(memoryStream);
            
        // Act & Assert
        await Assert.ThrowsAsync<NotSupportedException>(() => 
            stream.SetLengthAsync(100));
    }

    [Fact]
    public void LengthAndPosition_ThrowNotSupportedException()
    {
        // Arrange
        var memoryStream = new MemoryStream();
        var stream = new ByteToInt3TStream(memoryStream);
            
        // Act & Assert - Length
        Action actLength = () => { var length = stream.Length; };
        actLength.Should().Throw<NotSupportedException>();
            
        // Act & Assert - Position get
        Action actPositionGet = () => { var position = stream.Position; };
        actPositionGet.Should().Throw<NotSupportedException>();
            
        // Act & Assert - Position set
        Action actPositionSet = () => { stream.Position = 0; };
        actPositionSet.Should().Throw<NotSupportedException>();
    }
}