namespace Ternary3.Tests.Numbers;

using FluentAssertions;

public class TritArray27Tests
{
    [Fact]
    public void SetAndGetTritValues_WorksCorrectly()
    {
        var array = new TritArray27();
        // Set all to Zero, then set some to Positive and Negative
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = Trit.Zero;
        }

        array[0] = Trit.Positive;
        array[1] = Trit.Negative;
        array[2] = Trit.Positive;
        array[26] = Trit.Negative;

        Assert.Equal(Trit.Positive, array[0]);
        Assert.Equal(Trit.Negative, array[1]);
        Assert.Equal(Trit.Positive, array[2]);
        Assert.Equal(Trit.Negative, array[26]);
        // All others should be Zero
        for (var i = 3; i < 26; i++)
        {
            Assert.Equal(Trit.Zero, array[i]);
        }
    }

    [Fact]
    public void Addition_BasicOperations_WorkCorrectly()
    {
        var a = new TritArray27();
        var b = new TritArray27();

        // Test 1: 0 + 0 = 0
        var result = a + b;
        Assert.Equal(Trit.Zero, result[0]);

        // Test 2: 1 + 0 = 1
        a[0] = Trit.Positive;
        result = a + b;
        Assert.Equal(Trit.Positive, result[0]);

        // Test 3: -1 + 0 = -1
        a[0] = Trit.Negative;
        result = a + b;
        Assert.Equal(Trit.Negative, result[0]);

        // Test 4: 1 + 1 = -1 (wraps around)
        a[0] = Trit.Positive;
        b[0] = Trit.Positive;
        result = a + b;
        Assert.Equal(Trit.Negative, result[0]);

        // Test 5: -1 + -1 = 1 (wraps around)
        a[0] = Trit.Negative;
        b[0] = Trit.Negative;
        result = a + b;
        Assert.Equal(Trit.Positive, result[0]);

        // Test 6: 1 + -1 = 0
        a[0] = Trit.Positive;
        b[0] = Trit.Negative;
        result = a + b;
        Assert.Equal(Trit.Zero, result[0]);
    }

    [Fact]
    public void Addition_MultiplePositions_WorkCorrectly()
    {
        var a = new TritArray27();
        var b = new TritArray27();

        // Set values in multiple positions
        a[2] = Trit.Zero; // 0
        a[1] = Trit.Negative; // -1
        a[0] = Trit.Positive; // 1

        b[2] = Trit.Positive; // 1
        b[1] = Trit.Negative; // -1
        b[0] = Trit.Negative; // -1

        var result = a + b;

        Assert.Equal(Trit.Zero, result[2]); // 0 + 1 = 1
        Assert.Equal(Trit.Positive, result[1]); // -1 + -1 = 1 (wrap)
        Assert.Equal(Trit.Zero, result[0]); // 1 + -1 = 0
    }

    [Fact]
    public void Addition_AllPositions_PreservesZero()
    {
        var a = new TritArray27();
        var b = new TritArray27();

        // Set all positions to zero explicitly
        for (var i = 0; i < 27; i++)
        {
            a[i] = Trit.Zero;
            b[i] = Trit.Zero;
        }

        var result = a + b;

        // Check all positions are still zero
        for (var i = 0; i < 27; i++)
        {
            Assert.Equal(Trit.Zero, result[i]);
        }
    }

    [Fact]
    public void Addition_LargeValues_WrapsCorrectly()
    {
        var a = new TritArray27();
        var b = new TritArray27();

        a[1] = Trit.Positive; 
        a[2] = Trit.Positive; 
        b[1] = Trit.Positive; 
        b[2] = Trit.Positive; 

        var result = a + b;
        result[0].Should().Be(Trit.Zero);
        result[1].Should().Be(Trit.Negative); 
        result[2].Should().Be(Trit.Zero); 
        result[3].Should().Be(Trit.Positive); 
        result[4].Should().Be(Trit.Zero); 
    }

    private const sbyte T = -1;

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 0, 1)]
    [InlineData(T, 0, T)]
    [InlineData(1, 1, T)] // Wrap around
    [InlineData(T, T, 1)] // Wrap around
    [InlineData(1, T, 0)]
    public void Addition_Theory_WorksForAllBasicCases(sbyte aValue, sbyte bValue, sbyte expectedValue)
    {
        var a = (Trit)aValue;
        var b = (Trit)bValue;
        var expected = (Trit)expectedValue;

        var arrayA = new TritArray27();
        var arrayB = new TritArray27();

        arrayA[0] = a;
        arrayB[0] = b;

        var result = arrayA + arrayB;

        Assert.Equal(expected, result[0]);
    }

    [Fact]
    public void Subtraction_BasicOperations_WorkCorrectly()
    {
        var a = new TritArray27();
        var b = new TritArray27();

        // Test cases using subtraction
        a[0] = Trit.Positive;
        b[0] = Trit.Positive;
        var result = a - b;
        Assert.Equal(Trit.Zero, result[0]); // 1 - 1 = 0

        a[0] = Trit.Negative;
        b[0] = Trit.Positive;
        result = a - b;
        Assert.Equal(Trit.Negative, result[1]); // -1 - 1 = -1
        Assert.Equal(Trit.Positive, result[0]); // -1 - 1 = -1

        a[0] = Trit.Positive;
        b[0] = Trit.Negative;
        result = a - b;
        Assert.Equal(Trit.Positive, result[1]); // 1 - (-1) = 1
        Assert.Equal(Trit.Negative, result[0]); // 1 - (-1) = 1

        a[0] = Trit.Zero;
        b[0] = Trit.Positive;
        result = a - b;
        Assert.Equal(Trit.Negative, result[0]); // 0 - 1 = -1
    }

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 0, 1)]
    [InlineData(0, 1, T)]
    [InlineData(T, T, 0)]
    [InlineData(1, T, T)]
    public void Subtraction_Theory_WorksForAllBasicCases(sbyte aValue, sbyte bValue, sbyte expectedValue)
    {
        var a = (Trit)aValue;
        var b = (Trit)bValue;
        var expected = (Trit)expectedValue;

        // Create two TritArray27 instances
        var arrayA = new TritArray27();
        var arrayB = new TritArray27();

        arrayA[0] = a;
        arrayB[0] = b;

        var result = arrayA - arrayB;

        Assert.Equal(expected, result[0]);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(12)]
    [InlineData(13)]
    [InlineData(14)]
    [InlineData(15)]
    [InlineData(16)]
    [InlineData(17)]
    [InlineData(18)]
    [InlineData(19)]
    [InlineData(20)]
    [InlineData(21)]
    [InlineData(22)]
    [InlineData(23)]
    [InlineData(24)]
    [InlineData(25)]
    [InlineData(26)]
    public void Indexer_IndexFromEnd_GetsCorrectTrit(int fromEnd)
    {
        var arr = new TritArray27();
        for (var i = 0; i < arr.Length; i++)
            arr[i] = new((sbyte)((i % 3) - 1));
        var expected = arr[26 - fromEnd];
        var actual = arr[^ (fromEnd + 1)];
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, -1)]
    [InlineData(1, 0)]
    [InlineData(2, 1)]
    [InlineData(3, -1)]
    [InlineData(4, 0)]
    [InlineData(5, 1)]
    [InlineData(6, -1)]
    [InlineData(7, 0)]
    [InlineData(8, 1)]
    [InlineData(9, -1)]
    [InlineData(10, 0)]
    [InlineData(11, 1)]
    [InlineData(12, -1)]
    [InlineData(13, 0)]
    [InlineData(14, 1)]
    [InlineData(15, -1)]
    [InlineData(16, 0)]
    [InlineData(17, 1)]
    [InlineData(18, -1)]
    [InlineData(19, 0)]
    [InlineData(20, 1)]
    [InlineData(21, -1)]
    [InlineData(22, 0)]
    [InlineData(23, 1)]
    [InlineData(24, -1)]
    [InlineData(25, 0)]
    [InlineData(26, 1)]
    public void Indexer_IndexFromEnd_SetsCorrectTrit(int fromEnd, sbyte tritValue)
    {
        var arr = new TritArray27();
        arr[^ (fromEnd + 1)] = new(tritValue);
        arr[^ (fromEnd + 1)].Value.Should().Be(tritValue);
    }
}