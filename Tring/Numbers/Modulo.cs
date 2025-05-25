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
}


