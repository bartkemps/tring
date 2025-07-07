namespace Ternary3.IO;

using System;
using System.IO;

/// <summary>
/// A stream that converts an Int3T stream to a byte stream.
/// </summary>
/// <remarks>
/// This stream reads Int3T values from the underlying Int3T stream and converts them to bytes
/// using the BinaryTritEncoder. The stream is not thread-safe for concurrent read/write operations.
/// External synchronization is required if accessed from multiple threads.
/// </remarks>
public sealed class Int3TToByteStream(Int3TStream source, bool mustWriteMagicNumber = true, bool leaveOpen = false) : Stream
{
    private readonly Int3TStream source = source ?? throw new ArgumentNullException(nameof(source));
    private bool canRead = source.CanRead;
    private bool canWrite = source.CanWrite;
    private readonly BinaryTritEncoder encoder = new(mustWriteMagicNumber);
    private bool disposed;

    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        if (disposed) throw new ObjectDisposedException(nameof(Int3TToByteStream));
        if (!CanRead) throw new NotSupportedException("The stream does not support reading.");
        canWrite = false;

        if (buffer == null) throw new ArgumentNullException(nameof(buffer));
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative.");
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
        if (offset + count > buffer.Length) throw new ArgumentException("The sum of offset and count is larger than the buffer length.");

        // No bytes to read
        if (count == 0) return 0;

        var tritBufferSize = (count - (encoder.MustWriteMagicNumber ? 1 : 0) - (encoder.MustWriteSectionHeader ? 1 : 0)) * 5 / 3;
        var tritBuffer = new Int3T[tritBufferSize]; // required buffer size

        var tritsRead = await source.ReadAsync(tritBuffer, 0, tritBufferSize, cancellationToken);

        // No trits available
        if (tritsRead == 0) return 0;

        // Encode the trits to bytes
        var bytes = encoder.Encode(tritBuffer[..tritsRead], true).ToArray();

        // Copy to the output buffer
        Array.Copy(bytes, 0, buffer, offset, bytes.Length);

        return bytes.Length;
    }

    public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        if (disposed) throw new ObjectDisposedException(nameof(Int3TToByteStream));
        if (!CanWrite) throw new NotSupportedException("The stream does not support writing.");
        canRead = false;

        if (buffer == null) throw new ArgumentNullException(nameof(buffer));
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative.");
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
        if (offset + count > buffer.Length) throw new ArgumentException("The sum of offset and count is larger than the buffer length.");

        // No bytes to write
        if (count == 0) return;

        // Extract the bytes we want to decode
        byte[] bytesToDecode;
        if (offset == 0 && count == buffer.Length)
        {
            bytesToDecode = buffer;
        }
        else
        {
            bytesToDecode = new byte[count];
            Array.Copy(buffer, offset, bytesToDecode, 0, count);
        }

        // Decode the bytes to trits
        var trits = encoder.Decode(bytesToDecode).ToArray();

        // Write the trits to the source stream
        if (trits.Length > 0)
        {
            await source.WriteAsync(trits, 0, trits.Length, cancellationToken);
        }
    }

    /// <inheritdoc />
    public override async Task FlushAsync(CancellationToken cancellationToken)
    {
        if (disposed) throw new ObjectDisposedException(nameof(Int3TToByteStream));
        if (!CanWrite) throw new NotSupportedException("The stream does not support writing.");
        canRead = false;
        await source.FlushAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override void Flush() => FlushAsync().GetAwaiter().GetResult();

    /// <inheritdoc />
    public override int Read(byte[] buffer, int offset, int count) => ReadAsync(buffer, offset, count).GetAwaiter().GetResult();

    /// <inheritdoc />
    public override void Write(byte[] buffer, int offset, int count) => WriteAsync(buffer, offset, count).GetAwaiter().GetResult();

    /// <inheritdoc />
    public override long Seek(long offset, SeekOrigin origin)
    {
        if (disposed) throw new ObjectDisposedException(nameof(Int3TToByteStream));
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public override void SetLength(long value)
    {
        if (disposed) throw new ObjectDisposedException(nameof(Int3TToByteStream));
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public override bool CanRead => !disposed && canRead && source.CanRead;

    /// <inheritdoc />
    public override bool CanWrite => !disposed && canWrite && source.CanWrite;

    /// <inheritdoc />
    public override bool CanSeek => false;

    /// <inheritdoc />
    public override long Length
    {
        get
        {
            if (disposed) throw new ObjectDisposedException(nameof(Int3TToByteStream));
            throw new NotSupportedException("The stream does not support seeking.");
        }
    }

    /// <inheritdoc />
    public override long Position
    {
        get
        {
            if (disposed) throw new ObjectDisposedException(nameof(Int3TToByteStream));
            throw new NotSupportedException("The stream does not support seeking.");
        }
        set
        {
            if (disposed) throw new ObjectDisposedException(nameof(Int3TToByteStream));
            throw new NotSupportedException("The stream does not support seeking.");
        }
    }

    /// <summary>
    /// Finalizer to ensure resources are cleaned up if Dispose is not called.
    /// </summary>
    ~Int3TToByteStream()
    {
        Dispose(false);
    }

    /// <summary>
    /// Releases the resources used by the stream.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        if (!disposed)
        {
            // Try to flush any pending data
            if (CanWrite)
            {
                try
                {
                    Flush();
                }
                catch
                {
                    // Ignore exceptions during disposal
                }
            }

            // Dispose the underlying stream
            if (!leaveOpen)
            {
                source.DisposeAsync().GetAwaiter().GetResult();
            }

            disposed = true;
        }

        base.Dispose(disposing);
    }

    /// <summary>
    /// Asynchronously releases all resources used by the stream.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public override async ValueTask DisposeAsync()
    {
        if (!disposed)
        {
            // Try to flush any pending data
            if (CanWrite)
            {
                try
                {
                    await FlushAsync().ConfigureAwait(false);
                }
                catch
                {
                    // Ignore exceptions during disposal
                }
            }

            // Dispose the underlying stream
            if (!leaveOpen)
            {
                await source.DisposeAsync().ConfigureAwait(false);
            }

            disposed = true;
        }

        await base.DisposeAsync().ConfigureAwait(false);

        GC.SuppressFinalize(this);
    }
}