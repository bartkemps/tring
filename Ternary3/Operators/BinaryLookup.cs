namespace Ternary3.Operators;

/// <summary>
/// Provides binary operations for ternary logic, implementing standard three-valued logic operations.
/// </summary>
/// <remarks>
/// This class implements the standard Kleene and Priest logics for three-valued logic operations.
/// For more information, see <see href="https://en.wikipedia.org/wiki/Three-valued_logic"/>.
/// </remarks>
public class BinaryLookup
{
    // note: in the TritLookupTable, the trit encoding is different for some other places:
    // 00 = Negative, 01 = Zero, 10 = Positive

    /// <summary>
    /// Represents a constant that returns Positive (1) for any trit combination.
    /// </summary>
    /// <remarks>
    /// Truth table representation:
    /// <code>
    ///  1 | T 0 1
    /// ---+------
    ///  T | 1 1 1
    ///  0 | 1 1 1
    ///  1 | 1 1 1
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable Positive = new(0b101010_101010_101010);

    /// <summary>
    /// Represents a constant that returns Zero (0) for any trit combination.
    /// </summary>
    /// <remarks>
    /// Truth table representation:
    /// <code>
    ///  0 | T 0 1
    /// ---+------
    ///  T | 0 0 0
    ///  0 | 0 0 0
    ///  1 | 0 0 0
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable Zero = new(0b010101_010101_010101);

    /// <summary>
    /// Represents a constant that returns Negative (-1) for any trit combination.
    /// </summary>
    /// <remarks>
    /// Truth table representation:
    /// <code>
    /// -1 | T 0 1
    /// ---+------
    ///  T | T T T
    ///  0 | T T T
    ///  1 | T T T
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable Negative = new(0b000000_000000_000000);

    /// <summary>
    /// Implements the standard AND operation in three-valued logic (Kleene/Priest logic).
    /// </summary>
    /// <remarks>
    /// Truth table representation:
    /// <code>
    /// AND| T 0 1
    /// ---+------
    ///  T | T T T
    ///  0 | T 0 0
    ///  1 | T 0 1
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable And = new(0b100100_010100_000000);

    /// <summary>
    /// Implements the standard OR operation in three-valued logic (Kleene/Priest logic).
    /// </summary>
    /// <remarks>
    /// Truth table representation:
    /// <code>
    /// OR | T 0 1
    /// ---+------
    ///  T | T T 1
    ///  0 | T 0 1
    ///  1 | 1 1 1
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable Or = new(0b101010_100100_100000);

    /// <summary>
    /// Implements the standard XOR operation in three-valued logic (Kleene/Priest logic).
    /// </summary>
    /// <remarks>
    /// Truth table representation:
    /// <code>
    /// xor| T 0 1
    /// ---+------
    ///  T | T 0 1
    ///  0 | 0 0 0
    ///  1 | 1 0 T
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable Xor = new(0b000110_000000_100100);

    /// <summary>
    /// Implements the standard Plus operation in three-valued logic
    /// </summary>
    /// <remarks>
    /// Truth table representation:
    /// <code>
    ///  + | T 0 1
    /// ---+------
    ///  T | T T 0
    ///  0 | T 0 1
    ///  1 | 0 1 1
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable Plus = new(0b101001_100100_010000);

    /// <summary>
    /// Implements the standard Minus operation in three-valued logic.
    /// </summary>
    /// <remarks>
    /// Truth table representation:
    /// <code>
    ///  - | T 0 1
    /// ---+------
    ///  T | 0 T T
    ///  0 | 1 0 T
    ///  1 | 1 1 0
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable Minus = new(0b011010_000110_000001);

    /// <summary>
    /// ==&gt; The First implies the Second.
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
    public static readonly TritLookupTable Implicates = new(0b100100_100101_101010);

    /// <summary>
    /// Are the two trits equal?
    /// </summary>
    /// <remarks>
    /// <code>
    /// ==&gt;| T 0 1
    /// ---+------
    ///  T | 1 T T
    ///  0 | T 1 T
    ///  1 | T T 1
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable Is = new(0b100000_001000_000010);
    
    /// <summary>
    /// Is the first trit greater than the second?
    /// </summary>
    /// <remarks>
    /// <code>
    /// gt | T 0 1
    /// ---+------
    ///  T | T 1 1
    ///  0 | T T 1
    ///  1 | T T T
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable GreaterThan = new(0b000000_100000_101000);
    
    /// <summary>
    /// Is the first trit greater than the second?
    /// </summary>
    /// <remarks>
    /// <code>
    /// gt | T 0 1
    /// ---+------
    ///  T | 1 T T
    ///  0 | 1 1 T
    ///  1 | 1 1 1
    /// </code>
    /// </remarks>
    public static readonly TritLookupTable LesserThan = new(0b101010_001010_000010);
}