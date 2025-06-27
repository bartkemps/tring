namespace Ternary3.Formatting;

/// <summary>
/// Represents a customizable ternary format, allowing you to specify digit symbols, grouping, separators, and padding for formatting trit arrays.
/// </summary>
public class TernaryFormat(ITernaryFormat other) : ITernaryFormat
{
    /// <summary>
    /// Gets a built-in, culture-invariant ternary format with standard digit symbols and grouping.
    /// </summary>
    public static InvariantTernaryFormat Invariant { get; } = new();

    /// <summary>
    /// Gets a built-in minimal ternary format for compact representations.
    /// </summary>
    public static MinimalTernaryFormat Minimal { get; } = new();

    /// <summary>
    /// The currently used ternary format, when not explicitly specified.
    /// </summary>
    public static ITernaryFormat Current { get; set; } = Invariant;

    /// <summary>
    /// Initializes a new instance of the <see cref="TernaryFormat"/> class using the specified format as a template.
    /// </summary>
    /// <param name="other">The format to copy settings from.</param>
    public TernaryFormat() : this(Invariant)
    {
    }

    /// <inheritdoc/>
    public char NegativeTritDigit { get; set; } = other.NegativeTritDigit;

    /// <inheritdoc/>
    public char ZeroTritDigit { get; set; } = other.ZeroTritDigit;

    /// <inheritdoc/>
    public char PositiveTritDigit { get; set; } = other.PositiveTritDigit;

    /// <inheritdoc/>
    public IList<TritGroupDefinition> Groups { get; set; } 
        = new List<TritGroupDefinition>(other.Groups.Select(g=> new TritGroupDefinition(g.Separator, g.Size)));

    /// <inheritdoc/>
    public string DecimalSeparator { get; set; } = other.DecimalSeparator;

    /// <inheritdoc/>
    public TernaryPadding TernaryPadding { get; set; } = other.TernaryPadding;

    /// <summary>
    /// Adds a group definition to the format.
    /// </summary>
    /// <param name="size">The size of the group.</param>
    /// <param name="separator">The separator to use between groups at this level.</param>
    /// <returns>The current <see cref="TernaryFormat"/> instance for chaining.</returns>
    public TernaryFormat WithGroup(int size, string separator)
    {
        Groups.Add(new(separator, size));
        return this;
    }

    /// <summary>
    /// Removes all group definitions from the format.
    /// </summary>
    /// <returns>The current <see cref="TernaryFormat"/> instance for chaining.</returns>
    public TernaryFormat ClearGroups()
    {
        Groups.Clear();
        return this;
    }

    /// <summary>
    /// Previews the format by creating a sample TritArray27 and formatting it.
    /// </summary>
    /// <returns>A string representation of a sample formatted trit array.</returns>
    public override string ToString() => ToString(new(0b000000000_100100100_111000000, 0b000000000_001001001_000000111));

    /// <summary>
    /// Previews the format by creating a sample TritArray27 and formatting it.
    /// </summary>
    /// <returns>A string representation of a sample formatted trit array.</returns>
    public string ToString(TritArray27 trits)
    {
        return trits.ToString("ter");
    }
}