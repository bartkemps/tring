namespace Tring.Numbers;

internal static class Operations
{
    /// <summary>
    /// perform a trinary oparation (multiply by trit)
    ///    T  0  1
    /// T|1 0 T
    /// 0|0  0  0
    /// 1|T 0 1
    /// </summary>
    /// <param name="value1">a value between -121 and 121, representing a ternary value</param>
    /// <param name="value2">a value between -121 and 121, representing a ternary value</param>
    public static sbyte And(this sbyte value1, sbyte value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }
    
    public static int And(this short value1, short value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }
    
    public static int And(this int value1, int value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }
    
    public static long And(this long value1, long value2)
    {
        throw new NotImplementedException("Ternary operations are not implemented yet.");
    }
    
    /// <summary>
    /// perform a trinary oparation (add by trit, not modulo)
    ///    T  0  1
    /// T|T T 1
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
    
    /// <summary>
    /// perform a trinary oparation (add by trit, modulo)
    ///    T  0  1
    /// T|1 T T
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