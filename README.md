# Ternary3 - C# Balanced Ternary Number Library

## Basics

### What is Ternary3?

Ternary3 is a specialized C# library that implements balanced ternary arithmetic. While most computer systems use binary (base-2) arithmetic, this library enables computation in balanced ternary (base-3) with digits {-1, 0, 1}, often represented as {T, 0, 1}.

### Why Balanced Ternary?

Balanced ternary has several interesting properties that make it unique among number systems:

- It's the most efficient number system for representing numbers (using the minimum number of digits)
- It represents negative numbers naturally without special notation
- It simplifies many arithmetic operations (no separate rules for handling negative numbers)
- It has elegant rounding properties - floor and ceiling operations are simple digit truncations
- A balanced ternary computer was actually built in Russia in the 1950s (the Setun)

### Documentation

For more detailed documentation and mathematical background:
- [Library API Documentation](#Reference)
- [Wikipedia: Balanced Ternary](https://en.wikipedia.org/wiki/Balanced_ternary)
- [Source code repository](https://github.com/bartkemps/tring)

## Getting Started

### Installation

Install the package from NuGet:

```bash
dotnet add package Ternary
```

Or search for "Ternary" in the NuGet package manager in Visual Studio.

Package URL: https://www.nuget.org/packages/Ternary

Write a simple C# program to test the library:

```csharp
using static Ternary3.Operators.BinaryTritOperator;
using static System.Console;

WriteLine(-24 | Or | 4);
```

This will output a ternary number representing the result of the operation.

## Examples

### UnaryTritOperationDemo - Using Unary Operators

Demonstrates how to apply unary operations to ternary values using the UnaryTritOperator structure.

```csharp
namespace Examples;

using Ternary3;
using Ternary3.Operators;
// The class Operators.Unary contains static methods for unary operations on Trit values.
using static Ternary3.Operators.UnaryTritOperator;
// Common alias for 27-trit integers
using Tryte = Ternary3.Int27T;

public static class UnaryTritOperationDemo
{
    public static void Run()
    {
        Console.WriteLine($"\r\n\r\n{nameof(UnaryTritOperationDemo)}");
        
        // Operation on Int3T outputs TritArray3
        Int3T input1 = 5; // 1TT
        var output1 = input1 | AbsoluteValue; // TritArray3 111
        Console.WriteLine($"Absolute value of {input1} ({(TritArray3)input1}) = {(Int3T)output1} ({output1})"); // Absolute value of 5 (1TT) = 13 (111)

        // Custom operation on Int9T outputs TritArray9, which can be implicitly converted to Int9T
        Trit EchoA(Trit trit) => trit; // Custom operation: Identity function for Trit
        Int9T input2A = -10; // T0T
        Int9T output2A = input2A | EchoA; // T0T. Gets implicitly converted to Int9T
        Console.WriteLine($"-10 Echoed = {output2A}"); // Prints -10
        
        // Custom operation on Int9T outputs TritArray9, which can be implicitly converted to Int9T
        Trit[] echoB = [Trit.Negative, Trit.Zero, Trit.Positive];
        Int9T input2B = -10; // T0T
        Int9T output2B = input2B | echoB; // T0T. Gets implicitly converted to Int9T
        Console.WriteLine($"-10 Echoed = {output2B}"); // Prints -10
        
        // Custom operation on Int9T outputs TritArray9, which can be implicitly converted to Int9T
        var echoC = new UnaryTritOperator(false, null, true);
        Int9T input2C = -10; // T0T
        Int9T output2C = input2C | echoC; // T0T. Gets implicitly converted to Int9T
        Console.WriteLine($"-10 Echoed = {output2C}"); // Prints -10
        
        // Custom operation on Int9T outputs TritArray9, which can be implicitly converted to Int9T
        var echoD = new UnaryTritOperator(-1, 0, 1);
        Int9T input2D = -10; // T0T
        Int9T output2D = input2D | echoD; // T0T. Gets implicitly converted to Int9T
        Console.WriteLine($"-10 Echoed = {output2D}"); // Prints -10
        
        // Using an alias for a Tryte (27-trit integer) and the full name for the operator
        Tryte input3 = 11; // 11T
        long output3 = input3 | Ternary3.Operators.UnaryTritOperator.Negate; // TT1, using the full name for the operator. Result implicitly converted to long.
        Console.WriteLine($"11 Negated = {output3}"); // 11 Negated = -11
        
        //TritArray input with TritArray27. Output is also TritArray27, // which can be explicitly converted to long.
        TritArray27 input4 = 123456789;
        var output4 = input4 | Negate;
        Console.WriteLine($"123456789 Negated = {output4} ({(int)output4})"); // Prints 000000000 T0011TT01 T1T010100 (-123456789)
        
        //Single Trit input
        var input5 = Trit.Negative; // T (negative)
        var output5 = input5 | IsPositive; // No. It is not positive. No translates to T (negative)
        Console.WriteLine($"Is {input5} positive? {output5}"); // Is Negative positive? Negative
        
        // integer input will be implicitly converted to the corresponding TritArray.
        short input6 = -16; // T11T
        var output6 = input6 | Floor; // All T become 0. 0110;
        Console.WriteLine($"{input6} | {nameof(Floor)} becomes {output6}"); // -22 Floor becomes 000000110
    }
}
```

### BinaryTritOperationDemo - Operations on Trit Pairs

Demonstrates various binary operations on ternary values using lookup tables, showing how to work with built-in operations and create custom ones.

```csharp
namespace Examples;

using Ternary3;
using Ternary3.Operators;
using static Ternary3.Operators.BinaryTritOperator;

/// <summary>
/// Demonstrates various binary operations on ternary values using lookup tables.
/// A binary operation takes two ternary inputs and produces a ternary output.
/// </summary>
public class BinaryTritOperationDemo
{
    // Constant representing the negative trit (-1) for readability
    const int T = -1;

    public static void Run()
    {
        Console.WriteLine($"\r\n\r\n{nameof(BinaryTritOperationDemo)}");

        
        // EXAMPLE 1: Built-in AND operation on two sbytes
        // ---------------------------------------------
        // In ternary, AND returns the minimum value of each trit pair
        // Truth table for AND:
        // (-1 AND -1) = -1, (-1 AND 0) = -1, (-1 AND 1) = -1
        // ( 0 AND -1) = -1, ( 0 AND 0) =  0, ( 0 AND 1) =  0
        // ( 1 AND -1) = -1, ( 1 AND 0) =  0, ( 1 AND 1) =  1
        sbyte input1A = 8; // 10T in balanced ternary (where T is -1)
        sbyte input1B = 9; // 100 in balanced ternary
        var result1 = input1A | And | input1B; // Uses the predefined AND operation
        Console.WriteLine($"BinaryLookup And: {input1A} {nameof(And)} {input1B} = {(sbyte)result1} ({result1})"); 
        // Output: 8 (10T) because:
        // 10T (input1A) AND 100 (input1B) = 10T
        // Position by position: 1∧1=1, 0∧0=0, T∧0=T

        // EXAMPLE 2: Custom operation on two shorts using a BinaryTritOperator
        // ---------------------------------------------------------------
        // A BinaryTritOperator defines the output for each of the 9 possible input trit combinations
        short input2A = -6; // 000000T10 in balanced ternary
        short input2B = 13;  // 000000111 in balanced ternary
        // This lookup table defines a "mask" operation:
        // - Returns -1 only when both inputs are -1
        // - Returns 1 only when both inputs are 1
        // - Returns 0 in all other cases (including when inputs match as 0)
        BinaryTritOperator mask = new([
            [T, 0, 0],  // Row for when first input is -1 
            [0, 0, 0],  // Row for when first input is 0
            [0, 0, 1]   // Row for when first input is 1
        ]);
        var result2 = input2A | mask | input2B; // Apply the mask operation
        Console.WriteLine($"Custom operation on short: {(TritArray9)input2A} {nameof(mask)} {(TritArray9)input2B} = {result2}");
        // Output: 000000010 - Only positions where both inputs have the same non-zero value are preserved
        
        // EXAMPLE 3: Complex custom operation on int and long
        // ------------------------------------------------
        // The BinaryTritOperator can be initialized using nullable booleans:
        // - null means -1 (negative)
        // - false means 0 (zero)
        // - true means 1 (positive)
        var input3A = 123456789; 
        long input3B = 987654321; 
        // This "decreaseBy" operation decreases the trit value based on specific combinations
        // The table is read as [first operand, second operand] → result
        var decreaseBy = new BinaryTritOperator(
            null, true, false,   // When first trit is -1: [-1,-1]→-1, [-1,0]→1, [-1,1]→0
            false, null, true,   // When first trit is 0:  [0,-1]→0, [0,0]→-1, [0,1]→1
            true, false, null    // When first trit is 1:  [1,-1]→1, [1,0]→0, [1,1]→-1
        );
        var result3 = input3A | decreaseBy | input3B;
        Console.WriteLine($"Custom operation on int: {input3A} {nameof(decreaseBy)} {input3B} = {result3} ({(int)result3})");
        // The operation transforms each trit pair according to the lookup table

        // EXAMPLE 4: Inverting operation that ignores the second operand
        // -----------------------------------------------------------
        // This demonstrates how to create an operation that only depends on the first input
        TritArray27 input4A = 5;   // Converts to balanced ternary
        TritArray27 input4B = 10;  // Will be ignored by this operation
        // This table inverts the first operand and ignores the second:
        // For any first trit value, the output is the same regardless of the second trit
        var invertFirstIgnoreSecond = new BinaryTritOperator(
            Trit.Positive, Trit.Positive, Trit.Positive,  // When first trit is -1, always return 1
            Trit.Zero, Trit.Zero, Trit.Zero,              // When first trit is 0, always return 0
            Trit.Negative, Trit.Negative, Trit.Negative   // When first trit is 1, always return -1
        );
        var result4 = input4A | invertFirstIgnoreSecond | input4B; 
        Console.WriteLine($"Custom operation on int: {input4A} {nameof(invertFirstIgnoreSecond)} {input4B} = {result4} ({(int)result4})");
        // Each trit in input4A is inverted (1→-1, 0→0, -1→1) regardless of input4B's value
        
        // EXAMPLE 5: Built-in OR operation on individual trits
        // ------------------------------------------------
        // In balanced ternary, OR returns the maximum value of each trit pair
        // Truth table for OR:
        // (-1 OR -1) = -1, (-1 OR 0) = 0, (-1 OR 1) = 1
        // ( 0 OR -1) =  0, ( 0 OR 0) = 0, ( 0 OR 1) = 1
        // ( 1 OR -1) =  1, ( 1 OR 0) = 1, ( 1 OR 1) = 1
        var input5A = Trit.Positive;  // Value 1
        var input5B = Trit.Negative;  // Value -1
        var result5 = input5A | Or | input5B;  // 1 OR -1 = 1 (maximum value)
        Console.WriteLine($"Trit Or: {input5A} {nameof(Or)} {input5B} = {result5}");
    }
}
```

### OverflowDemo - Arithmetic Overflow and Shifting

Demonstrates how ternary numbers behave during overflow situations and bit shifting operations, which differ from binary numbers.

```csharp
namespace Examples;

using Ternary3;

public static class OverflowDemo
{
    public static void Run()
    {
        Console.WriteLine($"\r\n\r\n{nameof(OverflowDemo)}");
        
        // Addition and substraction may overflow and do so in a ternary way.
        // (so instead of cutting of "binary" bits, it cuts off "ternary" trits)
        Int3T input1A = 12; // 110
        Int3T input1B = 3; // 010
        var result1 = input1A + input1B; // 110 + 010 = 1TT0. Int3T only keeps 3 trits, so TT0 = -12
        Console.WriteLine($"Overflow: {input1A} + {input1B} = {result1}"); // Overflow: 110 + 010 = 1TT0

        Int3T input2A = 12; // 110
        Int3T input2B = -12; // TT0
        var result2 = input2A * input2B; // 110 * TT0 = 101T00 (144). Int3T only keeps 3 trits, so T00 = -9
        Console.WriteLine($"Overflow: {input2A} * {input2B} = {result2}"); // Overflow: 110 * TT0 = 101T00

        // Addition and sumple multiplication of TritArray3 also may overflow.
        // (Under the hood, these are often performed without conversion to binary)
        TritArray3 input3A = 12; // 110
        TritArray3 input3B = 3; // 010
        var result3 = input3A * input3B; // 110 * TT0 = 101T00 (144). Int3T only keeps 3 trits, so T00 = -9     
        Console.WriteLine($"Overflow: {input3A} * {input3B} = {result3} ({(int)result3})"); // Overflow: 110 * 010 = 101T00 (-9)
        
        Int3T input4 = 25; // 10T1. trimmed to 3 trits = 0T1 or -8;
        Console.WriteLine($"Overflow: 25 => {input4} ({(TritArray3)input4})"); // Overflow: 25 => -2 (0T1)

        TritArray3 input5 = 25; // 10T1. trimmed to 3 trits = 0T1 or -8;
        Console.WriteLine($"Overflow: 25 => {input5} ({(int)input5})"); // Overflow: 25 => (0T1) (25)

        // Shifting trits two positions in essence multiplies or divides by 9 (3^2).
        var input6 = TritArray9.MaxValue; // 111111111
        var result6A = input5 << 6;
        Console.WriteLine($"Shift: {input6} << 6 = {result6A} ({(int)result6A})"); // Shift: 111111111 << 6 = 111000000 (9477)
        var result6B = input5 >> -6;
        Console.WriteLine($"Shift: {input6} >> -6 = {result6B} ({(int)result6B})"); // Shift: 111111111 >> -6 = 111000000 (9477)
        var result6C = input5 << -6;
        Console.WriteLine($"Shift: {input6} << -6 = {result6C} ({(int)result6C})"); // Shift: 111111111 << -6 = 000000111 (13)
        var result6D = input5 >> 6;
        Console.WriteLine($"Shift: {input6} >> 6 = {result6D} ({(int)result6D})"); // Shift: 111111111 >> 6 = 000000111 (13)

        var input7 = Int27T.MinValue; // TTTTTTTTT TTTTTTTTT TTTTTTTTT
        var result7A = input7 << 25;
        Console.WriteLine($"Shift: {(TritArray27)input7} << 25 = {result7A} ({(TritArray27)result7A})"); // Shift: TTTT..TT << 25 = TT00...00 (-3389154437772)
        var result7B = input7 >> -25;
        Console.WriteLine($"Shift: {(TritArray27)input7} >> -25 = {result7B} ({(TritArray27)result7B})"); // Shift: TTTT..TT >> -25 = TT00...00 (-3389154437772)
        var result7C = input7 << -25;
        Console.WriteLine($"Shift: {(TritArray27)input7} << -25 = {result7C} ({(TritArray27)result7C})"); // Shift: TTTT..TT << -25 = 0000...TT (-4)
        var result7D = input7 >> 25;
        Console.WriteLine($"Shift: {(TritArray27)input7} >> 25 = {result7D} ({(TritArray27)result7D})"); // Shift: 111111111 >> 25 = 0000..TT (-4)
    }
}
```

## Examples

### Basic Arithmetic Operations

```csharp
using Ternary3.Numbers;

public class BasicArithmeticExample
{
    public static void Run()
    {
        // Create ternary numbers
        var a = new Trit(5);  // 1TT in balanced ternary
        var b = new Trit(7);  // 21T in balanced ternary
        
        // Addition
        Console.WriteLine($"{a} + {b} = {a + b}");
        
        // Subtraction
        Console.WriteLine($"{a} - {b} = {a - b}");
        
        // Multiplication
        Console.WriteLine($"{a} * {b} = {a * b}");
        
        // Division
        Console.WriteLine($"{a} / {b} = {a / b}");
        
        // Modulo
        Console.WriteLine($"{a} % {b} = {a % b}");
    }
}
```

### Logical Operations

```csharp
using Ternary3.Numbers;
using static Ternary3.Operators.BinaryLookup;

public class LogicalOperationsExample
{
    public static void Run()
    {
        var x = new Trit(5);  // 1TT in balanced ternary
        var y = new Trit(3);  // 10 in balanced ternary
        
        // Demonstrate logical AND operation
        Console.WriteLine($"{x} AND {y} = {x | And | y}");
        Console.WriteLine($"In decimal: 5 AND 3 = {(x | And | y).ToInt()}");
        
        // Demonstrate logical OR operation
        Console.WriteLine($"{x} OR {y} = {x | Or | y}");
        Console.WriteLine($"In decimal: 5 OR 3 = {(x | Or | y).ToInt()}");
        
        // Demonstrate logical XOR operation
        Console.WriteLine($"{x} XOR {y} = {x | Xor | y}");
        Console.WriteLine($"In decimal: 5 XOR 3 = {(x | Xor | y).ToInt()}");
        
        // Demonstrate more complex expressions
        var z = new Trit(8);  // 1T1 in balanced ternary
        var result = (x | And | y) | Xor | z;
        Console.WriteLine($"(5 AND 3) XOR 8 = {result}");
        Console.WriteLine($"In decimal: (5 AND 3) XOR 8 = {result.ToInt()}");
    }
}
```

### Unary Operations

```csharp
using Ternary3.Numbers;
using static Ternary3.Operators.Unary;

public class UnaryOperationsExample
{
    public static void Run()
    {
        var num = new Trit(8);  // 1T1
        
        // Floor operation
        Console.WriteLine($"{num} | Floor = {num | Floor}");
        
        // Ceiling operation
        Console.WriteLine($"{num} | Ceiling = {num | Ceiling}");
        
        // Negate operation
        Console.WriteLine($"{num} | Negate = {num | Negate}");
        
        // Increment operation
        Console.WriteLine($"{num} | Increment = {num | Increment}");
        
        // Decrement operation
        Console.WriteLine($"{num} | Decrement = {num | Decrement}");
    }
}
```

### Conversion Example

```csharp
using Ternary3.Numbers;

public class ConversionExample
{
    public static void Run()
    {
        // Convert from decimal to balanced ternary
        int decimalValue = 42;
        var ternaryValue = new Trit(decimalValue);
        Console.WriteLine($"Decimal {decimalValue} in balanced ternary: {ternaryValue}");
        
        // Convert from balanced ternary to decimal
        string ternaryString = "11T0T";
        var parsedValue = Trit.Parse(ternaryString);
        Console.WriteLine($"Balanced ternary {ternaryString} in decimal: {parsedValue.ToInt()}");
        
        // Convert from binary to balanced ternary
        string binaryString = "10101";
        var binaryValue = Convert.ToInt32(binaryString, 2);
        var ternaryFromBinary = new Trit(binaryValue);
        Console.WriteLine($"Binary {binaryString} in balanced ternary: {ternaryFromBinary}");
    }
}
```

## Formatting and Display

### Custom Formatting with ToString(ITernaryFormat)

You can now format any trit array (implementing `ITritArray`) using a custom or built-in ternary format. Use the new `ToString(ITernaryFormat format)` overload to control digit symbols, grouping, separators, and padding:

```csharp
var trits = new TritArray27(...);
var format = new Ternary3.Formatting.TernaryFormat()
    .WithGroup(3, " ")
    .WithGroup(3, "-");
string formatted = trits.ToString(format); // e.g. "T01-T01-T01 - TTT-000-111"
```

This allows you to display ternary numbers in a way that matches your application's needs or user preferences.

---

## Ternary3.Formatting Namespace Overview

The `Ternary3.Formatting` namespace provides flexible formatting for ternary numbers. Key types include:

- **ITernaryFormat**: Interface for formatting options, including digit symbols, group definitions, decimal separator, and padding mode.
- **TernaryFormat**: Default, fully customizable implementation of ITernaryFormat. Allows you to set digit characters, group sizes, separators, and padding. Includes fluent methods for group configuration.
- **InvariantTernaryFormat**: A built-in, culture-invariant format with standard digit symbols (T, 0, 1) and default grouping.
- **MinimalTernaryFormat**: (If present) A minimal format for compact ternary representations.
- **TritGroupDefinition**: Defines a group for hierarchical formatting, specifying the separator and group size at each level.
- **TernaryPadding**: Enum indicating how padding is applied (None, Group, or Full).

These types allow you to control how ternary numbers are displayed, supporting multi-level grouping, custom digit symbols, and culture-specific formatting.

## Reference

### Index

- **Core Types**
  - [`Trit` Struct](#trit-struct) - the ternary equivalent of a bit/bool
  - [`TritArray3` Struct](#tritarray3-struct) - a 3-trit number stored optimized for trit operation performance
  - [`TritArray9` Struct](#tritarray9-struct) - a 9-trit number stored optimized for trit operation performance
  - [`TritArray27` Struct](#tritarray27-struct) - a 27-trit number stored optimized for trit operation performance
  - [`Int3T` Struct](#int3t-struct) - a 3-trit number stored optimized for arithmetic operations
  - [`Int9T` Struct](#int9t-struct) - a 9-trit number stored optimized for arithmetic operations
  - [`Int27T` Struct](#int27t-struct) - a 27-trit number stored optimized for arithmetic operations
- **Operator Structs**
  - [`UnaryTritOperator` Struct](#unarytritoperator-struct) - Defines unary Trit operations and provides standard operations
  - [`BinaryTritOperator` Struct](#binarytritoperator-struct) - Defines binary Trit operations using a lookup table
- **Additional Types**
  - [`ITritArray` Interface](#itritarray-interface)
  - [`ITernaryInteger<T>` Interface](#iternaryintegert-interface)

### Core Types

#### `Trit` Struct

```csharp
namespace Ternary3.Numbers
```

Represents a trinary (three-valued) logical value that can be Negative (-1), Zero (0), or Positive (1).

**Static Properties:**
- `static readonly Trit Negative` - Represents the Negative (-1) or 'false' value of the Trit.
- `static readonly Trit Zero` - Represents the Zero (0) or 'null' value of the Trit.
- `static readonly Trit Positive` - Represents the Positive (1) or 'true' value of the Trit.

**Operators:**
- `static implicit operator Trit(bool? value)` - Converts the specified nullable Boolean value to a Trit value.
- `static implicit operator Trit(bool value)` - Converts the specified Boolean value to a Trit value.
- `static implicit operator bool?(Trit trit)` - Converts the specified Trit value to a nullable Boolean value.
- `static implicit operator sbyte(Trit trit)` - Converts the Trit value to its underlying signed byte representation.
- `static explicit operator Trit(sbyte value)` - Converts the specified signed byte value to a Trit value.
- `static implicit operator int(Trit trit)` - Converts the Trit value to its underlying integer representation.
- `static explicit operator Trit(int value)` - Converts the specified integer value to a Trit value.
- `static bool operator true(Trit trit)` - Returns true if the value is Positive (1), false otherwise.
- `static bool operator false(Trit trit)` - Returns true if the value is Negative (-1), false otherwise.
- `static Trit operator !(Trit trit)` - Performs a logical NOT operation on a Trit value.
- `static bool operator ==(Trit left, Trit right)` - Determines if two Trit values are equal.
- `static bool operator !=(Trit left, Trit right)` - Determines if two Trit values are not equal.
- `static unsafe UnsafeTritOperator operator |(Trit left, delegate*<Trit, Trit, Trit> operation)` - Creates an UnsafeTritOperator to enable custom operations using the pipe syntax.
- `static TritOperator operator |(Trit left, Func<Trit, Trit, Trit> operation)` - Creates a TritOperator to enable custom operations using the pipe syntax.
- `static LookupTritOperator operator |(Trit left, BinaryTritOperator table)` - Creates a LookupTritOperator to enable custom operations using the pipe syntax.
- `static LookupTritOperator operator |(Trit left, Trit[,] table)` - Creates a LookupTritOperator to enable custom operations using the pipe syntax.
- `static Trit operator |(Trit left, Func<Trit, Trit> operation)` - Applies a unary operation function to the left Trit.
- `static Trit operator |(Trit left, Trit[] table)` - Applies a unary operation lookup table to the left Trit.

**Methods:**
- `override string ToString()` - Returns a string representation of the current Trit value.
- `bool Equals(Trit other)` - Determines if this Trit is equal to another Trit.
- `override bool Equals(object? obj)` - Returns a value indicating whether this instance is equal to a specified object.
- `override int GetHashCode()` - Returns the hash code for this instance.

#### `TritArray3` Struct

```csharp
namespace Ternary3.Numbers
```

Represents a fixed-size array of 3 trits (ternary digits).

**Static Properties:**
- `static readonly TritArray3 MinValue` - Represents the minimum value that a TritArray3 can have (all trits set to -1).
- `static readonly TritArray3 MaxValue` - Represents the maximum value that a TritArray3 can have (all trits set to 1).
- `static readonly TritArray3 Zero` - Represents a TritArray3 with all trits set to zero.

**Properties:**
- `int Length` - Gets the length of the trit array, which is always 3.

**Indexer:**
- `Trit this[int index]` - Gets or sets the trit at the specified index.

**Operators:**
- `static TritArray3 operator |(TritArray3 array, Func<Trit, Trit> operation)` - Applies a unary operation to each trit in the array.
- `static TritArray3 operator |(TritArray3 array, Trit[] table)` - Applies a lookup table operation to each trit in the array.
- `static LookupTritArray3Operator operator |(TritArray3 array, Func<Trit, Trit, Trit> operation)` - Creates a binary operation context for this array.
- `static LookupTritArray3Operator operator |(TritArray3 array, BinaryTritOperator table)` - Creates a binary operation context for this array.
- `static LookupTritArray3Operator operator |(TritArray3 array, Trit[,] table)` - Creates a binary operation context for this array.
- `static TritArray3 operator <<(TritArray3 array, int shift)` - Performs a left bitwise shift on the trit array.
- `static TritArray3 operator >>(TritArray3 array, int shift)` - Performs a right bitwise shift on the trit array.
- `static TritArray3 operator +(TritArray3 value1, TritArray3 value2)` - Adds two TritArray3 values together.
- `static TritArray3 operator -(TritArray3 value1, TritArray3 value2)` - Subtracts one TritArray3 value from another.
- `static implicit operator TritArray3(Int3T value)` - Defines an implicit conversion of an Int3T to a TritArray3.
- `static implicit operator Int3T(TritArray3 array)` - Defines an implicit conversion of a TritArray3 to an Int3T.
- `static implicit operator TritArray3(sbyte value)` - Defines an implicit conversion of a sbyte to a TritArray3.
- `static implicit operator sbyte(TritArray3 array)` - Defines an implicit conversion of a TritArray3 to a sbyte.
- `static explicit operator TritArray3(int value)` - Defines an explicit conversion of an int to a TritArray3.
- `static implicit operator int(TritArray3 array)` - Defines an implicit conversion of a TritArray3 to an int.
- `static explicit operator TritArray3(long value)` - Defines an explicit conversion of a long to a TritArray3.
- `static implicit operator long(TritArray3 array)` - Defines an implicit conversion of a TritArray3 to a long.

**Methods:**
- `override string ToString()` - Returns a string representation of the trit array.

#### `TritArray9` Struct

```csharp
namespace Ternary3.Numbers
```

Represents a fixed-size array of 9 trits (ternary digits).

**Static Properties:**
- `static readonly TritArray9 MinValue` - Represents the minimum value that a TritArray9 can have (all trits set to -1).
- `static readonly TritArray9 MaxValue` - Represents the maximum value that a TritArray9 can have (all trits set to 1).
- `static readonly TritArray9 Zero` - Represents a TritArray9 with all trits set to zero.

**Properties:**
- `int Length` - Gets the length of the trit array, which is always 9.

**Indexer:**
- `Trit this[int index]` - Gets or sets the trit at the specified index.

**Similar operators and methods as TritArray3 but for 9 trits.**

#### `TritArray27` Struct

```csharp
namespace Ternary3.Numbers
```

Represents a fixed-size array of 27 trits (ternary digits).

**Static Properties:**
- `static readonly TritArray27 MinValue` - Represents the minimum value that a TritArray27 can have (all trits set to -1).
- `static readonly TritArray27 MaxValue` - Represents the maximum value that a TritArray27 can have (all trits set to 1).
- `static readonly TritArray27 Zero` - Represents a TritArray27 with all trits set to zero.

**Properties:**
- `int Length` - Gets the length of the trit array, which is always 27.

**Indexer:**
- `Trit this[int index]` - Gets or sets the trit at the specified index.

**Similar operators and methods as TritArray3 but for 27 trits.**

#### `Int3T` Struct

```csharp
namespace Ternary3.Numbers
```

Represents a 3-trit signed integer, modeled after the SByte type.

**Constants:**
- `const SByte MaxValueConstant` - Represents the maximum value of a Int3T, expressed as a SByte.
- `const SByte MinValueConstant` - Represents the minimum value of a Int3T, expressed as a SByte.

**Static Properties:**
- `static readonly Int3T MaxValue` - Represents the largest possible value of a Int3T.
- `static readonly Int3T MinValue` - Represents the smallest possible value of a Int3T.

**Operators:**
- `static Int3T operator <<(Int3T value, int shiftAmount)` - Performs a left shift operation on the ternary number, maintaining the original numeric type.
- `static Int3T operator >>(Int3T value, int shiftAmount)` - Performs a right shift operation on the ternary number, maintaining the original numeric type.
- `static Int3T operator >>>(Int3T value, int shiftAmount)` - Performs an unsigned right shift operation on the ternary number, maintaining the original numeric type. In this implementation, it behaves the same as the signed right shift.
- `static TritArray3 operator |(Int3T value, Func<Trit, Trit> operation)` - Applies a unary operation to each trit in this ternary number. This operation converts the number to a TritArray.
- `static TritArray3 operator |(Int3T value, Trit[] trits)` - Combines each trit in this ternary number with the corresponding trit in the provided array. This operation converts the number to a TritArray.
- Standard arithmetic and comparison operators

**Methods:**
- `override string ToString()` - Returns a string representation of the trit value.
- `bool Equals(Int3T other)` - Determines if this Int3T is equal to another Int3T.
- `override bool Equals(object? obj)` - Returns a value indicating whether this instance is equal to a specified object.
- `override int GetHashCode()` - Returns the hash code for this instance.

Similarly for Int9T and Int27T:

#### `Int9T` Struct

```csharp
namespace Ternary3.Numbers
```

Represents a 9-trit signed integer, modeled after the Int16 type.

**Constants:**
- `const Int16 MaxValueConstant` - Represents the maximum value of a Int9T, expressed as an Int16.
- `const Int16 MinValueConstant` - Represents the minimum value of a Int9T, expressed as an Int16.

**Static Properties:**
- `static readonly Int9T MaxValue` - Represents the largest possible value of a Int9T.
- `static readonly Int9T MinValue` - Represents the smallest possible value of a Int9T.

**Operators:**
- `static Int9T operator <<(Int9T value, int shiftAmount)` - Performs a left shift operation on the ternary number, maintaining the original numeric type.
- `static Int9T operator >>(Int9T value, int shiftAmount)` - Performs a right shift operation on the ternary number, maintaining the original numeric type.
- `static Int9T operator >>>(Int9T value, int shiftAmount)` - Performs an unsigned right shift operation on the ternary number, maintaining the original numeric type. In this implementation, it behaves the same as the signed right shift.
- `static TritArray9 operator |(Int9T value, Func<Trit, Trit> operation)` - Applies a unary operation to each trit in this ternary number. This operation converts the number to a TritArray.
- `static TritArray9 operator |(Int9T value, Trit[] trits)` - Combines each trit in this ternary number with the corresponding trit in the provided array. This operation converts the number to a TritArray.
- Standard arithmetic and comparison operators

**Methods:**
- `override string ToString()` - Returns a string representation of the trit value.
- `bool Equals(Int9T other)` - Determines if this Int9T is equal to another Int9T.
- `override bool Equals(object? obj)` - Returns a value indicating whether this instance is equal to a specified object.
- `override int GetHashCode()` - Returns the hash code for this instance.

#### `Int27T` Struct

```csharp
namespace Ternary3.Numbers
```

Represents a 27-trit signed integer, modeled after the Int64 type.

**Constants:**
- `const Int64 MaxValueConstant` - Represents the maximum value of a Int27T, expressed as an Int64.
- `const Int64 MinValueConstant` - Represents the minimum value of a Int27T, expressed as an Int64.

**Static Properties:**
- `static readonly Int27T MaxValue` - Represents the largest possible value of a Int27T.
- `static readonly Int27T MinValue` - Represents the smallest possible value of a Int27T.

**Operators:**
- `static Int27T operator <<(Int27T value, int shiftAmount)` - Performs a left shift operation on the ternary number, maintaining the original numeric type.
- `static Int27T operator >>(Int27T value, int shiftAmount)` - Performs a right shift operation on the ternary number, maintaining the original numeric type.
- `static Int27T operator >>>(Int27T value, int shiftAmount)` - Performs an unsigned right shift operation on the ternary number, maintaining the original numeric type. In this implementation, it behaves the same as the signed right shift.
- `static TritArray27 operator |(Int27T value, Func<Trit, Trit> operation)` - Applies a unary operation to each trit in this ternary number. This operation converts the number to a TritArray.
- `static TritArray27 operator |(Int27T value, Trit[] trits)` - Combines each trit in this ternary number with the corresponding trit in the provided array. This operation converts the number to a TritArray.
- Standard arithmetic and comparison operators

**Methods:**
- `override string ToString()` - Returns a string representation of the trit value.
- `bool Equals(Int27T other)` - Determines if this Int27T is equal to another Int27T.
- `override bool Equals(object? obj)` - Returns a value indicating whether this instance is equal to a specified object.
- `override int GetHashCode()` - Returns the hash code for this instance.

### Operator Classes

#### `UnaryTritOperator` Struct

```csharp
namespace Ternary3.Operators
```

Provides a set of predefined unary operations for Trit values.

**Static Properties and Methods:**
- `static Trit Apply(Trit target, Trit[] table)` - Apply a unary operation to a Trit value.
- `static readonly UnaryTritOperator Negative` - Negative value.
- `static readonly UnaryTritOperator Decrement` - Decrement.
- `static readonly UnaryTritOperator IsPositive` - Is the value positive?
- `static readonly UnaryTritOperator NegateAbsoluteValue` - Negate the Absolute Value.
- `static readonly UnaryTritOperator Ceil` - Ceiling Zero.
- `static readonly UnaryTritOperator Identity` - Identity function.
- `static readonly UnaryTritOperator IsZero` - Is the value zero?
- `static readonly UnaryTritOperator KeepNegative` - Keep negative values.
- `static readonly UnaryTritOperator IsNotNegative` - Is the value not negative?
- `static readonly UnaryTritOperator CeilIsNegative` - Ceiling for negative values.
- `static readonly UnaryTritOperator CeilIsNotZero` - Ceiling for non-zero values.
- `static readonly UnaryTritOperator KeepPositive` - Keep positive values.
- `static readonly UnaryTritOperator CeilIsNotPositive` - Ceiling for non-positive values.
- `static readonly UnaryTritOperator Zero` - Zero value.
- `static readonly UnaryTritOperator Floor` - Floor function.
- `static readonly UnaryTritOperator CyclicIncrement` - Cycle increment.
- `static readonly UnaryTritOperator FloorIsZero` - Floor for zero.
- `static readonly UnaryTritOperator Increment` - Increment function.
- `static readonly UnaryTritOperator IsNegative` - Is the value negative?
- `static readonly UnaryTritOperator CyclicDecrement` - Cycle decrement.
- `static readonly UnaryTritOperator IsNotZero` - Is the value not zero?
- `static readonly UnaryTritOperator Negate` - Negate function.
- `static readonly UnaryTritOperator FloorIsNegative` - Floor for negative values.
- `static readonly UnaryTritOperator AbsoluteValue` - Absolute value.
- `static readonly UnaryTritOperator IsNotPositive` - Is the value not positive?
- `static readonly UnaryTritOperator FloorIsNotPositive` - Floor for non-positive values.
- `static readonly UnaryTritOperator Positive` - Positive value.

#### `BinaryTritOperator` Struct

```csharp
namespace Ternary3.Operators
```

Provides a set of predefined binary operations implemented as lookup tables for Trit values.

**Static Properties:**
- `static readonly BinaryTritOperator Positive` - A constant that returns Positive (1) for any trit combination.
- `static readonly BinaryTritOperator Zero` - A constant that returns Zero (0) for any trit combination.
- `static readonly BinaryTritOperator Negative` - A constant that returns Negative (-1) for any trit combination.
- `static readonly BinaryTritOperator And` - The logical AND operation in three-valued logic.
- `static readonly BinaryTritOperator Or` - The logical OR operation in three-valued logic.
- `static readonly BinaryTritOperator Xor` - The logical XOR operation in three-valued logic.
- `static readonly BinaryTritOperator Plus` - The addition operation in three-valued logic.
- `static readonly BinaryTritOperator Minus` - The subtraction operation in three-valued logic.
- `static readonly BinaryTritOperator Implicates` - The logical implication operation.
- `static readonly BinaryTritOperator Is` - The equality check operation.
- `static readonly BinaryTritOperator GreaterThan` - The greater than comparison operation.
- `static readonly BinaryTritOperator LesserThan` - The less than comparison operation.

### Ternary3.Formatting Namespace

#### `TernaryFormat` Class

Represents a customizable ternary format, allowing you to specify digit symbols, grouping, separators, and padding for formatting trit arrays.

**Static Properties:**
- `static readonly TernaryFormat.Invariant` — A built-in, culture-invariant ternary format with standard digit symbols and grouping.
- `static readonly TernaryFormat.Minimal` — A built-in minimal ternary format for compact representations.

**Constructors:**
- `TernaryFormat()` — Initializes a new instance with the invariant format.
- `TernaryFormat(ITernaryFormat other)` — Initializes a new instance by copying settings from another format.

**Properties:**
- `char NegativeTritDigit` — The character used to represent a negative trit (-1).
- `char ZeroTritDigit` — The character used to represent a zero trit (0).
- `char PositiveTritDigit` — The character used to represent a positive trit (+1).
- `IList<TritGroupDefinition> Groups` — The list of group definitions, each specifying a separator and group size for hierarchical grouping.
- `string DecimalSeparator` — The string used as a decimal separator (for future floating-point trit support).
- `TernaryPadding TernaryPadding` — The padding mode for the formatted ternary string.

**Methods:**
- `TernaryFormat WithGroup(int size, string separator)` — Adds a group definition to the format and returns the current instance for chaining.
- `TernaryFormat ClearGroups()` — Removes all group definitions from the format and returns the current instance for chaining.
- `string ToString()` — Previews the format by creating a sample TritArray27 and formatting it.

#### `TernaryPadding` Enum

Specifies how padding is applied to the formatted ternary string.

- `None` — No padding is applied.
- `Group` — Padding is applied to fill the last group.
- `Full` — Padding is applied to fill the entire formatted string to a fixed width.
