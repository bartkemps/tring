using FluentAssertions;
using Ternary3.Operators;
using System.Numerics;
using Ternary3.Operators.Operations;

namespace Ternary3.Tests
{
    public partial class TritArrayTests
    {
        [Fact]
        public void Ctor_WithLength_SetsLengthAndZeroes()
        {
            var arr = new TritArray(130);
            arr.Length.Should().Be(130);
            for (var i = 0; i < arr.Length; i++)
                arr[i].Should().Be(Trit.Zero);
        }

        [Fact]
        public void Ctor_WithNegativeLength_Throws()
        {
            Action act = () => new TritArray(-1);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Ctor_ConcatenatesAll_TritArrays()
        {
            TritArray3 a = terT01;
            TritArray3 b = ter01T;
            var arr = new TritArray(a, b);
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
            var arr = new TritArray(a, b);
            arr.Length.Should().Be(30);
            arr[0].Should().Be(Trit.Positive);
            arr[3].Should().Be(Trit.Positive);
            arr[29].Should().Be(Trit.Negative);
        }

        [Fact]
        public void Ctor_FromNullArray_Throws()
        {
            Action act = () => new TritArray(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_FromEmptyArray_Throws()
        {
            var sut = new TritArray();
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
            var arr = new TritArray(20);
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
            var arr = new TritArray(20);
            arr[^ (fromEnd + 1)] = new(tritValue);
            arr[^ (fromEnd + 1)].Value.Should().Be(tritValue);
        }

        #region Shift Operators Tests

        [Fact]
        public void ShiftRight_ShouldMoveTritsRightAndFillWithZeros()
        {
            TritArray array = ter10T01;
            array.Length.Should().Be(5);
            var actual = array >> 2;
            actual.Length.Should().Be(5);
            actual.ToString("ter").Should().Be("00 10T");
        }

        [Fact]
        public void ShiftLeft_WithNegativeAmount_ShouldShiftRight()
        {
            TritArray array = ter10T01;
            array.Length.Should().Be(5);
            var actual = array << -2;
            actual.Length.Should().Be(5);
            actual.ToString("ter").Should().Be("00 10T");
        }
        
        [Fact]
        public void ShiftLeft_ShouldMoveTritsRightAndFillWithZeros()
        {
            TritArray array = ter10T01;
            array.Length.Should().Be(5);
            var actual = array << 2;
            actual.Length.Should().Be(5);
            actual.ToString("ter").Should().Be("T0 100");
        }
        
                [Fact]
                public void ShiftLeft_ShouldCorrectlyCrossBoundaries()
                {
                    TritArray array = new(129);
                    array[0] = Trit.Positive;
                    array[1] = Trit.Negative;
                    var actual = array << 127;
                    actual.Length.Should().Be(129);
                    actual[127].Should().Be(Trit.Positive, "because shifting left by 127 should move the first trit to the last position");
                    actual[128].Should().Be(Trit.Negative, "because shifting left by 127 should move the first trit to the last position");
                }

        [Fact]
        public void ShiftRight_WithNegativeAmount_ShouldShiftLeft()
        {
            TritArray array = ter10T01;
            array.Length.Should().Be(5);
            var actual = array >> -2;
            actual.Length.Should().Be(5);
            actual.ToString("ter").Should().Be("T0 100");
        }

        #endregion

        #region Arithmetic Operators Tests

        [Fact]
        public void Formatting_ShouldWork()
        {
            TritArray9 a9 = 4;
            a9.ToString().Should().Be("4");
            TritArray a = new([a9]);
            a.ToString().Should().Be("4");
            a.ToString("ter").Should().Be("000 000 011");
        }

        [Fact]
        public void Addition_ShouldWork()
        {
            TritArray a = 150;
            TritArray b = 200;
            var c = a + b;
            c.ToString().Should().Be("350");
        }

        [Fact]
        public void Addition_ShouldWork_ForBigNumbers()
        {
            TritArray a = BigInteger.Parse("150000000000000000000000000000000000000000000000000");
            a.ToString().Should().Be("150000000000000000000000000000000000000000000000000");
            TritArray b = BigInteger.Parse("200000000000000000000000000000000000000000000000000");
            b.ToString().Should().Be("200000000000000000000000000000000000000000000000000");
            var c = a + b;
            c.ToString().Should().Be("350000000000000000000000000000000000000000000000000");
        }

        [Fact]
        public void Substraction_ShouldWork()
        {
            TritArray a = 350;
            TritArray b = 200;
            var c = a - b;
            c.ToString().Should().Be("150");
        }

        [Fact]
        public void Substraction_ShouldWork_ForBigNumbers()
        {
            TritArray a = BigInteger.Parse("350000000000000000000000000000000000000000000000000");
            a.ToString().Should().Be("350000000000000000000000000000000000000000000000000");
            TritArray b = BigInteger.Parse("200000000000000000000000000000000000000000000000000");
            b.ToString().Should().Be("200000000000000000000000000000000000000000000000000");
            var c = a - b;
            c.ToString().Should().Be("150000000000000000000000000000000000000000000000000");
        }

        [Fact]
        public void Addition_ShouldHandleDifferentLengthArrays()
        {
            TritArray a = BigInteger.Parse("350000000000000000000000000000000000000000000000000");
            a.ToString().Should().Be("350000000000000000000000000000000000000000000000000");
            TritArray b = 5;
            b.ToString().Should().Be("5");
            var c = a + b;
            c.ToString().Should().Be("350000000000000000000000000000000000000000000000005");
            var d = a - b;
            d.ToString().Should().Be("349999999999999999999999999999999999999999999999995");
        }

        #endregion

        #region Unary and Binary Operations Tests

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, false, null)]
        [InlineData(false, false, true)]
        [InlineData(false, null, false)]
        [InlineData(false, null, null)]
        [InlineData(false, null, true)]
        [InlineData(false, true, false)]
        [InlineData(false, true, null)]
        [InlineData(false, true, true)]
        [InlineData(null, false, false)]
        [InlineData(null, false, null)]
        [InlineData(null, false, true)]
        [InlineData(null, null, false)]
        [InlineData(null, null, null)]
        [InlineData(null, null, true)]
        [InlineData(null, true, false)]
        [InlineData(null, true, null)]
        [InlineData(null, true, true)]
        [InlineData(true, false, false)]
        [InlineData(true, false, null)]
        [InlineData(true, false, true)]
        [InlineData(true, null, false)]
        [InlineData(true, null, null)]
        [InlineData(true, null, true)]
        [InlineData(true, true, false)]
        [InlineData(true, true, null)]
        [InlineData(true, true, true)]
        public void UnaryOperation_AppliesOperationToEachTrit(bool? n, bool? z, bool? p)
        {
            var op = new UnaryTritOperator(n,z,p);
            var target = new TritArray(131)
            {
                [128] = Trit.Negative,
                [129] = Trit.Zero,
                [130] = Trit.Positive
            };
            target.Length.Should().Be(131);
            var actual = target | op;
            actual[32].Should().Be(Trit.Zero | op);
            actual[96].Should().Be(Trit.Zero | op);
            actual[128].Should().Be(Trit.Negative | op);
            actual[129].Should().Be(Trit.Zero | op);
            actual[130].Should().Be(Trit.Positive | op);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(true, true, true)]
        [InlineData(false, false, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, false)]
        [InlineData(false, null, true)]
        public void LookupTable_AppliesTableToEachTrit(bool? n, bool? z, bool? p)
        {
            var op = new[] { (Trit)n, (Trit)z, (Trit)p };
            var target = new TritArray(131)
            {
                [0] = Trit.Negative,
                [1] = Trit.Zero,
                [2] = Trit.Positive,
                [128] = Trit.Negative,
                [129] = Trit.Zero,
                [130] = Trit.Positive
            };
            target.Length.Should().Be(131);
            var actual = target | op;
            actual[0].Should().Be(Trit.Negative | op);
            actual[1].Should().Be(Trit.Zero | op);
            actual[2].Should().Be(Trit.Positive | op);
            actual[128].Should().Be(Trit.Negative | op);
            actual[129].Should().Be(Trit.Zero | op);
            actual[130].Should().Be(Trit.Positive | op);
        }

        [Fact]
        public void BinaryOperation_AppliesFunctionToEachPairOfTrits()
        {
            TritArray a = terTTT000111;
            TritArray b = terT01T01T01;
            var actual = a | BinaryTritOperator.LesserThan | b;
            actual[0].Should().Be(Trit.Positive | BinaryTritOperator.LesserThan | Trit.Positive);
            actual[1].Should().Be(Trit.Positive | BinaryTritOperator.LesserThan | Trit.Zero);
            actual[2].Should().Be(Trit.Positive | BinaryTritOperator.LesserThan | Trit.Negative);
            actual[3].Should().Be(Trit.Zero | BinaryTritOperator.LesserThan | Trit.Positive);
            actual[4].Should().Be(Trit.Zero | BinaryTritOperator.LesserThan | Trit.Zero);
            actual[5].Should().Be(Trit.Zero | BinaryTritOperator.LesserThan | Trit.Negative);
            actual[6].Should().Be(Trit.Negative | BinaryTritOperator.LesserThan | Trit.Positive);
            actual[7].Should().Be(Trit.Negative | BinaryTritOperator.LesserThan | Trit.Zero);
            actual[8].Should().Be(Trit.Negative | BinaryTritOperator.LesserThan | Trit.Negative);
        }

        [Fact]
        public void BinaryOperation_GrowsToBiggestSize()
        {
            TritArray a = ter111;
            TritArray b = new(200);
            (a | BinaryTritOperator.Or | b).Length.Should().Be(200, "because the result should be as long as the longest input array");
            (b | BinaryTritOperator.Or |a).Length.Should().Be(200, "because the result should be as long as the longest input array");
        }

        #endregion
    }
}