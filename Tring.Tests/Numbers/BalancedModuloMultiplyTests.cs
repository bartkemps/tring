using FluentAssertions;
using Tring.Numbers;
using Xunit;

namespace Tring.Tests.Numbers
{
    public class BalancedModuloMultiplyTests
    {
        [Theory]
        [InlineData(1L, 1L, 2L, 1L)]      // Simple multiplication within range
        [InlineData(-1L, -1L, 2L, 1L)]    // Negative * negative = positive
        [InlineData(1L, -1L, 2L, -1L)]    // Mixed signs
        [InlineData(-1L, 1L, 2L, -1L)]    // Mixed signs reversed
        [InlineData(0L, 5L, 2L, 0L)]      // Zero multiplication
        [InlineData(5L, 0L, 2L, 0L)]      // Zero multiplication reversed
        public void BalancedModuloMultiply_BasicCases(long value1, long value2, long halfModulus, long expected)
        {
            value1.BalancedModuloMultiply(value2, halfModulus).Should().Be(expected);
        }

        [Theory]
        [InlineData(2L, 2L, 2L, -1L)]     // 4 mod 5 -> -1
        [InlineData(-2L, -2L, 2L, -1L)]   // 4 mod 5 -> -1
        [InlineData(2L, -2L, 2L, 1L)]     // -4 mod 5 -> 1
        [InlineData(-2L, 2L, 2L, 1L)]     // -4 mod 5 -> 1
        public void BalancedModuloMultiply_Wrapping(long value1, long value2, long halfModulus, long expected)
        {
            value1.BalancedModuloMultiply(value2, halfModulus).Should().Be(expected);
        }

        [Theory]
        [InlineData(long.MaxValue/2, 2L, long.MaxValue/2)]  // Large value multiplication
        [InlineData(long.MinValue/2, -2L, long.MaxValue/2)] // Large negative value multiplication
        public void BalancedModuloMultiply_LargeValues_ShouldStayBalanced(long value1, long value2, long halfModulus)
        {
            var result = value1.BalancedModuloMultiply(value2, halfModulus);
            Math.Abs(result).Should().BeLessThanOrEqualTo(halfModulus);
        }

        [Fact]
        public void BalancedModuloMultiply_RandomValues_ShouldStayBalanced()
        {
            var random = new Random(42); // Fixed seed for reproducibility
            for (int i = 0; i < 1000; i++)
            {
                var value1 = random.NextInt64(long.MinValue, long.MaxValue);
                var value2 = random.NextInt64(long.MinValue, long.MaxValue);
                var halfModulus = Math.Abs(random.NextInt64(1, long.MaxValue/2));

                var result = value1.BalancedModuloMultiply(value2, halfModulus);
                Math.Abs(result).Should().BeLessThanOrEqualTo(halfModulus);
            }
        }

        [Theory]
        [InlineData(3, 3, 3, 0)]       // 9 mod 7 -> 2 -> -1
        [InlineData(-3, -3, 3, 0)]     // 9 mod 7 -> 2 -> -1
        [InlineData(3, -3, 3, 0)]      // -9 mod 7 -> -2 -> 1
        [InlineData(-3, 3, 3, 0)]      // -9 mod 7 -> -2 -> 1
        public void BalancedModuloMultiply_ModulusWrapping_ShouldBeCorrect(long value1, long value2, long halfModulus, long expected)
        {
            value1.BalancedModuloMultiply(value2, halfModulus).Should().Be(expected);
        }
    }
}
