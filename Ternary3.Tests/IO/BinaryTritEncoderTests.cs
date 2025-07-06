using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Ternary3.IO;
using Xunit;

namespace Ternary3.Tests.IO
{
    public class BinaryTritEncoderTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void EncodeDecode_ReturnsOriginalSequence_ForVariousSizes(int size)
        {
            // Arrange
            var encoder = new BinaryTritEncoder();
            var original = GenerateInt3TSequence(size);

            // Act
            var encoded = encoder.Encode(original).ToArray();
            var decoded = encoder.Decode(encoded).ToArray();

            // Assert
            decoded.Should().BeEquivalentTo(original,
                options => options.WithStrictOrdering(),
                $"because encoding and then decoding {size} Int3T values should return the original values");
        }

        [Theory]
        [InlineData(-1, -1, -1, -1)]
        [InlineData(1, 0, -1, 1)]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 1, 1, 1)]
        [InlineData(-1, 1, 0, -1)]
        public void EncodeDecode_ReturnsOriginalSequence_ForSpecificPatterns(params int[] values)
        {
            // Arrange
            var encoder = new BinaryTritEncoder();
            var original = values.Select(v => (Int3T)v).ToArray();

            // Act
            var encoded = encoder.Encode(original).ToArray();
            var decoded = encoder.Decode(encoded).ToArray();

            // Assert
            decoded.Should().BeEquivalentTo(original,
                options => options.WithStrictOrdering(),
                $"because encoding and then decoding the pattern [{string.Join(", ", values)}] should return the original pattern");
        }

        [Fact]
        public void Decode_DoesNotRequireMagicNumbers()
        {
            // Arrange
            var encoder = new BinaryTritEncoder();
            byte[] twiceZero = [0, 0]; // Invalid magic numbers

            // Act
            var decoded = encoder.Decode(twiceZero).ToArray();

            // Assert
            decoded.Length.Should().Be(3);
        }

        private static Int3T[] GenerateInt3TSequence(int size)
        {
            if (size == 0) return [];

            var result = new Int3T[size];
            for (var i = 0; i < size; i++)
            {
                result[i] = (sbyte)(i % 3 - 1);
            }

            return result;
        }
    }
}