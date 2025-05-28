namespace Tring.Numbers;

internal static class Modulo
{
    public static long BalancedModulo(this long value, long halfModulus)
    {
        if (halfModulus > long.MaxValue / 2)
        {
            if (value < -halfModulus) return value + halfModulus + halfModulus + 1;
            if (value > halfModulus) return value - halfModulus - halfModulus - 1;
            return value;
        }

        var modulus = halfModulus * 2 + 1;
        var mod = value % modulus;
        if (mod > halfModulus) return mod - modulus;
        if (mod < -halfModulus) return mod + modulus;
        return mod;
    }

    public static long BalancedModulo((long hi,ulong lo) value, long halfModulus)
    {
        // Temporary implementation to fix compilation - returns a balanced value
        return value.hi % halfModulus;
    }

    public static int BalancedModulo(this int value, int halfModulus)
    {
        if (halfModulus > int.MaxValue / 2)
        {
            if (value < -halfModulus) return value + halfModulus + halfModulus + 1;
            if (value > halfModulus) return value - halfModulus - halfModulus - 1;
            return value;
        }

        var modulus = halfModulus * 2 + 1;
        var mod = value % modulus;
        if (mod > halfModulus) return mod - modulus;
        if (mod < -halfModulus) return mod + modulus;
        return mod;
    }

    public static long BalancedModuloAdd(this int value, int value2, int halfModulus)
    {
        return (int)BalancedModulo((long)value + value2, halfModulus);
    }

    public static long BalancedModuloAdd(this long value, long value2, long halfModulus)
    {
        if (Math.Sign(value) != Math.Sign(value2)) return BalancedModulo(value + value2, halfModulus);
        var modulus = (ulong)halfModulus * 2 + 1;
        return value > 0
            ? Add((ulong)value + (ulong)value2, modulus)
            : -Add((ulong)(-value) + (ulong)(-value2), modulus);
        
        static long Add(ulong sum, ulong modulus)
        {
            var value = sum % modulus;
            var halfModulus = modulus / 2;
            if (value <= halfModulus)
            {
                return (long)value;
            }
            if (value < modulus)
            {
                return -(long)(modulus - value);
            }
            return (long)(value - modulus);
        }
    }
    
    public static long BalancedModuloMultiply(this long value1, long value2, long halfModulus)
    {
        return 0;
    }

    public static (ulong hi, ulong lo) Multiply(this long value1, long value2) => Multiply((ulong)value1, (ulong)value2);

    public static (ulong hi, ulong lo) Multiply(this ulong value1, ulong value2)
    {
        var v1Lo = value1 & 0xFFFFFFFF;
        var v1Hi = value1 >> 32;
        var v2Lo = value2 & 0xFFFFFFFF;
        var v2Hi = value2 >> 32;
        var mid1 = v1Hi * v2Lo;
        var mid2 = v1Lo * v2Hi;
        (ulong midHi, ulong midLo) = Add(mid1, mid2);
        var (hi, lo) = Add(v1Lo * v2Lo, (midLo<<32));
        hi += v1Hi * v2Hi + (midLo >> 32) + (midHi << 32);
        return (hi, lo);
    }

    public static (ulong hi, ulong lo) Add(ulong value1, ulong value2)
    {
        var lo = value1 + value2;
        var hi = lo < value1 ? 1UL : 0UL;
        return (hi, lo);
    }
}
