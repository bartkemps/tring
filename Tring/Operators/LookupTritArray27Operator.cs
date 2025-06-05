namespace Tring.Operators;

using Numbers;
using Operations;

public readonly struct LookupTritArray27Operator
{
    private readonly TritArray27 trits;
    private readonly BinaryOperationBuilder<uint> builder;

    internal LookupTritArray27Operator(TritArray27 trits, TritLookupTable table)
    {
        this.trits = trits;
        this.builder = new(table);
    }

    internal LookupTritArray27Operator(TritArray27 trits, Trit[,] table)
        : this(trits, new TritLookupTable(table))
    {
    }

    internal LookupTritArray27Operator(TritArray27 trits, ReadOnlySpan<Trit> table)
        : this(trits, new TritLookupTable(table))
    {
    }

    internal LookupTritArray27Operator(TritArray27 trits, Func<Trit, Trit, Trit> operation)
        : this(trits, new TritLookupTable(operation))
    {
    }

    public static TritArray27 operator |(LookupTritArray27Operator left, TritArray27 right)
    {
        left.builder.Build()(left.trits.Negative, left.trits.Positive, right.Negative, right.Positive, out var negative, out var positive);
        return new() { Negative = negative, Positive = positive };
    }
}