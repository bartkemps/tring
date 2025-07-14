// filepath: C:\Users\kempsb\source\repos\Ternary\Ternary3\IO\MemoryInt3TStream.cs

namespace Ternary3.IO;

/// <summary>
/// Creates a stream whose backing store is memory. This implementation uses Int3T as the basic unit instead of bytes.
/// </summary>
public class MemoryInt3TStream : Int3TStream
{
    private Int3T[] buffer;
    private int capacity;
    private int expandable;
    private bool exposable;
    private bool isOpen;
    private int length;
    private int position;

    private const int DefaultCapacity = 0;

    /// <summary>
    /// Initializes a new instance of the MemoryInt3TStream class with an expandable capacity initialized to zero.
    /// </summary>
    public MemoryInt3TStream() : this(DefaultCapacity)
    {
    }

    /// <summary>
    /// Initializes a new instance of the MemoryInt3TStream class with an expandable capacity initialized to the specified value.
    /// </summary>
    /// <param name="capacity">The initial capacity of the stream.</param>
    /// <exception cref="ArgumentOutOfRangeException">capacity is negative.</exception>
    public MemoryInt3TStream(int capacity)
    {
        if (capacity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");
        }

        buffer = new Int3T[capacity];
        this.capacity = capacity;
        expandable = 1;
        exposable = true;
        isOpen = true;
    }

    /// <summary>
    /// Initializes a new instance of the MemoryInt3TStream class with the specified Int3T array as a backing store.
    /// </summary>
    /// <param name="buffer">The array of Int3T values used as the backing store.</param>
    /// <exception cref="ArgumentNullException">buffer is null.</exception>
    public MemoryInt3TStream(Int3T[] buffer) : this(buffer, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the MemoryInt3TStream class with the specified Int3T array as a backing store and a Boolean value indicating whether the stream can be written to.
    /// </summary>
    /// <param name="buffer">The array of Int3T values used as the backing store.</param>
    /// <param name="writable">A Boolean value indicating whether the stream supports writing.</param>
    /// <exception cref="ArgumentNullException">buffer is null.</exception>
    public MemoryInt3TStream(Int3T[] buffer, bool writable)
    {
        this.buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        length = capacity = buffer.Length;
        expandable = 1; // Make the stream expandable by default
        exposable = writable;
        isOpen = true;
    }

    /// <summary>
    /// Initializes a new instance of the MemoryInt3TStream class with the specified Int3T array segment as a backing store.
    /// </summary>
    /// <param name="buffer">The array of Int3T values used as the backing store.</param>
    /// <param name="index">The index into buffer at which the stream begins.</param>
    /// <param name="count">The length in Int3T values of the backing store array.</param>
    /// <exception cref="ArgumentNullException">buffer is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">index or count is negative.</exception>
    /// <exception cref="ArgumentException">The buffer length minus index is less than count.</exception>
    public MemoryInt3TStream(Int3T[] buffer, int index, int count) : this(buffer, index, count, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the MemoryInt3TStream class with the specified Int3T array segment as a backing store and a Boolean value indicating whether the stream can be written to.
    /// </summary>
    /// <param name="buffer">The array of Int3T values used as the backing store.</param>
    /// <param name="index">The index into buffer at which the stream begins.</param>
    /// <param name="count">The length in Int3T values of the backing store array.</param>
    /// <param name="writable">A Boolean value indicating whether the stream supports writing.</param>
    /// <exception cref="ArgumentNullException">buffer is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">index or count is negative.</exception>
    /// <exception cref="ArgumentException">The buffer length minus index is less than count.</exception>
    public MemoryInt3TStream(Int3T[] buffer, int index, int count, bool writable)
    {
        ArgumentNullException.ThrowIfNull(buffer);
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative.");
        }
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
        }
        if (buffer.Length - index < count)
        {
            throw new ArgumentException("Invalid buffer range specified.");
        }

        this.buffer = buffer;
        offset = index;
        length = capacity = count;
        exposable = writable;
        isOpen = true;
    }

    private int offset;

    /// <summary>
    /// Gets a value indicating whether the current stream supports reading.
    /// </summary>
    /// <value><see langword="true"/> if the stream supports reading; otherwise, <see langword="false"/>.</value>
    public override bool CanRead => isOpen;

    /// <summary>
    /// Gets a value indicating whether the current stream supports writing.
    /// </summary>
    /// <value><see langword="true"/> if the stream supports writing; otherwise, <see langword="false"/>.</value>
    public override bool CanWrite => isOpen && exposable;

    /// <summary>
    /// Gets a value indicating whether the current stream supports seeking.
    /// </summary>
    /// <value><see langword="true"/> if the stream supports seeking; otherwise, <see langword="false"/>.</value>
    public override bool CanSeek => isOpen;

    /// <summary>
    /// Gets the length in Int3T units of the stream.
    /// </summary>
    /// <value>A long value representing the length of the stream in Int3T units.</value>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public override long Length
    {
        get
        {
            EnsureNotClosed();
            return length;
        }
    }

