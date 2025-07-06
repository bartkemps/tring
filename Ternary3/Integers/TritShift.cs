namespace Ternary3.Integers;

internal static class TritShift
{
    private static readonly int[] pow3Cache = new int[20];  // Cache for int-sized powers (20 trits max in 32 bits)
    private static readonly long[] pow3LongCache = new long[40];  // Cache for long-sized powers
    private static readonly int allOnes;  
    private static readonly long allOnesLong;  
    
    static TritShift()
    {
        pow3Cache[0] = 1;
        pow3LongCache[0] = 1L;
        
        for (var i = 1; i < pow3Cache.Length; i++)
        {
            pow3Cache[i] = pow3Cache[i - 1] * 3;
        }

        for (var i = 1; i < pow3LongCache.Length; i++)
        {
            pow3LongCache[i] = pow3LongCache[i - 1] * 3;
        }

        allOnes = (int)(pow3LongCache[20] / 2);
        allOnesLong = pow3LongCache[39] / 2 * 3 + 1;
    }

    public static sbyte Shift(this sbyte value, int shift)
    {
        if (value == 0 || shift == 0) return value;
        if ((uint)(shift + 4) > 8) return 0;  // if shift not between -4 and 4
        return shift > 0 ? (sbyte)(value / pow3Cache[shift]) : (sbyte)(value * pow3Cache[-shift]).BalancedModulo(121);
    }
    
    public static short Shift(this short value, int shift)
    {
        if (value == 0 || shift == 0) return value;
        if ((uint)(shift + 9) > 18) return 0;  // if shift not between -9 and 9
        return shift > 0 ? (short)(value / pow3Cache[shift]) : (short)(value * pow3Cache[-shift]).BalancedModulo(29524);
    }
    
    public static int Shift(this int value, int shift)
    {
        if (value == 0 || shift == 0) return value;
        if ((uint)(shift + 19) > 38) return 0;  // if shift not between -19 and 19
        return shift > 0 ? value / pow3Cache[shift] : (int)((long)value * pow3Cache[-shift]).BalancedModulo(1743392200);
    }
    
    public static long Shift(this long value, int shift)
    {
        if (value == 0 || shift == 0) return value;
        if ((uint)(shift + 39) > 78) return 0;  // if shift not between -39 and 39
        if (shift > 0)
            return value / pow3LongCache[shift];
        return ((Int128)value * (Int128)pow3LongCache[-shift]).BalancedModulo(6078832729528464400);
    }

    public static Trit Index(this int value, int index, int maxIndex)
    {
        if (index < 0 || index > maxIndex) throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {maxIndex} for int trit representation.");
        return value switch
        {
            0 => Trit.Zero,
            < 0 => new((sbyte)((value - allOnes) / pow3Cache[index] % 3 + 1)),
            _ => new((sbyte)((value + allOnes) / pow3Cache[index] % 3 - 1))
        };
    }
    
    public static Trit Index(this long value, int index, int maxIndex)
    {
        if (index < 0 || index > maxIndex) throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {maxIndex} for int trit representation.");
        return value switch
        {
            0 => Trit.Zero,
            < 0 => new((sbyte)((value - allOnesLong) / pow3LongCache[index] % 3 + 1)),
            _ => new((sbyte)((value + allOnesLong) / pow3LongCache[index] % 3 - 1))
        };
    }
}



