namespace Ternary3;

using Formatting;
using TritArrays;

public interface ITritArray
{
    /// <summary>
    /// Gets the value at the specified index.
    /// </summary>
    /// <param name="index">The index of the value to get.</param>
    /// <returns>The trit value at the specified index.</returns>
    Trit this[int index] { get; }

    /// <summary>
    /// Gets the length of the trit array.
    /// </summary>
    int Length { get; }
    
    /// <summary>
    /// Converts the trit array to a string representation using the default format.
    /// </summary>
    public string ToString() => ToString(TernaryFormat.Invariant);

    /// <summary>
    /// Converts the trit array to a string representation using the specified format.
    /// </summary>
    public string ToString(ITernaryFormat format) => Formatter.Format(this, format);
}

public interface ITritArray<T> 
    : ITritArray, IEquatable<T>
    where T : struct 
{
}