namespace Ternary3.Operators;

/// <summary>
/// Represents an operator that combines a Trit value with a function pointer to an operation.
/// </summary>
/// <remarks>
/// Used exclusively with Trit's pipe operator overload to enable the syntax: 
/// <code>trit1 | &amp;Operation.Apply | trit2</code>
/// Provides high-performance operation through direct function pointer invocation.
/// Requires unsafe code context due to the use of function pointers.
/// </remarks>
public readonly unsafe struct UnsafeTritOperator
{
    private readonly Trit trit;
    private readonly delegate*<Trit, Trit, Trit> operation;

    internal UnsafeTritOperator(Trit trit, delegate*<Trit, Trit, Trit> operation)
    {
        this.trit = trit;
        this.operation = operation;
    }

    public static Trit operator |(UnsafeTritOperator left, Trit right) => left.operation(left.trit, right);
}