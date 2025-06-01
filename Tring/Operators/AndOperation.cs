namespace Tring.Operators;

using Numbers;

internal static partial class Operation
{
    public static Trit And(Trit value1, Trit value2)
    {
        return new((sbyte)(value1.Value * value2.Value));
    }

    /// <summary>
    /// perform a trinary oparation (multiply by trit)
    ///    T  0  1
    /// T|1 0 T
    /// 0|0  0  0
    /// 1|T 0 1
    /// </summary>
    /// <param name="value1">a value between -121 and 121, representing a ternary value</param>
    /// <param name="value2">a value between -121 and 121, representing a ternary value</param>
    public static sbyte And(sbyte value1, sbyte value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }

    public static int And(short value1, short value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }

    public static int And(int value1, int value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }

    public static long And(long value1, long value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }
}