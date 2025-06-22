namespace Ternary3.TritArrays;

using Formatting;

internal static class Formatter
{
    public static string Format(TritArray3 trits, string? format, IFormatProvider? provider = null)
    {
        if (provider is not null && provider.GetFormat(typeof(ICustomFormatter)) is ITernaryFormatter customFormatter)
        {
            return customFormatter.Format(format, trits, provider);
        }

        return ((Int3T)trits).ToString(format, new TernaryFormatProvider(provider));
    }
    
    public static string Format(TritArray9 trits, string? format, IFormatProvider? provider = null)
    {
        if (provider is not null && provider.GetFormat(typeof(ICustomFormatter)) is ITernaryFormatter customFormatter)
        {
            return customFormatter.Format(format, trits, provider);
        }

        return ((Int9T)trits).ToString(format, new TernaryFormatProvider(provider));
    }
    
    public static string Format(TritArray27 trits, string? format, IFormatProvider? provider = null)
    {
        if (provider is not null && provider.GetFormat(typeof(ICustomFormatter)) is ITernaryFormatter customFormatter)
        {
            return customFormatter.Format(format, trits, provider);
        }
        return ((Int27T)trits).ToString(format, new TernaryFormatProvider(provider));
    }
}