namespace Ternary3.Integers;

using System.Numerics;

internal static class TritShift
{
    private static readonly int[] Pow3Cache = new int[20];  // Cache for int-sized powers (20 trits max in 32 bits)
    private static readonly long[] Pow3LongCache = new long[40];  // Cache for long-sized powers
    private static readonly int AllOnes;  
    private static readonly long AllOnesLong;  
    
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

        AllOnes = (int)(Pow3LongCache[20] / 2);
        AllOnesLong = Pow3LongCache[39] / 2 * 3 + 1;
    }

    public static sbyte Shift(this sbyte value, int shift)
    {
        if (value == 0 || shift == 0) return value;
        if ((uint)(shift + 4) > 8) return 0;  // if shift not between -4 and 4
        if (shift < 0) return (sbyte)(value * Pow3Cache[-shift]).BalancedModulo(121);
        return (sbyte)(value > 0
            ? (value + Pow3Cache[shift] / 2) / Pow3Cache[shift]
            : (value - Pow3Cache[shift] / 2) / Pow3Cache[shift]);
    }
    
    public static short Shift(this short value, int shift)
    {
        if (value == 0 || shift == 0) return value;
        if ((uint)(shift + 9) > 18) return 0;  // if shift not between -9 and 9
        if (shift < 0) return(short)(value * Pow3Cache[-shift]).BalancedModulo(29524);
        return (short)(value > 0
            ? (value + Pow3Cache[shift] / 2) / Pow3Cache[shift]
            : (value - Pow3Cache[shift] / 2) / Pow3Cache[shift]);
    }
    
    public static int Shift(this int value, int shift)
    {
        if (value == 0 || shift == 0) return value;
        if ((uint)(shift + 19) > 38) return 0;  // if shift not between -19 and 19
        if (shift < 0) return (int)((long)value * Pow3Cache[-shift]).BalancedModulo(1743392200);
        if (value > 0)
        {
            var sum = value + Pow3Cache[shift] / 2;
            // it might be possible to handle this without long conversion
            if (sum < value) return (int)Shift((long)value, shift);
            return sum / Pow3Cache[shift];
        }
        else
        {
            var sum = value - Pow3Cache[shift] / 2;
            // it might be possible to handle this without long conversion
            if (sum > value) return (int)Shift((long)value, shift);
            return (value - Pow3Cache[shift] / 2) / Pow3Cache[shift];
        }
    }
    
    public static long Shift(this long value, int shift)
    {
        if (value == 0 || shift == 0) return value;
        if ((uint)(shift + 39) > 78) return 0;  // if shift not between -39 and 39
        if (shift < 0) return ((Int128)value * (Int128)Pow3LongCache[-shift]).BalancedModulo(6078832729528464400);
        if (value > 0)
        {
            var sum = value + Pow3LongCache[shift] / 2;
            // it might be possible to handle this without Int128 conversion
            if (sum < value) return (long)(((Int128)value + Pow3LongCache[shift] / 2) / Pow3LongCache[shift]);
            return sum / Pow3LongCache[shift];
        }
        else
        {
            var sum = value - Pow3LongCache[shift] / 2;
            // it might be possible to handle this without Int128 conversion
            if (sum > value) return (long)(((Int128)value - Pow3LongCache[shift] / 2) / Pow3LongCache[shift]);
            return (value - Pow3LongCache[shift] / 2) / Pow3LongCache[shift];
        }
    }

    public static Trit Index(this int value, int index, int maxIndex)
    {
        if (index < 0 || index > maxIndex) throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {maxIndex} for int trit representation.");
        return value switch
        {
            0 => Trit.Zero,
            < 0 => new((sbyte)((value - AllOnes) / Pow3Cache[index] % 3 + 1)),
            _ => new((sbyte)((value + AllOnes) / Pow3Cache[index] % 3 - 1))
        };
    }
    
    public static Trit Index(this long value, int index, int maxIndex)
    {
        if (index < 0 || index > maxIndex) throw new ArgumentOutOfRangeException(nameof(index), $"Index must be between 0 and {maxIndex} for int trit representation.");
        return value switch
        {
            0 => Trit.Zero,
            < 0 => new((sbyte)((value - AllOnesLong) / Pow3LongCache[index] % 3 + 1)),
            _ => new((sbyte)((value + AllOnesLong) / Pow3LongCache[index] % 3 - 1))
        };
    }
}



