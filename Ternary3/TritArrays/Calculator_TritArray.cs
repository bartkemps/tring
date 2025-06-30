namespace Ternary3.TritArrays;

using Microsoft.VisualBasic;
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
    public static void MultiplyBalancedTernary(
        List<ulong> negative1,
        List<ulong> positive1,
        List<ulong> negative2,
        List<ulong> positive2,
        out List<ulong> negativeResult,
        out List<ulong> positiveResult)
    {
        if (negative1.Count == 0 || negative2.Count == 0)
        {
            negativeResult = [];
            positiveResult = [];
            return;
        }
        if (negative1.Count ==1 && negative2.Count == 1)
        {
            MultiplyBalancedTernary(negative1[0], positive1[0], negative2[0], positive2[0], out var n, out var p);
            negativeResult = [n];
            positiveResult = [p];
        }
        if (false) // if one of the operands is a positive or negative power of 3, simply shift and maybe switch neg and pos
        {
            // simple shift
            // return
        }
        var val1 = TritConverter.ToBigInteger(negative1, positive1);
        var val2 = TritConverter.ToBigInteger(negative2, positive2);
        TritConverter.ToTrits(val1 * val2, out negativeResult, out positiveResult, out _);
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
        negativeResult = new(new ulong[blockShift].Concat(negative.Select(n => n << bits)).Take(count));
        positiveResult = new(new ulong[blockShift].Concat(positive.Select(p => p << bits)).Take(count));
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