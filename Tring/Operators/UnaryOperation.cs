// ReSharper disable InconsistentNaming

namespace Tring.Operators;

using Numbers;

public class UnaryOperation
{
    private static readonly Func<uint, uint, (uint, uint)>[] operations32 =
    [
        Negative, Decrement, IsPositive,
        NegateAbsoluteValue, Ceil, Identity,
        IsZero, KeepNegative, IsNotNegative,
        CeilIsNegative, CeilIsNotZero, KeepPositive,
        CeilIsNotPositive, Zero, Floor,
        CyclicIncrement, FloorIsZero, Increment,
        IsNegative, CyclicDecrement, IsNotZero,
        Negate, FloorIsNegative, AbsoluteValue,
        IsNotPositive, FloorIsNotPositive, Positive
    ];

    /// <summary>
    /// Negative value.
    /// Negative for negative, zero and positive.
    /// [T, T, T]
    /// </summary>
    public static Trit Negative(Trit trit) => Trit.Negative;

    private static (uint, uint) Negative(uint negative, uint positive) => (uint.MaxValue, 0u);

    /// <summary>
    /// Decrement.
    /// One less for every value greater than negative.
    /// [T, T, 0]
    /// </summary>
    public static Trit Decrement(Trit trit) => trit.Value == 1 ? Trit.Zero : Trit.Negative;

    private static (uint, uint) Decrement(uint negative, uint positive) => (negative | ~positive, 0u);

    /// <summary>
    /// Is the value positive?
    /// Positive for positive, negative otherwise.
    /// [T, T, 1]
    /// </summary>
    public static Trit IsPositive(Trit trit) => trit.Value == 1 ? Trit.Positive : Trit.Negative;

    private static (uint, uint) IsPositive(uint negative, uint positive) => (negative | ~positive, positive);

    /// <summary>
    /// Negate the Absolute Value.
    /// Zero for zero, negative otherwise.
    /// [T, 0, T]
    /// </summary>
    public static Trit NegateAbsoluteValue(Trit trit) => trit.Value == 0 ? Trit.Zero : Trit.Negative;

    private static (uint, uint) NegateAbsoluteValue(uint negative, uint positive) => (negative | positive, 0u);

    /// <summary>
    /// Ceiling Zero.
    /// Negative for negative, zero otherwise.
    /// [T, 0, 0]
    /// </summary>
    public static Trit Ceil(Trit trit) => trit.Value == -1 ? trit : Trit.Zero;

    private static (uint, uint) Ceil(uint negative, uint positive) => (negative, 0u);

    /// <summary>
    /// Identity.
    /// Negative for negative, zero for zero, positive for positive.
    /// [T, 0, 1]
    /// </summary>
    public static Trit Identity(Trit trit) => trit;

    private static (uint, uint) Identity(uint negative, uint positive) => (negative, positive);

    /// <summary>
    /// Is the value zero?
    /// Positive for zero, negative otherwise.
    /// [T, 1, T]
    /// </summary>
    public static Trit IsZero(Trit trit) => trit.Value == 0 ? Trit.Positive : Trit.Negative;

    private static (uint, uint) IsZero(uint negative, uint positive) => (negative | positive, ~negative & ~positive);

    /// <summary>
    /// Keep the value unchanged if it is negative.
    /// Zero for positive and vice versa.
    /// [T, 1, 0]
    /// </summary>
    public static Trit KeepNegative(Trit trit) => trit.Value == -1 ? trit : new(trit.Value ^ 1);

    private static (uint, uint) KeepNegative(uint negative, uint positive) => (negative, ~negative & ~positive);

    /// <summary>
    /// Is the value not negative?
    /// Negative for negative, positive otherwise.
    /// [T, 1, 1]
    /// </summary>
    public static Trit IsNotNegative(Trit trit) => trit.Value == -1 ? trit : Trit.Positive;

    private static (uint, uint) IsNotNegative(uint negative, uint positive) => (negative, ~negative);

    /// <summary>
    /// Ceiling Zero of Is Negative.
    /// Zero for negative, negative otherwise.
    /// [0, T, T]
    /// </summary>
    public static Trit CeilIsNegative(Trit trit) => trit.Value == -1 ? Trit.Zero : Trit.Negative;

    private static (uint, uint) CeilIsNegative(uint negative, uint positive) => (~negative, 0u);

    /// <summary>
    /// Is Not Zero, Ceiling Zero.
    /// Zero for positive, negative otherwise.
    /// [0, T, 0]
    /// </summary>
    public static Trit CeilIsNotZero(Trit trit) => trit.Value != 0 ? Trit.Zero : Trit.Negative;

    private static (uint, uint) CeilIsNotZero(uint negative, uint positive) => (~positive & ~negative, 0u);

    /// <summary>
    /// Keep Positive.
    /// Positive for positive, negative otherwise.
    /// [0, T, 1]
    /// </summary>
    public static Trit KeepPositive(Trit trit) => trit.Value == 1 ? Trit.Positive : new (-1-trit.Value);

    private static (uint, uint) KeepPositive(uint negative, uint positive) => (~positive & ~negative, positive);

    /// <summary>
    /// Is Not Positive, Ceiling Zero.
    /// Negative for positive, zero otherwise.
    /// [0, 0, T]
    /// </summary>
    public static Trit CeilIsNotPositive(Trit trit) => trit.Value == 1 ? Trit.Negative : Trit.Zero;

    private static (uint, uint) CeilIsNotPositive(uint negative, uint positive) => (positive, 0u);

    /// <summary>
    /// Zero.
    /// Always zero.
    /// [0, 0, 0]
    /// </summary>
    public static Trit Zero(Trit _) => Trit.Zero;

