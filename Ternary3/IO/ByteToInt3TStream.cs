namespace Ternary3.IO;

using System;
using System.IO;

/// <summary>
/// A stream that converts a byte stream to an Int3T stream.
/// </summary>
public sealed class ByteToInt3TStream(Stream source, bool mustWriteMagicNumber = true) : Int3TStream
{
    private bool canRead = source.CanRead;
    private bool canWrite = source.CanWrite;
    private BinaryTritEncoder encoder = new BinaryTritEncoder(mustWriteMagicNumber);
    public override async Task<int> ReadAsync(Int3T[] buffer, int offset, int count, CancellationToken cancellationToken = default)
    {
        if (!CanRead) throw new NotSupportedException("The stream does not support reading.");
        canWrite = false;
        throw new NotImplementedException();
    }

    public override async Task WriteAsync(Int3T[] buffer, int offset, int count, CancellationToken cancellationToken = default)
    {
        if (!CanWrite) throw new NotSupportedException("The stream does not support writing.");
        canRead = false;
        throw new NotImplementedException();
    }

    /// <summary>
    /// Flushes the stream, writing any buffered data to the underlying stream.
    /// Warning: flushing will restart part of the encoding, making size optimization impossible.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <exception cref="NotSupportedException"></exception>
    public override async Task FlushAsync(CancellationToken cancellationToken = default)
    {
        if (!CanWrite) throw new NotSupportedException("The stream does not support writing.");
        canRead = false;
        var bytes = encoder.Flush().ToArray();
        await source.WriteAsync(bytes, cancellationToken);
        await source.FlushAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override Task<long> SeekAsync(long offset, SeekOrigin origin, CancellationToken cancellationToken = default) => throw new NotSupportedException();
    /// <inheritdoc />
    public override Task SetLengthAsync(long value, CancellationToken cancellationToken = default) => throw new NotSupportedException();
    /// <inheritdoc />
    public override bool CanRead => source.CanRead;
    /// <inheritdoc />
    public override bool CanWrite => source.CanWrite;
    /// <inheritdoc />
    public override bool CanSeek => false;
    /// <inheritdoc />
    public override long Length => throw new NotSupportedException();
    /// <inheritdoc />
    public override long Position
    {
        get => throw new NotSupportedException("The stream does not support seeking.");
        set => throw new NotSupportedException("The stream does not support seeking.");
    }
}
