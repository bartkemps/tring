namespace Ternary3;

using Operators;

/// <summary>
/// Represents a trinary (three-valued) logical value that can be Negative (-1), Zero (0), or Positive (1).
/// </summary>
public readonly struct Trit: IEquatable<Trit>
{
    // In this class, a trit is represented by a short representing its value (-1, 0 and 1)
    // In other parts of this library, trits may be represented differently. 
    
    private const sbyte NegativeValue = -1;
    private const sbyte ZeroValue = 0;
    private const sbyte PositiveValue = 1;
    
    internal readonly sbyte Value;

    internal Trit(sbyte value) => Value = value;
    internal Trit(int value) => Value = (sbyte)value;

    /// <summary>
    /// Converts the specified nullable Boolean value to a Trit value.
    /// </summary>
    /// <param name="value">A nullable Boolean value to convert.</param>
    /// <returns>
    /// A Trit value where:
    /// - true converts to Positive (1)
    /// - false converts to Negative (-1)
    /// - null converts to Zero (0)
    /// </returns>
    public static implicit operator Trit(bool? value) => value switch
    {
        true => Positive,
        false => Negative,
        _ => Zero,
    };

    /// <summary>
    /// Converts the specified Boolean value to a Trit value.
    /// </summary>
    /// <param name="value">A Boolean value to convert.</param>
    /// <returns>
    /// A Trit value where:
    /// - true converts to Positive (1)
    /// - false converts to Negative (-1)
    /// </returns>
    public static implicit operator Trit(bool value) => value ? Positive : Negative;

    /// <summary>
    /// Converts the specified Trit value to a nullable Boolean value.
    /// </summary>
    /// <param name="trit">A Trit value to convert.</param>
    /// <returns>
    /// A nullable Boolean value where:
    /// - Positive (1) converts to true
    /// - Negative (-1) converts to false
    /// - Zero (0) converts to null
    /// </returns>
    public static implicit operator bool?(Trit trit) => trit.Value switch
    {
        PositiveValue => true, // Positive
        NegativeValue => false, // Negative
        _ => null, // Zero
    };

    /// <summary>
    /// Converts the Trit value to its underlying signed byte representation.
    /// </summary>
    /// <param name="trit">A Trit value to convert.</param>
    /// <returns>The signed byte value (-1, 0, or 1) representing the Trit.</returns>
    public static implicit operator sbyte(Trit trit) => trit.Value;

    /// <summary>
    /// Converts the specified signed byte value to a Trit value.
    /// </summary>
    /// <param name="value">The signed byte value to convert. Must be -1, 0, or 1.</param>
    /// <returns>A Trit value corresponding to the input.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not -1, 0, or 1.</exception>
    public static explicit operator Trit(sbyte value)
    {
        if (value is < -1 or > 1) throw new ArgumentOutOfRangeException(nameof(value), "Trit value must be -1, 0, or 1.");
        return new(value);
    }

    /// <summary>
    /// Converts the Trit value to its underlying integer representation.
    /// </summary>
    /// <param name="trit">A Trit value to convert.</param>
    /// <returns>The integer value (-1, 0, or 1) representing the Trit.</returns>
    public static implicit operator int(Trit trit) => trit.Value;

    /// <summary>
    /// Converts the specified integer value to a Trit value.
    /// </summary>
    /// <param name="value">The integer value to convert. Must be -1, 0, or 1.</param>
    /// <returns>A Trit value corresponding to the input.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not -1, 0, or 1.</exception>
    public static explicit operator Trit(int value)
    {
        if (value is < -1 or > 1) throw new ArgumentOutOfRangeException(nameof(value), "Trit value must be -1, 0, or 1.");
        return new((sbyte)value);
    }

    /// <summary>
    /// Represents the Zero (0) value of the Trit.
    /// </summary>
    public static readonly Trit Zero = new(ZeroValue);

    /// <summary>
    /// Represents the Positive (1) value of the Trit.
    /// </summary>
    public static readonly Trit Positive = new(PositiveValue);

    /// <summary>
    /// Represents the Negative (-1) value of the Trit.
    /// </summary>
    public static readonly Trit Negative = new(NegativeValue);

    /// <summary>
    /// Returns a string representation of the current Trit value.
    /// </summary>
    /// <returns>"Positive" for 1, "Zero" for 0, and "Negative" for -1.</returns>
    public override string ToString() => Value switch
    {
        PositiveValue => "Positive",
        NegativeValue => "Negative",
        _ => "Zero"
    };

    /// <summary>
    /// Returns true if the value is Positive (1), false otherwise.
    /// </summary>
    /// <param name="trit">The Trit value to check.</param>
    /// <returns>True if the value is Positive (1), false otherwise.</returns>
    public static bool operator true(Trit trit) => trit.Value == PositiveValue;

    /// <summary>
    /// Returns true if the value is Negative (-1), false otherwise.
    /// </summary>
    /// <param name="trit">The Trit value to check.</param>
    /// <returns>True if the value is Negative (-1), false otherwise.</returns>
    public static bool operator false(Trit trit) => trit.Value == NegativeValue;

    /// <summary>
    /// Performs a logical NOT operation on a Trit value.
    /// </summary>
    /// <param name="trit">The Trit value to negate.</param>
    /// <returns>The logical negation: Positive becomes Negative, Negative becomes Positive, Zero remains Zero.</returns>
    public static Trit operator !(Trit trit) => trit.Value == ZeroValue ? Zero : new((sbyte)-trit.Value);

    /// <summary>
    /// Determines if two Trit values are equal.
    /// </summary>
    /// <param name="left">The first Trit to compare.</param>
    /// <param name="right">The second Trit to compare.</param>
    /// <returns>True if both Trits have the same value, false otherwise.</returns>
    public static bool operator ==(Trit left, Trit right) => left.Value == right.Value;

    /// <summary>
    /// Determines if two Trit values are not equal.
    /// </summary>
    /// <param name="left">The first Trit to compare.</param>
    /// <param name="right">The second Trit to compare.</param>
    /// <returns>True if the Trits have different values, false otherwise.</returns>
    public static bool operator !=(Trit left, Trit right) => left.Value != right.Value;

    public bool Equals(Trit other) => Value == other.Value;

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified object.
    /// </summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>True if obj is a Trit and has the same value as this instance; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is Trit trit && Value == trit.Value;

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => Value.GetHashCode();

    /// <summary>
    /// Creates an UnsafeTritOperator to enable custom operations using the pipe syntax.
    /// </summary>
    /// <param name="left">The left Trit operand that will be stored in the operator.</param>
    /// <param name="operation">A function pointer to a method that accepts two Trit parameters and returns a Trit result.</param>
    /// <returns>An UnsafeTritOperator that combines the left Trit with the operation function pointer.</returns>
    /// <remarks>
    /// This enables syntax like: <c>trit1 | &amp;Operation.Apply | trit2</c> to perform custom operations.
    /// Requires unsafe code context due to the use of function pointers.
    /// </remarks>
    public static unsafe UnsafeTritOperator operator |(Trit left, delegate*<Trit, Trit, Trit> operation) => new(left, operation);

    /// <summary>
    /// Creates a TritOperator to enable custom operations using the pipe syntax.
    /// </summary>
    /// <param name="left">The left Trit operand that will be stored in the operator.</param>
    /// <param name="operation">A delegate function that accepts two Trit parameters and returns a Trit result.</param>
    /// <returns>A TritOperator that combines the left Trit with the operation delegate.</returns>
    /// <remarks>
    /// This enables syntax like: <c>trit1 | Operation.ApplyFunc | trit2</c> to perform custom operations.
    /// Provides a safe alternative to the function pointer approach that doesn't require unsafe code.
    /// </remarks>
    public static BinaryMethodTritOperator operator |(Trit left, Func<Trit, Trit, Trit> operation) => new(left, operation);

    /// <summary>
    /// Creates a ArrayLookupTritOperator to enable custom operations using pre-computed lookup tables.
    /// </summary>
    /// <param name="left">The left Trit operand that will be stored in the operator.</param>
    /// <param name="lookupTable">A 3x3 lookup table where indices [leftValue+1, rightValue+1] map to result Trit values.</param>
    /// <returns>A TableBasedTritOperator that combines the left Trit with the lookup table.</returns>
    /// <remarks>
    /// This enables syntax like: <c>trit1 | operationTable | trit2</c> to perform operations using table lookups.
    /// Optimizes performance for custom operations by using direct table access instead of function calls.
    /// The table must be indexed from 0-2, with 0 corresponding to -1, 1 to 0, and 2 to +1 Trit values.
    /// </remarks>
    public static BinaryLookupTritOperator operator |(Trit left, Trit[,]lookupTable) => new(left, lookupTable);

    /// <summary>
    /// Creates a LookupTritOperator to enable custom operations using pre-computed lookup tables.
    /// </summary>
    /// <param name="left">The left Trit operand that will be stored in the operator.</param>
    /// <param name="operation">A 3x3 lookup table where indices [leftValue+1, rightValue+1] map to result Trit values.</param>
    /// <returns>A TableBasedTritOperator that combines the left Trit with the lookup table.</returns>
    /// <remarks>
    /// This enables syntax like: <c>trit1 | operationTable | trit2</c> to perform operations using table lookups.
    /// Optimizes performance for custom operations by using direct table access instead of function calls.
    /// The table must be indexed from 0-2, with 0 corresponding to -1, 1 to 0, and 2 to +1 Trit values.
    /// </remarks>
    public static BinaryLookupTritOperator operator |(Trit left, BinaryTritOperator operation) => new(left, operation);
    
    /// <summary>
    /// Performs a unary operation on a Trit value using a lookup table. 
    /// </summary>
    /// <param name="left">The operand</param>
    /// <param name="operation">The operation, represented as a 3 trit array</param>
    public static Trit operator |(Trit left, Trit[] operation) => Unary.Apply(left, operation);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left">The operand</param>
    /// <param name="operation">The operation, represented as a function</param>
    public static Trit operator |(Trit left, Func<Trit, Trit> operation) => operation(left);

    /// <summary>
    /// Performs a logical XOR operation between a Boolean and a Trit value.
    /// </summary>
    /// <param name="left">The Boolean operand.</param>
    /// <param name="right">The Trit operand.</param>
    /// <returns>A Trit value representing the logical XOR operation.</returns>
    public static Trit operator ^(bool left, Trit right) => left ? !right : right;

    /// <summary>
    /// Performs a logical XOR operation between a Trit value and a Boolean.
    /// </summary>
    /// <param name="left">The Trit operand.</param>
    /// <param name="right">The Boolean operand.</param>
    /// <returns>A Trit value representing the logical XOR operation.</returns>
    public static Trit operator ^(Trit left, bool right) => right ? !left : left;
    
    public static Trit[] AllValues => [Negative, Zero, Positive];
}


