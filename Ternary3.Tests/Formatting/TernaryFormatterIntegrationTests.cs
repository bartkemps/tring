namespace Ternary3.Tests.Formatting;

using System;
using FluentAssertions;
using Ternary3;
using Xunit;
using Ternary3.Formatting;

public class TernaryFormatterIntegrationTests
{
    [Theory]
    [InlineData(-4, "0TT")]
    [InlineData(-1, "0T")]
    [InlineData(0, "0")]
    [InlineData(1, "01")]
    [InlineData(4, "011")]
    public void Formatter_Formats_IntTypes_AsTrits(long value, string expectedTrits)
    {
        var sut = new TernaryFormatter();
        sut.Format("ter", (Int27T)value, null).Should().EndWith(expectedTrits);
        sut.Format("ter", (Int9T)(short)value, null).Should().EndWith(expectedTrits);
        sut.Format("ter", (Int3T)(sbyte)value, null).Should().EndWith(expectedTrits);
        sut.Format("ter", (TernaryArray27)value, null).Should().EndWith(expectedTrits);
        sut.Format("ter", (TernaryArray9)(short)value, null).Should().EndWith(expectedTrits);
        sut.Format("ter", (TernaryArray3)(sbyte)value, null).Should().EndWith(expectedTrits);
    }

    [Theory]
    [InlineData(-4, "0TT")]
    [InlineData(-1, "0T")]
    [InlineData(0, "0")]
    [InlineData(1, "01")]
    [InlineData(4, "011")]
    public void Formatter_Formats_NumericTypes_AsTrits(long value, string expectedTrits)
    {
        var sut = new TernaryFormatter();
        sut.Format("ter", (sbyte)value, null).Should().EndWith(expectedTrits);
        sut.Format("ter", (short)value, null).Should().EndWith(expectedTrits);
        sut.Format("ter", (int)value, null).Should().EndWith(expectedTrits);
        sut.Format("ter", value, null).Should().EndWith(expectedTrits);
    }

    [Theory]
    [InlineData(9841L, "D", "9841")]
    [InlineData(-9841L, "D", "-9841")]
    public void Formatter_Forwards_To_Default_For_NonTernary_Format(long value, string format, string expectedStart)
    {
        var sut = new TernaryFormatter();
        var int27 = (Int27T)value;
        var int9 = (Int9T)(short)value;
        var int3 = (Int3T)(sbyte)value;
        sut.Format(format, int27, null).Should().StartWith(expectedStart);
        sut.Format(format, int9, null).Should().NotBeNull();
        sut.Format(format, int3, null).Should().NotBeNull();
    }

    [Fact]
    public void Formatter_Formats_TritArrays_AsTrits()
    {
        var sut = new TernaryFormatter();
        var arr3 = (TernaryArray3)(Int3T)1;
        var arr9 = (TernaryArray9)(Int9T)13;
        var arr27 = (TernaryArray27)(Int27T)9841L;
        sut.Format("ter", arr3, null).Should().Contain("1");
        sut.Format("ter", arr9, null).Should().Contain("111");
        sut.Format("ter", arr27, null).Should().NotBeNull();
    }

    [Fact]
    public void Formatter_Forwards_To_Default_For_Unsupported_Type()
    {
        var sut = new TernaryFormatter();
        var dt = new DateTime(2024, 1, 1);
        sut.Format("D", dt, null).Should().Contain("2024");
    }
}