// filepath: C:\Users\kempsb\source\repos\Ternary\Ternary3\IO\Int3TStream.cs

namespace Ternary3.IO;

/// <summary>
/// Provides an abstract base class for Int3T streams.
/// </summary>
/// <remarks>
/// Int3TStream is an abstract base class that represents a sequence of Int3T values (trybbles).
/// It is the ternary equivalent of the binary System.IO.Stream that works with bytes.
/// </remarks>
public abstract class Int3TStream : IAsyncDisposable
{
    private bool disposed;

    /// <summary>
    /// Gets a value indicating whether the current stream has been disposed.
    /// </summary>
    /// <value><see langword="true"/> if the stream has been disposed; otherwise, <see langword="false"/>.</value>
    protected bool Disposed => disposed;

    /// <summary>
    /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
    /// </summary>
    /// <value><see langword="true"/> if the stream supports reading; otherwise, <see langword="false"/>.</value>
    public abstract bool CanRead { get; }

    /// <summary>
    /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
    /// </summary>
    /// <value><see langword="true"/> if the stream supports writing; otherwise, <see langword="false"/>.</value>
    public abstract bool CanWrite { get; }

    /// <summary>
    /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
    /// </summary>
    /// <value><see langword="true"/> if the stream supports seeking; otherwise, <see langword="false"/>.</value>
    public abstract bool CanSeek { get; }

    /// <summary>
    /// When overridden in a derived class, gets the length in Int3T units of the stream.
    /// </summary>
    /// <value>A long value representing the length of the stream in Int3T units.</value>
    /// <exception cref="NotSupportedException">A class derived from Int3TStream does not support seeking.</exception>
    /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
    public abstract long Length { get; }

    /// <summary>
    /// When overridden in a derived class, gets or sets the position within the current stream.
    /// </summary>
    /// <value>The current position within the stream.</value>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
    public abstract long Position { get; set; }

    /// <summary>
    /// When overridden in a derived class, reads a sequence of Int3T values from the current stream and advances the position within the stream by the number of Int3T values read.
    /// </summary>
    /// <param name="buffer">An array of Int3T values. When this method returns, the buffer contains the specified Int3T array with the values between offset and (offset + count - 1) replaced by the Int3T values read from the current source.</param>
    /// <param name="offset">The zero-based Int3T offset in buffer at which to begin storing the data read from the current stream.</param>
    /// <param name="count">The maximum number of Int3T values to be read from the current stream.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The total number of Int3T values read into the buffer. This can be less than the number of Int3T values requested if that many Int3T values are not currently available, or zero (0) if the end of the stream has been reached.</returns>
    /// <exception cref="ArgumentNullException">buffer is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">offset or count is negative.</exception>
    /// <exception cref="ArgumentException">The sum of offset and count is larger than the buffer length.</exception>
    /// <exception cref="NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
    public abstract Task<int> ReadAsync(Int3T[] buffer, int offset, int count, CancellationToken cancellationToken = default);

    /// <summary>
    /// When overridden in a derived class, writes a sequence of Int3T values to the current stream and advances the current position within this stream by the number of Int3T values written.
    /// </summary>
    /// <param name="buffer">An array of Int3T values. This method copies count Int3T values from buffer to the current stream.</param>
    /// <param name="offset">The zero-based Int3T offset in buffer at which to begin copying Int3T values to the current stream.</param>
    /// <param name="count">The number of Int3T values to be written to the current stream.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <exception cref="ArgumentNullException">buffer is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">offset or count is negative.</exception>
    /// <exception cref="ArgumentException">The sum of offset and count is larger than the buffer length.</exception>
    /// <exception cref="NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
    public abstract Task WriteAsync(Int3T[] buffer, int offset, int count, CancellationToken cancellationToken = default);

    /// <summary>
    /// When overridden in a derived class, sets the position within the current stream.
    /// </summary>
    /// <param name="offset">A Int3T offset relative to the origin parameter.</param>
    /// <param name="origin">A value of type SeekOrigin indicating the reference point used to obtain the new position.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The new position within the current stream.</returns>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output.</exception>
    /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
    public abstract Task<long> SeekAsync(long offset, SeekOrigin origin, CancellationToken cancellationToken = default);

    /// <summary>
    /// When overridden in a derived class, sets the length of the current stream.
    /// </summary>
    /// <param name="value">The desired length of the current stream in Int3T values.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output.</exception>
    /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
    public abstract Task SetLengthAsync(long value, CancellationToken cancellationToken = default);

    /// <summary>
    /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
    /// </summary>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public abstract Task FlushAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously reads a Int3T value from the stream and advances the position within the stream by one Int3T value, or returns -1 if at the end of the stream.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation. The value of the TResult parameter contains the unsigned Int3T value cast to an Int32, or -1 if at the end of the stream.</returns>
    /// <exception cref="NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
    public virtual async Task<int> ReadInt3TAsync(CancellationToken cancellationToken = default)
    {
        var singleInt3T = new Int3T[1];
        var result = await ReadAsync(singleInt3T, 0, 1, cancellationToken).ConfigureAwait(false);
        if (result == 0)
        {
            return -1;
        }
        return Convert.ToInt32(singleInt3T[0]);
    }

