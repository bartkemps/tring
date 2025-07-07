namespace Ternary3.IO;

/// <summary>
/// Note: the consumer of this class needs to ensure it is either used for reading or for writing, not both.
/// </summary>
internal class BinaryTritEncoder(bool mustWriteMagicNumber = true)
{
    private bool mustWriteMagicNumber = mustWriteMagicNumber;
    private bool mustWriteSectionHeader = mustWriteMagicNumber;
    private int offset;
    private int workingValue;

    public IEnumerable<byte> Encode(IEnumerable<Int3T> source)
    {
        foreach (var int3T in source)
        {
            if (mustWriteMagicNumber)
            {
                yield return 245; // Magic number to indicate this is a binary trit encoding.
                mustWriteMagicNumber = false;
            }

            if (mustWriteSectionHeader)
            {
                yield return 244; // Magic number for the encoding format.
                mustWriteMagicNumber = false;
            }

            var val = (int)int3T + 13;
            switch (offset)
            {
                case 0:
                    workingValue = val;
                    offset = 3;
                    break;
                case 1:
                    workingValue += 3 * val;
                    offset = 4;
                    break;
                case 2:
                    workingValue += 9 * val;
                    yield return (byte)workingValue;
                    offset = 0;
                    break;
                case 3:
                    workingValue += 27 * (val % 9);
                    yield return (byte)workingValue;
                    workingValue = val / 9;
                    offset = 1;
                    break;
                default:
                    workingValue += 81 * (val % 3);
                    yield return (byte)workingValue;
                    workingValue = val / 3;
                    offset = 2;
                    break;
            }
        }
    }

    public IEnumerable<byte> Flush()
    {
        if (offset == 0) yield break;
        yield return offset < 3 ? (byte)(255 - workingValue) : (byte)workingValue;
        mustWriteSectionHeader = true;
    }

    public IEnumerable<Int3T> Decode(IEnumerable<byte> source)
    {
        foreach (var b in source)
        {
            switch (b)
            {
                case < 243:
                    // read 5 trits
                    switch (offset)
                    {
                        case 0:
                            workingValue = b % 27;
                            yield return (Int3T)(workingValue - 13);
                            workingValue = b / 27;
                            offset = 2;
                            continue;
                        case 1:
                            workingValue += 3 * (b % 9);
                            yield return (Int3T)(workingValue - 13);
                            workingValue = b / 9;
                            yield return (Int3T)(workingValue - 13);
                            offset = 0;
                            continue;
                        default: // offset = 2;
                            workingValue += 9 * (b % 3);
                            yield return (Int3T)(workingValue - 13);
                            workingValue = (b / 3) % 27;
                            yield return (Int3T)(workingValue - 13);
                            workingValue = b / 81;
                            offset = 1;
                            continue;
                    }
                case > 245:
                {
                    switch (offset)
                    {
                        case 1:
                            workingValue += 3 * ((255 - b) % 9);
                            yield return (Int3T)(workingValue - 13);
                            break;
                        case 2:
                            workingValue += 9 * ((255 - b) % 3);
                            yield return (Int3T)(workingValue - 13);
                            break;
                        // case 0 would be strange because there's no need ever to write like this,
                        // even if you need intermittent flushes
                    }

                    offset = 0;
                    break;
                }
                case 244 or 245:
                {
                    offset = 0;
                    break;
                }
            }
        }
    }
}