    /// <summary>
    /// Gets or sets the position within the current stream.
    /// </summary>
    /// <value>The current position within the stream.</value>
    /// <exception cref="ArgumentOutOfRangeException">The position is set to a negative value or a value greater than <see cref="Int32.MaxValue"/>.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public override long Position
    {
        get
        {
            EnsureNotClosed();
            return position;
        }
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Position cannot be negative.");
            }
            if (value > Int32.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Position cannot be greater than Int32.MaxValue.");
            }
            EnsureNotClosed();
            position = (int)value;
        }
    }

    /// <summary>
    /// Gets the array of Int3T values from which this stream was created.
    /// </summary>
    /// <returns>The Int3T array from which this stream was created, or null if this stream was not created from an array.</returns>
    /// <exception cref="UnauthorizedAccessException">The stream was not created with a publicly visible buffer.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public virtual Int3T[] GetBuffer()
    {
        if (!exposable)
        {
            throw new UnauthorizedAccessException("The buffer is not publicly accessible.");
        }
        EnsureNotClosed();
        return buffer;
    }

    /// <summary>
    /// Gets a Boolean value indicating whether the current stream has been closed or not.
    /// </summary>
    /// <value><see langword="true"/> if the stream is closed; otherwise, <see langword="false"/>.</value>
    public virtual bool IsClosed => !isOpen;

    /// <summary>
    /// Returns the array of Int3T values from which this stream was created. The returned array is not a copy and any changes will affect the MemoryInt3TStream.
    /// </summary>
    /// <param name="buffer">When this method returns, contains the array of Int3T values from which this stream was created.</param>
    /// <param name="index">When this method returns, contains the offset in the buffer at which the stream begins.</param>
    /// <param name="count">When this method returns, contains the length of the stream in Int3T values.</param>
    public virtual void GetBuffer(out Int3T[] buffer, out int index, out int count)
    {
        EnsureNotClosed();
        buffer = this.buffer;
        index = offset;
        count = length;
    }

    /// <summary>
    /// Creates a new array and copies all the data from the MemoryInt3TStream into it.
    /// </summary>
    /// <returns>A new Int3T array containing a copy of the MemoryInt3TStream data.</returns>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public virtual Int3T[] ToArray()
    {
        EnsureNotClosed();
        var copy = new Int3T[length];
        Array.Copy(buffer, offset, copy, 0, length);
        return copy;
    }

    /// <summary>
    /// Overrides the abstract ReadAsync method to read from the memory buffer.
    /// </summary>
    /// <param name="buffer">The buffer to read data into.</param>
    /// <param name="offset">The offset in the buffer at which to begin writing.</param>
    /// <param name="count">The maximum number of Int3T values to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous read operation. The value contains the number of Int3T values read.</returns>
    /// <exception cref="ArgumentNullException">buffer is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">offset or count is negative.</exception>
    /// <exception cref="ArgumentException">The sum of offset and count is larger than the buffer length.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public override Task<int> ReadAsync(Int3T[] buffer, int offset, int count, CancellationToken cancellationToken = default)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }
        if (offset < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative.");
        }
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
        }
        if (buffer.Length - offset < count)
        {
            throw new ArgumentException("Invalid buffer range specified.");
        }
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<int>(cancellationToken);
        }

        try
        {
            EnsureNotClosed();

            var n = length - position;
            if (n > count)
            {
                n = count;
            }
            if (n <= 0)
            {
                return Task.FromResult(0);
            }

            Array.Copy(this.buffer, this.offset + position, buffer, offset, n);
            position += n;
                
            return Task.FromResult(n);
        }
        catch (Exception e)
        {
            return Task.FromException<int>(e);
        }
    }

    /// <summary>
    /// Overrides the abstract WriteAsync method to write to the memory buffer.
    /// </summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The offset in the buffer at which to begin reading.</param>
    /// <param name="count">The maximum number of Int3T values to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    /// <exception cref="ArgumentNullException">buffer is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">offset or count is negative.</exception>
    /// <exception cref="ArgumentException">The sum of offset and count is larger than the buffer length.</exception>
    /// <exception cref="NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public override async Task WriteAsync(Int3T[] buffer, int offset, int count, CancellationToken cancellationToken = default)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }
        if (offset < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be negative.");
        }
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
        }
        if (buffer.Length - offset < count)
        {
            throw new ArgumentException("Invalid buffer range specified.");
        }
        if (cancellationToken.IsCancellationRequested)
        {
            await Task.FromCanceled(cancellationToken);
            return;
        }

        try
        {
            EnsureNotClosed();
            if (!CanWrite)
            {
                throw new NotSupportedException("The stream does not support writing.");
            }

            var newPosition = position + count;
            if (newPosition > capacity)
            {
                if (expandable <= 0)
                {
                    throw new NotSupportedException("The stream is not expandable.");
                }
                    
                // Expand the buffer
                var newCapacity = Math.Max(capacity * 2, newPosition);
                SetCapacity(newCapacity);
            }
                
            Array.Copy(buffer, offset, this.buffer, this.offset + position, count);
            position = newPosition;
            if (position > length)
            {
                length = position;
            }
        }
        catch (Exception e)
        {
            await Task.FromException(e);
            throw;
        }
    }

    /// <summary>
    /// Overrides the abstract SeekAsync method to set the position within the memory buffer.
    /// </summary>
    /// <param name="offset">A Int3T offset relative to the origin parameter.</param>
    /// <param name="origin">A value of type SeekOrigin indicating the reference point used to obtain the new position.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous seek operation. The value contains the new position within the stream.</returns>
    /// <exception cref="ArgumentOutOfRangeException">offset is greater than <see cref="Int32.MaxValue"/>.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public override Task<long> SeekAsync(long offset, SeekOrigin origin, CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<long>(cancellationToken);
        }

        try
        {
            EnsureNotClosed();

            if (offset > Int32.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset cannot be greater than Int32.MaxValue.");
            }

            int newPosition;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    newPosition = (int)offset;
                    break;
                case SeekOrigin.Current:
                    newPosition = position + (int)offset;
                    break;
                case SeekOrigin.End:
                    newPosition = length + (int)offset;
                    break;
                default:
                    throw new ArgumentException("Invalid SeekOrigin.", nameof(origin));
            }

            if (newPosition < 0)
            {
                throw new IOException("Cannot seek to a negative position.");
            }

            position = newPosition;
            return Task.FromResult((long)position);
        }
        catch (Exception e)
        {
            return Task.FromException<long>(e);
        }
    }

    /// <summary>
    /// Overrides the abstract SetLengthAsync method to set the length of the memory buffer.
    /// </summary>
    /// <param name="value">The desired length of the stream in Int3T values.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous set length operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">value is negative or greater than <see cref="Int32.MaxValue"/>.</exception>
    /// <exception cref="NotSupportedException">The stream does not support both writing and seeking.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public override async Task SetLengthAsync(long value, CancellationToken cancellationToken = default)
    {
        if (value < 0 || value > Int32.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Length must be non-negative and less than Int32.MaxValue.");
        }
        if (cancellationToken.IsCancellationRequested)
        {
            await Task.FromCanceled(cancellationToken);
            return;
        }

        try
        {
            EnsureNotClosed();
            if (!CanWrite)
            {
                throw new NotSupportedException("The stream does not support writing.");
            }
                
            var newLength = (int)value;
            // Store the original length before changing it
            var originalLength = length;
                
            if (newLength > capacity)
            {
                if (expandable <= 0)
                {
                    throw new NotSupportedException("The stream is not expandable.");
                }
                SetCapacity(newLength);
            }
                
            if (newLength < length)
            {
                // Zero out the data beyond the new length
                Array.Clear(buffer, offset + newLength, length - newLength);
            }
                
            length = newLength;
                
            // For consistency with MemoryStream behavior:
            // If expanding the stream, set position to the original length
            // If truncating and position > new length, set position to new length
            if (newLength > originalLength)
            {
                position = originalLength;
            }
            else if (position > length)
            {
                position = length;
            }
        }
        catch (Exception e)
        {
            await Task.FromException(e);
            throw;
        }
    }

    /// <summary>
    /// Overrides the abstract FlushAsync method. For MemoryInt3TStream, this is a no-op since there is no buffer to flush.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    public override Task FlushAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled(cancellationToken);
        }

        try
        {
            EnsureNotClosed();
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            return Task.FromException(e);
        }
    }

    /// <summary>
    /// Sets the capacity of the memory buffer underlying the MemoryInt3TStream to the specified value.
    /// </summary>
    /// <param name="value">The new capacity.</param>
    /// <exception cref="ArgumentOutOfRangeException">value is negative or less than the current length of the stream.</exception>
    /// <exception cref="NotSupportedException">The stream is not expandable.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public virtual void SetCapacity(int value)
    {
        if (value < length)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Capacity cannot be smaller than the current length.");
        }
            
        EnsureNotClosed();
        if (!exposable)
        {
            throw new NotSupportedException("The stream is not expandable.");
        }
            
        if (value != capacity)
        {
            if (value > 0)
            {
                var newBuffer = new Int3T[value];
                if (length > 0)
                {
                    Array.Copy(buffer, offset, newBuffer, 0, length);
                }
                buffer = newBuffer;
                offset = 0;
            }
            else
            {
                buffer = [];
                offset = 0;
            }
            capacity = value;
        }
    }

    /// <summary>
    /// Ensures the stream has not been closed.
    /// </summary>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    private void EnsureNotClosed()
    {
        if (!isOpen)
        {
            throw new ObjectDisposedException(null, "The stream is closed.");
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the MemoryInt3TStream and optionally releases the managed resources.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    protected override ValueTask DisposeAsyncCore()
    {
        isOpen = false;
        return ValueTask.CompletedTask;
    }
}