namespace Ternary3;

using Operators;

/// <summary>
/// Helper class to facilitate binary operations between two TritArray instances.
/// </summary>
public class LookupTritArrayOperator
{
    private readonly TritArray trits;
    private readonly BinaryTritOperator table;

    internal LookupTritArrayOperator(TritArray trits, BinaryTritOperator table)
    {
        this.trits = trits;
        this.table = table;
    }

    /// <summary>
    /// Applies the binary operation to the left and right arrays.
    /// </summary>
    /// <param name="left">The binary operation context.</param>
    /// <param name="right">The right-hand operand.</param>
    /// <returns>A new TritArray with the operation applied.</returns>
    public static TritArray operator |(LookupTritArrayOperator left, TritArray right)
    {
        left.table.Apply(left.trits.Negative, left.trits.Positive, right.Negative, right.Positive, out var negative, out var positive);
        return new TritArray(negative, positive, Math.Max(left.trits.Length, right.Length)).ApplyLength();
    }
}