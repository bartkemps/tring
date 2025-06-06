namespace Tring.Numbers;

internal readonly struct UInt32Pair(uint negative, uint positive)
{
    private readonly ulong packed = ((ulong)negative << 32) | positive;
    public uint Negative => (uint)(packed >> 32);
    public uint Positive => (uint)packed;
}