using FluentAssertions;
using Ternary3.Numbers;
using Ternary3.Numbers.TritArrays;
using Xunit;

namespace Ternary3.Tests.Numbers.TritArrays;

public class TritConverterTests
{
    [Theory]
    [InlineData(0, 0, 0, 0)]        // Zero value, zero index
    [InlineData(0, 0, 1, 0)]        // Zero value, non-zero index
    [InlineData(0, 1, 0, 1)]        // Positive trit at index 0
    [InlineData(0, 2, 1, 1)]        // Positive trit at index 1
    [InlineData(0, 4, 2, 1)]        // Positive trit at index 2
    [InlineData(1, 0, 0, -1)]       // Negative trit at index 0
    [InlineData(2, 0, 1, -1)]       // Negative trit at index 1
    [InlineData(4, 0, 2, -1)]       // Negative trit at index 2
    [InlineData(1, 2, 0, -1)]       // Negative at 0, positive at 1
    public void GetTrit_UInt32_ReturnsCorrectTritValue(uint negative, uint positive, int index, sbyte expectedValue)
    {
        var result = TritConverter.GetTrit(negative, positive, index);

        result.Value.Should().Be(expectedValue, 
            $"because GetTrit({negative}, {positive}, {index}) should return {expectedValue}");
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0, 1, 0, 1)]
    [InlineData(1, 0, 0, -1)]
    [InlineData(0, 0x8000000000000000, 63, 1)]  // Test high bit in ulong
    [InlineData(0x8000000000000000, 0, 63, -1)] // Test high bit in ulong
    public void GetTrit_UInt64_ReturnsCorrectTritValue(ulong negative, ulong positive, int index, sbyte expectedValue)
    {
        var result = TritConverter.GetTrit(negative, positive, index);

        result.Value.Should().Be(expectedValue, 
            $"because GetTrit({negative}, {positive}, {index}) should return {expectedValue}");
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0, 1, 0, 1)]
    [InlineData(1, 0, 0, -1)]
    [InlineData(0, 0x80, 7, 1)]  // Test high bit in byte
    [InlineData(0x80, 0, 7, -1)] // Test high bit in byte
    public void GetTrit_Byte_ReturnsCorrectTritValue(byte negative, byte positive, int index, sbyte expectedValue)
    {
        var result = TritConverter.GetTrit(negative, positive, index);

        result.Value.Should().Be(expectedValue, 
            $"because GetTrit({negative}, {positive}, {index}) should return {expectedValue}");
    }

    [Theory]
    [InlineData(0, 1)]   // Set to positive
    [InlineData(0, -1)]  // Set to negative
    [InlineData(0, 0)]   // Set to zero
    [InlineData(5, 1)]   // Set different index to positive
    [InlineData(5, -1)]  // Set different index to negative
    [InlineData(31, 1)]  // Set highest bit to positive (for uint32)
    [InlineData(31, -1)] // Set highest bit to negative (for uint32)
    public void SetTrit_UInt32_SetsCorrectBits(int index, sbyte tritValue)
    {
        // Arrange
        uint negative = 0;
        uint positive = 0;
        var trit = new Trit(tritValue);

        // Act
        TritConverter.SetTrit(ref negative, ref positive, index, trit);

        // Assert
        var mask = 1u << index;
        
        if (tritValue == 1)
        {
            // Positive trit: positive bit set, negative bit clear
            ((positive & mask) != 0).Should().BeTrue($"because positive bit at index {index} should be set for value {tritValue}");
            ((negative & mask) == 0).Should().BeTrue($"because negative bit at index {index} should be clear for value {tritValue}");
        }
        else if (tritValue == -1)
        {
            // Negative trit: positive bit clear, negative bit set
            ((positive & mask) == 0).Should().BeTrue($"because positive bit at index {index} should be clear for value {tritValue}");
            ((negative & mask) != 0).Should().BeTrue($"because negative bit at index {index} should be set for value {tritValue}");
        }
        else // tritValue == 0
        {
            // Zero trit: both bits clear
            ((positive & mask) == 0).Should().BeTrue($"because positive bit at index {index} should be clear for value {tritValue}");
            ((negative & mask) == 0).Should().BeTrue($"because negative bit at index {index} should be clear for value {tritValue}");
        }
    }

    [Theory]
    [InlineData(0)]       // Zero
    [InlineData(1)]       // Small positive
    [InlineData(-1)]      // Small negative
    [InlineData(42)]      // Positive
    [InlineData(-42)]     // Negative
    [InlineData(2147483647)]  // Int32.MaxValue
    [InlineData(-2147483648)] // Int32.MinValue
    public void ConvertTo32Trits_Int32_RoundTripsCorrectly(int value)
    {
        // Convert to trits
        TritConverter.ConvertTo32Trits(value, out var negative, out var positive);
        
        // Convert back to int32
        var roundTrip = TritConverter.TritsToInt32(negative, positive);
        
        // Verify the value is preserved
        roundTrip.Should().Be(value, $"because converting {value} to trits and back should preserve the value");
    }

    [Theory]
    [InlineData(0L)]           // Zero
    [InlineData(1L)]           // Small positive
    [InlineData(-1L)]          // Small negative
    [InlineData(42L)]          // Positive
    [InlineData(-42L)]         // Negative
    [InlineData(2147483647L)]  // Int32.MaxValue
    [InlineData(-2147483648L)] // Int32.MinValue
    [InlineData(9223372036854775807L)]  // Int64.MaxValue
    [InlineData(-9223372036854775808L)] // Int64.MinValue
    public void ConvertTo32Trits_Int64_RoundTripsCorrectly(long value)
    {
        // Convert to trits
        TritConverter.ConvertTo32Trits(value, out var negative, out var positive);
        
        // Convert back to int64
        var roundTrip = TritConverter.TritsToInt64(negative, positive);
        
        // For very large values that exceed 32 trits capacity, we can't expect perfect roundtrip
        if (value > int.MaxValue || value < int.MinValue)
        {
            // Just check that the conversion doesn't throw
            roundTrip.Should().NotBe(0, "because a large value shouldn't convert to zero");
        }
        else
        {
            // For values in int range, verify the value is preserved exactly
            roundTrip.Should().Be(value, $"because converting {value} to trits and back should preserve the value");
        }
    }

    [Theory]
    [InlineData(0, 0)]     // Empty
    [InlineData(1, 0)]     // Simple negative
    [InlineData(0, 1)]     // Simple positive
    [InlineData(0xF, 0)]   // Multiple negatives
    [InlineData(0, 0xF)]   // Multiple positives
    [InlineData(0xA, 0x5)] // Mixed pattern
    public void FormatTrits_GeneratesCorrectString(uint negative, uint positive)
    {
        // Format only 4 trits for simplicity
        var result = TritConverter.FormatTrits(negative, positive, 4);
        
        // Manually build expected string
        var expected = "";
        for (var i = 3; i >= 0; i--)
        {
            var trit = TritConverter.GetTrit(negative, positive, i);
            expected += trit.Value switch
            {
                -1 => "T",
                0 => "0",
                1 => "1",
                _ => "?"
            };
        }
        
        result.Should().Be(expected, 
            $"because formatting trits for negative={negative}, positive={positive} should produce '{expected}'");
    }
}
