namespace Ternary3;

using Formatting;

/// <summary>
/// Defines a contract for types that can be parsed from a string representation in a ternary format.
/// </summary>
/// <typeparam name="TSelf">The type the string is parsed to</typeparam>
public interface ITernaryParsable<out TSelf>
{
    /// <summary>
    /// Parses a string representation
    /// </summary>
    /// <param name="value">The string value to parse.</param>
    /// <returns>A value representing the parsed value.</returns>
    static abstract TSelf Parse(string value);

    /// <summary>
    /// Parses a string representation
    /// </summary>
    /// <param name="value">The string value to parse.</param>
    /// <param name="format">The format to use for parsing.</param>
    /// <returns>A value representing the parsed value.</returns>
    static abstract TSelf Parse(string value, ITernaryFormat format);

    /// <summary>
    /// Parses a string representation
    /// </summary>
    /// <param name="value">The string value to parse.</param>
    /// <param name="options">The options to use for parsing.</param>
    /// <returns>A value representing the parsed value.</returns>
    static abstract TSelf Parse(string value, TritParseOptions options);

    /// <summary>
    /// Parses a string representation
    /// </summary>
    /// <param name="value">The string value to parse.</param>
    /// <param name="format">The format to use for parsing.</param>
    /// <param name="options">The options to use for parsing.</param>
    /// <returns>A value representing the parsed value.</returns>
    static abstract TSelf Parse(string value, ITernaryFormat format, TritParseOptions options);
}