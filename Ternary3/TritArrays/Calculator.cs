namespace Ternary3.TritArrays;

using System.Numerics;

internal class Calculator
{
    /// <summary>
    /// Adds two balanced ternary numbers represented as separate positive and negative bit arrays.
    /// </summary>
    /// <param name="negative1">Negative bits of the first operand</param>
    /// <param name="positive1">Positive bits of the first operand</param>
    /// <param name="negative2">Negative bits of the second operand</param>
    /// <param name="positive2">Positive bits of the second operand</param>
    /// <param name="negativeResult">Resulting negative bits after addition</param>
    /// <param name="positiveResult">Resulting positive bits after addition</param>
    public static void AddBalancedTernary(
        uint negative1,
        uint positive1,
        uint negative2,
        uint positive2,
        out uint negativeResult,
        out uint positiveResult)
    {
        positiveResult = positive1;
        negativeResult = negative1;
        while (positive2 != 0 || negative2 != 0)
        {
            var bothPositive = positive1 & positive2;
            var bothNegative = negative1 & negative2;
            var onePositive = positive1 ^ positive2;
            var oneNegative = negative1 ^ negative2;
            positiveResult = (onePositive & ~negative1 & ~negative2) | (bothPositive & oneNegative) | (~positive1 & ~positive2 & bothNegative);
            negativeResult = (oneNegative & ~positive1 & ~positive2) | (bothNegative & onePositive) | (~negative1 & ~negative2 & bothPositive);
            positive1 = positiveResult;
            negative1 = negativeResult;
            positive2 = bothPositive << 1;
            negative2 = bothNegative << 1;
        }
    }

    public static void AddBalancedTernary(
        ulong negative1,
        ulong positive1,
        ulong negative2,
        ulong positive2,
        out ulong negativeResult,
        out ulong positiveResult)
    {
        positiveResult = positive1;
        negativeResult = negative1;
        while (positive2 != 0 || negative2 != 0)
        {
            var bothPositive = positive1 & positive2;
            var bothNegative = negative1 & negative2;
            var onePositive = positive1 ^ positive2;
            var oneNegative = negative1 ^ negative2;
            positiveResult = (onePositive & ~negative1 & ~negative2) | (bothPositive & oneNegative) | (~positive1 & ~positive2 & bothNegative);
            negativeResult = (oneNegative & ~positive1 & ~positive2) | (bothNegative & onePositive) | (~negative1 & ~negative2 & bothPositive);
            positive1 = positiveResult;
            negative1 = negativeResult;
            positive2 = bothPositive << 1;
            negative2 = bothNegative << 1;
        }
    }

    public static void MultiplyBalancedTernary(
        uint negative1,
        uint positive1,
        uint negative2,
        uint positive2,
        out uint negativeResult,
        out uint positiveResult)
    {
        // Count the number of significant trits in each operand
        var bitCount1 = BitOperations.PopCount(negative1 | positive1);
        var bitCount2 = BitOperations.PopCount(negative2 | positive2);
        const int maxNumberOfNonZeroBits = 7;

        // If either operand is small or if the total complexity is low
        if (bitCount2 <= maxNumberOfNonZeroBits)
        {
            MultiplyByAlgorithm(negative1, positive1, negative2, positive2, out negativeResult, out positiveResult);
        }
        else if (bitCount1 <= maxNumberOfNonZeroBits)
        {
            MultiplyByAlgorithm(negative2, positive2, negative1, positive1, out negativeResult, out positiveResult);
        }
        else if ((ulong)(negative1 | positive1) * (negative2 | positive2) <= 1U<<20)
        {
            MultiplyByConversionToInt32(negative1, positive1, negative2, positive2, out negativeResult, out positiveResult);
        }
        else
        {
            MultiplyByConversionToInt64(negative1, positive1, negative2, positive2, out negativeResult, out positiveResult);
        }
    }

    public static void MultiplyBalancedTernary(
        ulong negative1,
        ulong positive1,
        ulong negative2,
        ulong positive2,
        out ulong negativeResult,
        out ulong positiveResult)
    {
        var bitCount1 = BitOperations.PopCount(negative1 | positive1);
        var bitCount2 = BitOperations.PopCount(negative2 | positive2);
        const int maxNumberOfNonZeroBits = 10;

        // If either operand is small or if the total complexity is low
        if (bitCount2 <= maxNumberOfNonZeroBits)
        {
            MultiplyByAlgorithm(negative1, positive1, negative2, positive2, out negativeResult, out positiveResult);
        }
        else if (bitCount1 <= maxNumberOfNonZeroBits)
        {
            MultiplyByAlgorithm(negative2, positive2, negative1, positive1, out negativeResult, out positiveResult);
        }
        else if ((negative1 | positive1) * (negative2 | positive2) <= 1UL<<40)
        {
            MultiplyByConversionToInt64(negative1, positive1, negative2, positive2, out negativeResult, out positiveResult);
        }
        else
        {
            MultiplyByConversionToInt128(negative1, positive1, negative2, positive2, out negativeResult, out positiveResult);
        }
    }

