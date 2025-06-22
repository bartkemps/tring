namespace Ternary3.Formatting;

public class TernaryFormatProvider(IFormatProvider? inner = null, ITernaryFormat? format = null):IFormatProvider
{
    public object? GetFormat(Type? formatType)
    {
        return formatType == typeof(ICustomFormatter) || formatType == typeof(ITernaryFormatter)
            ? new TernaryFormatter(format) 
            : inner?.GetFormat(formatType);
    }
}