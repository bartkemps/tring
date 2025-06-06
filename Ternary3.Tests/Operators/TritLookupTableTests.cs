namespace Ternary3.Tests.Operators;

using FluentAssertions;
using Ternary3.Numbers;
using Ternary3.Operators;

public class TritLookupTableTests
{
    [Fact]
    public void LookupTritArray27Operator_Constructor_WithTritLookupTable_CreatesInstance()
    {
        var trits = new TritArray27();
        var op = new TritLookupTable(new Trit[,]
        {
            { null, true, true },
            { true, false, false },
            { true, false, false }
        });

        Int27T operand= 4387;

        var result1 = 7268 | op | operand;
        var result2 = operand | op | (Int27T)7268;
        
        result1.Should().Be(result2);
    }
}