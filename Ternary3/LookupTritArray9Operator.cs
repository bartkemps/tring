// <auto-generated />
namespace Ternary3;

using Operators;
using Operators.Operations;
using TritArrays;

/// <summary>
/// Represents an operator that combines a TernaryArray9 with a binary operation lookup table.
/// </summary>
/// <remarks>
/// Used to efficiently apply binary operations between TernaryArray9 instances by using optimized lookup tables.
/// The first operand is stored within the operator structure, and the second operand is provided via the pipe operator.
/// </remarks>
public readonly struct LookupTritArray9Operator
{
    private readonly TernaryArray9 ternaries;
    private readonly BinaryTritOperator table;

    internal LookupTritArray9Operator(TernaryArray9 ternaries, BinaryTritOperator table)
    {
        this.ternaries = ternaries;
        this.table = table;
    }

    internal LookupTritArray9Operator(TernaryArray9 ternaries, Trit[,] table)
        : this(ternaries, new BinaryTritOperator(table))
    {
    }

    internal LookupTritArray9Operator(TernaryArray9 ternaries, ReadOnlySpan<Trit> table)
        : this(ternaries, new BinaryTritOperator(table))
    {
    }

    internal LookupTritArray9Operator(TernaryArray9 ternaries, Func<Trit, Trit, Trit> operation)
        : this(ternaries, new BinaryTritOperator(operation))
    {
    }

    /// <summary>
    /// Performs a binary operation between the stored left operand (TernaryArray9) and the right operand using a lookup table.
    /// </summary>
    /// <param name="left">The LookupTritArray9Operator containing the left operand and operation details.</param>
    /// <param name="right">The right TernaryArray9 operand.</param>
    /// <returns>A new TernaryArray9 representing the result of applying the binary operation to each corresponding pair of ternaries.</returns>
    public static TernaryArray9 operator |(LookupTritArray9Operator left, TernaryArray9 right)
    {
        left.table.Apply(left.ternaries.Negative, left.ternaries.Positive, right.Negative, right.Positive, out var negative, out var positive);
        return new(negative, positive);
    }
    
    /// <summary>
    /// Performs a binary operation between the stored left operand (TernaryArray9) and an Int9T right operand using a lookup table.
    /// </summary>
    /// <param name="left">The LookupTritArray9Operator containing the left operand and operation details.</param>
    /// <param name="right">The right Int9T operand, which will be converted to a TernaryArray9.</param>
    /// <returns>A new TernaryArray9 representing the result of applying the binary operation to each corresponding pair of ternaries.</returns>
    public static TernaryArray9 operator |(LookupTritArray9Operator left, Int9T right)
    {
        var tritArray = (TernaryArray9)right;
        left.table.Apply(left.ternaries.Negative, left.ternaries.Positive, tritArray.Negative, tritArray.Positive, out var negative, out var positive);
        return new(negative, positive);
    }
       
    /// <summary>
    /// Performs a binary operation between the stored left operand (TernaryArray9) and a Int16 right operand using a lookup table.
    /// </summary>
    /// <param name="left">The LookupTritArray9Operator containing the left operand and operation details.</param>
    /// <param name="right">The right Int16 operand, which will be converted to a TernaryArray9.</param>
    /// <returns>A new TernaryArray9 representing the result of applying the binary operation to each corresponding pair of ternaries.</returns>
    public static TernaryArray9 operator |(LookupTritArray9Operator left, Int16 right)
    {
        TritConverter.To32Trits(right, out var rightNegative, out var rightPositive);
        left.table.Apply(left.ternaries.Negative, left.ternaries.Positive, (UInt16)rightNegative, (UInt16)rightPositive, out var negative, out var positive);
        return new(negative, positive);
    }
}