namespace Ternary3;

using Operators;

/// <summary>
/// Helper class to facilitate binary operations between two BigTritArray instances.
/// </summary>
public class LookupBigTritArrayOperator
{
    private readonly BigTritArray trits;
    private readonly BinaryTritOperator table;

    internal LookupBigTritArrayOperator(BigTritArray trits, BinaryTritOperator table)
    {
        this.trits = trits;
        this.table = table;
    }

    /// <summary>
    /// Applies the binary operation to the left and right arrays.
    /// </summary>
    /// <param name="left">The binary operation context.</param>
    /// <param name="right">The right-hand operand.</param>
    /// <returns>A new BigTritArray with the operation applied.</returns>
    public static BigTritArray operator |(LookupBigTritArrayOperator left, BigTritArray right)
    {
        if (right.Length != left.trits.Length) throw new NotImplementedException();
        left.table.Apply(left.trits.Negative, left.trits.Positive, right.Negative, right.Positive, out var negative, out var positive);
        return new(negative, positive, right.Length);
    }
}