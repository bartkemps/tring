using FluentAssertions;
using Ternary3.Numbers.TritArrays;
using Xunit;

namespace Ternary3.Tests.Numbers.TritArrays
{
    public class TritFormattingTests
    {
        [Theory]
        [InlineData(0, 0, 1, "0")]
        [InlineData(0, 1, 1, "1")]
        [InlineData(1, 0, 1, "T")]
        [InlineData(0b000, 0b111, 3, "111")]
        [InlineData(0b111, 0b000, 3, "TTT")]
        [InlineData(0b101, 0b000, 3, "T0T")]
        [InlineData(0b00100, 0b10001, 5, "10T01")]
        [InlineData(0b1000000000, 0b0111111110, 10, "T 111111110")]
        [InlineData(0b1111111111, 0b0000000000, 10, "T TTTTTTTTT")]
        [InlineData(0b1010101010, 0b0101010101, 10, "T 1T1T1T1T1")]
        public void FormatTrits_ReturnsCorrectStringRepresentation_ForGivenTrits(ulong negative, ulong positive, int length, string expected)
        {
            var result = TritConverter.FormatTrits(negative, positive, length);
            
            result.Should().Be(expected, $"because the trits should be formatted as '{expected}'");
        }
        
        [Theory]
        [InlineData(0b000000000000, 0b111111111111, 12, "111 111111111")]
        [InlineData(0b111111111111, 0b000000000000, 12, "TTT TTTTTTTTT")]
        [InlineData(0b000000000001, 0b111111110000, 12, "111 11111000T")]
        [InlineData(0b100100100100, 0b010010010010, 12, "T10 T10T10T10")]
        public void FormatTrits_AddsSpaces_EveryNinthTrit(ulong negative, ulong positive, int length, string expected)
        {
            var result = TritConverter.FormatTrits(negative, positive, length);
            
            result.Should().Be(expected, $"because spaces should be added every 9 trits");
        }
        
        [Theory]
        [InlineData(0b111000111_000000000_111000111, 0b000000000_000111000_000000000, 27, "TTT000TTT 000111000 TTT000TTT")]
        public void FormatTrits_HandlesLargeInput_Correctly(ulong negative, ulong positive, int length, string expected)
        {
            var result = TritConverter.FormatTrits(negative, positive, length);
            
            result.Should().Be(expected, $"because larger inputs should be formatted correctly with multiple spaces");
        }
    }
}
