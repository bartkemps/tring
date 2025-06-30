> **Perhaps the prettiest number system of all is the balanced ternary notation**
>
> -- *Donald Knuth, The art of computer programming*

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

public partial class Program
{
    public static void Main()
    {
         WriteLine(terT010 | Or | ter11);
    }
}
```

This will output a ternary number representing the result of the operation.

## Ternary Literals

Ternary3 supports the use of ternary literals in your code, making it easy to work with constants in balanced ternary notation. 

> **Note:** Ternary literals only work in partial classes. The source generator will only generate ternary constants for partial classes. If you want to use ternary literals, ensure your class is declared with the `partial` keyword.

For example:

```csharp
partial class MyClass {
    void Example() {
        Int3T x = ter01T + terT10; // x will be -4
    }
}
```

Ternary literals are identifiers of the form `ter010T`, `terT01`, etc., where each character after `ter` is either `0`, `1`, or `T` (for -1). Like other C# literals, underscores are allowed between digits for clarity (e.g., `ter_01_T`). For example:

```csharp
// These are valid ternary literals:
ter01T   // equivalent to decimal 2
terT10   // equivalent to decimal -6
ter0001  // equivalent to decimal 1
ter000_000_01T // equivalent to decimal 2
```

### How it works

When you reference a ternary literal in your code, the Ternary3 source generator automatically generates a constant with the correct value for you. This means you can use ternary literals as if they were predefined constants:

```csharp
int x = ter01T + terT10; // x will be -4
```

### Enabling/Disabling Ternary Literal Generation

By default, ternary literal generation is enabled for all partial classes in your project. You can control this behavior using the `[assembly: Ternary3.GenerateTernaryConstants]` attribute:

- To enable for all classes (default):
  ```csharp
  [assembly: Ternary3.GenerateTernaryConstants]
  ```
- To disable for all classes:
  ```csharp
  [assembly: Ternary3.GenerateTernaryConstants(false)]
  ```
- To enable for a specific class:
  ```csharp
  [Ternary3.GenerateTernaryConstants]
  partial class MyClass { /* ... */ }
  ```
- To disable for a specific class:
  ```csharp
  [Ternary3.GenerateTernaryConstants(false)]
  partial class MyClass { /* ... */ }
  ```
- To disable for all classes but re-enable for a specific class:
  ```csharp
  [assembly: Ternary3.GenerateTernaryConstants(false)]
  [Ternary3.GenerateTernaryConstants]
  partial class MyClass { /* ... */ }
  ```

See the source generator documentation for more advanced usage.

## Core Classes and Types

### TritArray

`TritArray` is a key type in the library that represents an arbitrary-length balanced ternary number. It provides various operations for working with ternary numbers:

```csharp
// Create a TritArray with a specific length (all trits initialized to 0)
var arr = new TritArray(10);

// Get and set individual trits
arr[3] = Trit.Positive;
Trit value = arr[3]; // Gets the trit at index 3

// Use C# 8.0 Index syntax
arr[^1] = Trit.Negative; // Set the last trit to negative (-1)
var lastTrit = arr[^1];  // Get the last trit

// Use C# 8.0 Range syntax to get a slice
var slice = arr[0..5];   // Get the first 5 trits as a new TritArray

// Resize the array
arr.Resize(15);         // Resize to a larger array (preserving values)

// Implicit casting from ternary literals
TritArray arr2 = terT10_TTT_01T;  // Directly assign a ternary literal to TritArray
TritArray3 arr3 = ter10T;         // Fixed-size array from literal
TritArray9 arr4 = ter10T_101_T01; // 9-trit array from literal 
TritArray27 arr5 = ter10T_101_T01_11T_01T_T10_T11; // 27-trit array from literal
```

### Fixed-Size TritArrays

The library includes specialized fixed-size implementations for common ternary number sizes:

- `TritArray3` - A 3-trit number (equivalent range to a byte)
- `TritArray9` - A 9-trit number (equivalent range to a short)
- `TritArray27` - A 27-trit number (equivalent range to an int)

These offer better performance than the general-purpose `TritArray` when working with known sizes.

### LookupTritArrayOperator

`LookupTritArrayOperator` provides optimized implementations of ternary operations for the TritArray types, using lookup tables for high-performance computation.

```csharp
// Create a TritArray
var arr = new TritArray(10);

