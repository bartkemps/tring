[assembly: Ternary3.GenerateTernaryConstants]

namespace Examples;

using Ternary3;

public partial class GeneratorDemos
{
    public static void Run()
    {
        Console.WriteLine($"The ternary value of T01 is {terT01}");
        var sum = terT01 + ter111_111;
        Console.WriteLine($"{nameof(terT01)} + {nameof(ter111_111)} = {sum} ({sum:ter})");
        Int27T int27Value = terTTT000111;
        TritArray27 tritArrayValue = ter111111111_000000000_TTTTTTTTT;
    }
}