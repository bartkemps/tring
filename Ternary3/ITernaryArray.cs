namespace Ternary3;

/// <summary>
/// Represents a basic interface for all trit array implementations, providing core functionality for working with ternary data.
/// This generic interface enables fluent, strongly-typed operations on trit arrays with proper return type preservation.
/// </summary>
public interface ITritArray
{
    /// <summary>
    /// Gets the value at the specified index.
    /// </summary>
    /// <param name="index">The index of the value to get.</param>
    /// <returns>The trit value at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when index is less than 0 or greater than {{trits - 1}}.</exception>
    Trit this[int index] { get; }

    /// <summary>
    /// Gets the value at the specified index from the end if index is from end.
    /// </summary>
    /// <param name="index">The index of the value to get (supports from-end).</param>
    /// <returns>The trit value at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when index is less than 0 or greater than {{trits - 1}}.</exception>
    Trit this[Index index] { get; }


    /// <summary>
    ///  Gets a sub-array of trits defined by the specified range.
    /// </summary>
    /// <param name="range">The range of indices to get.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when index is less than 0 or greater than {{trits - 1}}.</exception>
    ITritArray this[Range range] { get; }

    /// <summary>
    /// Gets the length of the trit array.
    /// </summary>
    int Length { get; }
}

/// <summary>
/// Represents a strongly-typed trit array that supports equality comparison and string formatting.
/// </summary>
/// <typeparam name="TSelf">The implementing type, enabling proper type preservation in operations.</typeparam>
public interface ITernaryArray<TSelf> 
    : ITritArray, 
        IEquatable<TSelf>,
        IFormattable,
        ITernaryParsable<TSelf>
{
}