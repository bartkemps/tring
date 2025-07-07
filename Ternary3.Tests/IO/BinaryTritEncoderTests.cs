using FluentAssertions;
using Ternary3.IO;

namespace Ternary3.Tests.IO
{
    public class BinaryTritEncoderTests
    {
        [Fact]
        public void Constructor_WithDefaultParameters_ShouldWriteMagicNumber()
        {
            var encoder = new BinaryTritEncoder();
            Int3T[] singleTrit = [1];
            
            var encoded = encoder.Encode(singleTrit, true).ToArray();
            
            encoded.Should().HaveCount(3);
            encoded[0].Should().Be(245); // Magic number
            encoded[1].Should().Be(244); // Format indicator
        }
        
        [Fact]
        public void Constructor_WithDefaultParameters_ShouldNotWriteMagicNumber()
        {
            var encoder = new BinaryTritEncoder(false);
            Int3T[] singleTrit = [1];
            
            var encoded = encoder.Encode(singleTrit, true).ToArray();
            
            encoded.Should().HaveCount(1);
        }

        [Theory]
        [InlineData(new int[] { 1,1,1,1 })]
        [InlineData(new int[] { 1, 0, -1, 1, 0, -1, 1, 0, -1 })]
        [InlineData(new int[] { -1, 0, 10, -10, 0, 1, -1, 0, 1 })]
        public void LargeSequences_EncodeDecode_PreservesAllValues(int[] values)
        {
            // Convert int array to Int3T array
            var originalTrits = values.Select(v => (Int3T)v).ToArray();
            
            // Use separate encoders for encoding and decoding
            var encoder = new BinaryTritEncoder();
            var decoder = new BinaryTritEncoder();
            
            var encoded = encoder.Encode(originalTrits, true).ToArray();
            var decoded = decoder.Decode(encoded).ToArray();
            
            decoded.Should().BeEquivalentTo(originalTrits,
                options => options.WithStrictOrdering(),
                "because all trits should be preserved in the encode-decode cycle");
        }
    }
}
