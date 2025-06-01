namespace Tring.Numbers;

using System.Runtime.CompilerServices;

/// <summary>
/// Encodes trits in two arrays: one for positive trits and one for negative trits.
/// (both are never set at the same time)
/// </summary>
internal static class TritConverter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Trit GetTrit(ref uint positive, ref uint negative, int index) 
        => new((int)((positive >> index) & 1) - (int)((negative >> index) & 1));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetTrit(ref uint positive, ref uint negative, int index, Trit value)
    {
        var mask = 1u << index;
        switch (value.Value)
        {
            case 1:
                positive |= mask;
                negative &= ~mask;
                break;
            case -1:
                positive &= ~mask;
                negative |= mask;
                break;
            default: // case 0
                positive &= ~mask;
                negative &= ~mask;
                break;
        }
    }
}