namespace Ternary3;

public interface ITritArray
{
    /// <summary>
    /// Gets the value at the specified index.
    /// </summary>
    /// <param name="index">The index of the value to get.</param>
    /// <returns>The trit value at the specified index.</returns>
    Trit this[int index] { get; }

    /// <summary>
    /// Gets the value at the specified index from the end if index is from end.
    /// </summary>
    /// <param name="index">The index of the value to get (supports from-end).</param>
    /// <returns>The trit value at the specified index.</returns>
    Trit this[Index index] { get; }

    /// <summary>
    /// Gets the length of the trit array.
    /// </summary>
    int Length { get; }
}

public interface ITritArray<T> 
    : ITritArray, IEquatable<T>, IFormattable
    where T : struct 
{
}