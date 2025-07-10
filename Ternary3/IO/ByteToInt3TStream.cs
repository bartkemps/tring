namespace Ternary3.IO;

using System;
using System.IO;

/// <summary>
/// A stream that converts a byte stream to an Int3T stream.
/// </summary>
/// <remarks>
/// This stream reads bytes from the underlying byte stream and converts them to Int3T values
/// using the BinaryTritEncoder. The stream is not thread-safe for concurrent read/write operations.
/// External synchronization is required if accessed from multiple threads.
/// </remarks>
public sealed class ByteToInt3TStream(Stream source, bool mustWriteMagicNumber = true, bool leaveOpen = false) : Int3TStream
{
    private readonly Stream source = source ?? throw new ArgumentNullException(nameof(source));
    private readonly BinaryTritEncoder encoder = new(mustWriteMagicNumber);
    private bool canRead = source.CanRead;
    private bool canWrite = source.CanWrite;
    private int tritBufferPosition = 0;
    private List<Int3T> tritBuffer = []; // Buffer for decoded trits
    private readonly byte[] byteBuffer = new byte[4096]; // Buffer for reading bytes

    /// <summary>
    /// Asynchronously reads a sequence of Int3T values from the stream.
    /// </summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="offset">The offset in the buffer at which to begin writing data.</param>
    /// <param name="count">The maximum number of Int3T values to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task that represents the asynchronous read operation. The value is
    /// the total number of Int3T values read into the buffer.
    /// </returns>
    /// <exception cref="ArgumentNullException">buffer is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">offset or count is negative.</exception>
    /// <exception cref="NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="ObjectDisposedException">The stream has been disposed.</exception>
    public override async Task<int> ReadAsync(Int3T[] buffer, int offset, int count, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(Disposed, this);
        if (!CanRead) throw new NotSupportedException("The stream does not support reading.");
        canWrite = false;
        ArgumentNullException.ThrowIfNull(buffer);
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative.");
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
        if (offset + count > buffer.Length) throw new ArgumentException("The sum of offset and count is larger than the buffer length.");

        // If we have trits in the buffer, use those first
        var tritsCopied = 0;
        if (tritBuffer.Count > tritBufferPosition)
        {
            var tritsAvailable = tritBuffer.Count - tritBufferPosition;
            var tritsToCopy = Math.Min(count, tritsAvailable);
            
            for (var i = 0; i < tritsToCopy; i++)
            {
                buffer[offset + i] = tritBuffer[tritBufferPosition + i];
            }
            
            tritBufferPosition += tritsToCopy;
            tritsCopied = tritsToCopy;
            
            // If we've copied enough, return
            if (tritsCopied == count) return tritsCopied;
        }

        // If we need more trits, read from the source stream
        var bytesRead = await source.ReadAsync(byteBuffer, 0, byteBuffer.Length, cancellationToken);
        if (bytesRead == 0) return tritsCopied; // End of stream

        // Decode the bytes to trits
        tritBuffer = encoder.Decode(byteBuffer.Take(bytesRead)).ToList();
        tritBufferPosition = 0;

        // Copy trits to the output buffer
        var remainingTritsToCopy = Math.Min(count - tritsCopied, tritBuffer.Count);
        for (var i = 0; i < remainingTritsToCopy; i++)
        {
            buffer[offset + tritsCopied + i] = tritBuffer[tritBufferPosition + i];
        }
        
        tritBufferPosition += remainingTritsToCopy;
        tritsCopied += remainingTritsToCopy;

        return tritsCopied;
    }

    /// <summary>
    /// Asynchronously writes a sequence of Int3T values to the stream.
    /// </summary>
    /// <param name="buffer">The buffer containing the data to write.</param>
    /// <param name="offset">The offset in the buffer from which to begin writing data.</param>
    /// <param name="count">The number of Int3T values to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    /// <exception cref="ArgumentNullException">buffer is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">offset or count is negative.</exception>
    /// <exception cref="NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="ObjectDisposedException">The stream has been disposed.</exception>
    public override async Task WriteAsync(Int3T[] buffer, int offset, int count, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(Disposed, this);
        if (!CanWrite) throw new NotSupportedException("The stream does not support writing.");
        canRead = false;

        if (buffer == null) throw new ArgumentNullException(nameof(buffer));
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative.");
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
        if (offset + count > buffer.Length) throw new ArgumentException("The sum of offset and count is larger than the buffer length.");

        // Get the portion of the buffer we want to encode
        var tritsToEncode = buffer.Skip(offset).Take(count);
        
        // Encode the Int3T values to bytes and write them to the source stream
        var bytesToWrite = encoder.Encode(tritsToEncode).ToArray();
        await source.WriteAsync(bytesToWrite, 0, bytesToWrite.Length, cancellationToken);
    }

    /// <summary>
    /// Flushes the stream, writing any buffered data to the underlying stream.
    /// Warning: flushing will restart part of the encoding, making size optimization impossible.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <exception cref="NotSupportedException"></exception>
    /// <exception cref="ObjectDisposedException">The stream has been disposed.</exception>
    public override async Task FlushAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(Disposed, this);
        if (!CanWrite) throw new NotSupportedException("The stream does not support writing.");
        canRead = false;
        var bytes = encoder.Flush().ToArray();
        await source.WriteAsync(bytes, cancellationToken);
        await source.FlushAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override Task<long> SeekAsync(long offset, SeekOrigin origin, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(Disposed, this);
        return Task.FromException<long>(new NotSupportedException());
    }

    /// <inheritdoc />
    public override Task SetLengthAsync(long value, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(Disposed, this);
        return Task.FromException<long>(new NotSupportedException());
    }

    /// <inheritdoc />
    public override bool CanRead => !Disposed && canRead && source.CanRead;

    /// <inheritdoc />
    public override bool CanWrite => !Disposed && canWrite && source.CanWrite;

    /// <inheritdoc />
    public override bool CanSeek => false;

    /// <inheritdoc />
    public override long Length
    {
        get
        {
            ObjectDisposedException.ThrowIf(Disposed, this);
            throw new NotSupportedException();
        }
    }

    /// <inheritdoc />
    public override long Position
    {
        get
        {
            ObjectDisposedException.ThrowIf(Disposed, this);
            throw new NotSupportedException("The stream does not support seeking.");
        }
        set
        {
            ObjectDisposedException.ThrowIf(Disposed, this);
            throw new NotSupportedException("The stream does not support seeking.");
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the stream and optionally releases the managed resources.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    protected override async ValueTask DisposeAsyncCore()
    {
        // Call the base implementation which handles flushing
        await base.DisposeAsyncCore().ConfigureAwait(false);
        
        // Dispose the underlying stream
        if (!leaveOpen)
        {
            await source.DisposeAsync().ConfigureAwait(false);
        }
    }
}
