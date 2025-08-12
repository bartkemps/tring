namespace Ternary3.IO;

using TernaryArrays;

/// <summary>
/// Provides a writer for writing ternary data types to an Int3TStream.
/// </summary>
/// <remarks>
/// TernaryWriter is designed to write ternary data types like Trit, Int3T, Int9T, Int27T, 
/// and various TernaryArray types to an underlying Int3TStream.
/// </remarks>
/// <example>
/// await using var fileStream = File.Create("MyNumber.data.t3");
/// await using var ternaryFileStream = new ByteToInt3TStream(fileStream);
/// await using var ternaryWriter = new TernaryWriter(ternaryFileStream);
/// await ternaryWriter.WriteAsync((Int27T)ter111000111TTT000TTT111000111);
/// </example>
public class TernaryWriter : IAsyncDisposable
{
    private readonly Int3TStream stream;
    private bool disposed;
    private readonly Int3T[] buffer = new Int3T[9]; // Reusable buffer for writing ternary values

    /// <summary>
    /// Initializes a new instance of the <see cref="TernaryWriter"/> class based on the specified stream.
    /// </summary>
    /// <param name="stream">The Int3TStream to write to.</param>
    /// <exception cref="ArgumentNullException">stream is null.</exception>
    /// <exception cref="ArgumentException">stream does not support writing.</exception>
    public TernaryWriter(Int3TStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (!stream.CanWrite)
        {
            throw new ArgumentException("Stream does not support writing.", nameof(stream));
        }

        this.stream = stream;
    }

    /// <summary>
    /// Gets the underlying Int3TStream associated with the TernaryWriter.
    /// </summary>
    /// <value>The Int3TStream associated with the TernaryWriter.</value>
    public Int3TStream BaseStream => stream;

    /// <summary>
    /// Writes a Trit value to the stream.
    /// </summary>
    /// <param name="value">The Trit value to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public async Task WriteAsync(Trit value, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        await stream.WriteInt3TAsync(value.Value, cancellationToken);
    }
    
    /// <summary>
    /// Writes a Trit value to the stream.
    /// </summary>
    /// <param name="value">The Trit value to write, as a boolean</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public async Task WriteAsync(bool? value, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        await stream.WriteInt3TAsync(((Trit)value).Value, cancellationToken);
    }

    /// <summary>
    /// Writes an Int3T value to the stream.
    /// </summary>
    /// <param name="value">The Int3T value to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public async Task WriteAsync(Int3T value, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        await stream.WriteInt3TAsync(value, cancellationToken);
    }

    /// <summary>
    /// Writes an Int9T value to the stream.
    /// </summary>
    /// <param name="value">The Int9T value to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public async Task WriteAsync(Int9T value, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        var intValue = (int)value + 9841;
        buffer[0] = (Int3T)(intValue % 27 - 13);
        buffer[1] = (Int3T)(intValue / 27 % 27 - 13);
        buffer[2] = (Int3T)(intValue / 729 - 13);
        await stream.WriteAsync(buffer, 0, 3, cancellationToken);
    }

    /// <summary>
    /// Writes an Int27T value to the stream.
    /// </summary>
    /// <param name="value">The Int27T value to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public async Task WriteAsync(Int27T value, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        var intValue = (long)value + 3812798742493;
        buffer[0] = (Int3T)(intValue % 27 - 13);
        buffer[1] = (Int3T)(intValue / 27 % 27 - 13);
        buffer[2] = (Int3T)(intValue / 729 % 27 - 13);
        buffer[3] = (Int3T)(intValue / 19683 % 27 - 13);
        buffer[4] = (Int3T)(intValue / 531441 % 27 - 13);
        buffer[5] = (Int3T)(intValue / 14348907 % 27 - 13);
        buffer[6] = (Int3T)(intValue / 387420489 % 27 - 13);
        buffer[7] = (Int3T)(intValue / 10460353203 % 27 - 13);
        buffer[8] = (Int3T)(intValue / 282429536481 % 27 - 13);
        await stream.WriteAsync(buffer, 0, 9, cancellationToken);
    }

    /// <summary>
    /// Writes a TernaryArray3 to the stream.
    /// </summary>
    /// <param name="array">The TernaryArray3 to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public async Task WriteAsync(TernaryArray3 array, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        await stream.WriteInt3TAsync(array, cancellationToken);
    }

    /// <summary>
    /// Writes a TernaryArray9 to the stream.
    /// </summary>
    /// <param name="array">The TernaryArray9 to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public async Task WriteAsync(TernaryArray9 array, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        buffer[0] = (Int3T)(TritConverter.LookupValue[array.Positive & 7] - TritConverter.LookupValue[array.Negative & 7]);
        buffer[1] = (Int3T)(TritConverter.LookupValue[(array.Positive >> 3) & 7] - TritConverter.LookupValue[(array.Negative >> 3) & 7]);
        buffer[2] = (Int3T)(TritConverter.LookupValue[(array.Positive >> 6) & 7] - TritConverter.LookupValue[(array.Negative >> 6) & 7]);
        await stream.WriteAsync(buffer, 0, 3, cancellationToken);
    }

    /// <summary>
    /// Writes a TernaryArray27 to the stream.
    /// </summary>
    /// <param name="array">The TernaryArray27 to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public async Task WriteAsync(TernaryArray27 array, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        buffer[0] = (Int3T)(TritConverter.LookupValue[array.Positive & 7] - TritConverter.LookupValue[array.Negative & 7]);
        buffer[1] = (Int3T)(TritConverter.LookupValue[(array.Positive >> 3) & 7] - TritConverter.LookupValue[(array.Negative >> 3) & 7]);
        buffer[2] = (Int3T)(TritConverter.LookupValue[(array.Positive >> 6) & 7] - TritConverter.LookupValue[(array.Negative >> 6) & 7]);
        buffer[3] = (Int3T)(TritConverter.LookupValue[(array.Positive >> 9) & 7] - TritConverter.LookupValue[(array.Negative >> 9) & 7]);
        buffer[4] = (Int3T)(TritConverter.LookupValue[(array.Positive >> 12) & 7] - TritConverter.LookupValue[(array.Negative >> 12) & 7]);
        buffer[5] = (Int3T)(TritConverter.LookupValue[(array.Positive >> 15) & 7] - TritConverter.LookupValue[(array.Negative >> 15) & 7]);
        buffer[6] = (Int3T)(TritConverter.LookupValue[(array.Positive >> 18) & 7] - TritConverter.LookupValue[(array.Negative >> 18) & 7]);
        buffer[7] = (Int3T)(TritConverter.LookupValue[(array.Positive >> 21) & 7] - TritConverter.LookupValue[(array.Negative >> 21) & 7]);
        buffer[8] = (Int3T)(TritConverter.LookupValue[(array.Positive >> 24) & 7] - TritConverter.LookupValue[(array.Negative >> 24) & 7]);
        await stream.WriteAsync(buffer, 0, 9, cancellationToken);
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(disposed, this);

    /// <summary>
    /// Asynchronously disposes of the current writer and the underlying stream.
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
    public override string ToString() => $"TernaryWriter: BaseStream={stream}";

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
    public override bool Equals(object? obj) => obj is TernaryWriter other && stream.Equals(other.stream);
}