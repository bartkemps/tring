using FluentAssertions;
using Tring.Numbers;
using Tring.Operators;
using Xunit;

namespace Tring.Tests.Numbers;

public class TritArray27BinaryOperationsTests
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
    public void And_SingleTrit_MatchesTritOperation(sbyte trit1Value, sbyte trit2Value)
    {
        var array1 = new TritArray27();
        var array2 = new TritArray27();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        array1[0] = trit1;
        array2[0] = trit2;

        var result = array1 | BinaryLookup.And | array2;
        var expected = trit1 | BinaryLookup.And | trit2;

        result[0].Should().Be(expected, $"because {trit1} AND {trit2} should equal {expected}");
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
    public void Or_SingleTrit_MatchesTritOperation(sbyte trit1Value, sbyte trit2Value)
    {
        var array1 = new TritArray27();
        var array2 = new TritArray27();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        array1[0] = trit1;
        array2[0] = trit2;

        var result = array1 | BinaryLookup.Or | array2;
        var expected = trit1 | BinaryLookup.Or | trit2;

        result[0].Should().Be(expected, $"because {trit1} OR {trit2} should equal {expected}");
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
    public void Xor_SingleTrit_MatchesTritOperation(sbyte trit1Value, sbyte trit2Value)
    {
        var array1 = new TritArray27();
        var array2 = new TritArray27();
        var trit1 = new Trit(trit1Value);
        var trit2 = new Trit(trit2Value);
        array1[0] = trit1;
        array2[0] = trit2;

        var result = array1 | BinaryLookup.Xor | array2;
        var expected = trit1 | BinaryLookup.Xor | trit2;

        result[0].Should().Be(expected, $"because {trit1} XOR {trit2} should equal {expected}");
    }

    [Fact]
    public void BinaryOperations_PreserveOtherTrits()
    {
        var array1 = new TritArray27();
        var array2 = new TritArray27();

        // Set some test values
        array1[1] = Trit.Positive;
        array1[2] = Trit.Negative;
        array2[1] = Trit.Negative;
        array2[2] = Trit.Zero;

        var result = array1 | BinaryLookup.And | array2;

        // Check position 0 (unset in both arrays)
        result[0].Should().Be(Trit.Zero, "because unset positions should remain Zero");
        
        // Check positions we set
        var expected1 = array1[1] | BinaryLookup.And | array2[1];
        var expected2 = array1[2] | BinaryLookup.And | array2[2];
        result[1].Should().Be(expected1, "because it should match individual trit operation");
        result[2].Should().Be(expected2, "because it should match individual trit operation");
        
        // Check that all other positions remain Zero
        for (var i = 3; i < array1.Length; i++)
        {
            result[i].Should().Be(Trit.Zero, $"because position {i} should remain Zero");
        }
    }

    [Fact]
    public void BinaryOperations_OperateOnAllPositions()
    {
        var array1 = new TritArray27();
        var array2 = new TritArray27();
        var operations = new[] { BinaryLookup.And, BinaryLookup.Or, BinaryLookup.Xor, BinaryLookup.Plus, BinaryLookup.Minus };

        // Set all positions with alternating values
        for (var i = 0; i < array1.Length; i++)
        {
            array1[i] = i % 3 == 0 ? Trit.Positive : (i % 3 == 1 ? Trit.Zero : Trit.Negative);
            array2[i] = i % 3 == 0 ? Trit.Negative : (i % 3 == 1 ? Trit.Positive : Trit.Zero);
        }

        foreach (var operation in operations)
        {
            var result = array1 | operation | array2;

            for (var i = 0; i < array1.Length; i++)
            {
                var expected = array1[i] | operation | array2[i];
                result[i].Should().Be(expected, 
                    $"because {array1[i]} {operation} {array2[i]} at position {i} should equal {expected}");
            }
        }
    }
}
