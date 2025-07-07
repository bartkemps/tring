namespace Ternary3.IO;

using System;
using System.IO;

/// <summary>
/// A stream that converts an Int3T stream to a byte stream.
/// </summary>
public sealed class Int3ToByteStream(Int3TStream source, bool mustWriteMagicNumber = true) : Stream
{
    private bool canRead = source.CanRead;
    private bool canWrite = source.CanWrite;
    private BinaryTritEncoder encoder = new BinaryTritEncoder(mustWriteMagicNumber);
    
    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        if (!CanRead) throw new NotSupportedException("The stream does not support reading.");
        canWrite = false;
        throw new NotImplementedException();
    }

    public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        if (!CanWrite) throw new NotSupportedException("The stream does not support writing.");
        canRead = false;
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override async Task FlushAsync(CancellationToken cancellationToken)
    {
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
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
    /// <inheritdoc />
    public override void SetLength(long value)=> throw new NotSupportedException();
    /// <inheritdoc />
    public override bool CanRead => canRead && source.CanRead;
    /// <inheritdoc />
    public override bool CanWrite => canWrite && source.CanWrite;
    /// <inheritdoc />
    public override bool CanSeek => false;
    /// <inheritdoc />
    public override long Length => throw new NotSupportedException("The stream does not support seeking.");
    /// <inheritdoc />
    public override long Position
    {
        get => throw new NotSupportedException("The stream does not support seeking.");
        set => throw new NotSupportedException("The stream does not support seeking.");
    }
}
