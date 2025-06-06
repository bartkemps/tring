using FluentAssertions;
using Tring.Numbers;
using Tring.Operators;
using Tring.Operators.Operations;
using Tring.Numbers.TritArrays;
using Xunit;
using static Tring.Operators.BinaryLookup;

namespace Tring.Tests.Operators;

public class BinaryLookupTests
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
    public void And_MatchesTruthTable(sbyte trit1Value, sbyte trit2Value)
    {
        var builder = new BinaryOperationBuilder<uint>(And);
        var operation = builder.Build();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        var expected = trit1 | And | trit2;
        var negative1 = trit1Value == -1 ? 1u : 0u;
        var positive1 = trit1Value == 1 ? 1u : 0u;
        var negative2 = trit2Value == -1 ? 1u : 0u;
        var positive2 = trit2Value == 1 ? 1u : 0u;

        operation(negative1, positive1, negative2, positive2, out uint negativeResult, out uint positiveResult);

        var actual = TritConverter.GetTrit(ref negativeResult, ref positiveResult, 0);
        actual.Should().Be(expected, $"because {trit1} AND {trit2} should equal {expected}");
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
    public void Or_MatchesTruthTable(sbyte trit1Value, sbyte trit2Value)
    {
        var builder = new BinaryOperationBuilder<uint>(Or);
        var operation = builder.Build();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        var expected = trit1 | Or | trit2;
        var negative1 = trit1Value == -1 ? 1u : 0u;
        var positive1 = trit1Value == 1 ? 1u : 0u;
        var negative2 = trit2Value == -1 ? 1u : 0u;
        var positive2 = trit2Value == 1 ? 1u : 0u;

        operation(negative1, positive1, negative2, positive2, out uint negativeResult, out uint positiveResult);

        var actual = TritConverter.GetTrit(ref negativeResult, ref positiveResult, 0);
        actual.Should().Be(expected, $"because {trit1} OR {trit2} should equal {expected}");
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
    public void Xor_MatchesTruthTable(sbyte trit1Value, sbyte trit2Value)
    {
        var builder = new BinaryOperationBuilder<uint>(Xor);
        var operation = builder.Build();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        var expected = trit1 | Xor | trit2;
        var negative1 = trit1Value == -1 ? 1u : 0u;
        var positive1 = trit1Value == 1 ? 1u : 0u;
        var negative2 = trit2Value == -1 ? 1u : 0u;
        var positive2 = trit2Value == 1 ? 1u : 0u;

        operation(negative1, positive1, negative2, positive2, out uint negativeResult, out uint positiveResult);

        var actual = TritConverter.GetTrit(ref negativeResult, ref positiveResult, 0);
        actual.Should().Be(expected, $"because {trit1} XOR {trit2} should equal {expected}");
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
    public void Plus_MatchesTruthTable(sbyte trit1Value, sbyte trit2Value)
    {
        var builder = new BinaryOperationBuilder<uint>(Plus);
        var operation = builder.Build();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        var expected = trit1 | Plus | trit2;
        var negative1 = trit1Value == -1 ? 1u : 0u;
        var positive1 = trit1Value == 1 ? 1u : 0u;
        var negative2 = trit2Value == -1 ? 1u : 0u;
        var positive2 = trit2Value == 1 ? 1u : 0u;

        operation(negative1, positive1, negative2, positive2, out uint negativeResult, out uint positiveResult);

        var actual = TritConverter.GetTrit(ref negativeResult, ref positiveResult, 0);
        actual.Should().Be(expected, $"because {trit1} + {trit2} should equal {expected}");
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
    public void Minus_MatchesTruthTable(sbyte trit1Value, sbyte trit2Value)
    {
        var builder = new BinaryOperationBuilder<uint>(Minus);
        var operation = builder.Build();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        var expected = trit1 | Minus | trit2;
        var negative1 = trit1Value == -1 ? 1u : 0u;
        var positive1 = trit1Value == 1 ? 1u : 0u;
        var negative2 = trit2Value == -1 ? 1u : 0u;
        var positive2 = trit2Value == 1 ? 1u : 0u;

        operation(negative1, positive1, negative2, positive2, out uint negativeResult, out uint positiveResult);

        var actual = TritConverter.GetTrit(ref negativeResult, ref positiveResult, 0);
        actual.Should().Be(expected, $"because {trit1} - {trit2} should equal {expected}");
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
    public void Implicates_MatchesTruthTable(sbyte trit1Value, sbyte trit2Value)
    {
        var builder = new BinaryOperationBuilder<uint>(Implicates);
        var operation = builder.Build();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        var expected = trit1 | Implicates | trit2;
        var negative1 = trit1Value == -1 ? 1u : 0u;
        var positive1 = trit1Value == 1 ? 1u : 0u;
        var negative2 = trit2Value == -1 ? 1u : 0u;
        var positive2 = trit2Value == 1 ? 1u : 0u;

        operation(negative1, positive1, negative2, positive2, out uint negativeResult, out uint positiveResult);

        var actual = TritConverter.GetTrit(ref negativeResult, ref positiveResult, 0);
        actual.Should().Be(expected, $"because {trit1} ==> {trit2} should equal {expected}");
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
    public void Is_MatchesTruthTable(sbyte trit1Value, sbyte trit2Value)
    {
        var builder = new BinaryOperationBuilder<uint>(Is);
        var operation = builder.Build();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        var expected = trit1 | Is | trit2;
        var negative1 = trit1Value == -1 ? 1u : 0u;
        var positive1 = trit1Value == 1 ? 1u : 0u;
        var negative2 = trit2Value == -1 ? 1u : 0u;
        var positive2 = trit2Value == 1 ? 1u : 0u;

        operation(negative1, positive1, negative2, positive2, out uint negativeResult, out uint positiveResult);

        var actual = TritConverter.GetTrit(ref negativeResult, ref positiveResult, 0);
        actual.Should().Be(expected, $"because {trit1} == {trit2} should equal {expected}");
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
    public void GreaterThan_MatchesTruthTable(sbyte trit1Value, sbyte trit2Value)
    {
        var builder = new BinaryOperationBuilder<uint>(GreaterThan);
        var operation = builder.Build();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        var expected = trit1 | GreaterThan | trit2;
        var negative1 = trit1Value == -1 ? 1u : 0u;
        var positive1 = trit1Value == 1 ? 1u : 0u;
        var negative2 = trit2Value == -1 ? 1u : 0u;
        var positive2 = trit2Value == 1 ? 1u : 0u;

        operation(negative1, positive1, negative2, positive2, out uint negativeResult, out uint positiveResult);

        var actual = TritConverter.GetTrit(ref negativeResult, ref positiveResult, 0);
        actual.Should().Be(expected, $"because {trit1} > {trit2} should equal {expected}");
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
    public void LesserThan_MatchesTruthTable(sbyte trit1Value, sbyte trit2Value)
    {
        var builder = new BinaryOperationBuilder<uint>(LesserThan);
        var operation = builder.Build();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        var expected = trit1 | LesserThan | trit2;
        var negative1 = trit1Value == -1 ? 1u : 0u;
        var positive1 = trit1Value == 1 ? 1u : 0u;
        var negative2 = trit2Value == -1 ? 1u : 0u;
        var positive2 = trit2Value == 1 ? 1u : 0u;

        operation(negative1, positive1, negative2, positive2, out uint negativeResult, out uint positiveResult);

        var actual = TritConverter.GetTrit(ref negativeResult, ref positiveResult, 0);
        actual.Should().Be(expected, $"because {trit1} < {trit2} should equal {expected}");
    }
}
