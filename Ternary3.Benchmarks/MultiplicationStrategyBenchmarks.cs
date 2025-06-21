using System;
using BenchmarkDotNet.Attributes;
using Ternary3.TritArrays;
using System.Collections.Generic;

[MemoryDiagnoser]
[WarmupCount(1)]
[IterationCount(2)]
public class MultiplicationStrategyBenchmarks
{
    [Params(1, 2, 3, 4, 6, 8, 12, 16, 20, 24, 28, 32)]
    public int NonZeroBits;

    private uint negative1, positive1, negative2, positive2;
    private ulong negative1L, positive1L, negative2L, positive2L;

    [GlobalSetup]
    public void Setup()
    {
        (negative1, positive1) = GenerateBalancedTernaryWithBits(NonZeroBits, 0xAAAA_AAAA);
        (negative2, positive2) = GenerateBalancedTernaryWithBits(NonZeroBits, 0x5555_5555);
        (negative1L, positive1L) = GenerateBalancedTernaryWithBits64(NonZeroBits, 0xAAAA_AAAA_AAAA_AAAA);
        (negative2L, positive2L) = GenerateBalancedTernaryWithBits64(NonZeroBits, 0x5555_5555_5555_5555);
    }

    [Benchmark]
    public void MultiplyByAlgorithm_UInt32()
    {
        Calculator.MultiplyByAlgorithm(negative1, positive1, negative2, positive2, out var n, out var p);
    }

    [Benchmark]
    public void MultiplyByConversionToInt32_UInt32()
    {
        Calculator.MultiplyByConversionToInt32(negative1, positive1, negative2, positive2, out var n, out var p);
    }

    [Benchmark]
    public void MultiplyByAlgorithm_UInt64()
    {
        Calculator.MultiplyByAlgorithm(negative1L, positive1L, negative2L, positive2L, out var n, out var p);
    }

    [Benchmark]
    public void MultiplyByConversionToInt64_UInt64()
    {
        Calculator.MultiplyByConversionToInt64(negative1L, positive1L, negative2L, positive2L, out var n, out var p);
    }

    private static (uint neg, uint pos) GenerateBalancedTernaryWithBits(int nonZeroBits, uint pattern)
    {
        uint neg = 0, pos = 0;
        for (var i = 0; i < nonZeroBits; i++)
        {
            if (((pattern >> i) & 1) == 0)
                neg |= (1u << i);
            else
                pos |= (1u << i);
        }
        return (neg, pos);
    }

    private static (ulong neg, ulong pos) GenerateBalancedTernaryWithBits64(int nonZeroBits, ulong pattern)
    {
        ulong neg = 0, pos = 0;
        for (var i = 0; i < nonZeroBits; i++)
        {
            if (((pattern >> i) & 1) == 0)
                neg |= (1UL << i);
            else
                pos |= (1UL << i);
        }
        return (neg, pos);
    }
}
