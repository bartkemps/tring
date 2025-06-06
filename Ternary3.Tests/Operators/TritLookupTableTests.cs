namespace Ternary3.Tests.Operators;

using FluentAssertions;
using Ternary3.Numbers;
using Ternary3.Operators;

public class TritLookupTableTests
{
    private const int T = -1;

    [Fact]
    public void LookupTritArray27Operator_CanUseIntArrayConstructor()
    {
        TritLookupTable op = new([
            [0, 1, 1],
            [1, T, T],
            [1, T, T]]);

        Int27T operand1 = 7268;
        Int27T operand2 = 4387;
        TritArray27 trits1 = operand1;
        TritArray27 trits2 = operand2;
        var result1 = 7268 | op | 4387;
        var result2 = operand1 | op | operand2;
        var result3 = trits1 | op | trits2;

        result1.Should().Be(result2);
        result1.Should().Be(result3);
    }
}