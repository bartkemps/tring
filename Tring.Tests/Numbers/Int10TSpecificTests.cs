namespace Tring.Tests.Numbers;

using FluentAssertions;
using Tring.Numbers;
using System.Globalization;
using Xunit;
using System;

public class Int10TSpecificTests
{
    [Fact]
    public void Constants_ShouldHaveCorrectValues()
    {
        Int10T.MaxValue.Should().Be((Int10T)29524);
        Int10T.MinValue.Should().Be((Int10T)(-29524));
    }
    

  
    [Fact]
    public void Int10T_ShouldHandleSpecificInt16Operations()
    {
        // Test operations with short values
        short shortValue = 100;
        Int10T ternaryValue = 200;
        
        // Mixed operations with short
        (ternaryValue + shortValue).Should().Be((Int10T)300);
        (shortValue + ternaryValue).Should().Be((Int10T)300);
        (ternaryValue - shortValue).Should().Be((Int10T)100);
        (shortValue - ternaryValue).Should().Be((Int10T)(-100));
        
        // Using int operations but producing Int10T results
        int intValue = 5000;
        Int10T result = (Int10T)(intValue * (int)ternaryValue);
        result.Should().Be((Int10T)(intValue * 200));
    }
}
