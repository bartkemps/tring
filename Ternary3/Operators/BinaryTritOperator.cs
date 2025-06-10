namespace Ternary3.Operators;

using System.Diagnostics;
using System.Runtime.CompilerServices;

/// <summary>
/// Represents a specialized 3x3 lookup table for Trit operations.
/// </summary>
/// <remarks>
/// Optimized for performance using two shorts as backing storage. Each trit uses 1 bit in each short:
/// - A trit is negative when its bit in Negative is 1 and its bit in Positive is 0
/// - A trit is zero when both bits are 0
/// - A trit is positive when its bit in Negative is 0 and its bit in Positive is 1
/// This representation allows for efficient lookups while minimizing storage (18 bits total).
/// </remarks>
[DebuggerDisplay("{DebugView()}")]
public partial struct BinaryTritOperator : IEquatable<BinaryTritOperator>
{
    internal TritArray9 Value;

    /// <summary>
    /// Creates a 3x3 BinaryTritOperator 
    /// </summary>
    public BinaryTritOperator()
    {
    }

    internal BinaryTritOperator(ushort negative, ushort positive)
    {
        Value = new(negative, positive);
    }

    public BinaryTritOperator(ReadOnlySpan<Trit> trits)
    {
        if (trits.Length != 9)
        {
            throw new ArgumentException("Trit array must contain exactly 9 elements.", nameof(trits));
        }

        Value[0] = trits[0];
        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 3; j++)
        {
            Value[i + 3 * j] = trits[i * 3 + j];
        }
    }

    /// <summary>
    /// Creates a BinaryTritOperator from individual Trit values in row-major order.
    /// </summary>
    /// <remarks>
    /// Parameters represent each cell in the 3x3 matrix, in row-major order:
    /// tTT tT0 tT1
    /// t0T t00 t01
    /// t1T t10 t11
    /// </remarks>
    public BinaryTritOperator(
        // ReSharper disable once InconsistentNaming
        Trit tritTT, Trit tritT0, Trit tritT1, Trit trit0T, Trit trit00, Trit trit01, Trit trit1T, Trit trit10, Trit trit11
    )
    {
        Value[0] = tritTT;
        Value[1] = tritT0;
        Value[2] = tritT1;
        Value[3] = trit0T;
        Value[4] = trit00;
        Value[5] = trit01;
        Value[6] = trit1T;
        Value[7] = trit10;
        Value[8] = trit11;
    }

    /// <summary>
    /// Creates a BinaryTritOperator from a <see cref="TritArray27"/>.
    /// </summary>
    /// <remarks>
    /// Parameters represent each cell in the 3x3 matrix, in row-major order:
    /// tTT tT0 tT1
    /// t0T t00 t01
    /// t1T t10 t11
    /// </remarks>
    public BinaryTritOperator(TritArray9 trits)
    {
        Value = trits;
    }

    public BinaryTritOperator(Trit[,] trits)
    {
        // check the dimension 
        if (trits.GetLength(0) != 3 || trits.GetLength(1) != 3)
        {
            throw new ArgumentException("Trit array must be a 3x3 matrix.");
        }

        // Fill the lookup table in row-major order
        // tTT tT0 tT1
        // t0T t00 t01
        // t1T t10 t11
        Value[0] = trits[0, 0];
        Value[1] = trits[0, 1];
        Value[2] = trits[0, 2];
        Value[3] = trits[1, 0];
        Value[4] = trits[1, 1];
        Value[5] = trits[1, 2];
        Value[6] = trits[2, 0];
        Value[7] = trits[2, 1];
        Value[8] = trits[2, 2];
    }

    public BinaryTritOperator(int[][] trits)
    {
        if (trits.Length != 3) throw new ArgumentException("Trit array must be a 3x3 matrix.", nameof(trits));
        for (var i = 0; i < 3; i++)
        {
            if (trits[i].Length != 3) throw new ArgumentException("Trit array must be a 3x3 matrix.", nameof(trits));
            for (var j = 0; j < 3; j++)
            {
                switch (trits[i][j])
                {
                    case -1: Value.Negative |= (ushort)(1 << (i * 3 + j)); break;
                    case 0: break;
                    case 1: Value.Positive |= (ushort)(1 << (i * 3 + j)); break;
                    default: throw new ArgumentException("All values must be -1, 0 or 1", nameof(trits));
                }
            }
        }
    }

    internal BinaryTritOperator(Func<Trit, Trit, Trit> operation) : this(
        operation(Trit.Negative, Trit.Negative),
        operation(Trit.Negative, Trit.Zero),
        operation(Trit.Negative, Trit.Positive),
        operation(Trit.Zero, Trit.Negative),
        operation(Trit.Zero, Trit.Zero),
        operation(Trit.Zero, Trit.Positive),
        operation(Trit.Positive, Trit.Negative),
        operation(Trit.Positive, Trit.Zero),
        operation(Trit.Positive, Trit.Positive))
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetTrit(int position, Trit value)
    {
        switch (value.Value)
        {
            case -1:
                Value.Negative |= (ushort)(1 << position);
                Value.Positive &= (ushort)~(1 << position);
                return;
            case 0:
                Value.Negative &= (ushort)~(1 << position);
                Value.Positive &= (ushort)~(1 << position);
                return;
            case 1:
                Value.Positive |= (ushort)(1 << position);
                Value.Negative &= (ushort)~(1 << position);
                return;
        }
    }


    /// <summary>
    /// Accesses the lookup table to get the result for a pair of Trit values.
    /// </summary>
    public Trit this[Trit left, Trit right]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Value[left.Value * 3 + right.Value + 4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        init => Value[left.Value * 3 + right.Value + 4] = value;
    }

    /// <summary>
    /// Adds the pipe operator to <see cref="long"/>.
    /// Overflows if the value exceeds the range of Int27T.
    /// </summary>
    /// <param name="value">The value to convert to trits</param>
    /// <param name="table">The lookup table</param>
    public static LookupTritArray27Operator operator |(long value, BinaryTritOperator table) => new((Int27T)value, table);

    /// <summary>
    /// Adds the pipe operator to <see cref="int"/>.
    /// </summary>
    /// <param name="value">The value to convert to trits</param>
    /// <param name="table">The lookup table</param>
    public static LookupTritArray27Operator operator |(int value, BinaryTritOperator table) => new((Int27T)value, table);

    /// <summary>
    /// Adds the pipe operator to <see cref="short"/>.
    /// Overflows if the value exceeds the range of Int27T.
    /// </summary>
    /// <param name="value">The value to convert to trits</param>
    /// <param name="table">The lookup table</param>
    public static LookupTritArray9Operator operator |(short value, BinaryTritOperator table) => new((Int9T)value, table);

    /// <summary>
    /// Adds the pipe operator to <see cref="sbyte"/>.
    /// Overflows if the value exceeds the range of Int27T.
    /// </summary>
    /// <param name="value">The value to convert to trits</param>
    /// <param name="table">The lookup table</param>
    public static LookupTritArray3Operator operator |(sbyte value, BinaryTritOperator table) => new((Int3T)value, table);

    /// <summary>
    /// Determines whether the specified object is equal to the current BinaryTritOperator.
    /// </summary>
    /// <param name="obj">The object to compare with the current BinaryTritOperator.</param>
    /// <returns>true if the objects are equal; otherwise, false.</returns>
    public readonly override bool Equals(object? obj)
    {
        return obj is BinaryTritOperator table && Equals(table);
    }

    /// <summary>
    /// Determines whether the specified BinaryTritOperator is equal to the current BinaryTritOperator.
    /// </summary>
    /// <param name="other">The BinaryTritOperator to compare with the current BinaryTritOperator.</param>
    /// <returns>true if the specified BinaryTritOperator is equal to the current BinaryTritOperator; otherwise, false.</returns>
    public readonly bool Equals(BinaryTritOperator other) => Value.Equals(other.Value);

    /// <summary>
    /// Returns the hash code for this BinaryTritOperator.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public readonly override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// Determines whether two specified BinaryTritOperator objects have the same value.
    /// </summary>
    /// <param name="left">The first BinaryTritOperator to compare.</param>
    /// <param name="right">The second BinaryTritOperator to compare.</param>
    /// <returns>true if the value of left is the same as the value of right; otherwise, false.</returns>
    public static bool operator ==(BinaryTritOperator left, BinaryTritOperator right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified BinaryTritOperator objects have different values.
    /// </summary>
    /// <param name="left">The first BinaryTritOperator to compare.</param>
    /// <param name="right">The second BinaryTritOperator to compare.</param>
    /// <returns>true if the value of left is different from the value of right; otherwise, false.</returns>
    public static bool operator !=(BinaryTritOperator left, BinaryTritOperator right) => !left.Equals(right);

    /// <summary>
    /// Returns a string representation of the lookup table in a grid format.
    /// </summary>
    /// <returns>A formatted string showing the 3x3 lookup table.</returns>
    internal readonly string DebugView()
    {
        var negative = Value.Negative;
        var postive = Value.Positive;
        return $"{T(0)} {T(1)} {T(2)} / {T(3)} {T(4)} {T(5)} / {T(6)} {T(7)} {T(8)}";

        string T(int i)
        {
            var isNegative = ((negative >> i) & 1) == 1;
            var isPositive = ((postive >> i) & 1) == 1;
            return isPositive ? "1" : isNegative ? "T" : "0";
        }
    }

    /// <summary>
    /// Returns a string representation of the lookup table in a grid format.
    /// </summary>
    /// <returns>A formatted string showing the 3x3 lookup table.</returns>
    public readonly override string ToString()
    {
        var negative = Value.Negative;
        var postitive = Value.Positive;
        return $"""
                   | T 0 1
                ---+-------
                 T | {T(0)} {T(1)} {T(2)}
                 0 | {T(3)} {T(4)} {T(5)}
                 1 | {T(6)} {T(7)} {T(8)}
                """;

        string T(int i)
        {
            var isNegative = ((negative >> i) & 1) == 1;
            var isPositive = ((postitive >> i) & 1) == 1;
            return isPositive ? "1" : isNegative ? "T" : "0";
        }
    }
}