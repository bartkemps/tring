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
        Action act = () => stream.SetLength(100);
        act.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void LengthAndPosition_ThrowNotSupportedException()
    {
        // Arrange
        var memoryInt3TStream = new MemoryInt3TStream();
        var stream = new Int3TToByteStream(memoryInt3TStream);
            
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
    
    [Theory]
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

        var buffer = new byte[200];
        
        var count = await stream1.ReadAsync(buffer, 0, buffer.Length);
        
        var outputStream = new MemoryInt3TStream();
        var stream2 = new Int3TToByteStream(outputStream);
        await stream2.WriteAsync(buffer, 0, count);
        
        var outputBuffer = outputStream.ToArray();
        
        // Assert
        outputBuffer.Take(tritsBuffer.Length).Should().BeEquivalentTo(tritsBuffer);
    }
    
    private class MockReadOnlyInt3TStream : Int3TStream
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;
        public override bool CanSeek => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public override Task<int> ReadAsync(Int3T[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        public override Task WriteAsync(Int3T[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("The stream does not support writing.");
        }

        public override Task<long> SeekAsync(long offset, SeekOrigin origin, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public override Task SetLengthAsync(long value, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public override Task FlushAsync(CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }
    }
}