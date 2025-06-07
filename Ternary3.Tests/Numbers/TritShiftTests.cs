using FluentAssertions;
using Ternary3.Numbers;

namespace Ternary3.Tests.Numbers;

using Ternary3.Numbers.Integers;

public class TritShiftTests
{
    [Theory]
    [InlineData(123, 0, 123)] // no shift
    [InlineData(0, 1, 0)] // zero value
    [InlineData(121, 5, 0)]  // overflow positive shift
    [InlineData(-121, 5, 0)]  // overflow positive shift
    [InlineData(121, -5, 0)]  // overflow negative shift
    [InlineData(-121, -5, 0)]  // overflow negative shift
    [InlineData(3, 1, 1)]  // normal shift 10 >>1 = 1 
    [InlineData(3, -1, 9)]  // normal shift 10 <<1=100
    [InlineData(-3, 1, -1)]  // normal shift T0 >>1 = T 
    [InlineData(-3, -1, -9)]  // normal shift T0 <<1=T00
    [InlineData(121, 4, 1)]  // 1..1 >> 4 = 1
    [InlineData(121, -4, 81)]  // 1..1 <<4 = 1..
    [InlineData(-121, 4, -1)]  // T..T >> 4 = T
    [InlineData(-121, -4, -81)]  // T..T << 4 = T..
    [InlineData(119, -4, -81)]  // 11..T << 4 = T..
    [InlineData(-119, -4, 81)]  // TT..1 << 4 = 1..
    public void SByteShift_ShouldWorkCorrectly(sbyte value, int shift, sbyte expected)
    {
        value.Shift(shift).Should().Be(expected);
    }

    [Theory]
    [InlineData(12345, 0, 12345)] // no shift
    [InlineData(0, 1, 0)] // zero value
    [InlineData(29524, 10, 0)]  // overflow positive shift
    [InlineData(-29524, 10, 0)]  // overflow positive shift
    [InlineData(29524, -10, 0)]  // overflow negative shift
    [InlineData(-29524, -10, 0)]  // overflow negative shift
    [InlineData(3, 1, 1)]  // normal shift 10 >>1 = 1
    [InlineData(3, -1, 9)]  // normal shift 10 <<1=100
    [InlineData(-3, 1, -1)]  // normal shift T0 >>1 = T
    [InlineData(-3, -1, -9)]  // normal shift T0 <<1=T00
    [InlineData(29524, 9, 1)]  // 1..1 >> 9 = 1
    [InlineData(29524, -9, 19683)]  // 1..1 << 9 = 1..
    [InlineData(-29524, 9, -1)]  // T..T >> 9 = T
    [InlineData(-29524, -9, -19683)]  // T..T << 9 = T..
    [InlineData(29522, -9, -19683)]  // 11..T << 9 = T..
    [InlineData(-29522, -9, 19683)]  // TT..1 << 9 = 1..

    public void ShortShift_ShouldWorkCorrectly(short value, int shift, short expected)
    {
        value.Shift(shift).Should().Be(expected);
    }

    [Theory]
    [InlineData(123456789, 0, 123456789)] // no shift
    [InlineData(0, 1, 0)] // zero value
    [InlineData(1743392200, 20, 0)]  // overflow positive shift
    [InlineData(-1743392200, 20, 0)]  // overflow positive shift
    [InlineData(1743392200, -20, 0)]  // overflow negative shift
    [InlineData(-1743392200, -20, 0)]  // overflow negative shift
    [InlineData(3, 1, 1)]  // normal shift 10 >>1 = 1 
    [InlineData(3, -1, 9)]  // normal shift 10 <<1=100
    [InlineData(-3, 1, -1)]  // normal shift T0 >>1 = T 
    [InlineData(-3, -1, -9)]  // normal shift T0 <<1=T00
    [InlineData(1743392200, 19, 1)]  // 1..1 >> 19 = 1
    [InlineData(1743392200, -19, 1162261467)]  // 1..1 << 19 = 1..
    [InlineData(-1743392200, 19, -1)]  // T..T >> 19 = T
    [InlineData(-1743392200, -19, -1162261467)]  // T..T << 19 = T..
    [InlineData(1743392198, -19, -1162261467)]  // 11..T << 19 = T..
    [InlineData(-1743392198, -19, 1162261467)]  // TT..1 << 19 = 1..

    public void IntShift_ShouldWorkCorrectly(int value, int shift, int expected)
    {
        value.Shift(shift).Should().Be(expected);
    }

    [Theory]
    [InlineData(1234567890, 0, 1234567890)] // no shift
    [InlineData(0, 1, 0)] // zero value
    [InlineData(6078832729528464400L, 40, 0L)]  // overflow positive shift
    [InlineData(-6078832729528464400L, 40, 0L)]  // overflow positive shift
    [InlineData(6078832729528464400L, -40, 0L)]  // overflow negative shift
    [InlineData(-6078832729528464400L, -40, 0L)]  // overflow negative shift
    [InlineData(3, 1, 1)]  // normal shift 10 >>1 = 1 
    [InlineData(3, -1, 9)]  // normal shift 10 <<1=100
    [InlineData(-3, 1, -1)]  // normal shift T0 >>1 = T 
    [InlineData(-3, -1, -9)]  // normal shift T0 <<1=T00
    [InlineData(6078832729528464400L, 39, 1L)]  // 1..1 >> 39 = 1
    [InlineData(6078832729528464400L, -39, 4052555153018976267L)]  // 1..1 << 39 = 1..
    [InlineData(-6078832729528464400L, 39, -1L)]  // T..T >> 39 = T
    [InlineData(-6078832729528464400L, -39, -4052555153018976267L)]  // T..T << 39 = T..
    [InlineData(6078832729528464398L, -39, -4052555153018976267L)]  // 11..T << 39 = T..
    [InlineData(-6078832729528464398L, -39, 4052555153018976267L)]  // TT..1 << 39 = 1..
    public void LongShift_ShouldWorkCorrectly(long value, int shift, long expected)
    {
        value.Shift(shift).Should().Be(expected);
    }
}