    private static (uint, uint) Zero(uint negative, uint positive) => (0u, 0u);

    /// <summary>
    /// Floor.
    /// Positive for positive, zero otherwise.
    /// [0, 0, 1]
    /// </summary>
    public static Trit Floor(Trit trit) => trit.Value == 1 ? Trit.Positive : Trit.Zero;

    private static (uint, uint) Floor(uint negative, uint positive) => (0u, positive & ~negative);

    /// <summary>
    /// Cyclic Increment.
    /// Positive for positive, negative otherwise.
    /// [0, 1, T]
    /// </summary>
    public static Trit CyclicIncrement(Trit trit) => trit.Value == 1 ? Trit.Negative : new (trit.Value + 1);

    private static (uint, uint) CyclicIncrement(uint negative, uint positive) => (positive, ~negative & ~positive);

    /// <summary>
    /// Floor Is Zero.
    /// Zero for zero, positive otherwise.
    /// [0, 1, 0]
    /// </summary>
    public static Trit FloorIsZero(Trit trit) => trit.Value == 0 ? Trit.Positive : Trit.Zero;

    private static (uint, uint) FloorIsZero(uint negative, uint positive) => (0u, ~positive & ~negative);

    /// <summary>
    /// Increment.
    /// Zero for negative, positive otherwise.
    /// [0, 1, 1]
    /// </summary>
    public static Trit Increment(Trit trit) => trit.Value == -1 ? Trit.Zero : Trit.Positive;

    private static (uint, uint) Increment(uint negative, uint positive) => (0u, positive | ~negative);

    /// <summary>
    /// Is the value negative?
    /// Negative for negative, positive otherwise.
    /// [1, T, T]
    /// </summary>
    public static Trit IsNegative(Trit trit) => trit.Value == -1 ? Trit.Positive : Trit.Negative;

    private static (uint, uint) IsNegative(uint negative, uint positive) => (~negative, negative);

    /// <summary>
    /// Cyclic Decrement.
    /// Negative for positive, zero for zero, positive for negative.
    /// [1, T, 0]
    /// </summary>
    public static Trit CyclicDecrement(Trit trit) => trit.Value == -1 ? Trit.Positive : new(trit.Value - 1);

    private static (uint, uint) CyclicDecrement(uint negative, uint positive) => (~positive & ~negative, negative);

    /// <summary>
    /// Is Not Zero.
    /// Positive for negative and positive, negative for zero.
    /// [1, T, 1]
    /// </summary>
    public static Trit IsNotZero(Trit trit) => trit.Value != 0 ? Trit.Positive : Trit.Negative;

    private static (uint, uint) IsNotZero(uint negative, uint positive) => (~negative & ~positive, negative | positive);

    /// <summary>
    /// Negate.
    /// Positive for negative, negative for positive.
    /// [1, 0, T]
    /// </summary>
    public static Trit Negate(Trit trit) => new (-trit.Value);

    private static (uint, uint) Negate(uint negative, uint positive) => (positive, negative);

    /// <summary>
    /// Floor Is Negative.
    /// Zero for zero, positive otherwise.
    /// [1, 0, 0]
    /// </summary>
    public static Trit FloorIsNegative(Trit trit) => trit.Value == -1 ? Trit.Positive : Trit.Zero;

    private static (uint, uint) FloorIsNegative(uint negative, uint positive) => (0u, negative);

    /// <summary>
    /// Absolute Value.
    /// Always positive.
    /// [1, 0, 1]
    /// </summary>
    public static Trit AbsoluteValue(Trit trit) => trit.Value == -1 ? Trit.Positive : trit;

    private static (uint, uint) AbsoluteValue(uint negative, uint positive) => (0u, positive | negative);

    /// <summary>
    /// Is Not Positive.
    /// Positive for negative and zero, negative for positive.
    /// [1, 1, T]
    /// </summary>
    public static Trit IsNotPositive(Trit trit) => trit.Value == 1 ? Trit.Negative : Trit.Positive;

    private static (uint, uint) IsNotPositive(uint negative, uint positive) => (positive, ~positive);

    /// <summary>
    /// Floor Is Not Positive.
    /// Zero for positive, positive otherwise.
    /// [1, 1, 0]
    /// </summary>
    public static Trit FloorIsNotPositive(Trit trit) => trit.Value == 1 ? Trit.Zero : Trit.Positive;

    private static (uint, uint) FloorIsNotPositive(uint negative, uint positive) => (0u, ~positive);

    /// <summary>
    /// Positive.
    /// Always positive.
    /// [1, 1, 1]
    /// </summary>
    public static Trit Positive(Trit _) => Trit.Positive;

    private static (uint, uint) Positive(uint negative, uint positive) => (0u, uint.MaxValue);

    public static (uint, uint) Apply(uint negative, uint positive, Func<uint, uint, (uint, uint)> operation) => operation(negative, positive);

    public static (uint, uint) Apply(uint negative, uint positive, Func<Trit, Trit> operation)
    {
        return operations32[13 + operation(Trit.Positive).Value + 3 * operation(Trit.Zero).Value + 9 * operation(Trit.Negative).Value](negative, positive);
    }

    public static (uint, uint) Apply(uint negative, uint positive, Trit[] table)
    {
        if (table.Length != 3) throw new ArgumentException("Table must have exactly 3 elements.", nameof(table));
        return operations32[13 + table[2].Value + 3 * table[1].Value + 9 * table[0].Value](negative, positive);
    }

    public static Trit Apply(Trit target, Trit[] table)
    {
        if (table.Length != 3) throw new ArgumentException("Table must have exactly 3 elements.", nameof(table));
        return table[target.Value + 1];
    }
}