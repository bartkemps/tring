namespace Tring.Numbers;

public struct TritArray3 : ITritArray
{
    byte value;

    public TritArray3(Int3T value)
    {
        this.value = TritConverter.GetTritsByte((sbyte)value, 4);
    }

    public bool? this[int index] => TritConverter.GetTrit(value, index);

    public int Length => 3;
}