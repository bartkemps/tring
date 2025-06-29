namespace Ternary3.TritArrays;

using System.Numerics;
using System.Runtime.CompilerServices;

partial class Calculator
{
    public static void AddBalancedTernary(
        List<ulong> negative1,
        List<ulong> positive1,
        List<ulong> negative2,
        List<ulong> positive2,
        out List<ulong> negativeResult,
        out List<ulong> positiveResult)
    {
        // Determine the length of the longer array
        var maxCount = Math.Max(negative1.Count, negative2.Count);

        // Initialize result lists with zeros, potentially one longer for carry
        negativeResult = new(new ulong[maxCount + 1]);
        positiveResult = new(new ulong[maxCount + 1]);
        ulong negCarry = 0;
        ulong posCarry = 0;
        for (var i = 0; i <= maxCount; i++)
        {
            // Get values from each array, defaulting to zero if out of bounds
            var neg1 = i < negative1.Count ? negative1[i] : 0UL;
            var pos1 = i < positive1.Count ? positive1[i] : 0UL;
            var neg2 = i < negative2.Count ? negative2[i] : 0UL;
            var pos2 = i < positive2.Count ? positive2[i] : 0UL;

            AddBalancedTernaryWithCarry(neg1, pos1, neg2, pos2, negCarry, posCarry,
                out var negResult, out var posResult, out var negativeCarry, out var positiveCarry);

            negCarry = negativeCarry;
            posCarry = positiveCarry;
            negativeResult[i] = negResult;
            positiveResult[i] = posResult;
        }

        var tritLength = TrimAndDetermineLength(negativeResult, positiveResult);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrimAndDetermineLength(List<ulong> negativeResult, List<ulong> positiveResult)
    {
        // Use the dedicated trim method to remove trailing zeros
        Trim(negativeResult, positiveResult);

        // Handle case where all elements were removed
        if (negativeResult.Count == 0)
        {
            return 0;
        }

        // Calculate the actual length in trits
        var tritLength = negativeResult.Count * 64;
        if (tritLength > 0)
        {
            // Adjust length by removing leading zeros
            var lastNegative = negativeResult[^1];
            var lastPositive = positiveResult[^1];
            var combined = lastNegative | lastPositive;
            if (combined != 0)
            {
                tritLength -= BitOperations.LeadingZeroCount(combined);
            }
            else
            {
                // If the last element is all zeros, the length should be zero
                tritLength = 0;
            }
        }

        return tritLength;
    }

    /// <summary>
    /// Removes trailing zero elements from both the negative and positive lists.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Trim(List<ulong> negative, List<ulong> positive)
    {
        while (negative.Count > 0 && negative[^1] == 0 && positive[^1] == 0)
        {
            negative.RemoveAt(negative.Count - 1);
            positive.RemoveAt(positive.Count - 1);
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BigTritArray MultiplyBalancedTernary(
        List<ulong> negative1,
        List<ulong> positive1,
        List<ulong> negative2,
        List<ulong> positive2,
        out List<ulong> negativeResult,
        out List<ulong> positiveResult)
    {
        var length1 = Math.Max(negative1.Count, positive1.Count);
        var length2 = Math.Max(negative2.Count, positive2.Count);
        var maxResultLength = length1 + length2;

        // Initialize result to zero
        negativeResult = new(new ulong[maxResultLength]);
        positiveResult = new(new ulong[maxResultLength]);

        // Early exit for multiplication by zero
        if (IsZero(negative1, positive1) || IsZero(negative2, positive2))
        {
            return new(negativeResult, positiveResult, 0);
        }

        // Count non-zero trits to determine complexity
        var nonZeroTrits1 = CountNonZeroTrits(negative1, positive1);
        var nonZeroTrits2 = CountNonZeroTrits(negative2, positive2);

        // Optimization: If one of the operands has very few non-zero trits, use the algorithm approach
        const int maxNumberOfNonZeroTrits = 10; // Threshold may need tuning
        if (nonZeroTrits2 <= maxNumberOfNonZeroTrits)
        {
            MultiplyByAlgorithm(negative1, positive1, negative2, positive2, negativeResult, positiveResult);
        }
        else if (nonZeroTrits1 <= maxNumberOfNonZeroTrits)
        {
            MultiplyByAlgorithm(negative2, positive2, negative1, positive1, negativeResult, positiveResult);
        }
        else
        {
            // For more complex cases, still use the algorithm but could consider other methods for future optimization
            MultiplyByAlgorithm(negative1, positive1, negative2, positive2, negativeResult, positiveResult);
        }

        // Use the shared helper method to trim trailing zeros and calculate the trit length
        var tritLength = TrimAndDetermineLength(negativeResult, positiveResult);

        return new(negativeResult, positiveResult, tritLength);
    }

    // Helper method to multiply using the algorithm approach
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void MultiplyByAlgorithm(
        List<ulong> negative1,
        List<ulong> positive1,
        List<ulong> negative2,
        List<ulong> positive2,
        List<ulong> negativeResult,
        List<ulong> positiveResult)
    {
        // Process each trit in the second operand
        var posShift = 0;
        var blockShift = 0; // Track how many full ulong blocks we've shifted
        var bitShift = 0; // Track bit shift within a block

        for (var blockIndex = 0; blockIndex < negative2.Count; blockIndex++)
        {
            var currentNegative2 = negative2[blockIndex];
            var currentPositive2 = blockIndex < positive2.Count ? positive2[blockIndex] : 0UL;

            bitShift = 0;

            while (currentNegative2 != 0 || currentPositive2 != 0)
            {
                // For positive trit in multiplier (1)
                if ((currentPositive2 & 1) != 0)
                {
                    // Create shifted copy of first operand
                    var shiftedNegative = new List<ulong>(new ulong[negative1.Count + blockShift + 1]);
                    var shiftedPositive = new List<ulong>(new ulong[positive1.Count + blockShift + 1]);

                    // Apply the shift (combination of block shift and bit shift)
                    ShiftOperand(negative1, positive1, shiftedNegative, shiftedPositive, blockShift, bitShift);

                    // Add shifted first operand to result
                    List<ulong> tmpNegative, tmpPositive;
                    AddBalancedTernary(negativeResult, positiveResult, shiftedNegative, shiftedPositive,
                        out tmpNegative, out tmpPositive);

                    negativeResult = tmpNegative;
                    positiveResult = tmpPositive;
                }

                // For negative trit in multiplier (-1)
                if ((currentNegative2 & 1) != 0)
                {
                    // Create shifted and negated copy of first operand
                    var shiftedNegative = new List<ulong>(new ulong[positive1.Count + blockShift + 1]);
                    var shiftedPositive = new List<ulong>(new ulong[negative1.Count + blockShift + 1]);

                    // Apply the shift and negation (swap positive and negative)
                    ShiftOperand(positive1, negative1, shiftedNegative, shiftedPositive, blockShift, bitShift);

                    // Add shifted and negated first operand to result
                    List<ulong> tmpNegative, tmpPositive;
                    AddBalancedTernary(negativeResult, positiveResult, shiftedNegative, shiftedPositive,
                        out tmpNegative, out tmpPositive);

                    negativeResult = tmpNegative;
                    positiveResult = tmpPositive;
                }

                // Move to next trit
                currentPositive2 >>= 1;
                currentNegative2 >>= 1;
                bitShift++;

                // If we've processed all 64 bits in this block, move to next block
                if (bitShift == 64)
                {
                    bitShift = 0;
                    break;
                }
            }

            // Update block shift after processing a block
            blockShift++;
        }
    }

    // Shift operand by the specified blocks and bits
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ShiftOperand(
        List<ulong> sourceNegative,
        List<ulong> sourcePositive,
        List<ulong> destNegative,
        List<ulong> destPositive,
        int blockShift,
        int bitShift)
    {
        if (bitShift == 0)
        {
            // Simple block shift, no bit manipulation needed
            for (var i = 0; i < sourceNegative.Count; i++)
                if (i + blockShift < destNegative.Count)
                    destNegative[i + blockShift] = sourceNegative[i];

            for (var i = 0; i < sourcePositive.Count; i++)
                if (i + blockShift < destPositive.Count)
                    destPositive[i + blockShift] = sourcePositive[i];
        }
        else
        {
            // Need to handle shifting bits across block boundaries
            for (var i = 0; i < sourceNegative.Count; i++)
            {
                if (i + blockShift < destNegative.Count)
                    destNegative[i + blockShift] |= sourceNegative[i] << bitShift;

                // Handle bits that cross to the next block
                if (i + blockShift + 1 < destNegative.Count && bitShift > 0)
                    destNegative[i + blockShift + 1] |= sourceNegative[i] >> (64 - bitShift);
            }

            for (var i = 0; i < sourcePositive.Count; i++)
            {
                if (i + blockShift < destPositive.Count)
                    destPositive[i + blockShift] |= sourcePositive[i] << bitShift;

                // Handle bits that cross to the next block
                if (i + blockShift + 1 < destPositive.Count && bitShift > 0)
                    destPositive[i + blockShift + 1] |= sourcePositive[i] >> (64 - bitShift);
            }
        }
    }

    // Helper method to check if a balanced ternary number is zero
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsZero(List<ulong> negative, List<ulong> positive)
    {
        for (var i = 0; i < Math.Max(negative.Count, positive.Count); i++)
        {
            var negValue = i < negative.Count ? negative[i] : 0UL;
            var posValue = i < positive.Count ? positive[i] : 0UL;

            if (negValue != 0 || posValue != 0)
                return false;
        }

        return true;
    }

    // Helper method to count non-zero trits
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CountNonZeroTrits(List<ulong> negative, List<ulong> positive)
    {
        var count = 0;

        for (var i = 0; i < Math.Max(negative.Count, positive.Count); i++)
        {
            var negValue = i < negative.Count ? negative[i] : 0UL;
            var posValue = i < positive.Count ? positive[i] : 0UL;

            // Count bits set in either array (represents non-zero trits)
            count += BitOperations.PopCount(negValue | posValue);
        }

        return count;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ShiftLeft(
        List<ulong> negative,
        List<ulong> positive,
        int shift,
        out List<ulong> negativeResult,
        out List<ulong> positiveResult
    )
    {
        switch (shift)
        {
            case 0:
                negativeResult = new(negative);
                positiveResult = new(positive);
                return;
            case < 0:
                ShiftRight(negative, positive, -shift, out negativeResult, out positiveResult);
                return;
        }

        var count = negative.Count;
        var blockShift = shift / 64;

        if (blockShift >= count)
        {
            negativeResult = new(new ulong[count]);
            positiveResult = new(new ulong[count]);
            return;
        }

        var bits = shift % 64;
        negativeResult = new(new ulong[blockShift].Concat(negative.Select(n => n << bits)).Take(count - blockShift));
        positiveResult = new(new ulong[blockShift].Concat(positive.Select(p => p << bits)).Take(count - blockShift));
        if (bits <= 0) return;
        for (var i = 0; i < count - blockShift - 1; i++)
        {
            negativeResult[i + blockShift + 1] |= (negative[i] >> (64 - bits));
            positiveResult[i + blockShift + 1] |= (positive[i] >> (64 - bits));
        }
    }

    public static void ShiftRight(
        List<ulong> negative,
        List<ulong> positive,
        int shift,
        out List<ulong> negativeResult,
        out List<ulong> positiveResult
    )
    {
        if (shift <= 0)
        {
            ShiftLeft(negative, positive, -shift, out negativeResult, out positiveResult);
            return;
        }

        var count = negative.Count;
        var bits = shift % 64;
        var blockShift = shift / 64;
        negativeResult = new(negative.Skip(blockShift).Select(n => n >> bits).Concat(new ulong[count - blockShift]));
        positiveResult = new(positive.Skip(blockShift).Select(p => p >> bits).Concat(new ulong[count - blockShift]));
        if (bits <= 0) return;
        for (var i = 0; i < count - blockShift - 1; i++)
        {
            negativeResult[i] |= (negative[i + blockShift + 1] << (64 - bits));
            positiveResult[i] |= (positive[i + blockShift + 1] << (64 - bits));
        }
    }
}