namespace Tring.Tests.Operators;

using FluentAssertions;
using Tring.Numbers;
using Tring.Operators;

public class OperatorTests
{
    
    [Theory]
    [InlineData(-1, 1)]
    public void Negative_ExecutesCorrectly(sbyte value,sbyte expected)
    {
        var array = new TritArray27();
        var trit = (Trit)value;
        
        var actual = trit | Operator.Negative;
        actual.Should().Be((Trit)expected);
        
        actual = trit | UnaryOperation.Negative;
        actual.Should().Be((Trit)expected);
        
        array[1] = trit;
        var result = array | Operator.Negative;
        result[1].Should().Be((Trit)expected);

        array[1] = trit;
        result = array | UnaryOperation.Negative;
        result[1].Should().Be((Trit)expected);
    }
}