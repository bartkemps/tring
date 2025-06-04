# Tring - Balanced Ternary Number Library

Tring is a comprehensive C# library for working with balanced ternary numbers. Unlike traditional binary computing where digits can be 0 or 1, balanced ternary uses the digits -1, 0, and 1 (often denoted as -, 0, and +).

## What are Balanced Ternary Numbers?

Balanced ternary is a non-standard positional numeral system, using three as the base with the digits −1, 0, and 1 (often written as T, 0, and 1). 

For example, the decimal number 8 is represented as `1 0 -1 0` in balanced ternary:
```
(1 × 3³) + (0 × 3²) + (-1 × 3¹) + (0 × 3⁰) = 27 + 0 - 3 + 0 = 24
```

This representation offers certain mathematical advantages:
- The negative of any number is obtained by simply inverting each digit (replacing 1 with -1 and vice versa)
- Rounding to the nearest integer is achieved by truncation
- The system can represent negative numbers without a separate sign bit

## Mathematical Richness of Ternary Logic

In binary logic, there are 2² = 4 possible unary operations (operations on one input):
1. Always output 0
2. Pass the input unchanged
3. Negate the input (flip)
4. Always output 1

And 2⁴ = 16 possible binary operations (operations on two inputs), including familiar ones like AND, OR, XOR, etc.

In ternary logic, the possibilities expand dramatically:
- 3³ = 27 possible unary operations
- 3⁹ = 19,683 possible binary operations

This provides an incredibly rich set of logical operations that can express complex relationships more concisely than binary logic.

## Library Components

### Trit

The fundamental unit of the library is the `Trit`, representing the three possible values:
- `-1` (negative)
- `0` (zero)
- `1` (positive)

A `Trit` can also be thought of as a nullable boolean, where:
- `-1` represents `false`
- `0` represents `null`
- `1` represents `true`

```csharp
Trit a = 1;
Trit b = 0;
Trit c = -1;

// Negation
Trit negated = -a; // negated = -1
```

### Ternary Integer Types

The library provides several integer types optimized for normal calculations with balanced ternary numbers:

- `Int3T`: 3-trit integer (-13 to +13)
- `Int5T`: 5-trit integer (-121 to +121)
- `Int9T`: 9-trit integer (-9,841 to +9,841) 
- `Int10T`: 10-trit integer (-29,524 to +29,524)
- `Int20T`: 20-trit integer (~1.7 billion range)
- `Int27T`: 27-trit integer (full range of a signed 32-bit integer)
- `Int40T`: 40-trit integer (large range similar to Int64)

These types handle arithmetic operations as you would expect, with proper overflow handling and trit shifts specific to ternary numbers.

```csharp
// Creating and using Int27T values
Int27T a = 42;
Int27T b = -13;

// Arithmetic operations
Int27T sum = a + b;       // 29
Int27T product = a * b;   // -546
Int27T quotient = a / b;  // -3

// Trit shifting
Int27T shiftedLeft = a << 1;   // 126 (42 × 3)
Int27T shiftedRight = a >> 1;  // 14 (42 ÷ 3)
```

### Trit Arrays

For operations focused on the trit-level manipulations, the library provides TritArray types:

- `TritArray27`: 27 trits, matching the size of Int27T

These are optimized for trit-wise operations but can also perform arithmetic:

```csharp
// Create TritArray27 instances
TritArray27 array1 = TritArray27.FromInt32(42);
TritArray27 array2 = TritArray27.FromInt32(-13);

// Trit-wise operations
TritArray27 andResult = array1 & array2;  // Trit-wise AND
TritArray27 orResult = array1 | array2;   // Trit-wise OR
TritArray27 xorResult = array1 ^ array2;  // Trit-wise XOR

// Access individual trits
Trit fifthTrit = array1[4];
```

### Operators

The library includes implementations of various ternary operators:

#### Unary Operators

Examples of the 27 unary operators include:
- `Identity`: Returns the input unchanged
- `Negate`: Flips positive to negative and vice versa
- `AbsoluteValue`: Returns the absolute value of the input
- `IsZero`: Returns 1 if the input is 0, otherwise returns 0
- `IsNonZero`: Returns 1 if the input is not 0, otherwise returns 0
- `IsPositive`: Returns 1 if the input is 1, otherwise returns 0
- `IsNegative`: Returns 1 if the input is -1, otherwise returns 0

```csharp
Trit t = -1;
Trit abs = UnaryOperation.AbsoluteValue(t);  // abs = 1
Trit isNeg = UnaryOperation.IsNegative(t);   // isNeg = 1
```

#### Binary Operators

Examples of the many binary operators include:
- `And`: Similar to binary AND but expanded for three values
- `Or`: Similar to binary OR but expanded for three values
- `Xor`: Similar to binary XOR but expanded for three values
- `Consensus`: Returns the consensus value if there is one
- `Min`: Returns the minimum of two values
- `Max`: Returns the maximum of two values

```csharp
Trit a = 1;
Trit b = 0;
Trit andResult = a & b;       // andResult = 0
Trit orResult = a | b;        // orResult = 1
Trit xorResult = a ^ b;       // xorResult = 1
```

## Performance Optimizations

The library includes several optimizations to ensure efficient computation:

1. **Automatic Conversions**: Types convert between integer and trit array representations as needed for optimal performance.

2. **Lookup Tables**: Common operations use pre-computed lookup tables for speed.

3. **Bit-level Representation**: Balanced ternary values are efficiently encoded using pairs of bits.

4. **Specialized Algorithms**: Custom algorithms for addition, multiplication and other operations take advantage of balanced ternary properties.

## Examples

### Converting Between Number Systems

```csharp
// Convert from decimal to balanced ternary
Int27T ternaryValue = 42;
string ternaryString = ternaryValue.ToString(); // "1111-"

// Convert from balanced ternary string to an Int27T
Int27T parsedValue = Int27T.Parse("1111-"); // 42
```

### Complex Calculations

```csharp
// Calculate powers using ternary
Int27T base = 3;
Int27T power = base.Pow(4); // 81

// Remainder operations
Int27T a = 10;
Int27T b = 3;
Int27T remainder = a % b; // 1
```

### Ternary Logic Operations

```csharp
// Create values
Int27T x = 42;
Int27T y = -13;

// Perform ternary logical operations
TritArray27 xArray = x.ToTritArray();
TritArray27 yArray = y.ToTritArray();

// Find consensus value for each trit position
TritArray27 consensus = Operator.Consensus.Apply(xArray, yArray);

// Convert back to integer if needed
Int27T result = Int27T.FromTritArray(consensus);
```

## Conclusion

Tring provides a full-featured foundation for working with balanced ternary numbers in .NET. Whether you're exploring alternative computing models, working on specialized algorithms, or just curious about non-binary number systems, this library gives you the tools to work efficiently with balanced ternary values.

## License

[License information]
