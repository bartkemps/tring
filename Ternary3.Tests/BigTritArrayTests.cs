using FluentAssertions;
using Ternary3.Operators;
using System.Numerics;

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

        #region Shift Operators Tests

        [Fact]
        public void ShiftRight_ShouldMoveTritsRightAndFillWithZeros()
        {
            BigTritArray array = ter10T01;
            array.Length.Should().Be(5);
            var actual = array >> 2;
            actual.Length.Should().Be(5);
            actual.ToString("ter").Should().Be("00 10T");
        }

        [Fact]
        public void ShiftLeft_WithNegativeAmount_ShouldShiftRight()
        {
            BigTritArray array = ter10T01;
            array.Length.Should().Be(5);
            var actual = array << -2;
            actual.Length.Should().Be(5);
            actual.ToString("ter").Should().Be("00 10T");
        }
        
        [Fact]
        public void ShiftLeft_ShouldMoveTritsRightAndFillWithZeros()
        {
            BigTritArray array = ter10T01;
            array.Length.Should().Be(5);
            var actual = array << 2;
            actual.Length.Should().Be(5);
            actual.ToString("ter").Should().Be("T0 100");
        }

        [Fact]
        public void ShiftRight_WithNegativeAmount_ShouldShiftLeft()
        {
            BigTritArray array = ter10T01;
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
            BigTritArray a = new([a9]);
            a.ToString().Should().Be("4");
            a.ToString("ter").Should().Be("000 000 011");
        }

        [Fact]
        public void Addition_ShouldWork()
        {
            BigTritArray a = 150;
            BigTritArray b = 200;
            BigTritArray c = a + b;
            c.ToString().Should().Be("350");
        }

        [Fact]
        public void Addition_ShouldWork_ForBigNumbers()
        {
            BigTritArray a = BigInteger.Parse("150000000000000000000000000000000000000000000000000");
            a.ToString().Should().Be("150000000000000000000000000000000000000000000000000");
            BigTritArray b = BigInteger.Parse("200000000000000000000000000000000000000000000000000");
            b.ToString().Should().Be("200000000000000000000000000000000000000000000000000");
            BigTritArray c = a + b;
            c.ToString().Should().Be("350000000000000000000000000000000000000000000000000");
        }

        [Fact]
        public void Substraction_ShouldWork()
        {
            BigTritArray a = 350;
            BigTritArray b = 200;
            BigTritArray c = a - b;
            c.ToString().Should().Be("150");
        }

        [Fact]
        public void Substraction_ShouldWork_ForBigNumbers()
        {
            BigTritArray a = BigInteger.Parse("350000000000000000000000000000000000000000000000000");
            a.ToString().Should().Be("350000000000000000000000000000000000000000000000000");
            BigTritArray b = BigInteger.Parse("200000000000000000000000000000000000000000000000000");
            b.ToString().Should().Be("200000000000000000000000000000000000000000000000000");
            BigTritArray c = a - b;
            c.ToString().Should().Be("150000000000000000000000000000000000000000000000000");
        }

        [Fact]
        public void Addition_ShouldAddTwoArrays_WithCarry()
        {
        }

        [Fact]
        public void Addition_ShouldHandleDifferentLengthArrays()
        {
        }

        [Fact]
        public void Subtraction_ShouldSubtractArrays_SmallValues()
        {
        }

        [Fact]
        public void Subtraction_ShouldHandleNegativeResults()
        {
        }

        [Fact]
        public void Multiplication_ShouldMultiplyTwoArrays()
        {
        }

        [Fact]
        public void Multiplication_ShouldHandleZero()
        {
        }

        [Fact]
        public void Multiplication_ShouldHandleNegativeValues()
        {
        }

        [Fact]
        public void Multiplication_ShouldHandleLargeValues()
        {
        }

        #endregion

        #region Unary and Binary Operations Tests

        [Theory]
        [InlineData(-1, 1)] // NOT of -1 is 1
        [InlineData(0, 0)] // NOT of 0 is 0
        [InlineData(1, -1)] // NOT of 1 is -1
        public void UnaryOperation_AppliesOperationToEachTrit(sbyte input, sbyte expected)
        {
            // Arrange
            var array = new BigTritArray(5);
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = new Trit(input);
            }

            // Create a NOT operation
            Func<Trit, Trit> notOp = t => new Trit((sbyte)(-t.Value));

            // Act
            var result = array | notOp;

            // Assert
            for (var i = 0; i < result.Length; i++)
            {
                result[i].Value.Should().Be(expected, $"because NOT {input} should equal {expected}");
            }
        }

        [Fact]
        public void LookupTable_AppliesTableToEachTrit()
        {
            // Arrange
            var array = new BigTritArray(5);
            array[0] = Trit.Negative; // -1
            array[1] = Trit.Zero; // 0
            array[2] = Trit.Positive; // 1
            array[3] = Trit.Negative; // -1
            array[4] = Trit.Positive; // 1

            // Create a lookup table that rotates trits: -1 -> 0 -> 1 -> -1
            var lookupTable = new[] { Trit.Zero, Trit.Positive, Trit.Negative };

            // Act
            var result = array | lookupTable;

            // Assert
            result[0].Should().Be(Trit.Zero); // -1 -> 0
            result[1].Should().Be(Trit.Positive); // 0 -> 1
            result[2].Should().Be(Trit.Negative); // 1 -> -1
            result[3].Should().Be(Trit.Zero); // -1 -> 0
            result[4].Should().Be(Trit.Negative); // 1 -> -1
        }

        [Theory]
        [InlineData(-1, -1, -1)] // -1 AND -1 = -1
        [InlineData(-1, 0, -1)] // -1 AND 0 = -1
        [InlineData(-1, 1, -1)] // -1 AND 1 = -1
        [InlineData(0, -1, -1)] // 0 AND -1 = -1
        [InlineData(0, 0, 0)] // 0 AND 0 = 0
        [InlineData(0, 1, 0)] // 0 AND 1 = 0
        [InlineData(1, -1, -1)] // 1 AND -1 = -1
        [InlineData(1, 0, 0)] // 1 AND 0 = 0
        [InlineData(1, 1, 1)] // 1 AND 1 = 1
        public void BinaryOperation_AppliesFunctionToEachPairOfTrits(sbyte leftInput, sbyte rightInput, sbyte expected)
        {
            // Arrange
            var leftArray = new BigTritArray(3);
            var rightArray = new BigTritArray(3);

            // Set all trits to the test values
            for (var i = 0; i < leftArray.Length; i++)
            {
                leftArray[i] = new Trit(leftInput);
                rightArray[i] = new Trit(rightInput);
            }

            // Create an AND operation
            Func<Trit, Trit, Trit> andOp = (a, b) => new Trit((sbyte)Math.Min(a.Value, b.Value));

            // Act
            var result = leftArray | andOp | rightArray;

            // Assert
            for (var i = 0; i < result.Length; i++)
            {
                result[i].Value.Should().Be(expected, $"because {leftInput} AND {rightInput} should equal {expected}");
            }
        }

        [Fact]
        public void BinaryOperation_HandlesArraysOfDifferentLengths()
        {
            // Arrange
            var leftArray = new BigTritArray(5);
            var rightArray = new BigTritArray(3);

            for (var i = 0; i < leftArray.Length; i++)
            {
                leftArray[i] = Trit.Positive;
            }

            for (var i = 0; i < rightArray.Length; i++)
            {
                rightArray[i] = Trit.Negative;
            }

            // Create XOR operation using a BinaryTritOperator
            var xorTable = new Trit[3, 3]
            {
                { Trit.Zero, Trit.Negative, Trit.Positive },
                { Trit.Negative, Trit.Zero, Trit.Positive },
                { Trit.Positive, Trit.Positive, Trit.Zero }
            };

            // Act
            var result = leftArray | xorTable | rightArray;

            // Assert
            result.Length.Should().Be(5);

            // For the overlapping part, we should have 1 XOR -1 = Positive
            for (var i = 0; i < rightArray.Length; i++)
            {
                result[i].Should().Be(Trit.Positive, $"because 1 XOR -1 should equal 1");
            }

            // For the non-overlapping part, we should have 1 XOR 0 = Positive
            for (var i = rightArray.Length; i < leftArray.Length; i++)
            {
                result[i].Should().Be(Trit.Positive, $"because 1 XOR 0 should equal 1");
            }
        }

        #endregion
    }
}