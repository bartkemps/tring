namespace Ternary3.Benchmarks;

using BenchmarkDotNet.Attributes;
using TritArrays;


[MemoryDiagnoser]
[WarmupCount(1)]
[IterationCount(2)]
public class CalculatorBenchmarks
{
    private const int Iterations = 100;
    private readonly uint[] negs32A, poss32A, negs32B, poss32B;
    private readonly ulong[] negs64A, poss64A, negs64B, poss64B;
    private readonly int[] int32A, int32B;
    private readonly long[] int64A, int64B;

    public CalculatorBenchmarks()
    {
        negs32A = new uint[Iterations];
        poss32A = new uint[Iterations];
        negs32B = new uint[Iterations];
        poss32B = new uint[Iterations];
        negs64A = new ulong[Iterations];
        poss64A = new ulong[Iterations];
        negs64B = new ulong[Iterations];
        poss64B = new ulong[Iterations];
        int32A = new int[Iterations];
        int32B = new int[Iterations];
        int64A = new long[Iterations];
        int64B = new long[Iterations];
        var rand = new Random(42);
        for (var i = 0; i < Iterations; i++)
        {
            // Use a mix of sparse and dense trit patterns
            int32A[i] = rand.Next(int.MinValue, int.MaxValue);
            int32B[i] = rand.Next(int.MinValue, int.MaxValue);
            var lA = ((long)rand.Next(int.MinValue, int.MaxValue) << 32) | (uint)rand.Next();
            var lB = ((long)rand.Next(int.MinValue, int.MaxValue) << 32) | (uint)rand.Next();
            int64A[i] = lA;
            int64B[i] = lB;
            TritConverter.To32Trits(int32A[i], out negs32A[i], out poss32A[i]);
            TritConverter.To32Trits(int32B[i], out negs32B[i], out poss32B[i]);
            TritConverter.To64Trits(int64A[i], out negs64A[i], out poss64A[i]);
            TritConverter.To64Trits(int64B[i], out negs64B[i], out poss64B[i]);
        }
    }

    [Benchmark]
    public void AddBalancedTernary_UInt32()
    {
        for (var i = 0; i < Iterations; i++)
        {
            Calculator.AddBalancedTernary(negs32A[i], poss32A[i], negs32B[i], poss32B[i], out var n, out var p);
        }
    }

    [Benchmark]
    public void AddBalancedTernary_UInt64()
    {
        for (var i = 0; i < Iterations; i++)
        {
            Calculator.AddBalancedTernary(negs64A[i], poss64A[i], negs64B[i], poss64B[i], out var n, out var p);
        }
    }

    [Benchmark]
    public void AddViaConversion_Int32()
    {
        for (var i = 0; i < Iterations; i++)
        {
            var a = TritConverter.ToInt32(negs32A[i], poss32A[i]);
            var b = TritConverter.ToInt32(negs32B[i], poss32B[i]);
            var sum = a + b;
            TritConverter.To32Trits(sum, out var n, out var p);
        }
    }

    [Benchmark]
    public void AddViaConversion_Int64()
    {
        for (var i = 0; i < Iterations; i++)
        {
            var a = TritConverter.ToInt64(negs64A[i], poss64A[i]);
            var b = TritConverter.ToInt64(negs64B[i], poss64B[i]);
            var sum = a + b;
            TritConverter.To64Trits(sum, out var n, out var p);
        }
    }
}