using FluentAssertions;
using Tring.Numbers;
using Tring.Numbers.TritArrays;
using Tring.Operators.Operations;

namespace Tring.Tests.Operators.Operations;

public class BinaryOperationBuilderTests
{
    [Theory]
    [InlineData(-1, -1, 0)] // -1 XOR -1 = 0
    [InlineData(-1, 0, -1)] // -1 XOR 0 = -1
    [InlineData(-1, 1, 1)]  // -1 XOR 1 = 1
    [InlineData(0, -1, -1)] // 0 XOR -1 = -1
    [InlineData(0, 0, 0)]   // 0 XOR 0 = 0
    [InlineData(0, 1, 1)]   // 0 XOR 1 = 1
    [InlineData(1, -1, 1)]  // 1 XOR -1 = 1
    [InlineData(1, 0, 1)]   // 1 XOR 0 = 1
    [InlineData(1, 1, 0)]   // 1 XOR 1 = 0
    public void XorOperationBuilderTest(sbyte trit1Value, sbyte trit2Value, sbyte expectedValue)
    {
        var xorTable = new TritLookupTable(
            null, false, true,
            false, null, true,
            true, true, null
        );
        var builder = new BinaryOperationBuilder(xorTable);
        var operation = builder.Build();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        var negative1 = trit1Value == -1 ? 1u : 0u;
        var positive1 = trit1Value == 1 ? 1u : 0u;
        var negative2 = trit2Value == -1 ? 1u : 0u;
        var positive2 = trit2Value == 1 ? 1u : 0u;

        operation(negative1, positive1, negative2, positive2, out uint negativeResult, out uint positiveResult);

        var resultTrit = TritConverter.GetTrit(ref positiveResult, ref negativeResult, 0);
        resultTrit.Value.Should().Be(expectedValue, $"because {trit1} XOR {trit2} should equal {new Trit(expectedValue)}");
    }
}