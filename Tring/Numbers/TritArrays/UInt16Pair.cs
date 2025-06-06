namespace Tring.Numbers;

internal readonly struct UInt16Pair(ushort negative, ushort positive)
{
    private readonly uint packed = (uint)(negative << 16) | positive;
    public ushort Negative => (ushort)(packed >> 16);
    public ushort Positive => (ushort)packed;
}