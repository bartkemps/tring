using System;
using FluentAssertions;
using Tring.Numbers;
using Xunit;

namespace Tring.Tests.Numbers;

public class IntT20CompareToTests
{
    [Fact]
    public void CompareTo_ShouldReturnZero_WhenValuesAreEqual()
    {
        IntT20 value = 42;
        value.CompareTo((IntT20)42).Should().Be(0);
    }

    [Fact]
    public void CompareTo_ShouldReturnNegative_WhenValueIsLess()
    {
        IntT20 value = 42;
        value.CompareTo((IntT20)100).Should().BeNegative();
    }

    [Fact]
    public void CompareTo_ShouldReturnPositive_WhenValueIsGreater()
    {
        IntT20 value = 42;
        value.CompareTo((IntT20)10).Should().BePositive();
    }

    [Fact]
    public void CompareTo_ShouldReturnPositive_WhenObjectIsNull()
    {
        IntT20 value = 42;
        value.CompareTo(null).Should().BePositive();
    }
    
    [Fact]
    public void CompareTo_WithInt32_ShouldCompareCorrectly()
    {
        IntT20 value = 42;
        value.CompareTo(42).Should().Be(0);
        value.CompareTo(100).Should().BeNegative();
        value.CompareTo(10).Should().BePositive();
    }
    
    [Fact]
    public void CompareTo_WithInt64_ShouldCompareCorrectly()
    {
        IntT20 value = 42;
        value.CompareTo((long)42).Should().Be(0);
        value.CompareTo((long)100).Should().BeNegative();
        value.CompareTo((long)10).Should().BePositive();
    }
    
    [Fact]
    public void CompareTo_WithInt16_ShouldCompareCorrectly()
    {
        IntT20 value = 42;
        value.CompareTo((short)42).Should().Be(0);
        value.CompareTo((short)100).Should().BeNegative();
        value.CompareTo((short)10).Should().BePositive();
    }
    
    [Fact]
    public void CompareTo_WithByte_ShouldCompareCorrectly()
    {
        IntT20 value = 42;
        value.CompareTo((byte)42).Should().Be(0);
        value.CompareTo((byte)100).Should().BeNegative();
        value.CompareTo((byte)10).Should().BePositive();
    }
    
    [Fact]
    public void CompareTo_WithSByte_ShouldCompareCorrectly()
    {
        IntT20 value = 42;
        value.CompareTo((sbyte)42).Should().Be(0);
        value.CompareTo((sbyte)100).Should().BeNegative();
        value.CompareTo((sbyte)10).Should().BePositive();
    }
    
    [Fact]
    public void CompareTo_WithUInt32_ShouldCompareCorrectly()
    {
        IntT20 value = 42;
        value.CompareTo((uint)42).Should().Be(0);
        value.CompareTo((uint)100).Should().BeNegative();
        value.CompareTo((uint)10).Should().BePositive();
    }
    
    [Fact]
    public void CompareTo_WithUInt64_ShouldCompareCorrectly()
    {
        IntT20 value = 42;
        value.CompareTo((ulong)42).Should().Be(0);
        value.CompareTo((ulong)100).Should().BeNegative();
        value.CompareTo((ulong)10).Should().BePositive();
    }
    
    [Fact]
    public void CompareTo_WithUInt16_ShouldCompareCorrectly()
    {
        IntT20 value = 42;
        value.CompareTo((ushort)42).Should().Be(0);
        value.CompareTo((ushort)100).Should().BeNegative();
        value.CompareTo((ushort)10).Should().BePositive();
    }
    
    [Fact]
    public void CompareTo_WithSingle_ShouldCompareCorrectly()
    {
        IntT20 value = 42;
        value.CompareTo(42.0f).Should().Be(0);
        value.CompareTo(100.0f).Should().BeNegative();
        value.CompareTo(10.0f).Should().BePositive();
    }
    
    [Fact]
    public void CompareTo_WithDouble_ShouldCompareCorrectly()
    {
        IntT20 value = 42;
        value.CompareTo(42.0).Should().Be(0);
        value.CompareTo(100.0).Should().BeNegative();
        value.CompareTo(10.0).Should().BePositive();
    }
    
    [Fact]
    public void CompareTo_WithDecimal_ShouldCompareCorrectly()
    {
        IntT20 value = 42;
        value.CompareTo(42.0m).Should().Be(0);
        value.CompareTo(100.0m).Should().BeNegative();
        value.CompareTo(10.0m).Should().BePositive();
    }
    
    [Fact]
    public void CompareTo_WithChar_ShouldCompareCorrectly()
    {
        IntT20 value = 42;
        value.CompareTo((char)42).Should().Be(0);
        value.CompareTo((char)100).Should().BeNegative();
        value.CompareTo((char)10).Should().BePositive();
    }
    
    [Fact]
    public void CompareTo_WithString_ShouldThrowArgumentException()
    {
        IntT20 value = 42;
        Action action = () => value.CompareTo("42");
        action.Should().Throw<ArgumentException>()
            .WithMessage("*Object is not a valid type*");
    }
}
