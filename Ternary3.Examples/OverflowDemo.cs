namespace Ternary3.Examples;

using Numbers;

public static class OverflowDemo
{
    public static void Run()
    {
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

        Int3T input3 = 25; // 10T1. trimmed to 3 trits = 0T1 or -8;
        Console.WriteLine($"Overflow: 25 => {input3} ({(TritArray3)input3})"); // Overflow: 25 => -2 (0T1)
        
        TritArray3 input4 = 25; // 10T1. trimmed to 3 trits = 0T1 or -8;
        Console.WriteLine($"Overflow: 25 => {input4} ({(int)input4})"); // Overflow: 25 => (0T1) (25)

        // Shifting trits two positions in essence multiplies or divides by 9 (3^2).
        TritArray9 input5 = Int9T.MaxValue; // 111111111
        var result5A = input5 << 6;
        Console.WriteLine($"Shift: {input5} << 6 = {result5A} ({(int)result5A})"); // Shift: 111111111 << 6 = 111000000 (9477)
        var result5B = input5 >> -6;
        Console.WriteLine($"Shift: {input5} >> -6 = {result5B} ({(int)result5B})"); // Shift: 111111111 >> -6 = 111000000 (9477)
        var result5C = input5 << -6;
        Console.WriteLine($"Shift: {input5} << -6 = {result5C} ({(int)result5C})"); // Shift: 111111111 << -6 = 000000111 (13)
        var result5D = input5 >> 6;
        Console.WriteLine($"Shift: {input5} >> 6 = {result5D} ({(int)result5D})"); // Shift: 111111111 >> 6 = 000000111 (13)
        
        Int27T input6 = Int27T.MinValue; // TTTTTTTTT TTTTTTTTT TTTTTTTTT
        var result6A = input6 << 25;
        Console.WriteLine($"Shift: {(TritArray27)input6} << 25 = {result6A} ({(TritArray27)result6A})"); // Shift: TTTT..TT << 25 = TT00...00 (-3389154437772)
        var result6B = input6 >> -25;
        Console.WriteLine($"Shift: {(TritArray27)input6} >> -25 = {result6B} ({(TritArray27)result6B})"); // Shift: TTTT..TT >> -25 = TT00...00 (-3389154437772)
        var result6C = input6 << -25;
        Console.WriteLine($"Shift: {(TritArray27)input6} << -25 = {result6C} ({(TritArray27)result6C})"); // Shift: TTTT..TT << -25 = 0000...TT (-4)
        var result6D = input6 >> 25;
        Console.WriteLine($"Shift: {(TritArray27)input6} >> 25 = {result6D} ({(TritArray27)result6D})"); // Shift: 111111111 >> 25 = 0000..TT (-4)

    }
}