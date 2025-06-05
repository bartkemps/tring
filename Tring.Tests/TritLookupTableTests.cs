using FluentAssertions;
using Tring.Numbers;

namespace Tring.Tests;

public class TritLookupTableTests
{
    [Fact]
    public void DefaultConstructor_ShouldCreateEmptyTable()
    {
        var table = new TritLookupTable();

        // All lookups should return 0
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                table[new Trit((sbyte)i), new Trit((sbyte)j)].Value.Should().Be(0);
            }
        }
    }

    [Fact]
    public void ArrayConstructor_ShouldThrowOnInvalidSize()
    {
        var invalidTable = new Trit[2, 2];
        var action = () => new TritLookupTable(invalidTable);
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void SpanConstructor_ShouldThrowOnInvalidSize()
    {
        var invalidSpan = new Trit[] { new(-1), new(0), new(1), new(-1) };
        var action = () => new TritLookupTable(invalidSpan);
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ArrayConstructor_ShouldCorrectlyInitializeTable()
    {
        var sourceTable = new Trit[,]
        {
            { false, true, null },
            { null, false, true },
            { true, null, null }
        };

        var table = new TritLookupTable(sourceTable);

        table[false, false].Should().Be(Trit.Negative);
        table[null, false].Should().Be(Trit.Zero);
        table[true, false].Should().Be(Trit.Positive);
    }

    [Fact]
    public void SpanConstructor_ShouldCorrectlyInitializeTable()
    {
        var sourceArray = new Trit[]
        {
            new(-1), new(0), new(1),
            new(-1), new(0), new(1),
            new(-1), new(0), new(1)
        };

        var table = new TritLookupTable(sourceArray);

        table[false, false].Should().Be(Trit.Negative);
        table[false, null].Should().Be(Trit.Negative);
        table[false, true].Should().Be(Trit.Negative);
        table[null, false].Should().Be(Trit.Zero);
        table[null, null].Should().Be(Trit.Zero);
        table[null, true].Should().Be(Trit.Zero);
        table[true, false].Should().Be(Trit.Positive);
        table[true, null].Should().Be(Trit.Positive);
        table[true, true].Should().Be(Trit.Positive);

    }

    [Fact]
    public void GetTrit_ShouldReturnCorrectValues()
    {
        var sourceArray = new Trit[]
        {
            new(1), new(0), new(-1),
            new(0), new(1), new(0),
            new(-1), new(0), new(1)
        };

        var table = new TritLookupTable(sourceArray);

        // Test some specific combinations
        table[new Trit(-1), new Trit(-1)].Should().Be(new Trit(1));
        table[new Trit(0), new Trit(0)].Should().Be(new Trit(1));
        table[new Trit(1), new Trit(1)].Should().Be(new Trit(1));
        table[new Trit(-1), new Trit(1)].Should().Be(new Trit(-1));
        table[new Trit(1), new Trit(-1)].Should().Be(new Trit(-1));
    }

    [Theory]
    [InlineData(-1, -1, 0)]
    [InlineData(-1, 0, 0)]
    [InlineData(-1, 1, 0)]
    [InlineData(0, -1, 0)]
    [InlineData(0, 0, 0)]
    [InlineData(0, 1, 0)]
    [InlineData(1, -1, 0)]
    [InlineData(1, 0, 0)]
    [InlineData(1, 1, 0)]
    public void Indexer_ShouldWorkWithAllTritCombinations(sbyte left, sbyte right, sbyte expected)
    {
        var table = new TritLookupTable(); // Default constructor creates table with all zeros
        table[new Trit(left), new Trit(right)].Should().Be(new Trit(expected));
    }

    [Fact]
    public void Indexer_SetShouldModifyOnlyTargetedValue()
    {
        var table = new TritLookupTable
        {
            // Modify a single position
            [false, null] = true
        };

        // Verify only the target position changed
        table[false, null].Should().Be(Trit.Positive);
        table[false, false].Should().Be(Trit.Zero);
        table[null, null].Should().Be(Trit.Zero);
        table[true, true].Should().Be(Trit.Zero);
    }

    [Fact]
    public void IndividualTritsConstructor_ShouldInitializeCorrectly()
    {
        var table = new TritLookupTable(
            new Trit(1), new Trit(0), new Trit(-1),
            new Trit(0), new Trit(1), new Trit(0),
            new Trit(-1), new Trit(0), new Trit(1)
        );

        // Check diagonal values
        table[new Trit(-1), new Trit(-1)].Should().Be(new Trit(1));
        table[new Trit(0), new Trit(0)].Should().Be(new Trit(1));
        table[new Trit(1), new Trit(1)].Should().Be(new Trit(1));

        // Check some off-diagonal values
        table[new Trit(-1), new Trit(0)].Should().Be(new Trit(0));
        table[new Trit(-1), new Trit(1)].Should().Be(new Trit(-1));
        table[new Trit(0), new Trit(-1)].Should().Be(new Trit(0));
    }

    [Fact]
    public void ImplicitConversion_FromArray_ShouldCreateEquivalentTable()
    {
        var sourceArray = new Trit[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                sourceArray[i, j] = new Trit((sbyte)((i + j) % 3 - 1));
            }
        }

        TritLookupTable table = sourceArray; // implicit conversion

        // Verify the conversion maintained all values
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                table[new Trit((sbyte)(i - 1)), new Trit((sbyte)(j - 1))]
                    .Should().Be(sourceArray[i, j]);
            }
        }
    }
}