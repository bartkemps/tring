namespace Ternary3.Formatting;

using TernaryArrays;

internal static class TernaryFormatValidator
{
    /// <summary>
    /// Checks if the provided format is valid for ternary number representation:
    /// The characters for negative, zero, and positive and decimal separator must be distinct and not
    /// occur in the group definitions.
    /// </summary>
    /// <param name="format"></param>
    public static bool Validate(ITernaryFormat format)
    {
        // Check if a format is null or the decimal separator is null or groups are null
        if (format?.DecimalSeparator == null || format.DecimalSeparator.Length == 0 || format.Groups == null) return false;

        // Ensure that the used characters for negative, zero, and positive trits and the decimal separator are distinct
        char[] meaningfulChars = [format.NegativeTritDigit,  format.ZeroTritDigit, format.PositiveTritDigit, ..format.DecimalSeparator.Distinct()];
        if (meaningfulChars.Length != meaningfulChars.Distinct().Count()) return false; // Ensure all characters are distinct

        // ensure that the used characters do not appear in any group separator
        foreach (var group in format.Groups)
        {
            var separator = group?.Separator;
            if (separator == null) return false;
            if (meaningfulChars.Any(c => separator.Contains(c))) return false;
        }

        return true;
    }

    public static bool Validate(ITernaryFormat format, TritParseOptions options)
    {
        if (!options.HasFlag(TritParseOptions.CaseInsensitive)) return Validate(format);
        
        // Check if a format is null or the decimal separator is null or groups are null
        if (format?.DecimalSeparator == null || format.DecimalSeparator.Length == 0 || format.Groups == null) return false;

        // Ensure that the used characters for negative, zero, and positive trits and the decimal separator are distinct
        char[] meaningfulChars = [
            char.ToLowerInvariant(format.NegativeTritDigit), 
            char.ToLowerInvariant(format.ZeroTritDigit), 
            char.ToLowerInvariant(format.PositiveTritDigit),
            ..format.DecimalSeparator.ToLowerInvariant().Distinct()];
        if (meaningfulChars.Length != meaningfulChars.Distinct().Count()) return false; // Ensure all characters are distinct

        // ensure that the used characters do not appear in any group separator
        foreach (var group in format.Groups)
        {
            var separator = group?.Separator.ToLowerInvariant();
            if (separator == null) return false;
            if (meaningfulChars.Any(c => separator.Contains(c))) return false;
        }

        return true;
    }
}