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
}
