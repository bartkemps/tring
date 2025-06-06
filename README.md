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

1. **Min** operation (similar to AND):

| Min | **T** | **0** | **1** |
|-----|-------|-------|-------|
| **T** |  T   |   T   |   T   |
| **0** |  T   |   0   |   0   |
| **1** |  T   |   0   |   1   |

2. **Max** operation (similar to OR):

| Max | **T** | **0** | **1** |
|-----|-------|-------|-------|
| **T** |  T   |   0   |   1   |
| **0** |  0   |   0   |   1   |
| **1** |  1   |   1   |   1   |

3. **Sum** operation (similar to XOR):

| Sum | **T** | **0** | **1** |
|-----|-------|-------|-------|
| **T** |  T   |   T   |   0   |
| **0** |  T   |   0   |   1   |
| **1** |  0   |   1   |   1   |

## Complete List of Unary Operations

In ternary logic, there are 3³ = 27 possible unary operations. Here's a complete list with their names and truth tables:

| Operation Name      | T    | 0    | 1    | Description |
|---------------------|------|------|------|-------------|
| Negative            | T    | T    | T    | Always outputs T (negative) |
| Decrement           | T    | T    | 0    | One less for every value greater than negative |
| IsPositive          | T    | T    | 1    | Positive for positive, negative otherwise |
| NegateAbsoluteValue | T    | 0    | T    | Zero for zero, negative otherwise |
| Ceil                | T    | 0    | 0    | Negative for negative, zero otherwise |
| Identity            | T    | 0    | 1    | Keep the value unchanged |
| IsZero              | T    | 1    | T    | Positive for zero, negative otherwise |
| KeepNegative        | T    | 1    | 0    | Keep negative unchanged, zero for positive and vice versa |
| IsNotNegative       | T    | 1    | 1    | Negative for negative, positive otherwise |
| CeilIsNegative      | 0    | T    | T    | Zero for negative, negative otherwise |
| CeilIsNotZero       | 0    | T    | 0    | Zero for non-zero values, negative for zero |
| KeepPositive        | 0    | T    | 1    | Positive for positive, negative otherwise |
| CeilIsNotPositive   | 0    | 0    | T    | Negative for positive, zero otherwise |
| Zero                | 0    | 0    | 0    | Always outputs 0 |
| Floor               | 0    | 0    | 1    | Positive for positive, zero otherwise |
| CyclicIncrement     | 0    | 1    | T    | Cycles values: T→0→1→T |
| FloorIsZero         | 0    | 1    | 0    | Zero for zero, positive otherwise |
| Increment           | 0    | 1    | 1    | Zero for negative, positive otherwise |
| IsNegative          | 1    | T    | T    | Positive for negative, negative otherwise |
| CyclicDecrement     | 1    | T    | 0    | Cycles values: 1→0→T→1 |
| IsNotZero           | 1    | T    | 1    | Positive for non-zero values, negative for zero |
| Negate              | 1    | 0    | T    | Flips positive to negative and vice versa |
| FloorIsNegative     | 1    | 0    | 0    | Positive for negative, zero otherwise |
| AbsoluteValue       | 1    | 0    | 1    | Convert negative to positive, keep others |
| IsNotPositive       | 1    | 1    | T    | Positive for non-positive values, negative for positive |
| FloorIsNotPositive  | 1    | 1    | 0    | Positive for non-positive values, zero for positive |
| Positive            | 1    | 1    | 1    | Always outputs 1 (positive) |

## Ternary Integer and Array Types

This library provides several core types for working with balanced ternary numbers:

### Integer Types

- **Int3T** (commonly named a Trybble)
- **Int9T**
- **Int27T** (commonly named a Tryte)

These types are optimized for common arithmetic operations such as addition, multiplication, and trit shifting. They represent signed integers using 3, 9, or 27 trits, respectively. Each type provides efficient implementations for arithmetic and bitwise operations in balanced ternary. They can be cast to their corresponding array types for ternary logic operations.

### Array Types

- **TritArray3**
- **TritArray9**
- **TritArray27**

These types represent fixed-size arrays of 3, 9, or 27 trits (ternary digits). They are optimized for ternary logic operations, such as applying unary or binary ternary logic operators across all trits. These types are especially useful for implementing ternary logic circuits or algorithms that require direct manipulation of trit values. They can be implicitly cast to and from their corresponding integer types.

### Relationship and Casting

- The integer types (**Int3T**, **Int9T**, **Int27T**) are optimized for arithmetic and shifting, while the array types (**TritArray3**, **TritArray9**, **TritArray27**) are optimized for ternary logic operations.
- You can cast an integer type to its array counterpart to perform logic operations, and vice versa. The conversion is implicit, making it easy to switch between arithmetic and logic representations as needed.

## Code Examples

### Basic Usage

Here's how to work with ternary numbers in Tring:

```csharp
// Creating ternary integers
Int27T number = 42;
Int27T negativeNumber = -15;

// Creating and manipulating trit arrays
TritArray27 array = new TritArray27();
array[0] = Trit.Positive; // Set the first trit to positive (1)
array[1] = Trit.Negative; // Set the second trit to negative (T)
array[2] = false;         // Set the third trit to zero (0)

// Applying unary operations
Trit trit = Trit.Positive;
Trit inverted = trit | Trit.Negate; // Apply unary negate operation
Trit absolute = trit | Trit.AbsoluteValue; // Get absolute value

// Manipulating ternary integers
Int27T result = number + negativeNumber; // Addition
Int27T product = number * 3;            // Multiplication
Int27T quotient = number / negativeNumber; // Division
```

### Advanced Operations

```csharp
// Working with Int27T shifts
Int27T x = 42;
Int27T leftShifted = x << 2;  // Shift left by 2 trits
Int27T rightShifted = x >> 1; // Shift right by 1 trit

// Working with TritArray27 shifts
TritArray27 array = new TritArray27();
// Set some initial values
array[0] = Trit.Positive;
array[1] = Trit.Negative;
array[2] = Trit.Zero;

// Shift operations on trit arrays
TritArray27 leftShiftedArray = array << 2;  // Shift left by 2 positions
TritArray27 rightShiftedArray = array >> 1; // Shift right by 1 position
```
