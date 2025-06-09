# Ternary3 - Balanced Ternary Number Library

Ternary3 is a comprehensive C# library for working with balanced ternary numbers. Unlike traditional binary computing where digits can be 0 or 1, balanced ternary uses the digits T, 0, and 1 (often denoted as -, 0, and +).

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

### Unary Operations

Unary operations transform a single trit value. Here's how to use them:

```csharp
// Operation on Int3T outputs TritArray3
Int3T input1 = 5; // 1TT in balanced ternary
var output1 = input1 | AbsoluteValue; // TritArray3 111
Console.WriteLine($"Absolute value of {input1} ({(TritArray3)input1}) = {(Int3T)output1} ({output1})"); 
// Absolute value of 5 (1TT) = 13 (111)

// Custom operation on Int9T outputs TritArray9, which can be implicitly converted to Int9T
Trit Echo(Trit trit) => trit; // Custom operation: Identity function for Trit
Int9T input2 = -10; // T0T
Int9T output2 = input2 | Echo; // T0T. Gets implicitly converted to Int9T
Console.WriteLine($"-10 Echoed = {output2}"); // Prints -10

// Using an alias for a Tryte (27-trit integer) and the full name for the operator
using Tryte = Numbers.Int27T;
Tryte input3 = 11; // 11T
long output3 = input3 | Ternary3.Operators.Unary.Negate; // TT1, using the full name for the operator.
Console.WriteLine($"11 Negated = {output3}"); // 11 Negated = -11

// TritArray input with TritArray27
TritArray27 input4 = 123456789;
var output4 = input4 | Negate;
Console.WriteLine($"123456789 Negated = {output4} ({(int)output4})"); 
// Prints 000000000 T0011TT01 T1T010100 (-123456789)

// Single Trit input
var input5 = Trit.Negative; // T (negative)
var output5 = input5 | IsPositive; // No. It is not positive. No translates to T (negative)
Console.WriteLine($"Is {input5} positive? {output5}"); // Is Negative positive? Negative
```

### Unary Lookup Operations

Unary operations can also be implemented using lookup tables:

```csharp
// Using a custom alias for 3-trit integers
using Tribble = Numbers.Int3T;

// Operation using lookup table on Int3T outputs TritArray3
Tribble input1 = 5; // 1TT
var output1 = input1 | UnaryLookup.AbsoluteValue; // TritArray3 111
Console.WriteLine($"Absolute value of {input1} ({(TritArray3)input1}) = {(Tribble)output1} ({output1})");

// Custom operation with lookup array
var echo = new Trit[] { false, null, true }; // Lookup table for identity operation
Int9T input2 = -10; // T0T
Int9T output2 = input2 | echo; // T0T. Gets implicitly converted to Int9T
Console.WriteLine($"-10 Echoed = {output2}"); // Prints -10
```

### Binary Operations with Lookup Tables

Binary operations take two ternary inputs and produce a ternary output:

```csharp
// Built-in AND operation on two sbytes
// In ternary, AND returns the minimum value of each trit pair
// Truth table for AND:
// (-1 AND -1) = -1, (-1 AND 0) = -1, (-1 AND 1) = -1
// ( 0 AND -1) = -1, ( 0 AND 0) =  0, ( 0 AND 1) =  0
// ( 1 AND -1) = -1, ( 1 AND 0) =  0, ( 1 AND 1) =  1
sbyte input1A = 8; // 10T in balanced ternary (where T is -1)
sbyte input1B = 9; // 100 in balanced ternary
TritArray3 result1 = input1A | BinaryLookup.And | input1B; // Uses the predefined AND operation
Console.WriteLine($"BinaryLookup And: {input1A} And {input1B} = {(sbyte)result1} ({result1})"); 
// Output: 8 (10T) because:
// 10T (input1A) AND 100 (input1B) = 10T
// Position by position: 1∧1=1, 0∧0=0, T∧0=T

// Custom operation on two shorts using a TritLookupTable
short input2A = -6; // 000000T10 in balanced ternary
short input2B = 13;  // 000000111 in balanced ternary

// This lookup table defines a "mask" operation:
// - Returns -1 only when both inputs are -1
// - Returns 1 only when both inputs are 1
// - Returns 0 in all other cases
TritLookupTable mask = new([
    [-1, 0, 0],  // Row for when first input is -1 
    [0, 0, 0],   // Row for when first input is 0
    [0, 0, 1]    // Row for when first input is 1
]);
var result2 = input2A | mask | input2B; // Apply the mask operation
Console.WriteLine($"Custom operation on short: {(TritArray9)input2A} mask {(TritArray9)input2B} = {result2}");
// Output: 000000010 - Only positions where both inputs have the same non-zero value are preserved
```

