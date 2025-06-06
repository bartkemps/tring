# Tring - Balanced Ternary Number Library

Tring is a comprehensive C# library for working with balanced ternary numbers. Unlike traditional binary computing where digits can be 0 or 1, balanced ternary uses the digits T, 0, and 1 (often denoted as -, 0, and +).

## What are Balanced Ternary Numbers?

Balanced ternary is a non-standard positional numeral system, using three as the base with the digits T, 0, and 1 (where T represents -1). 

For example, the decimal number 8 is represented as `1 0 T 0` in balanced ternary:
```
(1 × 3³) + (0 × 3²) + (T × 3¹) + (0 × 3⁰) = 27 + 0 - 3 + 0 = 24
```

This representation offers certain mathematical advantages:
- The negative of any number is obtained by simply inverting each digit (replacing 1 with T and vice versa)
- Rounding to the nearest integer is achieved by truncation
- The system can represent negative numbers without a separate sign bit

## Mathematical Richness of Ternary Logic

In binary logic, there are 2² = 4 possible unary operations (operations on one input):
1. Always output 0
2. Pass the input unchanged
3. Negate the input (flip)
4. Always output 1

And 2⁴ = 16 possible binary operations (operations on two inputs), including familiar ones like AND, OR, XOR, etc.

### Binary Logic Tables

For comparison, here's the standard binary AND operation truth table:

| AND | **0** | **1** |
|-----|-------|-------|
| **0** |  0   |   0   |
| **1** |  0   |   1   |

### Ternary Logic

In ternary logic, there are 3³ = 27 possible unary operations and 3⁹ = 19,683 possible binary operations! Here are some important examples:

#### Unary Operations

Some key unary operations in balanced ternary:

1. **Identity** (keeps the value unchanged):
```
T → T
0 → 0
1 → 1
```

2. **Invert** (flips positive to negative and vice versa):
```
T → 1
0 → 0
1 → T
```

3. **Absolute Value**:
```
T → 1
0 → 0
1 → 1
```

#### Binary Operations

Here are some fundamental binary operations in balanced ternary:

1. **AND** operation (similar to minimum):

| AND | **T** | **0** | **1** |
|-----|-------|-------|-------|
| **T** |  T   |   T   |   T   |
| **0** |  T   |   0   |   0   |
| **1** |  T   |   0   |   1   |

2. **OR** operation (similar to maximum):

| OR  | **T** | **0** | **1** |
|-----|-------|-------|-------|
| **T** |  T   |   T   |   1   |
| **0** |  T   |   0   |   1   |
| **1** |  1   |   1   |   1   |

3. **XOR** operation:

| XOR | **T** | **0** | **1** |
|-----|-------|-------|-------|
| **T** |  T   |   0   |   1   |
| **0** |  0   |   0   |   0   |
| **1** |  1   |   0   |   T   |

## Code Examples

### Basic Usage

Here's how to work with ternary numbers in Tring:

```csharp
// Creating a trit (ternary digit)
var positiveOne = new Trit(1);    // Represents 1
var zero = new Trit(0);           // Represents 0
var negativeOne = new Trit(-1);   // Represents T (or -1)

// Converting from boolean
var fromTrue = (Trit)true;        // Converts to 1
var fromFalse = (Trit)false;      // Converts to T
var fromNull = (Trit)(bool?)null; // Converts to 0

// Working with larger numbers (20 trits)
var number = new Int20T(42);      // Decimal 42 in balanced ternary
var negated = -number;            // Negating is simple!
var doubled = number + number;     // Arithmetic operations
```

### Logical Operations

```csharp
// Unary operations
var trit = new Trit(1);
var inverted = Unary.Invert(trit);           // T
var absolute = Unary.AbsoluteValue(trit);    // 1
var incremented = Unary.Increment(trit);     // 1 (saturates at 1)

// Binary operations
var a = new Trit(1);
var b = new Trit(-1);
var andResult = Binary.And[a, b];           // T
var orResult = Binary.Or[a, b];            // 1
var xorResult = Binary.Xor[a, b];          // 1
```

## Performance and Implementation

Tring implements balanced ternary numbers efficiently using standard binary hardware. For example, Int20T uses a 32-bit integer internally to store 20 trits, with careful bit manipulation to perform operations correctly and efficiently.

## Implemented Operations

### Unary Operations

Tring implements all 27 possible unary operations in balanced ternary logic. Some of the most commonly used operations include:

