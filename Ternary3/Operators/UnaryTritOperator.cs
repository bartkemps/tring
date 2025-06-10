namespace Ternary3.Operators;

using Operations;

public partial struct UnaryTritOperator
{
    int operationIndex = 0;
    private UnaryTritOperator(int operationIndex) => this.operationIndex = operationIndex;

    public UnaryTritOperator(Func<Trit, Trit> operation) :
        this((operation ?? throw new ArgumentException("Operation must not be null"))(Trit.Negative),
            operation(Trit.Zero),
            operation(Trit.Positive))
    {
    }

    public UnaryTritOperator(Span<Trit> lookup)
    {
        if (lookup.Length != 3) throw new ArgumentException("Lookup must be Three trits long", nameof(lookup));
        operationIndex = lookup[0].Value + 3 * lookup[1].Value + 9 * lookup[2].Value;
    }

    public UnaryTritOperator(Trit trit1, Trit trit2, Trit trit3) => operationIndex = trit1.Value + 3 * trit2.Value + 9 * trit3.Value;

    public static Trit operator |(Trit trit, UnaryTritOperator unaryTritOperator) => UnaryOperation.TritOperations[unaryTritOperator.operationIndex](trit);

    public static TritArray3 operator |(TritArray3 trits, UnaryTritOperator unaryTritOperator)
    {
        var result = UnaryOperation.BytePairOperations[unaryTritOperator.operationIndex](trits.Negative, trits.Positive);
        return new(result.Negative, result.Positive);
    }

    public static TritArray9 operator |(TritArray9 trits, UnaryTritOperator unaryTritOperator)
    {
        var result = UnaryOperation.UInt16PairOperations[unaryTritOperator.operationIndex](trits.Negative, trits.Positive);
        return new(result.Negative, result.Positive);
    }

    public static TritArray27 operator |(TritArray27 trits, UnaryTritOperator unaryTritOperator)
    {
        var result = UnaryOperation.UInt32PairOperations[unaryTritOperator.operationIndex](trits.Negative, trits.Positive);
        return new(result.Negative, result.Positive);
    }
}