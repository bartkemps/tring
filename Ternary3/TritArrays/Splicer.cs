namespace Ternary3.TritArrays;

internal static class Splicer
{
    public static void Splice(ulong negative, ulong positive, int length, Range range,
        out List<ulong> negativeResult, out List<ulong> positiveResult, out int resultLength)
    {
        var start = range.Start.GetOffset(length);
        var end = range.End.GetOffset(length);
        if (start < 0 || end > length || start >= end)
        {
            throw new ArgumentOutOfRangeException(nameof(range), "Invalid range for splicing.");
        }
        resultLength = end - start;
        if (resultLength == 0)
        {
            negativeResult = [];
            positiveResult = [];
            return;
        }
        negativeResult = [negative >> start];
        positiveResult = [positive >> start];
    }
    
    public static void Splice(List<ulong> negative, List<ulong> positive, int length, Range range,
        out List<ulong> negativeResult, out List<ulong> positiveResult, out int resultLength)
    {
        var start = range.Start.GetOffset(length);
        var end = range.End.GetOffset(length);
        if (start < 0 || end > length || start >= end)
        {
            throw new ArgumentOutOfRangeException(nameof(range), "Invalid range for splicing.");
        }

        resultLength = end - start;
        if (resultLength == 0)
        {
            negativeResult = [];
            positiveResult = [];
            return;
        }

        var resultWordsNeeded = (resultLength + 63) / 64;
        negativeResult = new(new ulong[resultWordsNeeded]);
        positiveResult = new(new ulong[resultWordsNeeded]);
        var sourceWordIdx = start / 64;
        var sourceBitOffset = start % 64;
        if (sourceBitOffset == 0)
        {
            for (var i = 0; i < resultWordsNeeded; i++)
            {
                negativeResult[i] = negative[sourceWordIdx + i];
                positiveResult[i] = positive[sourceWordIdx + i];
            }
            return;
        }

        for (var targetWordIdx = 0; targetWordIdx < resultWordsNeeded; targetWordIdx++)
        {
            negativeResult[targetWordIdx] |= negative[sourceWordIdx] >> sourceBitOffset;
            positiveResult[targetWordIdx] |= positive[sourceWordIdx] >> sourceBitOffset;

            if (sourceWordIdx + 1 < negative.Count)
            {
                negativeResult[targetWordIdx] |= negative[sourceWordIdx + 1] << (64 - sourceBitOffset);
                positiveResult[targetWordIdx] |= positive[sourceWordIdx + 1] << (64 - sourceBitOffset);
            }

            sourceWordIdx++;
        }
    }
}