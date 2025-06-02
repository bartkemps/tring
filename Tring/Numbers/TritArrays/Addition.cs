namespace Tring.TritArray;

internal class Addition
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
            positive1 = positiveResult;
            negative1 = negativeResult;
            positiveResult = ((positive1 ^ positive2) & ~negative1 & ~negative2) | (positive1 & positive2 & (negative1 ^ negative2)) | (~positive1 & ~positive2 & negative1 & negative2);
            negativeResult = ((negative1 ^ negative2) & ~positive1 & ~positive2) | (negative1 & negative2 & (positive1 ^ positive2)) | (~negative1 & ~negative2 & positive1 & positive2);
            positive2 = (positive1 & positive2) << 1;
            negative2 = (negative1 & negative2) << 1;
        }
    }
}