// Apply an operation using a LookupTritArrayOperator
var result = LookupTritArrayOperator.Negate(arr);
```

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

public static partial class UnaryTritOperationDemo
{
    public static void Run()
    {
        Console.WriteLine($"\r\n\r\n{nameof(UnaryTritOperationDemo)}");
        
        // Operation on Int3T outputs TritArray3
        Int3T input1 = ter1TT; // 5 in decimal
        var output1 = input1 | AbsoluteValue; // TritArray3 111
        Console.WriteLine($"Absolute value of {input1} ({(TritArray3)input1}) = {(Int3T)output1} ({output1})"); // Absolute value of 5 (1TT) = 13 (111)

        // Custom operation on Int9T outputs TritArray9, which can be implicitly converted to Int9T
        Trit EchoA(Trit trit) => trit; // Custom operation: Identity function for Trit
        Int9T input2A = terT0T; // -10 in decimal
        Int9T output2A = input2A | EchoA; // T0T. Gets implicitly converted to Int9T
        Console.WriteLine($"terT0T Echoed = {output2A}"); // Prints -10
        
        // Custom operation on Int9T outputs TritArray9, which can be implicitly converted to Int9T
        Trit[] echoB = [Trit.Negative, Trit.Zero, Trit.Positive];
        Int9T input2B = terT0T; // -10 in decimal
        Int9T output2B = input2B | echoB; // T0T. Gets implicitly converted to Int9T
        Console.WriteLine($"terT0T Echoed = {output2B}"); // Prints -10
        
        // Custom operation on Int9T outputs TritArray9, which can be implicitly converted to Int9T
        var echoC = new UnaryTritOperator(false, null, true);
        Int9T input2C = terT0T; // -10 in decimal
        Int9T output2C = input2C | echoC; // T0T. Gets implicitly converted to Int9T
        Console.WriteLine($"terT0T Echoed = {output2C}"); // Prints -10
        
        // Custom operation on Int9T outputs TritArray9, which can be implicitly converted to Int9T
        var echoD = new UnaryTritOperator(-1, 0, 1);
        Int9T input2D = terT0T; // -10 in decimal
        Int9T output2D = input2D | echoD; // T0T. Gets implicitly converted to Int9T
        Console.WriteLine($"terT0T Echoed = {output2D}"); // Prints -10
        
        // Using an alias for a Tryte (27-trit integer) and the full name for the operator
        Tryte input3 = ter11T; // 11 in decimal
        long output3 = input3 | Ternary3.Operators.UnaryTritOperator.Negate; // TT1, using the full name for the operator. Result implicitly converted to long.
        Console.WriteLine($"ter11T Negated = {output3}"); // ter11T Negated = -11
        
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

        // Addition and simple multiplication of TritArray3 also may overflow.
        // (Under the hood, these are often performed without conversion to binary)
        TritArray3 input3A = ter110; // 12 in decimal
        TritArray3 input3B = ter010; // 3 in decimal
        var result3 = input3A * input3B; // 110 * 010 = 11100 (36). TritArray3 only keeps 3 trits, so T00 = -9
        Console.WriteLine($"Overflow: {input3A} ({input3A:ter}) * {input3B} ({input3B:ter}) = {result3} ({(int)result3})");
        
        Int3T input4 = 25; // 10T1. trimmed to 3 trits = 0T1 or -2
        Console.WriteLine($"Overflow: 25 => {input4} ({input4:ter})"); // Overflow: 25 => -2 (0T1)

        TritArray3 input5 = 25; // 10T1. trimmed to 3 trits = 0T1 or -2
        Console.WriteLine($"Overflow: 25 => {input5} ({input5:ter})"); // Overflow: 25 => (0T1) 

        // Shifting trits two positions in essence multiplies or divides by 9 (3^2).
        var input6 = TritArray9.MaxValue; // 111111111
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
        Console.WriteLine($"Shift: {(TritArray27)input7} << 25 = {result7A} ({(TritArray27)result7A:ter})"); // Shift: TTTT..TT << 25 = TT00...00
        var result7B = input7 >> -25;
        Console.WriteLine($"Shift: {(TritArray27)input7} >> -25 = {result7B} ({(TritArray27)result7B:ter})"); // Shift: TTTT..TT >> -25 = TT00...00
        var result7C = input7 << -25;
        Console.WriteLine($"Shift: {(TritArray27)input7} << -25 = {result7C} ({(TritArray27)result7C:ter})"); // Shift: TTTT..TT << -25 = 0000...TT
        var result7D = input7 >> 25;
        Console.WriteLine($"Shift: {(TritArray27)input7} >> 25 = {result7D} ({(TritArray27)result7D:ter})"); // Shift: TTTT..TT >> 25 = 0000..TT
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

### Using TritArray Indexers

```csharp
// Create a TritArray
var array = new TritArray(10);

// Set values using the regular indexer
array[0] = Trit.Positive;
array[1] = Trit.Negative;

// Use C# 8.0 Index syntax for the end of the array
array[^1] = Trit.Positive;  // Sets the last trit
array[^2] = Trit.Negative;  // Sets the second-to-last trit

// Get a range of trits (creates a new TritArray)
var firstThree = array[0..3];  // Gets trits at positions 0, 1, and 2
var lastFour = array[^4..];    // Gets the last 4 trits
var middlePortion = array[2..^2]; // Gets all trits except the first 2 and last 2
```

### Ternary Formatting Example

You can easily print the ternary representation of numbers using the `:ter` format specifier:

```csharp
TritArray3 a = 7;
Int3T b = 7;
Console.WriteLine($"The ternary representation of {a} is {a:ter}");
Console.WriteLine($"The ternary representation of {b} is {b:ter}");
```
