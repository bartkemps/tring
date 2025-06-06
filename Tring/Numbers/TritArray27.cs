namespace Tring.Numbers;

using Operators;
using TritArray;
using TritArrays;

/// <summary>
/// Represents a fixed-size array of 27 trits (ternary digits).
/// </summary>
public struct TritArray27 : ITritArray
{
    internal uint Positive;
    internal uint Negative;

    /// <summary>
    /// Initializes a new instance of the TritArray27 struct with all trits set to zero.
    /// </summary>
    public TritArray27()
    {
    }

    private TritArray27(UInt32Pair trits)
    {
        Negative = trits.Negative;
        Positive = trits.Positive;
    }

    /// <summary>
    /// Gets or sets the trit at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the trit to get or set (must be between 0 and 26).</param>
    /// <returns>The trit at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when index is less than 0 or greater than 26.</exception>
    public Trit this[int index]
    {
        get
        {
            if (index is < 0 or >= 27)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 26.");
            }
            return TritConverter.GetTrit(ref Positive, ref Negative, index);
        }
        set
        {
            if (index is < 0 or >= 27)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 26.");
            }

            TritConverter.SetTrit(ref Positive, ref Negative, index, value);
        }
    }

    /// <summary>
    /// Gets the length of the trit array, which is always 27.
    /// </summary>
    public int Length => 27;

    /// <summary>
    /// Applies a unary operation to each trit in the array.
    /// </summary>
    /// <param name="array">The source array.</param>
    /// <param name="operation">The unary operation to apply to each trit.</param>
    /// <returns>A new TritArray27 with the operation applied to each trit.</returns>
    public static TritArray27 operator |(TritArray27 array, Func<Trit, Trit> operation)
        => new(UnaryOperation.Apply(array.Negative, array.Positive, operation));

    /// <summary>
    /// Applies a lookup table operation to each trit in the array.
    /// </summary>
    /// <param name="array">The source array.</param>
    /// <param name="table">The lookup table containing the transformation values.</param>
    /// <returns>A new TritArray27 with the lookup operation applied to each trit.</returns>
    public static TritArray27 operator |(TritArray27 array, Trit[] table)
        => new(UnaryOperation.Apply(array.Negative, array.Positive, table));

    /// <summary>
    /// Creates a binary operation context for this array.
    /// </summary>
    /// <param name="array">The source array.</param>
    /// <param name="operation">The binary operation to be applied.</param>
    /// <returns>A LookupTritArray27Operator that can be used to apply the operation with another array.</returns>
    public static LookupTritArray27Operator operator |(TritArray27 array, Func<Trit, Trit, Trit> operation)
        => new LookupTritArray27Operator(array, operation);
    public static LookupTritArray27Operator operator |(TritArray27 array, TritLookupTable table)
        => new LookupTritArray27Operator(array, table);
    public static LookupTritArray27Operator operator |(TritArray27 array, Trit[,] table)
        => new LookupTritArray27Operator(array, table);
    

    /// <summary>
    /// Performs a left bitwise shift on the trit array.
    /// </summary>
    /// <param name="array">The source array.</param>
    /// <param name="shift">The number of positions to shift.</param>
    /// <returns>A new TritArray27 with the bits shifted to the left.</returns>
    public static TritArray27 operator <<(TritArray27 array, int shift)
    {
        return shift switch
        {
            >= 27 => new(),
            < 0 => array >> -shift,
            _ => new() { Positive = array.Positive << shift, Negative = array.Negative << shift }
        };
    }

    /// <summary>
    /// Performs a right bitwise shift on the trit array.
    /// </summary>
    /// <param name="array">The source array.</param>
    /// <param name="shift">The number of positions to shift.</param>
    /// <returns>A new TritArray27 with the bits shifted to the right.</returns>
    public static TritArray27 operator >> (TritArray27 array, int shift)
    {
        return shift switch
        {
            >= 27 => new(),
            < 0 => array >> -shift,
            _ => new() { Positive = array.Positive >> shift, Negative = array.Negative >> shift }
        };
    }

    /// <summary>
    /// Adds two TritArray27 values together.
    /// </summary>
    /// <param name="value1">The first value to add.</param>
    /// <param name="value2">The second value to add.</param>
    /// <returns>A new TritArray27 representing the sum of the two values.</returns>
    public static TritArray27 operator +(TritArray27 value1, TritArray27 value2)
    {
        Calculator.AddBalancedTernary(value1.Negative, value1.Positive, value2.Negative, value2.Positive, out var negative, out var positive);    
        return new () {Negative = negative, Positive = positive};
    }
    
    /// <summary>
    /// Subtracts one TritArray27 value from another.
    /// </summary>
    /// <param name="value1">The value to subtract from.</param>
    /// <param name="value2">The value to subtract.</param>
    /// <returns>A new TritArray27 representing the difference between the two values.</returns>
    public static TritArray27 operator -(TritArray27 value1, TritArray27 value2)
    {
        Calculator.AddBalancedTernary(value1.Negative, value1.Positive, value2.Positive, value2.Negative, out var negative, out var positive);    
        return new () {Negative = negative, Positive = positive};
    }
}
