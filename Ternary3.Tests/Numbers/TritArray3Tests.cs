using FluentAssertions;
using System;
using Ternary3;
using Xunit;

namespace Ternary3.Tests.Numbers
{
    public class TritArray3Tests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Indexer_IndexFromEnd_GetsCorrectTrit(int fromEnd)
        {
            var arr = new TritArray3();
            arr[0] = Trit.Negative;
            arr[1] = Trit.Zero;
            arr[2] = Trit.Positive;

            var expected = arr[2 - fromEnd];
            var actual = arr[^ (fromEnd + 1)];
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        public void Indexer_IndexFromEnd_SetsCorrectTrit(int fromEnd, sbyte tritValue)
        {
            var arr = new TritArray3();
            arr[^ (fromEnd + 1)] = new(tritValue);
            arr[^ (fromEnd + 1)].Value.Should().Be(tritValue);
        }
    }
}
