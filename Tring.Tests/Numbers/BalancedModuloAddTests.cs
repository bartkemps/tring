using FluentAssertions;
using Tring.Numbers;
using Xunit;

namespace Tring.Tests.Numbers
{
    public class BalancedModuloAddTests
    {
        [Theory]
        [InlineData(1L, 1L, 2L, 2L)]  // Simple addition within range
        [InlineData(-1L, -1L, 2L, -2L)] // Negative addition within range
        [InlineData(2L, 2L, 2L, -1L)]   // Addition requiring balancing
        [InlineData(-2L, -2L, 2L, 1L)]  // Negative addition requiring balancing
        [InlineData(1L, -1L, 2L, 0L)]   // Mixed signs
        public void BalancedModuloAdd_BasicCases(long value1, long value2, long halfModulus, long expected)
        {
            value1.BalancedModuloAdd(value2, halfModulus).Should().Be(expected);
        }

        [Theory]
        [InlineData(long.MaxValue, 1L, long.MaxValue/2)]          // Near positive overflow
        [InlineData(long.MinValue, -1L, long.MaxValue/2)]        // Near negative overflow
        [InlineData(long.MaxValue/2, long.MaxValue/2, long.MaxValue/4)]  // Large positive values
        [InlineData(long.MinValue/2, long.MinValue/2, long.MaxValue/4)]  // Large negative values
        public void BalancedModuloAdd_LargeValues_ShouldStayBalanced(long value1, long value2, long halfModulus)
        {
            var result = value1.BalancedModuloAdd(value2, halfModulus);
            Math.Abs(result).Should().BeLessThanOrEqualTo(halfModulus);
        }

        [Theory]
        [InlineData(1L, long.MaxValue - 1, 2L)]  // Sum would overflow without unsigned handling
        [InlineData(-1L, long.MinValue + 1, 2L)] // Sum would underflow without unsigned handling
        public void BalancedModuloAdd_NearOverflow_ShouldWork(long value1, long value2, long halfModulus)
        {
            var result = value1.BalancedModuloAdd(value2, halfModulus);
            Math.Abs(result).Should().BeLessThanOrEqualTo(halfModulus);
        }

        [Theory]
        [InlineData(0L, 0L, long.MaxValue/2)]     // Edge case with maximum halfModulus
        [InlineData(1L, -1L, long.MaxValue/2)]    // Mixed signs with maximum halfModulus
        [InlineData(long.MaxValue/2, long.MaxValue/2, long.MaxValue/2)]  // Large values with maximum halfModulus
        public void BalancedModuloAdd_MaxHalfModulus_ShouldWork(long value1, long value2, long halfModulus)
        {
            var result = value1.BalancedModuloAdd(value2, halfModulus);
            Math.Abs(result).Should().BeLessThanOrEqualTo(halfModulus);
        }

        [Fact]
        public void BalancedModuloAdd_RandomValues_ShouldStayBalanced()
        {
            var random = new Random(42); // Fixed seed for reproducibility
            for (int i = 0; i < 1000; i++)
            {
                var value1 = random.NextInt64(long.MinValue, long.MaxValue);
                var value2 = random.NextInt64(long.MinValue, long.MaxValue);
                var halfModulus = Math.Abs(random.NextInt64(1, long.MaxValue/2));

                var result = value1.BalancedModuloAdd(value2, halfModulus);
                Math.Abs(result).Should().BeLessThanOrEqualTo(halfModulus);
            }
        }

        [Theory]
        [InlineData(4L, 5L, 3L, 2L)]     // Sum greater than modulus
        [InlineData(-4L, -5L, 3L, -2L)]    // Negative sum less than -modulus
        [InlineData(3L, 3L, 3L, -1L)]     // Sum equals modulus
        [InlineData(-3L, -3L, 3L, 1L)]    // Sum equals -modulus
        public void BalancedModuloAdd_ModulusWrapping_ShouldBeCorrect(long value1, long value2, long halfModulus, long expected)
        {
            value1.BalancedModuloAdd(value2, halfModulus).Should().Be(expected);
        }
    }
}
