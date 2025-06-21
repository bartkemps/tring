namespace Ternary3.Formatting;

/// <summary>
/// Provides a culture-invariant ternary format with standard digit symbols and grouping.
/// </summary>
public class InvariantTernaryFormat : ITernaryFormat
{
    /// <inheritdoc/>
    public char NegativeTritDigit => 'T';
    /// <inheritdoc/>
    public char ZeroTritDigit => '0';
    /// <inheritdoc/>
    public char PositiveTritDigit => '1';
    /// <inheritdoc/>
    public IList<TritGroupDefinition> Groups { get; } =
    [
        new(" "),
        new("-")
    ];
    /// <inheritdoc/>
    public string DecimalSeparator => ".";
    /// <inheritdoc/>
    public TernaryPadding TernaryPadding => TernaryPadding.Full;
}