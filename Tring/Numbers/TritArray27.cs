namespace Tring.Numbers;

using Operators;

public struct TritArray27 : ITritArray
{
    private uint positive;
    private uint negative;

    public TritArray27()
    {
    }

    private TritArray27(UInt32Pair trits)
    {
        negative = trits.Negative;
        positive = trits.Positive;
    }

    public Trit this[int index]
    {
        get
        {
            if (index is < 0 or >= 27)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 26.");
            }

            return TritConverter.GetTrit(ref positive, ref negative, index);
        }
        set
        {
            if (index is < 0 or >= 27)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 26.");
            }

            TritConverter.SetTrit(ref positive, ref negative, index, value);
        }
    }

    public int Length => 27;

    public static TritArray27 operator |(TritArray27 array, Func<Trit, Trit> operation)
        => new(UnaryOperation.Apply(array.negative, array.positive, operation));

    public static TritArray27 operator |(TritArray27 array, Trit[] table)
        => new(UnaryOperation.Apply(array.negative, array.positive, table));
}