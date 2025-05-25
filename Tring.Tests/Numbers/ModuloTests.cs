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
    }
}