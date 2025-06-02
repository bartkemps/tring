namespace Tring.Operators;

using Numbers;

public partial class UnaryOperation
{
    /// <summary>
    /// Apply a unary operation to a Trit value.
    /// </summary>
    /// <param name="target">The trit value</param>
    /// <param name="table">The possible values</param>
    /// <exception cref="ArgumentException"></exception>
    public static Trit Apply(Trit target, Trit[] table)
    {
        if (table.Length != 3) throw new ArgumentException("Table must have exactly 3 elements.", nameof(table));
        return table[target.Value + 1];
    }
    
    /// <summary>
    /// Negative value.
    /// Negative for negative, zero and positive.
    /// [T, T, T]
    /// </summary>
    public static Trit Negative(Trit trit) => Trit.Negative;


    /// <summary>
    /// Decrement.
    /// One less for every value greater than negative.
    /// [T, T, 0]
    /// </summary>
    public static Trit Decrement(Trit trit) => trit.Value == 1 ? Trit.Zero : Trit.Negative;


    /// <summary>
    /// Is the value positive?
    /// Positive for positive, negative otherwise.
    /// [T, T, 1]
    /// </summary>
    public static Trit IsPositive(Trit trit) => trit.Value == 1 ? Trit.Positive : Trit.Negative;

    /// <summary>
    /// Negate the Absolute Value.
    /// Zero for zero, negative otherwise.
    /// [T, 0, T]
    /// </summary>
    public static Trit NegateAbsoluteValue(Trit trit) => trit.Value == 0 ? Trit.Zero : Trit.Negative;


    /// <summary>
    /// Ceiling Zero.
    /// Negative for negative, zero otherwise.
    /// [T, 0, 0]
    /// </summary>
    public static Trit Ceil(Trit trit) => trit.Value == -1 ? trit : Trit.Zero;


    /// <summary>
    /// Identity.
    /// Negative for negative, zero for zero, positive for positive.
    /// [T, 0, 1]
    /// </summary>
    public static Trit Identity(Trit trit) => trit;


    /// <summary>
    /// Is the value zero?
    /// Positive for zero, negative otherwise.
    /// [T, 1, T]
    /// </summary>
    public static Trit IsZero(Trit trit) => trit.Value == 0 ? Trit.Positive : Trit.Negative;


    /// <summary>
    /// Keep the value unchanged if it is negative.
    /// Zero for positive and vice versa.
    /// [T, 1, 0]
    /// </summary>
    public static Trit KeepNegative(Trit trit) => trit.Value == -1 ? trit : new(trit.Value ^ 1);


    /// <summary>
    /// Is the value not negative?
    /// Negative for negative, positive otherwise.
    /// [T, 1, 1]
    /// </summary>
    public static Trit IsNotNegative(Trit trit) => trit.Value == -1 ? trit : Trit.Positive;


    /// <summary>
    /// Ceiling Zero of Is Negative.
    /// Zero for negative, negative otherwise.
    /// [0, T, T]
    /// </summary>
    public static Trit CeilIsNegative(Trit trit) => trit.Value == -1 ? Trit.Zero : Trit.Negative;


    /// <summary>
    /// Is Not Zero, Ceiling Zero.
    /// Zero for positive, negative otherwise.
    /// [0, T, 0]
    /// </summary>
    public static Trit CeilIsNotZero(Trit trit) => trit.Value != 0 ? Trit.Zero : Trit.Negative;


    /// <summary>
    /// Keep Positive.
    /// Positive for positive, negative otherwise.
    /// [0, T, 1]
    /// </summary>
    public static Trit KeepPositive(Trit trit) => trit.Value == 1 ? Trit.Positive : new(-1 - trit.Value);


    /// <summary>
    /// Is Not Positive, Ceiling Zero.
    /// Negative for positive, zero otherwise.
    /// [0, 0, T]
    /// </summary>
    public static Trit CeilIsNotPositive(Trit trit) => trit.Value == 1 ? Trit.Negative : Trit.Zero;


    /// <summary>
    /// Zero.
    /// Always zero.
    /// [0, 0, 0]
    /// </summary>
    public static Trit Zero(Trit _) => Trit.Zero;


    /// <summary>
    /// Floor.
    /// Positive for positive, zero otherwise.
    /// [0, 0, 1]
    /// </summary>
    public static Trit Floor(Trit trit) => trit.Value == 1 ? Trit.Positive : Trit.Zero;


    /// <summary>
    /// Cyclic Increment.
    /// Positive for positive, negative otherwise.
    /// [0, 1, T]
    /// </summary>
    public static Trit CyclicIncrement(Trit trit) => trit.Value == 1 ? Trit.Negative : new(trit.Value + 1);


    /// <summary>
    /// Floor Is Zero.
    /// Zero for zero, positive otherwise.
    /// [0, 1, 0]
    /// </summary>
    public static Trit FloorIsZero(Trit trit) => trit.Value == 0 ? Trit.Positive : Trit.Zero;


    /// <summary>
    /// Increment.
    /// Zero for negative, positive otherwise.
    /// [0, 1, 1]
    /// </summary>
    public static Trit Increment(Trit trit) => trit.Value == -1 ? Trit.Zero : Trit.Positive;


    /// <summary>
    /// Is the value negative?
    /// Negative for negative, positive otherwise.
    /// [1, T, T]
    /// </summary>
    public static Trit IsNegative(Trit trit) => trit.Value == -1 ? Trit.Positive : Trit.Negative;


    /// <summary>
    /// Cyclic Decrement.
    /// Negative for positive, zero for zero, positive for negative.
    /// [1, T, 0]
    /// </summary>
    public static Trit CyclicDecrement(Trit trit) => trit.Value == -1 ? Trit.Positive : new(trit.Value - 1);


    /// <summary>
    /// Is Not Zero.
    /// Positive for negative and positive, negative for zero.
    /// [1, T, 1]
    /// </summary>
    public static Trit IsNotZero(Trit trit) => trit.Value != 0 ? Trit.Positive : Trit.Negative;


    /// <summary>
    /// Negate.
    /// Positive for negative, negative for positive.
    /// [1, 0, T]
    /// </summary>
    public static Trit Negate(Trit trit) => new(-trit.Value);


    /// <summary>
    /// Floor Is Negative.
    /// Zero for zero, positive otherwise.
    /// [1, 0, 0]
    /// </summary>
    public static Trit FloorIsNegative(Trit trit) => trit.Value == -1 ? Trit.Positive : Trit.Zero;


    /// <summary>
    /// Absolute Value.
    /// Always positive.
    /// [1, 0, 1]
    /// </summary>
    public static Trit AbsoluteValue(Trit trit) => trit.Value == -1 ? Trit.Positive : trit;


    /// <summary>
    /// Is Not Positive.
    /// Positive for negative and zero, negative for positive.
    /// [1, 1, T]
    /// </summary>
    public static Trit IsNotPositive(Trit trit) => trit.Value == 1 ? Trit.Negative : Trit.Positive;


    /// <summary>
    /// Floor Is Not Positive.
    /// Zero for positive, positive otherwise.
    /// [1, 1, 0]
    /// </summary>
    public static Trit FloorIsNotPositive(Trit trit) => trit.Value == 1 ? Trit.Zero : Trit.Positive;


    /// <summary>
    /// Positive.
    /// Always positive.
    /// [1, 1, 1]
    /// </summary>
    public static Trit Positive(Trit _) => Trit.Positive;
}