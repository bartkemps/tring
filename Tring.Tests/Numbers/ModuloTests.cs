using FluentAssertions;
using Tring.Numbers;
using Xunit;

namespace Tring.Tests.Numbers
{
    public class ModuloTests
    {
        [Theory]
        [InlineData(-9, 1)]
        [InlineData(-4, 1)]
        [InlineData(-3, 2)]
        [InlineData(-2, -2)]
        [InlineData(-1, -1)]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, -2)]
        [InlineData(4, -1)]
        [InlineData(9, -1)]
        public void BalancedModulo_Int_ReturnsExpected(int value, int expected)
        {
            value.BalancedModulo(2).Should().Be(expected);
        }

        [Fact]
        public void BalancedModulo_Int_Limits()
        {
            int.MinValue.BalancedModulo(2).Should().BeLessThanOrEqualTo(2);
            int.MaxValue.BalancedModulo(2).Should().BeLessThanOrEqualTo(2);
            int.MaxValue.BalancedModulo(int.MaxValue - 2).Should().Be(3 - int.MaxValue);
            int.MinValue.BalancedModulo(-2 - int.MinValue).Should().Be(-3 - int.MinValue);
        }

        [Theory]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void BalancedModulo_Long_Limits(long value)
        {
            var actual = value.BalancedModulo(2);
            Math.Abs(actual).Should().BeLessThanOrEqualTo(2);
        }


        [Theory]
        [InlineData(-9L, 1L)]
        [InlineData(-4L, 1L)]
        [InlineData(-3L, 2L)]
        [InlineData(-2L, -2L)]
        [InlineData(-1L, -1L)]
        [InlineData(0L, 0L)]
        [InlineData(1L, 1L)]
        [InlineData(2L, 2L)]
        [InlineData(3L, -2L)]
        [InlineData(9L, -1L)]
        public void BalancedModulo_Long_ReturnsExpected(long value, long expected)
        {
            value.BalancedModulo(2).Should().Be(expected);
        }
        
        [Fact]
        public void BalancedModulo_Long_Limits2()
        {
            var long1 = (long)Int40T.MaxValue;
            long1.BalancedModuloAdd(long1, Int40T.MaxValueConstant).Should().Be(-1);
        }

        [Theory]
        [InlineData(10UL, 0, 10UL * 10UL)] 
        [InlineData(4294967295UL, 0, 4294967295UL * 4294967295UL)] 
        [InlineData(4294967296UL, 1, 0)] 
        [InlineData(18446744073709551615UL, 18446744073709551614UL, 1)] 
        public void Multiply_UInt64_ReturnExpectedValue(ulong value, ulong expectedHi, ulong expectedLo)
        {
            var (hi, lo) = value.Multiply(value);
            hi.Should().Be(expectedHi);
            lo.Should().Be(expectedLo);
        }
        
        [Theory]
        [InlineData(10L, 0, 10UL * 10UL)] 
        [InlineData(4294967295L, 0, 4294967295L * 4294967295UL)] 
        [InlineData(4294967296UL, 1, 0)] 
        [InlineData(9223372036854775807L, 4611686018427387903L, 1)] 
        [InlineData(9223372036854775807L, 4611686018427387903L, 1)] 
        public void Multiply_Int64_ReturnExpectedValue(long value, ulong expectedHi, ulong expectedLo)
        {
            var (hi, lo) = value.Multiply(value);
            hi.Should().Be(expectedHi);
            lo.Should().Be(expectedLo);
        }
    }
}

