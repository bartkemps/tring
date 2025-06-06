namespace Ternary3.Operators;

using Numbers;

public static class UnaryLookup
{
    /// <summary>
    /// Negative value.
    /// Negative for negative, zero and positive.
    /// [T, T, T]
    /// </summary>
    public static readonly Trit[] Negative = [Trit.Negative, Trit.Negative, Trit.Negative];

    /// <summary>
    /// Decrement.
    /// One less for every value greater than negative.
    /// [T, T, 0]
    /// </summary>
    public static readonly Trit[] Decrement = [Trit.Negative, Trit.Negative, Trit.Zero];

    /// <summary>
    /// Is the value positive?
    /// Positive for positive, negative otherwise.
    /// [T, T, 1]
    /// </summary>
    public static readonly Trit[] IsPositive = [Trit.Negative, Trit.Negative, Trit.Positive];

    /// <summary>
    /// Negate the Absolute Value
    /// Zero for zero, negative otherwise.
    /// [T, 0, T]
    /// </summary>
    public static readonly Trit[] NegateAbsoluteValue = [Trit.Negative, Trit.Zero, Trit.Negative];

    /// <summary>
    /// Ceiling Zero.
    /// Negative for negative, zero otherwise.
    /// [T, 0, 0]
    /// </summary>
    public static readonly Trit[] Ceil = [Trit.Negative, Trit.Zero, Trit.Zero];

    /// <summary>
    /// Identity.
    /// Negative for negative, zero for zero, positive for positive.
    /// [T, 0, 1]
    /// </summary>
    public static readonly Trit[] Identity = [Trit.Negative, Trit.Zero, Trit.Positive];

    /// <summary>
    /// Is the value zero?
    /// Positive for zero, negative otherwise.
    /// [T, 1, T]
    /// </summary>
    public static readonly Trit[] IsZero = [Trit.Negative, Trit.Positive, Trit.Negative];

    /// <summary>
    ///  Keep the value unchanged if it is negative.
    /// Zero for positive and vice versa.
    /// [T, 1, 0]
    /// </summary>
    public static readonly Trit[] KeepNegative = [Trit.Negative, Trit.Positive, Trit.Zero];

    /// <summary>
    /// Is the value not negative?
    /// Negative for negative, positive otherwise.
    /// [T, 1, 1]
    /// </summary>
    public static readonly Trit[] IsNotNegative = [Trit.Negative, Trit.Positive, Trit.Positive];

    /// <summary>
    /// Ceiling Zero of Is Negative.
    /// Zero for negative, negative otherwise.
    /// [0, T, T]
    /// </summary>
    public static readonly Trit[] CeilIsNegative = [Trit.Zero, Trit.Negative, Trit.Negative];

    /// <summary>
    /// Is Not Zero, Ceiling Zero.
    /// Negative for zero, zero otherwise.
    /// [0, T, 0]
    /// </summary>
    public static readonly Trit[] CeilIsNotZero = [Trit.Zero, Trit.Negative, Trit.Zero];

    /// <summary>
    /// Keep the value unchanged if it is positive.
    /// Negative for zero and vice versa.
    /// [0, T, 1]
    /// </summary>
    public static readonly Trit[] KeepPositive = [Trit.Zero, Trit.Negative, Trit.Positive];

    /// <summary>
    /// Is Not Positive, Ceiling Zero.
    /// Negative for positive, zero otherwise.
    /// [0, 0, T]
    /// </summary>
    public static readonly Trit[] CeilIsNotPositive = [Trit.Zero, Trit.Zero, Trit.Negative];

    /// <summary>
    /// Zero  value.
    /// Zero for negative, zero and positive.
    /// [0, 0, 0]
    /// </summary>
    public static readonly Trit[] Zero = [Trit.Zero, Trit.Zero, Trit.Zero];

    /// <summary>
    /// Floor zero.
    /// Positive for positive, zero otherwise.
    /// [0, 0, 1]
    /// </summary>
    public static readonly Trit[] Floor = [Trit.Zero, Trit.Zero, Trit.Positive];

    /// <summary>
    /// Cyclic increment.
    /// One more with overflow (positive becomes negative).
    /// [0, 1, T]
    /// </summary>
    public static readonly Trit[] CyclicIncrement = [Trit.Zero, Trit.Positive, Trit.Negative];

    /// <summary>
    /// Floor Zero of  Is Zero.
    /// Positive for zero, zero otherwise.
    /// [0, 1, 0]
    /// </summary>
    public static readonly Trit[] FloorIsZero = [Trit.Zero, Trit.Positive, Trit.Zero];

    /// <summary>
    /// Increment.
    /// Zero for negative, positive otherwise.
    /// [0, 1, 1]
    /// </summary>
    public static readonly Trit[] Increment = [Trit.Zero, Trit.Positive, Trit.Positive];

    /// <summary>
    /// Is the value negative?
    /// Positive for negative, negative otherwise.
    /// [1, T, T]
    /// </summary>
    public static readonly Trit[] IsNegative = [Trit.Positive, Trit.Negative, Trit.Negative];

    /// <summary>
    /// Cyclic Decrement.
    /// One less with overflow (negative becomes positive).
    /// [1, T, 0]
    /// </summary>
    public static readonly Trit[] CyclicDecrement = [Trit.Positive, Trit.Negative, Trit.Zero];

    /// <summary>
    /// Is the value not zero?
    /// Negative for zero, positive otherwise.
    /// [1, T, 1]
    /// </summary>
    public static readonly Trit[] IsNotZero = [Trit.Positive, Trit.Negative, Trit.Positive];

    /// <summary>
    /// Negate. Keep the value unchanged if it is zero.
    /// Positive for negative, negative for positive.
    /// [1, 0, T]
    /// </summary>
    public static readonly Trit[] Negate = [Trit.Positive, Trit.Zero, Trit.Negative];

    /// <summary>
    /// Floor Zero of  Is Negative.
    /// Positive for negative, zero otherwise.
    /// [1, 0, 0]
    /// </summary>
    public static readonly Trit[] FloorIsNegative = [Trit.Positive, Trit.Zero, Trit.Zero];

    /// <summary>
    /// Absolute value.
    /// Zero for zero, positive otherwise.
    /// [1, 0, 1]
    /// </summary>
    public static readonly Trit[] AbsoluteValue = [Trit.Positive, Trit.Zero, Trit.Positive];

    /// <summary>
    /// Is the value not  positive?
    /// Negative for positive, positive otherwise.
    /// [1, 1, T]
    /// </summary>
    public static readonly Trit[] IsNotPositive = [Trit.Positive, Trit.Positive, Trit.Negative];

    /// <summary>
    /// Floor Zero of  Is Not Positive.
    /// Zero for positive, positive otherwise.
    /// [1, 1, 0]
    /// </summary>
    public static readonly Trit[] FloorIsNotPositive = [Trit.Positive, Trit.Positive, Trit.Zero];

    /// <summary>
    /// Positive value.
    /// Positive for negative, zero and positive.
    /// [1, 1, 1]
    /// </summary>
    public static readonly Trit[] Positive = [Trit.Positive, Trit.Positive, Trit.Positive];
}

