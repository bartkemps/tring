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

In ternary logic, the possibilities expand dramatically:
- 3³ = 27 possible unary operations
- 3⁹ = 19,683 possible binary operations

### Ternary Logic Tables

#### Ternary Binary Operations

Here are a few examples of ternary binary operations:

**Ternary AND**:

| AND | **T** | **0** | **1** |
|-----|-------|-------|-------|
| **T** |   T   |   T   |   T   |
| **0** |   T   |   0   |   0   |
| **1** |   T   |   0   |   1   |

**Ternary OR**:

| OR | **T** | **0** | **1** |
|----|-------|-------|-------|
| **T** |   T   |   0   |   1   |
| **0** |   0   |   0   |   1   |
| **1** |   1   |   1   |   1   |

**Ternary XOR**:

| XOR | **T** | **0** | **1** |
|-----|-------|-------|-------|
| **T** |   1   |   T   |   0   |
| **0** |   T   |   0   |   1   |
| **1** |   0   |   1   |   T   |

**Ternary Consensus**:

| CONSENSUS | **T** | **0** | **1** |
|-----------|-------|-------|-------|
| **T**     |   T   |   0   |   0   |
| **0**     |   0   |   0   |   0   |
| **1**     |   0   |   0   |   1   |

#### All 27 Ternary Unary Operations

Here's a complete table of all 27 possible unary operations in ternary logic:

| Name | Input T | Input 0 | Input 1 | Description |
|------|---------|---------|---------|-------------|
| Negative | T | T | T | Always outputs T |
| Decrement | T | T | 0 | Decrements the input value |
| IsPositive | T | T | 1 | Returns 1 if input is positive, otherwise T |
| NegateAbsoluteValue | T | 0 | T | Negates the absolute value |
| Ceil | T | 0 | 0 | Returns T for negative inputs, 0 otherwise |
| Identity | T | 0 | 1 | Returns input unchanged |
| IsZero | T | 1 | T | Returns 1 if input is 0, otherwise T |
| KeepNegative | T | 1 | 0 | Returns T if input is negative, 0 otherwise |
| IsNotNegative | T | 1 | 1 | Returns 1 if input is not negative |
| CeilIsNegative | 0 | T | T | Returns 0 if input is negative, T otherwise |
| CeilIsNotZero | 0 | T | 0 | Returns 0 if input is not zero, T otherwise |
| KeepPositive | 0 | T | 1 | Returns input only if positive, 0 otherwise |
| CeilIsNotPositive | 0 | 0 | T | Returns 0 if input is not positive, T if positive |
| Zero | 0 | 0 | 0 | Always outputs 0 |
| Floor | 0 | 0 | 1 | Returns 0 for positive inputs, 1 otherwise |
| CyclicIncrement | 0 | 1 | T | Cycles through T → 0 → 1 → T |
| FloorIsZero | 0 | 1 | 0 | Returns 0 if input is zero, 1 otherwise |
| Increment | 0 | 1 | 1 | Increments the input value |
| IsNegative | 1 | T | T | Returns 1 if input is negative, T otherwise |
| CyclicDecrement | 1 | T | 0 | Cycles through 1 → 0 → T → 1 |
| IsNotZero | 1 | T | 1 | Returns 1 if input is not zero |
| Negate | 1 | 0 | T | Negates the input value |
| FloorIsNegative | 1 | 0 | 0 | Returns 1 if input is negative, 0 otherwise |
| AbsoluteValue | 1 | 0 | 1 | Returns absolute value of input |
| IsNotPositive | 1 | 1 | T | Returns 1 if input is not positive |
| FloorIsNotPositive | 1 | 1 | 0 | Returns 1 if input is not positive, 0 if positive |
| Positive | 1 | 1 | 1 | Always outputs 1 |

## Library Components

### Trit

The fundamental unit of the library is the `Trit`, representing the three possible values:
- `T` (negative, -1)
- `0` (zero)
- `1` (positive)

A `Trit` can also be thought of as a nullable boolean, where:
- `T` represents `false`
- `0` represents `null`
- `1` represents `true`

```csharp
Trit a = 1;
Trit b = 0;
Trit c = T; // Equivalent to writing -1
 
// Negation
Trit negated = -a; // negated = T
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
- `IsZero`: Returns 1 if the input is 0, otherwise returns T
- `IsNonZero`: Returns 1 if the input is not 0, otherwise returns T
- `IsPositive`: Returns 1 if the input is 1, otherwise returns T
- `IsNegative`: Returns 1 if the input is T, otherwise returns T

```csharp
Trit t = T;
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

