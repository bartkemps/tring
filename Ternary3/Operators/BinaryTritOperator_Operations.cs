namespace Ternary3.Operators;

/// <summary>
/// Provides binary operations for ternary logic, implementing standard three-valued logic operations.
/// </summary>
/// <remarks>
/// This class implements the standard Kleene and Priest logics for three-valued logic operations.
/// For more information, see <see href="https://en.wikipedia.org/wiki/Three-valued_logic"/>.
/// </remarks>
public partial struct BinaryTritOperator
{
    // note: in the BinaryTritOperator, the trit encoding is different for some other places:
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
    public static readonly BinaryTritOperator Positive = new(0,0b111111111);

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
    public static readonly BinaryTritOperator Zero = new(0,0);

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
    public static readonly BinaryTritOperator Negative = new(0b111111111, 0);

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
    public static readonly BinaryTritOperator And = new(0b001001111,0b100000000);

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
    public static readonly BinaryTritOperator Or = new(0b000001011, 0b111100100);

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
    public static readonly BinaryTritOperator Xor = new(0b100000001, 0b001000100);
    
    /// <summary>
    /// Implements the standard MULTIPLY operation in three-valued logic (Kleene/Priest logic).
    /// </summary>
    /// <remarks>
    /// Truth table representation:
    /// <code>
    ///  × | T 0 1
    /// ---+------
    ///  T | 1 0 T
    ///  0 | 0 0 0
    ///  1 | T 0 1
    /// </code>
    /// </remarks>
    public static readonly BinaryTritOperator Multiply = new(0b001000100, 0b100000001);

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
    public static readonly BinaryTritOperator Plus = new(0b000001011,0b110100000);

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
    public static readonly BinaryTritOperator Minus = new(0b000100110,0b011001000);

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
    public static readonly BinaryTritOperator Implicates = new(0b001000000,0b100100111);

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
    public static readonly BinaryTritOperator Is = new(0b011101110,0b100010001);
    
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
    public static readonly BinaryTritOperator GreaterThan = new(0b111011001,0b000100110);
    
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
    public static readonly BinaryTritOperator LesserThan = new(0b000100110,0b111011001);
}