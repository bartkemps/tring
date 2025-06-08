namespace Ternary3.Examples;

using Numbers;
using Operators;
using static Operators.BinaryLookup;

public class BinaryLookupDemo
{
    const int T = -1;

    public static void Run()
    {
        // existing operor AND on two sbytes
        // operators on sbytes cast them to TritArray3. Values outside the range of TritArray3 are truncated to fit.
        sbyte input1A = 8; // 10T
        sbyte input1B = 9; // 100
        TritArray3 result1 = input1A | And | input1B; // Operators.BinaryLookup.And returns the smallest of every trit
        Console.WriteLine($"BinaryLookup And: {input1A} & {input1B} = {(sbyte)result1} ({result1})"); // Output: 8 (10T)

        // custom operation on two shorts
        // All operations with two operands are defined by a TritLookupTable, which maps Trit values to Trit values.
        short input2A = -6; // 000000T10
        short input2B = 13; // 000000111
        TritLookupTable op = new([
            [T, 0, 0],
            [0, 0, 0],
            [0, 0, 1]
        ]);
        var result2 = input2A | op | input2B; // 000000010 (all trits that are the same)
        Console.Write($"Custom operation on short: {(TritArray9)input2A} op {(TritArray9)input2B} = {result2}");
    }
}