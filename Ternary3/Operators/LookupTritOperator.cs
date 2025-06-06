namespace Ternary3.Operators;

using Numbers;

/// <summary>
/// Represents an operator that combines a Trit value with a pre-computed lookup table for operations.
/// </summary>
/// <remarks>
/// Used exclusively with Trit's pipe operator overload to enable the syntax: 
/// <code>trit1 | operationLookupTable | trit2</code>
/// Optimizes performance by using direct table lookups instead of function calls.
/// The lookup table is a specialized 3x3 matrix where indices correspond to:
/// [leftTrit.Value+1, rightValue+1] for the values (-1,0,1).
/// </remarks>
public readonly struct ArrayLookupTritOperator
{
    private readonly Trit trit;
    private readonly Trit[,] table;

    internal ArrayLookupTritOperator(Trit trit, Trit[,] table)
    {
        if (table.GetLength(0) != 3 || table.GetLength(1) != 3)
        {
            throw new ArgumentException("Lookup table must be a 3x3 matrix.", nameof(table));
        }

        this.trit = trit;
        this.table = table;
    }

    public static Trit operator |(ArrayLookupTritOperator left, Trit right) => left.table[left.trit.Value + 1, right.Value + 1];
}

/// <summary>
/// Represents an operator that combines a Trit value with a pre-computed lookup table for operations.
/// </summary>
/// <remarks>
/// Used exclusively with Trit's pipe operator overload to enable the syntax: 
/// <code>trit1 | operationLookupTable | trit2</code>
/// Optimizes performance by using direct table lookups instead of function calls.
/// The lookup table is a specialized 3x3 matrix where indices correspond to:
/// [leftTrit.Value+1, rightValue+1] for the values (-1,0,1).
/// </remarks>
public readonly struct LookupTritOperator
{
    private readonly Trit trit;
    private readonly TritLookupTable table;

    internal LookupTritOperator(Trit trit, TritLookupTable table)
    {
        this.trit = trit;
        this.table = table;
    }

    public static Trit operator |(LookupTritOperator left, Trit right) => left.table.GetTrit(left.trit, right);
}