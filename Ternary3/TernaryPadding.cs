namespace Ternary3;

/// <summary>
/// Indicates how padding should be applied to the formatted ternary string.
/// </summary>
public enum TernaryPadding
{
    /// <summary>
    /// No padding is applied.
    /// </summary>
    None,
    /// <summary>
    /// Padding is applied to fill the last group.
    /// </summary>
    Group,
    /// <summary>
    /// Padding is applied to fill the entire formatted string to a fixed width.
    /// </summary>
    Full
}