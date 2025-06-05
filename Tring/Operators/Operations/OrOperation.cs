namespace Tring.Operators.Operations;

using Numbers;

internal static partial class Operation
{
    public static Trit Or(Trit value1, Trit value2)
    {
        return (value1.Value + value2.Value) switch
        {
            < 0 => Trit.Negative,
            0 => Trit.Zero,
            _ => Trit.Positive
        };
    }

    /// <summary>
    /// perform a trinary oparation (add by trit, not modulo)
    ///    T  0  1
    /// T|T T 0
    /// 0|T 0  1
    /// 1|0 1 1
    /// </summary>
    /// <param name="value1">a value representing a ternary value</param>
    /// <param name="value2">a value representing a ternary value</param>
    public static sbyte Or(this sbyte value1, sbyte value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }

    public static int Or(this short value1, short value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }

    public static int Or(this int value1, int value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }

    public static long Or(this long value1, long value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }
}