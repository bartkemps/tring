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

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(10)]
        public void ShiftLeft_ShouldMoveTritsLeftAndFillWithZeros(int shiftAmount)
        {
            // Arrange
            var array = new BigTritArray(20);
            // Set some pattern: -1, 0, 1, -1, 0, 1, ...
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = new Trit((sbyte)((i % 3) - 1));
            }
            
            // Act
            var result = array << shiftAmount;
            
            // Assert
            result.Length.Should().Be(array.Length);
            
            // First 'shiftAmount' trits should be zero
            for (var i = 0; i < shiftAmount && i < array.Length; i++)
            {
                result[i].Should().Be(Trit.Zero, $"because position {i} should be filled with zero after left shift of {shiftAmount}");
            }
            
            // Remaining trits should be shifted from original
            for (var i = shiftAmount; i < array.Length; i++)
            {
                result[i].Should().Be(array[i - shiftAmount], $"because position {i} should contain the value from position {i - shiftAmount}");
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(10)]
        public void ShiftRight_ShouldMoveTritsRightAndFillWithZeros(int shiftAmount)
        {
            // Arrange
            var array = new BigTritArray(20);
            // Set some pattern: -1, 0, 1, -1, 0, 1, ...
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = new Trit((sbyte)((i % 3) - 1));
            }
            
            // Act
            var result = array >> shiftAmount;
            
            // Assert
            result.Length.Should().Be(array.Length);
            
            // Last 'shiftAmount' trits should be zero
            for (var i = array.Length - shiftAmount; i < array.Length && i >= 0; i++)
            {
                result[i].Should().Be(Trit.Zero, $"because position {i} should be filled with zero after right shift of {shiftAmount}");
            }
            
            // Remaining trits should be shifted from original
            for (var i = 0; i < array.Length - shiftAmount; i++)
            {
                result[i].Should().Be(array[i + shiftAmount], $"because position {i} should contain the value from position {i + shiftAmount}");
            }
        }

        [Fact]
        public void ShiftLeft_WithNegativeAmount_ShouldShiftRight()
        {
            // Arrange
            var array = new BigTritArray(10);
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = new Trit((sbyte)((i % 3) - 1));
            }
            var expected = array >> 3;
            
            // Act
            var actual = array << -3;
            
            // Assert
            for (var i = 0; i < array.Length; i++)
            {
                actual[i].Should().Be(expected[i], $"because negative left shift should behave like right shift");
            }
        }

        [Fact]
        public void ShiftRight_WithNegativeAmount_ShouldShiftLeft()
        {
            // Arrange
            var array = new BigTritArray(10);
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = new Trit((sbyte)((i % 3) - 1));
            }
            var expected = array << 3;
            
            // Act
            var actual = array >> -3;
            
            // Assert
            for (var i = 0; i < array.Length; i++)
            {
                actual[i].Should().Be(expected[i], $"because negative right shift should behave like left shift");
            }
        }

        [Fact]
        public void Shift_BeyondArrayLength_ShouldReturnAllZeros()
        {
            // Arrange
            var array = new BigTritArray(10);
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = new Trit((sbyte)((i % 3) - 1));
            }
            
            // Act
            var leftResult = array << 15;
            var rightResult = array >> 15;
            
            // Assert
            for (var i = 0; i < array.Length; i++)
            {
                leftResult[i].Should().Be(Trit.Zero, $"because shift amount exceeds array length");
                rightResult[i].Should().Be(Trit.Zero, $"because shift amount exceeds array length");
            }
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
        public void Addition_ShouldAddTwoArrays_SmallValues()
        {
            // Arrange
            var a = new BigTritArray(5);
            var b = new BigTritArray(5);
            
            // Set a to represent 10 (base 10)
            // 10 base 10 = 1*3^2 + 0*3^1 + 1*3^0 = 1,0,1 in balanced ternary
            a[0] = Trit.Positive; // 1
            a[1] = Trit.Zero;     // 0
            a[2] = Trit.Positive; // 1
            
            // Set b to represent 5 (base 10)
            // 5 base 10 = 1*3^2 - 1*3^1 + 0*3^0 = 1,-1,0 in balanced ternary
            b[0] = Trit.Zero;      // 0
            b[1] = Trit.Negative;  // -1
            b[2] = Trit.Positive;  // 1
            
            // Expected result: 10 + 5 = 15
            // 15 base 10 = 1*3^2 + 2*3^1 + 0*3^0 = 1,2,0 in balanced ternary
            // But 2 overflows, so: 1*3^2 + 2*3^1 + 0*3^0 = 1*3^2 + (1*3^1 * 1*3^1) + 0*3^0 
            // = 1*3^2 + 1*3^1 + 1*3^2 + 0*3^0 = 2*3^2 + 1*3^1 + 0*3^0 = 2,1,0 in balanced ternary
            // But since we're using balanced ternary, 2 gets converted to 1,-1 with a carry
            // So the final result is 1,1,0,0 (from right to left) or 0,0,1,1 (from left to right)
            
            // Act
            var result = a + b;
            
            // Assert
            result.Length.Should().Be(5);
            result[0].Should().Be(Trit.Zero);
            result[1].Should().Be(Trit.Zero);
            result[2].Should().Be(Trit.Positive);
            result[3].Should().Be(Trit.Positive);
            result[4].Should().Be(Trit.Zero);

            // Converting to decimal to verify
            BigInteger resultAsInt = result;
            resultAsInt.Should().Be(15);
        }

        [Fact]
        public void Addition_ShouldAddTwoArrays_WithCarry()
        {
            // Arrange
            var a = new BigTritArray(6);
            var b = new BigTritArray(6);
            
            // Set a to represent value with trits that will cause carries
            a[0] = Trit.Positive;
            a[1] = Trit.Positive;
            a[2] = Trit.Positive;
            
            // Set b to the same pattern to force carries
            b[0] = Trit.Positive;
            b[1] = Trit.Positive;
            b[2] = Trit.Positive;
            
            // Act
            var result = a + b;
            
            // Assert
            result.Length.Should().Be(6);
            
            // Expected result pattern when adding 1+1 repeatedly with carries
            // 1+1 = -1 (with carry 1)
            result[0].Should().Be(Trit.Negative);
            // The carry 1 + 1+1 = 1+0 (with another carry 1)
            result[1].Should().Be(Trit.Zero);
            // The carry 1 + 1+1 = 0+1 (with another carry 1)
            result[2].Should().Be(Trit.Positive);
            // The final carry of 1
            result[3].Should().Be(Trit.Positive);
        }

        [Fact]
        public void Addition_ShouldHandleDifferentLengthArrays()
        {
            // Arrange
            var a = new BigTritArray(3);
            var b = new BigTritArray(5);
            
            a[0] = Trit.Positive; // 1
            a[1] = Trit.Positive; // 1
            a[2] = Trit.Positive; // 1
            
            b[0] = Trit.Negative; // -1
            b[1] = Trit.Negative; // -1
            b[2] = Trit.Zero;     // 0
            b[3] = Trit.Positive; // 1
            b[4] = Trit.Positive; // 1
            
            // Act
            var result = a + b;
            
            // Assert
            result.Length.Should().Be(5);
            
            // Check individual positions (simplified assertions for brevity)
            BigInteger resultAsInt = result;
            BigInteger aAsInt = a;
            BigInteger bAsInt = b;
            
            resultAsInt.Should().Be(aAsInt + bAsInt);
        }

        [Fact]
        public void Subtraction_ShouldSubtractArrays_SmallValues()
        {
            // Arrange
            var a = new BigTritArray(5);
            var b = new BigTritArray(5);
            
            // Set a to represent 10 (base 10)
            a[0] = Trit.Positive; // 1
            a[1] = Trit.Zero;     // 0
            a[2] = Trit.Positive; // 1
            
            // Set b to represent 5 (base 10)
            b[0] = Trit.Zero;      // 0
            b[1] = Trit.Negative;  // -1
            b[2] = Trit.Positive;  // 1
            
            // Expected: 10 - 5 = 5
            
            // Act
            var result = a - b;
            
            // Assert
            BigInteger resultAsInt = result;
            resultAsInt.Should().Be(5);
            
            result[0].Should().Be(Trit.Zero);
            result[1].Should().Be(Trit.Negative);
            result[2].Should().Be(Trit.Positive);
        }

        [Fact]
        public void Subtraction_ShouldHandleNegativeResults()
        {
            // Arrange
            var a = new BigTritArray(3);
            var b = new BigTritArray(3);
            
            // a = 5
            a[0] = Trit.Zero;
            a[1] = Trit.Negative;
            a[2] = Trit.Positive;
            
            // b = 10
            b[0] = Trit.Positive;
            b[1] = Trit.Zero;
            b[2] = Trit.Positive;
            
            // Expected: 5 - 10 = -5
            
            // Act
            var result = a - b;
            
            // Assert
            BigInteger resultAsInt = result;
            resultAsInt.Should().Be(-5);
            
            // -5 in balanced ternary is 0,1,-1 (from right to left)
            result[0].Should().Be(Trit.Zero);
            result[1].Should().Be(Trit.Positive);
            result[2].Should().Be(Trit.Negative);
        }

        [Fact]
        public void Multiplication_ShouldMultiplyTwoArrays()
        {
            // Arrange - Using the Calculator's MultiplyBalancedTernary method indirectly
            var a = (BigTritArray)5; // 5 decimal = 1,-1,0 in balanced ternary
            var b = (BigTritArray)3; // 3 decimal = 1,0 in balanced ternary
            
            // Act
            var result = a * b;
            
            // Assert
            BigInteger resultAsInt = result;
            resultAsInt.Should().Be(15); // 5 * 3 = 15
        }

        [Fact]
        public void Multiplication_ShouldHandleZero()
        {
            // Arrange
            var a = (BigTritArray)42;
            var b = (BigTritArray)0;
            
            // Act
            var result1 = a * b;
            var result2 = b * a;
            
            // Assert
            ((BigInteger)result1).Should().Be(0);
            ((BigInteger)result2).Should().Be(0);
        }

        [Fact]
        public void Multiplication_ShouldHandleNegativeValues()
        {
            // Arrange
            var a = (BigTritArray)(-7);
            var b = (BigTritArray)5;
            
            // Act
            var result1 = a * b;
            var result2 = b * a;
            
            // Assert
            ((BigInteger)result1).Should().Be(-35);
            ((BigInteger)result2).Should().Be(-35);
        }

        [Fact]
        public void Multiplication_ShouldHandleLargeValues()
        {
            // Arrange
            var a = (BigTritArray)BigInteger.Parse("12345678901234567890");
            var b = (BigTritArray)BigInteger.Parse("98765432109876543210");
            var expected = BigInteger.Parse("12345678901234567890") * BigInteger.Parse("98765432109876543210");
            
            // Act
            var result = a * b;
            
            // Assert
            ((BigInteger)result).Should().Be(expected);
        }

        #endregion

        #region Unary and Binary Operations Tests

        [Theory]
        [InlineData(-1, 1)]  // NOT of -1 is 1
        [InlineData(0, 0)]   // NOT of 0 is 0
        [InlineData(1, -1)]  // NOT of 1 is -1
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
            array[1] = Trit.Zero;     // 0
            array[2] = Trit.Positive; // 1
            array[3] = Trit.Negative; // -1
            array[4] = Trit.Positive; // 1
            
            // Create a lookup table that rotates trits: -1 -> 0 -> 1 -> -1
            var lookupTable = new[] { Trit.Zero, Trit.Positive, Trit.Negative };
            
            // Act
            var result = array | lookupTable;
            
            // Assert
            result[0].Should().Be(Trit.Zero);     // -1 -> 0
            result[1].Should().Be(Trit.Positive); // 0 -> 1
            result[2].Should().Be(Trit.Negative); // 1 -> -1
            result[3].Should().Be(Trit.Zero);     // -1 -> 0
            result[4].Should().Be(Trit.Negative); // 1 -> -1
        }

        [Theory]
        [InlineData(-1, -1, -1)] // -1 AND -1 = -1
        [InlineData(-1, 0, -1)]  // -1 AND 0 = -1
        [InlineData(-1, 1, -1)]  // -1 AND 1 = -1
        [InlineData(0, -1, -1)]  // 0 AND -1 = -1
        [InlineData(0, 0, 0)]    // 0 AND 0 = 0
        [InlineData(0, 1, 0)]    // 0 AND 1 = 0
        [InlineData(1, -1, -1)]  // 1 AND -1 = -1
        [InlineData(1, 0, 0)]    // 1 AND 0 = 0
        [InlineData(1, 1, 1)]    // 1 AND 1 = 1
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
            var xorTable = new Trit[3, 3] {
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