    private static void MultiplyByAlgorithm(
        uint negative1, uint positive1,
        uint negative2, uint positive2,
        out uint negativeResult, out uint positiveResult)
    {
        // Initialize result to zero
        positiveResult = 0;
        negativeResult = 0;

        // Early exit for multiplication by zero
        if ((positive1 | negative1) == 0 || (positive2 | negative2) == 0) return;

        uint posShift = 0;
        uint pos1 = positive1, neg1 = negative1;

        // Process each trit in the second operand
        while (positive2 != 0 || negative2 != 0)
        {
            // For positive trit in multiplier (1)
            if ((positive2 & 1) != 0)
            {
                // Add shifted first operand to result
                var shiftedPos = pos1 << (int)posShift;
                var shiftedNeg = neg1 << (int)posShift;

                AddBalancedTernary(negativeResult, positiveResult, shiftedNeg, shiftedPos, out var tmpNeg, out var tmpPos);
                positiveResult = tmpPos;
                negativeResult = tmpNeg;
            }

            // For negative trit in multiplier (-1)
            if ((negative2 & 1) != 0)
            {
                // Add shifted and negated first operand to result
                var shiftedPos = neg1 << (int)posShift;
                var shiftedNeg = pos1 << (int)posShift;

                AddBalancedTernary(negativeResult, positiveResult, shiftedNeg, shiftedPos, out var tmpNeg, out var tmpPos);
                positiveResult = tmpPos;
                negativeResult = tmpNeg;
            }

            // Move to next trit
            positive2 >>= 1;
            negative2 >>= 1;
            posShift++;
        }
    }

    private static void MultiplyByAlgorithm(
        ulong negative1, ulong positive1,
        ulong negative2, ulong positive2,
        out ulong negativeResult, out ulong positiveResult)
    {
        // Initialize result to zero
        positiveResult = 0;
        negativeResult = 0;

        // Early exit for multiplication by zero
        if ((positive1 | negative1) == 0 || (positive2 | negative2) == 0) return;

        uint posShift = 0;
        ulong pos1 = positive1, neg1 = negative1;

        // Process each trit in the second operand
        while (positive2 != 0 || negative2 != 0)
        {
            // For positive trit in multiplier (1)
            if ((positive2 & 1) != 0)
            {
                // Add shifted first operand to result
                var shiftedPos = pos1 << (int)posShift;
                var shiftedNeg = neg1 << (int)posShift;

                AddBalancedTernary(negativeResult, positiveResult, shiftedNeg, shiftedPos, out var tmpNeg, out var tmpPos);
                positiveResult = tmpPos;
                negativeResult = tmpNeg;
            }

            // For negative trit in multiplier (-1)
            if ((negative2 & 1) != 0)
            {
                // Add shifted and negated first operand to result
                var shiftedPos = neg1 << (int)posShift;
                var shiftedNeg = pos1 << (int)posShift;

                AddBalancedTernary(negativeResult, positiveResult, shiftedNeg, shiftedPos, out var tmpNeg, out var tmpPos);
                positiveResult = tmpPos;
                negativeResult = tmpNeg;
            }

            // Move to next trit
            positive2 >>= 1;
            negative2 >>= 1;
            posShift++;
        }
    }

    private static void MultiplyByConversionToInt32(
        uint negative1, uint positive1,
        uint negative2, uint positive2,
        out uint negativeResult, out uint positiveResult)
    {
        // Convert to binary integers (positive - negative)
        var binary1 = TritConverter.ToInt32(negative1, positive1);
        var binary2 = TritConverter.ToInt32(negative2, positive2);

        // Perform multiplication in binary
        var result = binary1 * binary2;

        // Convert back to balanced ternary
        TritConverter.To32Trits(result, out negativeResult, out positiveResult);
    }

    private static void MultiplyByConversionToInt64(
        uint negative1, uint positive1,
        uint negative2, uint positive2,
        out uint negativeResult, out uint positiveResult)
    {
        // Convert to binary integers (positive - negative)
        var binary1 = TritConverter.ToInt64(negative1, positive1);
        var binary2 = TritConverter.ToInt64(negative2, positive2);

        // Perform multiplication in binary
        var result = binary1 * binary2;

        // Convert back to balanced ternary
        TritConverter.To32Trits(result, out negativeResult, out positiveResult);
    }
    
    private static void MultiplyByConversionToInt64(
        ulong negative1, ulong positive1,
        ulong negative2, ulong positive2,
        out ulong negativeResult, out ulong positiveResult)
    {
        // Convert to binary integers (positive - negative)
        var binary1 = TritConverter.ToInt64(negative1, positive1);
        var binary2 = TritConverter.ToInt64(negative2, positive2);

        // Perform multiplication in binary
        var result = binary1 * binary2;

        // Convert back to balanced ternary
        TritConverter.To64Trits(result, out negativeResult, out positiveResult);
    }

    private static void MultiplyByConversionToInt128(
        ulong negative1, ulong positive1,
        ulong negative2, ulong positive2,
        out ulong negativeResult, out ulong positiveResult)
    {
        // Convert to binary integers (positive - negative)
        var binary1 = TritConverter.ToInt128(negative1, positive1);
        var binary2 = TritConverter.ToInt128(negative2, positive2);

        // Perform multiplication in binary
        var result = binary1 * binary2;

        // Convert back to balanced ternary
        TritConverter.To64Trits(result, out negativeResult, out positiveResult);
    }
}

