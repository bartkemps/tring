namespace Tring.Numbers;

/// <summary>
/// Represents a trinary (three-valued) logical value that can be Negative (-1), Zero (0), or Positive (1).
/// </summary>
public struct Trit
{
    private readonly sbyte value;

    private Trit(sbyte value)
    {
        this.value = value;
    }

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
    public static implicit operator bool?(Trit trit) => trit.value switch
    {
        1 => true, // Positive
        -1 => false, // Negative
        _ => null, // Zero
    };

    /// <summary>
    /// Converts the Trit value to its underlying signed byte representation.
    /// </summary>
    /// <param name="trit">A Trit value to convert.</param>
    /// <returns>The signed byte value (-1, 0, or 1) representing the Trit.</returns>
    public static implicit operator sbyte(Trit trit) => trit.value;

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
    public static implicit operator int(Trit trit) => trit.value;

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
    public static readonly Trit Zero = new(0);

    /// <summary>
    /// Represents the Positive (1) value of the Trit.
    /// </summary>
    public static readonly Trit Positive = new(1);

    /// <summary>
    /// Represents the Negative (-1) value of the Trit.
    /// </summary>
    public static readonly Trit Negative = new(-1);

    /// <summary>
    /// Returns a string representation of the current Trit value.
    /// </summary>
    /// <returns>"Positive" for 1, "Zero" for 0, and "Negative" for -1.</returns>
    public override string ToString() => value switch
    {
        1 => "Positive",
        -1 => "Negative",
        _ => "Zero"
    };

    /// <summary>
    /// Returns true if the value is Positive (1), false otherwise.
    /// </summary>
    /// <param name="trit">The Trit value to check.</param>
    /// <returns>True if the value is Positive (1), false otherwise.</returns>
    public static bool operator true(Trit trit) => trit.value == 1;

    /// <summary>
    /// Returns true if the value is Negative (-1), false otherwise.
    /// </summary>
    /// <param name="trit">The Trit value to check.</param>
    /// <returns>True if the value is Negative (-1), false otherwise.</returns>
    public static bool operator false(Trit trit) => trit.value == -1;

    /// <summary>
    /// Performs a logical NOT operation on a Trit value.
    /// </summary>
    /// <param name="trit">The Trit value to negate.</param>
    /// <returns>The logical negation: Positive becomes Negative, Negative becomes Positive, Zero remains Zero.</returns>
    public static Trit operator !(Trit trit) => trit.value == 0 ? Zero : new Trit((sbyte)-trit.value);

    /// <summary>
    /// Determines if two Trit values are equal.
    /// </summary>
    /// <param name="left">The first Trit to compare.</param>
    /// <param name="right">The second Trit to compare.</param>
    /// <returns>True if both Trits have the same value, false otherwise.</returns>
    public static bool operator ==(Trit left, Trit right) => left.value == right.value;

    /// <summary>
    /// Determines if two Trit values are not equal.
    /// </summary>
    /// <param name="left">The first Trit to compare.</param>
    /// <param name="right">The second Trit to compare.</param>
    /// <returns>True if the Trits have different values, false otherwise.</returns>
    public static bool operator !=(Trit left, Trit right) => left.value != right.value;

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified object.
    /// </summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>True if obj is a Trit and has the same value as this instance; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is Trit trit && value == trit.value;

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => value.GetHashCode();

    /// <summary>
    /// Performs a logical AND operation on two Trit values.
    /// </summary>
    /// <param name="left">The first Trit operand.</param>
    /// <param name="right">The second Trit operand.</param>
    /// <returns>
    /// Truth table (T = Negative, 0 = Zero, 1 = Positive):
    /// <code>
    ///    &amp;  | T  0  1
    ///    ---+--------
    ///    T  | 1  0  T
    ///    0  | 0  0  0
    ///    1  | T  0  1
    /// </code>
    /// </returns>
    public static Trit operator &(Trit left, Trit right)
    {
        return (Trit)(sbyte)(left.value * right.value);
    }

    /// <summary>
    /// Performs a logical AND operation between a Boolean and a Trit value.
    /// </summary>
    /// <param name="left">The Boolean operand.</param>
    /// <param name="right">The Trit operand.</param>
    /// <returns>A bool value representing the logical AND operation, where Trit.Positive is treated as true.</returns>
    public static bool operator &(bool left, Trit right) => left && right == Positive;

    /// <summary>
    /// Performs a logical AND operation between a Trit value and a Boolean.
    /// </summary>
    /// <param name="left">The Trit operand.</param>
    /// <param name="right">The Boolean operand.</param>
    /// <returns>A bool value representing the logical AND operation, where Trit.Positive is treated as true.</returns>
    public static bool operator &(Trit left, bool right) => left == Positive && right;

    /// <summary>
    /// perform a trinary oparation (add by trit, no modulo)
    /// </summary>
    /// <param name="left">The first Trit operand.</param>
    /// <param name="right">The second Trit operand.</param>
    /// <returns>
    /// Truth table (T = Negative, 0 = Zero, 1 = Positive):
    /// <code>
    ///    |  | T  0  1
    ///    ---+--------
    ///    T  | T  T  0
    ///    0  | T  0  1
    ///    1  | 0  1  1
    /// </code>
    /// </returns>
    public static Trit operator |(Trit left, Trit right) => (left.value + right.value) switch
        {
            < 0 => Negative,
            > 0 => Positive,
            _ => Zero
        };

    /// <summary>
    /// Performs a logical OR operation between a Boolean and a Trit value.
    /// </summary>
    /// <param name="left">The Boolean operand.</param>
    /// <param name="right">The Trit operand.</param>
    /// <returns>A bool value representing the logical OR operation, where Trit.Positive is treated as true.</returns>
    public static bool operator |(bool left, Trit right) => left || right == Positive;

    /// <summary>
    /// Performs a logical OR operation between a Trit value and a Boolean.
    /// </summary>
    /// <param name="left">The Trit operand.</param>
    /// <param name="right">The Boolean operand.</param>
    /// <returns>A bool value representing the logical OR operation, where Trit.Positive is treated as true.</returns>
    public static bool operator |(Trit left, bool right) => left == Positive || right;

    /// <summary>
    /// perform a trinary oparation (add by trit, modulo)
    /// </summary>
    /// <param name="left">The first Trit operand.</param>
    /// <param name="right">The second Trit operand.</param>
    /// <returns>
    /// Truth table (T = Negative, 0 = Zero, 1 = Positive):
    /// <code>
    ///    ^  | T  0  1
    ///    ---+--------
    ///    T  | 1  T  0
    ///    0  | T  0  1
    ///    1  | 0  1  T
    /// </code>
    /// </returns>
    public static Trit operator ^(Trit left, Trit right) => (left.value + right.value) switch
    {
        0 => Zero,
        -2 or 1 => Positive,
        _ => Negative
    };

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
}

