namespace Ternary3.Operators;

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
public readonly struct BinaryLookupTritOperator
{
    private readonly Trit trit;
    private readonly BinaryTritOperator table;

    internal BinaryLookupTritOperator(Trit trit, Trit[,] table)
    {
        this.trit = trit;
        this.table = new(table);
    }

    internal BinaryLookupTritOperator(Trit trit, BinaryTritOperator table)
    {
        this.trit = trit;
        this.table = table;
    }

    public static Trit operator |(BinaryLookupTritOperator left, Trit right) => left.table[left.trit, right];
}