// filepath: c:\Users\kempsb\source\repos\Ternary\Ternary3.Tests\TritArrayTests.Range.cs
using FluentAssertions;

namespace Ternary3.Tests
{
    public partial class TritArrayTests
    {
        [Fact]
        public void Range_ReturnsExpectedValues_ForFullRange()
        {
            // Create a TritArray with known values
            var source = new BigTritArray(10);
            for (var i = 0; i < source.Length; i++)
            {
                source[i] = (i % 3) switch
                {
                    0 => Trit.Negative,
                    1 => Trit.Zero,
                    _ => Trit.Positive
                };
            }

            var result = source[0..10];

            result.Length.Should().Be(10);
            for (var i = 0; i < result.Length; i++)
            {
                result[i].Should().Be(source[i], $"because index {i} should match the original array");
            }
        }

        [Fact]
        public void Range_ReturnsExpectedValues_ForPartialRange()
        {
            // Create a TritArray with known values
            var source = new BigTritArray(10);
            for (var i = 0; i < source.Length; i++)
            {
                source[i] = (i % 3) switch
                {
                    0 => Trit.Negative,
                    1 => Trit.Zero,
                    _ => Trit.Positive
                };
            }

            var result = source[2..8];

            result.Length.Should().Be(6);
            for (var i = 0; i < result.Length; i++)
            {
                result[i].Should().Be(source[i + 2], $"because index {i} in result should match index {i + 2} in source");
            }
        }

        [Fact]
        public void Range_ReturnsExpectedValues_WhenCrossingWordBoundary()
        {
            // Create a large array to ensure we cross 64-bit word boundaries
            var source = new BigTritArray(100);
            for (var i = 0; i < source.Length; i++)
            {
                source[i] = (i % 3) switch
                {
                    0 => Trit.Negative,
                    1 => Trit.Zero,
                    _ => Trit.Positive
                };
            }

            // Get a range that crosses a 64-bit boundary (64 trits per ulong)
            var result = source[60..70];

            result.Length.Should().Be(10);
            for (var i = 0; i < result.Length; i++)
            {
                result[i].Should().Be(source[i + 60], $"because index {i} in result should match index {i + 60} in source");
            }
        }

        [Fact]
        public void Range_WithEndIndexFromEnd_ReturnsExpectedValues()
        {
            var source = new BigTritArray(10);
            for (var i = 0; i < source.Length; i++)
            {
                source[i] = (i % 3) switch
                {
                    0 => Trit.Negative,
                    1 => Trit.Zero,
                    _ => Trit.Positive
                };
            }

            var result = source[2..^2];

            result.Length.Should().Be(6);
            for (var i = 0; i < result.Length; i++)
            {
                result[i].Should().Be(source[i + 2], $"because index {i} in result should match index {i + 2} in source");
            }
        }

        [Fact]
        public void Range_WithStartAndEndIndexFromEnd_ReturnsExpectedValues()
        {
            var source = new BigTritArray(10);
            for (var i = 0; i < source.Length; i++)
            {
                source[i] = (i % 3) switch
                {
                    0 => Trit.Negative,
                    1 => Trit.Zero,
                    _ => Trit.Positive
                };
            }

            var result = source[^8..^2];

            result.Length.Should().Be(6);
            for (var i = 0; i < result.Length; i++)
            {
                result[i].Should().Be(source[i + 2], $"because index {i} in result should match index {i + 2} in source");
            }
        }

        [Fact]
        public void Range_WithImplicitStart_ReturnsExpectedValues()
        {
            var source = new BigTritArray(10);
            for (var i = 0; i < source.Length; i++)
            {
                source[i] = (i % 3) switch
                {
                    0 => Trit.Negative,
                    1 => Trit.Zero,
                    _ => Trit.Positive
                };
            }

            var result = source[..5];

            result.Length.Should().Be(5);
            for (var i = 0; i < result.Length; i++)
            {
                result[i].Should().Be(source[i], $"because index {i} should match the original array");
            }
        }

        [Fact]
        public void Range_WithImplicitEnd_ReturnsExpectedValues()
        {
            var source = new BigTritArray(10);
            for (var i = 0; i < source.Length; i++)
            {
                source[i] = (i % 3) switch
                {
                    0 => Trit.Negative,
                    1 => Trit.Zero,
                    _ => Trit.Positive
                };
            }

            var result = source[5..];

            result.Length.Should().Be(5);
            for (var i = 0; i < result.Length; i++)
            {
                result[i].Should().Be(source[i + 5], $"because index {i} in result should match index {i + 5} in source");
            }
        }

        [Fact]
        public void Range_WithInvalidRange_ThrowsArgumentOutOfRangeException()
        {
            var source = new BigTritArray(10);

            // Start index greater than end index
            Action act1 = () => _ = source[8..5];
            act1.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*Invalid range for splicing.*");

            // Negative start index after resolution
            Action act2 = () => _ = source[^12..5];
            act2.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*Invalid range for splicing.*");

            // End index beyond array length
            Action act3 = () => _ = source[5..15];
            act3.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*Invalid range for splicing.*");
        }
        
        [Fact]
        public void Range_WithNonZeroStartBit_HandlesAlignment()
        {
            // Create an array with a specific pattern
            var source = new BigTritArray(20);
            for (var i = 0; i < source.Length; i++)
            {
                source[i] = i % 2 == 0 ? Trit.Negative : Trit.Positive;
            }

            // Test range with non-zero starting bit
            var result = source[3..10];
            
            result.Length.Should().Be(7);
            for (var i = 0; i < result.Length; i++)
            {
                result[i].Should().Be(source[i + 3], $"because index {i} in result should match index {i + 3} in source");
            }
        }
    }
}
