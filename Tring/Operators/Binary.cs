namespace Tring.Operators;

/// <summary>
/// Some binary operations on trits.
/// <see href="https://en.wikipedia.org/wiki/Three-valued_logic"/>
/// </summary>
public class Binary
{
    // note: in the TritLookupTable, the trit encoding is different for some other places:
    // 00 = Negative, 01 = Zero, 10 = Positive
    
    /// <summary>
    /// Positive for any trit combination.
    /// </summary>
    /// <remarks>
    /// <code>
    ///  1 | T 0 1
    /// ---+------
    ///  T | 1 1 1
    ///  0 | 1 1 1
    ///  1 | 1 1 1
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable Positive = new TritLookupTable(0b101010_101010_101010);

    /// <summary>
    /// Zero for any trit combination.
    /// </summary>
    /// <remarks>
    /// <code>
    ///  0 | T 0 1
    /// ---+------
    ///  T | 0 0 0
    ///  0 | 0 0 0
    ///  1 | 0 0 0
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable Zero = new TritLookupTable(0b010101_010101_010101);

    /// <summary>
    /// Negative for any trit combination.
    /// </summary>
    /// <remarks>
    /// <code>
    ///  T | T 0 1
    /// ---+------
    ///  T | T T T
    ///  0 | T T T
    ///  1 | T T T
    /// </code>
   /// </remarks>
    public static readonly TritLookupTable Negative = new TritLookupTable();

    /// <summary>
    /// Plus for any trit combination.
    /// </summary>
    /// <remarks>
    /// <code>
    /// pls| T 0 1
    /// ---+------
    ///  T | T T 0
    ///  0 | T 0 1
    ///  1 | 0 1 1
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable Plus = new TritLookupTable(0b000001_000110_011010);

    /// <summary>
    /// Minus for any trit combination.
    /// </summary>
    /// <remarks>
    /// <code>
    /// min| T 0 1
    /// ---+------
    ///  T | 0 T T
    ///  0 | 1 0 T
    ///  1 | 1 1 0
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable Minus = new TritLookupTable(0b010000_100001_101000);

    /// <summary>
    /// AND for any trit combination. (also known as "floor")
    /// </summary>
    /// <remarks>
    /// <code>
    /// and| T 0 1
    /// ---+------
    ///  T | T T T
    ///  0 | T 0 0
    ///  1 | T 0 1
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable And = new TritLookupTable(0b000000_000101_000110);

    /// <summary>
    /// OR for any trit combination. (also known as "ceil")
    /// </summary>
    /// <remarks>
    /// <code>
    /// or | T 0 1
    /// ---+------
    ///  T | T 0 1
    ///  0 | 0 0 1
    ///  1 | 1 1 1
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable Or = new TritLookupTable(0b000110_010110_101010);

    /// <summary>
    /// XOR for any trit combination.
    /// </summary>
    /// <remarks>
    /// <code>
    /// xor| T 0 1
    /// ---+------
    ///  T | T 0 1
    ///  0 | 0 0 0
    ///  1 | 1 0 T
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable Xor = new TritLookupTable(0b000110_010101_100100);
    
    /// <summary>
    /// ==&gt; for any trit combination.
    /// </summary>
    /// <remarks>
    /// <code>
    /// ==&gt;| T 0 1
    /// ---+------
    ///  T | 1 1 1
    ///  0 | 0 0 1
    ///  1 | T 0 1
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable Implicates = new TritLookupTable(0b101010_010110_000110);
}