## Using Tring in Your Code

Tring offers an elegant and intuitive API for working with balanced ternary values and operations. The library implements a notation that functions almost like a domain-specific language (DSL) for ternary logic - which is essential given the complexity of balanced ternary:

- **27 different unary operations** (compared to just 4 in binary logic)
- **19,683 possible binary operations** (compared to just 16 in binary logic)

This notation makes working with this vast space of operations practical and approachable.

### Notation Examples

Tring's pipe-based notation provides multiple ways to work with ternary operations, creating a clean, functional syntax that's both powerful and readable:

#### Unary Operations

```csharp
// Using a local function
Trit NotNegative(Trit t) => t != Trit.Negative;
var result1 = Trit.Positive | NotNegative; // Applies NotNegative to Trit.Positive

// Using built-in operators
// Import with: using static Tring.Operators.Operator;
var result2 = Trit.Positive | Operator.IsNotNegative;

// Using a lookup table directly
var result3 = Trit.Positive | [Trit.Negative, Trit.Positive, Trit.Positive];

// All three approaches produce the same result: Trit.Positive
```

#### Binary Operations

```csharp
// Using a local function with two parameters
Trit Or(Trit t1, Trit t2) => (t1, t2) switch
{
    (Trit.Negative, Trit.Negative) => Trit.Negative,
    (Trit.Positive, _) or (_, Trit.Positive) => Trit.Positive,
    _ => Trit.Zero
};
var result1 = Trit.Positive | Or | Trit.Negative;

// Using a lookup table
Trit[,] @or = new Trit[3, 3]
{
    { false, false, true }, 
    { false, false, true }, 
    { true, true, true } 
};
var result2 = Trit.Positive | @or | Trit.Negative;

// Using negation with the built-in IsEqualTo operator
var result3 = -(Trit.Positive | Operator.Or | Trit.Negative);

// All three approaches produce the same result: Trit.Positive
```

#### Operations on TritArray27

The same notation works elegantly with TritArray27, applying the operation to each trit position:

```csharp
// Create TritArray27 instances
var array1 = (TritArray27)(42);
var array2 = (TritArray27)(-13);

// Apply unary operations
var negatedArray = -array1; // Negate each trit
var incrementedArray = array1 | Operator.Increment; // Increment each trit

// Apply binary operations using the same pipe syntax
var andResult = array1 | Operator.And | array2; // Apply AND to each trit position
var xorResult = array1 | (Trit t1, Trit t2) => t1 ^ t2 | array2; // Using a lambda

// Apply custom operations using lookup tables
Trit[,] customOperation = new Trit[3, 3]
{
    { Trit.Negative, Trit.Zero, Trit.Positive },
    { Trit.Zero, Trit.Positive, Trit.Negative },
    { Trit.Positive, Trit.Negative, Trit.Zero }
};
var customResult = array1 | customOperation | array2;
```

#### Operations on Int27T

The same notation can be used with Int27T, but it's important to note that this is less efficient as it requires implicit conversions between Int27T and TritArray27 under the hood:

```csharp
// Create Int27T instances
Int27T int1 = 42;
Int27T int2 = -13;

// Apply unary operations (internally converts to TritArray27 and back)
Int27T negatedInt = int1 | Operator.Negate; // Much less efficient than -int1
Int27T absInt = int1 | Operator.AbsoluteValue; // Less efficient than int1.Abs()

// Apply binary operations (each step involves conversions)
Int27T andResult = int1 | Operator.And | int2; // Less efficient than standard operators
Int27T customResult = int1 | customOperation | int2; // Custom operation via lookup table

// For optimal performance with Int27T, use the standard operators instead:
Int27T efficientResult = int1 & int2; // More efficient than the pipe syntax for basic operations
```

The pipe operator (`|`) provides a clean and readable syntax for applying ternary operations, allowing for expressive code when working with balanced ternary logic. This approach transforms what would otherwise be an unwieldy set of function calls into a concise, fluent interface that resembles a specialized language for ternary computing.

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
string ternaryString = ternaryValue.ToString(); // "1111T"

// Convert from balanced ternary string to an Int27T
Int27T parsedValue = Int27T.Parse("1111T"); // 42
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
