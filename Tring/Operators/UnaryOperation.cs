// ReSharper disable InconsistentNaming

namespace Tring.Operators;

using Numbers;

public class UnaryOperation
{
    private static readonly Func<uint, uint, (uint, uint)>[] operations32 =
    [
        Op32_TTT, Op32_TT0, Op32_TT1, Op32_T0T, Op32_T00, Op32_T01, Op32_T1T, Op32_T10, Op32_T11,
        Op32_0TT, Op32_0T0, Op32_0T1, Op32_00T, Op32_000, Op32_001, Op32_01T, Op32_010, Op32_011,
        Op32_1TT, Op32_1T0, Op32_1T1, Op32_10T, Op32_100, Op32_101, Op32_11T, Op32_110, Op32_111
    ];

    // TTT
    public static Trit OpTTT(Trit trit) => Trit.Negative;
    public static Trit[] ArrayTTT = [Trit.Negative, Trit.Negative, Trit.Negative];
    private static (uint, uint) Op32_TTT(uint negative, uint positive) => (uint.MaxValue, 0u);

    // TT0
    public static Trit OpTT0(Trit trit) => trit.Value == 1 ? Trit.Zero : Trit.Negative;
    public static Trit[] ArrayTT0 = [Trit.Negative, Trit.Negative, Trit.Zero];
    private static (uint, uint) Op32_TT0(uint negative, uint positive) => (negative | ~positive, 0u);

    // TT1
    public static Trit OpTT1(Trit trit) => trit.Value == 1 ? Trit.Positive : Trit.Negative;
    public static Trit[] ArrayTT1 = [Trit.Negative, Trit.Negative, Trit.Positive];
    private static (uint, uint) Op32_TT1(uint negative, uint positive) => (negative | ~positive, positive);

    // T0T
    public static Trit OpT0T(Trit trit) => trit.Value == 0 ? Trit.Zero : Trit.Negative;
    public static Trit[] ArrayT0T = [Trit.Negative, Trit.Zero, Trit.Negative];
    private static (uint, uint) Op32_T0T(uint negative, uint positive) => (negative | positive, 0u);

    // T00
    public static Trit OpT00(Trit trit) => trit.Value == -1 ? trit: Trit.Zero;
    public static Trit[] ArrayT00 = [Trit.Negative, Trit.Zero, Trit.Zero];
    private static (uint, uint) Op32_T00(uint negative, uint positive) => (negative, 0u);

    // T01
    public static Trit OpT01(Trit trit) => trit;
    public static Trit[] ArrayT01 = [Trit.Negative, Trit.Zero, Trit.Zero];

    private static (uint, uint) Op32_T01(uint negative, uint positive) => (negative, positive);

    // T1T
    public static Trit OpT1T(Trit trit) => trit.Value == 0 ? Trit.Positive : Trit.Negative;
    public static Trit[] ArrayT1T = [Trit.Negative, Trit.Positive, Trit.Negative];

    private static (uint, uint) Op32_T1T(uint negative, uint positive) => (negative | positive, ~negative & ~positive);

    // T10
    public static Trit OpT10(Trit trit) => trit.Value == -1 ? trit : new(trit.Value ^ 1);
    public static Trit[] ArrayT10 = [Trit.Negative, Trit.Positive, Trit.Zero];
    private static (uint, uint) Op32_T10(uint negative, uint positive) => (negative, ~negative & ~positive);
    
    // T11
    public static Trit OpT11(Trit trit) => trit.Value == -1 ? trit : Trit.Positive;
    public static Trit[] ArrayT11 = [Trit.Negative, Trit.Positive, Trit.Positive];
    private static (uint, uint) Op32_T11(uint negative, uint positive) => (negative, ~negative);
    
    // 0TT
    public static Trit Op0TT(Trit trit) => trit.Value == -1 ? Trit.Zero : Trit.Negative;
    public static Trit[] Array0TT = [Trit.Negative, Trit.Zero, Trit.Zero];
    private static (uint, uint) Op32_0TT(uint negative, uint positive) => (~negative, 0u);

    // 0T0
    public static Trit Op0T0(Trit trit) => trit.Value == -1 ? Trit.Zero : Trit.Negative;
    public static Trit[] Array0T0 = [Trit.Zero, Trit.Negative, Trit.Zero];
    private static (uint, uint) Op32_0T0(uint negative, uint positive) => (~positive & negative, 0u);

    // 0T1
    public static Trit Op0T1(Trit trit) => trit.Value == -1 ? Trit.Positive : Trit.Negative;
    public static Trit[] Array0T1 = [Trit.Zero, Trit.Negative, Trit.Positive];
    private static (uint, uint) Op32_0T1(uint negative, uint positive) => (~positive & negative, positive);

