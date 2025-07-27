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

        // Addition and sumple multiplication of TernaryArray3 also may overflow.
        // (Under the hood, these are often performed without conversion to binary)
        TernaryArray3 input3A = 12; // 110
        TernaryArray3 input3B = 3; // 010
        var result3 = input3A * input3B; // 110 * TT0 = 101T00 (144). Int3T only keeps 3 trits, so T00 = -9     
        Console.WriteLine($"Overflow: {input3A} * {input3B} = {result3} ({(int)result3})"); // Overflow: 110 * 010 = 101T00 (-9)
        
        Int3T input4 = 25; // 10T1. trimmed to 3 trits = 0T1 or -8;
        Console.WriteLine($"Overflow: 25 => {input4} ({(TernaryArray3)input4})"); // Overflow: 25 => -2 (0T1)

        TernaryArray3 input5 = 25; // 10T1. trimmed to 3 trits = 0T1 or -8;
        Console.WriteLine($"Overflow: 25 => {input5} ({(int)input5})"); // Overflow: 25 => (0T1) (25)

        // Shifting trits two positions in essence multiplies or divides by 9 (3^2).
        var input6 = TernaryArray9.MaxValue; // 111111111
        var result6A = input5 << 6;
        Console.WriteLine($"Shift: {input6:ter} << 6 = {result6A:ter} ({result6A})"); // Shift: 111111111 << 6 = 111000000 (9477)
        var result6B = input5 >> -6;
        Console.WriteLine($"Shift: {input6:ter} >> -6 = {result6B:ter} ({result6B})"); // Shift: 111111111 >> -6 = 111000000 (9477)
        var result6C = input5 << -6;
        Console.WriteLine($"Shift: {input6:ter} << -6 = {result6C:ter} ({result6C})"); // Shift: 111111111 << -6 = 000000111 (13)
        var result6D = input5 >> 6;
        Console.WriteLine($"Shift: {input6:ter} >> 6 = {result6D:ter} ({result6D})"); // Shift: 111111111 >> 6 = 000000111 (13)

        var input7 = Int27T.MinValue; // TTTTTTTTT TTTTTTTTT TTTTTTTTT
        var result7A = input7 << 25;
        Console.WriteLine($"Shift: {input7:ter} << 25 = {result7A:ter} ({result7A})"); // Shift: TTTT..TT << 25 = TT00...00 (-3389154437772)
        var result7B = input7 >> -25;
        Console.WriteLine($"Shift: {input7:ter} >> -25 = {result7B:ter} ({result7B})"); // Shift: TTTT..TT >> -25 = TT00...00 (-3389154437772)
        var result7C = input7 << -25;
        Console.WriteLine($"Shift: {input7:ter} << -25 = {result7C:ter} ({result7C})"); // Shift: TTTT..TT << -25 = 0000...TT (-4)
        var result7D = input7 >> 25;
        Console.WriteLine($"Shift: {input7:ter} >> 25 = {result7D:ter} ({result7D})"); // Shift: 111111111 >> 25 = 0000..TT (-4)
    }
}