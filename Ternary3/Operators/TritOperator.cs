namespace Ternary3.Operators;

using Numbers;

/// <summary>
/// Represents an operator that combines a Trit value with a delegate to an operation.
/// </summary>
/// <remarks>
/// Used exclusively with Trit's pipe operator overload to enable the syntax: 
/// <code>trit1 | Operation.ApplyFunc | trit2</code>
/// Provides a safe alternative to UnsafeTritOperator by using delegates instead of function pointers.
/// Can be used in any context without unsafe code requirements.
/// </remarks>
public readonly struct TritOperator
{
    private readonly Trit trit;
    private readonly Func<Trit, Trit, Trit> operation;

    internal TritOperator(Trit trit, Func<Trit, Trit, Trit> operation)
    {
        this.trit = trit;
        this.operation = operation;
    }

    public static Trit operator |(TritOperator left, Trit right) => left.operation(left.trit, right);
}