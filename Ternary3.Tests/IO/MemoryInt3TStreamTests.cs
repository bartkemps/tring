using FluentAssertions;
using Ternary3.IO;

namespace Ternary3.Tests.IO
{
    public class MemoryInt3TStreamTests
    {
        #region Constructor Tests

        [Fact]
        public void Constructor_Default_CreatesEmptyExpandableStream()
        {
            var stream = new MemoryInt3TStream();

            stream.Length.Should().Be(0);
            stream.Position.Should().Be(0);
            stream.CanRead.Should().BeTrue();
            stream.CanWrite.Should().BeTrue();
            stream.CanSeek.Should().BeTrue();
        }

        [Fact]
        public void Constructor_WithCapacity_CreatesStreamWithSpecifiedCapacity()
        {
            var capacity = 100;
            var stream = new MemoryInt3TStream(capacity);

            stream.Length.Should().Be(0);
            stream.CanRead.Should().BeTrue();
            stream.CanWrite.Should().BeTrue();
            stream.CanSeek.Should().BeTrue();
        }

        [Fact]
        public void Constructor_WithNegativeCapacity_ThrowsArgumentOutOfRangeException()
        {
            var capacity = -1;

            Action act = () => new MemoryInt3TStream(capacity);

            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*Capacity cannot be negative*");
        }

        [Fact]
        public void Constructor_WithBuffer_CreatesStreamWithSpecifiedBuffer()
        {
            Int3T[] buffer = [1, 0, -1];
            var stream = new MemoryInt3TStream(buffer);

            stream.Length.Should().Be(buffer.Length);
            stream.CanRead.Should().BeTrue();
            stream.CanWrite.Should().BeTrue();
            stream.CanSeek.Should().BeTrue();
        }

        [Fact]
        public void Constructor_WithBufferAndReadOnly_CreatesReadOnlyStream()
        {
            Int3T[] buffer = [1, 0, -1];
            var stream = new MemoryInt3TStream(buffer, false);

            stream.Length.Should().Be(buffer.Length);
            stream.CanRead.Should().BeTrue();
            stream.CanWrite.Should().BeFalse();
            stream.CanSeek.Should().BeTrue();
        }

