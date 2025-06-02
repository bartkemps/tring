namespace Tring.Numbers;

internal readonly struct UInt64Pair(ulong negative, ulong positive)
{
    private readonly UInt128 packed = ((UInt128)(ulong)negative << 64) | (ulong)positive;
    public ulong Negative => (ulong)(packed >> 64);
    public ulong Positive => (ulong)packed;
}