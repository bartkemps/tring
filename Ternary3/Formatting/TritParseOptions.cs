namespace Ternary3.Formatting;

/// <summary>
/// Specifies options for parsing ternary number strings.
/// </summary>
[Flags]
public enum TritParseOptions
{
    /// <summary>
    /// No special parsing options.
    /// </summary>
    None = 0,
    /// <summary>
    /// Allows the input string to contain no digits. The result will be zero.
    /// </summary>
    AllowAbsenceOfDigits = 1 << 1,
    /// <summary>
    /// Ignores whitespace characters in the input string.
    /// </summary>
    AllowWhitespace = 1 << 2,
    /// <summary>
    /// Ignores dash characters in the input string, unless they are defined as digits in the format.
    /// </summary>
    AllowDashes = 1 << 3,
    /// <summary>
    /// Ignores underscore characters in the input string, unless they are defined as digits in the format.
    /// </summary>
    AllowUnderscores = 1 << 4,
    /// <summary>
    /// Ignores group separator characters in the input string.
    /// </summary>
    AllowGroupSerparators = 1 << 5,
    /// <summary>
    /// Ignores invalid characters in the input string. This also allows for whitespace, dashes, underscores.
    /// </summary>
    AllowInvalidCharacters = 1 << 6,
    /// <summary>
    /// If the parsed value exceeds the maximum number of trits, it will be truncated to fit the maximum size.
    /// </summary>
    AllowOverflow = 1 << 7,
    /// <summary>
    /// Allows a decimal separator in the input string, which will be treated as a decimal point for floating-point trit values.
    /// When parsing to an interger ternary value, the decimal separator will be ignored, and the value will be rounded down.
    /// </summary>
    AllowDecimal = 1 << 9,
    /// <summary>
    /// Do not strictly enforce the case of characters in the input string. For example, 't' and 'T' will be treated as equivalent.
    /// </summary>
    CaseInsensitive = 1 << 8,
    /// <summary>
    /// Lax parsing options that allow for a wide range of input formats
    /// </summary>
    Default = AllowAbsenceOfDigits | AllowWhitespace | AllowDashes | AllowUnderscores | AllowGroupSerparators | AllowOverflow | CaseInsensitive | AllowDecimal
}