### Overflow Behavior

Ternary numbers in this library behave differently when overflow occurs:

```csharp
// Addition and subtraction may overflow and do so in a ternary way
// (cutting off ternary trits rather than binary bits)
Int3T input1A = 12; // 110
Int3T input1B = 3;  // 010
var result1 = input1A + input1B; // 110 + 010 = 1TT0. Int3T only keeps 3 trits, so TT0 = -2
Console.WriteLine($"Overflow: {input1A} + {input1B} = {result1}"); // Overflow: 12 + 3 = -2

// Multiplication overflow
Int3T input2A = 12; // 110
Int3T input2B = -12; // TT0
var result2 = input2A * input2B; // 110 * TT0 = 101T00 (144). Int3T only keeps 3 trits, so T00 = -9
Console.WriteLine($"Overflow: {input2A} * {input2B} = {result2}"); // Overflow: 12 * -12 = -9

// Shifting trits (multiplies/divides by powers of 3)
var input6 = TritArray9.MaxValue; // 111111111
var result6A = input6 << 6;
Console.WriteLine($"Shift: {input6} << 6 = {result6A} ({(int)result6A})"); 
// Shift: 111111111 << 6 = 111000000 (9477)

var input7 = Int27T.MinValue; // TTTTTTTTT TTTTTTTTT TTTTTTTTT
var result7C = input7 << -25;
Console.WriteLine($"Shift: {(TritArray27)input7} << -25 = {result7C} ({(TritArray27)result7C})"); 
// Shift: TTTT..TT << -25 = 0000...TT (-4)
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

### Clean Code with Static Imports and Constants

Using static imports and constants can make ternary code significantly more readable. Here's a complete example demonstrating this practice:

```csharp
using System;
using Ternary3.Numbers;
using Ternary3.Operators;

// Static imports for commonly used operations - choose ONE implementation approach
using static Ternary3.Operators.Unary;  // Function-based implementation
// using static Ternary3.Operators.UnaryLookup;  // Lookup table-based implementation (alternative)
using static Ternary3.Operators.BinaryLookup;

namespace Ternary3.Examples
{
    public static class CleanCodeExample
    {
        // Common aliases to make code more expressive
        using Tribble = Numbers.Int3T;
        using Tryte = Numbers.Int27T;
        
        // Constants for common trit values improve readability
        private const int T = -1;  // Negative trit
        private const int O = 0;   // Zero trit
        private const int P = 1;   // Positive trit
        
        public static void Run()
        {
            Console.WriteLine("=== Clean Code Examples with Ternary3 ===");
            
            // Example 1: Working with individual trits
            DemonstrateTrits();
            
            // Example 2: Working with small ternary integers (3 trits)
            DemonstrateSmallIntegers();
            
            // Example 3: Working with custom lookups
            DemonstrateCustomLookups();
            
            // Example 4: Creating a ternary calculator
            DemonstrateCalculator();
        }
        
        private static void DemonstrateTrits()
        {
            Console.WriteLine("\n[Trit Operations]");
            
            // Thanks to static imports, we can use operations directly
            var positive = Trit.Positive;
            var negative = Trit.Negative;
            
            // Operations read more naturally
            var posNegated = positive | Negate;           // T
            var posAbsValue = positive | AbsoluteValue;   // 1
            var negAbsValue = negative | AbsoluteValue;   // 1
            
            Console.WriteLine($"Positive trit: {positive}");
            Console.WriteLine($"Negative trit: {negative}");
            Console.WriteLine($"Positive negated: {posNegated}");
            Console.WriteLine($"Abs value of positive: {posAbsValue}");
            Console.WriteLine($"Abs value of negative: {negAbsValue}");
            
            // Truth value operations
            var truthValue = positive | Or | negative;
            Console.WriteLine($"Positive OR Negative = {truthValue}");
        }
        
