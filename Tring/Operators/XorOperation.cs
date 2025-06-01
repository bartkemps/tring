namespace Tring.Operators;

using Numbers;

internal static partial class Operation
{
    public static Trit Xor(Trit value1, Trit value2)
    {
        return (value1.Value + value2.Value) switch
        {
            0 => Trit.Zero,
            -2 or 1 => Trit.Positive,
            _ => Trit.Negative
        };
    }

    /// <summary>
    /// perform a trinary oparation (add by trit, modulo)
    ///    T  0  1
    /// T|1 T 0
    /// 0|T 0  1
    /// 1|0 1 T
    /// </summary>
    /// <param name="value1">a value representing a ternary value</param>
    /// <param name="value2">a value representing a ternary value</param>
    public static sbyte Xor(this sbyte value1, sbyte value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }

    public static short Xor(this short value1, short value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }

    public static int Xor(this int value1, int value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }

    public static long Xor(this long value1, long value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }
}