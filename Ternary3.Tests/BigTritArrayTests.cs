using FluentAssertions;

namespace Ternary3.Tests
{
    public partial class BigTritArrayTests
    {
        [Fact]
        public void Ctor_WithLength_SetsLengthAndZeroes()
        {
            var arr = new BigTritArray(130);
            arr.Length.Should().Be(130);
            for (var i = 0; i < arr.Length; i++)
                arr[i].Should().Be(Trit.Zero);
        }

        [Fact]
        public void Ctor_WithNegativeLength_Throws()
        {
            Action act = () => new BigTritArray(-1);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Ctor_ConcatenatesAll_TritArrays()
        {
            TritArray3 a = terT01;
            TritArray3 b = ter01T;
            var arr = new BigTritArray(a, b);
            arr.Length.Should().Be(6);
            arr[0].Should().Be(Trit.Positive);
            arr[1].Should().Be(Trit.Zero);
            arr[2].Should().Be(Trit.Negative);
            arr[3].Should().Be(Trit.Negative);
            arr[4].Should().Be(Trit.Positive);
            arr[5].Should().Be(Trit.Zero);
        }
        
        [Fact]
        public void Ctor_FromTritArrays_ConcatenatesAllWhenOverflowing()
        {
            TritArray3 a = ter001;
            TritArray27 b = terT00000000_000000000_000000001;
            var arr = new BigTritArray(a, b);
            arr.Length.Should().Be(30);
            arr[0].Should().Be(Trit.Positive);
            arr[3].Should().Be(Trit.Positive);
            arr[29].Should().Be(Trit.Negative);
        }

        [Fact]
        public void Ctor_FromNullArray_Throws()
        {
            Action act = () => new BigTritArray(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_FromEmptyArray_Throws()
        {
            var sut = new BigTritArray();
            sut.Length.Should().Be(0);
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
        public void Indexer_IndexFromEnd_GetsCorrectTrit(int fromEnd)
        {
            var arr = new BigTritArray(20);
            for (var i = 0; i < arr.Length; i++)
                arr[i] = new((sbyte)((i % 3) - 1));
            var expected = arr[19 - fromEnd];
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
        public void Indexer_IndexFromEnd_SetsCorrectTrit(int fromEnd, sbyte tritValue)
        {
            var arr = new BigTritArray(20);
            arr[^ (fromEnd + 1)] = new(tritValue);
            arr[^ (fromEnd + 1)].Value.Should().Be(tritValue);
        }
    }
}
