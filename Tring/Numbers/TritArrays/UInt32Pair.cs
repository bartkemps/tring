namespace Tring.Numbers;

internal readonly struct UInt32Pair(long negative, long positive)
{
    private readonly UInt128 packed = ((UInt128)(uint)negative << 64) | (uint)positive;
    public uint Negative => (uint)(packed >> 64);
    public uint Positive => (uint)packed;
}