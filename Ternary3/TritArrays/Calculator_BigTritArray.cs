namespace Ternary3.TritArrays;

partial class Calculator
{
    public static BigTritArray Add(
        List<ulong> negative1,
        List<ulong> positive1, 
        List<ulong> negative2, 
        List<ulong> positive2,
        out List<ulong> negativeResult,
        out List<ulong> positiveResult)
    {
        // the resulting TritArray may have the length of the longer one or one more
        // calls AddBalancedTernary with ulongs
        // simply swap parameters for subtraction
        throw new NotImplementedException();
    }
    
    
    public static BigTritArray Multiply(
        List<ulong> negative1,
        List<ulong> positive1, 
        List<ulong> negative2, 
        List<ulong> positive2,
        out List<ulong> negativeResult,
        out List<ulong> positiveResult)
    {
        // the resulting TritArray may have a length of the sum of the two
        throw new NotImplementedException();
    }
    
    public static BigTritArray ShiftRight(
        List<ulong> negative,
        List<ulong> positive, 
        int shift,
        out List<ulong> negativeResult,
        out List<ulong> positiveResult)
    {
        // a negative shift results in a left shift
        // warn when shifting left a large number
        // maybe limit the shift to the Max(Length, 512)?
        throw new NotImplementedException();
    }

}