1. **Identity** - Returns the original trit value unchanged
2. **Invert/Negate** - Flips positive to negative and vice versa (T→1, 0→0, 1→T)
3. **Absolute Value** - Returns the absolute value of a trit (T→1, 0→0, 1→1)
4. **Zero** - Always returns zero regardless of input
5. **Positive/Negative** - Always returns 1 or T respectively
6. **Increment/Decrement** - Increases or decreases the trit value (with saturation)
7. **IsPositive/IsNegative/IsZero** - Returns 1 if the condition is true, otherwise T

### Binary Operations

Tring implements key binary operations from the 19,683 possibilities in balanced ternary logic:

1. **AND** - Similar to the minimum function (T,1→T)
2. **OR** - Similar to the maximum function (T,1→1)
3. **XOR** - Exclusive OR with interesting balanced properties
4. **CONSENSUS** - Returns the consensus value if any, otherwise T
5. **ACCEPT** - Returns first value if second is positive, otherwise T
6. **Addition** - Balanced ternary addition with proper carry
7. **Subtraction** - Balanced ternary subtraction
8. **Multiplication** - Balanced ternary multiplication

## Library Components

### Trit

The fundamental unit of balanced ternary logic is the `Trit`, which can have values of -1 (T), 0, or 1.

```csharp
// Ways to create Trits
var positiveOne = new Trit(1);             // Directly from integer
var zero = Trit.Zero;                      // Using static property
var negativeOne = Trit.Negative;           // Using static property

// Converting from boolean
var fromTrue = (Trit)true;                 // Converts to 1
var fromFalse = (Trit)false;               // Converts to T (negative)
var fromNull = (Trit)(bool?)null;          // Converts to 0
```

### IntT Types

Tring provides multiple balanced ternary integer implementations optimized for different sizes:

- **Int3T** - 3-trit integer (-13 to 13)
- **Int5T** - 5-trit integer (-121 to 121)
- **Int9T** - 9-trit integer (-9,841 to 9,841)
- **Int10T** - 10-trit integer (-29,524 to 29,524)
- **Int20T** - 20-trit integer (approximately -3.5 billion to 3.5 billion)
- **Int27T** - 27-trit integer (large range suitable for most applications)
- **Int40T** - 40-trit integer (extremely large range exceeding standard 64-bit integers)

Each IntT type supports full arithmetic operations, bit manipulation, and logical operations:

```csharp
// Working with ternary integers
var number = new Int20T(42);            // Decimal 42 in balanced ternary
var negated = -number;                  // Negation (flips all trits)
var doubled = number + number;          // Addition
var halved = number / new Int20T(2);    // Division
var remainder = number % new Int20T(3); // Modulus
```

### TritArray27

The `TritArray27` class represents a fixed-size array of 27 trits, optimized for performance using bitwise operations. It provides:

- Individual trit access and manipulation
- Bulk operations on all trits simultaneously
- Support for the pipe operator for applying operations
- Efficient storage using packed bits

```csharp
// Creating and working with TritArray27
var array = new TritArray27();
array.SetTrit(0, Trit.Positive);
array.SetTrit(1, Trit.Zero);

// Getting individual trits
var firstTrit = array.GetTrit(0);
```

## The Pipe Operator

Tring features a powerful pipe operator `|` that enables functional-style operation application. The pipe operator can be used with:

1. **Delegate Functions** - Apply custom operations
2. **Method References** - Call existing operations
3. **Arrays** - Use lookup tables for operations
4. **TritLookupTable** - Efficiently apply predefined operations

### Using the Pipe Operator

```csharp
// With delegates
var result1 = trit | (t => Trit.Invert(t));  // Apply unary inversion

// With delegate binary operation
var result2 = trit1 | ((a, b) => a.Value > 0 ? b : Trit.Zero) | trit2;

// With TritLookupTable (most efficient)
var andTable = new TritLookupTable(
    Trit.Negative, Trit.Negative, Trit.Negative,
    Trit.Negative, Trit.Zero, Trit.Zero,
    Trit.Negative, Trit.Zero, Trit.Positive
);
var result3 = trit1 | andTable | trit2;

// Alternative constructor with bool/null values
var orTable = new TritLookupTable(
    false, false, true,  // Row for T (negative)
    false, null, true,   // Row for 0
    true, true, true     // Row for 1 (positive)
);
var result4 = trit1 | orTable | trit2;

// With TritArray27
var arrayResult = tritArray | (t => Trit.Invert(t));  // Apply to every trit
```

The pipe operator creates a more readable and functional approach to applying operations, allowing for elegant chaining of operations and promoting code reusability.
