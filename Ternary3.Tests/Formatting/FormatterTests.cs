// using System.Collections.Generic;
// using FluentAssertions;
// using Ternary3;
// using Ternary3.TritArrays;
// using Xunit;
//
// namespace Ternary3.Tests;
//
// using Formatting;
//
// public class FormatterTests
// {
//     private class SimpleTritArray : ITernaryArray
//     {
//         private readonly Trit[] _trits;
//         public SimpleTritArray(params sbyte[] values) => _trits = Array.ConvertAll(values, v => new Trit(v));
//         public Trit this[int index] => _trits[index];
//         public int Length => _trits.Length;
//         public string ToString(ITernaryFormat format) => Formatter.Format(this, format);
//     }
//
//     [Theory]
//     [InlineData(new sbyte[] { -1, 0, 1 }, "1 0 X")]
//     [InlineData(new sbyte[] { -1, -1, -1 }, "X X X")]
//     [InlineData(new sbyte[] { 0, 0, 0, 1, 1, 1 }, "1 1 1 0 0 0")]
//     public void Format_UsesDigitsAndGrouping(sbyte[] ternaries, string expected)
//     {
//         var format = new TernaryFormat
//         {
//             NegativeTritDigit = 'X',
//             ZeroTritDigit = '0',
//             PositiveTritDigit = '1',
//             Groups = new List<TritGroupDefinition> { new(" ", 1) },
//             TernaryPadding = TernaryPadding.None
//         };
//         var arr = new SimpleTritArray(ternaries);
//         var actual = Formatter.Format(arr, format);
//         actual.Should().Be(expected);
//     }
//
//     [Fact]
//     public void Format_AppliesPadding()
//     {
//         var format = new TernaryFormat
//         {
//             NegativeTritDigit = 'T',
//             ZeroTritDigit = '0',
//             PositiveTritDigit = '1',
//             Groups = new List<TritGroupDefinition> { new(",", 4) },
//             TernaryPadding = TernaryPadding.Full
//         };
//         var arr = new SimpleTritArray(-1, 0, 1, 0, 0);
//         var actual = Formatter.Format(arr, format);
//         actual.Should().Be("0,010T".Replace(" ", string.Empty)); // padded left with 0
//     }
//
//     [Fact]
//     public void Format_EmptyArray_ReturnsEmptyString()
//     {
//         var format = new TernaryFormat();
//         var arr = new SimpleTritArray();
//         var actual = Formatter.Format(arr, format);
//         actual.Should().Be("");
//     }
//
//     [Fact]
//     public void Format_MultiLevelGrouping()
//     {
//         var format = new TernaryFormat
//         {
//             NegativeTritDigit = 'T',
//             ZeroTritDigit = '0',
//             PositiveTritDigit = '1',
//             Groups = new List<TritGroupDefinition>
//             {
//                 new("-", 2),
//                 new(":", 2)
//             },
//             TernaryPadding = TernaryPadding.None
//         };
//         var arr = new SimpleTritArray(0, -1, 1, 0, 1, -1, 0, 1);
//         var actual = Formatter.Format(arr, format);
//         // Hierarchical grouping: (10-T1)-(01-T0)
//         actual.Should().Be("10-T1:01-T0");
//     }
//
//     [Fact]
//     public void Format_MultiLevelGrouping_ToString()
//     {
//         var format = new TernaryFormat
//         {
//             Groups = new List<TritGroupDefinition>
//             {
//                 new("-", 3),
//                 new(":", 3)
//             },
//             TernaryPadding = TernaryPadding.None
//         };
//         var actual = format.ToString();
//         actual.Should().Be("T01-T01-T01:TTT-000-111");
//     }
//     
//     [Fact]
//     public void Format_PadToGroup()
//     {
//         var format = new TernaryFormat
//         {
//             Groups = new List<TritGroupDefinition>
//             {
//                 new("-", 5),
//                 new(":", 2)
//             },
//             TernaryPadding = TernaryPadding.Group
//         };
//         var actual = format.ToString();
//         actual.Should().Be("00T01-T01T0:1TTT0-00111");
//     }
// }