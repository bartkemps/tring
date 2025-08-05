> **Perhaps the prettiest number system of all is the balanced ternary notation**
>
> -- *Donald Knuth, The art of computer programming*

# Ternary3 - C# Balanced Ternary Number Library

### Table of Contents
  - [Introduction](#introduction)
    - [What is Ternary3?](#what-is-ternary3)
    - [Features](#features)
    - [Resources](#resources)
  - [Getting Started](#getting-started)
  - [Ternary Literals](#ternary-literals)
  - [Core Classes and Types](#core-classes-and-types)
    - [TernaryArray](#TernaryArray)
    - [Fixed-Size TernaryArrays](#fixed-size-TernaryArrays)
    - [LookupTernaryArrayOperator](#lookupTernaryArrayoperator)
  - [IO](#io)
    - [Int3TStream](#int3tstream)
    - [MemoryInt3TStream](#memoryint3tstream)
    - [ByteToInt3TStream](#bytetoint3tstream)
    - [Int3TToByteStream](#int3ttobytestream)
  - [Operators](#operators)
    - [UnaryTritOperationDemo](#unarytritoperationdemo---using-unary-operators)
    - [BinaryTritOperationDemo](#binarytritoperationdemo---operations-on-trit-pairs)
    - [OverflowDemo](#overflowdemo---arithmetic-overflow-and-shifting)
  - [Formatting and Display](#formatting-and-display)
  - [Reference](#reference)

## Introduction

### What is Ternary3?

Ternary3 is a specialized C# library that implements balanced ternary arithmetic. While most computer systems use binary (base-2) arithmetic, this library enables computation in balanced ternary (base-3) with digits {-1, 0, 1}, often represented as {T, 0, 1}.

In balanced ternary, the digit 'T' represents -1, '0' represents 0, and '1' represents 1. For example, the balanced ternary number "1T01" represents: 
1×3³ + (-1)×3² + 0×3¹ + 1×3⁰ = 27 - 9 + 0 + 1 = 19

> Please let me know what you're using this library for.
> I would love to hear about your projects and how Ternary3 is helping you.
> You can reach me at b.kemps@betabit.nl

Balanced ternary has several interesting properties that make it unique among number systems:

- It's the most efficient number system for representing numbers (using the minimum number of digits)
- It represents negative numbers naturally without special notation
- It simplifies many arithmetic operations (no separate rules for handling negative numbers)
- It has elegant rounding properties - floor and ceiling operations are simple digit truncations
- A balanced ternary computer was actually built in Russia in the 1950s (the Setun)

### Features
- **Specialized Types**: Provides specialized types for 3, 9 and 27 trit numbers, as well as a variable-length `TernaryArray` type for arbitrary-length balanced ternary numbers.
  The `BigTernaryArray` supports ternary numbers with millions of digits. 
- **Parsing and Formatting**: Performant conversion between the balanced ternary types, strings and numbers.
- **Ternary Literals**: Use ternary literals directly in your code with source generation for constants.
- **Arithmetic Operations**: Supports addition, subtraction, multiplication, division, and modulo operations on ternary numbers.
- **Logical Operations**: Supports trit shifts, all 27 operations on a single ternary number, and all 19,683 operations on pairs of ternary numbers.
  Operations are translated to binary operations using lookup tables for performance execution.
- **IO Support**: Provides streams for reading and writing ternary numbers, and adapters to read from and write to any binary stream.

### Resources

For more detailed documentation and mathematical background:
- [Wikipedia: Balanced Ternary](https://en.wikipedia.org/wiki/Balanced_ternary)
- The [Source code repository](https://github.com/bartkemps/tring) for this library.
- The [Referenece](#Reference) for this library.

## Getting Started

Install the package from NuGet:

```bash
dotnet add package Ternary
```

Or search for "Ternary" in the NuGet package manager in Visual Studio or Rider.

Package URL: https://www.nuget.org/packages/Ternary

Write a simple C# program to test the library:

```csharp
using static Ternary3.Operators.BinaryTritOperator;
using static System.Console;

// code generators will generate constants -24 and 4 for ternary literals
// Or is the ternary OR operator, which is defined in the library
WriteLine(terT010 | Or | ter11);
```

This will output a ternary number representing the result of the operation.

## Ternary Literals

Ternary3 supports the use of ternary literals in your code, making it easy to work with constants in balanced ternary notation.

For example:

```csharp
Int3T x = ter01T + terT10; // x will be -4
```

Ternary literals are identifiers of the form `ter010T`, `terT01`, etc., where each character after `ter` is either `0`, `1`, or `T` (for -1). Like other C# literals, underscores are allowed between digits for clarity (e.g., `ter_01_T`). For example:

```csharp
// These are valid ternary literals:
ter01T   // equivalent to decimal 2
terT10   // equivalent to decimal -6
ter0001  // equivalent to decimal 1
ter000_000_01T // equivalent to decimal 2
```

### Source Generator Implementation

When you reference a ternary literal in your code, the Ternary3 source generator automatically generates a constant with the correct value for you. This means you can use ternary literals as if they were predefined constants:

```csharp
int x = ter01T + terT10; // x will be -4
```

The generator creates a global `Literals` class in the `Ternary3` namespace with a `global using static` directive, making all ternary constants available throughout your project without any additional qualifiers.

## Core Classes and Types

### TernaryArray

`TernaryArray` is a key type in the library that represents an arbitrary-length balanced ternary number. It provides various operations for working with ternary numbers:

```csharp
// Create a TernaryArray with a specific length (all trits initialized to 0)
var arr = new TernaryArray(10);

// Get and set individual trits
arr[3] = Trit.Positive;
Trit value = arr[3]; // Gets the trit at index 3

// Use C# 8.0 Index syntax
arr[^1] = Trit.Negative; // Set the last trit to negative (-1)
var lastTrit = arr[^1];  // Get the last trit

// Use C# 8.0 Range syntax to get a slice
var slice = arr[0..5];   // Get the first 5 trits as a new TernaryArray

// Resize the array
arr.Resize(15);         // Resize to a larger array (preserving values)

// Implicit casting from ternary literals
TernaryArray arr2 = terT10_TTT_01T;  // Directly assign a ternary literal to TernaryArray
TernaryArray3 arr3 = ter10T;         // Fixed-size array from literal
TernaryArray9 arr4 = ter10T_101_T01; // 9-trit array from literal
TernaryArray27 arr5 = ter10T_101_T01_11T_01T_T10_T11; // 27-trit array from literal
```

### Fixed-Size TernaryArrays

The library includes specialized fixed-size implementations for common ternary number sizes:

- `TernaryArray3` - A 3-trit number (equivalent range to a byte)
- `TernaryArray9` - A 9-trit number (equivalent range to a short)
- `TernaryArray27` - A 27-trit number (equivalent range to an int)

These offer better performance than the general-purpose `TernaryArray` when working with known sizes.

### LookupTernaryArrayOperator

`LookupTernaryArrayOperator` provides optimized implementations of ternary operations for the TernaryArray types, using lookup tables for high-performance computation.

```csharp
// Create a TernaryArray
var arr = new TernaryArray(10);

// Apply an operation using a LookupTernaryArrayOperator
var result = LookupTernaryArrayOperator.Negate(arr);
```

## IO

The Ternary3.IO namespace provides stream classes for working with Int3T (ternary) data, similar to how System.IO.Stream works with bytes. These streams enable reading, writing, and converting between ternary and binary data formats.

### Int3TStream

The `Int3TStream` is an abstract base class that represents a sequence of Int3T values (trybbles). It is the ternary equivalent of the binary `System.IO.Stream` that works with bytes.

Key properties and methods:
- `CanRead`, `CanWrite`, `CanSeek` - indicate stream capabilities
- `Length`, `Position` - manage stream position and size
- `ReadAsync()`, `WriteAsync()` - asynchronous read/write operations
- `ReadInt3TAsync()`, `WriteInt3TAsync()` - read/write single Int3T values

### MemoryInt3TStream

A concrete implementation of `Int3TStream` that uses memory as its backing store, similar to `MemoryStream` for bytes.

```csharp
using Ternary3.IO;

// Create an expandable memory stream
var stream = new MemoryInt3TStream();

// Write some Int3T values
await stream.WriteInt3TAsync(1);
await stream.WriteInt3TAsync(-1);
await stream.WriteInt3TAsync(0);

// Reset position to read from beginning
stream.Position = 0;

// Read the values back
var buffer = new Int3T[3];
int bytesRead = await stream.ReadAsync(buffer, 0, 3);
```

### ByteToInt3TStream

Converts a byte stream to an Int3T stream using the `BinaryTritEncoder`. This enables reading ternary data that has been encoded in binary format.

```csharp
using Ternary3.IO;

// Convert bytes to Int3T stream
using var fileStream = File.OpenRead("ternary_data.bin");
using var int3tStream = new ByteToInt3TStream(fileStream);

// Read ternary values
var buffer = new Int3T[100];
int tritsRead = await int3tStream.ReadAsync(buffer, 0, 100);
```

### Int3TToByteStream

Converts an Int3T stream to a byte stream, allowing ternary data to be written in binary format for storage or transmission.

```csharp
using Ternary3.IO;

// Create source ternary stream
var memoryStream = new MemoryInt3TStream();
await memoryStream.WriteInt3TAsync(1);
await memoryStream.WriteInt3TAsync(-1);

// Convert to bytes
memoryStream.Position = 0;
using var byteStream = new Int3TToByteStream(memoryStream);

// Write to file
using var fileStream = File.Create("output.bin");
await byteStream.CopyToAsync(fileStream);
```

## Operators

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

public static partial class UnaryTritOperationDemo
{
    public static void Run()
    {
        Console.WriteLine($"\r\n\r\n{nameof(UnaryTritOperationDemo)}");

        // Operation on Int3T outputs TernaryArray3
        Int3T input1 = ter1TT; // 5 in decimal
        var output1 = input1 | AbsoluteValue; // TernaryArray3 111
        Console.WriteLine($"Absolute value of {input1} ({(TernaryArray3)input1}) = {(Int3T)output1} ({output1})"); // Absolute value of 5 (1TT) = 13 (111)

        // Custom operation on Int9T outputs TernaryArray9, which can be implicitly converted to Int9T
        Trit EchoA(Trit trit) => trit; // Custom operation: Identity function for Trit
        Int9T input2A = terT0T; // -10 in decimal
        Int9T output2A = input2A | EchoA; // T0T. Gets implicitly converted to Int9T
        Console.WriteLine($"terT0T Echoed = {output2A}"); // Prints -10

        // Custom operation on Int9T outputs TernaryArray9, which can be implicitly converted to Int9T
        Trit[] echoB = [Trit.Negative, Trit.Zero, Trit.Positive];
        Int9T input2B = terT0T; // -10 in decimal
        Int9T output2B = input2B | echoB; // T0T. Gets implicitly converted to Int9T
        Console.WriteLine($"terT0T Echoed = {output2B}"); // Prints -10

        // Custom operation on Int9T outputs TernaryArray9, which can be implicitly converted to Int9T
        var echoC = new UnaryTritOperator(false, null, true);
        Int9T input2C = terT0T; // -10 in decimal
        Int9T output2C = input2C | echoC; // T0T. Gets implicitly converted to Int9T
        Console.WriteLine($"terT0T Echoed = {output2C}"); // Prints -10

        // Custom operation on Int9T outputs TernaryArray9, which can be implicitly converted to Int9T
        var echoD = new UnaryTritOperator(-1, 0, 1);
        Int9T input2D = terT0T; // -10 in decimal
        Int9T output2D = input2D | echoD; // T0T. Gets implicitly converted to Int9T
        Console.WriteLine($"terT0T Echoed = {output2D}"); // Prints -10

        // Using an alias for a Tryte (27-trit integer) and the full name for the operator
        Tryte input3 = ter11T; // 11 in decimal
        long output3 = input3 | Ternary3.Operators.UnaryTritOperator.Negate; // TT1, using the full name for the operator. Result implicitly converted to long.
        Console.WriteLine($"ter11T Negated = {output3}"); // ter11T Negated = -11

        //TernaryArray input with TernaryArray27. Output is also TernaryArray27, // which can be explicitly converted to long.
        TernaryArray27 input4 = 123456789;
        var output4 = input4 | Negate;
        Console.WriteLine($"123456789 Negated = {output4} ({(int)output4})"); // Prints 000000000 T0011TT01 T1T010100 (-123456789)

        //Single Trit input
        var input5 = Trit.Negative; // T (negative)
        var output5 = input5 | IsPositive; // No. It is not positive. No translates to T (negative)
        Console.WriteLine($"Is {input5} positive? {output5}"); // Is Negative positive? Negative

        // integer input will be implicitly converted to the corresponding TernaryArray.
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
public partial class BinaryTritOperationDemo
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
        Int3T input1A = ter10T;  // 8 in decimal (10T in balanced ternary)
        Int3T input1B = ter100;  // 9 in decimal (100 in balanced ternary)
        var result1 = input1A | And | input1B; // Uses the predefined AND operation
        Console.WriteLine($"BinaryLookup And: {input1A} ({input1A:ter}) {nameof(And)} {input1B} ({input1B:ter}) = {(sbyte)result1} ({result1:ter})");
        // Output: 8 (10T) because:
        // 10T (input1A) AND 100 (input1B) = 10T
        // Position by position: 1∧1=1, 0∧0=0, T∧0=T

        // EXAMPLE 2: Custom operation on two shorts using a BinaryTritOperator
        // ---------------------------------------------------------------
        // A BinaryTritOperator defines the output for each of the 9 possible input trit combinations
        Int9T input2A = terT10; // -6 in decimal
        Int9T input2B = ter111;  // 13 in decimal
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
        Console.WriteLine($"Custom operation on short: {(TernaryArray9)input2A} {nameof(mask)} {(TernaryArray9)input2B} = {result2}");
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
        Int3T input4A = ter11T;   // 11 in decimal (11T in balanced ternary)
        Int3T input4B = ter101;  // 10 in decimal (101 in balanced ternary)
        // This table inverts the first operand and ignores the second:
        // For any first trit value, the output is the same regardless of the second trit
        var invertFirstIgnoreSecond = new BinaryTritOperator(
            Trit.Positive, Trit.Positive, Trit.Positive,  // When first trit is -1, always return 1
            Trit.Zero, Trit.Zero, Trit.Zero,              // When first trit is 0, always return 0
            Trit.Negative, Trit.Negative, Trit.Negative   // When first trit is 1, always return -1
        );
        var result4 = input4A | invertFirstIgnoreSecond | input4B;
        Console.WriteLine($"Custom operation on int: {input4A:ter} {nameof(invertFirstIgnoreSecond)} {input4B:ter} = {result4} ({result4:ter})");
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

public static partial class OverflowDemo
{
    public static void Run()
    {
        Console.WriteLine($"\r\n\r\n{nameof(OverflowDemo)}");

        // Addition and subtraction may overflow and do so in a ternary way.
        // (so instead of cutting off "binary" bits, it cuts off "ternary" trits)
        Int3T input1A = ter110; // 12 in decimal
        Int3T input1B = ter010; // 3 in decimal
        var result1 = input1A + input1B; // 110 + 010 = 1TT0. Int3T only keeps 3 trits, so TT0 = -12
        Console.WriteLine($"Overflow: {input1A} ({input1A:ter}) + {input1B} ({input1B:ter}) = {result1} ({result1:ter})"); // Overflow: 110 + 010 = 1TT0

        Int3T input2A = ter110; // 12 in decimal
        Int3T input2B = terTT0; // -12 in decimal
        var result2 = input2A * input2B; // 110 * TT0 = 101T00 (144). Int3T only keeps 3 trits, so T00 = -9
        Console.WriteLine($"Overflow: {input2A} ({input2A:ter}) * {input2B} ({input2B:ter}) = {result2} ({result2:ter})"); // Overflow: 110 * TT0 = T00

        // Addition and simple multiplication of TernaryArray3 also may overflow.
        // (Under the hood, these are often performed without conversion to binary)
        TernaryArray3 input3A = ter110; // 12 in decimal
        TernaryArray3 input3B = ter010; // 3 in decimal
        var result3 = input3A * input3B; // 110 * 010 = 11100 (36). TernaryArray3 only keeps 3 trits, so T00 = -9
        Console.WriteLine($"Overflow: {input3A} ({input3A:ter}) * {input3B} ({input3B:ter}) = {result3} ({(int)result3})");

        Int3T input4 = 25; // 10T1. trimmed to 3 trits = 0T1 or -2
        Console.WriteLine($"Overflow: 25 => {input4} ({input4:ter})"); // Overflow: 25 => -2 (0T1)

        TernaryArray3 input5 = 25; // 10T1. trimmed to 3 trits = 0T1 or -2
        Console.WriteLine($"Overflow: 25 => {input5} ({input5:ter})"); // Overflow: 25 => (0T1)

        // Shifting trits two positions in essence multiplies or divides by 9 (3^2).
        var input6 = TernaryArray9.MaxValue; // 111111111
        var result6A = input5 << 6;
        Console.WriteLine($"Shift: {input6:ter} << 6 = {result6A} ({(int)result6A})"); // Shift: 111111111 << 6 = 111000000 (9477)
        var result6B = input5 >> -6;
        Console.WriteLine($"Shift: {input6:ter} >> -6 = {result6B} ({(int)result6B})"); // Shift: 111111111 >> -6 = 111000000 (9477)
        var result6C = input5 << -6;
        Console.WriteLine($"Shift: {input6:ter} << -6 = {result6C} ({(int)result6C})"); // Shift: 111111111 << -6 = 000000111 (13)
        var result6D = input5 >> 6;
        Console.WriteLine($"Shift: {input6:ter} >> 6 = {result6D} ({(int)result6D})"); // Shift: 111111111 >> 6 = 000000111 (13)

        var input7 = Int27T.MinValue; // TTTTTTTTT TTTTTTTTT TTTTTTTTT
        var result7A = input7 << 25;
        Console.WriteLine($"Shift: {(TernaryArray27)input7} << 25 = {result7A} ({(TernaryArray27)result7A:ter})"); // Shift: TTTT..TT << 25 = TT00...00
        var result7B = input7 >> -25;
        Console.WriteLine($"Shift: {(TernaryArray27)input7} >> -25 = {result7B} ({(TernaryArray27)result7B:ter})"); // Shift: TTTT..TT >> -25 = TT00...00
        var result7C = input7 << -25;
        Console.WriteLine($"Shift: {(TernaryArray27)input7} << -25 = {result7C} ({(TernaryArray27)result7C:ter})"); // Shift: TTTT..TT << -25 = 0000...TT
        var result7D = input7 >> 25;
        Console.WriteLine($"Shift: {(TernaryArray27)input7} >> 25 = {result7D} ({(TernaryArray27)result7D:ter})"); // Shift: TTTT..TT >> 25 = 0000..TT
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

### Using TernaryArray Indexers

```csharp
// Create a TernaryArray
var array = new TernaryArray(10);

// Set values using the regular indexer
array[0] = Trit.Positive;
array[1] = Trit.Negative;

// Use C# 8.0 Index syntax for the end of the array
array[^1] = Trit.Positive;  // Sets the last trit
array[^2] = Trit.Negative;  // Sets the second-to-last trit

// Get a range of trits (creates a new TernaryArray)
var firstThree = array[0..3];  // Gets trits at positions 0, 1, and 2
var lastFour = array[^4..];    // Gets the last 4 trits
var middlePortion = array[2..^2]; // Gets all trits except the first 2 and last 2
```

### Ternary Formatting Example

You can easily print the ternary representation of numbers using the `:ter` format specifier:

```csharp
TernaryArray3 a = 7;
Int3T b = 7;
Console.WriteLine($"The ternary representation of {a} is {a:ter}");
Console.WriteLine($"The ternary representation of {b} is {b:ter}");
```

This will output the balanced ternary representation of the values.

---

## Formatting and Display

### Custom Formatting with ToString(ITernaryFormat)

You can now format any trit array (implementing `ITernaryArray`) using a custom or built-in ternary format. Use the new `ToString(ITernaryFormat format)` overload to control digit symbols, grouping, separators, and padding:

```csharp
var trits = (TernaryArray27)12345;
var format = new Ternary3.Formatting.TernaryFormat()
    .WithGroup(3, " ")
    .WithGroup(3, "-");
string formatted = trits.ToString(format); // Outputs: "00-000-010 T01-01T"
```

This allows you to display ternary numbers in a way that matches your application's needs or user preferences.

### Available Format Specifiers

When using the `ToString(string? format)` method, the following format specifier is available:

- `"ter"` - Ternary format: Formats the number as balanced ternary with 'T', '0', and '1' digits.

Examples:
```csharp
Int27T value = 42;
Console.WriteLine(value.ToString());     // Outputs the decimal value: "42"
Console.WriteLine($"{value:ter}");       // Outputs the ternary representation: "1111"
```


## Reference


### **Ternary3** namespace
- [Ternary3.BigTernaryArray](#ternary3bigternaryarray)
- [Ternary3.GenerateTernaryConstantsAttribute](#ternary3generateternaryconstantsattribute)
- [Ternary3.Int27T](#ternary3int27t)
- [Ternary3.Int3T](#ternary3int3t)
- [Ternary3.Int9T](#ternary3int9t)
- [Ternary3.ITernaryArray](#ternary3iternaryarray)
- [Ternary3.ITernaryArray<TSelf>](#ternary3iternaryarraytself)
- [Ternary3.ITernaryInteger<TSelf>](#ternary3iternaryintegertself)
- [Ternary3.ITernaryParsable<TSelf>](#ternary3iternaryparsabletself)
- [Ternary3.ITritwiseOperator<TSelf, TOther, TResult>](#ternary3itritwiseoperatortself-tother-tresult)
- [Ternary3.LookupBigTernaryArrayOperator](#ternary3lookupbigternaryarrayoperator)
- [Ternary3.LookupTernaryArray27Operator](#ternary3lookupternaryarray27operator)
- [Ternary3.LookupTernaryArray3Operator](#ternary3lookupternaryarray3operator)
- [Ternary3.LookupTernaryArray9Operator](#ternary3lookupternaryarray9operator)
- [Ternary3.LookupTernaryArrayOperator](#ternary3lookupternaryarrayoperator)
- [Ternary3.TernaryArray](#ternary3ternaryarray)
- [Ternary3.TernaryArray27](#ternary3ternaryarray27)
- [Ternary3.TernaryArray3](#ternary3ternaryarray3)
- [Ternary3.TernaryArray9](#ternary3ternaryarray9)
- [Ternary3.TernaryPadding](#ternary3ternarypadding)
- [Ternary3.Trit](#ternary3trit)

### **Ternary3.Operators** namespace
- [Ternary3.Operators.BinaryLookupTritOperator](#ternary3operatorsbinarylookuptritoperator)
- [Ternary3.Operators.BinaryMethodTritOperator](#ternary3operatorsbinarymethodtritoperator)
- [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator)
- [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator)

### **Ternary3.IO** namespace
- [Ternary3.IO.ByteToInt3TStream](#ternary3iobytetoint3tstream)
- [Ternary3.IO.Int3TStream](#ternary3ioint3tstream)
- [Ternary3.IO.Int3TToByteStream](#ternary3ioint3ttobytestream)
- [Ternary3.IO.MemoryInt3TStream](#ternary3iomemoryint3tstream)

### **Ternary3.Formatting** namespace
- [Ternary3.Formatting.InvariantTernaryFormat](#ternary3formattinginvariantternaryformat)
- [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat)
- [Ternary3.Formatting.TritGroupDefinition](#ternary3formattingtritgroupdefinition)
- [Ternary3.Formatting.ITernaryFormatter](#ternary3formattingiternaryformatter)
- [Ternary3.Formatting.MinimalTernaryFormat](#ternary3formattingminimalternaryformat)
- [Ternary3.Formatting.TernaryFormat](#ternary3formattingternaryformat)
- [Ternary3.Formatting.TernaryFormatProvider](#ternary3formattingternaryformatprovider)
- [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions)
## Ternary3.BigTernaryArray

>Represents a variable-length array of trits (ternary digits) backed by lists of ulongs.
>This class provides support for arbitrary-length balanced ternary numbers and operations.

### Static Methods

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value)

>Parses a string representation of a BigTernaryArray.
>
>**Parameters:**
>- `value`: The string value to parse.
>
>
>**Returns:**
>A BigTernaryArray representing the parsed value.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Parses a string representation of a BigTernaryArray.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>
>
>**Returns:**
>A BigTernaryArray representing the parsed value.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a BigTernaryArray.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A BigTernaryArray representing the parsed value.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a BigTernaryArray.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A BigTernaryArray representing the parsed value.


### Static Fields

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **Zero**

>Represents a BigTernaryArray with all trits set to zero.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **One**

>Represents a BigTernaryArray with value of one (a single trit set to positive).


### Constructors

#### **BigTernaryArray**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) length)

>Initializes a new instance of the BigTernaryArray class with a specified length.
>
>**Parameters:**
>- `length`: The length of the trit array. Must be non-negative.
>
>
>**Exceptions:**
>- `System.ArgumentOutOfRangeException`: Thrown when length is negative.

#### **BigTernaryArray**([Ternary3.ITernaryArray[]](#ternary3iternaryarray) arrays)

>Initializes a new instance of the BigTernaryArray class by concatenating multiple trit arrays.
>
>**Parameters:**
>- `arrays`: The arrays to concatenate.
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: Thrown when arrays is null.


### Methods

#### [System.Void](https://learn.microsoft.com/en-us/dotnet/api/system.void) **Resize**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) length)

>Resizes the TernaryArray to the specified length.
>
>**Parameters:**
>- `length`: The new length of the array. Must be non-negative.
>
>
>**Exceptions:**
>- `System.ArgumentOutOfRangeException`: Thrown if the specified length is negative.
>
>
>**Remarks:**
>
>This method adjusts the underlying storage to accommodate the new length by either adding or removing
>            elements from the internal storage lists. If the new length is the same as the current length, no operation is performed.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**()

>Returns a string representation of the TernaryArray.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format)

>Returns a string representation of the TernaryArray.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of the TernaryArray.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format, [System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of the TernaryArray.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Returns a string representation of this instance, formatted balanced ternarily according to the specified format.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([Ternary3.BigTernaryArray](#ternary3bigternaryarray) other)

>Determines whether this instance and another BigTernaryArray are equal.
>
>**Parameters:**
>- `other`: The BigTernaryArray to compare with this instance.
>
>
>**Returns:**
>true if the specified BigTernaryArray is equal to this instance; otherwise, false.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) obj)

>Determines whether this instance and a specified object are equal.
>
>**Parameters:**
>- `obj`: The object to compare with this instance.
>
>
>**Returns:**
>true if the specified object is equal to this instance; otherwise, false.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **GetHashCode**()

>Returns a hash code for this instance.
>
>**Returns:**
>A hash code for the current BigTernaryArray.
>
>
>**Remarks:**
>
>This type is mutable, but hash code calculation is allowed for equality checks.
>            The hash code is computed based on the non-zero trits in the array.


### Properties

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **Length** { get; set; }

>Gets or sets the number of trits in this array.

#### [Ternary3.Trit](#ternary3trit) **this[[System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) index]** { get; set; }

No documentation available.

#### [Ternary3.Trit](#ternary3trit) **this[[System.Index](https://learn.microsoft.com/en-us/dotnet/api/system.index) index]** { get; set; }

No documentation available.

#### [Ternary3.ITernaryArray](#ternary3iternaryarray) **this[[System.Range](https://learn.microsoft.com/en-us/dotnet/api/system.range) range]** { get; }

No documentation available.


### Operators

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **operator |**([Ternary3.BigTernaryArray](#ternary3bigternaryarray) array, [System.Func<Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-2) operation)

>Applies a unary operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray with the operation applied to each trit.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **operator |**([Ternary3.BigTernaryArray](#ternary3bigternaryarray) array, [Ternary3.Trit[]](#ternary3trit) table)

>Applies a lookup table operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `table`: The lookup table containing the transformation values.
>
>
>**Returns:**
>A new TernaryArray with the lookup operation applied to each trit.

#### [Ternary3.LookupBigTernaryArrayOperator](#ternary3lookupbigternaryarrayoperator) **operator |**([Ternary3.BigTernaryArray](#ternary3bigternaryarray) array, [System.Func<Ternary3.Trit, Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-3) operation)

>Applies a unary operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray with the operation applied to each trit.

#### [Ternary3.LookupBigTernaryArrayOperator](#ternary3lookupbigternaryarrayoperator) **operator |**([Ternary3.BigTernaryArray](#ternary3bigternaryarray) array, [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) table)

>Creates a binary operation context for this array.
>
>**Parameters:**
>- `array`: The source array.
>- `table`: The binary operation lookup table.
>
>
>**Returns:**
>A binary operation context that can be used with another array.

#### [Ternary3.LookupBigTernaryArrayOperator](#ternary3lookupbigternaryarrayoperator) **operator |**([Ternary3.BigTernaryArray](#ternary3bigternaryarray) array, [Ternary3.Trit[,]](#ternary3trit) table)

>Applies a unary operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray with the operation applied to each trit.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **operator &lt;&lt;**([Ternary3.BigTernaryArray](#ternary3bigternaryarray) array, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shift)

>Performs a left bitwise shift on the trit array.
>
>**Parameters:**
>- `array`: The source array.
>- `shift`: The number of positions to shift.
>
>
>**Returns:**
>A new TernaryArray with the bits shifted to the left.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **operator &gt;&gt;**([Ternary3.BigTernaryArray](#ternary3bigternaryarray) array, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shift)

>Performs a right bitwise shift on the trit array.
>
>**Parameters:**
>- `array`: The source array.
>- `shift`: The number of positions to shift.
>
>
>**Returns:**
>A new TernaryArray with the bits shifted to the right.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **operator +**([Ternary3.BigTernaryArray](#ternary3bigternaryarray) value1, [Ternary3.BigTernaryArray](#ternary3bigternaryarray) value2)

>Adds two TernaryArray values together.
>
>**Parameters:**
>- `value1`: The first value to add.
>- `value2`: The second value to add.
>
>
>**Returns:**
>A new TernaryArray representing the sum of the two values.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **operator -**([Ternary3.BigTernaryArray](#ternary3bigternaryarray) value1, [Ternary3.BigTernaryArray](#ternary3bigternaryarray) value2)

>Subtracts one TernaryArray value from another.
>
>**Parameters:**
>- `value1`: The value to subtract from.
>- `value2`: The value to subtract.
>
>
>**Returns:**
>A new TernaryArray representing the difference between the two values.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **operator &ast;**([Ternary3.BigTernaryArray](#ternary3bigternaryarray) value1, [Ternary3.BigTernaryArray](#ternary3bigternaryarray) value2)

>Multiplies one TernaryArray value from another.
>
>**Parameters:**
>- `value1`: The value to subtract from.
>- `value2`: The value to subtract.
>
>
>**Returns:**
>A new TernaryArray representing the product between the two values.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **operator implicit**([System.Numerics.BigInteger](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger) value)

>Implicit conversion from BigInteger to TernaryArray.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **operator implicit**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) value)

>Implicit conversion from BigInteger to TernaryArray.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **operator implicit**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value)

>Implicit conversion from BigInteger to TernaryArray.

#### [System.Numerics.BigInteger](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger) **operator implicit**([Ternary3.BigTernaryArray](#ternary3bigternaryarray) array)

>Implicit conversion from BigInteger to TernaryArray.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **operator explicit**([Ternary3.BigTernaryArray](#ternary3bigternaryarray) array)

>Explicit conversion from TernaryArray to long.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **operator explicit**([Ternary3.BigTernaryArray](#ternary3bigternaryarray) array)

>Explicit conversion from TernaryArray to long.


## Ternary3.GenerateTernaryConstantsAttribute

>Attribute to indicate that ternary constants should be generated for the target type or member.
>
>**Parameters:**
>- `enabled`: Whether or not to generate ternary constants.
>            This can be used to override the assemby level setting for classes or structs.

### Constructors

#### **GenerateTernaryConstantsAttribute**([System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) enabled)

>Attribute to indicate that ternary constants should be generated for the target type or member.
>
>**Parameters:**
>- `enabled`: Whether or not to generate ternary constants.
>            This can be used to override the assemby level setting for classes or structs.


### Properties

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Enabled** { get; }

>Gets whether ternary constant generation is enabled.


## Ternary3.Int27T

>Represents a 27-trit  signed integer, modeled after the `Int64` type.

### Static Methods

#### [Ternary3.Int27T](#ternary3int27t) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value)

>Parses a string representation of a Int27T.
>
>**Parameters:**
>- `value`: The string value to parse.
>
>
>**Returns:**
>A Int27T representing the parsed value.

#### [Ternary3.Int27T](#ternary3int27t) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Parses a string representation of a Int27T.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>
>
>**Returns:**
>A Int27T representing the parsed value.

#### [Ternary3.Int27T](#ternary3int27t) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a Int27T.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A Int27T representing the parsed value.

#### [Ternary3.Int27T](#ternary3int27t) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a Int27T.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A Int27T representing the parsed value.

#### [Ternary3.Int27T](#ternary3int27t) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) s, [System.Globalization.NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles) style)

>Converts the string representation of a number in a specified style to its [Int27T](#ternary3.int27t) equivalent.
>
>**Parameters:**
>- `s`: A string containing a number to convert.
>- `style`: A bitwise combination of enumeration values that indicate the style elements that can be present in `s` .
>
>
>**Returns:**
>A [Int27T](#ternary3.int27t) equivalent to the number contained in `s` .
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: `s` is `null` .
>- `System.ArgumentException`: `style` is not a `NumberStyles` value or `style` includes the [AllowHexSpecifier](#f:system.globalization.numberstyles.allowhexspecifier) value.
>- `System.FormatException`: `s` is not in a format compliant with `style` .
>- `System.OverflowException`: `s` represents a number less than [MinValue](#f:ternary3.int27t.minvalue) or greater than [MaxValue](#f:ternary3.int27t.maxvalue) .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **TryParse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) s, [Ternary3.Int27T&](#ternary3int27t) result)

>Tries to convert the string representation of a number to its [Int27T](#ternary3.int27t) equivalent, and returns a value that indicates whether the conversion succeeded.
>
>**Parameters:**
>- `s`: A string containing a number to convert.
>- `result`: When this method returns, contains the [Int27T](#ternary3.int27t) value equivalent to the number contained in `s` if the conversion succeeded, or zero if the conversion failed. This parameter is passed uninitialized.
>
>
>**Returns:**
>`true` if `s` was converted successfully; otherwise, `false` .


### Static Fields

#### [Ternary3.Int27T](#ternary3int27t) **MaxValue**

>Represents the largest possible value of a [Int27T](#ternary3.int27t) . This field is static readonly.

#### [Ternary3.Int27T](#ternary3int27t) **MinValue**

>Represents the smallest possible value of a [Int27T](#ternary3.int27t) . This field is static readonly.
>
>**Examples:**
>
>```csharp
>// This code outputs TTTTTTTTTTTTTTTTTTTTTTTTTT1.
>Int27T value = Int27T.MinValue + 2;
>console.WriteLine($"The value is: {value:ter}");
>```

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **MaxValueConstant**

>Represents the maximum value of a [Int27T](#ternary3.int27t) , expressed as a `Int64` This field is constant.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **MinValueConstant**

>Represents the minimum value of a [Int27T](#ternary3.int27t) , expressed as a `Int64` This field is constant.


### Methods

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) obj)

>Returns a value indicating whether this instance is equal to a specified object.
>
>**Parameters:**
>- `obj`: An object to compare with this instance.
>
>
>**Returns:**
>`true` if `obj` is an instance of [Int27T](#ternary3.int27t) or a compatible numeric type
>            and equals the value of this instance; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([Ternary3.Int27T](#ternary3int27t) other)

>Returns a value indicating whether this instance is equal to a specified [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `other`: A [Int27T](#ternary3.int27t) value to compare to this instance.
>
>
>**Returns:**
>`true` if `other` has the same value as this instance; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) other)

>Returns a value indicating whether this instance is equal to a specified `Int64` value.
>
>**Parameters:**
>- `other`: A `Int64` value to compare to this instance.
>
>
>**Returns:**
>`true` if `other` has the same value as this instance; otherwise, `false` .

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **GetHashCode**()

>Returns the hash code for this instance.
>
>**Returns:**
>A 32-bit signed integer hash code.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**()

>Returns a string representation of this instance.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format)

>Returns a string representation of this instance.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of this instance.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format, [System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of this instance.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Returns a string representation of this instance, formatted balanced ternarily according to the specified format.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **CompareTo**([System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) obj)

>Compares this instance to a specified object and returns an indication of their relative values.
>
>**Parameters:**
>- `obj`: An object to compare, or `null` .
>
>
>**Returns:**
>A signed number indicating the relative values of this instance and `obj` .
>            Return Value Description:
>            Less than zero: This instance is less than `obj` .
>            Zero: This instance is equal to `obj` .
>            Greater than zero: This instance is greater than `obj` or `obj` is `null` .
>
>
>**Exceptions:**
>- `System.ArgumentException`: `obj` is not a [Int27T](#ternary3.int27t) or a type that can be converted to an integer.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **CompareTo**([Ternary3.Int27T](#ternary3int27t) other)

>Compares this instance to a specified [Int27T](#ternary3.int27t) object and returns an indication of their relative values.
>
>**Parameters:**
>- `other`: An [Int27T](#ternary3.int27t) object to compare.
>
>
>**Returns:**
>A signed number indicating the relative values of this instance and `other` .
>            Return Value Description:
>            Less than zero: This instance is less than `other` .
>            Zero: This instance is equal to `other` .
>            Greater than zero: This instance is greater than `other` .

#### [System.TypeCode](https://learn.microsoft.com/en-us/dotnet/api/system.typecode) **GetTypeCode**()

>Gets the `TypeCode` for this instance.
>
>**Returns:**
>[Int32](#f:system.typecode.int32) .


### Properties

#### [Ternary3.Trit](#ternary3trit) **this[[System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) index]** { get; }

No documentation available.

#### [Ternary3.Trit](#ternary3trit) **this[[System.Index](https://learn.microsoft.com/en-us/dotnet/api/system.index) index]** { get; }

No documentation available.


### Operators

#### [Ternary3.Int27T](#ternary3int27t) **operator implicit**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) value)

>Defines an implicit conversion of a Int64 to a [Int27T](#ternary3.int27t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int27T](#ternary3.int27t) that represents the converted value.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **operator implicit**([Ternary3.Int27T](#ternary3int27t) value)

>Defines an implicit conversion of a Int64 to a [Int27T](#ternary3.int27t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int27T](#ternary3.int27t) that represents the converted value.

#### [Ternary3.Int27T](#ternary3int27t) **operator implicit**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value)

>Defines an implicit conversion of a Int64 to a [Int27T](#ternary3.int27t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int27T](#ternary3.int27t) that represents the converted value.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **operator explicit**([Ternary3.Int27T](#ternary3int27t) value)

>Defines an explicit conversion of a Int27T to a `Int32` .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A `Int32` that represents the converted value.

#### [Ternary3.Int27T](#ternary3int27t) **operator explicit**([System.Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128) value)

>Defines an explicit conversion of a Int27T to a `Int32` .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A `Int32` that represents the converted value.

#### [System.Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128) **operator implicit**([Ternary3.Int27T](#ternary3int27t) value)

>Defines an implicit conversion of a Int64 to a [Int27T](#ternary3.int27t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int27T](#ternary3.int27t) that represents the converted value.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator ==**([System.IConvertible](https://learn.microsoft.com/en-us/dotnet/api/system.iconvertible) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether two [Int27T](#ternary3.int27t) instances are equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are equal; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator !=**([System.IConvertible](https://learn.microsoft.com/en-us/dotnet/api/system.iconvertible) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether two [Int27T](#ternary3.int27t) instances are not equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are not equal; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator ==**([Ternary3.Int27T](#ternary3int27t) left, [System.IConvertible](https://learn.microsoft.com/en-us/dotnet/api/system.iconvertible) right)

>Returns a value indicating whether two [Int27T](#ternary3.int27t) instances are equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are equal; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator !=**([Ternary3.Int27T](#ternary3int27t) left, [System.IConvertible](https://learn.microsoft.com/en-us/dotnet/api/system.iconvertible) right)

>Returns a value indicating whether two [Int27T](#ternary3.int27t) instances are not equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are not equal; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator ==**([Ternary3.Int27T](#ternary3int27t) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether two [Int27T](#ternary3.int27t) instances are equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are equal; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator !=**([Ternary3.Int27T](#ternary3int27t) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether two [Int27T](#ternary3.int27t) instances are not equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are not equal; otherwise, `false` .

#### [Ternary3.Int27T](#ternary3int27t) **operator +**([Ternary3.Int27T](#ternary3int27t) left, [Ternary3.Int27T](#ternary3int27t) right)

>Adds two ternary numbers together, maintaining the original numeric type.
>
>**Parameters:**
>- `left`: The first ternary number.
>- `right`: The second ternary number.
>
>
>**Returns:**
>A new ternary number of the same type containing the sum.

#### [Ternary3.Int27T](#ternary3int27t) **operator -**([Ternary3.Int27T](#ternary3int27t) left, [Ternary3.Int27T](#ternary3int27t) right)

>Subtracts the second ternary number from the first, maintaining the original numeric type.
>
>**Parameters:**
>- `left`: The number to subtract from.
>- `right`: The number to subtract.
>
>
>**Returns:**
>A new ternary number of the same type containing the difference.

#### [Ternary3.Int27T](#ternary3int27t) **operator &ast;**([Ternary3.Int27T](#ternary3int27t) left, [Ternary3.Int27T](#ternary3int27t) right)

>Multiplies two ternary numbers together, maintaining the original numeric type.
>
>**Parameters:**
>- `left`: The first number to multiply.
>- `right`: The second number to multiply.
>
>
>**Returns:**
>A new ternary number of the same type containing the product.

#### [Ternary3.Int27T](#ternary3int27t) **operator /**([Ternary3.Int27T](#ternary3int27t) left, [Ternary3.Int27T](#ternary3int27t) right)

>Divides the first ternary number by the second, maintaining the original numeric type.
>
>**Parameters:**
>- `left`: The dividend.
>- `right`: The divisor.
>
>
>**Returns:**
>A new ternary number of the same type containing the quotient.

#### [Ternary3.Int27T](#ternary3int27t) **operator %**([Ternary3.Int27T](#ternary3int27t) left, [Ternary3.Int27T](#ternary3int27t) right)

>Computes the remainder after dividing the first ternary number by the second.
>
>**Parameters:**
>- `left`: The dividend.
>- `right`: The divisor.
>
>
>**Returns:**
>A new ternary number of the same type containing the remainder.

#### [Ternary3.Int27T](#ternary3int27t) **operator -**([Ternary3.Int27T](#ternary3int27t) value)

>Returns the negation of a ternary number, maintaining the original numeric type.
>
>**Parameters:**
>- `value`: The ternary number to negate.
>
>
>**Returns:**
>A new ternary number of the same type containing the negated value.

#### [Ternary3.Int27T](#ternary3int27t) **operator +**([Ternary3.Int27T](#ternary3int27t) value)

>Returns the same ternary number (unary plus operator).
>
>**Parameters:**
>- `value`: The ternary number.
>
>
>**Returns:**
>The same ternary number.

#### [Ternary3.Int27T](#ternary3int27t) **operator +**([Ternary3.Int27T](#ternary3int27t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Adds a ternary number and a native numeric value, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The ternary number.
>- `right`: The native numeric value to add.
>
>
>**Returns:**
>A new ternary number containing the sum.

#### [Ternary3.Int27T](#ternary3int27t) **operator +**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int27T](#ternary3int27t) right)

>Adds a native numeric value and a ternary number, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The native numeric value to add.
>- `right`: The ternary number.
>
>
>**Returns:**
>A new ternary number containing the sum.

#### [Ternary3.Int27T](#ternary3int27t) **operator -**([Ternary3.Int27T](#ternary3int27t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Subtracts a native numeric value from a ternary number, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The ternary number to subtract from.
>- `right`: The native numeric value to subtract.
>
>
>**Returns:**
>A new ternary number containing the difference.

#### [Ternary3.Int27T](#ternary3int27t) **operator -**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int27T](#ternary3int27t) right)

>Subtracts a ternary number from a native numeric value, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The native numeric value.
>- `right`: The ternary number to subtract.
>
>
>**Returns:**
>A new ternary number containing the difference.

#### [Ternary3.Int27T](#ternary3int27t) **operator &ast;**([Ternary3.Int27T](#ternary3int27t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Multiplies a ternary number by a native numeric value, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The ternary number.
>- `right`: The native numeric value to multiply by.
>
>
>**Returns:**
>A new ternary number of the same type containing the product.

#### [Ternary3.Int27T](#ternary3int27t) **operator &ast;**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int27T](#ternary3int27t) right)

>Multiplies a native numeric value by a ternary number, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The native numeric value.
>- `right`: The ternary number to multiply by.
>
>
>**Returns:**
>A new ternary number containing the product.

#### [Ternary3.Int27T](#ternary3int27t) **operator /**([Ternary3.Int27T](#ternary3int27t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Divides a ternary number by a native numeric value, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The ternary number to divide.
>- `right`: The native numeric value to divide by.
>
>
>**Returns:**
>A new ternary number of the same type containing the quotient.

#### [Ternary3.Int27T](#ternary3int27t) **operator /**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int27T](#ternary3int27t) right)

>Divides a native numeric value by a ternary number, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The native numeric value to divide.
>- `right`: The ternary number to divide by.
>
>
>**Returns:**
>A new ternary number containing the quotient.

#### [Ternary3.Int27T](#ternary3int27t) **operator %**([Ternary3.Int27T](#ternary3int27t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Computes the remainder after dividing a ternary number by a native numeric value, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The ternary number to divide.
>- `right`: The native numeric value to divide by.
>
>
>**Returns:**
>A new ternary number of the same type containing the remainder.

#### [Ternary3.Int27T](#ternary3int27t) **operator %**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int27T](#ternary3int27t) right)

>Computes the remainder after dividing a native numeric value by a ternary number, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The native numeric value to divide.
>- `right`: The ternary number to divide by.
>
>
>**Returns:**
>A new ternary number containing the remainder.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int27T](#ternary3int27t) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether one [Int27T](#ternary3.int27t) is greater than another.
>
>**Parameters:**
>- `left`: The first value to compare.
>- `right`: The second value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int27T](#ternary3int27t) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether one [Int27T](#ternary3.int27t) is less than another.
>
>**Parameters:**
>- `left`: The first value to compare.
>- `right`: The second value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int27T](#ternary3int27t) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether one [Int27T](#ternary3.int27t) is greater than or equal to another.
>
>**Parameters:**
>- `left`: The first value to compare.
>- `right`: The second value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int27T](#ternary3int27t) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether one [Int27T](#ternary3.int27t) is less than or equal to another.
>
>**Parameters:**
>- `left`: The first value to compare.
>- `right`: The second value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int27T](#ternary3int27t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than a `SByte` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `SByte` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int27T](#ternary3int27t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than a `SByte` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `SByte` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than or equal to a `SByte` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `SByte` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than or equal to a `SByte` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `SByte` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `SByte` value is greater than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `SByte` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `SByte` value is less than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `SByte` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `SByte` value is greater than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `SByte` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `SByte` value is less than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `SByte` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int27T](#ternary3int27t) left, [System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than a `Byte` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Byte` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int27T](#ternary3int27t) left, [System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than a `Byte` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Byte` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than or equal to a `Byte` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Byte` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than or equal to a `Byte` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Byte` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Byte` value is greater than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Byte` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Byte` value is less than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Byte` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Byte` value is greater than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Byte` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Byte` value is less than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Byte` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int27T](#ternary3int27t) left, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than a `Int32` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Int32` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int27T](#ternary3int27t) left, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than a `Int32` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Int32` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than or equal to a `Int32` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Int32` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than or equal to a `Int32` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Int32` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Int32` value is greater than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Int32` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Int32` value is less than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Int32` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Int32` value is greater than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Int32` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Int32` value is less than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Int32` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int27T](#ternary3int27t) left, [System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than a `UInt32` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `UInt32` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int27T](#ternary3int27t) left, [System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than a `UInt32` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `UInt32` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than or equal to a `UInt32` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `UInt32` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than or equal to a `UInt32` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `UInt32` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `UInt32` value is greater than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `UInt32` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `UInt32` value is less than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `UInt32` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `UInt32` value is greater than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `UInt32` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `UInt32` value is less than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `UInt32` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int27T](#ternary3int27t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than a `Int16` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Int16` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int27T](#ternary3int27t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than a `Int16` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Int16` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than or equal to a `Int16` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Int16` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than or equal to a `Int16` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Int16` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Int16` value is greater than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Int16` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Int16` value is less than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Int16` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Int16` value is greater than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Int16` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Int16` value is less than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Int16` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int27T](#ternary3int27t) left, [System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than a `UInt16` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `UInt16` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int27T](#ternary3int27t) left, [System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than a `UInt16` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `UInt16` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than or equal to a `UInt16` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `UInt16` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than or equal to a `UInt16` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `UInt16` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `UInt16` value is greater than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `UInt16` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `UInt16` value is less than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `UInt16` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `UInt16` value is greater than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `UInt16` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `UInt16` value is less than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `UInt16` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int27T](#ternary3int27t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than a `Int64` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Int64` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int27T](#ternary3int27t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than a `Int64` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Int64` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than or equal to a `Int64` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Int64` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than or equal to a `Int64` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Int64` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Int64` value is greater than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Int64` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Int64` value is less than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Int64` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Int64` value is greater than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Int64` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Int64` value is less than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Int64` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int27T](#ternary3int27t) left, [System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than a `UInt64` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `UInt64` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int27T](#ternary3int27t) left, [System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than a `UInt64` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `UInt64` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than or equal to a `UInt64` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `UInt64` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than or equal to a `UInt64` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `UInt64` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `UInt64` value is greater than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `UInt64` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `UInt64` value is less than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `UInt64` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `UInt64` value is greater than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `UInt64` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `UInt64` value is less than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `UInt64` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int27T](#ternary3int27t) left, [System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than a `Single` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Single` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int27T](#ternary3int27t) left, [System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than a `Single` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Single` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than or equal to a `Single` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Single` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than or equal to a `Single` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Single` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Single` value is greater than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Single` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Single` value is less than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Single` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Single` value is greater than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Single` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Single` value is less than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Single` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int27T](#ternary3int27t) left, [System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than a `Double` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Double` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int27T](#ternary3int27t) left, [System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than a `Double` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Double` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than or equal to a `Double` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Double` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than or equal to a `Double` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Double` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Double` value is greater than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Double` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Double` value is less than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Double` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Double` value is greater than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Double` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Double` value is less than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Double` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int27T](#ternary3int27t) left, [System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than a `Decimal` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Decimal` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int27T](#ternary3int27t) left, [System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than a `Decimal` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Decimal` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than or equal to a `Decimal` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Decimal` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than or equal to a `Decimal` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `Decimal` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Decimal` value is greater than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Decimal` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Decimal` value is less than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Decimal` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Decimal` value is greater than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Decimal` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `Decimal` value is less than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `Decimal` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int27T](#ternary3int27t) left, [System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than a `IComparable` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `IComparable` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int27T](#ternary3int27t) left, [System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than a `IComparable` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `IComparable` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is greater than or equal to a `IComparable` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `IComparable` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int27T](#ternary3int27t) left, [System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) right)

>Returns a value indicating whether a [Int27T](#ternary3.int27t) value is less than or equal to a `IComparable` value.
>
>**Parameters:**
>- `left`: The [Int27T](#ternary3.int27t) value to compare.
>- `right`: The `IComparable` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `IComparable` value is greater than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `IComparable` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `IComparable` value is less than a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `IComparable` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `IComparable` value is greater than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `IComparable` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) left, [Ternary3.Int27T](#ternary3int27t) right)

>Returns a value indicating whether a `IComparable` value is less than or equal to a [Int27T](#ternary3.int27t) value.
>
>**Parameters:**
>- `left`: The `IComparable` value to compare.
>- `right`: The [Int27T](#ternary3.int27t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [Ternary3.Int27T](#ternary3int27t) **operator ++**([Ternary3.Int27T](#ternary3int27t) value)

>Increments a ternary number by one, maintaining the original numeric type.
>
>**Parameters:**
>- `value`: The ternary number to increment.
>
>
>**Returns:**
>A new ternary number with a value one greater than `value` .

#### [Ternary3.Int27T](#ternary3int27t) **operator --**([Ternary3.Int27T](#ternary3int27t) value)

>Decrements a ternary number by one, maintaining the original numeric type.
>
>**Parameters:**
>- `value`: The ternary number to decrement.
>
>
>**Returns:**
>A new ternary number with a value one less than `value` .

#### [Ternary3.Int27T](#ternary3int27t) **operator &lt;&lt;**([Ternary3.Int27T](#ternary3int27t) value, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shiftAmount)

>Performs a left shift operation on the ternary number, maintaining the original numeric type.
>
>**Parameters:**
>- `value`: The ternary number to shift.
>- `shiftAmount`: The number of positions to shift left. Negative values result in a right shift.
>
>
>**Returns:**
>A new ternary number of the same type containing the shifted value.

#### [Ternary3.Int27T](#ternary3int27t) **operator &gt;&gt;**([Ternary3.Int27T](#ternary3int27t) value, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shiftAmount)

>Performs a right shift operation on the ternary number, maintaining the original numeric type.
>
>**Parameters:**
>- `value`: The ternary number to shift.
>- `shiftAmount`: The number of positions to shift right. Negative values result in a left shift.
>
>
>**Returns:**
>A new ternary number of the same type containing the shifted value.

#### [Ternary3.Int27T](#ternary3int27t) **operator &gt;&gt;&gt;**([Ternary3.Int27T](#ternary3int27t) value, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shiftAmount)

>Performs an unsigned right shift operation on the ternary number, maintaining the original numeric type.
>In this implementation, it behaves the same as the signed right shift.
>
>**Parameters:**
>- `value`: The ternary number to shift.
>- `shiftAmount`: The number of positions to shift right. Negative values result in a left shift.
>
>
>**Returns:**
>A new ternary number of the same type containing the shifted value.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator |**([Ternary3.Int27T](#ternary3int27t) value, [System.Func<Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-2) operation)

>Applies a unary operation to each trit in this ternary number. This operation converts the number to a TernaryArray.
>
>**Parameters:**
>- `value`: The ternary number to convert and operate on.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray containing the result of applying the operation.
>
>
>**Remarks:**
>
>This operation causes an implicit conversion to TernaryArray before applying the operation.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator |**([Ternary3.Int27T](#ternary3int27t) value, [Ternary3.Trit[]](#ternary3trit) trits)

>Combines each trit in this ternary number with the corresponding trit in the provided array. This operation converts the number to a TernaryArray.
>
>**Parameters:**
>- `value`: The ternary number to convert and combine.
>- `trits`: The array of trits to combine with.
>
>
>**Returns:**
>A new TernaryArray containing the result of the operation.
>
>
>**Remarks:**
>
>This operation causes an implicit conversion to TernaryArray before combining with the provided trits.


## Ternary3.Int3T

>Represents a 3-trit  signed integer, modeled after the `SByte` type.

### Static Methods

#### [Ternary3.Int3T](#ternary3int3t) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value)

>Parses a string representation of a Int3T.
>
>**Parameters:**
>- `value`: The string value to parse.
>
>
>**Returns:**
>A Int3T representing the parsed value.

#### [Ternary3.Int3T](#ternary3int3t) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Parses a string representation of a Int3T.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>
>
>**Returns:**
>A Int3T representing the parsed value.

#### [Ternary3.Int3T](#ternary3int3t) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a Int3T.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A Int3T representing the parsed value.

#### [Ternary3.Int3T](#ternary3int3t) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a Int3T.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A Int3T representing the parsed value.

#### [Ternary3.Int3T](#ternary3int3t) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) s, [System.Globalization.NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles) style)

>Converts the string representation of a number in a specified style to its [Int3T](#ternary3.int3t) equivalent.
>
>**Parameters:**
>- `s`: A string containing a number to convert.
>- `style`: A bitwise combination of enumeration values that indicate the style elements that can be present in `s` .
>
>
>**Returns:**
>A [Int3T](#ternary3.int3t) equivalent to the number contained in `s` .
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: `s` is `null` .
>- `System.ArgumentException`: `style` is not a `NumberStyles` value or `style` includes the [AllowHexSpecifier](#f:system.globalization.numberstyles.allowhexspecifier) value.
>- `System.FormatException`: `s` is not in a format compliant with `style` .
>- `System.OverflowException`: `s` represents a number less than [MinValue](#f:ternary3.int3t.minvalue) or greater than [MaxValue](#f:ternary3.int3t.maxvalue) .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **TryParse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) s, [Ternary3.Int3T&](#ternary3int3t) result)

>Tries to convert the string representation of a number to its [Int3T](#ternary3.int3t) equivalent, and returns a value that indicates whether the conversion succeeded.
>
>**Parameters:**
>- `s`: A string containing a number to convert.
>- `result`: When this method returns, contains the [Int3T](#ternary3.int3t) value equivalent to the number contained in `s` if the conversion succeeded, or zero if the conversion failed. This parameter is passed uninitialized.
>
>
>**Returns:**
>`true` if `s` was converted successfully; otherwise, `false` .


### Static Fields

#### [Ternary3.Int3T](#ternary3int3t) **MaxValue**

>Represents the largest possible value of a [Int3T](#ternary3.int3t) . This field is static readonly.

#### [Ternary3.Int3T](#ternary3int3t) **MinValue**

>Represents the smallest possible value of a [Int3T](#ternary3.int3t) . This field is static readonly.
>
>**Examples:**
>
>```csharp
>// This code outputs TT1.
>Int3T value = Int3T.MinValue + 2;
>console.WriteLine($"The value is: {value:ter}");
>```

#### [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) **MaxValueConstant**

>Represents the maximum value of a [Int3T](#ternary3.int3t) , expressed as a `SByte` This field is constant.

#### [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) **MinValueConstant**

>Represents the minimum value of a [Int3T](#ternary3.int3t) , expressed as a `SByte` This field is constant.


### Methods

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) obj)

>Returns a value indicating whether this instance is equal to a specified object.
>
>**Parameters:**
>- `obj`: An object to compare with this instance.
>
>
>**Returns:**
>`true` if `obj` is an instance of [Int3T](#ternary3.int3t) or a compatible numeric type
>            and equals the value of this instance; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([Ternary3.Int3T](#ternary3int3t) other)

>Returns a value indicating whether this instance is equal to a specified [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `other`: A [Int3T](#ternary3.int3t) value to compare to this instance.
>
>
>**Returns:**
>`true` if `other` has the same value as this instance; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) other)

>Returns a value indicating whether this instance is equal to a specified `SByte` value.
>
>**Parameters:**
>- `other`: A `SByte` value to compare to this instance.
>
>
>**Returns:**
>`true` if `other` has the same value as this instance; otherwise, `false` .

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **GetHashCode**()

>Returns the hash code for this instance.
>
>**Returns:**
>A 32-bit signed integer hash code.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**()

>Returns a string representation of this instance.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format)

>Returns a string representation of this instance.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of this instance.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format, [System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of this instance.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Returns a string representation of this instance, formatted balanced ternarily according to the specified format.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **CompareTo**([System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) obj)

>Compares this instance to a specified object and returns an indication of their relative values.
>
>**Parameters:**
>- `obj`: An object to compare, or `null` .
>
>
>**Returns:**
>A signed number indicating the relative values of this instance and `obj` .
>            Return Value Description:
>            Less than zero: This instance is less than `obj` .
>            Zero: This instance is equal to `obj` .
>            Greater than zero: This instance is greater than `obj` or `obj` is `null` .
>
>
>**Exceptions:**
>- `System.ArgumentException`: `obj` is not a [Int3T](#ternary3.int3t) or a type that can be converted to an integer.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **CompareTo**([Ternary3.Int3T](#ternary3int3t) other)

>Compares this instance to a specified [Int3T](#ternary3.int3t) object and returns an indication of their relative values.
>
>**Parameters:**
>- `other`: An [Int3T](#ternary3.int3t) object to compare.
>
>
>**Returns:**
>A signed number indicating the relative values of this instance and `other` .
>            Return Value Description:
>            Less than zero: This instance is less than `other` .
>            Zero: This instance is equal to `other` .
>            Greater than zero: This instance is greater than `other` .

#### [System.TypeCode](https://learn.microsoft.com/en-us/dotnet/api/system.typecode) **GetTypeCode**()

>Gets the `TypeCode` for this instance.
>
>**Returns:**
>[Int32](#f:system.typecode.int32) .


### Properties

#### [Ternary3.Trit](#ternary3trit) **this[[System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) index]** { get; }

No documentation available.

#### [Ternary3.Trit](#ternary3trit) **this[[System.Index](https://learn.microsoft.com/en-us/dotnet/api/system.index) index]** { get; }

No documentation available.


### Operators

#### [Ternary3.Int3T](#ternary3int3t) **operator implicit**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) value)

>Defines an implicit conversion of a SByte to a [Int3T](#ternary3.int3t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int3T](#ternary3.int3t) that represents the converted value.

#### [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) **operator implicit**([Ternary3.Int3T](#ternary3int3t) value)

>Defines an implicit conversion of a SByte to a [Int3T](#ternary3.int3t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int3T](#ternary3.int3t) that represents the converted value.

#### [Ternary3.Int3T](#ternary3int3t) **operator explicit**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value)

>Defines an explicit conversion of a Int32 to a [Int3T](#ternary3.int3t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int3T](#ternary3.int3t) that represents the converted value.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **operator implicit**([Ternary3.Int3T](#ternary3int3t) value)

>Defines an implicit conversion of a SByte to a [Int3T](#ternary3.int3t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int3T](#ternary3.int3t) that represents the converted value.

#### [Ternary3.Int3T](#ternary3int3t) **operator explicit**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) value)

>Defines an explicit conversion of a Int32 to a [Int3T](#ternary3.int3t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int3T](#ternary3.int3t) that represents the converted value.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **operator implicit**([Ternary3.Int3T](#ternary3int3t) value)

>Defines an implicit conversion of a SByte to a [Int3T](#ternary3.int3t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int3T](#ternary3.int3t) that represents the converted value.

#### [Ternary3.Int3T](#ternary3int3t) **operator explicit**([System.Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128) value)

>Defines an explicit conversion of a Int32 to a [Int3T](#ternary3.int3t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int3T](#ternary3.int3t) that represents the converted value.

#### [System.Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128) **operator implicit**([Ternary3.Int3T](#ternary3int3t) value)

>Defines an implicit conversion of a SByte to a [Int3T](#ternary3.int3t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int3T](#ternary3.int3t) that represents the converted value.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator ==**([System.IConvertible](https://learn.microsoft.com/en-us/dotnet/api/system.iconvertible) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether two [Int3T](#ternary3.int3t) instances are equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are equal; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator !=**([System.IConvertible](https://learn.microsoft.com/en-us/dotnet/api/system.iconvertible) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether two [Int3T](#ternary3.int3t) instances are not equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are not equal; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator ==**([Ternary3.Int3T](#ternary3int3t) left, [System.IConvertible](https://learn.microsoft.com/en-us/dotnet/api/system.iconvertible) right)

>Returns a value indicating whether two [Int3T](#ternary3.int3t) instances are equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are equal; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator !=**([Ternary3.Int3T](#ternary3int3t) left, [System.IConvertible](https://learn.microsoft.com/en-us/dotnet/api/system.iconvertible) right)

>Returns a value indicating whether two [Int3T](#ternary3.int3t) instances are not equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are not equal; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator ==**([Ternary3.Int3T](#ternary3int3t) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether two [Int3T](#ternary3.int3t) instances are equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are equal; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator !=**([Ternary3.Int3T](#ternary3int3t) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether two [Int3T](#ternary3.int3t) instances are not equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are not equal; otherwise, `false` .

#### [Ternary3.Int3T](#ternary3int3t) **operator +**([Ternary3.Int3T](#ternary3int3t) left, [Ternary3.Int3T](#ternary3int3t) right)

>Adds two ternary numbers together, maintaining the original numeric type.
>
>**Parameters:**
>- `left`: The first ternary number.
>- `right`: The second ternary number.
>
>
>**Returns:**
>A new ternary number of the same type containing the sum.

#### [Ternary3.Int3T](#ternary3int3t) **operator -**([Ternary3.Int3T](#ternary3int3t) left, [Ternary3.Int3T](#ternary3int3t) right)

>Subtracts the second ternary number from the first, maintaining the original numeric type.
>
>**Parameters:**
>- `left`: The number to subtract from.
>- `right`: The number to subtract.
>
>
>**Returns:**
>A new ternary number of the same type containing the difference.

#### [Ternary3.Int3T](#ternary3int3t) **operator &ast;**([Ternary3.Int3T](#ternary3int3t) left, [Ternary3.Int3T](#ternary3int3t) right)

>Multiplies two ternary numbers together, maintaining the original numeric type.
>
>**Parameters:**
>- `left`: The first number to multiply.
>- `right`: The second number to multiply.
>
>
>**Returns:**
>A new ternary number of the same type containing the product.

#### [Ternary3.Int3T](#ternary3int3t) **operator /**([Ternary3.Int3T](#ternary3int3t) left, [Ternary3.Int3T](#ternary3int3t) right)

>Divides the first ternary number by the second, maintaining the original numeric type.
>
>**Parameters:**
>- `left`: The dividend.
>- `right`: The divisor.
>
>
>**Returns:**
>A new ternary number of the same type containing the quotient.

#### [Ternary3.Int3T](#ternary3int3t) **operator %**([Ternary3.Int3T](#ternary3int3t) left, [Ternary3.Int3T](#ternary3int3t) right)

>Computes the remainder after dividing the first ternary number by the second.
>
>**Parameters:**
>- `left`: The dividend.
>- `right`: The divisor.
>
>
>**Returns:**
>A new ternary number of the same type containing the remainder.

#### [Ternary3.Int3T](#ternary3int3t) **operator -**([Ternary3.Int3T](#ternary3int3t) value)

>Returns the negation of a ternary number, maintaining the original numeric type.
>
>**Parameters:**
>- `value`: The ternary number to negate.
>
>
>**Returns:**
>A new ternary number of the same type containing the negated value.

#### [Ternary3.Int3T](#ternary3int3t) **operator +**([Ternary3.Int3T](#ternary3int3t) value)

>Returns the same ternary number (unary plus operator).
>
>**Parameters:**
>- `value`: The ternary number.
>
>
>**Returns:**
>The same ternary number.

#### [Ternary3.Int3T](#ternary3int3t) **operator +**([Ternary3.Int3T](#ternary3int3t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Adds a ternary number and a native numeric value, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The ternary number.
>- `right`: The native numeric value to add.
>
>
>**Returns:**
>A new ternary number containing the sum.

#### [Ternary3.Int3T](#ternary3int3t) **operator +**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int3T](#ternary3int3t) right)

>Adds a native numeric value and a ternary number, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The native numeric value to add.
>- `right`: The ternary number.
>
>
>**Returns:**
>A new ternary number containing the sum.

#### [Ternary3.Int3T](#ternary3int3t) **operator -**([Ternary3.Int3T](#ternary3int3t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Subtracts a native numeric value from a ternary number, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The ternary number to subtract from.
>- `right`: The native numeric value to subtract.
>
>
>**Returns:**
>A new ternary number containing the difference.

#### [Ternary3.Int3T](#ternary3int3t) **operator -**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int3T](#ternary3int3t) right)

>Subtracts a ternary number from a native numeric value, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The native numeric value.
>- `right`: The ternary number to subtract.
>
>
>**Returns:**
>A new ternary number containing the difference.

#### [Ternary3.Int3T](#ternary3int3t) **operator &ast;**([Ternary3.Int3T](#ternary3int3t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Multiplies a ternary number by a native numeric value, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The ternary number.
>- `right`: The native numeric value to multiply by.
>
>
>**Returns:**
>A new ternary number of the same type containing the product.

#### [Ternary3.Int3T](#ternary3int3t) **operator &ast;**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int3T](#ternary3int3t) right)

>Multiplies a native numeric value by a ternary number, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The native numeric value.
>- `right`: The ternary number to multiply by.
>
>
>**Returns:**
>A new ternary number containing the product.

#### [Ternary3.Int3T](#ternary3int3t) **operator /**([Ternary3.Int3T](#ternary3int3t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Divides a ternary number by a native numeric value, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The ternary number to divide.
>- `right`: The native numeric value to divide by.
>
>
>**Returns:**
>A new ternary number of the same type containing the quotient.

#### [Ternary3.Int3T](#ternary3int3t) **operator /**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int3T](#ternary3int3t) right)

>Divides a native numeric value by a ternary number, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The native numeric value to divide.
>- `right`: The ternary number to divide by.
>
>
>**Returns:**
>A new ternary number containing the quotient.

#### [Ternary3.Int3T](#ternary3int3t) **operator %**([Ternary3.Int3T](#ternary3int3t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Computes the remainder after dividing a ternary number by a native numeric value, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The ternary number to divide.
>- `right`: The native numeric value to divide by.
>
>
>**Returns:**
>A new ternary number of the same type containing the remainder.

#### [Ternary3.Int3T](#ternary3int3t) **operator %**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int3T](#ternary3int3t) right)

>Computes the remainder after dividing a native numeric value by a ternary number, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The native numeric value to divide.
>- `right`: The ternary number to divide by.
>
>
>**Returns:**
>A new ternary number containing the remainder.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int3T](#ternary3int3t) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether one [Int3T](#ternary3.int3t) is greater than another.
>
>**Parameters:**
>- `left`: The first value to compare.
>- `right`: The second value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int3T](#ternary3int3t) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether one [Int3T](#ternary3.int3t) is less than another.
>
>**Parameters:**
>- `left`: The first value to compare.
>- `right`: The second value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int3T](#ternary3int3t) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether one [Int3T](#ternary3.int3t) is greater than or equal to another.
>
>**Parameters:**
>- `left`: The first value to compare.
>- `right`: The second value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int3T](#ternary3int3t) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether one [Int3T](#ternary3.int3t) is less than or equal to another.
>
>**Parameters:**
>- `left`: The first value to compare.
>- `right`: The second value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int3T](#ternary3int3t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than a `SByte` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `SByte` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int3T](#ternary3int3t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than a `SByte` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `SByte` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than or equal to a `SByte` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `SByte` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than or equal to a `SByte` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `SByte` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `SByte` value is greater than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `SByte` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `SByte` value is less than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `SByte` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `SByte` value is greater than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `SByte` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `SByte` value is less than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `SByte` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int3T](#ternary3int3t) left, [System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than a `Byte` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Byte` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int3T](#ternary3int3t) left, [System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than a `Byte` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Byte` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than or equal to a `Byte` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Byte` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than or equal to a `Byte` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Byte` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Byte` value is greater than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Byte` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Byte` value is less than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Byte` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Byte` value is greater than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Byte` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Byte` value is less than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Byte` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int3T](#ternary3int3t) left, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than a `Int32` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Int32` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int3T](#ternary3int3t) left, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than a `Int32` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Int32` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than or equal to a `Int32` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Int32` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than or equal to a `Int32` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Int32` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Int32` value is greater than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Int32` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Int32` value is less than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Int32` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Int32` value is greater than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Int32` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Int32` value is less than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Int32` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int3T](#ternary3int3t) left, [System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than a `UInt32` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `UInt32` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int3T](#ternary3int3t) left, [System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than a `UInt32` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `UInt32` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than or equal to a `UInt32` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `UInt32` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than or equal to a `UInt32` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `UInt32` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `UInt32` value is greater than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `UInt32` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `UInt32` value is less than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `UInt32` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `UInt32` value is greater than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `UInt32` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `UInt32` value is less than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `UInt32` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int3T](#ternary3int3t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than a `Int16` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Int16` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int3T](#ternary3int3t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than a `Int16` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Int16` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than or equal to a `Int16` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Int16` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than or equal to a `Int16` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Int16` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Int16` value is greater than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Int16` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Int16` value is less than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Int16` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Int16` value is greater than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Int16` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Int16` value is less than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Int16` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int3T](#ternary3int3t) left, [System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than a `UInt16` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `UInt16` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int3T](#ternary3int3t) left, [System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than a `UInt16` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `UInt16` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than or equal to a `UInt16` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `UInt16` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than or equal to a `UInt16` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `UInt16` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `UInt16` value is greater than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `UInt16` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `UInt16` value is less than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `UInt16` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `UInt16` value is greater than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `UInt16` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `UInt16` value is less than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `UInt16` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int3T](#ternary3int3t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than a `Int64` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Int64` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int3T](#ternary3int3t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than a `Int64` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Int64` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than or equal to a `Int64` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Int64` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than or equal to a `Int64` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Int64` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Int64` value is greater than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Int64` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Int64` value is less than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Int64` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Int64` value is greater than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Int64` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Int64` value is less than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Int64` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int3T](#ternary3int3t) left, [System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than a `UInt64` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `UInt64` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int3T](#ternary3int3t) left, [System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than a `UInt64` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `UInt64` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than or equal to a `UInt64` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `UInt64` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than or equal to a `UInt64` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `UInt64` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `UInt64` value is greater than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `UInt64` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `UInt64` value is less than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `UInt64` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `UInt64` value is greater than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `UInt64` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `UInt64` value is less than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `UInt64` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int3T](#ternary3int3t) left, [System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than a `Single` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Single` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int3T](#ternary3int3t) left, [System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than a `Single` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Single` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than or equal to a `Single` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Single` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than or equal to a `Single` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Single` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Single` value is greater than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Single` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Single` value is less than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Single` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Single` value is greater than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Single` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Single` value is less than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Single` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int3T](#ternary3int3t) left, [System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than a `Double` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Double` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int3T](#ternary3int3t) left, [System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than a `Double` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Double` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than or equal to a `Double` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Double` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than or equal to a `Double` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Double` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Double` value is greater than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Double` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Double` value is less than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Double` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Double` value is greater than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Double` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Double` value is less than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Double` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int3T](#ternary3int3t) left, [System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than a `Decimal` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Decimal` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int3T](#ternary3int3t) left, [System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than a `Decimal` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Decimal` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than or equal to a `Decimal` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Decimal` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than or equal to a `Decimal` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `Decimal` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Decimal` value is greater than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Decimal` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Decimal` value is less than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Decimal` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Decimal` value is greater than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Decimal` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `Decimal` value is less than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `Decimal` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int3T](#ternary3int3t) left, [System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than a `IComparable` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `IComparable` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int3T](#ternary3int3t) left, [System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than a `IComparable` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `IComparable` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is greater than or equal to a `IComparable` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `IComparable` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int3T](#ternary3int3t) left, [System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) right)

>Returns a value indicating whether a [Int3T](#ternary3.int3t) value is less than or equal to a `IComparable` value.
>
>**Parameters:**
>- `left`: The [Int3T](#ternary3.int3t) value to compare.
>- `right`: The `IComparable` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `IComparable` value is greater than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `IComparable` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `IComparable` value is less than a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `IComparable` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `IComparable` value is greater than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `IComparable` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) left, [Ternary3.Int3T](#ternary3int3t) right)

>Returns a value indicating whether a `IComparable` value is less than or equal to a [Int3T](#ternary3.int3t) value.
>
>**Parameters:**
>- `left`: The `IComparable` value to compare.
>- `right`: The [Int3T](#ternary3.int3t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [Ternary3.Int3T](#ternary3int3t) **operator ++**([Ternary3.Int3T](#ternary3int3t) value)

>Increments a ternary number by one, maintaining the original numeric type.
>
>**Parameters:**
>- `value`: The ternary number to increment.
>
>
>**Returns:**
>A new ternary number with a value one greater than `value` .

#### [Ternary3.Int3T](#ternary3int3t) **operator --**([Ternary3.Int3T](#ternary3int3t) value)

>Decrements a ternary number by one, maintaining the original numeric type.
>
>**Parameters:**
>- `value`: The ternary number to decrement.
>
>
>**Returns:**
>A new ternary number with a value one less than `value` .

#### [Ternary3.Int3T](#ternary3int3t) **operator &lt;&lt;**([Ternary3.Int3T](#ternary3int3t) value, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shiftAmount)

>Performs a left shift operation on the ternary number, maintaining the original numeric type.
>
>**Parameters:**
>- `value`: The ternary number to shift.
>- `shiftAmount`: The number of positions to shift left. Negative values result in a right shift.
>
>
>**Returns:**
>A new ternary number of the same type containing the shifted value.

#### [Ternary3.Int3T](#ternary3int3t) **operator &gt;&gt;**([Ternary3.Int3T](#ternary3int3t) value, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shiftAmount)

>Performs a right shift operation on the ternary number, maintaining the original numeric type.
>
>**Parameters:**
>- `value`: The ternary number to shift.
>- `shiftAmount`: The number of positions to shift right. Negative values result in a left shift.
>
>
>**Returns:**
>A new ternary number of the same type containing the shifted value.

#### [Ternary3.Int3T](#ternary3int3t) **operator &gt;&gt;&gt;**([Ternary3.Int3T](#ternary3int3t) value, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shiftAmount)

>Performs an unsigned right shift operation on the ternary number, maintaining the original numeric type.
>In this implementation, it behaves the same as the signed right shift.
>
>**Parameters:**
>- `value`: The ternary number to shift.
>- `shiftAmount`: The number of positions to shift right. Negative values result in a left shift.
>
>
>**Returns:**
>A new ternary number of the same type containing the shifted value.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator |**([Ternary3.Int3T](#ternary3int3t) value, [System.Func<Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-2) operation)

>Applies a unary operation to each trit in this ternary number. This operation converts the number to a TernaryArray.
>
>**Parameters:**
>- `value`: The ternary number to convert and operate on.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray containing the result of applying the operation.
>
>
>**Remarks:**
>
>This operation causes an implicit conversion to TernaryArray before applying the operation.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator |**([Ternary3.Int3T](#ternary3int3t) value, [Ternary3.Trit[]](#ternary3trit) trits)

>Combines each trit in this ternary number with the corresponding trit in the provided array. This operation converts the number to a TernaryArray.
>
>**Parameters:**
>- `value`: The ternary number to convert and combine.
>- `trits`: The array of trits to combine with.
>
>
>**Returns:**
>A new TernaryArray containing the result of the operation.
>
>
>**Remarks:**
>
>This operation causes an implicit conversion to TernaryArray before combining with the provided trits.


## Ternary3.Int9T

>Represents a 9-trit  signed integer, modeled after the `Int16` type.

### Static Methods

#### [Ternary3.Int9T](#ternary3int9t) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value)

>Parses a string representation of a Int9T.
>
>**Parameters:**
>- `value`: The string value to parse.
>
>
>**Returns:**
>A Int9T representing the parsed value.

#### [Ternary3.Int9T](#ternary3int9t) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Parses a string representation of a Int9T.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>
>
>**Returns:**
>A Int9T representing the parsed value.

#### [Ternary3.Int9T](#ternary3int9t) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a Int9T.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A Int9T representing the parsed value.

#### [Ternary3.Int9T](#ternary3int9t) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a Int9T.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A Int9T representing the parsed value.

#### [Ternary3.Int9T](#ternary3int9t) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) s, [System.Globalization.NumberStyles](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles) style)

>Converts the string representation of a number in a specified style to its [Int9T](#ternary3.int9t) equivalent.
>
>**Parameters:**
>- `s`: A string containing a number to convert.
>- `style`: A bitwise combination of enumeration values that indicate the style elements that can be present in `s` .
>
>
>**Returns:**
>A [Int9T](#ternary3.int9t) equivalent to the number contained in `s` .
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: `s` is `null` .
>- `System.ArgumentException`: `style` is not a `NumberStyles` value or `style` includes the [AllowHexSpecifier](#f:system.globalization.numberstyles.allowhexspecifier) value.
>- `System.FormatException`: `s` is not in a format compliant with `style` .
>- `System.OverflowException`: `s` represents a number less than [MinValue](#f:ternary3.int9t.minvalue) or greater than [MaxValue](#f:ternary3.int9t.maxvalue) .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **TryParse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) s, [Ternary3.Int9T&](#ternary3int9t) result)

>Tries to convert the string representation of a number to its [Int9T](#ternary3.int9t) equivalent, and returns a value that indicates whether the conversion succeeded.
>
>**Parameters:**
>- `s`: A string containing a number to convert.
>- `result`: When this method returns, contains the [Int9T](#ternary3.int9t) value equivalent to the number contained in `s` if the conversion succeeded, or zero if the conversion failed. This parameter is passed uninitialized.
>
>
>**Returns:**
>`true` if `s` was converted successfully; otherwise, `false` .


### Static Fields

#### [Ternary3.Int9T](#ternary3int9t) **MaxValue**

>Represents the largest possible value of a [Int9T](#ternary3.int9t) . This field is static readonly.

#### [Ternary3.Int9T](#ternary3int9t) **MinValue**

>Represents the smallest possible value of a [Int9T](#ternary3.int9t) . This field is static readonly.
>
>**Examples:**
>
>```csharp
>// This code outputs TTTTTTTT1.
>Int9T value = Int9T.MinValue + 2;
>console.WriteLine($"The value is: {value:ter}");
>```

#### [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) **MaxValueConstant**

>Represents the maximum value of a [Int9T](#ternary3.int9t) , expressed as a `Int16` This field is constant.

#### [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) **MinValueConstant**

>Represents the minimum value of a [Int9T](#ternary3.int9t) , expressed as a `Int16` This field is constant.


### Methods

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) obj)

>Returns a value indicating whether this instance is equal to a specified object.
>
>**Parameters:**
>- `obj`: An object to compare with this instance.
>
>
>**Returns:**
>`true` if `obj` is an instance of [Int9T](#ternary3.int9t) or a compatible numeric type
>            and equals the value of this instance; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([Ternary3.Int9T](#ternary3int9t) other)

>Returns a value indicating whether this instance is equal to a specified [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `other`: A [Int9T](#ternary3.int9t) value to compare to this instance.
>
>
>**Returns:**
>`true` if `other` has the same value as this instance; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) other)

>Returns a value indicating whether this instance is equal to a specified `Int16` value.
>
>**Parameters:**
>- `other`: A `Int16` value to compare to this instance.
>
>
>**Returns:**
>`true` if `other` has the same value as this instance; otherwise, `false` .

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **GetHashCode**()

>Returns the hash code for this instance.
>
>**Returns:**
>A 32-bit signed integer hash code.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**()

>Returns a string representation of this instance.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format)

>Returns a string representation of this instance.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of this instance.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format, [System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of this instance.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Returns a string representation of this instance, formatted balanced ternarily according to the specified format.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **CompareTo**([System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) obj)

>Compares this instance to a specified object and returns an indication of their relative values.
>
>**Parameters:**
>- `obj`: An object to compare, or `null` .
>
>
>**Returns:**
>A signed number indicating the relative values of this instance and `obj` .
>            Return Value Description:
>            Less than zero: This instance is less than `obj` .
>            Zero: This instance is equal to `obj` .
>            Greater than zero: This instance is greater than `obj` or `obj` is `null` .
>
>
>**Exceptions:**
>- `System.ArgumentException`: `obj` is not a [Int9T](#ternary3.int9t) or a type that can be converted to an integer.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **CompareTo**([Ternary3.Int9T](#ternary3int9t) other)

>Compares this instance to a specified [Int9T](#ternary3.int9t) object and returns an indication of their relative values.
>
>**Parameters:**
>- `other`: An [Int9T](#ternary3.int9t) object to compare.
>
>
>**Returns:**
>A signed number indicating the relative values of this instance and `other` .
>            Return Value Description:
>            Less than zero: This instance is less than `other` .
>            Zero: This instance is equal to `other` .
>            Greater than zero: This instance is greater than `other` .

#### [System.TypeCode](https://learn.microsoft.com/en-us/dotnet/api/system.typecode) **GetTypeCode**()

>Gets the `TypeCode` for this instance.
>
>**Returns:**
>[Int32](#f:system.typecode.int32) .


### Properties

#### [Ternary3.Trit](#ternary3trit) **this[[System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) index]** { get; }

No documentation available.

#### [Ternary3.Trit](#ternary3trit) **this[[System.Index](https://learn.microsoft.com/en-us/dotnet/api/system.index) index]** { get; }

No documentation available.


### Operators

#### [Ternary3.Int9T](#ternary3int9t) **operator implicit**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) value)

>Defines an implicit conversion of a Int16 to a [Int9T](#ternary3.int9t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int9T](#ternary3.int9t) that represents the converted value.

#### [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) **operator implicit**([Ternary3.Int9T](#ternary3int9t) value)

>Defines an implicit conversion of a Int16 to a [Int9T](#ternary3.int9t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int9T](#ternary3.int9t) that represents the converted value.

#### [Ternary3.Int9T](#ternary3int9t) **operator explicit**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value)

>Defines an explicit conversion of a Int32 to a [Int9T](#ternary3.int9t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int9T](#ternary3.int9t) that represents the converted value.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **operator implicit**([Ternary3.Int9T](#ternary3int9t) value)

>Defines an implicit conversion of a Int16 to a [Int9T](#ternary3.int9t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int9T](#ternary3.int9t) that represents the converted value.

#### [Ternary3.Int9T](#ternary3int9t) **operator explicit**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) value)

>Defines an explicit conversion of a Int32 to a [Int9T](#ternary3.int9t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int9T](#ternary3.int9t) that represents the converted value.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **operator implicit**([Ternary3.Int9T](#ternary3int9t) value)

>Defines an implicit conversion of a Int16 to a [Int9T](#ternary3.int9t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int9T](#ternary3.int9t) that represents the converted value.

#### [Ternary3.Int9T](#ternary3int9t) **operator explicit**([System.Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128) value)

>Defines an explicit conversion of a Int32 to a [Int9T](#ternary3.int9t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int9T](#ternary3.int9t) that represents the converted value.

#### [System.Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128) **operator implicit**([Ternary3.Int9T](#ternary3int9t) value)

>Defines an implicit conversion of a Int16 to a [Int9T](#ternary3.int9t) .
>
>**Parameters:**
>- `value`: The value to convert.
>
>
>**Returns:**
>A [Int9T](#ternary3.int9t) that represents the converted value.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator ==**([System.IConvertible](https://learn.microsoft.com/en-us/dotnet/api/system.iconvertible) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether two [Int9T](#ternary3.int9t) instances are equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are equal; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator !=**([System.IConvertible](https://learn.microsoft.com/en-us/dotnet/api/system.iconvertible) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether two [Int9T](#ternary3.int9t) instances are not equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are not equal; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator ==**([Ternary3.Int9T](#ternary3int9t) left, [System.IConvertible](https://learn.microsoft.com/en-us/dotnet/api/system.iconvertible) right)

>Returns a value indicating whether two [Int9T](#ternary3.int9t) instances are equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are equal; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator !=**([Ternary3.Int9T](#ternary3int9t) left, [System.IConvertible](https://learn.microsoft.com/en-us/dotnet/api/system.iconvertible) right)

>Returns a value indicating whether two [Int9T](#ternary3.int9t) instances are not equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are not equal; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator ==**([Ternary3.Int9T](#ternary3int9t) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether two [Int9T](#ternary3.int9t) instances are equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are equal; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator !=**([Ternary3.Int9T](#ternary3int9t) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether two [Int9T](#ternary3.int9t) instances are not equal.
>
>**Parameters:**
>- `left`: The first instance to compare.
>- `right`: The second instance to compare.
>
>
>**Returns:**
>`true` if the values of `left` and `right` are not equal; otherwise, `false` .

#### [Ternary3.Int9T](#ternary3int9t) **operator +**([Ternary3.Int9T](#ternary3int9t) left, [Ternary3.Int9T](#ternary3int9t) right)

>Adds two ternary numbers together, maintaining the original numeric type.
>
>**Parameters:**
>- `left`: The first ternary number.
>- `right`: The second ternary number.
>
>
>**Returns:**
>A new ternary number of the same type containing the sum.

#### [Ternary3.Int9T](#ternary3int9t) **operator -**([Ternary3.Int9T](#ternary3int9t) left, [Ternary3.Int9T](#ternary3int9t) right)

>Subtracts the second ternary number from the first, maintaining the original numeric type.
>
>**Parameters:**
>- `left`: The number to subtract from.
>- `right`: The number to subtract.
>
>
>**Returns:**
>A new ternary number of the same type containing the difference.

#### [Ternary3.Int9T](#ternary3int9t) **operator &ast;**([Ternary3.Int9T](#ternary3int9t) left, [Ternary3.Int9T](#ternary3int9t) right)

>Multiplies two ternary numbers together, maintaining the original numeric type.
>
>**Parameters:**
>- `left`: The first number to multiply.
>- `right`: The second number to multiply.
>
>
>**Returns:**
>A new ternary number of the same type containing the product.

#### [Ternary3.Int9T](#ternary3int9t) **operator /**([Ternary3.Int9T](#ternary3int9t) left, [Ternary3.Int9T](#ternary3int9t) right)

>Divides the first ternary number by the second, maintaining the original numeric type.
>
>**Parameters:**
>- `left`: The dividend.
>- `right`: The divisor.
>
>
>**Returns:**
>A new ternary number of the same type containing the quotient.

#### [Ternary3.Int9T](#ternary3int9t) **operator %**([Ternary3.Int9T](#ternary3int9t) left, [Ternary3.Int9T](#ternary3int9t) right)

>Computes the remainder after dividing the first ternary number by the second.
>
>**Parameters:**
>- `left`: The dividend.
>- `right`: The divisor.
>
>
>**Returns:**
>A new ternary number of the same type containing the remainder.

#### [Ternary3.Int9T](#ternary3int9t) **operator -**([Ternary3.Int9T](#ternary3int9t) value)

>Returns the negation of a ternary number, maintaining the original numeric type.
>
>**Parameters:**
>- `value`: The ternary number to negate.
>
>
>**Returns:**
>A new ternary number of the same type containing the negated value.

#### [Ternary3.Int9T](#ternary3int9t) **operator +**([Ternary3.Int9T](#ternary3int9t) value)

>Returns the same ternary number (unary plus operator).
>
>**Parameters:**
>- `value`: The ternary number.
>
>
>**Returns:**
>The same ternary number.

#### [Ternary3.Int9T](#ternary3int9t) **operator +**([Ternary3.Int9T](#ternary3int9t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Adds a ternary number and a native numeric value, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The ternary number.
>- `right`: The native numeric value to add.
>
>
>**Returns:**
>A new ternary number containing the sum.

#### [Ternary3.Int9T](#ternary3int9t) **operator +**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int9T](#ternary3int9t) right)

>Adds a native numeric value and a ternary number, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The native numeric value to add.
>- `right`: The ternary number.
>
>
>**Returns:**
>A new ternary number containing the sum.

#### [Ternary3.Int9T](#ternary3int9t) **operator -**([Ternary3.Int9T](#ternary3int9t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Subtracts a native numeric value from a ternary number, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The ternary number to subtract from.
>- `right`: The native numeric value to subtract.
>
>
>**Returns:**
>A new ternary number containing the difference.

#### [Ternary3.Int9T](#ternary3int9t) **operator -**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int9T](#ternary3int9t) right)

>Subtracts a ternary number from a native numeric value, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The native numeric value.
>- `right`: The ternary number to subtract.
>
>
>**Returns:**
>A new ternary number containing the difference.

#### [Ternary3.Int9T](#ternary3int9t) **operator &ast;**([Ternary3.Int9T](#ternary3int9t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Multiplies a ternary number by a native numeric value, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The ternary number.
>- `right`: The native numeric value to multiply by.
>
>
>**Returns:**
>A new ternary number of the same type containing the product.

#### [Ternary3.Int9T](#ternary3int9t) **operator &ast;**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int9T](#ternary3int9t) right)

>Multiplies a native numeric value by a ternary number, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The native numeric value.
>- `right`: The ternary number to multiply by.
>
>
>**Returns:**
>A new ternary number containing the product.

#### [Ternary3.Int9T](#ternary3int9t) **operator /**([Ternary3.Int9T](#ternary3int9t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Divides a ternary number by a native numeric value, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The ternary number to divide.
>- `right`: The native numeric value to divide by.
>
>
>**Returns:**
>A new ternary number of the same type containing the quotient.

#### [Ternary3.Int9T](#ternary3int9t) **operator /**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int9T](#ternary3int9t) right)

>Divides a native numeric value by a ternary number, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The native numeric value to divide.
>- `right`: The ternary number to divide by.
>
>
>**Returns:**
>A new ternary number containing the quotient.

#### [Ternary3.Int9T](#ternary3int9t) **operator %**([Ternary3.Int9T](#ternary3int9t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Computes the remainder after dividing a ternary number by a native numeric value, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The ternary number to divide.
>- `right`: The native numeric value to divide by.
>
>
>**Returns:**
>A new ternary number of the same type containing the remainder.

#### [Ternary3.Int9T](#ternary3int9t) **operator %**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int9T](#ternary3int9t) right)

>Computes the remainder after dividing a native numeric value by a ternary number, maintaining the original ternary type.
>
>**Parameters:**
>- `left`: The native numeric value to divide.
>- `right`: The ternary number to divide by.
>
>
>**Returns:**
>A new ternary number containing the remainder.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int9T](#ternary3int9t) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether one [Int9T](#ternary3.int9t) is greater than another.
>
>**Parameters:**
>- `left`: The first value to compare.
>- `right`: The second value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int9T](#ternary3int9t) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether one [Int9T](#ternary3.int9t) is less than another.
>
>**Parameters:**
>- `left`: The first value to compare.
>- `right`: The second value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int9T](#ternary3int9t) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether one [Int9T](#ternary3.int9t) is greater than or equal to another.
>
>**Parameters:**
>- `left`: The first value to compare.
>- `right`: The second value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int9T](#ternary3int9t) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether one [Int9T](#ternary3.int9t) is less than or equal to another.
>
>**Parameters:**
>- `left`: The first value to compare.
>- `right`: The second value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int9T](#ternary3int9t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than a `SByte` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `SByte` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int9T](#ternary3int9t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than a `SByte` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `SByte` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than or equal to a `SByte` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `SByte` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than or equal to a `SByte` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `SByte` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `SByte` value is greater than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `SByte` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `SByte` value is less than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `SByte` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `SByte` value is greater than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `SByte` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `SByte` value is less than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `SByte` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int9T](#ternary3int9t) left, [System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than a `Byte` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Byte` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int9T](#ternary3int9t) left, [System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than a `Byte` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Byte` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than or equal to a `Byte` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Byte` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than or equal to a `Byte` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Byte` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Byte` value is greater than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Byte` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Byte` value is less than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Byte` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Byte` value is greater than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Byte` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Byte` value is less than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Byte` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int9T](#ternary3int9t) left, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than a `Int32` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Int32` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int9T](#ternary3int9t) left, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than a `Int32` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Int32` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than or equal to a `Int32` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Int32` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than or equal to a `Int32` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Int32` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Int32` value is greater than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Int32` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Int32` value is less than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Int32` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Int32` value is greater than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Int32` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Int32` value is less than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Int32` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int9T](#ternary3int9t) left, [System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than a `UInt32` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `UInt32` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int9T](#ternary3int9t) left, [System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than a `UInt32` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `UInt32` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than or equal to a `UInt32` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `UInt32` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than or equal to a `UInt32` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `UInt32` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `UInt32` value is greater than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `UInt32` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `UInt32` value is less than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `UInt32` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `UInt32` value is greater than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `UInt32` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `UInt32` value is less than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `UInt32` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int9T](#ternary3int9t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than a `Int16` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Int16` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int9T](#ternary3int9t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than a `Int16` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Int16` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than or equal to a `Int16` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Int16` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than or equal to a `Int16` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Int16` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Int16` value is greater than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Int16` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Int16` value is less than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Int16` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Int16` value is greater than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Int16` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Int16` value is less than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Int16` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int9T](#ternary3int9t) left, [System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than a `UInt16` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `UInt16` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int9T](#ternary3int9t) left, [System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than a `UInt16` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `UInt16` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than or equal to a `UInt16` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `UInt16` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than or equal to a `UInt16` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `UInt16` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `UInt16` value is greater than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `UInt16` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `UInt16` value is less than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `UInt16` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `UInt16` value is greater than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `UInt16` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `UInt16` value is less than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `UInt16` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int9T](#ternary3int9t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than a `Int64` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Int64` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int9T](#ternary3int9t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than a `Int64` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Int64` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than or equal to a `Int64` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Int64` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than or equal to a `Int64` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Int64` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Int64` value is greater than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Int64` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Int64` value is less than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Int64` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Int64` value is greater than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Int64` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Int64` value is less than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Int64` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int9T](#ternary3int9t) left, [System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than a `UInt64` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `UInt64` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int9T](#ternary3int9t) left, [System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than a `UInt64` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `UInt64` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than or equal to a `UInt64` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `UInt64` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than or equal to a `UInt64` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `UInt64` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `UInt64` value is greater than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `UInt64` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `UInt64` value is less than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `UInt64` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `UInt64` value is greater than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `UInt64` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `UInt64` value is less than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `UInt64` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int9T](#ternary3int9t) left, [System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than a `Single` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Single` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int9T](#ternary3int9t) left, [System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than a `Single` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Single` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than or equal to a `Single` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Single` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than or equal to a `Single` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Single` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Single` value is greater than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Single` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Single` value is less than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Single` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Single` value is greater than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Single` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Single](https://learn.microsoft.com/en-us/dotnet/api/system.single) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Single` value is less than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Single` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int9T](#ternary3int9t) left, [System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than a `Double` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Double` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int9T](#ternary3int9t) left, [System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than a `Double` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Double` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than or equal to a `Double` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Double` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than or equal to a `Double` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Double` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Double` value is greater than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Double` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Double` value is less than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Double` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Double` value is greater than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Double` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Double` value is less than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Double` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int9T](#ternary3int9t) left, [System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than a `Decimal` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Decimal` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int9T](#ternary3int9t) left, [System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than a `Decimal` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Decimal` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than or equal to a `Decimal` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Decimal` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than or equal to a `Decimal` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `Decimal` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Decimal` value is greater than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Decimal` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Decimal` value is less than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Decimal` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Decimal` value is greater than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Decimal` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `Decimal` value is less than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `Decimal` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([Ternary3.Int9T](#ternary3int9t) left, [System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than a `IComparable` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `IComparable` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([Ternary3.Int9T](#ternary3int9t) left, [System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than a `IComparable` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `IComparable` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is greater than or equal to a `IComparable` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `IComparable` value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([Ternary3.Int9T](#ternary3int9t) left, [System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) right)

>Returns a value indicating whether a [Int9T](#ternary3.int9t) value is less than or equal to a `IComparable` value.
>
>**Parameters:**
>- `left`: The [Int9T](#ternary3.int9t) value to compare.
>- `right`: The `IComparable` value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;**([System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `IComparable` value is greater than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `IComparable` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;**([System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `IComparable` value is less than a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `IComparable` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &gt;=**([System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `IComparable` value is greater than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `IComparable` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is greater than or equal to `right` ; otherwise, `false` .

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator &lt;=**([System.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable) left, [Ternary3.Int9T](#ternary3int9t) right)

>Returns a value indicating whether a `IComparable` value is less than or equal to a [Int9T](#ternary3.int9t) value.
>
>**Parameters:**
>- `left`: The `IComparable` value to compare.
>- `right`: The [Int9T](#ternary3.int9t) value to compare.
>
>
>**Returns:**
>`true` if `left` is less than or equal to `right` ; otherwise, `false` .

#### [Ternary3.Int9T](#ternary3int9t) **operator ++**([Ternary3.Int9T](#ternary3int9t) value)

>Increments a ternary number by one, maintaining the original numeric type.
>
>**Parameters:**
>- `value`: The ternary number to increment.
>
>
>**Returns:**
>A new ternary number with a value one greater than `value` .

#### [Ternary3.Int9T](#ternary3int9t) **operator --**([Ternary3.Int9T](#ternary3int9t) value)

>Decrements a ternary number by one, maintaining the original numeric type.
>
>**Parameters:**
>- `value`: The ternary number to decrement.
>
>
>**Returns:**
>A new ternary number with a value one less than `value` .

#### [Ternary3.Int9T](#ternary3int9t) **operator &lt;&lt;**([Ternary3.Int9T](#ternary3int9t) value, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shiftAmount)

>Performs a left shift operation on the ternary number, maintaining the original numeric type.
>
>**Parameters:**
>- `value`: The ternary number to shift.
>- `shiftAmount`: The number of positions to shift left. Negative values result in a right shift.
>
>
>**Returns:**
>A new ternary number of the same type containing the shifted value.

#### [Ternary3.Int9T](#ternary3int9t) **operator &gt;&gt;**([Ternary3.Int9T](#ternary3int9t) value, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shiftAmount)

>Performs a right shift operation on the ternary number, maintaining the original numeric type.
>
>**Parameters:**
>- `value`: The ternary number to shift.
>- `shiftAmount`: The number of positions to shift right. Negative values result in a left shift.
>
>
>**Returns:**
>A new ternary number of the same type containing the shifted value.

#### [Ternary3.Int9T](#ternary3int9t) **operator &gt;&gt;&gt;**([Ternary3.Int9T](#ternary3int9t) value, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shiftAmount)

>Performs an unsigned right shift operation on the ternary number, maintaining the original numeric type.
>In this implementation, it behaves the same as the signed right shift.
>
>**Parameters:**
>- `value`: The ternary number to shift.
>- `shiftAmount`: The number of positions to shift right. Negative values result in a left shift.
>
>
>**Returns:**
>A new ternary number of the same type containing the shifted value.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator |**([Ternary3.Int9T](#ternary3int9t) value, [System.Func<Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-2) operation)

>Applies a unary operation to each trit in this ternary number. This operation converts the number to a TernaryArray.
>
>**Parameters:**
>- `value`: The ternary number to convert and operate on.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray containing the result of applying the operation.
>
>
>**Remarks:**
>
>This operation causes an implicit conversion to TernaryArray before applying the operation.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator |**([Ternary3.Int9T](#ternary3int9t) value, [Ternary3.Trit[]](#ternary3trit) trits)

>Combines each trit in this ternary number with the corresponding trit in the provided array. This operation converts the number to a TernaryArray.
>
>**Parameters:**
>- `value`: The ternary number to convert and combine.
>- `trits`: The array of trits to combine with.
>
>
>**Returns:**
>A new TernaryArray containing the result of the operation.
>
>
>**Remarks:**
>
>This operation causes an implicit conversion to TernaryArray before combining with the provided trits.


## Ternary3.ITernaryArray

>Represents a basic interface for all trit array implementations, providing core functionality for working with ternary data.
>This generic interface enables fluent, strongly-typed operations on trit arrays with proper return type preservation.

### Properties

#### [Ternary3.Trit](#ternary3trit) **this[[System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) index]** { get; }

No documentation available.

#### [Ternary3.Trit](#ternary3trit) **this[[System.Index](https://learn.microsoft.com/en-us/dotnet/api/system.index) index]** { get; }

No documentation available.

#### [Ternary3.ITernaryArray](#ternary3iternaryarray) **this[[System.Range](https://learn.microsoft.com/en-us/dotnet/api/system.range) range]** { get; }

No documentation available.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **Length** { get; }

>Gets the length of the trit array.
>
>**Examples:**
>
>```csharp
> TernaryArray trits1 = ter000111000TTT;
>Console.WriteLine(trits1.Length); // Outputs: 9
> TernaryArray27 trits2 = ter000111000TTT;
>Console.WriteLine(trits2.Length); // Outputs: 27
>```


## Ternary3.ITernaryArray<TSelf>

>Represents a strongly-typed trit array that supports equality comparison and string formatting.

## Ternary3.ITernaryInteger<TSelf>

>Defines an integer type that is represented in a base-2 format.

### Static Methods

#### [ValueTuple`2](https://learn.microsoft.com/en-us/dotnet/api/valuetuple`2) **DivRem**([TSelf](https://learn.microsoft.com/en-us/dotnet/api/tself) left, [TSelf](https://learn.microsoft.com/en-us/dotnet/api/tself) right)

>Computes the quotient and remainder of two values.
>
>**Parameters:**
>- `left`: The value which `right` divides.
>- `right`: The value which divides `left` .
>
>
>**Returns:**
>The quotient and remainder of `left` divided by `right` .


## Ternary3.ITernaryParsable<TSelf>

>Defines a contract for types that can be parsed from a string representation in a ternary format.

### Static Methods

#### [TSelf](https://learn.microsoft.com/en-us/dotnet/api/tself) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value)

>Parses a string representation
>
>**Parameters:**
>- `value`: The string value to parse.
>
>
>**Returns:**
>A value representing the parsed value.

#### [TSelf](https://learn.microsoft.com/en-us/dotnet/api/tself) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Parses a string representation
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>
>
>**Returns:**
>A value representing the parsed value.

#### [TSelf](https://learn.microsoft.com/en-us/dotnet/api/tself) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation
>
>**Parameters:**
>- `value`: The string value to parse.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A value representing the parsed value.

#### [TSelf](https://learn.microsoft.com/en-us/dotnet/api/tself) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A value representing the parsed value.


## Ternary3.ITritwiseOperator<TSelf, TOther, TResult>

>Defines a mechanism for performing bitwise operations over two values.

## Ternary3.LookupBigTernaryArrayOperator

>Helper class to facilitate binary operations between two BigTernaryArray instances.

### Operators

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **operator |**([Ternary3.LookupBigTernaryArrayOperator](#ternary3lookupbigternaryarrayoperator) left, [Ternary3.BigTernaryArray](#ternary3bigternaryarray) right)

>Applies the binary operation to the left and right arrays.
>
>**Parameters:**
>- `left`: The binary operation context.
>- `right`: The right-hand operand.
>
>
>**Returns:**
>A new BigTernaryArray with the operation applied.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **operator |**([Ternary3.LookupBigTernaryArrayOperator](#ternary3lookupbigternaryarrayoperator) left, [Ternary3.TernaryArray](#ternary3ternaryarray) right)

>Applies the binary operation to the left and right arrays.
>
>**Parameters:**
>- `left`: The binary operation context.
>- `right`: The right-hand operand.
>
>
>**Returns:**
>A new BigTernaryArray with the operation applied.


## Ternary3.LookupTernaryArray27Operator

>Represents an operator that combines a TernaryArray27 with a binary operation lookup table.
>
>**Remarks:**
>
>Used to efficiently apply binary operations between TernaryArray27 instances by using optimized lookup tables.
>            The first operand is stored within the operator structure, and the second operand is provided via the pipe operator.

### Operators

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator |**([Ternary3.LookupTernaryArray27Operator](#ternary3lookupternaryarray27operator) left, [Ternary3.TernaryArray27](#ternary3ternaryarray27) right)

>Performs a binary operation between the stored left operand (TernaryArray27) and the right operand using a lookup table.
>
>**Parameters:**
>- `left`: The LookupTernaryArray27Operator containing the left operand and operation details.
>- `right`: The right TernaryArray27 operand.
>
>
>**Returns:**
>A new TernaryArray27 representing the result of applying the binary operation to each corresponding pair of trits.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator |**([Ternary3.LookupTernaryArray27Operator](#ternary3lookupternaryarray27operator) left, [Ternary3.Int27T](#ternary3int27t) right)

>Performs a binary operation between the stored left operand (TernaryArray27) and an Int27T right operand using a lookup table.
>
>**Parameters:**
>- `left`: The LookupTernaryArray27Operator containing the left operand and operation details.
>- `right`: The right Int27T operand, which will be converted to a TernaryArray27.
>
>
>**Returns:**
>A new TernaryArray27 representing the result of applying the binary operation to each corresponding pair of trits.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator |**([Ternary3.LookupTernaryArray27Operator](#ternary3lookupternaryarray27operator) left, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) right)

>Performs a binary operation between the stored left operand (TernaryArray27) and a Int32 right operand using a lookup table.
>
>**Parameters:**
>- `left`: The LookupTernaryArray27Operator containing the left operand and operation details.
>- `right`: The right Int32 operand, which will be converted to a TernaryArray27.
>
>
>**Returns:**
>A new TernaryArray27 representing the result of applying the binary operation to each corresponding pair of trits.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator |**([Ternary3.LookupTernaryArray27Operator](#ternary3lookupternaryarray27operator) left, [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) right)

>Performs a binary operation between the stored left operand (TernaryArray27) and a Int64 right operand using a lookup table.
>
>**Parameters:**
>- `left`: The LookupTernaryArray27Operator containing the left operand and operation details.
>- `right`: The right Int64 operand, which will be converted to a TernaryArray27.
>
>
>**Returns:**
>A new TernaryArray27 representing the result of applying the binary operation to each corresponding pair of trits.


## Ternary3.LookupTernaryArray3Operator

>Represents an operator that combines a TernaryArray3 with a binary operation lookup table.
>
>**Remarks:**
>
>Used to efficiently apply binary operations between TernaryArray3 instances by using optimized lookup tables.
>            The first operand is stored within the operator structure, and the second operand is provided via the pipe operator.

### Operators

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator |**([Ternary3.LookupTernaryArray3Operator](#ternary3lookupternaryarray3operator) left, [Ternary3.TernaryArray3](#ternary3ternaryarray3) right)

>Performs a binary operation between the stored left operand (TernaryArray3) and the right operand using a lookup table.
>
>**Parameters:**
>- `left`: The LookupTernaryArray3Operator containing the left operand and operation details.
>- `right`: The right TernaryArray3 operand.
>
>
>**Returns:**
>A new TernaryArray3 representing the result of applying the binary operation to each corresponding pair of trits.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator |**([Ternary3.LookupTernaryArray3Operator](#ternary3lookupternaryarray3operator) left, [Ternary3.Int3T](#ternary3int3t) right)

>Performs a binary operation between the stored left operand (TernaryArray3) and an Int3T right operand using a lookup table.
>
>**Parameters:**
>- `left`: The LookupTernaryArray3Operator containing the left operand and operation details.
>- `right`: The right Int3T operand, which will be converted to a TernaryArray3.
>
>
>**Returns:**
>A new TernaryArray3 representing the result of applying the binary operation to each corresponding pair of trits.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator |**([Ternary3.LookupTernaryArray3Operator](#ternary3lookupternaryarray3operator) left, [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) right)

>Performs a binary operation between the stored left operand (TernaryArray3) and a SByte right operand using a lookup table.
>
>**Parameters:**
>- `left`: The LookupTernaryArray3Operator containing the left operand and operation details.
>- `right`: The right SByte operand, which will be converted to a TernaryArray3.
>
>
>**Returns:**
>A new TernaryArray3 representing the result of applying the binary operation to each corresponding pair of trits.


## Ternary3.LookupTernaryArray9Operator

>Represents an operator that combines a TernaryArray9 with a binary operation lookup table.
>
>**Remarks:**
>
>Used to efficiently apply binary operations between TernaryArray9 instances by using optimized lookup tables.
>            The first operand is stored within the operator structure, and the second operand is provided via the pipe operator.

### Operators

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator |**([Ternary3.LookupTernaryArray9Operator](#ternary3lookupternaryarray9operator) left, [Ternary3.TernaryArray9](#ternary3ternaryarray9) right)

>Performs a binary operation between the stored left operand (TernaryArray9) and the right operand using a lookup table.
>
>**Parameters:**
>- `left`: The LookupTernaryArray9Operator containing the left operand and operation details.
>- `right`: The right TernaryArray9 operand.
>
>
>**Returns:**
>A new TernaryArray9 representing the result of applying the binary operation to each corresponding pair of trits.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator |**([Ternary3.LookupTernaryArray9Operator](#ternary3lookupternaryarray9operator) left, [Ternary3.Int9T](#ternary3int9t) right)

>Performs a binary operation between the stored left operand (TernaryArray9) and an Int9T right operand using a lookup table.
>
>**Parameters:**
>- `left`: The LookupTernaryArray9Operator containing the left operand and operation details.
>- `right`: The right Int9T operand, which will be converted to a TernaryArray9.
>
>
>**Returns:**
>A new TernaryArray9 representing the result of applying the binary operation to each corresponding pair of trits.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator |**([Ternary3.LookupTernaryArray9Operator](#ternary3lookupternaryarray9operator) left, [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) right)

>Performs a binary operation between the stored left operand (TernaryArray9) and a Int16 right operand using a lookup table.
>
>**Parameters:**
>- `left`: The LookupTernaryArray9Operator containing the left operand and operation details.
>- `right`: The right Int16 operand, which will be converted to a TernaryArray9.
>
>
>**Returns:**
>A new TernaryArray9 representing the result of applying the binary operation to each corresponding pair of trits.


## Ternary3.LookupTernaryArrayOperator

>Helper class to facilitate binary operations between two TernaryArray instances.

### Operators

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **operator |**([Ternary3.LookupTernaryArrayOperator](#ternary3lookupternaryarrayoperator) left, [Ternary3.TernaryArray](#ternary3ternaryarray) right)

>Applies the binary operation to the left and right arrays.
>
>**Parameters:**
>- `left`: The binary operation context.
>- `right`: The right-hand operand.
>
>
>**Returns:**
>A new TernaryArray with the operation applied.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **operator |**([Ternary3.LookupTernaryArrayOperator](#ternary3lookupternaryarrayoperator) left, [Ternary3.BigTernaryArray](#ternary3bigternaryarray) right)

>Applies the binary operation to the left and right arrays.
>
>**Parameters:**
>- `left`: The binary operation context.
>- `right`: The right-hand operand.
>
>
>**Returns:**
>A new BigTernaryArray with the operation applied.


## Ternary3.TernaryArray

>Represents a fixed-size array of 27 trits (ternary digits).

### Static Methods

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value)

>Parses a string representation of a TernaryArray.
>
>**Parameters:**
>- `value`: The string value to parse.
>
>
>**Returns:**
>A TernaryArray representing the parsed value.

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Parses a string representation of a TernaryArray.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>
>
>**Returns:**
>A TernaryArray representing the parsed value.

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a TernaryArray.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A TernaryArray representing the parsed value.

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a TernaryArray.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A TernaryArray representing the parsed value.


### Static Fields

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **Zero**

>Represents a TernaryArray with all trits set to zero.


### Constructors

#### **TernaryArray**()

>Initializes a new instance of the TernaryArray struct with all trits set to zero.


### Methods

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **GetHashCode**()

>

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) obj)

>

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([Ternary3.TernaryArray](#ternary3ternaryarray) other)

>

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**()

>Returns a string representation of the TernaryArray.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format)

>Returns a string representation of the TernaryArray.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of the TernaryArray.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format, [System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of the TernaryArray.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Returns a string representation of this instance, formatted balanced ternarily according to the specified format.


### Properties

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **MinValue** { get; }

>Represents the minimum value that a TernaryArray can have (all trits set to -1).

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **MaxValue** { get; }

>Represents the maximum value that a TernaryArray can have (all trits set to 1).

#### [Ternary3.Trit](#ternary3trit) **this[[System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) index]** { get; set; }

No documentation available.

#### [Ternary3.ITernaryArray](#ternary3iternaryarray) **this[[System.Range](https://learn.microsoft.com/en-us/dotnet/api/system.range) range]** { get; }

No documentation available.

#### [Ternary3.Trit](#ternary3trit) **this[[System.Index](https://learn.microsoft.com/en-us/dotnet/api/system.index) index]** { get; set; }

No documentation available.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **Length** { get; }

>Gets the length of the trit array


### Operators

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **operator |**([Ternary3.TernaryArray](#ternary3ternaryarray) array, [System.Func<Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-2) operation)

>Applies a unary operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray with the operation applied to each trit.

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **operator |**([Ternary3.TernaryArray](#ternary3ternaryarray) array, [Ternary3.Trit[]](#ternary3trit) table)

>Applies a lookup table operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `table`: The lookup table containing the transformation values.
>
>
>**Returns:**
>A new TernaryArray with the lookup operation applied to each trit.

#### [Ternary3.LookupTernaryArrayOperator](#ternary3lookupternaryarrayoperator) **operator |**([Ternary3.TernaryArray](#ternary3ternaryarray) array, [System.Func<Ternary3.Trit, Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-3) operation)

>Applies a unary operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray with the operation applied to each trit.

#### [Ternary3.LookupTernaryArrayOperator](#ternary3lookupternaryarrayoperator) **operator |**([Ternary3.TernaryArray](#ternary3ternaryarray) array, [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) table)

>Creates a binary operation context for this array using a lookup table.
>
>**Parameters:**
>- `array`: The source array.
>- `table`: The lookup table containing the transformation values.
>
>
>**Returns:**
>A LookupTernaryArrayOperator that can be used to apply the operation with another array.

#### [Ternary3.LookupTernaryArrayOperator](#ternary3lookupternaryarrayoperator) **operator |**([Ternary3.TernaryArray](#ternary3ternaryarray) array, [Ternary3.Trit[,]](#ternary3trit) table)

>Applies a unary operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray with the operation applied to each trit.

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **operator &lt;&lt;**([Ternary3.TernaryArray](#ternary3ternaryarray) array, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shift)

>Performs a left bitwise shift on the trit array.
>
>**Parameters:**
>- `array`: The source array.
>- `shift`: The number of positions to shift.
>
>
>**Returns:**
>A new TernaryArray with the bits shifted to the left.

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **operator &gt;&gt;**([Ternary3.TernaryArray](#ternary3ternaryarray) array, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shift)

>Performs a right bitwise shift on the trit array.
>
>**Parameters:**
>- `array`: The source array.
>- `shift`: The number of positions to shift.
>
>
>**Returns:**
>A new TernaryArray with the bits shifted to the right.

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **operator +**([Ternary3.TernaryArray](#ternary3ternaryarray) value1, [Ternary3.TernaryArray](#ternary3ternaryarray) value2)

>Adds two TernaryArray values together.
>
>**Parameters:**
>- `value1`: The first value to add.
>- `value2`: The second value to add.
>
>
>**Returns:**
>A new TernaryArray representing the sum of the two values.

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **operator -**([Ternary3.TernaryArray](#ternary3ternaryarray) value1, [Ternary3.TernaryArray](#ternary3ternaryarray) value2)

>Subtracts one TernaryArray value from another.
>
>**Parameters:**
>- `value1`: The value to subtract from.
>- `value2`: The value to subtract.
>
>
>**Returns:**
>A new TernaryArray representing the difference between the two values.

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **operator implicit**([Ternary3.Int27T](#ternary3int27t) value)

>Defines an implicit conversion of an Int27T to a TernaryArray.
>
>**Parameters:**
>- `value`: The Int27T value to convert.
>
>
>**Returns:**
>A TernaryArray representing the same value.

#### [Ternary3.Int27T](#ternary3int27t) **operator implicit**([Ternary3.TernaryArray](#ternary3ternaryarray) array)

>Defines an implicit conversion of an Int27T to a TernaryArray.
>
>**Parameters:**
>- `value`: The Int27T value to convert.
>
>
>**Returns:**
>A TernaryArray representing the same value.

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **operator implicit**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value)

>Defines an implicit conversion of an Int27T to a TernaryArray.
>
>**Parameters:**
>- `value`: The Int27T value to convert.
>
>
>**Returns:**
>A TernaryArray representing the same value.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **operator explicit**([Ternary3.TernaryArray](#ternary3ternaryarray) array)

>Defines an explicit conversion of a TernaryArray to an int.
>
>**Parameters:**
>- `array`: The TernaryArray to convert.
>
>
>**Returns:**
>An int representing the same value.

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **operator implicit**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) value)

>Defines an implicit conversion of an Int27T to a TernaryArray.
>
>**Parameters:**
>- `value`: The Int27T value to convert.
>
>
>**Returns:**
>A TernaryArray representing the same value.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **operator explicit**([Ternary3.TernaryArray](#ternary3ternaryarray) array)

>Defines an explicit conversion of a TernaryArray to an int.
>
>**Parameters:**
>- `array`: The TernaryArray to convert.
>
>
>**Returns:**
>An int representing the same value.

#### [System.Int128](https://learn.microsoft.com/en-us/dotnet/api/system.int128) **operator explicit**([Ternary3.TernaryArray](#ternary3ternaryarray) array)

>Defines an explicit conversion of a TernaryArray to an int.
>
>**Parameters:**
>- `array`: The TernaryArray to convert.
>
>
>**Returns:**
>An int representing the same value.


## Ternary3.TernaryArray27

>Represents a fixed-size array of 27 trits (ternary digits).

### Static Methods

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value)

>Parses a string representation of a TernaryArray27.
>
>**Parameters:**
>- `value`: The string value to parse.
>
>
>**Returns:**
>A TernaryArray27 representing the parsed value.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Parses a string representation of a TernaryArray27.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>
>
>**Returns:**
>A TernaryArray27 representing the parsed value.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a TernaryArray27.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A TernaryArray27 representing the parsed value.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a TernaryArray27.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A TernaryArray27 representing the parsed value.


### Static Fields

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **MinValue**

>Represents the minimum value that a TernaryArray27 can have (all trits set to -1).

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **MaxValue**

>Represents the maximum value that a TernaryArray27 can have (all trits set to 1).

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **Zero**

>Represents a TernaryArray27 with all trits set to zero.


### Constructors

#### **TernaryArray27**()

>Initializes a new instance of the TernaryArray27 struct with all trits set to zero.


### Methods

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **GetHashCode**()

>

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) obj)

>

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([Ternary3.TernaryArray27](#ternary3ternaryarray27) other)

>

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**()

>Returns a string representation of the TernaryArray27.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format)

>Returns a string representation of the TernaryArray27.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of the TernaryArray27.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format, [System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of the TernaryArray27.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Returns a string representation of this instance, formatted balanced ternarily according to the specified format.


### Properties

#### [Ternary3.Trit](#ternary3trit) **this[[System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) index]** { get; set; }

No documentation available.

#### [Ternary3.ITernaryArray](#ternary3iternaryarray) **this[[System.Range](https://learn.microsoft.com/en-us/dotnet/api/system.range) range]** { get; }

No documentation available.

#### [Ternary3.Trit](#ternary3trit) **this[[System.Index](https://learn.microsoft.com/en-us/dotnet/api/system.index) index]** { get; set; }

No documentation available.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **Length** { get; }

>Gets the length of the trit array, which is always 27.


### Operators

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator |**([Ternary3.TernaryArray27](#ternary3ternaryarray27) array, [System.Func<Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-2) operation)

>Applies a unary operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray27 with the operation applied to each trit.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator |**([Ternary3.TernaryArray27](#ternary3ternaryarray27) array, [Ternary3.Trit[]](#ternary3trit) table)

>Applies a lookup table operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `table`: The lookup table containing the transformation values.
>
>
>**Returns:**
>A new TernaryArray27 with the lookup operation applied to each trit.

#### [Ternary3.LookupTernaryArray27Operator](#ternary3lookupternaryarray27operator) **operator |**([Ternary3.TernaryArray27](#ternary3ternaryarray27) array, [System.Func<Ternary3.Trit, Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-3) operation)

>Applies a unary operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray27 with the operation applied to each trit.

#### [Ternary3.LookupTernaryArray27Operator](#ternary3lookupternaryarray27operator) **operator |**([Ternary3.TernaryArray27](#ternary3ternaryarray27) array, [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) table)

>Creates a binary operation context for this array using a lookup table.
>
>**Parameters:**
>- `array`: The source array.
>- `table`: The lookup table containing the transformation values.
>
>
>**Returns:**
>A LookupTernaryArray27Operator that can be used to apply the operation with another array.

#### [Ternary3.LookupTernaryArray27Operator](#ternary3lookupternaryarray27operator) **operator |**([Ternary3.TernaryArray27](#ternary3ternaryarray27) array, [Ternary3.Trit[,]](#ternary3trit) table)

>Applies a unary operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray27 with the operation applied to each trit.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator &lt;&lt;**([Ternary3.TernaryArray27](#ternary3ternaryarray27) array, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shift)

>Performs a left bitwise shift on the trit array.
>
>**Parameters:**
>- `array`: The source array.
>- `shift`: The number of positions to shift.
>
>
>**Returns:**
>A new TernaryArray27 with the bits shifted to the left.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator &gt;&gt;**([Ternary3.TernaryArray27](#ternary3ternaryarray27) array, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shift)

>Performs a right bitwise shift on the trit array.
>
>**Parameters:**
>- `array`: The source array.
>- `shift`: The number of positions to shift.
>
>
>**Returns:**
>A new TernaryArray27 with the bits shifted to the right.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator +**([Ternary3.TernaryArray27](#ternary3ternaryarray27) value1, [Ternary3.TernaryArray27](#ternary3ternaryarray27) value2)

>Adds two TernaryArray27 values together.
>
>**Parameters:**
>- `value1`: The first value to add.
>- `value2`: The second value to add.
>
>
>**Returns:**
>A new TernaryArray27 representing the sum of the two values.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator -**([Ternary3.TernaryArray27](#ternary3ternaryarray27) value1, [Ternary3.TernaryArray27](#ternary3ternaryarray27) value2)

>Subtracts one TernaryArray27 value from another.
>
>**Parameters:**
>- `value1`: The value to subtract from.
>- `value2`: The value to subtract.
>
>
>**Returns:**
>A new TernaryArray27 representing the difference between the two values.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator implicit**([Ternary3.Int27T](#ternary3int27t) value)

>Defines an implicit conversion of an Int27T to a TernaryArray27.
>
>**Parameters:**
>- `value`: The Int27T value to convert.
>
>
>**Returns:**
>A TernaryArray27 representing the same value.

#### [Ternary3.Int27T](#ternary3int27t) **operator implicit**([Ternary3.TernaryArray27](#ternary3ternaryarray27) array)

>Defines an implicit conversion of an Int27T to a TernaryArray27.
>
>**Parameters:**
>- `value`: The Int27T value to convert.
>
>
>**Returns:**
>A TernaryArray27 representing the same value.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator implicit**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value)

>Defines an implicit conversion of an Int27T to a TernaryArray27.
>
>**Parameters:**
>- `value`: The Int27T value to convert.
>
>
>**Returns:**
>A TernaryArray27 representing the same value.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **operator explicit**([Ternary3.TernaryArray27](#ternary3ternaryarray27) array)

>Defines an explicit conversion of a TernaryArray27 to an int.
>
>**Parameters:**
>- `array`: The TernaryArray27 to convert.
>
>
>**Returns:**
>An int representing the same value.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator implicit**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) value)

>Defines an implicit conversion of an Int27T to a TernaryArray27.
>
>**Parameters:**
>- `value`: The Int27T value to convert.
>
>
>**Returns:**
>A TernaryArray27 representing the same value.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **operator implicit**([Ternary3.TernaryArray27](#ternary3ternaryarray27) array)

>Defines an implicit conversion of an Int27T to a TernaryArray27.
>
>**Parameters:**
>- `value`: The Int27T value to convert.
>
>
>**Returns:**
>A TernaryArray27 representing the same value.


## Ternary3.TernaryArray3

>Represents a fixed-size array of 3 trits (ternary digits).

### Static Methods

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value)

>Parses a string representation of a TernaryArray3.
>
>**Parameters:**
>- `value`: The string value to parse.
>
>
>**Returns:**
>A TernaryArray3 representing the parsed value.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Parses a string representation of a TernaryArray3.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>
>
>**Returns:**
>A TernaryArray3 representing the parsed value.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a TernaryArray3.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A TernaryArray3 representing the parsed value.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a TernaryArray3.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A TernaryArray3 representing the parsed value.


### Static Fields

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **MinValue**

>Represents the minimum value that a TernaryArray3 can have (all trits set to -1).

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **MaxValue**

>Represents the maximum value that a TernaryArray3 can have (all trits set to 1).

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **Zero**

>Represents a TernaryArray3 with all trits set to zero.


### Constructors

#### **TernaryArray3**()

>Initializes a new instance of the TernaryArray3 struct with all trits set to zero.


### Methods

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **GetHashCode**()

>

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) obj)

>

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([Ternary3.TernaryArray3](#ternary3ternaryarray3) other)

>

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**()

>Returns a string representation of the TernaryArray3.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format)

>Returns a string representation of the TernaryArray3.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of the TernaryArray3.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format, [System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of the TernaryArray3.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Returns a string representation of this instance, formatted balanced ternarily according to the specified format.


### Properties

#### [Ternary3.Trit](#ternary3trit) **this[[System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) index]** { get; set; }

No documentation available.

#### [Ternary3.ITernaryArray](#ternary3iternaryarray) **this[[System.Range](https://learn.microsoft.com/en-us/dotnet/api/system.range) range]** { get; }

No documentation available.

#### [Ternary3.Trit](#ternary3trit) **this[[System.Index](https://learn.microsoft.com/en-us/dotnet/api/system.index) index]** { get; set; }

No documentation available.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **Length** { get; }

>Gets the length of the trit array, which is always 3.


### Operators

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator |**([Ternary3.TernaryArray3](#ternary3ternaryarray3) array, [System.Func<Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-2) operation)

>Applies a unary operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray3 with the operation applied to each trit.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator |**([Ternary3.TernaryArray3](#ternary3ternaryarray3) array, [Ternary3.Trit[]](#ternary3trit) table)

>Applies a lookup table operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `table`: The lookup table containing the transformation values.
>
>
>**Returns:**
>A new TernaryArray3 with the lookup operation applied to each trit.

#### [Ternary3.LookupTernaryArray3Operator](#ternary3lookupternaryarray3operator) **operator |**([Ternary3.TernaryArray3](#ternary3ternaryarray3) array, [System.Func<Ternary3.Trit, Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-3) operation)

>Applies a unary operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray3 with the operation applied to each trit.

#### [Ternary3.LookupTernaryArray3Operator](#ternary3lookupternaryarray3operator) **operator |**([Ternary3.TernaryArray3](#ternary3ternaryarray3) array, [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) table)

>Creates a binary operation context for this array using a lookup table.
>
>**Parameters:**
>- `array`: The source array.
>- `table`: The lookup table containing the transformation values.
>
>
>**Returns:**
>A LookupTernaryArray3Operator that can be used to apply the operation with another array.

#### [Ternary3.LookupTernaryArray3Operator](#ternary3lookupternaryarray3operator) **operator |**([Ternary3.TernaryArray3](#ternary3ternaryarray3) array, [Ternary3.Trit[,]](#ternary3trit) table)

>Applies a unary operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray3 with the operation applied to each trit.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator &lt;&lt;**([Ternary3.TernaryArray3](#ternary3ternaryarray3) array, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shift)

>Performs a left bitwise shift on the trit array.
>
>**Parameters:**
>- `array`: The source array.
>- `shift`: The number of positions to shift.
>
>
>**Returns:**
>A new TernaryArray3 with the bits shifted to the left.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator &gt;&gt;**([Ternary3.TernaryArray3](#ternary3ternaryarray3) array, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shift)

>Performs a right bitwise shift on the trit array.
>
>**Parameters:**
>- `array`: The source array.
>- `shift`: The number of positions to shift.
>
>
>**Returns:**
>A new TernaryArray3 with the bits shifted to the right.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator +**([Ternary3.TernaryArray3](#ternary3ternaryarray3) value1, [Ternary3.TernaryArray3](#ternary3ternaryarray3) value2)

>Adds two TernaryArray3 values together.
>
>**Parameters:**
>- `value1`: The first value to add.
>- `value2`: The second value to add.
>
>
>**Returns:**
>A new TernaryArray3 representing the sum of the two values.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator -**([Ternary3.TernaryArray3](#ternary3ternaryarray3) value1, [Ternary3.TernaryArray3](#ternary3ternaryarray3) value2)

>Subtracts one TernaryArray3 value from another.
>
>**Parameters:**
>- `value1`: The value to subtract from.
>- `value2`: The value to subtract.
>
>
>**Returns:**
>A new TernaryArray3 representing the difference between the two values.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator implicit**([Ternary3.Int3T](#ternary3int3t) value)

>Defines an implicit conversion of an Int3T to a TernaryArray3.
>
>**Parameters:**
>- `value`: The Int3T value to convert.
>
>
>**Returns:**
>A TernaryArray3 representing the same value.

#### [Ternary3.Int3T](#ternary3int3t) **operator implicit**([Ternary3.TernaryArray3](#ternary3ternaryarray3) array)

>Defines an implicit conversion of an Int3T to a TernaryArray3.
>
>**Parameters:**
>- `value`: The Int3T value to convert.
>
>
>**Returns:**
>A TernaryArray3 representing the same value.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator implicit**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) value)

>Defines an implicit conversion of an Int3T to a TernaryArray3.
>
>**Parameters:**
>- `value`: The Int3T value to convert.
>
>
>**Returns:**
>A TernaryArray3 representing the same value.

#### [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) **operator implicit**([Ternary3.TernaryArray3](#ternary3ternaryarray3) array)

>Defines an implicit conversion of an Int3T to a TernaryArray3.
>
>**Parameters:**
>- `value`: The Int3T value to convert.
>
>
>**Returns:**
>A TernaryArray3 representing the same value.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator explicit**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value)

>Defines an explicit conversion of an int to a TernaryArray3.
>
>**Parameters:**
>- `value`: The int value to convert.
>
>
>**Returns:**
>A TernaryArray3 representing the same value.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **operator implicit**([Ternary3.TernaryArray3](#ternary3ternaryarray3) array)

>Defines an implicit conversion of an Int3T to a TernaryArray3.
>
>**Parameters:**
>- `value`: The Int3T value to convert.
>
>
>**Returns:**
>A TernaryArray3 representing the same value.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator explicit**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) value)

>Defines an explicit conversion of an int to a TernaryArray3.
>
>**Parameters:**
>- `value`: The int value to convert.
>
>
>**Returns:**
>A TernaryArray3 representing the same value.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **operator implicit**([Ternary3.TernaryArray3](#ternary3ternaryarray3) array)

>Defines an implicit conversion of an Int3T to a TernaryArray3.
>
>**Parameters:**
>- `value`: The Int3T value to convert.
>
>
>**Returns:**
>A TernaryArray3 representing the same value.


## Ternary3.TernaryArray9

>Represents a fixed-size array of 9 trits (ternary digits).

### Static Methods

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value)

>Parses a string representation of a TernaryArray9.
>
>**Parameters:**
>- `value`: The string value to parse.
>
>
>**Returns:**
>A TernaryArray9 representing the parsed value.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Parses a string representation of a TernaryArray9.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>
>
>**Returns:**
>A TernaryArray9 representing the parsed value.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a TernaryArray9.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A TernaryArray9 representing the parsed value.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **Parse**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) value, [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format, [Ternary3.Formatting.TritParseOptions](#ternary3formattingtritparseoptions) options)

>Parses a string representation of a TernaryArray9.
>
>**Parameters:**
>- `value`: The string value to parse.
>- `format`: The format to use for parsing.
>- `options`: The options to use for parsing.
>
>
>**Returns:**
>A TernaryArray9 representing the parsed value.


### Static Fields

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **MinValue**

>Represents the minimum value that a TernaryArray9 can have (all trits set to -1).

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **MaxValue**

>Represents the maximum value that a TernaryArray9 can have (all trits set to 1).

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **Zero**

>Represents a TernaryArray9 with all trits set to zero.


### Constructors

#### **TernaryArray9**()

>Initializes a new instance of the TernaryArray9 struct with all trits set to zero.


### Methods

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **GetHashCode**()

>

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) obj)

>

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([Ternary3.TernaryArray9](#ternary3ternaryarray9) other)

>

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**()

>Returns a string representation of the TernaryArray9.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format)

>Returns a string representation of the TernaryArray9.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of the TernaryArray9.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) format, [System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) provider)

>Returns a string representation of the TernaryArray9.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format)

>Returns a string representation of this instance, formatted balanced ternarily according to the specified format.


### Properties

#### [Ternary3.Trit](#ternary3trit) **this[[System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) index]** { get; set; }

No documentation available.

#### [Ternary3.ITernaryArray](#ternary3iternaryarray) **this[[System.Range](https://learn.microsoft.com/en-us/dotnet/api/system.range) range]** { get; }

No documentation available.

#### [Ternary3.Trit](#ternary3trit) **this[[System.Index](https://learn.microsoft.com/en-us/dotnet/api/system.index) index]** { get; set; }

No documentation available.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **Length** { get; }

>Gets the length of the trit array, which is always 9.


### Operators

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator |**([Ternary3.TernaryArray9](#ternary3ternaryarray9) array, [System.Func<Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-2) operation)

>Applies a unary operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray9 with the operation applied to each trit.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator |**([Ternary3.TernaryArray9](#ternary3ternaryarray9) array, [Ternary3.Trit[]](#ternary3trit) table)

>Applies a lookup table operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `table`: The lookup table containing the transformation values.
>
>
>**Returns:**
>A new TernaryArray9 with the lookup operation applied to each trit.

#### [Ternary3.LookupTernaryArray9Operator](#ternary3lookupternaryarray9operator) **operator |**([Ternary3.TernaryArray9](#ternary3ternaryarray9) array, [System.Func<Ternary3.Trit, Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-3) operation)

>Applies a unary operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray9 with the operation applied to each trit.

#### [Ternary3.LookupTernaryArray9Operator](#ternary3lookupternaryarray9operator) **operator |**([Ternary3.TernaryArray9](#ternary3ternaryarray9) array, [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) table)

>Creates a binary operation context for this array using a lookup table.
>
>**Parameters:**
>- `array`: The source array.
>- `table`: The lookup table containing the transformation values.
>
>
>**Returns:**
>A LookupTernaryArray9Operator that can be used to apply the operation with another array.

#### [Ternary3.LookupTernaryArray9Operator](#ternary3lookupternaryarray9operator) **operator |**([Ternary3.TernaryArray9](#ternary3ternaryarray9) array, [Ternary3.Trit[,]](#ternary3trit) table)

>Applies a unary operation to each trit in the array.
>
>**Parameters:**
>- `array`: The source array.
>- `operation`: The unary operation to apply to each trit.
>
>
>**Returns:**
>A new TernaryArray9 with the operation applied to each trit.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator &lt;&lt;**([Ternary3.TernaryArray9](#ternary3ternaryarray9) array, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shift)

>Performs a left bitwise shift on the trit array.
>
>**Parameters:**
>- `array`: The source array.
>- `shift`: The number of positions to shift.
>
>
>**Returns:**
>A new TernaryArray9 with the bits shifted to the left.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator &gt;&gt;**([Ternary3.TernaryArray9](#ternary3ternaryarray9) array, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) shift)

>Performs a right bitwise shift on the trit array.
>
>**Parameters:**
>- `array`: The source array.
>- `shift`: The number of positions to shift.
>
>
>**Returns:**
>A new TernaryArray9 with the bits shifted to the right.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator +**([Ternary3.TernaryArray9](#ternary3ternaryarray9) value1, [Ternary3.TernaryArray9](#ternary3ternaryarray9) value2)

>Adds two TernaryArray9 values together.
>
>**Parameters:**
>- `value1`: The first value to add.
>- `value2`: The second value to add.
>
>
>**Returns:**
>A new TernaryArray9 representing the sum of the two values.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator -**([Ternary3.TernaryArray9](#ternary3ternaryarray9) value1, [Ternary3.TernaryArray9](#ternary3ternaryarray9) value2)

>Subtracts one TernaryArray9 value from another.
>
>**Parameters:**
>- `value1`: The value to subtract from.
>- `value2`: The value to subtract.
>
>
>**Returns:**
>A new TernaryArray9 representing the difference between the two values.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator implicit**([Ternary3.Int9T](#ternary3int9t) value)

>Defines an implicit conversion of an Int9T to a TernaryArray9.
>
>**Parameters:**
>- `value`: The Int9T value to convert.
>
>
>**Returns:**
>A TernaryArray9 representing the same value.

#### [Ternary3.Int9T](#ternary3int9t) **operator implicit**([Ternary3.TernaryArray9](#ternary3ternaryarray9) array)

>Defines an implicit conversion of an Int9T to a TernaryArray9.
>
>**Parameters:**
>- `value`: The Int9T value to convert.
>
>
>**Returns:**
>A TernaryArray9 representing the same value.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator implicit**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) value)

>Defines an implicit conversion of an Int9T to a TernaryArray9.
>
>**Parameters:**
>- `value`: The Int9T value to convert.
>
>
>**Returns:**
>A TernaryArray9 representing the same value.

#### [System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) **operator implicit**([Ternary3.TernaryArray9](#ternary3ternaryarray9) array)

>Defines an implicit conversion of an Int9T to a TernaryArray9.
>
>**Parameters:**
>- `value`: The Int9T value to convert.
>
>
>**Returns:**
>A TernaryArray9 representing the same value.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator explicit**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value)

>Defines an explicit conversion of an int to a TernaryArray9.
>
>**Parameters:**
>- `value`: The int value to convert.
>
>
>**Returns:**
>A TernaryArray9 representing the same value.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **operator implicit**([Ternary3.TernaryArray9](#ternary3ternaryarray9) array)

>Defines an implicit conversion of an Int9T to a TernaryArray9.
>
>**Parameters:**
>- `value`: The Int9T value to convert.
>
>
>**Returns:**
>A TernaryArray9 representing the same value.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator explicit**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) value)

>Defines an explicit conversion of an int to a TernaryArray9.
>
>**Parameters:**
>- `value`: The int value to convert.
>
>
>**Returns:**
>A TernaryArray9 representing the same value.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **operator implicit**([Ternary3.TernaryArray9](#ternary3ternaryarray9) array)

>Defines an implicit conversion of an Int9T to a TernaryArray9.
>
>**Parameters:**
>- `value`: The Int9T value to convert.
>
>
>**Returns:**
>A TernaryArray9 representing the same value.


## Ternary3.TernaryPadding

>Indicates how padding should be applied to the formatted ternary string.

### Enum Values

- **None** = `0`
>No padding is applied.
- **Group** = `1`
>Padding is applied to fill the last group.
- **Full** = `2`
>Padding is applied to fill the entire formatted string to a fixed width.

## Ternary3.Trit

>Represents a trinary (three-valued) logical value that can be Negative (-1), Zero (0), or Positive (1).

### Static Properties

#### [Ternary3.Trit[]](#ternary3trit) **AllValues** { get; }

>Gets an array containing all possible Trit values: Negative, Zero, and Positive.


### Static Fields

#### [Ternary3.Trit](#ternary3trit) **Zero**

>Represents the Zero (0) value of the Trit.

#### [Ternary3.Trit](#ternary3trit) **Positive**

>Represents the Positive (1) value of the Trit.

#### [Ternary3.Trit](#ternary3trit) **Negative**

>Represents the Negative (-1) value of the Trit.


### Methods

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**()

>Returns a string representation of the current Trit value.
>
>**Returns:**
>"Positive" for 1, "Zero" for 0, and "Negative" for -1.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([Ternary3.Trit](#ternary3trit) other)

>Indicates whether the current Trit is equal to another Trit.
>
>**Parameters:**
>- `other`: A Trit to compare with this Trit.
>
>
>**Returns:**
>True if the value of the current Trit is equal to the value of the other Trit; otherwise, false.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) obj)

>Returns a value indicating whether this instance is equal to a specified object.
>
>**Parameters:**
>- `obj`: An object to compare with this instance.
>
>
>**Returns:**
>True if obj is a Trit and has the same value as this instance; otherwise, false.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **GetHashCode**()

>Returns the hash code for this instance.
>
>**Returns:**
>A 32-bit signed integer hash code.


### Operators

#### [Ternary3.Trit](#ternary3trit) **operator implicit**([System.Nullable<System.Boolean>](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1) value)

>Converts the specified nullable Boolean value to a Trit value.
>
>**Parameters:**
>- `value`: A nullable Boolean value to convert.
>
>
>**Returns:**
>A Trit value where:
>            - true converts to Positive (1)
>            - false converts to Negative (-1)
>            - null converts to Zero (0)

#### [Ternary3.Trit](#ternary3trit) **operator implicit**([System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) value)

>Converts the specified nullable Boolean value to a Trit value.
>
>**Parameters:**
>- `value`: A nullable Boolean value to convert.
>
>
>**Returns:**
>A Trit value where:
>            - true converts to Positive (1)
>            - false converts to Negative (-1)
>            - null converts to Zero (0)

#### [System.Nullable<System.Boolean>](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1) **operator implicit**([Ternary3.Trit](#ternary3trit) trit)

>Converts the specified nullable Boolean value to a Trit value.
>
>**Parameters:**
>- `value`: A nullable Boolean value to convert.
>
>
>**Returns:**
>A Trit value where:
>            - true converts to Positive (1)
>            - false converts to Negative (-1)
>            - null converts to Zero (0)

#### [System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) **operator implicit**([Ternary3.Trit](#ternary3trit) trit)

>Converts the specified nullable Boolean value to a Trit value.
>
>**Parameters:**
>- `value`: A nullable Boolean value to convert.
>
>
>**Returns:**
>A Trit value where:
>            - true converts to Positive (1)
>            - false converts to Negative (-1)
>            - null converts to Zero (0)

#### [Ternary3.Trit](#ternary3trit) **operator explicit**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) value)

>Converts the specified signed byte value to a Trit value.
>
>**Parameters:**
>- `value`: The signed byte value to convert. Must be -1, 0, or 1.
>
>
>**Returns:**
>A Trit value corresponding to the input.
>
>
>**Exceptions:**
>- `System.ArgumentOutOfRangeException`: Thrown when the value is not -1, 0, or 1.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **operator implicit**([Ternary3.Trit](#ternary3trit) trit)

>Converts the specified nullable Boolean value to a Trit value.
>
>**Parameters:**
>- `value`: A nullable Boolean value to convert.
>
>
>**Returns:**
>A Trit value where:
>            - true converts to Positive (1)
>            - false converts to Negative (-1)
>            - null converts to Zero (0)

#### [Ternary3.Trit](#ternary3trit) **operator explicit**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value)

>Converts the specified signed byte value to a Trit value.
>
>**Parameters:**
>- `value`: The signed byte value to convert. Must be -1, 0, or 1.
>
>
>**Returns:**
>A Trit value corresponding to the input.
>
>
>**Exceptions:**
>- `System.ArgumentOutOfRangeException`: Thrown when the value is not -1, 0, or 1.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator true**([Ternary3.Trit](#ternary3trit) trit)

>Returns true if the value is Positive (1), false otherwise.
>
>**Parameters:**
>- `trit`: The Trit value to check.
>
>
>**Returns:**
>True if the value is Positive (1), false otherwise.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator false**([Ternary3.Trit](#ternary3trit) trit)

>Returns true if the value is Negative (-1), false otherwise.
>
>**Parameters:**
>- `trit`: The Trit value to check.
>
>
>**Returns:**
>True if the value is Negative (-1), false otherwise.

#### [Ternary3.Trit](#ternary3trit) **operator !**([Ternary3.Trit](#ternary3trit) trit)

>Performs a logical NOT operation on a Trit value.
>
>**Parameters:**
>- `trit`: The Trit value to negate.
>
>
>**Returns:**
>The logical negation: Positive becomes Negative, Negative becomes Positive, Zero remains Zero.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator ==**([Ternary3.Trit](#ternary3trit) left, [Ternary3.Trit](#ternary3trit) right)

>Determines if two Trit values are equal.
>
>**Parameters:**
>- `left`: The first Trit to compare.
>- `right`: The second Trit to compare.
>
>
>**Returns:**
>True if both Trits have the same value, false otherwise.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator !=**([Ternary3.Trit](#ternary3trit) left, [Ternary3.Trit](#ternary3trit) right)

>Determines if two Trit values are not equal.
>
>**Parameters:**
>- `left`: The first Trit to compare.
>- `right`: The second Trit to compare.
>
>
>**Returns:**
>True if the Trits have different values, false otherwise.

#### [Ternary3.Operators.BinaryMethodTritOperator](#ternary3operatorsbinarymethodtritoperator) **operator |**([Ternary3.Trit](#ternary3trit) left, [System.Func<Ternary3.Trit, Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-3) operation)

>Creates a TritOperator to enable custom operations using the pipe syntax.
>
>**Parameters:**
>- `left`: The left Trit operand that will be stored in the operator.
>- `operation`: A delegate function that accepts two Trit parameters and returns a Trit result.
>
>
>**Returns:**
>A TritOperator that combines the left Trit with the operation delegate.
>
>
>**Remarks:**
>
>This enables syntax like: trit1 | Operation.ApplyFunc | trit2 to perform custom operations.
>            Provides a safe alternative to the function pointer approach that doesn't require unsafe code.

#### [Ternary3.Operators.BinaryLookupTritOperator](#ternary3operatorsbinarylookuptritoperator) **operator |**([Ternary3.Trit](#ternary3trit) left, [Ternary3.Trit[,]](#ternary3trit) lookupTable)

>Creates a TritOperator to enable custom operations using the pipe syntax.
>
>**Parameters:**
>- `left`: The left Trit operand that will be stored in the operator.
>- `operation`: A delegate function that accepts two Trit parameters and returns a Trit result.
>
>
>**Returns:**
>A TritOperator that combines the left Trit with the operation delegate.
>
>
>**Remarks:**
>
>This enables syntax like: trit1 | Operation.ApplyFunc | trit2 to perform custom operations.
>            Provides a safe alternative to the function pointer approach that doesn't require unsafe code.

#### [Ternary3.Operators.BinaryLookupTritOperator](#ternary3operatorsbinarylookuptritoperator) **operator |**([Ternary3.Trit](#ternary3trit) left, [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) operation)

>Creates a LookupTritOperator to enable custom operations using pre-computed lookup tables.
>
>**Parameters:**
>- `left`: The left Trit operand that will be stored in the operator.
>- `operation`: A 3x3 lookup table where indices [leftValue+1, rightValue+1] map to result Trit values.
>
>
>**Returns:**
>A TableBasedTritOperator that combines the left Trit with the lookup table.
>
>
>**Remarks:**
>
>This enables syntax like: trit1 | operationTable | trit2 to perform operations using table lookups.
>            Optimizes performance for custom operations by using direct table access instead of function calls.
>            The table must be indexed from 0-2, with 0 corresponding to -1, 1 to 0, and 2 to +1 Trit values.

#### [Ternary3.Trit](#ternary3trit) **operator |**([Ternary3.Trit](#ternary3trit) left, [Ternary3.Trit[]](#ternary3trit) operation)

>Performs a unary operation on a Trit value using a lookup table.
>
>**Parameters:**
>- `left`: The operand
>- `operation`: The operation, represented as a 3 trit array

#### [Ternary3.Trit](#ternary3trit) **operator |**([Ternary3.Trit](#ternary3trit) left, [System.Func<Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-2) operation)

>Creates a TritOperator to enable custom operations using the pipe syntax.
>
>**Parameters:**
>- `left`: The left Trit operand that will be stored in the operator.
>- `operation`: A delegate function that accepts two Trit parameters and returns a Trit result.
>
>
>**Returns:**
>A TritOperator that combines the left Trit with the operation delegate.
>
>
>**Remarks:**
>
>This enables syntax like: trit1 | Operation.ApplyFunc | trit2 to perform custom operations.
>            Provides a safe alternative to the function pointer approach that doesn't require unsafe code.

#### [Ternary3.Trit](#ternary3trit) **operator ^**([System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) left, [Ternary3.Trit](#ternary3trit) right)

>Performs a logical XOR operation between a Boolean and a Trit value.
>
>**Parameters:**
>- `left`: The Boolean operand.
>- `right`: The Trit operand.
>
>
>**Returns:**
>A Trit value representing the logical XOR operation.

#### [Ternary3.Trit](#ternary3trit) **operator ^**([Ternary3.Trit](#ternary3trit) left, [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) right)

>Performs a logical XOR operation between a Trit value and a Boolean.
>
>**Parameters:**
>- `left`: The Trit operand.
>- `right`: The Boolean operand.
>
>
>**Returns:**
>A Trit value representing the logical XOR operation.


## Ternary3.Operators.BinaryLookupTritOperator

>Represents an operator that combines a Trit value with a pre-computed lookup table for operations.
>
>**Remarks:**
>
>Used exclusively with Trit's pipe operator overload to enable the syntax: 
>```csharp
>trit1 | operationLookupTable | trit2
>```
> Optimizes performance by using direct table lookups instead of function calls.
>            The lookup table is a specialized 3x3 matrix where indices correspond to:
>            [leftTrit.Value+1, rightValue+1] for the values (-1,0,1).

### Operators

#### [Ternary3.Trit](#ternary3trit) **operator |**([Ternary3.Operators.BinaryLookupTritOperator](#ternary3operatorsbinarylookuptritoperator) left, [Ternary3.Trit](#ternary3trit) right)

>Performs a binary operation between the stored left operand (trit) and the right operand using the lookup table.
>
>**Parameters:**
>- `left`: The BinaryLookupTritOperator containing the left operand and the operation lookup table.
>- `right`: The right operand.
>
>
>**Returns:**
>The result of applying the binary operation defined by the lookup table to the two trit operands.


## Ternary3.Operators.BinaryMethodTritOperator

>Represents an operator that combines a Trit value with a delegate to an operation.
>
>**Remarks:**
>
>Used exclusively with Trit's pipe operator overload to enable the syntax: 
>```csharp
>trit1 | Operation.ApplyFunc | trit2
>```
> Provides a safe alternative to UnsafeTritOperator by using delegates instead of function pointers.
>            Can be used in any context without unsafe code requirements.

### Operators

#### [Ternary3.Trit](#ternary3trit) **operator |**([Ternary3.Operators.BinaryMethodTritOperator](#ternary3operatorsbinarymethodtritoperator) left, [Ternary3.Trit](#ternary3trit) right)

>Performs a binary operation between the stored left operand (trit) and the right operand using the operation function.
>
>**Parameters:**
>- `left`: The BinaryMethodTritOperator containing the left operand and the operation function.
>- `right`: The right operand.
>
>
>**Returns:**
>The result of applying the binary operation to the two trit operands.


## Ternary3.Operators.BinaryTritOperator

>Represents a specialized 3x3 lookup table for Trit operations.
>
>**Remarks:**
>
>Optimized for performance using two shorts as backing storage. Each trit uses 1 bit in each short:
>            - A trit is negative when its bit in Negative is 1 and its bit in Positive is 0
>            - A trit is zero when both bits are 0
>            - A trit is positive when its bit in Negative is 0 and its bit in Positive is 1
>            This representation allows for efficient lookups while minimizing storage (18 bits total).

### Static Fields

#### [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) **Positive**

>Represents a constant that returns Positive (1) for any trit combination.
>
>**Remarks:**
>
>Truth table representation: 
>```csharp
> 1 | T 0 1
>---+------
> T | 1 1 1
> 0 | 1 1 1
> 1 | 1 1 1
>```

#### [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) **Zero**

>Represents a constant that returns Zero (0) for any trit combination.
>
>**Remarks:**
>
>Truth table representation: 
>```csharp
> 0 | T 0 1
>---+------
> T | 0 0 0
> 0 | 0 0 0
> 1 | 0 0 0
>```

#### [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) **Negative**

>Represents a constant that returns Negative (-1) for any trit combination.
>
>**Remarks:**
>
>Truth table representation: 
>```csharp
>-1 | T 0 1
>---+------
> T | T T T
> 0 | T T T
> 1 | T T T
>```

#### [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) **And**

>Implements the standard AND operation in three-valued logic (Kleene/Priest logic).
>
>**Remarks:**
>
>Truth table representation: 
>```csharp
>AND| T 0 1
>---+------
> T | T T T
> 0 | T 0 0
> 1 | T 0 1
>```

#### [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) **Or**

>Implements the standard OR operation in three-valued logic (Kleene/Priest logic).
>
>**Remarks:**
>
>Truth table representation: 
>```csharp
>OR | T 0 1
>---+------
> T | T T 1
> 0 | T 0 1
> 1 | 1 1 1
>```

#### [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) **Xor**

>Implements the standard XOR operation in three-valued logic (Kleene/Priest logic).
>
>**Remarks:**
>
>Truth table representation: 
>```csharp
>xor| T 0 1
>---+------
> T | T 0 1
> 0 | 0 0 0
> 1 | 1 0 T
>```

#### [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) **Multiply**

>Implements the standard MULTIPLY operation in three-valued logic (Kleene/Priest logic).
>
>**Remarks:**
>
>Truth table representation: 
>```csharp
> × | T 0 1
>---+------
> T | 1 0 T
> 0 | 0 0 0
> 1 | T 0 1
>```

#### [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) **Plus**

>Implements the standard Plus operation in three-valued logic
>
>**Remarks:**
>
>Truth table representation: 
>```csharp
> + | T 0 1
>---+------
> T | T T 0
> 0 | T 0 1
> 1 | 0 1 1
>```

#### [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) **Minus**

>Implements the standard Minus operation in three-valued logic.
>
>**Remarks:**
>
>Truth table representation: 
>```csharp
> - | T 0 1
>---+------
> T | 0 T T
> 0 | 1 0 T
> 1 | 1 1 0
>```

#### [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) **Implicates**

>==&gt; The First implies the Second.
>
>**Remarks:**
>
>
>```csharp
>==>| T 0 1
>---+------
> T | 1 1 1
> 0 | 0 0 1
> 1 | T 0 1
>```

#### [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) **Is**

>Are the two trits equal?
>
>**Remarks:**
>
>
>```csharp
>==>| T 0 1
>---+------
> T | 1 T T
> 0 | T 1 T
> 1 | T T 1
>```

#### [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) **GreaterThan**

>Is the first trit greater than the second?
>
>**Remarks:**
>
>
>```csharp
>gt | T 0 1
>---+------
> T | T 1 1
> 0 | T T 1
> 1 | T T T
>```

#### [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) **LesserThan**

>Is the first trit greater than the second?
>
>**Remarks:**
>
>
>```csharp
>gt | T 0 1
>---+------
> T | 1 T T
> 0 | 1 1 T
> 1 | 1 1 1
>```


### Constructors

#### **BinaryTritOperator**()

>Creates a new instance of the [BinaryTritOperator](#ternary3.operators.binarytritoperator) structure with default values.

#### **BinaryTritOperator**([System.ReadOnlySpan<Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1) trits)

>Creates a new instance of the [BinaryTritOperator](#ternary3.operators.binarytritoperator) structure with default values.

#### **BinaryTritOperator**([Ternary3.Trit](#ternary3trit) tritTT, [Ternary3.Trit](#ternary3trit) tritT0, [Ternary3.Trit](#ternary3trit) tritT1, [Ternary3.Trit](#ternary3trit) trit0T, [Ternary3.Trit](#ternary3trit) trit00, [Ternary3.Trit](#ternary3trit) trit01, [Ternary3.Trit](#ternary3trit) trit1T, [Ternary3.Trit](#ternary3trit) trit10, [Ternary3.Trit](#ternary3trit) trit11)

>Creates a BinaryTritOperator from individual Trit values in row-major order.
>
>**Parameters:**
>- `tritTT`: The result when the first operand is Negative and the second operand is Negative.
>- `tritT0`: The result when the first operand is Negative and the second operand is Zero.
>- `tritT1`: The result when the first operand is Negative and the second operand is Positive.
>- `trit0T`: The result when the first operand is Zero and the second operand is Negative.
>- `trit00`: The result when the first operand is Zero and the second operand is Zero.
>- `trit01`: The result when the first operand is Zero and the second operand is Positive.
>- `trit1T`: The result when the first operand is Positive and the second operand is Negative.
>- `trit10`: The result when the first operand is Positive and the second operand is Zero.
>- `trit11`: The result when the first operand is Positive and the second operand is Positive.
>
>
>**Remarks:**
>
>Parameters represent each cell in the 3x3 matrix, in row-major order:
>            tTT tT0 tT1
>            t0T t00 t01
>            t1T t10 t11

#### **BinaryTritOperator**([Ternary3.TernaryArray9](#ternary3ternaryarray9) ternaries)

>Creates a BinaryTritOperator from a [TernaryArray9](#ternary3.ternaryarray9) instance.
>
>**Parameters:**
>- `ternaries`: A TernaryArray9 containing the operation results in row-major order.
>
>
>**Remarks:**
>
>Parameters represent each cell in the 3x3 matrix, in row-major order:
>            tTT tT0 tT1
>            t0T t00 t01
>            t1T t10 t11

#### **BinaryTritOperator**([Ternary3.Trit[,]](#ternary3trit) trits)

>Creates a new instance of the [BinaryTritOperator](#ternary3.operators.binarytritoperator) structure with default values.

#### **BinaryTritOperator**([System.Int32[][]](https://learn.microsoft.com/en-us/dotnet/api/system.int32[][]) trits)

>Creates a BinaryTritOperator from a jagged array of integer values.
>
>**Parameters:**
>- `trits`: A 3x3 jagged array of integers where values must be -1, 0, or 1.
>
>
>**Exceptions:**
>- `System.ArgumentException`: Thrown when the array dimensions are not 3x3 or when any value is not -1, 0, or 1.


### Methods

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) obj)

>Determines whether the specified object is equal to the current BinaryTritOperator.
>
>**Parameters:**
>- `obj`: The object to compare with the current BinaryTritOperator.
>
>
>**Returns:**
>true if the objects are equal; otherwise, false.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Equals**([Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) other)

>Determines whether the specified BinaryTritOperator is equal to the current BinaryTritOperator.
>
>**Parameters:**
>- `other`: The BinaryTritOperator to compare with the current BinaryTritOperator.
>
>
>**Returns:**
>true if the specified BinaryTritOperator is equal to the current BinaryTritOperator; otherwise, false.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **GetHashCode**()

>Returns the hash code for this BinaryTritOperator.
>
>**Returns:**
>A 32-bit signed integer hash code.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**()

>Returns a string representation of the lookup table in a grid format.
>
>**Returns:**
>A formatted string showing the 3x3 lookup table.


### Properties

#### [Ternary3.Trit](#ternary3trit) **this[[Ternary3.Trit](#ternary3trit) left, [Ternary3.Trit](#ternary3trit) right]** { get; set; }

No documentation available.


### Operators

#### [Ternary3.LookupTernaryArray27Operator](#ternary3lookupternaryarray27operator) **operator |**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) value, [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) table)

>Adds the pipe operator to `Int64` .
>Overflows if the value exceeds the range of Int27T.
>
>**Parameters:**
>- `value`: The value to convert to trits
>- `table`: The lookup table

#### [Ternary3.LookupTernaryArray27Operator](#ternary3lookupternaryarray27operator) **operator |**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value, [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) table)

>Adds the pipe operator to `Int32` .
>
>**Parameters:**
>- `value`: The value to convert to trits
>- `table`: The lookup table

#### [Ternary3.LookupTernaryArray9Operator](#ternary3lookupternaryarray9operator) **operator |**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) value, [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) table)

>Adds the pipe operator to `Int16` .
>Overflows if the value exceeds the range of Int27T.
>
>**Parameters:**
>- `value`: The value to convert to trits
>- `table`: The lookup table

#### [Ternary3.LookupTernaryArray3Operator](#ternary3lookupternaryarray3operator) **operator |**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) value, [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) table)

>Adds the pipe operator to `SByte` .
>Overflows if the value exceeds the range of Int27T.
>
>**Parameters:**
>- `value`: The value to convert to trits
>- `table`: The lookup table

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator ==**([Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) left, [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) right)

>Determines whether two specified BinaryTritOperator objects have the same value.
>
>**Parameters:**
>- `left`: The first BinaryTritOperator to compare.
>- `right`: The second BinaryTritOperator to compare.
>
>
>**Returns:**
>true if the value of left is the same as the value of right; otherwise, false.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **operator !=**([Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) left, [Ternary3.Operators.BinaryTritOperator](#ternary3operatorsbinarytritoperator) right)

>Determines whether two specified BinaryTritOperator objects have different values.
>
>**Parameters:**
>- `left`: The first BinaryTritOperator to compare.
>- `right`: The second BinaryTritOperator to compare.
>
>
>**Returns:**
>true if the value of left is different from the value of right; otherwise, false.


## Ternary3.Operators.UnaryTritOperator

>Represents a unary operator for trit operations.

### Static Fields

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **Negative**

>Negative value.
>Negative for negative, zero and positive.
>[T, T, T]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **Decrement**

>Decrement.
>One less for every value greater than negative.
>[T, T, 0]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **IsPositive**

>Is the value positive?
>Positive for positive, negative otherwise.
>[T, T, 1]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **NegateAbsoluteValue**

>Negate the Absolute Value.
>Zero for zero, negative otherwise.
>[T, 0, T]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **Ceil**

>Ceiling Zero.
>Negative for negative, zero otherwise.
>[T, 0, 0]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **Identity**

>Identity.
>Negative for negative, zero for zero, positive for positive.
>[T, 0, 1]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **IsZero**

>Is the value zero?
>Positive for zero, negative otherwise.
>[T, 1, T]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **KeepNegative**

>Keep the value unchanged if it is negative.
>Zero for positive and vice versa.
>[T, 1, 0]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **IsNotNegative**

>Is the value not negative?
>Negative for negative, positive otherwise.
>[T, 1, 1]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **CeilIsNegative**

>Ceiling Zero of Is Negative.
>Zero for negative, negative otherwise.
>[0, T, T]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **CeilIsNotZero**

>Is Not Zero, Ceiling Zero.
>Zero for positive, negative otherwise.
>[0, T, 0]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **KeepPositive**

>Keep Positive.
>Positive for positive, negative otherwise.
>[0, T, 1]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **CeilIsNotPositive**

>Is Not Positive, Ceiling Zero.
>Negative for positive, zero otherwise.
>[0, 0, T]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **Zero**

>Zero.
>Always zero.
>[0, 0, 0]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **Floor**

>Floor.
>Positive for positive, zero otherwise.
>[0, 0, 1]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **FloorIsPositive**

>Alias for [Floor](#f:ternary3.operators.unarytritoperator.floor) .
>Positive for positive, zero otherwise.
>[0, 0, 1]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **CyclicIncrement**

>Cyclic Increment.
>Positive for positive, negative otherwise.
>[0, 1, T]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **FloorIsZero**

>Floor Is Zero.
>Zero for zero, positive otherwise.
>[0, 1, 0]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **Increment**

>Increment.
>Zero for negative, positive otherwise.
>[0, 1, 1]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **IsNegative**

>Is the value negative?
>Negative for negative, positive otherwise.
>[1, T, T]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **CyclicDecrement**

>Cyclic Decrement.
>Negative for positive, zero for zero, positive for negative.
>[1, T, 0]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **IsNotZero**

>Is Not Zero.
>Positive for negative and positive, negative for zero.
>[1, T, 1]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **Negate**

>Negate.
>Positive for negative, negative for positive.
>[1, 0, T]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **FloorIsNegative**

>Floor Is Negative.
>Zero for zero, positive otherwise.
>[1, 0, 0]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **AbsoluteValue**

>Absolute Value.
>Always positive.
>[1, 0, 1]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **IsNotPositive**

>Is Not Positive.
>Positive for negative and zero, negative for positive.
>[1, 1, T]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **FloorIsNotPositive**

>Floor Is Not Positive.
>Zero for positive, positive otherwise.
>[1, 1, 0]

#### [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) **Positive**

>Positive.
>Always positive.
>[1, 1, 1]


### Constructors

#### **UnaryTritOperator**([System.Func<Ternary3.Trit, Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.func-2) operation)

>Initializes a new instance of the [UnaryTritOperator](#ternary3.operators.unarytritoperator) struct with the specified operation index.
>
>**Parameters:**
>- `operationIndex`: The index of the operation in the lookup table.

#### **UnaryTritOperator**([System.Span<Ternary3.Trit>](https://learn.microsoft.com/en-us/dotnet/api/system.span-1) lookup)

>Initializes a new instance of the [UnaryTritOperator](#ternary3.operators.unarytritoperator) struct with the specified operation index.
>
>**Parameters:**
>- `operationIndex`: The index of the operation in the lookup table.

#### **UnaryTritOperator**([Ternary3.Trit](#ternary3trit) trit1, [Ternary3.Trit](#ternary3trit) trit2, [Ternary3.Trit](#ternary3trit) trit3)

>Initializes a new instance of the [UnaryTritOperator](#ternary3.operators.unarytritoperator) struct with the results for each input trit value.
>
>**Parameters:**
>- `trit1`: The result for Trit.Negative input.
>- `trit2`: The result for Trit.Zero input.
>- `trit3`: The result for Trit.Positive input.

#### **UnaryTritOperator**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value1, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value2, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value3)

>Initializes a new instance of the [UnaryTritOperator](#ternary3.operators.unarytritoperator) struct with three integers representing trit values.
>
>**Parameters:**
>- `value1`: The result for Trit.Negative input (-1, 0, or 1).
>- `value2`: The result for Trit.Zero input (-1, 0, or 1).
>- `value3`: The result for Trit.Positive input (-1, 0, or 1).
>
>
>**Exceptions:**
>- `System.ArgumentOutOfRangeException`: Thrown when any value is not -1, 0, or 1.

#### **UnaryTritOperator**([System.ReadOnlySpan<System.Int32>](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1) values)

>Initializes a new instance of the [UnaryTritOperator](#ternary3.operators.unarytritoperator) struct with the specified operation index.
>
>**Parameters:**
>- `operationIndex`: The index of the operation in the lookup table.

#### **UnaryTritOperator**([System.ReadOnlySpan<System.Nullable<System.Boolean>>](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-2) values)

>Initializes a new instance of the [UnaryTritOperator](#ternary3.operators.unarytritoperator) struct with the specified operation index.
>
>**Parameters:**
>- `operationIndex`: The index of the operation in the lookup table.


### Methods

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**()

>Returns a string representation of the unary operation in a detailed format.
>
>**Returns:**
>A formatted string showing the operation results for each input value.


### Operators

#### [Ternary3.Trit](#ternary3trit) **operator |**([Ternary3.Trit](#ternary3trit) trit, [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) unaryTritOperator)

>Applies the unary operation to a single trit.
>
>**Parameters:**
>- `trit`: The trit to operate on.
>- `unaryTritOperator`: The operator to apply.
>
>
>**Returns:**
>The result of applying the operation to the trit.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator |**([Ternary3.TernaryArray3](#ternary3ternaryarray3) ternaries, [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) unaryTritOperator)

>Applies the unary operation to a TernaryArray3 instance.
>
>**Parameters:**
>- `ternaries`: The TernaryArray3 to operate on.
>- `unaryTritOperator`: The operator to apply.
>
>
>**Returns:**
>A new TernaryArray3 with the results of applying the operation to each trit.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator |**([Ternary3.TernaryArray9](#ternary3ternaryarray9) ternaries, [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) unaryTritOperator)

>Applies the unary operation to a TernaryArray9 instance.
>
>**Parameters:**
>- `ternaries`: The TernaryArray9 to operate on.
>- `unaryTritOperator`: The operator to apply.
>
>
>**Returns:**
>A new TernaryArray9 with the results of applying the operation to each trit.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator |**([Ternary3.TernaryArray27](#ternary3ternaryarray27) ternaries, [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) unaryTritOperator)

>Applies the unary operation to a TernaryArray27 instance.
>
>**Parameters:**
>- `ternaries`: The TernaryArray27 to operate on.
>- `unaryTritOperator`: The operator to apply.
>
>
>**Returns:**
>A new TernaryArray27 with the results of applying the operation to each trit.

#### [Ternary3.TernaryArray](#ternary3ternaryarray) **operator |**([Ternary3.TernaryArray](#ternary3ternaryarray) ternaries, [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) unaryTritOperator)

>Applies the unary operation to a TernaryArray instance.
>
>**Parameters:**
>- `ternaries`: The TernaryArray to operate on.
>- `unaryTritOperator`: The operator to apply.
>
>
>**Returns:**
>A new TernaryArray with the results of applying the operation to each trit.

#### [Ternary3.BigTernaryArray](#ternary3bigternaryarray) **operator |**([Ternary3.BigTernaryArray](#ternary3bigternaryarray) ternaries, [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) unaryTritOperator)

>Applies the unary operation to a TernaryArray instance.
>
>**Parameters:**
>- `ternaries`: The TernaryArray to operate on.
>- `unaryTritOperator`: The operator to apply.
>
>
>**Returns:**
>A new TernaryArray with the results of applying the operation to each trit.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator |**([Ternary3.Int3T](#ternary3int3t) trits, [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) unaryTritOperator)

>Applies the unary operation to an Int3T instance by converting it to a TernaryArray3 first.
>
>**Parameters:**
>- `trits`: The Int3T to operate on.
>- `unaryTritOperator`: The operator to apply.
>
>
>**Returns:**
>A new TernaryArray3 with the results of applying the operation.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator |**([Ternary3.Int9T](#ternary3int9t) trits, [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) unaryTritOperator)

>Applies the unary operation to an Int9T instance by converting it to a TernaryArray9 first.
>
>**Parameters:**
>- `trits`: The Int9T to operate on.
>- `unaryTritOperator`: The operator to apply.
>
>
>**Returns:**
>A new TernaryArray9 with the results of applying the operation.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator |**([Ternary3.Int27T](#ternary3int27t) trits, [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) unaryTritOperator)

>Applies the unary operation to an Int27T instance by converting it to a TernaryArray27 first.
>
>**Parameters:**
>- `trits`: The Int27T to operate on.
>- `unaryTritOperator`: The operator to apply.
>
>
>**Returns:**
>A new TernaryArray27 with the results of applying the operation.

#### [Ternary3.TernaryArray3](#ternary3ternaryarray3) **operator |**([System.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte) trits, [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) unaryTritOperator)

>Applies the unary operation to an sbyte by converting it to a TernaryArray3 first.
>
>**Parameters:**
>- `trits`: The sbyte to operate on.
>- `unaryTritOperator`: The operator to apply.
>
>
>**Returns:**
>A new TernaryArray3 with the results of applying the operation.

#### [Ternary3.TernaryArray9](#ternary3ternaryarray9) **operator |**([System.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16) trits, [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) unaryTritOperator)

>Applies the unary operation to a short by converting it to a TernaryArray9 first.
>
>**Parameters:**
>- `trits`: The short to operate on.
>- `unaryTritOperator`: The operator to apply.
>
>
>**Returns:**
>A new TernaryArray9 with the results of applying the operation.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator |**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) trits, [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) unaryTritOperator)

>Applies the unary operation to an int by converting it to a TernaryArray27 first.
>
>**Parameters:**
>- `trits`: The int to operate on.
>- `unaryTritOperator`: The operator to apply.
>
>
>**Returns:**
>A new TernaryArray27 with the results of applying the operation.

#### [Ternary3.TernaryArray27](#ternary3ternaryarray27) **operator |**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) trits, [Ternary3.Operators.UnaryTritOperator](#ternary3operatorsunarytritoperator) unaryTritOperator)

>Applies the unary operation to a long by converting it to a TernaryArray27 first.
>
>**Parameters:**
>- `trits`: The long to operate on.
>- `unaryTritOperator`: The operator to apply.
>
>
>**Returns:**
>A new TernaryArray27 with the results of applying the operation.


## Ternary3.IO.ByteToInt3TStream

>A stream that converts a byte stream to an Int3T stream.
>
>**Remarks:**
>
>This stream reads bytes from the underlying byte stream and converts them to Int3T values
>            using the BinaryTritEncoder. The stream is not thread-safe for concurrent read/write operations.
>            External synchronization is required if accessed from multiple threads.

### Constructors

#### **ByteToInt3TStream**([System.IO.Stream](https://learn.microsoft.com/en-us/dotnet/api/system.io.stream) source, [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) mustWriteMagicNumber, [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) leaveOpen)

>A stream that converts a byte stream to an Int3T stream.
>
>**Remarks:**
>
>This stream reads bytes from the underlying byte stream and converts them to Int3T values
>            using the BinaryTritEncoder. The stream is not thread-safe for concurrent read/write operations.
>            External synchronization is required if accessed from multiple threads.


### Methods

#### [System.Threading.Tasks.Task<System.Int32>](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1) **ReadAsync**([Ternary3.Int3T[]](#ternary3int3t) buffer, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) offset, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) count, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Asynchronously reads a sequence of Int3T values from the stream.
>
>**Parameters:**
>- `buffer`: The buffer to write the data into.
>- `offset`: The offset in the buffer at which to begin writing data.
>- `count`: The maximum number of Int3T values to read.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous read operation. The value is
>            the total number of Int3T values read into the buffer.
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: buffer is null.
>- `System.ArgumentOutOfRangeException`: offset or count is negative.
>- `System.NotSupportedException`: The stream does not support reading.
>- `System.ObjectDisposedException`: The stream has been disposed.

#### [System.Threading.Tasks.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) **WriteAsync**([Ternary3.Int3T[]](#ternary3int3t) buffer, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) offset, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) count, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Asynchronously writes a sequence of Int3T values to the stream.
>
>**Parameters:**
>- `buffer`: The buffer containing the data to write.
>- `offset`: The offset in the buffer from which to begin writing data.
>- `count`: The number of Int3T values to write.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous write operation.
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: buffer is null.
>- `System.ArgumentOutOfRangeException`: offset or count is negative.
>- `System.NotSupportedException`: The stream does not support writing.
>- `System.ObjectDisposedException`: The stream has been disposed.

#### [System.Threading.Tasks.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) **FlushAsync**([System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Flushes the stream, writing any buffered data to the underlying stream.
>Warning: flushing will restart part of the encoding, making size optimization impossible.
>
>**Parameters:**
>- `cancellationToken`: The cancellation token
>
>
>**Exceptions:**
>- `System.NotSupportedException`: 
>- `System.ObjectDisposedException`: The stream has been disposed.

#### [System.Threading.Tasks.Task<System.Int64>](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1) **SeekAsync**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) offset, [System.IO.SeekOrigin](https://learn.microsoft.com/en-us/dotnet/api/system.io.seekorigin) origin, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>When overridden in a derived class, sets the position within the current stream.
>
>**Parameters:**
>- `offset`: A Int3T offset relative to the origin parameter.
>- `origin`: A value of type SeekOrigin indicating the reference point used to obtain the new position.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>The new position within the current stream.
>
>
>**Exceptions:**
>- `System.IO.IOException`: An I/O error occurs.
>- `System.NotSupportedException`: The stream does not support seeking, such as if the stream is constructed from a pipe or console output.
>- `System.ObjectDisposedException`: Methods were called after the stream was closed.
>- `System.OperationCanceledException`: The operation was canceled.

#### [System.Threading.Tasks.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) **SetLengthAsync**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) value, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>When overridden in a derived class, sets the length of the current stream.
>
>**Parameters:**
>- `value`: The desired length of the current stream in Int3T values.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Exceptions:**
>- `System.IO.IOException`: An I/O error occurs.
>- `System.NotSupportedException`: The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output.
>- `System.ObjectDisposedException`: Methods were called after the stream was closed.
>- `System.OperationCanceledException`: The operation was canceled.

#### [System.Threading.Tasks.ValueTask](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.valuetask) **DisposeAsyncCore**()

>Releases the unmanaged resources used by the stream and optionally releases the managed resources.
>
>**Returns:**
>A task that represents the asynchronous dispose operation.


### Properties

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **CanRead** { get; }

>When overridden in a derived class, gets a value indicating whether the current stream supports reading.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **CanWrite** { get; }

>When overridden in a derived class, gets a value indicating whether the current stream supports writing.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **CanSeek** { get; }

>When overridden in a derived class, gets a value indicating whether the current stream supports seeking.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **Length** { get; }

>When overridden in a derived class, gets the length in Int3T units of the stream.
>
>**Exceptions:**
>- `System.NotSupportedException`: A class derived from Int3TStream does not support seeking.
>- `System.ObjectDisposedException`: Methods were called after the stream was closed.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **Position** { get; set; }

>When overridden in a derived class, gets or sets the position within the current stream.
>
>**Exceptions:**
>- `System.IO.IOException`: An I/O error occurs.
>- `System.NotSupportedException`: The stream does not support seeking.
>- `System.ObjectDisposedException`: Methods were called after the stream was closed.


## Ternary3.IO.Int3TStream

>Provides an abstract base class for Int3T streams.
>
>**Remarks:**
>
>Int3TStream is an abstract base class that represents a sequence of Int3T values (trybbles).
>            It is the ternary equivalent of the binary System.IO.Stream that works with bytes.

### Constructors

#### **Int3TStream**()

No documentation available.


### Methods

#### [System.Threading.Tasks.Task<System.Int32>](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1) **ReadAsync**([Ternary3.Int3T[]](#ternary3int3t) buffer, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) offset, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) count, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>When overridden in a derived class, reads a sequence of Int3T values from the current stream and advances the position within the stream by the number of Int3T values read.
>
>**Parameters:**
>- `buffer`: An array of Int3T values. When this method returns, the buffer contains the specified Int3T array with the values between offset and (offset + count - 1) replaced by the Int3T values read from the current source.
>- `offset`: The zero-based Int3T offset in buffer at which to begin storing the data read from the current stream.
>- `count`: The maximum number of Int3T values to be read from the current stream.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>The total number of Int3T values read into the buffer. This can be less than the number of Int3T values requested if that many Int3T values are not currently available, or zero (0) if the end of the stream has been reached.
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: buffer is null.
>- `System.ArgumentOutOfRangeException`: offset or count is negative.
>- `System.ArgumentException`: The sum of offset and count is larger than the buffer length.
>- `System.NotSupportedException`: The stream does not support reading.
>- `System.ObjectDisposedException`: Methods were called after the stream was closed.
>- `System.IO.IOException`: An I/O error occurs.
>- `System.OperationCanceledException`: The operation was canceled.

#### [System.Threading.Tasks.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) **WriteAsync**([Ternary3.Int3T[]](#ternary3int3t) buffer, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) offset, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) count, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>When overridden in a derived class, writes a sequence of Int3T values to the current stream and advances the current position within this stream by the number of Int3T values written.
>
>**Parameters:**
>- `buffer`: An array of Int3T values. This method copies count Int3T values from buffer to the current stream.
>- `offset`: The zero-based Int3T offset in buffer at which to begin copying Int3T values to the current stream.
>- `count`: The number of Int3T values to be written to the current stream.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: buffer is null.
>- `System.ArgumentOutOfRangeException`: offset or count is negative.
>- `System.ArgumentException`: The sum of offset and count is larger than the buffer length.
>- `System.NotSupportedException`: The stream does not support writing.
>- `System.ObjectDisposedException`: Methods were called after the stream was closed.
>- `System.IO.IOException`: An I/O error occurs.
>- `System.OperationCanceledException`: The operation was canceled.

#### [System.Threading.Tasks.Task<System.Int64>](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1) **SeekAsync**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) offset, [System.IO.SeekOrigin](https://learn.microsoft.com/en-us/dotnet/api/system.io.seekorigin) origin, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>When overridden in a derived class, sets the position within the current stream.
>
>**Parameters:**
>- `offset`: A Int3T offset relative to the origin parameter.
>- `origin`: A value of type SeekOrigin indicating the reference point used to obtain the new position.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>The new position within the current stream.
>
>
>**Exceptions:**
>- `System.IO.IOException`: An I/O error occurs.
>- `System.NotSupportedException`: The stream does not support seeking, such as if the stream is constructed from a pipe or console output.
>- `System.ObjectDisposedException`: Methods were called after the stream was closed.
>- `System.OperationCanceledException`: The operation was canceled.

#### [System.Threading.Tasks.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) **SetLengthAsync**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) value, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>When overridden in a derived class, sets the length of the current stream.
>
>**Parameters:**
>- `value`: The desired length of the current stream in Int3T values.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Exceptions:**
>- `System.IO.IOException`: An I/O error occurs.
>- `System.NotSupportedException`: The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output.
>- `System.ObjectDisposedException`: Methods were called after the stream was closed.
>- `System.OperationCanceledException`: The operation was canceled.

#### [System.Threading.Tasks.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) **FlushAsync**([System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
>
>**Exceptions:**
>- `System.IO.IOException`: An I/O error occurs.

#### [System.Threading.Tasks.Task<System.Int32>](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1) **ReadInt3TAsync**([System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Asynchronously reads a Int3T value from the stream and advances the position within the stream by one Int3T value, or returns -1 if at the end of the stream.
>
>**Parameters:**
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous read operation. The value of the TResult parameter contains the unsigned Int3T value cast to an Int32, or -1 if at the end of the stream.
>
>
>**Exceptions:**
>- `System.NotSupportedException`: The stream does not support reading.
>- `System.ObjectDisposedException`: Methods were called after the stream was closed.

#### [System.Threading.Tasks.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) **WriteInt3TAsync**([Ternary3.Int3T](#ternary3int3t) value, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Asynchronously writes a Int3T value to the current position in the stream and advances the position within the stream by one Int3T value.
>
>**Parameters:**
>- `value`: The Int3T value to write to the stream.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous write operation.
>
>
>**Exceptions:**
>- `System.NotSupportedException`: The stream does not support writing.
>- `System.ObjectDisposedException`: Methods were called after the stream was closed.

#### [System.Threading.Tasks.Task<System.Int32>](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1) **ReadAsync**([Ternary3.Int3T[]](#ternary3int3t) buffer, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Asynchronously reads a sequence of Int3T values from the current stream and writes them to the buffer.
>
>**Parameters:**
>- `buffer`: The buffer to write the data into.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous read operation. The value of the TResult parameter contains the total number of Int3T values read into the buffer.
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: buffer is null.
>- `System.NotSupportedException`: The stream does not support reading.
>- `System.ObjectDisposedException`: Methods were called after the stream was closed.

#### [System.Threading.Tasks.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) **WriteAsync**([Ternary3.Int3T[]](#ternary3int3t) buffer, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Asynchronously writes a sequence of Int3T values from the buffer to the current stream.
>
>**Parameters:**
>- `buffer`: The buffer to read the data from.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous write operation.
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: buffer is null.
>- `System.NotSupportedException`: The stream does not support writing.
>- `System.ObjectDisposedException`: Methods were called after the stream was closed.

#### [System.Threading.Tasks.Task<Ternary3.Int3T[]>](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1) **ReadToEndAsync**([System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Asynchronously reads all Int3T values from the current position to the end of the stream.
>
>**Parameters:**
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous read operation. The value of the TResult parameter contains a Int3T array with the data read from the stream.
>
>
>**Exceptions:**
>- `System.NotSupportedException`: The stream does not support reading.
>- `System.ObjectDisposedException`: Methods were called after the stream was closed.
>- `System.OutOfMemoryException`: There is insufficient memory to allocate a buffer for the returned array.
>- `System.IO.IOException`: An I/O error occurs.

#### [System.Void](https://learn.microsoft.com/en-us/dotnet/api/system.void) **Flush**()

>When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
>
>**Exceptions:**
>- `System.IO.IOException`: An I/O error occurs.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **Read**([Ternary3.Int3T[]](#ternary3int3t) buffer, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) offset, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) count)

>Reads a sequence of Int3T values from the current stream and advances the position within the stream by the number of Int3T values read.
>
>**Parameters:**
>- `buffer`: An array of Int3T values.
>- `offset`: The zero-based Int3T offset in buffer at which to begin storing the data read from the current stream.
>- `count`: The maximum number of Int3T values to be read from the current stream.
>
>
>**Returns:**
>The total number of Int3T values read into the buffer.
>
>
>**Exceptions:**
>- `System.IO.IOException`: An I/O error occurs.
>
>
>**Remarks:**
>
>This is a synchronous method. Consider using [CancellationToken)](#m:ternary3.io.int3tstream.readasync(ternary3.int3t[],system.int32,system.int32,system.threading.cancellationtoken)) instead for better performance.

#### [System.Void](https://learn.microsoft.com/en-us/dotnet/api/system.void) **Write**([Ternary3.Int3T[]](#ternary3int3t) buffer, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) offset, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) count)

>Writes a sequence of Int3T values to the current stream and advances the current position within this stream by the number of Int3T values written.
>
>**Parameters:**
>- `buffer`: An array of Int3T values.
>- `offset`: The zero-based Int3T offset in buffer at which to begin copying Int3T values to the current stream.
>- `count`: The number of Int3T values to be written to the current stream.
>
>
>**Exceptions:**
>- `System.IO.IOException`: An I/O error occurs.
>
>
>**Remarks:**
>
>This is a synchronous method. Consider using [CancellationToken)](#m:ternary3.io.int3tstream.writeasync(ternary3.int3t[],system.int32,system.int32,system.threading.cancellationtoken)) instead for better performance.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **Seek**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) offset, [System.IO.SeekOrigin](https://learn.microsoft.com/en-us/dotnet/api/system.io.seekorigin) origin)

>Sets the position within the current stream.
>
>**Parameters:**
>- `offset`: A Int3T offset relative to the origin parameter.
>- `origin`: A value of type SeekOrigin indicating the reference point used to obtain the new position.
>
>
>**Returns:**
>The new position within the current stream.
>
>
>**Exceptions:**
>- `System.IO.IOException`: An I/O error occurs.
>
>
>**Remarks:**
>
>This is a synchronous method. Consider using [CancellationToken)](#m:ternary3.io.int3tstream.seekasync(system.int64,system.io.seekorigin,system.threading.cancellationtoken)) instead for better performance.

#### [System.Threading.Tasks.ValueTask](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.valuetask) **DisposeAsync**()

>Asynchronously disposes the stream, releasing all resources used by it.
>
>**Returns:**
>A task representing the asynchronous dispose operation.

#### [System.Threading.Tasks.ValueTask](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.valuetask) **DisposeAsyncCore**()

>Releases the unmanaged resources used by the Int3TStream and optionally releases the managed resources.
>
>**Returns:**
>A task that represents the asynchronous dispose operation.

#### [System.Threading.Tasks.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) **CopyToAsync**([Ternary3.IO.Int3TStream](#ternary3ioint3tstream) destination, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) count, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Asynchronously copies a specific number of Int3T values from the current Int3TStream to another Int3TStream.
>
>**Parameters:**
>- `destination`: The Int3TStream to which the contents of the current stream will be copied.
>- `count`: The number of Int3T values to copy.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous copy operation.
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: destination is null.
>- `System.ArgumentOutOfRangeException`: count is negative.
>- `System.NotSupportedException`: The current stream does not support reading, or the destination stream does not support writing.
>- `System.ObjectDisposedException`: Either the current stream or the destination stream is disposed.
>- `System.IO.IOException`: An I/O error occurred.

#### [System.Threading.Tasks.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) **CopyToAsync**([Ternary3.IO.Int3TStream](#ternary3ioint3tstream) destination, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Asynchronously copies all Int3T values from the current Int3TStream to another Int3TStream.
>
>**Parameters:**
>- `destination`: The Int3TStream to which the contents of the current stream will be copied.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous copy operation.
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: destination is null.
>- `System.NotSupportedException`: The current stream does not support reading, or the destination stream does not support writing.
>- `System.ObjectDisposedException`: Either the current stream or the destination stream is disposed.
>- `System.IO.IOException`: An I/O error occurred.


### Properties

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **Disposed** { get; }

>Gets a value indicating whether the current stream has been disposed.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **CanRead** { get; }

>When overridden in a derived class, gets a value indicating whether the current stream supports reading.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **CanWrite** { get; }

>When overridden in a derived class, gets a value indicating whether the current stream supports writing.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **CanSeek** { get; }

>When overridden in a derived class, gets a value indicating whether the current stream supports seeking.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **Length** { get; }

>When overridden in a derived class, gets the length in Int3T units of the stream.
>
>**Exceptions:**
>- `System.NotSupportedException`: A class derived from Int3TStream does not support seeking.
>- `System.ObjectDisposedException`: Methods were called after the stream was closed.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **Position** { get; set; }

>When overridden in a derived class, gets or sets the position within the current stream.
>
>**Exceptions:**
>- `System.IO.IOException`: An I/O error occurs.
>- `System.NotSupportedException`: The stream does not support seeking.
>- `System.ObjectDisposedException`: Methods were called after the stream was closed.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **CanTimeout** { get; }

>Gets a value that determines whether the current stream can timeout.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **ReadTimeout** { get; set; }

>Gets or sets a value, in milliseconds, that determines how long the stream will attempt to read before timing out.
>
>**Exceptions:**
>- `System.InvalidOperationException`: The stream does not support timeouts.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **WriteTimeout** { get; set; }

>Gets or sets a value, in milliseconds, that determines how long the stream will attempt to write before timing out.
>
>**Exceptions:**
>- `System.InvalidOperationException`: The stream does not support timeouts.


## Ternary3.IO.Int3TToByteStream

>A stream that converts an Int3T stream to a byte stream.
>
>**Remarks:**
>
>This stream reads Int3T values from the underlying Int3T stream and converts them to bytes
>            using the BinaryTritEncoder. The stream is not thread-safe for concurrent read/write operations.
>            External synchronization is required if accessed from multiple threads.

### Constructors

#### **Int3TToByteStream**([Ternary3.IO.Int3TStream](#ternary3ioint3tstream) source, [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) mustWriteMagicNumber, [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) leaveOpen)

>A stream that converts an Int3T stream to a byte stream.
>
>**Remarks:**
>
>This stream reads Int3T values from the underlying Int3T stream and converts them to bytes
>            using the BinaryTritEncoder. The stream is not thread-safe for concurrent read/write operations.
>            External synchronization is required if accessed from multiple threads.


### Methods

#### [System.Threading.Tasks.Task<System.Int32>](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1) **ReadAsync**([System.Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte[]) buffer, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) offset, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) count, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Asynchronously reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
>
>**Parameters:**
>- `buffer`: The buffer to write the data into.
>- `offset`: The byte offset in buffer at which to begin writing data read from the stream.
>- `count`: The maximum number of bytes to read.
>- `cancellationToken`: A token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous read operation. The value of the TResult parameter contains the total number of bytes read into the buffer.

#### [System.Threading.Tasks.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) **WriteAsync**([System.Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte[]) buffer, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) offset, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) count, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Asynchronously writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
>
>**Parameters:**
>- `buffer`: The buffer containing data to write to the stream.
>- `offset`: The zero-based byte offset in buffer at which to begin copying bytes to the stream.
>- `count`: The number of bytes to be written to the stream.
>- `cancellationToken`: A token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous write operation.

#### [System.Threading.Tasks.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) **FlushAsync**([System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>

#### [System.Void](https://learn.microsoft.com/en-us/dotnet/api/system.void) **Flush**()

>

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **Read**([System.Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte[]) buffer, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) offset, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) count)

>

#### [System.Void](https://learn.microsoft.com/en-us/dotnet/api/system.void) **Write**([System.Byte[]](https://learn.microsoft.com/en-us/dotnet/api/system.byte[]) buffer, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) offset, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) count)

>

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **Seek**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) offset, [System.IO.SeekOrigin](https://learn.microsoft.com/en-us/dotnet/api/system.io.seekorigin) origin)

>

#### [System.Void](https://learn.microsoft.com/en-us/dotnet/api/system.void) **SetLength**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) value)

>

#### [System.Void](https://learn.microsoft.com/en-us/dotnet/api/system.void) **Finalize**()

>Finalizer to ensure resources are cleaned up if Dispose is not called.

#### [System.Void](https://learn.microsoft.com/en-us/dotnet/api/system.void) **Dispose**([System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) disposing)

>Releases the resources used by the stream.
>
>**Parameters:**
>- `disposing`: true to release both managed and unmanaged resources; false to release only unmanaged resources.

#### [System.Threading.Tasks.ValueTask](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.valuetask) **DisposeAsync**()

>Asynchronously releases all resources used by the stream.
>
>**Returns:**
>A task that represents the asynchronous dispose operation.


### Properties

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **CanRead** { get; }

>

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **CanWrite** { get; }

>

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **CanSeek** { get; }

>

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **Length** { get; }

>

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **Position** { get; set; }

>


## Ternary3.IO.MemoryInt3TStream

>Creates a stream whose backing store is memory. This implementation uses Int3T as the basic unit instead of bytes.

### Constructors

#### **MemoryInt3TStream**()

>Initializes a new instance of the MemoryInt3TStream class with an expandable capacity initialized to zero.

#### **MemoryInt3TStream**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) capacity)

>Initializes a new instance of the MemoryInt3TStream class with an expandable capacity initialized to the specified value.
>
>**Parameters:**
>- `capacity`: The initial capacity of the stream.
>
>
>**Exceptions:**
>- `System.ArgumentOutOfRangeException`: capacity is negative.

#### **MemoryInt3TStream**([Ternary3.Int3T[]](#ternary3int3t) buffer)

>Initializes a new instance of the MemoryInt3TStream class with the specified Int3T array as a backing store.
>
>**Parameters:**
>- `buffer`: The array of Int3T values used as the backing store.
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: buffer is null.

#### **MemoryInt3TStream**([Ternary3.Int3T[]](#ternary3int3t) buffer, [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) writable)

>Initializes a new instance of the MemoryInt3TStream class with the specified Int3T array as a backing store and a Boolean value indicating whether the stream can be written to.
>
>**Parameters:**
>- `buffer`: The array of Int3T values used as the backing store.
>- `writable`: A Boolean value indicating whether the stream supports writing.
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: buffer is null.

#### **MemoryInt3TStream**([Ternary3.Int3T[]](#ternary3int3t) buffer, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) index, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) count)

>Initializes a new instance of the MemoryInt3TStream class with the specified Int3T array segment as a backing store.
>
>**Parameters:**
>- `buffer`: The array of Int3T values used as the backing store.
>- `index`: The index into buffer at which the stream begins.
>- `count`: The length in Int3T values of the backing store array.
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: buffer is null.
>- `System.ArgumentOutOfRangeException`: index or count is negative.
>- `System.ArgumentException`: The buffer length minus index is less than count.

#### **MemoryInt3TStream**([Ternary3.Int3T[]](#ternary3int3t) buffer, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) index, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) count, [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) writable)

>Initializes a new instance of the MemoryInt3TStream class with the specified Int3T array segment as a backing store and a Boolean value indicating whether the stream can be written to.
>
>**Parameters:**
>- `buffer`: The array of Int3T values used as the backing store.
>- `index`: The index into buffer at which the stream begins.
>- `count`: The length in Int3T values of the backing store array.
>- `writable`: A Boolean value indicating whether the stream supports writing.
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: buffer is null.
>- `System.ArgumentOutOfRangeException`: index or count is negative.
>- `System.ArgumentException`: The buffer length minus index is less than count.


### Methods

#### [Ternary3.Int3T[]](#ternary3int3t) **GetBuffer**()

>Gets the array of Int3T values from which this stream was created.
>
>**Returns:**
>The Int3T array from which this stream was created, or null if this stream was not created from an array.
>
>
>**Exceptions:**
>- `System.UnauthorizedAccessException`: The stream was not created with a publicly visible buffer.
>- `System.ObjectDisposedException`: The stream is closed.

#### [System.Void](https://learn.microsoft.com/en-us/dotnet/api/system.void) **GetBuffer**([Ternary3.Int3T[]&](#ternary3int3t) buffer, [System.Int32&](https://learn.microsoft.com/en-us/dotnet/api/system.int32&) index, [System.Int32&](https://learn.microsoft.com/en-us/dotnet/api/system.int32&) count)

>Gets the array of Int3T values from which this stream was created.
>
>**Returns:**
>The Int3T array from which this stream was created, or null if this stream was not created from an array.
>
>
>**Exceptions:**
>- `System.UnauthorizedAccessException`: The stream was not created with a publicly visible buffer.
>- `System.ObjectDisposedException`: The stream is closed.

#### [Ternary3.Int3T[]](#ternary3int3t) **ToArray**()

>Creates a new array and copies all the data from the MemoryInt3TStream into it.
>
>**Returns:**
>A new Int3T array containing a copy of the MemoryInt3TStream data.
>
>
>**Exceptions:**
>- `System.ObjectDisposedException`: The stream is closed.

#### [System.Threading.Tasks.Task<System.Int32>](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1) **ReadAsync**([Ternary3.Int3T[]](#ternary3int3t) buffer, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) offset, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) count, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Overrides the abstract ReadAsync method to read from the memory buffer.
>
>**Parameters:**
>- `buffer`: The buffer to read data into.
>- `offset`: The offset in the buffer at which to begin writing.
>- `count`: The maximum number of Int3T values to read.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous read operation. The value contains the number of Int3T values read.
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: buffer is null.
>- `System.ArgumentOutOfRangeException`: offset or count is negative.
>- `System.ArgumentException`: The sum of offset and count is larger than the buffer length.
>- `System.ObjectDisposedException`: The stream is closed.

#### [System.Threading.Tasks.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) **WriteAsync**([Ternary3.Int3T[]](#ternary3int3t) buffer, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) offset, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) count, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Overrides the abstract WriteAsync method to write to the memory buffer.
>
>**Parameters:**
>- `buffer`: The buffer to write data from.
>- `offset`: The offset in the buffer at which to begin reading.
>- `count`: The maximum number of Int3T values to write.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous write operation.
>
>
>**Exceptions:**
>- `System.ArgumentNullException`: buffer is null.
>- `System.ArgumentOutOfRangeException`: offset or count is negative.
>- `System.ArgumentException`: The sum of offset and count is larger than the buffer length.
>- `System.NotSupportedException`: The stream does not support writing.
>- `System.ObjectDisposedException`: The stream is closed.

#### [System.Threading.Tasks.Task<System.Int64>](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1) **SeekAsync**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) offset, [System.IO.SeekOrigin](https://learn.microsoft.com/en-us/dotnet/api/system.io.seekorigin) origin, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Overrides the abstract SeekAsync method to set the position within the memory buffer.
>
>**Parameters:**
>- `offset`: A Int3T offset relative to the origin parameter.
>- `origin`: A value of type SeekOrigin indicating the reference point used to obtain the new position.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous seek operation. The value contains the new position within the stream.
>
>
>**Exceptions:**
>- `System.ArgumentOutOfRangeException`: offset is greater than [MaxValue](#f:system.int32.maxvalue) .
>- `System.ObjectDisposedException`: The stream is closed.
>- `System.IO.IOException`: An I/O error occurs.

#### [System.Threading.Tasks.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) **SetLengthAsync**([System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) value, [System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Overrides the abstract SetLengthAsync method to set the length of the memory buffer.
>
>**Parameters:**
>- `value`: The desired length of the stream in Int3T values.
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous set length operation.
>
>
>**Exceptions:**
>- `System.ArgumentOutOfRangeException`: value is negative or greater than [MaxValue](#f:system.int32.maxvalue) .
>- `System.NotSupportedException`: The stream does not support both writing and seeking.
>- `System.ObjectDisposedException`: The stream is closed.

#### [System.Threading.Tasks.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task) **FlushAsync**([System.Threading.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) cancellationToken)

>Overrides the abstract FlushAsync method. For MemoryInt3TStream, this is a no-op since there is no buffer to flush.
>
>**Parameters:**
>- `cancellationToken`: The token to monitor for cancellation requests.
>
>
>**Returns:**
>A task that represents the asynchronous flush operation.

#### [System.Void](https://learn.microsoft.com/en-us/dotnet/api/system.void) **SetCapacity**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) value)

>Sets the capacity of the memory buffer underlying the MemoryInt3TStream to the specified value.
>
>**Parameters:**
>- `value`: The new capacity.
>
>
>**Exceptions:**
>- `System.ArgumentOutOfRangeException`: value is negative or less than the current length of the stream.
>- `System.NotSupportedException`: The stream is not expandable.
>- `System.ObjectDisposedException`: The stream is closed.

#### [System.Threading.Tasks.ValueTask](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.valuetask) **DisposeAsyncCore**()

>Releases the unmanaged resources used by the MemoryInt3TStream and optionally releases the managed resources.
>
>**Returns:**
>A task that represents the asynchronous dispose operation.


### Properties

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **CanRead** { get; }

>Gets a value indicating whether the current stream supports reading.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **CanWrite** { get; }

>Gets a value indicating whether the current stream supports writing.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **CanSeek** { get; }

>Gets a value indicating whether the current stream supports seeking.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **Length** { get; }

>Gets the length in Int3T units of the stream.
>
>**Exceptions:**
>- `System.ObjectDisposedException`: The stream is closed.

#### [System.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64) **Position** { get; set; }

>Gets or sets the position within the current stream.
>
>**Exceptions:**
>- `System.ArgumentOutOfRangeException`: The position is set to a negative value or a value greater than [MaxValue](#f:system.int32.maxvalue) .
>- `System.ObjectDisposedException`: The stream is closed.

#### [System.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean) **IsClosed** { get; }

>Gets a Boolean value indicating whether the current stream has been closed or not.


## Ternary3.Formatting.InvariantTernaryFormat

>Provides a culture-invariant ternary format with standard digit symbols and grouping.

### Constructors

#### **InvariantTernaryFormat**()

No documentation available.


### Properties

#### [System.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char) **NegativeTritDigit** { get; }

>Gets the character used to represent a negative trit (-1).

#### [System.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char) **ZeroTritDigit** { get; }

>Gets the character used to represent a zero trit (0).

#### [System.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char) **PositiveTritDigit** { get; }

>Gets the character used to represent a positive trit (+1).

#### [System.Collections.Generic.IList<Ternary3.Formatting.TritGroupDefinition>](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1) **Groups** { get; }

>Gets the list of group definitions, each specifying a separator and group size for hierarchical grouping.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **DecimalSeparator** { get; }

>Gets the string used as a decimal separator (for future floating-point trit support).

#### [Ternary3.TernaryPadding](#ternary3ternarypadding) **TernaryPadding** { get; }

>Gets the padding mode for the formatted ternary string.


## Ternary3.Formatting.ITernaryFormat

>Provides formatting options for representing arrays of trits as strings, including digit symbols, grouping, separators, and padding.

### Properties

#### [System.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char) **NegativeTritDigit** { get; }

>Gets the character used to represent a negative trit (-1).

#### [System.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char) **ZeroTritDigit** { get; }

>Gets the character used to represent a zero trit (0).

#### [System.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char) **PositiveTritDigit** { get; }

>Gets the character used to represent a positive trit (+1).

#### [System.Collections.Generic.IList<Ternary3.Formatting.TritGroupDefinition>](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1) **Groups** { get; }

>Gets the list of group definitions, each specifying a separator and group size for hierarchical grouping.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **DecimalSeparator** { get; }

>Gets the string used as a decimal separator (for future floating-point trit support).

#### [Ternary3.TernaryPadding](#ternary3ternarypadding) **TernaryPadding** { get; }

>Gets the padding mode for the formatted ternary string.


## Ternary3.Formatting.TritGroupDefinition

>Defines a group for hierarchical trit formatting, specifying the separator and group size.
>
>**Parameters:**
>- `separator`: The separator string for this group level.
>- `size`: The number of items in this group.

### Constructors

#### **TritGroupDefinition**([System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) separator, [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) size)

>Defines a group for hierarchical trit formatting, specifying the separator and group size.
>
>**Parameters:**
>- `separator`: The separator string for this group level.
>- `size`: The number of items in this group.


### Properties

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **Separator** { get; set; }

>Gets or sets the separator string for this group level.

#### [System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) **Size** { get; set; }

>Gets or sets the number of items in this group.


## Ternary3.Formatting.ITernaryFormatter

>A custom formatter that knows how to format trits

## Ternary3.Formatting.MinimalTernaryFormat

>Provides a minimal format for ternary numbers using 't', '0', and '1' for negative, zero, and positive trits respectively.

### Constructors

#### **MinimalTernaryFormat**()

No documentation available.


### Properties

#### [System.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char) **NegativeTritDigit** { get; }

>Gets the character used to represent a negative trit (-1).

#### [System.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char) **ZeroTritDigit** { get; }

>Gets the character used to represent a zero trit (0).

#### [System.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char) **PositiveTritDigit** { get; }

>Gets the character used to represent a positive trit (+1).

#### [System.Collections.Generic.IList<Ternary3.Formatting.TritGroupDefinition>](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1) **Groups** { get; }

>Gets the list of group definitions, each specifying a separator and group size for hierarchical grouping.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **DecimalSeparator** { get; }

>Gets the string used as a decimal separator (for future floating-point trit support).

#### [Ternary3.TernaryPadding](#ternary3ternarypadding) **TernaryPadding** { get; }

>Gets the padding mode for the formatted ternary string.


## Ternary3.Formatting.TernaryFormat

>Represents a customizable ternary format, allowing you to specify digit symbols, grouping, separators, and padding for formatting trit arrays.
>
>**Parameters:**
>- `other`: The format to copy settings from.

### Static Properties

#### [Ternary3.Formatting.InvariantTernaryFormat](#ternary3formattinginvariantternaryformat) **Invariant** { get; }

>Gets a built-in, culture-invariant ternary format with standard digit symbols and grouping.

#### [Ternary3.Formatting.MinimalTernaryFormat](#ternary3formattingminimalternaryformat) **Minimal** { get; }

>Gets a built-in minimal ternary format for compact representations.

#### [Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) **Current** { get; set; }

>The currently used ternary format, when not explicitly specified.


### Constructors

#### **TernaryFormat**([Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) other)

>Represents a customizable ternary format, allowing you to specify digit symbols, grouping, separators, and padding for formatting trit arrays.
>
>**Parameters:**
>- `other`: The format to copy settings from.

#### **TernaryFormat**()

>Initializes a new instance of the [TernaryFormat](#ternary3.formatting.ternaryformat) class using the specified format as a template.


### Methods

#### [Ternary3.Formatting.TernaryFormat](#ternary3formattingternaryformat) **WithGroup**([System.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32) size, [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) separator)

>Adds a group definition to the format.
>
>**Parameters:**
>- `size`: The size of the group.
>- `separator`: The separator to use between groups at this level.
>
>
>**Returns:**
>The current [TernaryFormat](#ternary3.formatting.ternaryformat) instance for chaining.

#### [Ternary3.Formatting.TernaryFormat](#ternary3formattingternaryformat) **ClearGroups**()

>Removes all group definitions from the format.
>
>**Returns:**
>The current [TernaryFormat](#ternary3.formatting.ternaryformat) instance for chaining.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**()

>Previews the format by creating a sample TernaryArray27 and formatting it.
>
>**Returns:**
>A string representation of a sample formatted trit array.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **ToString**([Ternary3.TernaryArray27](#ternary3ternaryarray27) ternaries)

>Previews the format by creating a sample TernaryArray27 and formatting it.
>
>**Parameters:**
>- `ternaries`: The trit array to format.
>
>
>**Returns:**
>A string representation of a sample formatted trit array.


### Properties

#### [System.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char) **NegativeTritDigit** { get; set; }

>Gets the character used to represent a negative trit (-1).

#### [System.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char) **ZeroTritDigit** { get; set; }

>Gets the character used to represent a zero trit (0).

#### [System.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char) **PositiveTritDigit** { get; set; }

>Gets the character used to represent a positive trit (+1).

#### [System.Collections.Generic.IList<Ternary3.Formatting.TritGroupDefinition>](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ilist-1) **Groups** { get; set; }

>Gets the list of group definitions, each specifying a separator and group size for hierarchical grouping.

#### [System.String](https://learn.microsoft.com/en-us/dotnet/api/system.string) **DecimalSeparator** { get; set; }

>Gets the string used as a decimal separator (for future floating-point trit support).

#### [Ternary3.TernaryPadding](#ternary3ternarypadding) **TernaryPadding** { get; set; }

>Gets the padding mode for the formatted ternary string.


## Ternary3.Formatting.TernaryFormatProvider

>Provides formatting capabilities for ternary numbers by implementing IFormatProvider.
>
>**Parameters:**
>- `format`: The ternary format to use, or null to use the current format.
>- `inner`: The inner format provider for chaining, or null if none.

### Constructors

#### **TernaryFormatProvider**([Ternary3.Formatting.ITernaryFormat](#ternary3formattingiternaryformat) format, [System.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) inner)

>Provides formatting capabilities for ternary numbers by implementing IFormatProvider.
>
>**Parameters:**
>- `format`: The ternary format to use, or null to use the current format.
>- `inner`: The inner format provider for chaining, or null if none.


### Methods

#### [System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) **GetFormat**([System.Type](https://learn.microsoft.com/en-us/dotnet/api/system.type) formatType)

>Gets an object that provides formatting services for the specified type.
>
>**Parameters:**
>- `formatType`: The type of format object to return.
>
>
>**Returns:**
>A TernaryFormatter if the formatType is ICustomFormatter or ITernaryFormatter;
>            otherwise, delegates to the inner provider if present.


## Ternary3.Formatting.TritParseOptions

>Specifies options for parsing ternary number strings.

### Enum Values

- **None** = `0`
>No special parsing options.
- **AllowAbsenceOfDigits** = `2`
>Allows the input string to contain no digits. The result will be zero.
- **AllowWhitespace** = `4`
>Ignores whitespace characters in the input string.
- **AllowDashes** = `8`
>Ignores dash characters in the input string, unless they are defined as digits in the format.
- **AllowUnderscores** = `16`
>Ignores underscore characters in the input string, unless they are defined as digits in the format.
- **AllowGroupSerparators** = `32`
>Ignores group separator characters in the input string.
- **AllowInvalidCharacters** = `64`
>Ignores invalid characters in the input string. This also allows for whitespace, dashes, underscores.
- **AllowOverflow** = `128`
>If the parsed value exceeds the maximum number of trits, it will be truncated to fit the maximum size.
- **AllowDecimal** = `512`
>Allows a decimal separator in the input string, which will be treated as a decimal point for floating-point trit values.
>When parsing to an interger ternary value, the decimal separator will be ignored, and the value will be rounded down.
- **CaseInsensitive** = `256`
>Do not strictly enforce the case of characters in the input string. For example, 't' and 'T' will be treated as equivalent.
- **Default** = `958`
>Lax parsing options that allow for a wide range of input formats

