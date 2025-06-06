namespace Ternary3;

using Numbers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

/// <summary>
/// Represents a specialized 3x3 lookup table for Trit operations.
/// </summary>
/// <remarks>
/// Optimized for performance using an Int32 as backing storage where each Trit value
/// uses 2 bits directly storing -1, 0, or 1. This eliminates the need for masking and
/// value conversion at the cost of using more bits per trit (18 bits total).
/// </remarks>
[DebuggerDisplay("{DebugView()}")]
public struct TritLookupTable : IEquatable<TritLookupTable>
{
    // Each Trit uses 2 bits storing the actual -1, 0, 1 value
    internal readonly int Value;

    // Number of bits used per trit value
    private const int BitsPerTrit = 2;
    private const int BitMask = 0b11;

    /// <summary>
    /// Creates a 3x3 TritLookupTable 
    /// </summary>
    public TritLookupTable()
    {
        Value = 0b010101010101010101;
    }

    /// <summary>
    /// Creates a TritLookupTable from individual Trit values in row-major order.
    /// </summary>
    /// <remarks>
    /// Parameters represent each cell in the 3x3 matrix, in row-major order:
    /// tTT tT0 tT1
    /// t0T t00 t01
    /// t1T t10 t11
    /// </remarks>
    public TritLookupTable(
        // ReSharper disable once InconsistentNaming
        Trit tritTT, Trit tritT0, Trit tritT1, Trit trit0T, Trit trit00, Trit trit01, Trit trit1T, Trit trit10, Trit trit11
    )
    {
        Value =
            ((tritTT.Value + 1) << (0 * BitsPerTrit)) |
            ((tritT0.Value + 1) << (1 * BitsPerTrit)) |
            ((tritT1.Value + 1) << (2 * BitsPerTrit)) |
            ((trit0T.Value + 1) << (3 * BitsPerTrit)) |
            ((trit00.Value + 1) << (4 * BitsPerTrit)) |
            ((trit01.Value + 1) << (5 * BitsPerTrit)) |
            ((trit1T.Value + 1) << (6 * BitsPerTrit)) |
            ((trit10.Value + 1) << (7 * BitsPerTrit)) |
            ((trit11.Value + 1) << (8 * BitsPerTrit));
    }

    internal TritLookupTable(int data)
    {
        Value = data;
    }