        private static void DemonstrateSmallIntegers()
        {
            Console.WriteLine("\n[Small Ternary Integer Operations]");
            
            // Using the Tribble alias (3-trit integer)
            Tribble a = 10;   // 101 in balanced ternary
            Tribble b = -8;   // T01 in balanced ternary
            
            Console.WriteLine($"a = {a} ({(TritArray3)a})");
            Console.WriteLine($"b = {b} ({(TritArray3)b})");
            
            // Arithmetic operations
            var sum = a + b;
            var product = a * b;
            
            Console.WriteLine($"a + b = {sum} ({(TritArray3)sum})");
            Console.WriteLine($"a * b = {product} ({(TritArray3)product})");
            
            // Logic operations - note the clean syntax
            var absA = a | AbsoluteValue;
            var absB = b | AbsoluteValue;
            
            Console.WriteLine($"|a| = {absA} ({(TritArray3)absA})");
            Console.WriteLine($"|b| = {absB} ({(TritArray3)absB})");
            
            // Binary operation (similar to AND)
            var min = a | Min | b;
            Console.WriteLine($"Min(a,b) = {min} ({(TritArray3)min})");
        }
        
        private static void DemonstrateCustomLookups()
        {
            Console.WriteLine("\n[Custom Lookup Tables]");
            
            // Define a custom binary operation using a lookup table
            // This is a "selective copy" operation that:
            // - Keeps the first operand's value if the second is positive
            // - Returns zero if the second is zero
            // - Returns negative of first if the second is negative
            var selectiveCopy = new TritLookupTable(
                // When first input is T (-1):
                new[] { Trit.Negative, Trit.Zero, Trit.Positive },
                // When first input is 0:
                new[] { Trit.Zero, Trit.Zero, Trit.Zero },
                // When first input is 1:
                new[] { Trit.Negative, Trit.Zero, Trit.Positive }
            );
            
            // Alternative initialization using constants
            var anotherLookup = new TritLookupTable(
                T, O, P,  // When first trit is T
                O, O, O,  // When first trit is 0
                T, O, P   // When first trit is P
            );
            
            Int9T value = 42;    // 1110 in balanced ternary
            Int9T mask = 13;     // 111 in balanced ternary
            
            var result = value | selectiveCopy | mask;
            Console.WriteLine($"SelectiveCopy({value}, {mask}) = {result} ({(TritArray9)result})");
            
            // Test with different mask values
            Int9T zeroMask = 0;
            Int9T negMask = -13;
            
            Console.WriteLine($"SelectiveCopy({value}, {zeroMask}) = {value | selectiveCopy | zeroMask}");
            Console.WriteLine($"SelectiveCopy({value}, {negMask}) = {value | selectiveCopy | negMask}");
        }
        
        private static void DemonstrateCalculator()
        {
            Console.WriteLine("\n[Ternary Calculator]");
            
            // Using the Tryte alias (27-trit integer)
            Tryte x = 42;
            Tryte y = -27;
            
            // Mini calculator that supports basic operations
            var calculator = new TernaryCalculator();
            
            Console.WriteLine($"x = {x}, y = {y}");
            Console.WriteLine($"x + y = {calculator.Add(x, y)}");
            Console.WriteLine($"x - y = {calculator.Subtract(x, y)}");
            Console.WriteLine($"x * y = {calculator.Multiply(x, y)}");
            Console.WriteLine($"x / y = {calculator.Divide(x, y)}");
            Console.WriteLine($"x % y = {calculator.Mod(x, y)}");
            Console.WriteLine($"|x| = {calculator.Abs(x)}");
            Console.WriteLine($"-x = {calculator.Negate(x)}");
        }
    }
    
    // Example utility class demonstrating how to encapsulate ternary operations
    public class TernaryCalculator
    {
        // Using static imports keeps the implementation clean
        using static Operators.Unary;
        
        public Int27T Add(Int27T a, Int27T b) => a + b;
        public Int27T Subtract(Int27T a, Int27T b) => a - b;
        public Int27T Multiply(Int27T a, Int27T b) => a * b;
        public Int27T Divide(Int27T a, Int27T b) => a / b;
        public Int27T Mod(Int27T a, Int27T b) => a % b;
        
        // Operations using ternary logic operators
        public Int27T Abs(Int27T value) => value | AbsoluteValue;
        public Int27T Negate(Int27T value) => value | Operators.Unary.Negate;
        
        // Example of a more complex operation
        public Int27T Clamp(Int27T value, Int27T min, Int27T max)
        {
            // If value < min, return min
            // If value > max, return max
            // Otherwise return value
            var lessThanMin = value < min;
            var greaterThanMax = value > max;
            
            if (lessThanMin)
                return min;
            if (greaterThanMax)
                return max;
            return value;
        }
    }
}
```
