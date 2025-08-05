namespace Ternary3;

using Operators;

/// <summary>
/// Helper class to facilitate binary operations between two TernaryArray instances.
/// </summary>
public class LookupTernaryArrayOperator
{
    private readonly TernaryArray ternaries;
    private readonly BinaryTritOperator table;

    internal LookupTernaryArrayOperator(TernaryArray ternaries, BinaryTritOperator table)
    {
        this.ternaries = ternaries;
        this.table = table;
    }

    internal LookupTernaryArrayOperator(TernaryArray ternaries, Trit[,] table)
    : this(ternaries, new BinaryTritOperator(table))
    {
    }

    internal LookupTernaryArrayOperator(TernaryArray ternaries, ReadOnlySpan<Trit> table)
        : this(ternaries, new BinaryTritOperator(table))
    {
    }

    internal LookupTernaryArrayOperator(TernaryArray ternaries, Func<Trit, Trit, Trit> operation)
        : this(ternaries, new BinaryTritOperator(operation))
    {
    }

    /// <summary>
    /// Applies the binary operation to the left and right arrays.
    /// </summary>
    /// <param name="left">The binary operation context.</param>
    /// <param name="right">The right-hand operand.</param>
    /// <returns>A new TernaryArray with the operation applied.</returns>
    public static TernaryArray operator |(LookupTernaryArrayOperator left, TernaryArray right)
    {
        left.table.Apply(left.ternaries.Negative, left.ternaries.Positive, right.Negative, right.Positive, out var negative, out var positive);
        return new TernaryArray(negative, positive, Math.Max(left.ternaries.NumberOfTrits, right.NumberOfTrits));
    }

    /// <summary>
    /// Applies the binary operation to the left and right arrays.
    /// </summary>
    /// <param name="left">The binary operation context.</param>
    /// <param name="right">The right-hand operand.</param>
    /// <returns>A new BigTernaryArray with the operation applied.</returns>
    public static BigTernaryArray operator |(LookupTernaryArrayOperator left, BigTernaryArray right)
    {
        left.table.Apply([left.ternaries.Negative], [left.ternaries.Positive], right.Negative, right.Positive, out var negative, out var positive);
        return new BigTernaryArray(negative, positive, Math.Max(left.ternaries.NumberOfTrits, right.Length)).ApplyLength();
    }
}