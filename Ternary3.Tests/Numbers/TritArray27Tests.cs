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

    // [Fact]
    // public void Negation_Works()
    // {
    //     var a = new TritArray27();
    //     a[0] = Trit.Positive;
    //     a[1] = Trit.Negative;
    //     a[2] = Trit.Zero;
    //
    //     var result = -a;
    //
    //     Assert.Equal(Trit.Negative, result[0]);
    //     Assert.Equal(Trit.Positive, result[1]);
    //     Assert.Equal(Trit.Zero, result[2]);
    // }

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
}