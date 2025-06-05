namespace Tring.Numbers;

using Operators;
using TritArray;
using TritArrays;

public struct TritArray27 : ITritArray
{
    internal uint Positive;
    internal uint Negative;

    public TritArray27()
    {
    }

    private TritArray27(UInt32Pair trits)
    {
        Negative = trits.Negative;
        Positive = trits.Positive;
    }

    public Trit this[int index]
    {
        get
        {
            if (index is < 0 or >= 27)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 26.");
            }
            return TritConverter.GetTrit(ref Positive, ref Negative, index);
        }
        set
        {
            if (index is < 0 or >= 27)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 26.");
            }

            TritConverter.SetTrit(ref Positive, ref Negative, index, value);
        }
    }

    public int Length => 27;

    public static TritArray27 operator |(TritArray27 array, Func<Trit, Trit> operation)
        => new(UnaryOperation.Apply(array.Negative, array.Positive, operation));

    public static TritArray27 operator |(TritArray27 array, Trit[] table)
        => new(UnaryOperation.Apply(array.Negative, array.Positive, table));

    public static LookupTritArray27Operator operator |(TritArray27 array, Func<Trit, Trit, Trit> operation)
        => new LookupTritArray27Operator(array, operation);
    public static LookupTritArray27Operator operator |(TritArray27 array, TritLookupTable table)
        => new LookupTritArray27Operator(array, table);
    public static LookupTritArray27Operator operator |(TritArray27 array, Trit[,] table)
        => new LookupTritArray27Operator(array, table);
    

    public static TritArray27 operator <<(TritArray27 array, int shift)
    {
        return shift switch
        {
            >= 27 => new(),
            < 0 => array >> -shift,
            _ => new() { Positive = array.Positive << shift, Negative = array.Negative << shift }
        };
    }

    public static TritArray27 operator >> (TritArray27 array, int shift)
    {
        return shift switch
        {
            >= 27 => new(),
            < 0 => array >> -shift,
            _ => new() { Positive = array.Positive >> shift, Negative = array.Negative >> shift }
        };
    }

    public static TritArray27 operator +(TritArray27 value1, TritArray27 value2)
    {
        Calculator.AddBalancedTernary(value1.Negative, value1.Positive, value2.Negative, value2.Positive, out var negative, out var positive);    
        return new () {Negative = negative, Positive = positive};
    }
    
    public static TritArray27 operator -(TritArray27 value1, TritArray27 value2)
    {
        Calculator.AddBalancedTernary(value1.Negative, value1.Positive, value2.Positive, value2.Negative, out var negative, out var positive);    
        return new () {Negative = negative, Positive = positive};
    }
}
