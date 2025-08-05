namespace Ternary3.Benchmarks;

using BenchmarkDotNet.Attributes;
using TernaryArrays;

[MemoryDiagnoser]
[WarmupCount(1)]
[IterationCount(2)]
public class ConversionBenchmarks
{
    private const int Iterations = 100;
    private readonly int[] int32Inputs;
    private readonly long[] int64Inputs;
    private readonly uint[] uint32Inputs;
    private readonly ulong[] uint64Inputs;

    public ConversionBenchmarks()
    {
        int32Inputs = new int[Iterations];
        int64Inputs = new long[Iterations];
        uint32Inputs = new uint[Iterations];
        uint64Inputs = new ulong[Iterations];
        for (var k = 0; k < Iterations; k++)
        {
            int32Inputs[k] = int.MinValue + (int)((long)k * int.MaxValue / Iterations);
            int64Inputs[k] = long.MinValue + (long)((long)k * long.MaxValue / Iterations);
            uint32Inputs[k] = (uint)(uint.MinValue + (ulong)k * uint.MaxValue / Iterations);
            uint64Inputs[k] = uint.MinValue + (ulong)k * uint.MaxValue / (ulong)Iterations;
        }
    }

    [Benchmark]
    public void Int32_To32Trits()
    {
        for (var k = 0; k < Iterations; k++)
        {
            var i = int32Inputs[k];
            TritConverter.To32Trits(i, out var neg, out var pos);
        }
    }

    [Benchmark]
    public void Int64_To64Trits()
    {
        for (var k = 0; k < Iterations; k++)
        {
            var i = int64Inputs[k];
            TritConverter.To64Trits(i, out var neg, out var pos);
        }
    }

    [Benchmark]
    public void UInt32_To32Trits()
    {
        for (var k = 0; k < Iterations; k++)
        {
            var i = uint32Inputs[k];
            TritConverter.To32Trits((int)i, out var neg, out var pos);
        }
    }

    [Benchmark]
    public void UInt64_To64Trits()
    {
        for (var k = 0; k < Iterations; k++)
        {
            var i = uint64Inputs[k];
            TritConverter.To64Trits((long)i, out var neg, out var pos);
        }
    }

    [Benchmark]
    public void ToInt32_FromTrits()
    {
        var negs = new uint[Iterations];
        var poss = new uint[Iterations];
        for (var k = 0; k < Iterations; k++)
        {
            TritConverter.To32Trits(int32Inputs[k], out negs[k], out poss[k]);
        }
        for (var k = 0; k < Iterations; k++)
        {
            var roundTrip = TritConverter.ToInt32(negs[k], poss[k]);
        }
    }

    [Benchmark]
    public void ToInt64_FromTrits()
    {
        var negs = new ulong[Iterations];
        var poss = new ulong[Iterations];
        for (var k = 0; k < Iterations; k++)
        {
            TritConverter.To64Trits(int64Inputs[k], out negs[k], out poss[k]);
        }
        for (var k = 0; k < Iterations; k++)
        {
            var roundTrip = TritConverter.ToInt64(negs[k], poss[k]);
        }
    }
}