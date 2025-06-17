using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Ternary3.Benchmarks
{
    using TritArrays;

    [MemoryDiagnoser]
    public class TritConversionBenchmarks
    {
        [Params(0, 1, -1, 42, -42, int.MaxValue, int.MinValue)]
        public int IntValue;

        [Params(0L, 1L, -1L, 42L, -42L, int.MaxValue, int.MinValue, 100000000000000000L, -100000000000000000L, long.MaxValue, long.MinValue)]
        public long LongValue;

        private uint negative, positive;
        private uint negative2, positive2;

        [Benchmark]
        public void TritConverter_ConvertTo32Trits_Int()
        {
            TritConverter.ConvertTo32Trits(IntValue, out negative, out positive);
        }

        [Benchmark]
        public void Converter_ToTrits_Int()
        {
            Converter.ToTrits(IntValue, out negative2, out positive2);
        }

        [Benchmark]
        public void TritConverter_ConvertTo32Trits_Long()
        {
            TritConverter.ConvertTo32Trits(LongValue, out negative, out positive);
        }

        [Benchmark]
        public void Converter_ToTrits_Long()
        {
            Converter.ToTrits(LongValue, out negative2, out positive2);
        }
    }

    public class Program
    {
        [Fact(Skip="Benchmarks are not run in unit tests.")]
        public void RunBenchmarks()
        {
            BenchmarkRunner.Run<TritConversionBenchmarks>();
        }
    }
}