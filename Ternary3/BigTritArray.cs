namespace Ternary3;

using System.Numerics;
using System.Diagnostics.CodeAnalysis;
using Formatting;
using Operators;
using TritArrays;

/// <summary>
/// Represents a variable-length array of trits (ternary digits) backed by lists of ulongs.
/// This class provides support for arbitrary-length balanced ternary numbers and operations.
/// </summary>
public class BigTritArray : ITritArray<BigTritArray>
{
    internal List<ulong> Positive;
    internal List<ulong> Negative;
    private ulong mask;

    /// <summary>
    /// Gets or sets the number of trits in this array.
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// Represents a BigTritArray with all trits set to zero.
    /// </summary>
    public static readonly BigTritArray Zero = new(0);

    /// <summary>
    /// Represents a BigTritArray with value of one (a single trit set to positive).
    /// </summary>
    public static readonly BigTritArray One = new([0], [1], 1);


    /// <summary>
    /// Resizes the TritArray to the specified length.
    /// </summary>
    /// <param name="length">The new length of the array. Must be non-negative.</param>
    /// <remarks>
    /// This method adjusts the underlying storage to accommodate the new length by either adding or removing
    /// elements from the internal storage lists. If the new length is the same as the current length, no operation is performed.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified length is negative.</exception>
    public void Resize(int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        if (length == Length) return;
        Length = length;
        mask = (1UL << (length % 64)) - 1;
        if (mask == 0) mask = ulong.MaxValue;
        if (length == 0)
        {
            Negative.Clear();
            Positive.Clear();
            return;
        }

        var longsNeeded = (length + 63) / 64;
        if (Positive.Count < longsNeeded)
        {
            Positive.AddRange(new ulong[longsNeeded - Positive.Count]);
            Negative.AddRange(new ulong[longsNeeded - Negative.Count]);
        }
        else if (Positive.Count > longsNeeded)
        {
            Positive.RemoveRange(longsNeeded, Positive.Count - longsNeeded);
            Negative.RemoveRange(longsNeeded, Negative.Count - longsNeeded);
            ApplyLength();
        }
    }

    internal BigTritArray(List<ulong> negative, List<ulong> positive, int length)
    {
        Negative = negative;
        Positive = positive;
        Length = length;
        mask = (1UL << (length % 64)) - 1;
        if (mask == 0) mask = ulong.MaxValue;
    }

    /// <summary>
    /// Initializes a new instance of the BigTritArray class with a specified length.
    /// </summary>
    /// <param name="length">The length of the trit array. Must be non-negative.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when length is negative.</exception>
    public BigTritArray(int length)
    {
        if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
        Length = length;
        mask = (1UL << (length % 64)) - 1;
        if (mask == 0) mask = ulong.MaxValue;
        var longsNeeded = (length + 63) / 64;
        Positive = [.. new ulong[longsNeeded]];
        Negative = [.. new ulong[longsNeeded]];
    }

    /// <summary>
    /// Initializes a new instance of the BigTritArray class by concatenating multiple trit arrays.
    /// </summary>
    /// <param name="arrays">The arrays to concatenate.</param>
    /// <exception cref="ArgumentNullException">Thrown when arrays is null.</exception>
    public BigTritArray(params ITritArray[] arrays) : this((arrays ?? throw new ArgumentNullException(nameof(arrays))).Sum(a => a.Length))
    {
        var offset = 0;
        foreach (var arr in arrays)
        {
            InitializeTritArray(arr, offset);
            offset += arr.Length;
        }

        Length = offset;
        mask = 1UL << (offset % 64 - 1);
    }
    
    /// <summary>
    /// Parses a string representation of a BigTritArray.
    /// </summary>
    /// <param name="value">The string value to parse.</param>
    /// <returns>A BigTritArray representing the parsed value.</returns>
    public static BigTritArray Parse(string value) => Parser.ParseBigTritArray(value);
    
