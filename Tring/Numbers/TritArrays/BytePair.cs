namespace Tring.Numbers;

internal readonly struct BytePair(byte negative, byte positive)
{
    private readonly ushort packed = (ushort)((negative << 8) | positive);
    public byte Negative => (byte)(packed >> 8);
    public byte Positive => (byte)packed;
}