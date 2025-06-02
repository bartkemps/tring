namespace Tring.TritArray;

internal class Calculator
{
    /// <summary>
    /// Adds two balanced ternary numbers represented as separate positive and negative bit arrays.
    /// </summary>
    /// <param name="positive1">Positive bits of the first operand</param>
    /// <param name="negative1">Negative bits of the first operand</param>
    /// <param name="positive2">Positive bits of the second operand</param>
    /// <param name="negative2">Negative bits of the second operand</param>
    /// <param name="positiveResult">Resulting positive bits after addition</param>
    /// <param name="negativeResult">Resulting negative bits after addition</param>
    public static void AddBalancedTernary(
        uint positive1, 
        uint negative1, 
        uint positive2, 
        uint negative2, 
        out uint positiveResult, 
        out uint negativeResult)
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
    
    public static void MultiplyBalancedTernary(uint positive1, uint negative1, uint positive2, uint negative2, out uint positiveResult, out uint negativeResult)
    {
        // Count the number of significant trits in each operand
        var trits1 = CountSignificantTrits(positive1, negative1);
        var trits2 = CountSignificantTrits(positive2, negative2);
    
        // If either operand is small or if the total complexity is low
        if (trits1 <= 4 || trits2 <= 4 || (trits1 + trits2 <= 12))
        {
            MultiplyByAlgorithm(positive1, negative1, positive2, negative2, out positiveResult, out negativeResult);
        }
        else
        {
            MultiplyByConversion(positive1, negative1, positive2, negative2, out positiveResult, out negativeResult);
        }
    }
    
    private static int CountSignificantTrits(uint positive, uint negative)
    {
        var combined = positive | negative;
        if (combined == 0) return 0;
    
        // Find position of highest set bit
        var highestBit = 0;
        uint mask = 1;
        while (combined >= mask && highestBit < 32)
        {
            highestBit++;
            mask <<= 1;
        }
        return highestBit;
    }
    
    private static void MultiplyByAlgorithm(
        uint positive1, uint negative1,  // First operand
        uint positive2, uint negative2,  // Second operand
        out uint positiveResult, out uint negativeResult)  // Result
    {
        // Initialize result to zero
        positiveResult = 0;
        negativeResult = 0;
    
        // Early exit for multiplication by zero
        if ((positive1 == 0 && negative1 == 0) || (positive2 == 0 && negative2 == 0))
            return;
    
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
    
                uint tmpPos, tmpNeg;
                AddBalancedTernary(positiveResult, negativeResult, shiftedPos, shiftedNeg, out tmpPos, out tmpNeg);
                positiveResult = tmpPos;
                negativeResult = tmpNeg;
            }
    
            // For negative trit in multiplier (-1)
            if ((negative2 & 1) != 0)
            {
                // Add shifted and negated first operand to result
                var shiftedPos = neg1 << (int)posShift;
                var shiftedNeg = pos1 << (int)posShift;
    
                uint tmpPos, tmpNeg;
                AddBalancedTernary(positiveResult, negativeResult, shiftedPos, shiftedNeg, out tmpPos, out tmpNeg);
                positiveResult = tmpPos;
                negativeResult = tmpNeg;
            }
    
            // Move to next trit
            positive2 >>= 1;
            negative2 >>= 1;
            posShift++;
        }
    }
    
    private static void MultiplyByConversion(
        uint positive1, uint negative1,
        uint positive2, uint negative2,
        out uint positiveResult, out uint negativeResult)
    {
        // Convert to binary integers (positive - negative)
        var binary1 = ConvertToSignedBinary(positive1, negative1);
        var binary2 = ConvertToSignedBinary(positive2, negative2);
    
        // Perform multiplication in binary
        var result = binary1 * binary2;
    
        // Convert back to balanced ternary
        ConvertToBTernary(result, out positiveResult, out negativeResult);
    }
    
    private static long ConvertToSignedBinary(uint positive, uint negative)
    {
        long result = 0;
        long powerOfThree = 1;
    
        while (positive != 0 || negative != 0)
        {
            if ((positive & 1) != 0)
                result += powerOfThree;
            if ((negative & 1) != 0)
                result -= powerOfThree;
    
            positive >>= 1;
            negative >>= 1;
            powerOfThree *= 3;
        }
    
        return result;
    }
    
    private static void ConvertToBTernary(long value, out uint positive, out uint negative)
    {
        positive = 0;
        negative = 0;
        uint position = 0;
    
        while (value != 0)
        {
            // Get remainder in the range [0,2]
            var remainder = ((value % 3) + 3) % 3;
    
            if (remainder == 1)
                positive |= 1u << (int)position;
            else if (remainder == 2) // Represents -1 in balanced ternary
            {
                negative |= 1u << (int)position;
                value += 3; // Adjust for the borrowed digit
            }
    
            value /= 3;
            position++;
        }
    }
}