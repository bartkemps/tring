namespace Ternary3.IO;

using TernaryArrays;

/// <summary>
/// Provides a reader for reading ternary data types from an Int3TStream.
/// </summary>
/// <remarks>
/// TernaryReader is designed to read ternary data types like Trit, Int3T, Int9T, Int27T, 
/// and various TernaryArray types from an underlying Int3TStream.
/// </remarks>
/// <example>
///<code>
/// await using var stream = new MemoryInt3TStream();
/// await using var writer = new TernaryWriter(stream);
/// await using var reader = new TernaryReader(stream);
/// var int9TValue = (Int9T)ter1000T; // 81 - 1 = 80;;
/// await writer.WriteAsync(int9TValue);
///         
/// // Reset stream position
/// stream.Position = 0;
///         
/// // Read Int9TArray. This should be 80.
/// var readArray = await reader.ReadTernaryArray9Async();
/// var convertedArray = (Int9T)readArray;
///         
/// // Reset stream position
/// stream.Position = 0;
///         
/// // Read Int9T. This should also be 80.
/// var readInt9T = await reader.ReadInt9TAsync();
/// </code>
/// </example>
public class TernaryReader : IAsyncDisposable
{
    private readonly Int3TStream stream;
    private bool disposed;
    private readonly Int3T[] buffer = new Int3T[9]; // Reusable buffer for reading ternary values

    /// <summary>
    /// Initializes a new instance of the <see cref="TernaryReader"/> class based on the specified stream.
    /// </summary>
    /// <param name="stream">The Int3TStream to read from.</param>
    /// <exception cref="ArgumentNullException">stream is null.</exception>
    /// <exception cref="ArgumentException">stream does not support reading.</exception>
    public TernaryReader(Int3TStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (!stream.CanRead)
        {
            throw new ArgumentException("Stream does not support reading.", nameof(stream));
        }

        this.stream = stream;
    }

    /// <summary>
    /// Gets the underlying Int3TStream associated with the TernaryReader.
    /// </summary>
    /// <value>The Int3TStream associated with the TernaryReader.</value>
    public Int3TStream BaseStream => stream;

    /// <summary>
    /// Reads a Trit value from the stream.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation with the read Trit value.</returns>
    public async Task<Trit> ReadTritAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        var value = await stream.ReadInt3TAsync(cancellationToken);
        return new Trit(value);
    }

    /// <summary>
    /// Reads a Trit value from the stream as a nullable boolean.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation with the read Trit value as a nullable boolean.</returns>
    public async Task<bool?> ReadNullableBooleanAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        var value = await stream.ReadInt3TAsync(cancellationToken);
        return value[0];
    }

    /// <summary>
    /// Reads an Int3T value from the stream.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation with the read Int3T value.</returns>
    public async Task<Int3T> ReadInt3TAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        return await stream.ReadInt3TAsync(cancellationToken);
    }

    /// <summary>
    /// Reads an Int9T value from the stream.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation with the read Int9T value.</returns>
    public async Task<Int9T> ReadInt9TAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        var count = await stream.ReadAsync(buffer, 0, 3, cancellationToken);
        if (count < 3)
        {
            throw new EndOfStreamException("Unexpected end of stream while reading Int9T value.");
        }

        return (Int9T)(buffer[0] + (int)buffer[1] * 27 + buffer[2] * 729);
    }

    /// <summary>
    /// Reads an Int27T value from the stream.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation with the read Int27T value.</returns>
    public async Task<Int27T> ReadInt27TAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        var count = await stream.ReadAsync(buffer, 0, 9, cancellationToken);
        if (count < 9)
        {
            throw new EndOfStreamException("Unexpected end of stream while reading Int27T value.");
        }

        return
            buffer[0] +
            buffer[1] * 27L +
            buffer[2] * 729L +
            buffer[3] * 19683L +
            buffer[4] * 531441L +
            buffer[5] * 14348907L +
            buffer[6] * 387420489L +
            buffer[7] * 10460353203L +
            buffer[8] * 282429536481L;
    }

    /// <summary>
    /// Reads a TernaryArray3 from the stream.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation with the read TernaryArray3 value.</returns>
    public async Task<TernaryArray3> ReadTernaryArray3Async(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        return await stream.ReadInt3TAsync(cancellationToken);
    }

    /// <summary>
    /// Reads a TernaryArray9 from the stream.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation with the read TernaryArray9 value.</returns>
    public async Task<TernaryArray9> ReadTernaryArray9Async(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        var count = await stream.ReadAsync(buffer, 0, 3, cancellationToken);
        if (count < 3)
        {
            throw new EndOfStreamException("Unexpected end of stream while reading TernaryArray9 value.");
        }
        var positive  =
            TritConverter.LookupTrits(-buffer[0]) |
            (TritConverter.LookupTrits(-buffer[1]) << 3) |
            (TritConverter.LookupTrits(-buffer[2]) << 6);
        var negative  = 
            TritConverter.LookupTrits(buffer[0]) |
            (TritConverter.LookupTrits(buffer[1]) << 3) |
            (TritConverter.LookupTrits(buffer[2]) << 6);
        return new TernaryArray9((ushort)negative, (ushort)positive);
    }

    /// <summary>
    /// Reads a TernaryArray27 from the stream.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation with the read TernaryArray27 value.</returns>
    public async Task<TernaryArray27> ReadTernaryArray27Async(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        var count = await stream.ReadAsync(buffer, 0, 9, cancellationToken);
        if (count < 9)
        {
            throw new EndOfStreamException("Unexpected end of stream while reading TernaryArray27 value.");
        }

        var positive =
            TritConverter.LookupTrits(-buffer[0]) |
            (TritConverter.LookupTrits(-buffer[1]) << 3) |
            (TritConverter.LookupTrits(-buffer[2]) << 6) |
            (TritConverter.LookupTrits(-buffer[3]) << 9) |
            (TritConverter.LookupTrits(-buffer[4]) << 12) |
            (TritConverter.LookupTrits(-buffer[5]) << 15) |
            (TritConverter.LookupTrits(-buffer[6]) << 18) |
            (TritConverter.LookupTrits(-buffer[7]) << 21) |
            (TritConverter.LookupTrits(-buffer[8]) << 24);
            
        var negative  =
            TritConverter.LookupTrits(buffer[0]) |
            (TritConverter.LookupTrits(buffer[1]) << 3) |
            (TritConverter.LookupTrits(buffer[2]) << 6) |
            (TritConverter.LookupTrits(buffer[3]) << 9) |
            (TritConverter.LookupTrits(buffer[4]) << 12) |
            (TritConverter.LookupTrits(buffer[5]) << 15) |
            (TritConverter.LookupTrits(buffer[6]) << 18) |
            (TritConverter.LookupTrits(buffer[7]) << 21) |
            (TritConverter.LookupTrits(buffer[8]) << 24);
        return new TernaryArray27(negative, positive);
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(disposed, this);

    /// <summary>
    /// Asynchronously disposes of the current reader and the underlying stream.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (!disposed)
        {
            disposed = true;
            await stream.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => $"TernaryReader: BaseStream={stream}";

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => stream.GetHashCode();

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is TernaryReader other && stream.Equals(other.stream);
}