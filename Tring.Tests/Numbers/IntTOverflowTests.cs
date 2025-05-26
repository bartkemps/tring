namespace Tring.Tests.Numbers;

using FluentAssertions;
using Tring.Numbers;

public class IntTOverflowTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(10)]
    [InlineData(-10)]
    public void Cast_OverflowsCorrectly_WithMultiplesOf59049(int factor)
    {
        var expected = (Int10T)1000;
        var actual = (Int10T)(factor * 59049 + 1000);
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(10)]
    [InlineData(-10)]
    public void Cast_OverflowsCorrectly_WithMultiplesOf3486784401(long factor)
    {
        var expected = (Int20T)1000;
        var actual = (Int20T)(factor * 3486784401 + 1000);
        actual.Should().Be(expected);
    }

    [Fact]
    public void Overflow_OverflowsCorrectly_WhenUnchecked()
    {
        unchecked
        {
            short max = Int10T.MaxValue;
            short min = Int10T.MinValue;

            var overFlowed = (Int10T)(max + 1);
            ((short)overFlowed).Should().Be(min);

            var underFlowed = (Int10T)(min - 1);
            ((short)underFlowed).Should().Be(max);
        }
    }

    [Fact]
    public void Addition_OverflowsCorrectly()
    {
        (Int10T.MaxValue + Int10T.MaxValue).Should().Be((Int10T)(-1));
        //(Int10T.MaxValue * (Int10T)3).Should().Be((Int10T)(-3));
        //(Int20T.MaxValue + Int20T.MaxValue).Should().Be((Int20T)(-1));
        //(Int40T.MaxValue + Int40T.MaxValue).Should().Be((Int40T)(-1));
    }
    

    
}