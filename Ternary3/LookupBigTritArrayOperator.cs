namespace Ternary3;

using Operators;

/// <summary>
/// Helper class to facilitate binary operations between two BigTernaryArray instances.
/// </summary>
public class LookupBigTritArrayOperator
{
    private readonly BigTernaryArray ternaries;
    private readonly BinaryTritOperator table;

    internal LookupBigTritArrayOperator(BigTernaryArray ternaries, BinaryTritOperator table)
    {
        this.ternaries = ternaries;
        this.table = table;
    }

    /// <summary>
    /// Applies the binary operation to the left and right arrays.
    /// </summary>
    /// <param name="left">The binary operation context.</param>
    /// <param name="right">The right-hand operand.</param>
    /// <returns>A new BigTernaryArray with the operation applied.</returns>
    public static BigTernaryArray operator |(LookupBigTritArrayOperator left, BigTernaryArray right)
    {
        left.table.Apply(left.ternaries.Negative, left.ternaries.Positive, right.Negative, right.Positive, out var negative, out var positive);
        return new BigTernaryArray(negative, positive, Math.Max(left.ternaries.Length, right.Length)).ApplyLength();
    }

    /// <summary>
    /// Applies the binary operation to the left and right arrays.
    /// </summary>
    /// <param name="left">The binary operation context.</param>
    /// <param name="right">The right-hand operand.</param>
    /// <returns>A new BigTernaryArray with the operation applied.</returns>
    public static BigTernaryArray operator |(LookupBigTritArrayOperator left, TernaryArray right)
    {
        left.table.Apply(left.ternaries.Negative, left.ternaries.Positive, [right.Negative], [right.Positive], out var negative, out var positive);
        return new BigTernaryArray(negative, positive, Math.Max(left.ternaries.Length, right.NumberOfTrits)).ApplyLength();
    }
}