    internal TritLookupTable(Func<Trit, Trit, Trit> operation) : this(
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

    /// <summary>
    /// Creates a 3x3 TritLookupTable from a 3x3 array of Trit values.
    /// </summary>
    /// <param name="tableData">A 3x3 array of Trit values representing the operation lookup table.</param>
    /// <exception cref="ArgumentException">Thrown if the input array is not a 3x3 matrix.</exception>
    public TritLookupTable(Trit[,] tableData)
    {
        if (tableData.GetLength(0) != 3 || tableData.GetLength(1) != 3)
        {
            throw new ArgumentException("Table must be a 3x3 matrix representing trinary operations.", nameof(tableData));
        }

        Value = 0;
        for (var row = 0; row < 3; row++)
        {
            for (var col = 0; col < 3; col++)
            {
                var position = (row+ 3 * col) * BitsPerTrit;
                Value |= (tableData[row, col].Value + 1) << position;
            }
        }
    }

    /// <summary>
    /// Creates a TritLookupTable from a span of Trit values in row-major order.
    /// </summary>
    /// <param name="flatTable">A span of exactly 9 Trit values in row-major order.</param>
    /// <exception cref="ArgumentException">Thrown if the span doesn't contain exactly 9 elements.</exception>
    public TritLookupTable(ReadOnlySpan<Trit> flatTable)
    {
        if (flatTable.Length != 9)
        {
            throw new ArgumentException("Span must contain exactly 9 Trit values for a 3x3 matrix.", nameof(flatTable));
        }

        Value = 0;
        for (var i = 0; i < 9; i++)
        {
            var position = i * BitsPerTrit;
            Value |= (flatTable[i].Value + 1) << position;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Trit GetTrit(Trit left, Trit right)
    {
        var position = (left.Value + 3 * right.Value + 4) * BitsPerTrit;
        var bits = (Value >> position) & BitMask;
        return new((sbyte)(bits - 1));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TritLookupTable WithTrit(Trit left, Trit right, Trit value)
    {
        var position = (left.Value + 3 * right.Value + 4) * BitsPerTrit;
        var bits = (value.Value + 1) & BitMask;
        var newData = (Value & ~(BitMask << position)) | (bits << position);
        return new(newData);
    }

    /// <summary>
    /// Accesses the lookup table to get the result for a pair of Trit values.
    /// </summary>
    public Trit this[Trit left, Trit right]
    {
        get => GetTrit(left, right);
        init => this = WithTrit(left, right, value);
    }

    /// <summary>
    /// Implicitly converts a 2D array to a TritLookupTable.
    /// </summary>
    /// <param name="tableData">A 3x3 array representing the lookup table.</param>
    /// <returns>A new TritLookupTable instance.</returns>
    public static implicit operator TritLookupTable(Trit[,]? tableData) => tableData == null ? default : new(tableData);

    
    /// <summary>
    /// Adds the pipe operator to <see cref="long"/>.
    /// Overflows if the value exceeds the range of Int27T.
    /// </summary>
    /// <param name="value">The value to convert to trits</param>
    /// <param name="table">The lookup table</param>
    public static LookupTritArray27Operator operator |(long value, TritLookupTable table) => new((Int27T)value, table);

    /// <summary>
    /// Adds the pipe operator to <see cref="int"/>.
    /// </summary>
    /// <param name="value">The value to convert to trits</param>
    /// <param name="table">The lookup table</param>
    public static LookupTritArray27Operator operator |(int value, TritLookupTable table) => new((Int27T)value, table);

    /// <summary>
    /// Adds the pipe operator to <see cref="short"/>.
    /// Overflows if the value exceeds the range of Int27T.
    /// </summary>
    /// <param name="value">The value to convert to trits</param>
    /// <param name="table">The lookup table</param>
    public static LookupTritArray9Operator operator |(short value, TritLookupTable table) => new((Int9T)value, table);

    /// <summary>
    /// Adds the pipe operator to <see cref="sbyte"/>.
    /// Overflows if the value exceeds the range of Int27T.
    /// </summary>
    /// <param name="value">The value to convert to trits</param>
    /// <param name="table">The lookup table</param>
    public static LookupTritArray3Operator operator |(sbyte value, TritLookupTable table) => new((Int3T)value, table);
    
    /// <summary>
    /// Determines whether the specified object is equal to the current TritLookupTable.
    /// </summary>
    /// <param name="obj">The object to compare with the current TritLookupTable.</param>
    /// <returns>true if the objects are equal; otherwise, false.</returns>
    public readonly override bool Equals(object? obj)
    {
        return obj is TritLookupTable table && Equals(table);
    }

    /// <summary>
    /// Determines whether the specified TritLookupTable is equal to the current TritLookupTable.
    /// </summary>
    /// <param name="other">The TritLookupTable to compare with the current TritLookupTable.</param>
    /// <returns>true if the specified TritLookupTable is equal to the current TritLookupTable; otherwise, false.</returns>
    public readonly bool Equals(TritLookupTable other) => Value == other.Value;

    /// <summary>
    /// Returns the hash code for this TritLookupTable.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public readonly override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// Determines whether two specified TritLookupTable objects have the same value.
    /// </summary>
    /// <param name="left">The first TritLookupTable to compare.</param>
    /// <param name="right">The second TritLookupTable to compare.</param>
    /// <returns>true if the value of left is the same as the value of right; otherwise, false.</returns>
    public static bool operator ==(TritLookupTable left, TritLookupTable right) => left.Value == right.Value;

    /// <summary>
    /// Determines whether two specified TritLookupTable objects have different values.
    /// </summary>
    /// <param name="left">The first TritLookupTable to compare.</param>
    /// <param name="right">The second TritLookupTable to compare.</param>
    /// <returns>true if the value of left is different from the value of right; otherwise, false.</returns>
    public static bool operator !=(TritLookupTable left, TritLookupTable right) => left.Value != right.Value;

    /// <summary>
    /// Returns a string representation of the lookup table in a grid format.
    /// </summary>
    /// <returns>A formatted string showing the 3x3 lookup table.</returns>
    internal readonly string DebugView()
    {
        var value = Value;
        return $"{T(0)} {T(3)} {T(6)} / {T(1)} {T(4)} {T(7)} / {T(2)} {T(5)} {T(8)}";

        string T(int i) => ((value >> (i * BitsPerTrit)) & BitMask) switch
        {
            0 => "T",
            1 => "0",
            2 => "1",
            _ => "?"
        };
    }

    /// <summary>
    /// Returns a string representation of the lookup table in a grid format.
    /// </summary>
    /// <returns>A formatted string showing the 3x3 lookup table.</returns>
    public readonly override string ToString()
    {
        var value = Value;
        return $"""
                   | T 0 1
                ---+-------
                 T | {T(0)} {T(3)} {T(6)}
                 0 | {T(1)} {T(4)} {T(7)}
                 1 | {T(2)} {T(5)} {T(8)}
                """;

        string T(int i) => ((value >> (i * BitsPerTrit)) & BitMask) switch
        {
            0 => "T",
            1 => "0",
            2 => "1",
            _ => "?"
        };
    }
}