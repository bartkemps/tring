namespace Ternary3.Operators;

public partial struct UnaryTritOperator
{
    /// <summary>
    /// Negative value.
    /// Negative for negative, zero and positive.
    /// [T, T, T]
    /// </summary>
    public static readonly UnaryTritOperator Negative = new(0);


    /// <summary>
    /// Decrement.
    /// One less for every value greater than negative.
    /// [T, T, 0]
    /// </summary>
    public static readonly UnaryTritOperator Decrement = new(1);


    /// <summary>
    /// Is the value positive?
    /// Positive for positive, negative otherwise.
    /// [T, T, 1]
    /// </summary>
    public static readonly UnaryTritOperator IsPositive = new(2);

    /// <summary>
    /// Negate the Absolute Value.
    /// Zero for zero, negative otherwise.
    /// [T, 0, T]
    /// </summary>
    public static readonly UnaryTritOperator NegateAbsoluteValue = new(3);


    /// <summary>
    /// Ceiling Zero.
    /// Negative for negative, zero otherwise.
    /// [T, 0, 0]
    /// </summary>
    public static readonly UnaryTritOperator Ceil = new(4);

    /// <summary>
    /// Identity.
    /// Negative for negative, zero for zero, positive for positive.
    /// [T, 0, 1]
    /// </summary>
    public static readonly UnaryTritOperator Identity = new(5);


    /// <summary>
    /// Is the value zero?
    /// Positive for zero, negative otherwise.
    /// [T, 1, T]
    /// </summary>
    public static readonly UnaryTritOperator IsZero = new(6);


    /// <summary>
    /// Keep the value unchanged if it is negative.
    /// Zero for positive and vice versa.
    /// [T, 1, 0]
    /// </summary>
    public static readonly UnaryTritOperator KeepNegative = new(7);


    /// <summary>
    /// Is the value not negative?
    /// Negative for negative, positive otherwise.
    /// [T, 1, 1]
    /// </summary>
    public static readonly UnaryTritOperator IsNotNegative = new(8);


    /// <summary>
    /// Ceiling Zero of Is Negative.
    /// Zero for negative, negative otherwise.
    /// [0, T, T]
    /// </summary>
    public static readonly UnaryTritOperator CeilIsNegative = new(9);


    /// <summary>
    /// Is Not Zero, Ceiling Zero.
    /// Zero for positive, negative otherwise.
    /// [0, T, 0]
    /// </summary>
    public static readonly UnaryTritOperator CeilIsNotZero = new(10);


    /// <summary>
    /// Keep Positive.
    /// Positive for positive, negative otherwise.
    /// [0, T, 1]
    /// </summary>
    public static readonly UnaryTritOperator KeepPositive = new(11);


    /// <summary>
    /// Is Not Positive, Ceiling Zero.
    /// Negative for positive, zero otherwise.
    /// [0, 0, T]
    /// </summary>
    public static readonly UnaryTritOperator CeilIsNotPositive = new(12);


    /// <summary>
    /// Zero.
    /// Always zero.
    /// [0, 0, 0]
    /// </summary>
    public static readonly UnaryTritOperator Zero = new(13);


    /// <summary>
    /// Floor.
    /// Positive for positive, zero otherwise.
    /// [0, 0, 1]
    /// </summary>
    public static readonly UnaryTritOperator Floor = new(14);
    
    /// <summary>
    /// Alias for <see cref="Floor"/>.
    /// Positive for positive, zero otherwise.
    /// [0, 0, 1]
    /// </summary>
    public static readonly UnaryTritOperator FloorIsPositive = new(14);


    /// <summary>
    /// Cyclic Increment.
    /// Positive for positive, negative otherwise.
    /// [0, 1, T]
    /// </summary>
    public static readonly UnaryTritOperator CyclicIncrement = new(15);


    /// <summary>
    /// Floor Is Zero.
    /// Zero for zero, positive otherwise.
    /// [0, 1, 0]
    /// </summary>
    public static readonly UnaryTritOperator FloorIsZero = new(16);


    /// <summary>
    /// Increment.
    /// Zero for negative, positive otherwise.
    /// [0, 1, 1]
    /// </summary>
    public static readonly UnaryTritOperator Increment = new(17);


    /// <summary>
    /// Is the value negative?
    /// Negative for negative, positive otherwise.
    /// [1, T, T]
    /// </summary>
    public static readonly UnaryTritOperator IsNegative = new(18);


    /// <summary>
    /// Cyclic Decrement.
    /// Negative for positive, zero for zero, positive for negative.
    /// [1, T, 0]
    /// </summary>
    public static readonly UnaryTritOperator CyclicDecrement = new(19);


    /// <summary>
    /// Is Not Zero.
    /// Positive for negative and positive, negative for zero.
    /// [1, T, 1]
    /// </summary>
    public static readonly UnaryTritOperator IsNotZero = new(20);


    /// <summary>
    /// Negate.
    /// Positive for negative, negative for positive.
    /// [1, 0, T]
    /// </summary>
    public static readonly UnaryTritOperator Negate = new(21);


    /// <summary>
    /// Floor Is Negative.
    /// Zero for zero, positive otherwise.
    /// [1, 0, 0]
    /// </summary>
    public static readonly UnaryTritOperator FloorIsNegative = new(22);


    /// <summary>
    /// Absolute Value.
    /// Always positive.
    /// [1, 0, 1]
    /// </summary>
    public static readonly UnaryTritOperator AbsoluteValue = new(23);


    /// <summary>
    /// Is Not Positive.
    /// Positive for negative and zero, negative for positive.
    /// [1, 1, T]
    /// </summary>
    public static readonly UnaryTritOperator IsNotPositive = new(24);


    /// <summary>
    /// Floor Is Not Positive.
    /// Zero for positive, positive otherwise.
    /// [1, 1, 0]
    /// </summary>
    public static readonly UnaryTritOperator FloorIsNotPositive = new(25);


    /// <summary>
    /// Positive.
    /// Always positive.
    /// [1, 1, 1]
    /// </summary>
    public static readonly UnaryTritOperator Positive = new(26);
}