    /// <summary>
    /// Parses a string representation of a BigTritArray.
    /// </summary>
    /// <param name="value">The string value to parse.</param>
    /// <param name="format">The format to use for parsing.</param>
    /// <returns>A BigTritArray representing the parsed value.</returns>
    public static BigTritArray Parse(string value, ITernaryFormat format) => Parser.ParseBigTritArray(value, format);
    
    /// <summary>
    /// Parses a string representation of a BigTritArray.
    /// </summary>
    /// <param name="value">The string value to parse.</param>
    /// <param name="options">The options to use for parsing.</param>
    /// <returns>A BigTritArray representing the parsed value.</returns>
    public static BigTritArray Parse(string value, TritParseOptions options) => Parser.ParseBigTritArray(value, null, options);
    
    /// <summary>
    /// Parses a string representation of a BigTritArray.
    /// </summary>
    /// <param name="value">The string value to parse.</param>
    /// <param name="format">The format to use for parsing.</param>
    /// <param name="options">The options to use for parsing.</param>
    /// <returns>A BigTritArray representing the parsed value.</returns>
    public static BigTritArray Parse(string value, ITernaryFormat format, TritParseOptions options) => Parser.ParseBigTritArray(value, format, options);

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
            case TritArray value:
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
        var bitMask = ((ulong)1 << length) - 1;
        valueNegative &= bitMask;
        valuePositive &= bitMask;
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

