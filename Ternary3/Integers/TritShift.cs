namespace Ternary3.Integers;

internal static class TritShift
{
    private static readonly int[] Pow3Cache = new int[20];  // Cache for int-sized powers (20 trits max in 32 bits)
    private static readonly long[] Pow3LongCache = new long[40];  // Cache for long-sized powers
    private static readonly int allOnes;  
    private static readonly long allOnesLong;  
    
    static TritShift()
    {
        Pow3Cache[0] = 1;
        Pow3LongCache[0] = 1L;
        
        for (var i = 1; i < Pow3Cache.Length; i++)
        {
            Pow3Cache[i] = Pow3Cache[i - 1] * 3;
        }

        for (var i = 1; i < Pow3LongCache.Length; i++)
        {
            Pow3LongCache[i] = Pow3LongCache[i - 1] * 3;
        }

        allOnes = (int)(Pow3LongCache[20] / 2);
        allOnesLong = Pow3LongCache[39] / 2 * 3 + 1;
    }

    public static sbyte Shift(this sbyte value, int shift)
    {
        if (value == 0 || shift == 0) return value;
        if ((uint)(shift + 4) > 8) return 0;  // if shift not between -4 and 4
        return shift > 0 ? (sbyte)(value / Pow3Cache[shift]) : (sbyte)(value * Pow3Cache[-shift]).BalancedModulo(121);
    }
    
    public static short Shift(this short value, int shift)
    {
        if (value == 0 || shift == 0) return value;
        if ((uint)(shift + 9) > 18) return 0;  // if shift not between -9 and 9
        return shift > 0 ? (short)(value / Pow3Cache[shift]) : (short)(value * Pow3Cache[-shift]).BalancedModulo(29524);
    }
    
    public static int Shift(this int value, int shift)
    {
        if (value == 0 || shift == 0) return value;
        if ((uint)(shift + 19) > 38) return 0;  // if shift not between -19 and 19
        return shift > 0 ? value / Pow3Cache[shift] : (int)((long)value * Pow3Cache[-shift]).BalancedModulo(1743392200);
    }
    
    public static long Shift(this long value, int shift)
    {
        if (value == 0 || shift == 0) return value;
        if ((uint)(shift + 39) > 78) return 0;  // if shift not between -39 and 39
        if (shift > 0)
            return value / Pow3LongCache[shift];
        return ((Int128)value * (Int128)Pow3LongCache[-shift]).BalancedModulo(6078832729528464400);
    }

    public static Trit Index(this int value, int index, int maxIndex)
    {
        if (index < 0 || index > maxIndex) throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {maxIndex} for int trit representation.");
        return value switch
        {
            0 => Trit.Zero,
            < 0 => new((sbyte)((value - allOnes) / Pow3Cache[index] % 3 + 1)),
            _ => new((sbyte)((value + allOnes) / Pow3Cache[index] % 3 - 1))
        };
    }
    
    public static Trit Index(this long value, int index, int maxIndex)
    {
        if (index < 0 || index > maxIndex) throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {maxIndex} for int trit representation.");
        return value switch
        {
            0 => Trit.Zero,
            < 0 => new((sbyte)((value - allOnesLong) / Pow3LongCache[index] % 3 + 1)),
            _ => new((sbyte)((value + allOnesLong) / Pow3LongCache[index] % 3 - 1))
        };
    }
}



