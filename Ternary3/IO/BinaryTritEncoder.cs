namespace Ternary3.IO;

internal class BinaryTritEncoder
{
    public IEnumerable<byte> Encode(IEnumerable<Int3T> source)
    {
        yield return 245; // Magic number to indicate this is a binary trit encoding.
        yield return 244; // Magic number for the encoding format.
        var offset = 0;
        int outputByte = 0;
        foreach (var int3T in source)
        {
            var val = (int)int3T + 13;
            switch (offset)
            {
                case 0:
                    outputByte = val;
                    offset = 3;
                    break;
                case 1:
                    outputByte += 3 * val;
                    offset = 4;
                    break;
                case 2:
                    outputByte += 9 * val;
                    yield return (byte)outputByte;
                    offset = 0;
                    break;
                case 3:
                    outputByte += 27 * (val % 9);
                    yield return (byte)outputByte;
                    outputByte = val / 9;
                    offset = 1;
                    break;
                default:
                    outputByte += 81 * (val % 3);
                    yield return (byte)outputByte;
                    outputByte = val / 3;
                    offset = 2;
                    break;
            }
        }

        if (offset == 0) yield break;
        // return the magically encoded rest
        yield return offset < 3 ? (byte)(255 - outputByte) : (byte)outputByte;
    }

    public IEnumerable<Int3T> Decode(IEnumerable<byte> source)
    {
        var offset = 0;
        int outputInt3T = 0;
        foreach (var b in source)
        {
            switch (b)
            {
                case < 243:
                    // read 5 trits
                    switch (offset)
                    {
                        case 0:
                            outputInt3T = b % 27;
                            yield return (Int3T)(outputInt3T - 13);
                            outputInt3T = b / 27;
                            offset = 2;
                            continue;
                        case 1:
                            outputInt3T += 3 * (b % 9);
                            yield return (Int3T)(outputInt3T - 13);
                            outputInt3T = b / 9;
                            yield return (Int3T)(outputInt3T - 13);
                            offset = 0;
                            continue;
                        default: // offset = 2;
                            outputInt3T += 9 * (b % 3);
                            yield return (Int3T)(outputInt3T - 13);
                            outputInt3T = (b / 3) % 27;
                            yield return (Int3T)(outputInt3T - 13);
                            outputInt3T = b / 81;
                            offset = 1;
                            continue;
                    }
                case > 245:
                {
                    switch (offset)
                    {
                        case 1:
                            outputInt3T += (255 - b) % 9;
                            yield return (Int3T)(outputInt3T - 13);
                            break;
                        case 2:
                            outputInt3T += (255 - b) % 3;
                            yield return (Int3T)(outputInt3T - 13);
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