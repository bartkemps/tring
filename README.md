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