    /// <summary>
    /// Asynchronously writes a Int3T value to the current position in the stream and advances the position within the stream by one Int3T value.
    /// </summary>
    /// <param name="value">The Int3T value to write to the stream.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    /// <exception cref="NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
    public virtual Task WriteInt3TAsync(Int3T value, CancellationToken cancellationToken = default)
    {
        var singleInt3T = new Int3T[1];
        singleInt3T[0] = value;
        return WriteAsync(singleInt3T, 0, 1, cancellationToken);
    }

    /// <summary>
    /// Asynchronously reads a sequence of Int3T values from the current stream and writes them to the buffer.
    /// </summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation. The value of the TResult parameter contains the total number of Int3T values read into the buffer.</returns>
    /// <exception cref="ArgumentNullException">buffer is null.</exception>
    /// <exception cref="NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
    public virtual Task<int> ReadAsync(Int3T[] buffer, CancellationToken cancellationToken = default)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }
        return ReadAsync(buffer, 0, buffer.Length, cancellationToken);
    }

    /// <summary>
    /// Asynchronously writes a sequence of Int3T values from the buffer to the current stream.
    /// </summary>
    /// <param name="buffer">The buffer to read the data from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    /// <exception cref="ArgumentNullException">buffer is null.</exception>
    /// <exception cref="NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
    public virtual Task WriteAsync(Int3T[] buffer, CancellationToken cancellationToken = default)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }
        return WriteAsync(buffer, 0, buffer.Length, cancellationToken);
    }

    /// <summary>
    /// Asynchronously reads all Int3T values from the current position to the end of the stream.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation. The value of the TResult parameter contains a Int3T array with the data read from the stream.</returns>
    /// <exception cref="NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned array.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public virtual async Task<Int3T[]> ReadToEndAsync(CancellationToken cancellationToken = default)
    {
        if (!CanRead)
        {
            throw new NotSupportedException("Stream does not support reading.");
        }

        if (CanSeek)
        {
            var length = Length - Position;
            if (length > int.MaxValue)
            {
                throw new IOException("Stream is too large to read to end.");
            }

            var result = new Int3T[(int)length];
            var bytesRead = await ReadAsync(result, 0, result.Length, cancellationToken).ConfigureAwait(false);
            if (bytesRead < result.Length)
            {
                // If we didn't read as many bytes as we expected, resize the array
                Array.Resize(ref result, bytesRead);
            }
            return result;
        }
        else
        {
            // For non-seekable streams, read in chunks
            const int initialCapacity = 4096;
            var capacity = initialCapacity;
            var buffer = new Int3T[capacity];
            var totalRead = 0;
            int bytesRead;

            while ((bytesRead = await ReadAsync(buffer, totalRead, capacity - totalRead, cancellationToken).ConfigureAwait(false)) > 0)
            {
                totalRead += bytesRead;
                if (totalRead == capacity)
                {
                    // Double the buffer size
                    capacity *= 2;
                    Array.Resize(ref buffer, capacity);
                }
            }

            // Resize to the actual data size
            if (totalRead != capacity)
            {
                Array.Resize(ref buffer, totalRead);
            }

            return buffer;
        }
    }
        
    /// <summary>
    /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
    /// </summary>
    /// <remarks>
    /// This is a synchronous method. Consider using <see cref="FlushAsync"/> instead for better performance.
    /// </remarks>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public virtual void Flush() => FlushAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Reads a sequence of Int3T values from the current stream and advances the position within the stream by the number of Int3T values read.
    /// </summary>
    /// <remarks>
    /// This is a synchronous method. Consider using <see cref="ReadAsync(Int3T[], int, int, System.Threading.CancellationToken)"/> instead for better performance.
    /// </remarks>
    /// <param name="buffer">An array of Int3T values.</param>
    /// <param name="offset">The zero-based Int3T offset in buffer at which to begin storing the data read from the current stream.</param>
    /// <param name="count">The maximum number of Int3T values to be read from the current stream.</param>
    /// <returns>The total number of Int3T values read into the buffer.</returns>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public virtual int Read(Int3T[] buffer, int offset, int count) => ReadAsync(buffer, offset, count).GetAwaiter().GetResult();

    /// <summary>
    /// Writes a sequence of Int3T values to the current stream and advances the current position within this stream by the number of Int3T values written.
    /// </summary>
    /// <remarks>
    /// This is a synchronous method. Consider using <see cref="WriteAsync(Int3T[], int, int, System.Threading.CancellationToken)"/> instead for better performance.
    /// </remarks>
    /// <param name="buffer">An array of Int3T values.</param>
    /// <param name="offset">The zero-based Int3T offset in buffer at which to begin copying Int3T values to the current stream.</param>
    /// <param name="count">The number of Int3T values to be written to the current stream.</param>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public virtual void Write(Int3T[] buffer, int offset, int count) => WriteAsync(buffer, offset, count).GetAwaiter().GetResult();

    /// <summary>
    /// Sets the position within the current stream.
    /// </summary>
    /// <remarks>
    /// This is a synchronous method. Consider using <see cref="SeekAsync"/> instead for better performance.
    /// </remarks>
    /// <param name="offset">A Int3T offset relative to the origin parameter.</param>
    /// <param name="origin">A value of type SeekOrigin indicating the reference point used to obtain the new position.</param>
    /// <returns>The new position within the current stream.</returns>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public virtual long Seek(long offset, SeekOrigin origin) => SeekAsync(offset, origin).GetAwaiter().GetResult();


    /// <summary>
    /// Asynchronously disposes the stream, releasing all resources used by it.
    /// </summary>
    /// <returns>A task representing the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the Int3TStream and optionally releases the managed resources.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    protected virtual async ValueTask DisposeAsyncCore()
    {
        // If the stream is writable, flush any buffered data before disposing
        if (!disposed && CanWrite)
        {
            try
            {
                await FlushAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                // Ignore exceptions during disposal
            }
        }
        disposed = true;
    }

    /// <summary>
    /// Gets a value that determines whether the current stream can timeout.
    /// </summary>
    /// <value><see langword="true"/> if the stream can timeout; otherwise, <see langword="false"/>.</value>
    public virtual bool CanTimeout => false;

    /// <summary>
    /// Gets or sets a value, in milliseconds, that determines how long the stream will attempt to read before timing out.
    /// </summary>
    /// <value>A value, in milliseconds, that determines how long the stream will attempt to read before timing out.</value>
    /// <exception cref="InvalidOperationException">The stream does not support timeouts.</exception>
    public virtual int ReadTimeout 
    {
        get { throw new InvalidOperationException("Timeout is not supported on this stream."); }
        set { throw new InvalidOperationException("Timeout is not supported on this stream."); }
    }

    /// <summary>
    /// Gets or sets a value, in milliseconds, that determines how long the stream will attempt to write before timing out.
    /// </summary>
    /// <value>A value, in milliseconds, that determines how long the stream will attempt to write before timing out.</value>
    /// <exception cref="InvalidOperationException">The stream does not support timeouts.</exception>
    public virtual int WriteTimeout
    {
        get { throw new InvalidOperationException("Timeout is not supported on this stream."); }
        set { throw new InvalidOperationException("Timeout is not supported on this stream."); }
    }

    /// <summary>
    /// Asynchronously copies a specific number of Int3T values from the current Int3TStream to another Int3TStream.
    /// </summary>
    /// <param name="destination">The Int3TStream to which the contents of the current stream will be copied.</param>
    /// <param name="count">The number of Int3T values to copy.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    /// <exception cref="ArgumentNullException">destination is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">count is negative.</exception>
    /// <exception cref="NotSupportedException">The current stream does not support reading, or the destination stream does not support writing.</exception>
    /// <exception cref="ObjectDisposedException">Either the current stream or the destination stream is disposed.</exception>
    /// <exception cref="IOException">An I/O error occurred.</exception>
    public virtual async Task CopyToAsync(Int3TStream destination, int count, CancellationToken cancellationToken = default)
    {
        if (destination == null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Non-negative number required.");
        }

        if (!CanRead)
        {
            throw new NotSupportedException("This stream does not support reading.");
        }

        if (!destination.CanWrite)
        {
            throw new NotSupportedException("The destination stream does not support writing.");
        }

        var buffer = new Int3T[Math.Min(count, 4096)];
        int bytesRead;
        var totalRead = 0;

        while (totalRead < count && (bytesRead = await ReadAsync(buffer, 0, Math.Min(buffer.Length, count - totalRead), cancellationToken).ConfigureAwait(false)) > 0)
        {
            await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
            totalRead += bytesRead;
        }
    }

    /// <summary>
    /// Asynchronously copies all Int3T values from the current Int3TStream to another Int3TStream.
    /// </summary>
    /// <param name="destination">The Int3TStream to which the contents of the current stream will be copied.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    /// <exception cref="ArgumentNullException">destination is null.</exception>
    /// <exception cref="NotSupportedException">The current stream does not support reading, or the destination stream does not support writing.</exception>
    /// <exception cref="ObjectDisposedException">Either the current stream or the destination stream is disposed.</exception>
    /// <exception cref="IOException">An I/O error occurred.</exception>
    public virtual async Task CopyToAsync(Int3TStream destination, CancellationToken cancellationToken = default)
    {
        if (destination == null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (!CanRead)
        {
            throw new NotSupportedException("This stream does not support reading.");
        }

        if (!destination.CanWrite)
        {
            throw new NotSupportedException("The destination stream does not support writing.");
        }

        var buffer = new Int3T[4096];
        int bytesRead;

        while ((bytesRead = await ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) > 0)
        {
            await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
        }
    }
}