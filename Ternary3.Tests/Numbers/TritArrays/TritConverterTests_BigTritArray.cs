namespace Ternary3.Tests.Numbers.TritArrays;

using System.Numerics;
using FluentAssertions;
using Ternary3.TritArrays;
using Xunit;

partial class TritConverterTests
{
    [Fact]
    public void ToTrits_ReturnsEmptyArrays_WhenValueIsZero()
    {
        BigInteger value = 0;
        TritConverter.ToTrits(value, out var negative, out var positive, out var length);
        negative.Should().BeEmpty("because zero has no trits");
        positive.Should().BeEmpty("because zero has no trits");
        length.Should().Be(0, "because zero has no trits");
    }
    
    [Theory]
    [InlineData(1, 0, 1, 1)]
    [InlineData(terTTT000TTT000, 0b111000111000, 0, 12)]
    public void ToTrits_ReturnsEmptyArrays_Max64Trits(long lo, ulong expectedNegative, ulong expectedPositive, int expectedLength)
    {
        var value = new BigInteger(lo);
        TritConverter.ToTrits(value, out var negative, out var positive, out var length);
        
        negative.Should().HaveCount(1, "because the negative trit count should match the expected count");
        positive.Should().HaveCount(1, "because the positive trit count should match the expected count");
        negative[0].Should().Be(expectedNegative, "because the negative trit should match the expected value");
        positive[0].Should().Be(expectedPositive, "because the positive trit should match the expected value");
        length.Should().Be(expectedLength, "because the total length should equal the sum of negative and positive counts");
    }
   
}