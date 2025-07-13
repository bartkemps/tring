namespace Ternary3.Tests.IO;

using FluentAssertions;
using Ternary3.IO;

public class Int3TToByteStreamTests
{

    [Fact]
    public void Constructor_WithInt3TStream_CreatesWritableStream()
    {
        var memoryInt3TStream = new MemoryInt3TStream();
        var stream = new Int3TToByteStream(memoryInt3TStream);

        stream.CanRead.Should().BeTrue();
        stream.CanWrite.Should().BeTrue();
        stream.CanSeek.Should().BeFalse();
    }

    [Fact]
    public void Constructor_WithNullStream_ThrowsArgumentNullException()
    {
        Action act = () => new Int3TToByteStream(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task WriteAsync_WithInvalidParameters_ThrowsArgumentExceptions()
    {
        // Arrange
        var memoryInt3TStream = new MemoryInt3TStream();
        var stream = new Int3TToByteStream(memoryInt3TStream);
        var buffer = new byte[10];
            
        // Act & Assert - Null buffer
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            stream.WriteAsync(null!, 0, 10));
                
        // Act & Assert - Negative offset
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => 
            stream.WriteAsync(buffer, -1, 5));
                
        // Act & Assert - Negative count
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => 
            stream.WriteAsync(buffer, 0, -1));
                
        // Act & Assert - Offset + count > buffer length
        await Assert.ThrowsAsync<ArgumentException>(() => 
            stream.WriteAsync(buffer, 5, 6));
    }

    [Fact]
    public void SeekAsync_ThrowsNotSupportedException()
    {
        // Arrange
        var memoryInt3TStream = new MemoryInt3TStream();
        var stream = new Int3TToByteStream(memoryInt3TStream);
            
        // Act & Assert
        Assert.Throws<NotSupportedException>(() => 
            stream.Seek(0, SeekOrigin.Begin));
    }

    [Fact]
    public void SetLengthAsync_ThrowsNotSupportedException()
    {
        // Arrange
        var memoryInt3TStream = new MemoryInt3TStream();
        var stream = new Int3TToByteStream(memoryInt3TStream);
            
        // Act & Assert
        var act = () => stream.SetLength(100);
        act.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void LengthAndPosition_ThrowNotSupportedException()
    {
        // Arrange
        var memoryInt3TStream = new MemoryInt3TStream();
        var stream = new Int3TToByteStream(memoryInt3TStream);
            
        // Act & Assert - Length
        var actLength = () => { var length = stream.Length; };
        actLength.Should().Throw<NotSupportedException>();
            
        // Act & Assert - Position get
        var actPositionGet = () => { var position = stream.Position; };
        actPositionGet.Should().Throw<NotSupportedException>();
            
        // Act & Assert - Position set
        var actPositionSet = () => { stream.Position = 0; };
        actPositionSet.Should().Throw<NotSupportedException>();
    }
    
    [Theory]
    [InlineData(new int[] { })]
    [InlineData(new [] { 1 })]
    [InlineData(new [] { 1, 2 })]
    [InlineData(new [] { 1, 2, 3 })]
    [InlineData(new [] { 1, 2, 3, 4 })]
    [InlineData(new [] { 1, 2, 3, 4, 5 })]
    [InlineData(new [] { 1, 2, 3, 4, 5, 6 })]
    [InlineData(new [] { 1, 2, 3, 4, 5, 6, 7 })]
    [InlineData(new [] { 1, 2, 3, 4, 5, 6, 7, 8})]
    [InlineData(new [] { 1, 10, 0, 0 })]
    [InlineData(new [] { 1, 10, 1, 0 })]
    [InlineData(new [] { 1, 10, 1, 11 })]
    [InlineData(new [] { 1, 10, 1, 11, 0 })]
    [InlineData(new [] { 1, 10, 1, 11, 0, 0 })]
    public async Task WriteAsync_WithTrits_ConvertsTwoWays(int[] trits)
    {
        // Arrange
        var inputStream = new MemoryInt3TStream();
        var tritsBuffer = trits.Select(t => (Int3T)t).ToArray();
        await inputStream.WriteAsync(tritsBuffer, 0, tritsBuffer.Length);
        await inputStream.SeekAsync(0, SeekOrigin.Begin);
        var stream1 = new Int3TToByteStream(inputStream);

        var buffer = new byte[10];
        
        var count = await stream1.ReadAsync(buffer, 0, buffer.Length);
        
        var outputStream = new MemoryInt3TStream();
        var stream2 = new Int3TToByteStream(outputStream);
        await stream2.WriteAsync(buffer, 0, count);
        
        var outputBuffer = outputStream.ToArray();
        
        // Assert
        outputBuffer.Take(tritsBuffer.Length).Should().BeEquivalentTo(tritsBuffer);
    }

    [Fact]
    public async Task WriteAsync_SupportsMultipleReads()
    {
        var inputStream = new MemoryInt3TStream([-13,-12,-11,-10,-9,-8,-7]);
        await inputStream.SeekAsync(0, SeekOrigin.Begin);
        var stream1 = new Int3TToByteStream(inputStream);
        var buffer1 = new byte[12];
        var count1 = await stream1.ReadAsync(buffer1, 0, 4);
        var count2 = await stream1.ReadAsync(buffer1, 4, 4);
        var count3 = await stream1.ReadAsync(buffer1, 8, 4);
        var outputInt3TStream1 = new MemoryInt3TStream();
        var outputStream1 = new Int3TToByteStream(outputInt3TStream1);
        await outputStream1.WriteAsync(buffer1, 0, count1 + count2 + count3);
        var actual1 = outputInt3TStream1.ToArray();
        
        await inputStream.SeekAsync(0, SeekOrigin.Begin);
        var stream2 = new Int3TToByteStream(inputStream);
        var buffer2 = new byte[12];
        var count4 = await stream2.ReadAsync(buffer2, 0, 12);
        var outputInt3TStream2 = new MemoryInt3TStream();
        var outputStream2 = new Int3TToByteStream(outputInt3TStream2);
        await outputStream2.WriteAsync(buffer2, 0, count4);
        var actual2 = outputInt3TStream2.ToArray();
        
        (count1 + count2 + count3).Should().BeGreaterThan(count4);
        actual1.Should().BeEquivalentTo(inputStream.ToArray());
        actual2.Should().BeEquivalentTo(inputStream.ToArray());
    }
}