namespace Tring.Numbers;

public class TritArray
{
    public TritArray(Int3T[] values)
    {

    }
}

internal class TritConverter
{
    public static Trit GetTrit(byte value, int index)
    {
        // trits are encoded as follows:
        // 00 = 0
        // 01 = 1
        // 10 = not used
        // 11 = -1
        // a byte has 4 trits, so we can extract the trit at the specified index
        if (index is < 0 or > 3) throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 3.");
        var tritValue = (value >> (2 * index) & 0b11);
        return tritValue switch
        {
            0 => null, // 00 -> 0
            1 => true, // 01 -> 1
            3 => false, // 11 -> -1 (interpreted as false for this context)
            _ => throw new InvalidOperationException("Unexpected trit value.")
        };
    }

    public static byte GetTritsByte(sbyte value, int numTrits)
    {
        if (numTrits < 0 || numTrits > 4)
            throw new ArgumentOutOfRangeException(nameof(numTrits), "Can store at most 4 trits in a byte.");

        byte result = 0;
        // Add 13 to handle the range (-13 to 13)
        int balancedValue = value + 13;

        for (int i = 0; i < numTrits; i++)
        {
            // Extract trit at position i
            int tritValue = balancedValue % 3; // 0, 1, or 2
            balancedValue /= 3;

            // Convert from [0,1,2] to desired bit pattern
            byte encodedTrit = tritValue switch
            {
                0 => 0b00, // 0 -> 00
                1 => 0b01, // 1 -> 01
                2 => 0b11, // -1 -> 11 (represented as 2 in modulo arithmetic)
                _ => throw new InvalidOperationException("Invalid trit value")
            };

            // Insert encoded trit at the correct position
            result |= (byte)(encodedTrit << (i * 2));
        }

        return result;
    }
}