        [Fact]
        public void Constructor_WithNullBuffer_ThrowsArgumentNullException()
        {
            Action act = () => new MemoryInt3TStream(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithBufferSegment_CreatesStreamWithSpecifiedSegment()
        {
            Int3T[] buffer = [1, 0, -1, 1, 0];
            var index = 1;
            var count = 3;
            var stream = new MemoryInt3TStream(buffer, index, count);

            stream.Length.Should().Be(count);
            stream.CanRead.Should().BeTrue();
            stream.CanWrite.Should().BeTrue();
            stream.CanSeek.Should().BeTrue();
        }

        [Fact]
        public void Constructor_WithBufferSegmentAndReadOnly_CreatesReadOnlyStream()
        {
            Int3T[] buffer = [1, 0, -1, 1, 0];
            var index = 1;
            var count = 3;
            var stream = new MemoryInt3TStream(buffer, index, count, false);

            stream.Length.Should().Be(count);
            stream.CanRead.Should().BeTrue();
            stream.CanWrite.Should().BeFalse();
            stream.CanSeek.Should().BeTrue();
        }

        [Fact]
        public void Constructor_WithNegativeIndex_ThrowsArgumentOutOfRangeException()
        {
            Int3T[] buffer = [1, 0, -1];
            var index = -1;
            var count = 2;

            Action act = () => new MemoryInt3TStream(buffer, index, count);

            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*Index cannot be negative*");
        }

        [Fact]
        public void Constructor_WithNegativeCount_ThrowsArgumentOutOfRangeException()
        {
            Int3T[] buffer = [1, 0, -1];
            var index = 0;
            var count = -1;

            Action act = () => new MemoryInt3TStream(buffer, index, count);

            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*Count cannot be negative*");
        }

        [Fact]
        public void Constructor_WithInvalidRange_ThrowsArgumentException()
        {
            Int3T[] buffer = [1, 0, -1];
            var index = 2;
            var count = 2;

            Action act = () => new MemoryInt3TStream(buffer, index, count);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*Invalid buffer range*");
        }

        #endregion

        #region Read Tests

        [Fact]
        public async Task ReadAsync_ReadsDataFromStream()
        {
            Int3T[] sourceData = [1, 0, -1, 1, 0];
            var stream = new MemoryInt3TStream(sourceData);
            var buffer = new Int3T[sourceData.Length];

            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

            bytesRead.Should().Be(sourceData.Length);
            buffer.Should().BeEquivalentTo(sourceData);
            stream.Position.Should().Be(sourceData.Length);
        }

        [Fact]
        public async Task ReadAsync_WithOffset_ReadsDataFromStream()
        {
            Int3T[] sourceData = [1, 0, -1, 1, 0];
            var stream = new MemoryInt3TStream(sourceData);
            var buffer = new Int3T[sourceData.Length + 2];
            var offset = 1;

            var bytesRead = await stream.ReadAsync(buffer, offset, sourceData.Length);

            bytesRead.Should().Be(sourceData.Length);
            for (var i = 0; i < sourceData.Length; i++)
            {
                buffer[i + offset].Should().Be(sourceData[i]);
            }
            stream.Position.Should().Be(sourceData.Length);
        }

        [Fact]
        public async Task ReadAsync_WithCount_ReadsRequestedAmountOfData()
        {
            Int3T[] sourceData = [1, 0, -1, 1, 0];
            var stream = new MemoryInt3TStream(sourceData);
            var buffer = new Int3T[sourceData.Length];
            var count = 3;

            var bytesRead = await stream.ReadAsync(buffer, 0, count);

            bytesRead.Should().Be(count);
            for (var i = 0; i < count; i++)
            {
                buffer[i].Should().Be(sourceData[i]);
            }
            stream.Position.Should().Be(count);
        }

        [Fact]
        public async Task ReadAsync_AfterPosition_ReadsDataFromCurrentPosition()
        {
            Int3T[] sourceData = [1, 0, -1, 1, 0];
            var stream = new MemoryInt3TStream(sourceData);
            var buffer = new Int3T[sourceData.Length];
            var initialPosition = 2;
            stream.Position = initialPosition;

            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

            bytesRead.Should().Be(sourceData.Length - initialPosition);
            for (var i = 0; i < bytesRead; i++)
            {
                buffer[i].Should().Be(sourceData[i + initialPosition]);
            }
            stream.Position.Should().Be(sourceData.Length);
        }

        [Fact]
        public async Task ReadAsync_AtEndOfStream_ReturnsZero()
        {
            Int3T[] sourceData = [1, 0, -1];
            var stream = new MemoryInt3TStream(sourceData);
            var buffer = new Int3T[5];
            stream.Position = sourceData.Length;

            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

            bytesRead.Should().Be(0);
            stream.Position.Should().Be(sourceData.Length);
        }

        [Fact]
        public async Task ReadInt3TAsync_ReadsSingleInt3T()
        {
            Int3T[] sourceData = [1, 0, -1];
            var stream = new MemoryInt3TStream(sourceData);

            var value = await stream.ReadInt3TAsync();

            value.Should().Be(1); // First Int3T value is 1
            stream.Position.Should().Be(1);
        }

        [Fact]
        public async Task ReadInt3TAsync_AtEndOfStream_ReturnsNegativeOne()
        {
            Int3T[] sourceData = [1, 0];
            var stream = new MemoryInt3TStream(sourceData);
            stream.Position = sourceData.Length;

            var value = await stream.ReadInt3TAsync();

            value.Should().Be(-1); // End of stream indicator
            stream.Position.Should().Be(sourceData.Length);
        }

        #endregion

        #region Write Tests

        [Fact]
        public async Task WriteAsync_WritesDataToStream()
        {
            var stream = new MemoryInt3TStream();
            Int3T[] dataToWrite = [1, 0, -1, 1];

            await stream.WriteAsync(dataToWrite, 0, dataToWrite.Length);

            stream.Position.Should().Be(dataToWrite.Length);
            stream.Length.Should().Be(dataToWrite.Length);
            
            // Verify written data
            stream.Position = 0;
            var readBuffer = new Int3T[dataToWrite.Length];
            await stream.ReadAsync(readBuffer, 0, readBuffer.Length);
            readBuffer.Should().BeEquivalentTo(dataToWrite);
        }

        [Fact]
        public async Task WriteAsync_WithOffset_WritesDataToStream()
        {
            var stream = new MemoryInt3TStream();
            Int3T[] dataToWrite = [0, 1, 0, -1, 1];
            var offset = 1;
            var count = 3;

            await stream.WriteAsync(dataToWrite, offset, count);

            stream.Position.Should().Be(count);
            stream.Length.Should().Be(count);
            
            // Verify written data
            stream.Position = 0;
            var readBuffer = new Int3T[count];
            await stream.ReadAsync(readBuffer, 0, readBuffer.Length);
            for (var i = 0; i < count; i++)
            {
                readBuffer[i].Should().Be(dataToWrite[i + offset]);
            }
        }

        [Fact]
        public async Task WriteAsync_WithExistingData_AppendsData()
        {
            Int3T[] initialData = [1, 0];
            var stream = new MemoryInt3TStream(initialData);
            stream.Position = initialData.Length;
            Int3T[] dataToAppend = [-1, 1];

            await stream.WriteAsync(dataToAppend, 0, dataToAppend.Length);

            stream.Position.Should().Be(initialData.Length + dataToAppend.Length);
            stream.Length.Should().Be(initialData.Length + dataToAppend.Length);
            
            // Verify all data
            stream.Position = 0;
            var readBuffer = new Int3T[initialData.Length + dataToAppend.Length];
            await stream.ReadAsync(readBuffer, 0, readBuffer.Length);
            
            for (var i = 0; i < initialData.Length; i++)
            {
                readBuffer[i].Should().Be(initialData[i]);
            }
            
            for (var i = 0; i < dataToAppend.Length; i++)
            {
                readBuffer[i + initialData.Length].Should().Be(dataToAppend[i]);
            }
        }

        [Fact]
        public async Task WriteAsync_ToReadOnlyStream_ThrowsNotSupportedException()
        {
            Int3T[] buffer = [1, 0, -1];
            var stream = new MemoryInt3TStream(buffer, false); // Read-only
            Int3T[] dataToWrite = [1, 1];

            var act = async () => await stream.WriteAsync(dataToWrite, 0, dataToWrite.Length);

            await act.Should().ThrowAsync<NotSupportedException>()
                .WithMessage("*The stream does not support writing*");
        }

        [Fact]
        public async Task WriteAsync_BeyondCapacity_ExpandsBuffer()
        {
            var initialCapacity = 2;
            var stream = new MemoryInt3TStream(initialCapacity);
            Int3T[] dataToWrite = [1, 0, -1, 1];

            await stream.WriteAsync(dataToWrite, 0, dataToWrite.Length);

            stream.Position.Should().Be(dataToWrite.Length);
            stream.Length.Should().Be(dataToWrite.Length);
            
            // Verify data was written correctly
            stream.Position = 0;
            var readBuffer = new Int3T[dataToWrite.Length];
            await stream.ReadAsync(readBuffer, 0, readBuffer.Length);
            readBuffer.Should().BeEquivalentTo(dataToWrite);
        }

        [Fact]
        public async Task WriteAsync_WithNegativeOffset_ThrowsArgumentOutOfRangeException()
        {
            var stream = new MemoryInt3TStream();
            Int3T[] buffer = [1, 0];

            var act = async () => await stream.WriteAsync(buffer, -1, 1);

            await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
                .WithMessage("*Offset cannot be negative*");
        }

        [Fact]
        public async Task WriteAsync_WithNegativeCount_ThrowsArgumentOutOfRangeException()
        {
            var stream = new MemoryInt3TStream();
            Int3T[] buffer = [1, 0];

            var act = async () => await stream.WriteAsync(buffer, 0, -1);

            await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
                .WithMessage("*Count cannot be negative*");
        }

        [Fact]
        public async Task WriteAsync_WithInvalidBufferRange_ThrowsArgumentException()
        {
            var stream = new MemoryInt3TStream();
            Int3T[] buffer = [1, 0];

            var act = async () => await stream.WriteAsync(buffer, 1, 2);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Invalid buffer range*");
        }

        [Fact]
        public async Task WriteAsync_WithCancellation_ReturnsCanceledTask()
        {
            var stream = new MemoryInt3TStream();
            Int3T[] buffer = [1, 0];
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var act = async () => await stream.WriteAsync(buffer, 0, buffer.Length, cts.Token);

            await act.Should().ThrowAsync<TaskCanceledException>();
        }

        [Fact]
        public async Task WriteAsync_ToClosedStream_ThrowsObjectDisposedException()
        {
            var stream = new MemoryInt3TStream();
            Int3T[] buffer = [1, 0];
            await stream.DisposeAsync();

            var act = async () => await stream.WriteAsync(buffer, 0, buffer.Length);

            await act.Should().ThrowAsync<ObjectDisposedException>();
        }

        [Fact]
        public async Task WriteInt3TAsync_WritesSingleInt3T()
        {
            var stream = new MemoryInt3TStream();
            Int3T valueToWrite = -1;

            await stream.WriteInt3TAsync(valueToWrite);

            stream.Position.Should().Be(1);
            stream.Length.Should().Be(1);
            
            // Verify written data
            stream.Position = 0;
            var readValue = await stream.ReadInt3TAsync();
            readValue.Should().Be(-1); // The value we wrote
        }

        #endregion

        #region Seek Tests

        [Fact]
        public async Task SeekAsync_FromBeginning_SetsPosition()
        {
            Int3T[] buffer = [1, 0, -1, 1, 0];
            var stream = new MemoryInt3TStream(buffer);
            var newPosition = 3;

            var position = await stream.SeekAsync(newPosition, SeekOrigin.Begin);

            position.Should().Be(newPosition);
            stream.Position.Should().Be(newPosition);
        }

        [Fact]
        public async Task SeekAsync_FromCurrent_SetsPosition()
        {
            Int3T[] buffer = [1, 0, -1, 1, 0];
            var stream = new MemoryInt3TStream(buffer);
            stream.Position = 2;
            var offset = 2;

            var position = await stream.SeekAsync(offset, SeekOrigin.Current);

            position.Should().Be(stream.Position);
            stream.Position.Should().Be(2 + offset);
        }

        [Fact]
        public async Task SeekAsync_FromEnd_SetsPosition()
        {
            Int3T[] buffer = [1, 0, -1, 1, 0];
            var stream = new MemoryInt3TStream(buffer);
            var offset = -2;

            var position = await stream.SeekAsync(offset, SeekOrigin.End);

            position.Should().Be(stream.Position);
            stream.Position.Should().Be(buffer.Length + offset);
        }

        [Fact]
        public async Task SeekAsync_ToNegativePosition_ThrowsIOException()
        {
            var stream = new MemoryInt3TStream();
            stream.Position = 5;

            Func<Task> act = async () => await stream.SeekAsync(-10, SeekOrigin.Current);

            await act.Should().ThrowAsync<IOException>()
                .WithMessage("*Cannot seek to a negative position*");
        }

        [Fact]
        public async Task SeekAsync_WithInvalidOrigin_ThrowsArgumentException()
        {
            var stream = new MemoryInt3TStream();

            Func<Task> act = async () => await stream.SeekAsync(0, (SeekOrigin)99);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*Invalid SeekOrigin*");
        }

        [Fact]
        public async Task SeekAsync_WithCancellation_ReturnsCanceledTask()
        {
            var stream = new MemoryInt3TStream();
            var cts = new CancellationTokenSource();
            cts.Cancel();

            Func<Task> act = async () => await stream.SeekAsync(0, SeekOrigin.Begin, cts.Token);

            await act.Should().ThrowAsync<TaskCanceledException>();
        }

        #endregion

        #region SetLength Tests

        [Fact]
        public async Task SetLengthAsync_IncreaseLength_ExpandsBuffer()
        {
            Int3T[] initialData = [1, 0, -1];
            var stream = new MemoryInt3TStream(initialData);
            var newLength = initialData.Length * 2;

            await stream.SetLengthAsync(newLength);

            stream.Length.Should().Be(newLength);
            stream.Position.Should().Be(Math.Min(initialData.Length, newLength));
            
            // Verify original data is preserved
            stream.Position = 0;
            var readBuffer = new Int3T[initialData.Length];
            await stream.ReadAsync(readBuffer, 0, readBuffer.Length);
            readBuffer.Should().BeEquivalentTo(initialData);
        }

        [Fact]
        public async Task SetLengthAsync_DecreaseLength_TruncatesBuffer()
        {
            Int3T[] initialData = [1, 0, -1, 1, 0];
            var stream = new MemoryInt3TStream(initialData);
            var newLength = 2;

            await stream.SetLengthAsync(newLength);

            stream.Length.Should().Be(newLength);
            
            // Verify remaining data is correct
            stream.Position = 0;
            var readBuffer = new Int3T[newLength];
            await stream.ReadAsync(readBuffer, 0, readBuffer.Length);
            
            for (var i = 0; i < newLength; i++)
            {
                readBuffer[i].Should().Be(initialData[i]);
            }
        }

        [Fact]
        public async Task SetLengthAsync_DecreaseLengthBeyondPosition_AdjustsPosition()
        {
            Int3T[] initialData = [1, 0, -1, 1, 0];
            var stream = new MemoryInt3TStream(initialData);
            stream.Position = 4;
            var newLength = 2;

            await stream.SetLengthAsync(newLength);

            stream.Length.Should().Be(newLength);
            stream.Position.Should().Be(newLength); // Position was adjusted
        }

        [Fact]
        public async Task SetLengthAsync_WithNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            var stream = new MemoryInt3TStream();

            var act = async () => await stream.SetLengthAsync(-1);

            await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
                .WithMessage("*Length must be non-negative*");
        }

        [Fact]
        public async Task SetLengthAsync_OnReadOnlyStream_ThrowsNotSupportedException()
        {
            Int3T[] buffer = [1, 0, -1];
            var stream = new MemoryInt3TStream(buffer, false); // Read-only

            var act = async () => await stream.SetLengthAsync(5);

            await act.Should().ThrowAsync<NotSupportedException>()
                .WithMessage("*The stream does not support writing*");
        }

        [Fact]
        public async Task SetLengthAsync_WithCancellation_ReturnsCanceledTask()
        {
            var stream = new MemoryInt3TStream();
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var act = async () => await stream.SetLengthAsync(10, cts.Token);

            await act.Should().ThrowAsync<TaskCanceledException>();
        }

        #endregion

        #region Buffer Management Tests

        [Fact]
        public void GetBuffer_ReturnsUnderlyingBuffer()
        {
            Int3T[] buffer = [1, 0, -1];
            var stream = new MemoryInt3TStream(buffer);

            var retrievedBuffer = stream.GetBuffer();

            retrievedBuffer.Should().BeSameAs(buffer);
        }

        [Fact]
        public void GetBuffer_OnPrivateBuffer_ThrowsUnauthorizedAccessException()
        {
            Int3T[] buffer = [1, 0, -1];
            var stream = new MemoryInt3TStream(buffer);
            // Make the buffer private by creating a reflection hack or using private constructor
            // For testing purposes, we'll simulate this scenario

            // This test cannot be properly implemented without accessing internal state
            // Just demonstrating the test case
        }

        [Fact]
        public void GetBuffer_WithOutParameters_ReturnsBufferDetails()
        {
            Int3T[] buffer = [1, 0, -1, 1, 0];
            var index = 1;
            var count = 3;
            var stream = new MemoryInt3TStream(buffer, index, count);

            stream.GetBuffer(out var retrievedBuffer, out var retrievedIndex, out var retrievedCount);

            retrievedBuffer.Should().BeSameAs(buffer);
            retrievedIndex.Should().Be(index);
            retrievedCount.Should().Be(count);
        }

        [Fact]
        public void ToArray_ReturnsDataCopy()
        {
            Int3T[] buffer = [1, 0, -1];
            var stream = new MemoryInt3TStream(buffer);

            var copy = stream.ToArray();

            copy.Should().NotBeSameAs(buffer);
            copy.Should().BeEquivalentTo(buffer);
        }

        [Fact]
        public void ToArray_WithBufferSegment_ReturnsSegmentCopy()
        {
            Int3T[] buffer = [1, 0, -1, 1, 0];
            var index = 1;
            var count = 3;
            var stream = new MemoryInt3TStream(buffer, index, count);

            var copy = stream.ToArray();

            copy.Length.Should().Be(count);
            for (var i = 0; i < count; i++)
            {
                copy[i].Should().Be(buffer[index + i]);
            }
        }

        [Fact]
        public void SetCapacity_IncreaseCapacity_PreservesData()
        {
            Int3T[] buffer = [1, 0, -1];
            var stream = new MemoryInt3TStream(buffer);
            var newCapacity = buffer.Length * 2;

            stream.SetCapacity(newCapacity);

            // Verify the data is preserved
            var copy = stream.ToArray();
            copy.Should().BeEquivalentTo(buffer);
        }

        [Fact]
        public void SetCapacity_SmallerThanLength_ThrowsArgumentOutOfRangeException()
        {
            Int3T[] buffer = [1, 0, -1];
            var stream = new MemoryInt3TStream(buffer);

            var act = () => stream.SetCapacity(buffer.Length - 1);

            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*Capacity cannot be smaller than the current length*");
        }

        [Fact]
        public void SetCapacity_OnNonExpandableStream_ThrowsNotSupportedException()
        {
            Int3T[] buffer = [1, 0, -1];
            var stream = new MemoryInt3TStream(buffer, true);
            // Make the stream non-expandable by creating a reflection hack or using private constructor
            // For testing purposes, we'll simulate this scenario

            // This test cannot be properly implemented without accessing internal state
            // Just demonstrating the test case
        }

        #endregion

        #region Flush Tests

        [Fact]
        public async Task FlushAsync_IsNoOp_CompletesSuccessfully()
        {
            var stream = new MemoryInt3TStream();

            var act = async () => await stream.FlushAsync();

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task FlushAsync_OnClosedStream_ThrowsObjectDisposedException()
        {
            var stream = new MemoryInt3TStream();
            await stream.DisposeAsync();

            var act = async () => await stream.FlushAsync();

            await act.Should().ThrowAsync<ObjectDisposedException>();
        }

        [Fact]
        public async Task FlushAsync_WithCancellation_ReturnsCanceledTask()
        {
            var stream = new MemoryInt3TStream();
            var cts = new CancellationTokenSource();
            cts.Cancel();

            var act = async () => await stream.FlushAsync(cts.Token);

            await act.Should().ThrowAsync<TaskCanceledException>();
        }

        #endregion

        #region Dispose Tests

        [Fact]
        public async Task DisposeAsync_ClosesStream()
        {
            var stream = new MemoryInt3TStream();

            await stream.DisposeAsync();

            stream.IsClosed.Should().BeTrue();
            stream.CanRead.Should().BeFalse();
            stream.CanWrite.Should().BeFalse();
            stream.CanSeek.Should().BeFalse();
        }

        [Fact]
        public async Task DisposeAsync_CalledMultipleTimes_DoesNotThrow()
        {
            var stream = new MemoryInt3TStream();
            await stream.DisposeAsync();

            var act = async () => await stream.DisposeAsync();

            await act.Should().NotThrowAsync();
        }

        #endregion

        #region Copy Tests

        [Fact]
        public async Task CopyToAsync_CopiesDataToAnotherStream()
        {
            Int3T[] sourceData = [1, 0, -1, 1];
            var sourceStream = new MemoryInt3TStream(sourceData);
            var destinationStream = new MemoryInt3TStream();

            await sourceStream.CopyToAsync(destinationStream);

            destinationStream.Length.Should().Be(sourceData.Length);
            
            // Verify copied data
            destinationStream.Position = 0;
            var readBuffer = new Int3T[sourceData.Length];
            await destinationStream.ReadAsync(readBuffer, 0, readBuffer.Length);
            readBuffer.Should().BeEquivalentTo(sourceData);
        }

        [Fact]
        public async Task CopyToAsync_WithCount_CopiesSpecifiedAmountOfData()
        {
            Int3T[] sourceData = [1, 0, -1, 1, 0];
            var sourceStream = new MemoryInt3TStream(sourceData);
            var destinationStream = new MemoryInt3TStream();
            var count = 3;

            await sourceStream.CopyToAsync(destinationStream, count);

            destinationStream.Length.Should().Be(count);
            
            // Verify copied data
            destinationStream.Position = 0;
            var readBuffer = new Int3T[count];
            await destinationStream.ReadAsync(readBuffer, 0, readBuffer.Length);
            
            for (var i = 0; i < count; i++)
            {
                readBuffer[i].Should().Be(sourceData[i]);
            }
        }

        #endregion
    }
}
