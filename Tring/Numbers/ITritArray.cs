namespace Tring.Numbers;

public interface ITritArray
{
    /// <summary>
    /// Gets the value at the specified index.
    /// </summary>
    /// <param name="index">The index of the value to get.</param>
    /// <returns>The trit value at the specified index.</returns>
    Trit this[int index] { get; set; }

    /// <summary>
    /// Gets the length of the trit array.
    /// </summary>
    int Length { get; }
}