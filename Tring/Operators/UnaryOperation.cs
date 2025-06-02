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
        CeilIsNegative, IsNotZeroCeil, KeepPositive, 
        IsNotPositiveCeil, Zero, Floor, 
        CyclicIncrement, FloorIsZero, Increment,
        IsNegative, CyclicDecrement, IsNotZero, 
        Negate, FloorIsNegative, AbsoluteValue,
        IsNotPositive, FloorIsNotPositive, Positive
    ];

    // TTT [T, T, T]
    public static Trit Negative(Trit trit) => Trit.Negative;
    private static (uint, uint) Negative(uint negative, uint positive) => (uint.MaxValue, 0u);

    // TT0 [T, T, 0]
    public static Trit Decrement(Trit trit) => trit.Value == 1 ? Trit.Zero : Trit.Negative;
    private static (uint, uint) Decrement(uint negative, uint positive) => (negative | ~positive, 0u);

    // TT1 [T, T, 1]
    public static Trit IsPositive(Trit trit) => trit.Value == 1 ? Trit.Positive : Trit.Negative;
    private static (uint, uint) IsPositive(uint negative, uint positive) => (negative | ~positive, positive);

    // T0T [T, 0, T]
    public static Trit NegateAbsoluteValue(Trit trit) => trit.Value == 0 ? Trit.Zero : Trit.Negative;
    private static (uint, uint) NegateAbsoluteValue(uint negative, uint positive) => (negative | positive, 0u);

    // T00 [T, 0, 0]
    public static Trit Ceil(Trit trit) => trit.Value == -1 ? trit: Trit.Zero;
    private static (uint, uint) Ceil(uint negative, uint positive) => (negative, 0u);

    // T01 [T, 0, 1]
    public static Trit Identity(Trit trit) => trit;
    private static (uint, uint) Identity(uint negative, uint positive) => (negative, positive);

    // T1T [T, 1, T]
    public static Trit IsZero(Trit trit) => trit.Value == 0 ? Trit.Positive : Trit.Negative;
    private static (uint, uint) IsZero(uint negative, uint positive) => (negative | positive, ~negative & ~positive);

    // T10 [T, 1, 0]
    public static Trit KeepNegative(Trit trit) => trit.Value == -1 ? trit : new(trit.Value ^ 1);
    private static (uint, uint) KeepNegative(uint negative, uint positive) => (negative, ~negative & ~positive);
    
    // T11 [T, 1, 1]
    public static Trit IsNotNegative(Trit trit) => trit.Value == -1 ? trit : Trit.Positive;
    private static (uint, uint) IsNotNegative(uint negative, uint positive) => (negative, ~negative);
    
    // 0TT [0, T, T]
    public static Trit CeilIsNegative(Trit trit) => trit.Value == -1 ? Trit.Zero : Trit.Negative;
    private static (uint, uint) CeilIsNegative(uint negative, uint positive) => (~negative, 0u);

    // 0T0 [0, T, 0]
    public static Trit IsNotZeroCeil(Trit trit) => trit.Value == -1 ? Trit.Zero : Trit.Negative;
    private static (uint, uint) IsNotZeroCeil(uint negative, uint positive) => (~positive & negative, 0u);

    // 0T1 [0, T, 1]
    public static Trit KeepPositive(Trit trit) => trit.Value == -1 ? Trit.Positive : Trit.Negative;
    private static (uint, uint) KeepPositive(uint negative, uint positive) => (~positive & negative, positive);

    // 00T [0, 0, T]
    public static Trit IsNotPositiveCeil(Trit trit) => trit.Value == 1 ? Trit.Negative : Trit.Zero;
    private static (uint, uint) IsNotPositiveCeil(uint negative, uint positive) => (0u, 0u);

    // 000 [0, 0, 0]
    public static Trit Zero(Trit _) => Trit.Zero;
    private static (uint, uint) Zero(uint negative, uint positive) => (0u, 0u);

    // 001 [0, 0, 1]
    public static Trit Floor(Trit trit) => trit.Value == 1 ? Trit.Positive : Trit.Zero;
    private static (uint, uint) Floor(uint negative, uint positive) => (0u, positive & ~negative);

    // 01T [0, 1, T]
    public static Trit CyclicIncrement(Trit trit) => trit.Value == -1 ? Trit.Negative : Trit.Positive;
    private static (uint, uint) CyclicIncrement(uint negative, uint positive) => (~positive, positive);

    // 010 [0, 1, 0]
    public static Trit FloorIsZero(Trit trit) => trit.Value == 0 ? Trit.Zero : Trit.Positive;
    private static (uint, uint) FloorIsZero(uint negative, uint positive) => (0u, positive);

    // 011 [0, 1, 1]
    public static Trit Increment(Trit _) => Trit.Positive;
    private static (uint, uint) Increment(uint negative, uint positive) => (0u, positive | ~negative);

    // 1TT [1, T, T]
    public static Trit IsNegative(Trit trit) => trit.Value == -1 ? Trit.Negative : Trit.Positive;
    private static (uint, uint) IsNegative(uint negative, uint positive) => (~positive, positive);

    // 1T0 [1, T, 0]
    public static Trit CyclicDecrement(Trit trit) => trit.Value == 0 ? Trit.Zero : trit.Value == 1 ? Trit.Negative : Trit.Positive;
    private static (uint, uint) CyclicDecrement(uint negative, uint positive) => (~positive & negative, positive);

    // 1T1 [1, T, 1]
    public static Trit IsNotZero(Trit trit) => trit.Value == -1 ? Trit.Positive : trit.Value == 0 ? Trit.Negative : Trit.Positive;
    private static (uint, uint) IsNotZero(uint negative, uint positive) => (~positive & negative, positive | ~negative);

    // 10T [1, 0, T]
    public static Trit Negate(Trit trit) => trit.Value == 1 ? Trit.Negative : Trit.Positive;
    private static (uint, uint) Negate(uint negative, uint positive) => (0u, positive);

    // 100 [1, 0, 0]
    public static Trit FloorIsNegative(Trit trit) => trit.Value == 0 ? Trit.Zero : Trit.Positive;
    private static (uint, uint) FloorIsNegative(uint negative, uint positive) => (0u, positive & ~negative);

    // 101 [1, 0, 1]
    public static Trit AbsoluteValue(Trit _) => Trit.Positive;
    private static (uint, uint) AbsoluteValue(uint negative, uint positive) => (0u, positive | ~negative);

    // 11T [1, 1, T]
    public static Trit IsNotPositive(Trit trit) => trit.Value == 1 ? Trit.Negative : Trit.Positive;
    private static (uint, uint) IsNotPositive(uint negative, uint positive) => (0u, positive | ~negative);

    // 110 [1, 1, 0]
    public static Trit FloorIsNotPositive(Trit trit) => trit.Value == 1 ? Trit.Zero : Trit.Positive;
    private static (uint, uint) FloorIsNotPositive(uint negative, uint positive) => (0u, positive);

    // 111 [1, 1, 1]
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
        return operations32[13 + table[0].Value + 3 * table[1].Value + 9 * table[2].Value](negative, positive);
    }
}
