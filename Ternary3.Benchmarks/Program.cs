using BenchmarkDotNet.Running;

namespace Ternary3.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<ConversionBenchmarks>();
        //BenchmarkRunner.Run<CalculatorBenchmarks>();
        //BenchmarkRunner.Run<MultiplicationStrategyBenchmarks>();
    }
}
