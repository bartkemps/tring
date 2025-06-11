using FluentAssertions;
using Ternary3.Operators;
using Xunit;

namespace Ternary3.Tests.Operators
{
    public class UnaryTritOperatorTests
    {
        [Theory]
        [InlineData(-1, -1, -1, "[T T T]")]
        [InlineData(1, 1, 1, "[1 1 1]")]
        [InlineData(0, 0, 0, "[0 0 0]")]
        [InlineData(-1, 0, 1, "[T 0 1]")]
        [InlineData(1, 0, -1, "[1 0 T]")]
        public void DebugView_ReturnsCompactRepresentation(int negativeOut, int zeroOut, int positiveOut, string expected)
        {
            var unaryOperator = new UnaryTritOperator(negativeOut, zeroOut, positiveOut);
            
            var actual = unaryOperator.GetType().GetMethod("DebugView", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.Invoke(unaryOperator, null) as string;
            
            actual.Should().Be(expected, $"the DebugView for [{negativeOut}, {zeroOut}, {positiveOut}] should display as {expected}");
        }

        [Theory]
        [InlineData(-1, 0, 1)]
        [InlineData(1, 0, -1)]
        [InlineData(0, 0, 0)]
        [InlineData(-1, -1, -1)]
        [InlineData(1, 1, 1)]
        public void ToString_ReturnsFormattedTable(int negativeOut, int zeroOut, int positiveOut)
        {
            var unaryOperator = new UnaryTritOperator(negativeOut, zeroOut, positiveOut);
            
            var actual = unaryOperator.ToString();
            
            // Convert trit values to their character representations
            var negativeChar = negativeOut == -1 ? 'T' : negativeOut == 0 ? '0' : '1';
            var zeroChar = zeroOut == -1 ? 'T' : zeroOut == 0 ? '0' : '1';
            var positiveChar = positiveOut == -1 ? 'T' : positiveOut == 0 ? '0' : '1';
            
            var expected = $"""
                           Input | Output
                           ------+-------
                              T  |   {negativeChar}
                              0  |   {zeroChar}
                              1  |   {positiveChar}
                           """;
            
            actual.Should().Be(expected, $"the ToString for [{negativeOut}, {zeroOut}, {positiveOut}] should produce a properly formatted table");
        }
        
        [Theory]
        [InlineData(-1, -1, -1)]
        [InlineData(1, 1, 1)]
        [InlineData(-1, 0, 1)]
        public void GetOutputChar_ReturnsCorrectCharacterForEachTrit(int negativeOut, int zeroOut, int positiveOut)
        {
            var unaryOperator = new UnaryTritOperator(negativeOut, zeroOut, positiveOut);
            
            var actual = unaryOperator.ToString();
            
            // Verify that Trit.Negative maps to the first output
            actual.Should().Contain($"T  |   {(negativeOut == -1 ? 'T' : negativeOut == 0 ? '0' : '1')}", 
                "the negative trit should map to the correct output");
            
            // Verify that Trit.Zero maps to the second output  
            actual.Should().Contain($"0  |   {(zeroOut == -1 ? 'T' : zeroOut == 0 ? '0' : '1')}", 
                "the zero trit should map to the correct output");
            
            // Verify that Trit.Positive maps to the third output
            actual.Should().Contain($"1  |   {(positiveOut == -1 ? 'T' : positiveOut == 0 ? '0' : '1')}", 
                "the positive trit should map to the correct output");
        }
    }
}
