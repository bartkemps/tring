namespace Ternary3.Formatting;

public class TernaryFormatProvider(ITernaryFormat? format = null, IFormatProvider? inner = null) : IFormatProvider
{
    public object? GetFormat(Type? formatType)
    {
        return formatType == typeof(ICustomFormatter) || formatType == typeof(ITernaryFormatter)
            ? new TernaryFormatter(format ?? TernaryFormat.Current, inner)
            : inner?.GetFormat(formatType);
    }
}