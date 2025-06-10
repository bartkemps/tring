namespace Examples;

using Ternary3;
// The class Operators.Unary contains static methods for unary operations on Trit values.
using static Ternary3.Operators.UnaryTritOperator;
// Common alias for 27-trit integers
using Tryte = Ternary3.Int27T;

public static class UnaryDemo
{
    public static void Run()
    {
        Console.WriteLine($"\r\n\r\n{nameof(UnaryDemo)}");
        
        // Operation on Int3T outputs TritArray3
        Int3T input1 = 5; // 1TT
        var output1 = input1 | AbsoluteValue; // TritArray3 111
        Console.WriteLine($"Absolute value of {input1} ({(TritArray3)input1}) = {(Int3T)output1} ({output1})"); // Absolute value of 5 (1TT) = 13 (111)

        // Custom operation on Int9T outputs TritArray9, which can be implicitly converted to Int9T
        Trit Echo(Trit trit) => trit; // Custom operation: Identity function for Trit
        Int9T input2A = -10; // T0T
        Int9T output2A = input2A | Echo; // T0T. Gets implicitly converted to Int9T
        Console.WriteLine($"-10 Echoed = {output2A}"); // Prints -10
        
        // Custom operation on Int9T outputs TritArray9, which can be implicitly converted to Int9T
        var echo = new Trit[] { false, null, true };
        Int9T input2B = -10; // T0T
        Int9T output2B = input2B | echo; // T0T. Gets implicitly converted to Int9T
        Console.WriteLine($"-10 Echoed = {output2B}"); // Prints -10
        
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
        
        // integer input will be implicitly converted to a TritArray27.
        var input6 = -16; // T11T
        var output6 = input6 | Floor; // All T become 0. 0110;
        Console.WriteLine($"{input6} | {nameof(Floor)} becomes {output6}"); // -22 Floor becomes   000000000  000000000 000000110
    }
}