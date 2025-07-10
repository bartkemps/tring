namespace Ternary3.Formatting;

/// <summary>
/// Provides a minimal format for ternary numbers using 't', '0', and '1' for negative, zero, and positive trits respectively.
/// </summary>
public class MinimalTernaryFormat : ITernaryFormat
{
    /// <inheritdoc/>
    public char NegativeTritDigit => 't';
    /// <inheritdoc/>
    public char ZeroTritDigit => '0';
    /// <inheritdoc/>
    public char PositiveTritDigit => '1';
    /// <inheritdoc/>
    public IList<TritGroupDefinition> Groups { get; } = [];
    /// <inheritdoc/>
    public string DecimalSeparator => ".";
    /// <inheritdoc/>
    public TernaryPadding TernaryPadding => TernaryPadding.None;
}