    /// <inheritdoc/>
    public Trit this[int index]
    {
        get => index >= 0 && index < Length
            ? TritConverter.GetTrit(Negative, Positive, index)
            : throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {Length - 1}.");
        set
        {
            if (index < 0 || index >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {Length - 1}.");
            }

            TritConverter.SetTrit(ref Negative, ref Positive, index, value);
        }
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public ITritArray this[Range range]
    {
        get
        {
            Splicer.Splice(Negative, Positive, Length, range, out var negative, out var positive, out var length);
            return new BigTritArray(negative, positive, length).ApplyLength();
        }
    }

    /// <summary>
    /// Returns a string representation of the TritArray.
    /// </summary>
    public override string ToString() => Formatter.Format(this, null, null);

    /// <summary>
    /// Returns a string representation of the TritArray.
    /// </summary>
    public string ToString(string? format) => Formatter.Format(this, format, null);

    /// <summary>
    /// Returns a string representation of the TritArray.
    /// </summary>
    public string ToString(IFormatProvider? provider) => Formatter.Format(this, null, provider);

    /// <summary>
    /// Returns a string representation of the TritArray.
    /// </summary>
    public string ToString(string? format, IFormatProvider? provider) => Formatter.Format(this, format, provider);

    /// <summary>
    /// Returns a string representation of this instance, formatted balanced ternarily according to the specified format.
    /// </summary>
    public string ToString(ITernaryFormat format) => Formatter.Format(this, format);

    internal BigTritArray ApplyLength()
    {
        if (Length > 0)
        {
            Positive[^1] &= mask;
            Negative[^1] &= mask;
        }

        return this;
    }

    /// <summary>
    /// Determines whether this instance and another BigTritArray are equal.
    /// </summary>
    /// <param name="other">The BigTritArray to compare with this instance.</param>
    /// <returns>true if the specified BigTritArray is equal to this instance; otherwise, false.</returns>
    public bool Equals(BigTritArray? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;
        var n1 = Enumerable.Reverse(Negative).SkipWhile(x => x == 0);
        var p1 = Enumerable.Reverse(Positive).SkipWhile(x => x == 0);
        var n2 = Enumerable.Reverse(other.Negative).SkipWhile(x => x == 0);
        var p2 = Enumerable.Reverse(other.Positive).SkipWhile(x => x == 0);
        return n1.SequenceEqual(n2) && p1.SequenceEqual(p2);
    }

    /// <summary>
    /// Determines whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>true if the specified object is equal to this instance; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is BigTritArray other && Equals(other);

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for the current BigTritArray.</returns>
    /// <remarks>
    /// This type is mutable, but hash code calculation is allowed for equality checks.
    /// The hash code is computed based on the non-zero trits in the array.
    /// </remarks>
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode", Justification = "this is a mutable type, but we want to allow hash code calculation for equality checks")]
    public override int GetHashCode()
    {
        if (Length == 0) return 0;
        if (Negative.Count == 1) return HashCode.Combine(Negative[0], Positive[0]);
        return HashCode.Combine(
            Enumerable.Reverse(Positive).SkipWhile(x => x == 0).Aggregate(0, HashCode.Combine),
            Enumerable.Reverse(Negative).SkipWhile(x => x == 0).Aggregate(0, HashCode.Combine)
        );
    }

    #region Operators

    /// <summary>
    /// Applies a unary operation to each trit in the array.
    /// </summary>
    /// <param name="array">The source array.</param>
    /// <param name="operation">The unary operation to apply to each trit.</param>
    /// <returns>A new TritArray with the operation applied to each trit.</returns>
    public static BigTritArray operator |(BigTritArray array, Func<Trit, Trit> operation) => array | new UnaryTritOperator(operation);

    /// <summary>
    /// Applies a lookup table operation to each trit in the array.
    /// </summary>
    /// <param name="array">The source array.</param>
    /// <param name="table">The lookup table containing the transformation values.</param>
    /// <returns>A new TritArray with the lookup operation applied to each trit.</returns>
    public static BigTritArray operator |(BigTritArray array, Trit[] table)
    {
        if (table.Length != 3)
            throw new ArgumentException("Table must contain exactly three elements for Negative, Zero, and Positive inputs", nameof(table));

        var result = new BigTritArray(array.Length);
        for (var i = 0; i < array.Length; i++)
        {
            result[i] = array[i] | table;
        }

        return result.ApplyLength();
    }

    /// <summary>
    /// Creates a binary operation context for this array.
    /// </summary>
    /// <param name="array">The source array.</param>
    /// <param name="operation">The binary operation to be applied.</param>
    /// <returns>A binary operation context that can be used with another array.</returns>
    public static LookupBigTritArrayOperator operator |(BigTritArray array, Func<Trit, Trit, Trit> operation) => new(array, new(operation));

    /// <summary>
    /// Creates a binary operation context for this array.
    /// </summary>
    /// <param name="array">The source array.</param>
    /// <param name="table">The binary operation lookup table.</param>
    /// <returns>A binary operation context that can be used with another array.</returns>
    public static LookupBigTritArrayOperator operator |(BigTritArray array, BinaryTritOperator table) => new(array, table);

    /// <summary>
    /// Creates a binary operation context for this array.
    /// </summary>
    /// <param name="array">The source array.</param>
    /// <param name="table">The binary operation lookup table.</param>
    /// <returns>A binary operation context that can be used with another array.</returns>
    public static LookupBigTritArrayOperator operator |(BigTritArray array, Trit[,] table) => new(array, new(table));

    /// <summary>
    /// Performs a left bitwise shift on the trit array.
    /// </summary>
    /// <param name="array">The source array.</param>
    /// <param name="shift">The number of positions to shift.</param>
    /// <returns>A new TritArray with the bits shifted to the left.</returns>
    public static BigTritArray operator <<(BigTritArray array, int shift)
    {
        Calculator.ShiftLeft(array.Negative, array.Positive, shift, out var negativeResult, out var positiveResult);
        return new BigTritArray(negativeResult, positiveResult, array.Length).ApplyLength();
    }

    /// <summary>
    /// Performs a right bitwise shift on the trit array.
    /// </summary>
    /// <param name="array">The source array.</param>
    /// <param name="shift">The number of positions to shift.</param>
    /// <returns>A new TritArray with the bits shifted to the right.</returns>
    public static BigTritArray operator >> (BigTritArray array, int shift)
    {
        Calculator.ShiftRight(array.Negative, array.Positive, shift, out var negativeResult, out var positiveResult);
        return new BigTritArray(negativeResult, positiveResult, array.Length).ApplyLength();
    }

    /// <summary>
    /// Adds two TritArray values together.
    /// </summary>
    /// <param name="value1">The first value to add.</param>
    /// <param name="value2">The second value to add.</param>
    /// <returns>A new TritArray representing the sum of the two values.</returns>
    public static BigTritArray operator +(BigTritArray value1, BigTritArray value2)
    {
        Calculator.AddBalancedTernary(value1.Negative, value1.Positive, value2.Negative, value2.Positive, out var negativeResult, out var positiveResult);
        var length = Calculator.TrimAndDetermineLength(negativeResult, positiveResult);
        return new BigTritArray(negativeResult, positiveResult, length).ApplyLength();
    }

    /// <summary>
    /// Subtracts one TritArray value from another.
    /// </summary>
    /// <param name="value1">The value to subtract from.</param>
    /// <param name="value2">The value to subtract.</param>
    /// <returns>A new TritArray representing the difference between the two values.</returns>
    public static BigTritArray operator -(BigTritArray value1, BigTritArray value2)
    {
        value1.ApplyLength();
        value2.ApplyLength();
        Calculator.AddBalancedTernary(value1.Negative, value1.Positive, value2.Positive, value2.Negative, out var negativeResult, out var positiveResult);
        var length = Calculator.TrimAndDetermineLength(negativeResult, positiveResult);
        return new BigTritArray(negativeResult, positiveResult, length).ApplyLength();
    }

    /// <summary>
    /// Multiplies one TritArray value from another.
    /// </summary>
    /// <param name="value1">The value to subtract from.</param>
    /// <param name="value2">The value to subtract.</param>
    /// <returns>A new TritArray representing the product between the two values.</returns>
    public static BigTritArray operator *(BigTritArray value1, BigTritArray value2)
    {
        value1.ApplyLength();
        value2.ApplyLength();
        Calculator.MultiplyBalancedTernary(value1.Negative, value1.Positive, value2.Positive, value2.Negative,
            out var negativeResult, out var positiveResult);
        var length = Calculator.TrimAndDetermineLength(negativeResult, positiveResult);
        return new BigTritArray(negativeResult, positiveResult, length).ApplyLength();
    }

    #endregion


    #region Conversion Operators

    /// <summary>
    /// Implicit conversion from BigInteger to TritArray.
    /// </summary>
    public static implicit operator BigTritArray(BigInteger value)
    {
        TritConverter.ToTrits(value, out var negative, out var positive, out var length);
        return new(negative, positive, length);
    }

    /// <summary>
    /// Implicit conversion from long to TritArray.
    /// </summary>
    public static implicit operator BigTritArray(long value)
    {
        TritConverter.ToTrits(value, out var negative, out var positive, out var length);
        return new(negative, positive, length);
    }

    /// <summary>
    /// Implicit conversion from int to TritArray.
    /// </summary>
    public static implicit operator BigTritArray(int value)
    {
        TritConverter.ToTrits(value, out var negative, out var positive, out var length);
        return new(negative, positive, length);
    }

    /// <summary>
    /// Implicit conversion from TritArray to BigInteger.
    /// </summary>
    public static implicit operator BigInteger(BigTritArray array) => TritConverter.ToBigInteger(array.Negative, array.Positive);

    /// <summary>
    /// Explicit conversion from TritArray to long.
    /// </summary>
    public static explicit operator long(BigTritArray array) => TritConverter.ToInt64(array.Negative, array.Positive, array.Length);

    /// <summary>
    /// Explicit conversion from TritArray to int.
    /// </summary>
    public static explicit operator int(BigTritArray array) => TritConverter.ToInt32(array.Negative, array.Positive, array.Length);

    #endregion
}