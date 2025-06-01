namespace Tring.Operators;

using Numbers;

public static class UnaryOperation
{
    // TTT
    public static Trit OpTTT(this Trit _) => Trit.Negative;
    // 001
    public static Trit OpTT0(this Trit trit) => trit.Value == 1 ? Trit.Zero : Trit.Negative;
}