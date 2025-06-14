﻿namespace Ternary3.Integers;

internal static class TritShift
{
    private static readonly int[] pow3Cache = new int[20];  // Cache for int-sized powers (20 trits max in 32 bits)
    private static readonly long[] pow3LongCache = new long[40];  // Cache for long-sized powers

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
        else
            return ((Int128)value * (Int128)pow3LongCache[-shift]).BalancedModulo(6078832729528464400);
    }
}



