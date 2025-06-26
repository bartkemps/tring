using FluentAssertions;

namespace Ternary3.Tests;

using Ternary3.Operators;

public partial class BinaryTritOperatorTests
{
    [Fact]
    public void DefaultConstructor_ShouldCreateEmptyTable()
    {
        var table = new BinaryTritOperator();

        // All lookups should return 0
        for (var i = -1; i <= 1; i++)
        {
            for (var j = -1; j <= 1; j++)
            {
                table[new Trit((sbyte)i), new Trit((sbyte)j)].Value.Should().Be(0);
            }
        }
    }

    [Fact]
    public void ArrayConstructor_ShouldThrowOnInvalidSize()
    {
        var invalidTable = new Trit[2, 2];
        var action = () => new BinaryTritOperator(invalidTable);
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void SpanConstructor_ShouldThrowOnInvalidSize()
    {
        var invalidSpan = new Trit[] { new(-1), new(0), new(1), new(-1) };
        var action = () => new BinaryTritOperator(invalidSpan);
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

        var table = new BinaryTritOperator(sourceTable);

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

        var table = new BinaryTritOperator(sourceArray);

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

        var table = new BinaryTritOperator(sourceArray);

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
        var table = new BinaryTritOperator(); // Default constructor creates table with all zeros
        table[new Trit(left), new Trit(right)].Should().Be(new Trit(expected));
    }

    [Fact]
    public void Indexer_SetShouldModifyOnlyTargetedValue()
    {
        var table = new BinaryTritOperator
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
        var table = new BinaryTritOperator(
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
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                sourceArray[i, j] = new Trit((sbyte)((i + j) % 3 - 1));
            }
        }

        var table = new BinaryTritOperator(sourceArray);

        // Verify the conversion maintained all values
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                table[new Trit((sbyte)(i - 1)), new Trit((sbyte)(j - 1))]
                    .Should().Be(sourceArray[i, j]);
            }
        }
    }

    [Fact]
    public void FuncConstructor_ShouldInitializeCorrectly()
    {
        // Define a simple XOR operation
        Trit XorOperation(Trit a, Trit b)
        {
            if (a.Value == 0 || b.Value == 0) return new Trit(0);
            return a.Value == b.Value ? new Trit(-1) : new Trit(1);
        }

        var table = new BinaryTritOperator(XorOperation);

        // Test all combinations
        table[new Trit(-1), new Trit(-1)].Should().Be(new Trit(-1));
        table[new Trit(-1), new Trit(0)].Should().Be(new Trit(0));
        table[new Trit(-1), new Trit(1)].Should().Be(new Trit(1));
        table[new Trit(0), new Trit(-1)].Should().Be(new Trit(0));
        table[new Trit(0), new Trit(0)].Should().Be(new Trit(0));
        table[new Trit(0), new Trit(1)].Should().Be(new Trit(0));
        table[new Trit(1), new Trit(-1)].Should().Be(new Trit(1));
        table[new Trit(1), new Trit(0)].Should().Be(new Trit(0));
        table[new Trit(1), new Trit(1)].Should().Be(new Trit(-1));
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenTablesAreTheSame()
    {
        var table1 = new BinaryTritOperator(
            new Trit(1), new Trit(0), new Trit(-1),
            new Trit(0), new Trit(1), new Trit(0),
            new Trit(-1), new Trit(0), new Trit(1)
        );
        
        var table2 = new BinaryTritOperator(
            new Trit(1), new Trit(0), new Trit(-1),
            new Trit(0), new Trit(1), new Trit(0),
            new Trit(-1), new Trit(0), new Trit(1)
        );

        // Object.Equals
        table1.Equals((object)table2).Should().BeTrue();
        
        // IEquatable<BinaryTritOperator>.Equals
        table1.Equals(table2).Should().BeTrue();
        
        // == operator
        (table1 == table2).Should().BeTrue();
        
        // != operator
        (table1 != table2).Should().BeFalse();
        
        // GetHashCode
        table1.GetHashCode().Should().Be(table2.GetHashCode());
    }
    
    [Fact]
    public void Equals_ShouldReturnFalse_WhenTablesAreDifferent()
    {
        var table1 = new BinaryTritOperator(
            new Trit(1), new Trit(0), new Trit(-1),
            new Trit(0), new Trit(1), new Trit(0),
            new Trit(-1), new Trit(0), new Trit(1)
        );
        
        var table2 = new BinaryTritOperator(
            new Trit(-1), new Trit(0), new Trit(1),
            new Trit(0), new Trit(-1), new Trit(0),
            new Trit(1), new Trit(0), new Trit(-1)
        );

        // Object.Equals
        table1.Equals((object)table2).Should().BeFalse();
        
        // IEquatable<BinaryTritOperator>.Equals
        table1.Equals(table2).Should().BeFalse();
        
        // == operator
        (table1 == table2).Should().BeFalse();
        
        // != operator
        (table1 != table2).Should().BeTrue();
    }
    
    [Fact]
    public void Equals_ShouldReturnFalse_WhenComparedWithNull()
    {
        var table = new BinaryTritOperator();
        object? o = null;
        table.Equals(null).Should().BeFalse();
        table.Equals(o).Should().BeFalse();
    }
    
    [Fact]
    public void Equals_ShouldReturnFalse_WhenComparedWithDifferentType()
    {
        var table = new BinaryTritOperator();
        var otherObject = "Not a BinaryTritOperator";
        
        table.Equals(otherObject).Should().BeFalse();
    }
    
    [Fact]
    public void ToString_ShouldReturnFormattedTable()
    {
        var table = new BinaryTritOperator(
            new Trit(-1), new Trit(0), new Trit(1),
            new Trit(0), new Trit(-1), new Trit(0),
            new Trit(1), new Trit(0), new Trit(-1)
        );
        
        var result = table.ToString();
        
        result.Should().Contain("T 0 1");      // Header row
        result.Should().Contain("T | T 0 1");  // First data row
        result.Should().Contain("0 | 0 T 0");  // Second data row
        result.Should().Contain("1 | 1 0 T");  // Third data row
    }
    
    [Fact]
    public void DebugView_ShouldReturnCompactStringRepresentation()
    {
        var table = new BinaryTritOperator(
            new Trit(-1), new Trit(0), new Trit(1),
            new Trit(0), new Trit(-1), new Trit(0),
            new Trit(1), new Trit(0), new Trit(-1)
        );
        
        // Using reflection to access the internal DebugView method
        var debugViewMethod = typeof(BinaryTritOperator).GetMethod("DebugView", 
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        
        var result = debugViewMethod?.Invoke(table, null) as string;
        
        result.Should().NotBeNull();
        result.Should().Be("T 0 1 / 0 T 0 / 1 0 T");
    }
}