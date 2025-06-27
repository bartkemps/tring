namespace Ternary3;

using Formatting;
using TritArrays;
using System.Numerics;

public class BigTritArray : ITritArray, IEquatable<BigTritArray>, IFormattable
{
    internal List<ulong> Positive;
    internal List<ulong> Negative;
    public int Length { get; }

    internal BigTritArray(List<ulong> negative, List<ulong> positive, int length)
    {
        Negative = negative;
        Positive = positive;
        Length = length;
    }

    public BigTritArray(int length)
    {
        if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
        Length = length;
        var longsNeeded = (length + 63) / 64;
        Positive = [..new ulong[longsNeeded]];
        Negative = [..new ulong[longsNeeded]];
    }

    public BigTritArray(params ITritArray[] arrays): this ((arrays ?? throw new ArgumentNullException(nameof(arrays))).Sum(a=>a.Length))
    {
        var offset = 0;
        foreach (var arr in arrays)
        {
            InitializeTritArray(arr, offset);
            offset += arr.Length;
        }
    }

    private void InitializeTritArray(ITritArray arr, int offset)
    {
        switch (arr)
        {
            case TritArray27 value:
            {
                CopyRange(value.Negative, value.Positive, offset, value.Length);
                return;
            }
            case TritArray9 value:
            {
                CopyRange(value.Negative, value.Positive, offset, value.Length);
                return;
            }
            case TritArray3 value:
            {
                CopyRange(value.Negative, value.Positive, offset, value.Length);
                return;
            }
            default:
            {
                CopyRange(arr, offset);
                return;
            }
        }
    }

    private void CopyRange(ulong valueNegative, ulong valuePositive, int offset, int length)
    {
        var mask = ((ulong)1 << length) - 1;
        valueNegative &= mask;
        valuePositive &= mask;
        var startLong = offset / 64;
        var startBit = offset % 64;
        // If the offset is aligned, just OR in the bits
        if (startBit == 0)
        {
            Negative[startLong] |= valueNegative;
            Positive[startLong] |= valuePositive;
            return;
        }
        // The offset is unaligned 
        var bitsInFirst = Math.Min(64 - startBit, length);
        Negative[startLong] |= valueNegative << startBit;
        Positive[startLong] |= valuePositive << startBit;
        if (bitsInFirst >= length) return;
        Negative[startLong + 1] |= valueNegative >> bitsInFirst;
        Positive[startLong + 1] |= valuePositive >> bitsInFirst;
    }
    private void CopyRange(ITritArray arr, int offset)
    {
        for (var i = 0; i < arr.Length; i++)
        {
            this[offset + i] = arr[i];
        }
    }

    public Trit this[int index]
    {
        get => index >= 0 && index < Length 
            ? TritConverter.GetTrit(Negative, Positive, index)
            : throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {Length-1}.");
        set
        {
            if (index < 0 || index >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {Length-1}.");
            }
            TritConverter.SetTrit(ref Negative, ref Positive, index, value);
        }
    }
    
    /// <summary>
    /// Gets or sets the trit at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the trit to get or set (must be between 0 and 26).</param>
    /// <returns>The trit at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when index is less than 0 or greater than 26.</exception>
    public Trit this[Index index]
    {
        get => index.IsFromEnd ? this[Length - index.Value] : this[index.Value];
        set
        {
            if (index.IsFromEnd)
            {
                this[Length - index.Value] = value;
            }
            else
            {
                this[index.Value] = value;
            }
        }
    }

    /// <summary>
    /// Returns a string representation of the BigTritArray.
    /// </summary>
    public override string ToString() => Formatter.Format(this, null, null);

    /// <summary>
    /// Returns a string representation of the BigTritArray.
    /// </summary>
    public string ToString(string? format) => Formatter.Format(this, format, null);

    /// <summary>
    /// Returns a string representation of the BigTritArray.
    /// </summary>
    public string ToString(IFormatProvider? provider) => Formatter.Format(this, null, provider);

    /// <summary>
    /// Returns a string representation of the BigTritArray.
    /// </summary>
    public string ToString(string? format, IFormatProvider? provider) => Formatter.Format(this, format, provider);

    /// <summary>
    /// Returns a string representation of this instance, formatted balanced ternarily according to the specified format.
    /// </summary>
    public string ToString(ITernaryFormat format) => Formatter.Format(this, format);

    // todo: equal values (leading zeros) should be equal, even if they have different lengths
    public bool Equals(BigTritArray? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;
        if (Length != other.Length) return false;
        ApplyLength();
        return Positive.SequenceEqual(other.Positive) && Negative.SequenceEqual(other.Negative);
    }

    private void ApplyLength()
    {
        var mask = 1UL << Length % 64 - 1;
        Positive[^1] &= mask;
        Negative[^1] &= mask;
    }

    public override bool Equals(object? obj) => obj is BigTritArray other && Equals(other);

    public override int GetHashCode()
    {
        // Use HashCode.Combine for better hash distribution and simplicity
        return HashCode.Combine(
            Length,
            Positive.Aggregate(0, HashCode.Combine),
            Negative.Aggregate(0, HashCode.Combine)
        );
    }

    #region Conversion Operators

    /// <summary>
    /// Implicit conversion from BigInteger to BigTritArray.
    /// </summary>
    public static implicit operator BigTritArray(BigInteger value)
    {
        TritConverter.ToTrits(value, out var negative, out var positive, out var length);
        return new (negative, positive, length);
    }

    /// <summary>
    /// Implicit conversion from long to BigTritArray.
    /// </summary>
    public static implicit operator BigTritArray(long value)
    {
        TritConverter.ToTrits(value, out var negative, out var positive, out var length);
        return new (negative, positive, length);
    }

    /// <summary>
    /// Implicit conversion from int to BigTritArray.
    /// </summary>
    public static implicit operator BigTritArray(int value)
    {
        TritConverter.ToTrits(value, out var negative, out var positive, out var length);
        return new (negative, positive, length);
    }

    /// <summary>
    /// Implicit conversion from BigTritArray to BigInteger.
    /// </summary>
    public static implicit operator BigInteger(BigTritArray array) => TritConverter.ToBigInteger(array.Negative, array.Positive);

    /// <summary>
    /// Explicit conversion from BigTritArray to long.
    /// </summary>
    public static explicit operator long(BigTritArray array) => TritConverter.ToInt64(array.Negative, array.Positive, array.Length);

    /// <summary>
    /// Explicit conversion from BigTritArray to int.
    /// </summary>
    public static explicit operator int(BigTritArray array) => TritConverter.ToInt32(array.Negative, array.Positive, array.Length);

    #endregion
}