namespace Ternary3.Generators;

using System.Linq;

public class Conversion
{
    public static long ToLong(string digits)
    {
        long value = 0;
        long scale = 1;
        foreach (var digit in digits.Reverse())
        {
            var i = "T01".IndexOf(digit);
            if (i == -1) continue;
            value += (i - 1) * scale;
            scale *= 3;
        }

        return value;
    }
}