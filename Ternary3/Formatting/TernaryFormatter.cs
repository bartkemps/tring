namespace Ternary3.Formatting;

using TritArrays;

public class TernaryFormatter(ITernaryFormat? ternaryFormat = null) : ITernaryFormatter
{
    private readonly ITernaryFormat format = ternaryFormat ?? TernaryFormat.Invariant;
    
    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        if (arg is ITritArray trits) { return Format(trits); }
        return formatProvider?.GetFormat(typeof(ICustomFormatter)) is ITernaryFormatter customFormatter
            ? customFormatter.Format(format, arg, formatProvider)
            : throw new ArgumentException("Argument is not a valid trit array.", nameof(arg));
    }

    private string Format(ITritArray trits)
    {
        if (trits.Length == 0) return string.Empty;

        var lastIndex = trits.Length - 1;
        if (format.TernaryPadding != TernaryPadding.Full)
        {
            while (lastIndex > 0 && trits[lastIndex].Value == 0)
            {
                lastIndex--;
            }
        }

        if (format.TernaryPadding == TernaryPadding.Group)
        {
            var firstGroupSize = format.Groups[0].Size;
            if (lastIndex % firstGroupSize != firstGroupSize - 1)
            {
                lastIndex += firstGroupSize - (lastIndex % firstGroupSize) - 1;
                if (lastIndex >= trits.Length)
                {
                    lastIndex = trits.Length - 1;
                }
            }
        }

        var groupCounts = new int[format.Groups.Count];
        var result = new List<string>();
        for (var i = 0; i <= lastIndex - 1; i++)
        {
            result.Insert(0, MapTrit(trits[i]).ToString());
            groupCounts[0]++;
            for (var level = 0; level < format.Groups.Count; level++)
            {
                if (groupCounts[level] == format.Groups[level].Size)
                {
                    groupCounts[level] = 0;
                    if (level + 1 < format.Groups.Count)
                    {
                        groupCounts[level + 1]++;
                    }

                    if (level + 1 == format.Groups.Count || groupCounts[level + 1] < format.Groups[level + 1].Size)
                    {
                        result.Insert(0, format.Groups[level].Separator);
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
            -1 => format.NegativeTritDigit,
            0 => format.ZeroTritDigit,
            1 => format.PositiveTritDigit,
            _ => '?'
        };
    }
}