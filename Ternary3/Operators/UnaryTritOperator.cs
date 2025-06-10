namespace Ternary3.Operators;

using Operations;

/// <summary>
/// Represents a unary operator for trit operations.
/// </summary>
public partial struct UnaryTritOperator
{
    int operationIndex = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnaryTritOperator"/> struct with the specified operation index.
    /// </summary>
    /// <param name="operationIndex">The index of the operation in the lookup table.</param>
    private UnaryTritOperator(int operationIndex) => this.operationIndex = operationIndex;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnaryTritOperator"/> struct with the specified operation function.
    /// </summary>
    /// <param name="operation">A function that defines the operation to perform on a trit.</param>
    /// <exception cref="ArgumentException">Thrown when the operation is null.</exception>
    public UnaryTritOperator(Func<Trit, Trit> operation) :
        this((operation ?? throw new ArgumentException("Operation must not be null"))(Trit.Negative),
            operation(Trit.Zero),
            operation(Trit.Positive))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnaryTritOperator"/> struct with a lookup span of trits.
    /// </summary>
    /// <param name="lookup">A span of three trits representing the operation results for negative, zero, and positive inputs.</param>
    /// <exception cref="ArgumentException">Thrown when the lookup span does not contain exactly three trits.</exception>
    public UnaryTritOperator(Span<Trit> lookup)
    {
        if (lookup.Length != 3) throw new ArgumentException("Lookup must be Three trits long", nameof(lookup));
        operationIndex = 13+lookup[0].Value + 3 * lookup[1].Value + 9 * lookup[2].Value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnaryTritOperator"/> struct with the results for each input trit value.
    /// </summary>
    /// <param name="trit1">The result for Trit.Negative input.</param>
    /// <param name="trit2">The result for Trit.Zero input.</param>
    /// <param name="trit3">The result for Trit.Positive input.</param>
    public UnaryTritOperator(Trit trit1, Trit trit2, Trit trit3) => operationIndex = 13+trit1.Value + 3 * trit2.Value + 9 * trit3.Value;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnaryTritOperator"/> struct with three integers representing trit values.
    /// </summary>
    /// <param name="value1">The result for Trit.Negative input (-1, 0, or 1).</param>
    /// <param name="value2">The result for Trit.Zero input (-1, 0, or 1).</param>
    /// <param name="value3">The result for Trit.Positive input (-1, 0, or 1).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any value is not -1, 0, or 1.</exception>
    public UnaryTritOperator(int value1, int value2, int value3)
    {
        if (value1 is < -1 or > 1) throw new ArgumentOutOfRangeException(nameof(value1), "Value must be -1, 0, or 1");
        if (value2 is < -1 or > 1) throw new ArgumentOutOfRangeException(nameof(value2), "Value must be -1, 0, or 1");
        if (value3 is < -1 or > 1) throw new ArgumentOutOfRangeException(nameof(value3), "Value must be -1, 0, or 1");
        operationIndex = 13+value1 + 3 * value2 + 9 * value3;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnaryTritOperator"/> struct with a span of integers.
    /// </summary>
    /// <param name="values">A span containing exactly 3 integers (-1, 0, or 1) representing trit values 
    /// in order of Negative, Zero, Positive inputs.</param>
    /// <exception cref="ArgumentException">Thrown when the span does not contain exactly three elements.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any value in the span is not -1, 0, or 1.</exception>
    public UnaryTritOperator(ReadOnlySpan<int> values)
    {
        if (values.Length != 3) throw new ArgumentException("Span must contain exactly three values", nameof(values));
        for (var i = 0; i < 3; i++)
        {
            if (values[i] is < -1 or > 1) throw new ArgumentOutOfRangeException(nameof(values), $"Value at index {i} must be -1, 0, or 1");
        }

        operationIndex = 13+values[0] + 3 * values[1] + 9 * values[2];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnaryTritOperator"/> struct with an array of nullable boolean values.
    /// </summary>
    /// <param name="values">An array containing exactly 3 nullable boolean values (false = -1, null = 0, true = 1)
    /// in order of Negative, Zero, Positive inputs.</param>
    /// <exception cref="ArgumentException">Thrown when the array does not contain exactly three elements.</exception>
    public UnaryTritOperator(ReadOnlySpan<bool?> values)
    {
        if (values.Length != 3) throw new ArgumentException("Array must contain exactly three values", nameof(values));

        var trit1 = values[0] switch { true => 2, false => 0, _ => -1 };
        var trit2 = values[1] switch { true => 2, false => 0, _ => -1 };
        var trit3 = values[2] switch { true => 2, false => 0, _ => -1 };

        operationIndex = trit1 + 3 * trit2 + 9 * trit3;
    }

    /// <summary>
    /// Applies the unary operation to a single trit.
    /// </summary>
    /// <param name="trit">The trit to operate on.</param>
    /// <param name="unaryTritOperator">The operator to apply.</param>
    /// <returns>The result of applying the operation to the trit.</returns>
    public static Trit operator |(Trit trit, UnaryTritOperator unaryTritOperator) => UnaryOperation.TritOperations[unaryTritOperator.operationIndex](trit);

    /// <summary>
    /// Applies the unary operation to a TritArray3 instance.
    /// </summary>
    /// <param name="trits">The TritArray3 to operate on.</param>
    /// <param name="unaryTritOperator">The operator to apply.</param>
    /// <returns>A new TritArray3 with the results of applying the operation to each trit.</returns>
    public static TritArray3 operator |(TritArray3 trits, UnaryTritOperator unaryTritOperator)
    {
        var result = UnaryOperation.BytePairOperations[unaryTritOperator.operationIndex](trits.Negative, trits.Positive);
        return new(result.Negative, result.Positive);
    }

    /// <summary>
    /// Applies the unary operation to a TritArray9 instance.
    /// </summary>
    /// <param name="trits">The TritArray9 to operate on.</param>
    /// <param name="unaryTritOperator">The operator to apply.</param>
    /// <returns>A new TritArray9 with the results of applying the operation to each trit.</returns>
    public static TritArray9 operator |(TritArray9 trits, UnaryTritOperator unaryTritOperator)
    {
        var result = UnaryOperation.UInt16PairOperations[unaryTritOperator.operationIndex](trits.Negative, trits.Positive);
        return new(result.Negative, result.Positive);
    }

    /// <summary>
    /// Applies the unary operation to a TritArray27 instance.
    /// </summary>
    /// <param name="trits">The TritArray27 to operate on.</param>
    /// <param name="unaryTritOperator">The operator to apply.</param>
    /// <returns>A new TritArray27 with the results of applying the operation to each trit.</returns>
    public static TritArray27 operator |(TritArray27 trits, UnaryTritOperator unaryTritOperator)
    {
        var result = UnaryOperation.UInt32PairOperations[unaryTritOperator.operationIndex](trits.Negative, trits.Positive);
        return new(result.Negative, result.Positive);
    }

    /// <summary>
    /// Applies the unary operation to an Int3T instance by converting it to a TritArray3 first.
    /// </summary>
    /// <param name="trits">The Int3T to operate on.</param>
    /// <param name="unaryTritOperator">The operator to apply.</param>
    /// <returns>A new TritArray3 with the results of applying the operation.</returns>
    public static TritArray3 operator |(Int3T trits, UnaryTritOperator unaryTritOperator) => ((TritArray3)trits) | unaryTritOperator;

    /// <summary>
    /// Applies the unary operation to an Int9T instance by converting it to a TritArray9 first.
    /// </summary>
    /// <param name="trits">The Int9T to operate on.</param>
    /// <param name="unaryTritOperator">The operator to apply.</param>
    /// <returns>A new TritArray9 with the results of applying the operation.</returns>
    public static TritArray9 operator |(Int9T trits, UnaryTritOperator unaryTritOperator) => ((TritArray9)trits) | unaryTritOperator;

    /// <summary>
    /// Applies the unary operation to an Int27T instance by converting it to a TritArray27 first.
    /// </summary>
    /// <param name="trits">The Int27T to operate on.</param>
    /// <param name="unaryTritOperator">The operator to apply.</param>
    /// <returns>A new TritArray27 with the results of applying the operation.</returns>
    public static TritArray27 operator |(Int27T trits, UnaryTritOperator unaryTritOperator) => ((TritArray27)trits) | unaryTritOperator;

    /// <summary>
    /// Applies the unary operation to an sbyte by converting it to a TritArray3 first.
    /// </summary>
    /// <param name="trits">The sbyte to operate on.</param>
    /// <param name="unaryTritOperator">The operator to apply.</param>
    /// <returns>A new TritArray3 with the results of applying the operation.</returns>
    public static TritArray3 operator |(sbyte trits, UnaryTritOperator unaryTritOperator) => ((TritArray3)trits) | unaryTritOperator;

    /// <summary>
    /// Applies the unary operation to a short by converting it to a TritArray9 first.
    /// </summary>
    /// <param name="trits">The short to operate on.</param>
    /// <param name="unaryTritOperator">The operator to apply.</param>
    /// <returns>A new TritArray9 with the results of applying the operation.</returns>
    public static TritArray9 operator |(short trits, UnaryTritOperator unaryTritOperator) => ((TritArray9)trits) | unaryTritOperator;

    /// <summary>
    /// Applies the unary operation to an int by converting it to a TritArray27 first.
    /// </summary>
    /// <param name="trits">The int to operate on.</param>
    /// <param name="unaryTritOperator">The operator to apply.</param>
    /// <returns>A new TritArray27 with the results of applying the operation.</returns>
    public static TritArray27 operator |(int trits, UnaryTritOperator unaryTritOperator) => ((TritArray27)trits) | unaryTritOperator;

    /// <summary>
    /// Applies the unary operation to a long by converting it to a TritArray27 first.
    /// </summary>
    /// <param name="trits">The long to operate on.</param>
    /// <param name="unaryTritOperator">The operator to apply.</param>
    /// <returns>A new TritArray27 with the results of applying the operation.</returns>
    public static TritArray27 operator |(long trits, UnaryTritOperator unaryTritOperator) => ((TritArray27)trits) | unaryTritOperator;
}