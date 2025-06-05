namespace Tring.Numbers.TritArrays;

using System.Runtime.CompilerServices;

/// <summary>
/// Encodes trits in two arrays: one for positive trits and one for negative trits.
/// (both are never set at the same time)
/// </summary>
internal static class TritConverter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Trit GetTrit(ref byte positive, ref byte negative, int index)
        => new((int)((positive >> index) & 1) - (int)((negative >> index) & 1));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Trit GetTrit(ref uint positive, ref uint negative, int index)
        => new((int)((positive >> index) & 1) - (int)((negative >> index) & 1));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Trit GetTrit(ref ulong positive, ref ulong negative, int index)
        => new((int)((positive >> index) & 1) - (int)((negative >> index) & 1));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetTrit(ref uint positive, ref uint negative, int index, Trit value)
    {
        var mask = 1u << index;
        switch (value.Value)
        {
            case 1:
                positive |= mask;
                negative &= ~mask;
                break;
            case -1:
                positive &= ~mask;
                negative |= mask;
                break;
            default: // case 0
                positive &= ~mask;
                negative &= ~mask;
                break;
        }
    }

    public static void ConvertTo32Trits(int value, out uint negative, out uint positive)
    {
        positive = 0;
        negative = 0;
        if (value == 0) return;
        var isNegative = value < 0;
        if (value > 0) value = -value;
        for(var index=0; value < 0 && index < 32; index++)
        {
            var remainder = value % 3;
            value /= 3;

            switch (remainder)
            {
                case 0:
                    break;
                case -1:
                    positive |= 1u << index;
                    break;
                case -2:
                    negative |= 1u << index;
                    value--;
                    break;
            }
        }
        if (isNegative)
        {
            (negative, positive) = (positive, negative);
        }
    }
    
    public static void ConvertTo32Trits(long value, out uint negative, out uint positive)
    {
        positive = 0;
        negative = 0;
        if (value == 0) return;
        var isNegative = value < 0;
        if (value > 0) value = -value;
        for(var index=0; value < 0 && index < 32; index++)
        {
            var remainder = value % 3;
            value /= 3;

            switch (remainder)
            {
                case 0:
                    break;
                case -1:
                    positive |= 1u << index;
                    break;
                case -2:
                    negative |= 1u << index;
                    value--;
                    break;
            }
        }
        if (isNegative)
        {
            (negative, positive) = (positive, negative);
        }
    }
    
    public static void ConvertTo64Trits(long value, out ulong negative, out ulong positive)
    {
        positive = 0;
        negative = 0;
        if (value == 0) return;
        var isNegative = value < 0;
        if (value > 0) value = -value;
        for(var index=0; value < 0 && index < 64; index++)
        {
            var remainder = value % 3;
            value /= 3;

            switch (remainder)
            {
                case 0:
                    break;
                case -1:
                    positive |= 1ul << index;
                    break;
                case -2:
                    negative |= 1ul << index;
                    value--;
                    break;
            }
        }
        if (isNegative)
        {
            (negative, positive) = (positive, negative);
        }
    }

    public static void ConvertTo64Trits(Int128 value, out ulong negative, out ulong positive)
    {
        positive = 0;
        negative = 0;
        if (value == 0) return;
        var isNegative = value < 0;
        if (value > 0) value = -value;
        for(var index=0; value < 0 && index < 128; index++)
        {
            var remainder = (int)value % 3;
            value /= 3;

            switch (remainder)
            {
                case 0:
                    break;
                case -1:
                    positive |= 1ul << index;
                    break;
                case -2:
                    negative |= 1ul << index;
                    value--;
                    break;
            }
        }
        if (isNegative)
        {
            (negative, positive) = (positive, negative);
        }
    }

    public static int TritsToInt32(uint negative, uint positive)
    {
        var result = 0;
        var power = 1;

        for (var i = 0; i < 32; i++)
        {
            if ((positive & (1u << i)) != 0)
                result += power;
            else if ((negative & (1u << i)) != 0)
                result -= power;
            power *= 3;
        }

        return result;
    }
    
    public static long TritsToInt64(uint negative, uint positive)
    {
        var result = 0L;
        var power = 1L;

        for (var i = 0; i < 32; i++)
        {
            if ((positive & (1u << i)) != 0)
                result += power;
            else if ((negative & (1u << i)) != 0)
                result -= power;
            power *= 3;
        }

        return result;
    }
    
    public static long TritsToInt64(ulong negative, ulong positive)
    {
        var result = 0L;
        var power = 1L;

        for (var i = 0; i < 64; i++)
        {
            if ((positive & (1ul << i)) != 0)
                result += power;
            else if ((negative & (1ul << i)) != 0)
                result -= power;
            power *= 3;
        }

        return result;
    }
    
    public static Int128 TritsToInt128(ulong negative, ulong positive)
    {
        Int128 result = 0;
        Int128 power = 1;

        for (var i = 0; i < 128; i++)
        {
            if ((positive & (1ul << i)) != 0)
                result += power;
            else if ((negative & (1ul << i)) != 0)
                result -= power;
            power *= 3;
        }

        return result;
    }
}