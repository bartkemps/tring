namespace Tring.Tests.Numbers;

using FluentAssertions;
using System.Globalization;
using Tring.Numbers;

public class Int32Tests
{
    [Fact]
    public void Constants_ShouldHaveCorrectValues()
    {
        int.MaxValue.Should().Be(2147483647);
        int.MinValue.Should().Be(-2147483648);
    }

    [Theory]
    [InlineData("0", 0)]
    [InlineData("42", 42)]
    [InlineData("-42", -42)]
    [InlineData("2147483647", int.MaxValue)]
    [InlineData("-2147483648", int.MinValue)]
    public void Parse_ValidStrings_ShouldReturnExpectedValue(string input, int expected)
    {
        int.Parse(input).Should().Be(expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    public void Parse_InvalidStrings_ShouldThrowFormatException(string input)
    {
        var action = () => int.Parse(input);
        action.Should().Throw<FormatException>();
    }

    [Theory]
    [InlineData("2147483648")]
    [InlineData("-2147483649")]
    public void Parse_InvalidStrings_ShouldThrowOverflowException(string input)
    {
        var action = () => int.Parse(input);
        action.Should().Throw<OverflowException>();
    }

    [Theory]
    [InlineData(" 42", 42)]  // Leading space
    [InlineData("42 ", 42)]  // Trailing space
    [InlineData(" 42 ", 42)] // Both
    [InlineData("+42", 42)]  // Explicit positive
    [InlineData("0042", 42)] // Leading zeros
    public void Parse_WithWhitespaceAndSigns_ShouldWorkCorrectly(string input, int expected)
    {
        int.Parse(input).Should().Be(expected);
    }

    [Theory]
    [InlineData("2A", NumberStyles.HexNumber, 42)]        // Hex without prefix
    [InlineData("2a", NumberStyles.HexNumber, 42)]        // Lowercase hex
    [InlineData("00002A", NumberStyles.HexNumber, 42)]    // Leading zeros
    public void Parse_HexadecimalValues_ShouldWorkCorrectly(string input, NumberStyles style, int expected)
    {
        int.Parse(input, style).Should().Be(expected);
    }

    [Fact]
    public void ArithmeticOperators_ShouldWorkCorrectly()
    {
        // Addition
        (5 + 3).Should().Be(8);
        var maxVal = int.MaxValue;
        var overflow = () => { checked { var _ = maxVal + 1; } };
        overflow.Should().Throw<OverflowException>();

        // Subtraction
        (5 - 3).Should().Be(2);
        var minVal = int.MinValue;
        var underflow = () => { checked { var _ = minVal - 1; } };
        underflow.Should().Throw<OverflowException>();

        // Multiplication
        (5 * 3).Should().Be(15);
        var mulOverflow = () => { checked { var _ = maxVal * 2; } };
        mulOverflow.Should().Throw<OverflowException>();

        // Division
        (6 / 2).Should().Be(3);
        var zero = 0;  // Using variable to avoid constant computation warning
        var divByZero = () => { _ = 5 / zero; };
        divByZero.Should().Throw<DivideByZeroException>();
    }

    [Fact]
    public void ComparisonOperators_ShouldWorkCorrectly()
    {
        // Using variables that could change to avoid constant value warnings
        var a = GetTestValue(10);
        var b = GetTestValue(3);
        
        (a > b).Should().BeTrue();
        (b < a).Should().BeTrue();
        (a >= b).Should().BeTrue();
        (b <= a).Should().BeTrue();
        (a != b).Should().BeTrue();
        
        var c = a;  // Testing equality with same value
        (a == c).Should().BeTrue();
    }

    private static int GetTestValue(int value) => value;  // Helper to avoid constant value warnings

    [Theory]
    [InlineData(0, "0")]
    [InlineData(42, "42")]
    [InlineData(-42, "-42")]
    [InlineData(2147483647, "2147483647")]
    [InlineData(-2147483648, "-2147483648")]
    public void ToString_ShouldReturnCorrectString(int input, string expected)
    {
        input.ToString().Should().Be(expected);
    }

    [Theory]
    [InlineData("F0", "42", 42)]
    [InlineData("D5", "00042", 42)]
    [InlineData("X", "2A", 42)]
    [InlineData("X8", "0000002A", 42)]
    public void ToString_WithFormat_ShouldReturnCorrectString(string format, string expected, int input)
    {
        input.ToString(format).Should().Be(expected);
    }

    [Fact]
    public void Parse_WithCulture_ShouldWorkCorrectly()
    {
        var germanCulture = new CultureInfo("de-DE");
        var americanCulture = new CultureInfo("en-US");

        // German culture uses period as thousand separator and comma as decimal
        int.Parse("1.000", NumberStyles.AllowThousands, germanCulture).Should().Be(1000);
        
        // US culture uses comma as thousand separator
        int.Parse("1,000", NumberStyles.AllowThousands, americanCulture).Should().Be(1000);
    }

    [Fact]
    public void CompareTo_ShouldWorkCorrectly()
    {
        var a = 42;
        var b = 24;
        
        a.CompareTo(b).Should().BePositive();
        b.CompareTo(a).Should().BeNegative();
        a.CompareTo(a).Should().Be(0);
    }

    [Fact]
    public void MinMax_ShouldReturnCorrectValue()
    {
        var x = GetTestValue(5);
        var y = GetTestValue(3);
        Math.Min(x, y).Should().Be(3);
        Math.Max(x, y).Should().Be(5);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(42, true)]  // 42 is even
    [InlineData(-42, true)] // -42 is even
    public void IsEven_ShouldReturnCorrectValue(int value, bool expected)
    {
        var isEven = value % 2 == 0;
        isEven.Should().Be(expected);
    }

    [Fact]
    public void Abs_ShouldReturnCorrectValue()
    {
        var negativeValue = GetTestValue(-42);
        Math.Abs(negativeValue).Should().Be(42);
        
        // MinValue overflow case
        var minValue = int.MinValue;
        var action = () => Math.Abs(minValue);
        action.Should().Throw<OverflowException>();
    }

    [Theory]
    [InlineData(NumberStyles.Integer, "42", 42)]
    [InlineData(NumberStyles.HexNumber, "2A", 42)]
    [InlineData(NumberStyles.AllowThousands, "1,000", 1000)]
    public void Parse_WithNumberStyles_ShouldWorkCorrectly(NumberStyles style, string input, int expected)
    {
        int.Parse(input, style).Should().Be(expected);
    }

    [Fact]
    public void BitwiseOperators_ShouldWorkCorrectly()
    {
        // AND
        (5 & 3).Should().Be(1);
        
        // OR
        (5 | 3).Should().Be(7);
        
        // XOR
        (5 ^ 3).Should().Be(6);
        
        // NOT
        (~5).Should().Be(-6);
        
        // Left shift
        (1 << 2).Should().Be(4);
        
        // Right shift
        (4 >> 2).Should().Be(1);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(42)]
    [InlineData(-42)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void GetHashCode_ShouldBeConsistent(int value)
    {
        var hash1 = value.GetHashCode();
        var hash2 = value.GetHashCode();
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void TryParse_ShouldHandleValidAndInvalidInput()
    {
        int result;
        int.TryParse("42", out result).Should().BeTrue();
        result.Should().Be(42);

        int.TryParse("invalid", out result).Should().BeFalse();
        result.Should().Be(0);  // Default value when parsing fails
    }

    [Fact]
    public void ImplicitConversions_ToInt64_ShouldWorkCorrectly()
    {
        var value = 42;
        long longValue = value; // Implicit conversion to Int64
        longValue.Should().Be(42L);

        value = -42;
        longValue = value;
        longValue.Should().Be(-42L);

        value = int.MaxValue;
        longValue = value;
        longValue.Should().Be(2147483647L);

        value = int.MinValue;
        longValue = value;
        longValue.Should().Be(-2147483648L);
    }

    [Fact]
    public void ExplicitConversions_FromInt64_ShouldWorkCorrectly()
    {
        // Valid conversions
        var withinRange = 42L;
        var intValue = (int)withinRange;
        intValue.Should().Be(42);

        // Overflow cases
        checked
        {
            var tooLarge = long.MaxValue;
            var largeConversion = () => { var _ = (int)tooLarge; };
            largeConversion.Should().Throw<OverflowException>();

            var tooSmall = long.MinValue;
            var smallConversion = () => { var _ = (int)tooSmall; };
            smallConversion.Should().Throw<OverflowException>();
        }
    }

    [Fact]
    public void ExplicitConversions_ToInt16_ShouldWorkCorrectly()
    {
        // Valid conversions
        var smallValue = 42;
        var shortValue = (short)smallValue;
        shortValue.Should().Be((short)42);

        // Overflow cases
        checked
        {
            var tooLarge = 32768; // Greater than short.MaxValue
            var largeConversion = () => { var _ = (short)tooLarge; };
            largeConversion.Should().Throw<OverflowException>();

            var tooSmall = -32769; // Less than short.MinValue
            var smallConversion = () => { var _ = (short)tooSmall; };
            smallConversion.Should().Throw<OverflowException>();
        }
    }

    [Fact]
    public void ImplicitConversions_FromInt16_ShouldWorkCorrectly()
    {
        short value = 42;
        int intValue = value; // Implicit conversion from Int16
        intValue.Should().Be(42);

        value = -42;
        intValue = value;
        intValue.Should().Be(-42);

        value = short.MaxValue;
        intValue = value;
        intValue.Should().Be(32767);

        value = short.MinValue;
        intValue = value;
        intValue.Should().Be(-32768);
    }

    [Fact]
    public void UncheckedConversions_ShouldTruncateValues()
    {
        unchecked
        {
            // Int64 to Int32
            var large = int.MaxValue + 1L;
            var truncated = (int)large;
            truncated.Should().Be(int.MinValue); // Wraps around to minimum value

            // Int32 to Int16
            var tooBig = short.MaxValue + 1;
            var truncatedShort = (short)tooBig;
            truncatedShort.Should().Be(short.MinValue); // Wraps around to minimum value
        }
    }

    [Fact]
    public void CheckedCasts_ShouldThrowOnOverflow()
    {
        checked
        {
            // Int64 to Int32
            var tooLarge = (long)int.MaxValue + 1;
            var largeAction = () => { var _ = (int)tooLarge; };
            largeAction.Should().Throw<OverflowException>();

            // Int32 to Int16
            var tooBig = (int)short.MaxValue + 1;
            var shortAction = () => { var _ = (short)tooBig; };
            shortAction.Should().Throw<OverflowException>();
        }
    }

    [Fact]
    public void UncheckedCasts_ShouldTruncateSilently()
    {
        unchecked
        {
            // Int64 to Int32
            var tooLarge = (long)int.MaxValue + 1;
            var truncated = (int)tooLarge;
            truncated.Should().Be(int.MinValue); // Wraps around

            // Int32 to Int16
            var tooBig = (int)short.MaxValue + 1;
            var truncatedShort = (short)tooBig;
            truncatedShort.Should().Be(short.MinValue); // Wraps around
        }
    }

    [Fact]
    public void DefaultContext_ShouldBeUnchecked()
    {
        // By default, C# uses unchecked context for casts
        var tooLarge = (long)int.MaxValue + 1;
        var truncated = (int)tooLarge;
        truncated.Should().Be(int.MinValue); // Should wrap around without throwing

        var tooBig = (int)short.MaxValue + 1;
        var truncatedShort = (short)tooBig;
        truncatedShort.Should().Be(short.MinValue); // Should wrap around without throwing
    }

    [Fact]
    public void DivRem_ShouldWorkCorrectly()
    {
        var dividend = 7;
        var divisor = 3;
        int remainder;
        var quotient = Math.DivRem(dividend, divisor, out remainder);
        
        quotient.Should().Be(2);
        remainder.Should().Be(1);

        // Edge case - MinValue divided by -1 causes overflow
        var action = () => 
        {
            checked
            {
                _ = Math.DivRem(int.MinValue, -1, out remainder);
            }
        };
        action.Should().Throw<OverflowException>();
    }

    [Theory]
    [InlineData("2A", NumberStyles.HexNumber, 42)]        // Hex without prefix
    [InlineData("2a", NumberStyles.HexNumber, 42)]        // Lowercase hex
    [InlineData("00002A", NumberStyles.HexNumber, 42)]    // Leading zeros
    [InlineData("+123", NumberStyles.Integer, 123)]       // Explicit positive
    [InlineData("123,456", NumberStyles.AllowThousands, 123456)]  // Thousands separator
    public void Parse_SpecialFormats_ShouldWorkCorrectly(string input, NumberStyles style, int expected)
    {
        int.Parse(input, style).Should().Be(expected);
    }

    [Fact]
    public void Parse_CurrencyFormat_ShouldWorkCorrectly()
    {
        var usCulture = new CultureInfo("en-US");
        int.Parse("$42", NumberStyles.Currency, usCulture).Should().Be(42);
        int.Parse(" 42 ", NumberStyles.Currency, usCulture).Should().Be(42);
    }

    [Theory]
    [InlineData(int.MaxValue, int.MinValue, -1)]  // MaxValue + MinValue = -1
    [InlineData(int.MinValue, int.MinValue, 0)]   // MinValue + MinValue = 0 (overflow)
    [InlineData(int.MaxValue, int.MaxValue, -2)]  // MaxValue + MaxValue = -2 (overflow)
    public void Unchecked_Addition_ShouldWrapAround(int a, int b, int expected)
    {
        unchecked
        {
            (a + b).Should().Be(expected);
        }
    }

    [Theory]
    [InlineData(7, 3, 1)]     // Basic positive case
    [InlineData(-7, 3, -1)]   // Negative dividend
    [InlineData(7, -3, 1)]    // Negative divisor
    [InlineData(-7, -3, -1)]  // Both negative
    public void Modulus_WithNegativeNumbers_ShouldWorkCorrectly(int dividend, int divisor, int expected)
    {
        (dividend % divisor).Should().Be(expected);
    }

    [Fact]
    public void FormattableToString_ShouldWorkCorrectly()
    {
        var value = 42;
        IFormattable formattable = value;
        
        formattable.ToString("D5", CultureInfo.InvariantCulture).Should().Be("00042");
        formattable.ToString("X4", CultureInfo.InvariantCulture).Should().Be("002A");
    }

    [Theory]
    [InlineData(42, "N0", "42")]          // Number format
    [InlineData(42, "C0", "$42")]         // Currency format
    [InlineData(42, "P0", "4,200%")]      // Percentage format
    [InlineData(42, "E2", "4.20E+001")]   // Scientific format
    public void CustomFormatting_ShouldWorkCorrectly(int value, string format, string expected)
    {
        var americanCulture = new CultureInfo("en-US");
        value.ToString(format, americanCulture).Should().Be(expected);
    }
}
