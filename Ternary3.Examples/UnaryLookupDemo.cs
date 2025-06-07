namespace Ternary3.Examples;

using Numbers;

// The class Operators.UnaryLookup contains lookup-tables for unary operations on Trit values.
using static Operators.UnaryLookup;
// Uncommon alias for 3-trit integers
using Tribble = Numbers.Int3T;

public static class UnaryLookupDemo
{
    public static void Run()
    {
        // Operation on Int3T outputs TritArray3
        Tribble input1 = 5; // 1TT
        var output1 = input1 | AbsoluteValue; // TritArray3 111
        Console.WriteLine($"Absolute value of {input1} ({(TritArray3)input1}) = {(Tribble)output1} ({output1})"); // Absolute value of 5 (1TT) = 13 (111)

        // Custom operation on Int9T outputs TritArray9, which can be implicitly converted to Int9T
        var echo = new Trit[] { false, null, true };
        Int9T input2 = -10; // T0T
        Int9T output2 = input2 | echo; // T0T. Gets implicitly converted to Int9T
        Console.WriteLine($"-10 Echoed = {output2}"); // Prints -10
        
        // Using an alias for a Tryte (27-trit integer) and the full name for the operator
        Int27T input3 = 11; // 11T
        long output3 = input3 | Ternary3.Operators.UnaryLookup.Negate; // TT1, using the full name for the operator. Result implicitly converted to long.
        Console.WriteLine($"11 Negated = {output3}"); // 11 Negated = -11
        
        //TritArray input with TritArray27. Output is also TritArray27, // which can be explicitly converted to long.
        TritArray27 input4 = 123456789;
        TritArray27 output4 = input4 | Negate;
        Console.WriteLine($"123456789 Negated = {output4} ({(int)output4})"); // Prints 000000000 T0011TT01 T1T010100 (-123456789)
        
        //Single Trit input
        var input5 = Trit.Negative; // T (negative)
        var output5 = input5 | IsPositive; // No. It is not positive. No translates to T (negative)
        Console.WriteLine($"Is {input5} positive? {output5}"); // Is Negative positive? Negative
    }
}