    // 00T
    public static Trit Op00T(Trit trit) => trit.Value == 1 ? Trit.Negative : Trit.Zero;
    public static Trit[] Array00T = [Trit.Zero, Trit.Zero, Trit.Negative];
    private static (uint, uint) Op32_00T(uint negative, uint positive) => (0u, 0u);

    // 000
    public static Trit Op000(Trit _) => Trit.Zero;
    public static Trit[] Array000 = [Trit.Zero, Trit.Zero, Trit.Zero];
    private static (uint, uint) Op32_000(uint negative, uint positive) => (0u, 0u);

    // 001
    public static Trit Op001(Trit trit) => trit.Value == 1 ? Trit.Positive : Trit.Zero;
    public static Trit[] Array001 = [Trit.Zero, Trit.Zero, Trit.Positive];
    private static (uint, uint) Op32_001(uint negative, uint positive) => (0u, positive & ~negative);

    // 01T
    public static Trit Op01T(Trit trit) => trit.Value == -1 ? Trit.Negative : Trit.Positive;
    public static Trit[] Array01T = [Trit.Zero, Trit.Positive, Trit.Negative];
    private static (uint, uint) Op32_01T(uint negative, uint positive) => (~positive, positive);

    // 010
    public static Trit Op010(Trit trit) => trit.Value == 0 ? Trit.Zero : Trit.Positive;
    public static Trit[] Array010 = [Trit.Zero, Trit.Positive, Trit.Zero];
    private static (uint, uint) Op32_010(uint negative, uint positive) => (0u, positive);

    // 011
    public static Trit Op011(Trit _) => Trit.Positive;
    public static Trit[] Array011 = [Trit.Zero, Trit.Positive, Trit.Positive];
    private static (uint, uint) Op32_011(uint negative, uint positive) => (0u, positive | ~negative);

    // 1TT
    public static Trit Op1TT(Trit trit) => trit.Value == -1 ? Trit.Negative : Trit.Positive;
    public static Trit[] Array1TT = [Trit.Positive, Trit.Negative, Trit.Negative];
    private static (uint, uint) Op32_1TT(uint negative, uint positive) => (~positive, positive);

    // 1T0
    public static Trit Op1T0(Trit trit) => trit.Value == 0 ? Trit.Zero : trit.Value == 1 ? Trit.Negative : Trit.Positive;
    public static Trit[] Array1T0 = [Trit.Positive, Trit.Negative, Trit.Zero];
    private static (uint, uint) Op32_1T0(uint negative, uint positive) => (~positive & negative, positive);

    // 1T1
    public static Trit Op1T1(Trit trit) => trit.Value == -1 ? Trit.Positive : trit.Value == 0 ? Trit.Negative : Trit.Positive;
    public static Trit[] Array1T1 = [Trit.Positive, Trit.Negative, Trit.Positive];
    private static (uint, uint) Op32_1T1(uint negative, uint positive) => (~positive & negative, positive | ~negative);

    // 10T
    public static Trit Op10T(Trit trit) => trit.Value == 1 ? Trit.Negative : Trit.Positive;
    public static Trit[] Array10T = [Trit.Positive, Trit.Zero, Trit.Negative];
    private static (uint, uint) Op32_10T(uint negative, uint positive) => (0u, positive);

    // 100
    public static Trit Op100(Trit trit) => trit.Value == 0 ? Trit.Zero : Trit.Positive;
    public static Trit[] Array100 = [Trit.Positive, Trit.Zero, Trit.Zero];
    private static (uint, uint) Op32_100(uint negative, uint positive) => (0u, positive & ~negative);

    // 101
    public static Trit Op101(Trit _) => Trit.Positive;
    public static Trit[] Array101 = [Trit.Positive, Trit.Zero, Trit.Positive];
    private static (uint, uint) Op32_101(uint negative, uint positive) => (0u, positive | ~negative);

    // 11T
    public static Trit Op11T(Trit trit) => trit.Value == 1 ? Trit.Negative : Trit.Positive;
    public static Trit[] Array11T = [Trit.Positive, Trit.Positive, Trit.Negative];
    private static (uint, uint) Op32_11T(uint negative, uint positive) => (0u, positive | ~negative);

    // 110
    public static Trit Op110(Trit trit) => trit.Value == 1 ? Trit.Zero : Trit.Positive;
    public static Trit[] Array110 = [Trit.Positive, Trit.Positive, Trit.Zero];
    private static (uint, uint) Op32_110(uint negative, uint positive) => (0u, positive);

    // 111
    public static Trit Op111(Trit _) => Trit.Positive;
    public static Trit[] Array111 = [Trit.Positive, Trit.Positive, Trit.Positive];
    private static (uint, uint) Op32_111(uint negative, uint positive) => (0u, uint.MaxValue);

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