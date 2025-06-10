namespace Ternary3.Operators;

/// <summary>
/// Represents an operator that combines a Trit value with a delegate to an operation.
/// </summary>
/// <remarks>
/// Used exclusively with Trit's pipe operator overload to enable the syntax: 
/// <code>trit1 | Operation.ApplyFunc | trit2</code>
/// Provides a safe alternative to UnsafeTritOperator by using delegates instead of function pointers.
/// Can be used in any context without unsafe code requirements.
/// </remarks>
public readonly struct BinaryMethodTritOperator
{
    private readonly Trit trit;
    private readonly Func<Trit, Trit, Trit> operation;

    internal BinaryMethodTritOperator(Trit trit, Func<Trit, Trit, Trit> operation)
    {
        this.trit = trit;
        this.operation = operation;
    }

    /// <summary>
    /// Performs a binary operation between the stored left operand (trit) and the right operand using the operation function.
    /// </summary>
    /// <param name="left">The BinaryMethodTritOperator containing the left operand and the operation function.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>The result of applying the binary operation to the two trit operands.</returns>
    public static Trit operator |(BinaryMethodTritOperator left, Trit right) => left.operation(left.trit, right);
}