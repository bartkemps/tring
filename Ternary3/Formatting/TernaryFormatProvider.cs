namespace Ternary3.Formatting;

/// <summary>
/// Provides formatting capabilities for ternary numbers by implementing IFormatProvider.
/// </summary>
/// <param name="format">The ternary format to use, or null to use the current format.</param>
/// <param name="inner">The inner format provider for chaining, or null if none.</param>
public class TernaryFormatProvider(
    ITernaryFormat? format = null, 
    IFormatProvider? inner = null) : IFormatProvider
{
    /// <summary>
    /// Gets an object that provides formatting services for the specified type.
    /// </summary>
    /// <param name="formatType">The type of format object to return.</param>
    /// <returns>
    /// A TernaryFormatter if the formatType is ICustomFormatter or ITernaryFormatter;
    /// otherwise, delegates to the inner provider if present.
    /// </returns>
    public object? GetFormat(Type? formatType)
    {
        return formatType == typeof(ICustomFormatter) || formatType == typeof(ITernaryFormatter)
            ? new TernaryFormatter(format ?? TernaryFormat.Current, inner)
            : inner?.GetFormat(formatType);
    }
}