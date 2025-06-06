using FluentAssertions;
using Ternary3.Numbers;
using Ternary3.Numbers.TritArrays;
using Ternary3.Operators.Operations;

namespace Ternary3.Tests.Operators.Operations;

public class BinaryOperationBuilderTests
{
    [Theory]
    [InlineData(-1, -1)] 
    [InlineData(-1, 0)]
    [InlineData(-1, 1)]
    [InlineData(0, -1)] 
    [InlineData(0, 0)]
    [InlineData(0, 1)] 
    [InlineData(1, -1)] 
    [InlineData(1, 0)]
    [InlineData(1, 1)] 
    public void Build_GeneratesOperation_ForUInt32(sbyte trit1Value, sbyte trit2Value)
    {
        var xor = new TritLookupTable(
            null, false, true,
            false, null, true,
            true, true, null
        );
        var builder = new BinaryOperationBuilder<uint>(xor);
        var operation = builder.Build();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        var expected = (trit1 | xor | trit2);
        var negative1 = trit1Value == -1 ? 1u : 0u;
        var positive1 = trit1Value == 1 ? 1u : 0u;
        var negative2 = trit2Value == -1 ? 1u : 0u;
        var positive2 = trit2Value == 1 ? 1u : 0u;

        operation(negative1, positive1, negative2, positive2, out var negativeResult, out var positiveResult);

        var actual = TritConverter.GetTrit(negativeResult, positiveResult, 0);
        actual.Value.Should().Be(expected.Value, $"because {trit1} XOR {trit2} should equal {expected}");
    }
    
    [Theory]
    [InlineData(-1, -1)] 
    [InlineData(-1, 0)]
    [InlineData(-1, 1)]
    [InlineData(0, -1)] 
    [InlineData(0, 0)]
    [InlineData(0, 1)] 
    [InlineData(1, -1)] 
    [InlineData(1, 0)]
    [InlineData(1, 1)] 
    public void Build_GeneratesOperation_ForUInt64(sbyte trit1Value, sbyte trit2Value)
    {
        var addWithoutOverflow = new TritLookupTable(
            false, false, null,
            false, null, true,
            null, true, true
        );
        var builder = new BinaryOperationBuilder<ulong>(addWithoutOverflow);
        var operation = builder.Build();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        var expected = (trit1 | addWithoutOverflow | trit2);
        var negative1 = trit1Value == -1 ? 1ul : 0ul;
        var positive1 = trit1Value == 1 ? 1ul : 0ul;
        var negative2 = trit2Value == -1 ? 1ul : 0ul;
        var positive2 = trit2Value == 1 ? 1ul : 0ul;

        operation(negative1, positive1, negative2, positive2, out var negativeResult, out var positiveResult);

        var actual = TritConverter.GetTrit(negativeResult, positiveResult, 0);
        actual.Value.Should().Be(expected.Value, $"because {trit1} XOR {trit2} should equal {expected}");
    }
    
    [Theory]
    [InlineData(-1, -1)] 
    [InlineData(-1, 0)]
    [InlineData(-1, 1)]
    [InlineData(0, -1)] 
    [InlineData(0, 0)]
    [InlineData(0, 1)] 
    [InlineData(1, -1)] 
    [InlineData(1, 0)]
    [InlineData(1, 1)] 
    public void Build_GeneratesOperation_ForByte(sbyte trit1Value, sbyte trit2Value)
    {
        var substract = new TritLookupTable(
            null, false, false,
            true, null, false,
            true, true, null
        );
        var builder = new BinaryOperationBuilder<byte>(substract);
        var operation = builder.Build();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        var expected = trit1 | substract | trit2;
        var negative1 = (byte)(trit1Value == -1 ? 1 : 0);
        var positive1 = (byte)(trit1Value == 1 ? 1 : 0);
        var negative2 = (byte)(trit2Value == -1 ? 1 : 0);
        var positive2 = (byte)(trit2Value == 1 ? 1 : 0);

        operation(negative1, positive1, negative2, positive2, out var negativeResult, out var positiveResult);

        var actual = TritConverter.GetTrit(negativeResult, positiveResult, 0);
        actual.Value.Should().Be(expected.Value, $"because {trit1} XOR {trit2} should equal {expected}");
    }
}