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
    [InlineData(ter1111111111_1111111111_1111111111_1111111111, 0, 0b1111111111_1111111111_1111111111_1111111111, 40)]
    [InlineData(6078832729528464401, 0b1111111111_1111111111_1111111111_1111111111, 0b1_0000000000_0000000000_0000000000_0000000000, 41)]
    [InlineData(long.MaxValue, 687268435026, 1498450313609, 41)]
    [InlineData(long.MinValue, 1498450313608, 687268435025, 41)]
    public void ToTrits_ReturnsCorrectValue_Max64Trits(long lo, ulong expectedNegative, ulong expectedPositive, int expectedLength)
    {
        var value = new BigInteger(lo);
        TritConverter.ToTrits(value, out var negative, out var positive, out var length);
        
        negative.Should().HaveCount(1, "because the negative trit count should match the expected count");
        positive.Should().HaveCount(1, "because the positive trit count should match the expected count");
        negative[0].Should().Be(expectedNegative, "because the negative trit should match the expected value");
        positive[0].Should().Be(expectedPositive, "because the positive trit should match the expected value");
        length.Should().Be(expectedLength, "because the total length should equal the sum of negative and positive counts");
    }

    [Theory]
    [InlineData("9223372036854775808", 687268435025, 1498450313608, 41)]
    [InlineData("3", 0b0, 0b10, 2)]
    [InlineData("3433683820292512484657849089281", 0b0, 0b0, 65)]
    [InlineData("5895092288869291585760436430706259332839105796137920554548480", 0, ulong.MaxValue, 128)]
    [InlineData("-5895092288869291585760436430706259332839105796137920554548480", ulong.MaxValue, 0, 128)]
    [InlineData("5895092288869291585760436430706259332839105796137920554548481", ulong.MaxValue, 0, 129)]
    [InlineData("-5895092288869291585760436430706259332839105796137920554548481", 0, ulong.MaxValue, 129)]
    public void ToTrits_ReturnsCorrectValue_MoreThan64Trits(string number, ulong expectedNegative, ulong expectedPositive, int expectedLength)
    {
        var value = BigInteger.Parse(number);
        TritConverter.ToTrits(value, out var negative, out var positive, out var length);

        negative[0].Should().Be(expectedNegative, "because the negative trit should match the expected value");
        positive[0].Should().Be(expectedPositive, "because the positive trit should match the expected value");
        length.Should().Be(expectedLength, "because the total length should equal the sum of negative and positive counts");
    }

    [Theory]
    [InlineData("0")]
    [InlineData("3")]
    [InlineData("6078832729528464401")]
    [InlineData("5895092288869291585760436430706259332839105796137920554548480")]
    [InlineData("-5895092288869291585760436430706259332839105796137920554548480")]
    [InlineData("5895092288869291585760436430706259332839105796137920554548481")]
    [InlineData("-5895092288869291585760436430706259332839105796137920554548481")]
    public void ToBigInteger_InversesToTrits(string expected)
    {
        var expectedValue = BigInteger.Parse(expected);
        TritConverter.ToTrits(expectedValue, out var negative, out var positive, out _);

        var actual = TritConverter.ToBigInteger(negative, positive);

        actual.Should().Be(BigInteger.Parse(expected), "because the conversion should yield the original value when trits are correctly formed");
    }


}