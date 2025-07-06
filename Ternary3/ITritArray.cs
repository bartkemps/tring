namespace Ternary3;

using System.Numerics;

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

public interface ITritArray<TSelf> 
    : ITritArray, 
        IEquatable<TSelf>,
        IFormattable
{
}