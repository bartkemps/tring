namespace Ternary3.Formatting
{
    /// <summary>
    /// Provides formatting options for representing arrays of trits as strings, including digit symbols, grouping, separators, and padding.
    /// </summary>
    public interface ITernaryFormat
    {
        /// <summary>
        /// Gets the character used to represent a negative trit (-1).
        /// </summary>
        char NegativeTritDigit { get; }
        /// <summary>
        /// Gets the character used to represent a zero trit (0).
        /// </summary>
        char ZeroTritDigit { get; }
        /// <summary>
        /// Gets the character used to represent a positive trit (+1).
        /// </summary>
        char PositiveTritDigit { get; }
        /// <summary>
        /// Gets the list of group definitions, each specifying a separator and group size for hierarchical grouping.
        /// </summary>
        IList<TritGroupDefinition> Groups { get; }
        /// <summary>
        /// Gets the string used as a decimal separator (for future floating-point trit support).
        /// </summary>
        string DecimalSeparator { get; }
        /// <summary>
        /// Gets the padding mode for the formatted ternary string.
        /// </summary>
        TernaryPadding TernaryPadding { get; }
    }

    /// <summary>
    /// Defines a group for hierarchical trit formatting, specifying the separator and group size.
    /// </summary>
    /// <param name="separator">The separator string for this group level.</param>
    /// <param name="size">The number of items in this group.</param>
    public class TritGroupDefinition(string separator = " ", int size = 3)
    {
        /// <summary>
        /// Gets or sets the separator string for this group level.
        /// </summary>
        public string Separator { get; set; } = separator;
        /// <summary>
        /// Gets or sets the number of items in this group.
        /// </summary>
        public int Size { get; set; } = size;
    }
}