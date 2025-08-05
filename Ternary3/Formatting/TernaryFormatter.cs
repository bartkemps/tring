namespace Ternary3.Formatting;

using System.Numerics;

internal class TernaryFormatter(ITernaryFormat? ternaryFormat = null, IFormatProvider? inner = null) : ITernaryFormatter
{
    private readonly ITernaryFormat ternaryFormat = ternaryFormat ?? TernaryFormat.Current;
    
    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        // Allowlisted ternary format strings
        if (format == "ter")
        {
            return arg switch
            {
                TernaryArray27 value => Format(value),
                TernaryArray9 value => Format(value),
                TernaryArray3 value => Format(value),
                TernaryArray value => Format(value),
                BigTernaryArray value => Format(value),
                ITernaryArray value => Format(value),
                Int27T value => Format((TernaryArray27)value),
                Int9T value => Format((TernaryArray9)value),
                Int3T value => Format((TernaryArray3)value),
                long value => Format((TernaryArray)value),
                int value => Format((TernaryArray)value),
                uint value => Format((TernaryArray)value),
                short value => Format((TernaryArray)value),
                ushort value => Format((TernaryArray)value),
                sbyte value => Format((TernaryArray)value),
                byte value => Format((TernaryArray)value),
                _ => throw new FormatException($"Cannot format {arg?.GetType().Name} as ternary.")
            };
        }

        // Forward to default formatters using a switch expression and avoid infinite recursion
        return arg switch
        {
            TernaryArray27 value => ((long)(Int27T)value).ToString(format, inner),
            TernaryArray9 value => ((short)(Int9T)value).ToString(format, inner),
            TernaryArray3 value => ((sbyte)(Int3T)value).ToString(format, inner),
            BigTernaryArray value => ((BigInteger)value).ToString(format, inner),
            TernaryArray value => ((Int128)value).ToString(format, inner),
            Int27T value => ((Int128)value).ToString(format, inner),
            Int9T value => ((short)value).ToString(format, inner),
            Int3T value => ((sbyte)value).ToString(format, inner),
            IFormattable formattable => formattable.ToString(format, inner),
            _ => arg?.ToString() ?? string.Empty
        };
    }

    // To do: optimized version for known trit types
    public string Format(ITernaryArray trits)
    {
        if (!TernaryFormatValidator.Validate(ternaryFormat)) 
        {
            throw new FormatException("Invalid ternary format.");
        }
        if (trits.Length == 0) return string.Empty;

        var lastIndex = trits.Length - 1;
        if (ternaryFormat.TernaryPadding != TernaryPadding.Full)
        {
            while (lastIndex > 0 && trits[lastIndex].Value == 0)
            {
                lastIndex--;
            }
        }

        if (ternaryFormat.TernaryPadding == TernaryPadding.Group)
        {
            var firstGroupSize = ternaryFormat.Groups[0].Size;
            if (lastIndex % firstGroupSize != firstGroupSize - 1)
            {
                lastIndex += firstGroupSize - (lastIndex % firstGroupSize) - 1;
                if (lastIndex >= trits.Length)
                {
                    lastIndex = trits.Length - 1;
                }
            }
        }

        var groupCounts = new int[ternaryFormat.Groups.Count];
        var result = new List<string>();
        for (var i = 0; i <= lastIndex - 1; i++)
        {
            result.Insert(0, MapTrit(trits[i]).ToString());
            groupCounts[0]++;
            for (var level = 0; level < ternaryFormat.Groups.Count; level++)
            {
                if (groupCounts[level] == ternaryFormat.Groups[level].Size)
                {
                    groupCounts[level] = 0;
                    if (level + 1 < ternaryFormat.Groups.Count)
                    {
                        groupCounts[level + 1]++;
                    }

                    if (level + 1 == ternaryFormat.Groups.Count || groupCounts[level + 1] < ternaryFormat.Groups[level + 1].Size)
                    {
                        result.Insert(0, ternaryFormat.Groups[level].Separator);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        result.Insert(0, MapTrit(trits[lastIndex]).ToString());
        return string.Concat(result);

        char MapTrit(Trit t) => t.Value switch
        {
            -1 => ternaryFormat.NegativeTritDigit,
            0 => ternaryFormat.ZeroTritDigit,
            1 => ternaryFormat.PositiveTritDigit,
            _ => '?'
        };
    }
}