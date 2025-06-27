namespace Ternary3.TritArrays;

using System.Numerics;
using System.Runtime.CompilerServices;

internal static class TritConverter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void To32Trits(Int32 value, out UInt32 negative, out UInt32 positive)
    {
        if (value is > -364 and < 364)
        {
            positive = LookupTrits(-(int)value);
            negative = LookupTrits((int)value);
            return;
        }

        var swap = false;
        if (value > 0)
        {
            swap = true;
            value = -value;
        }

        negative = 0;
        positive = 0;
        var index = 0;

        while (value != 0 && index < 32)
        {
            value = Math.DivRem(value, 729, out var remainder);
            if (remainder < -364)
            {
                remainder += 729;
                value--;
            }

            negative |= (UInt32)LookupTrits((int)remainder) << index;
            positive |= (UInt32)LookupTrits(-(int)remainder) << index;
            index += 6;
        }

        if (swap) (positive, negative) = (negative, positive);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void To32Trits(Int64 value, out UInt32 negative, out UInt32 positive)
    {
        if (value is > -364 and < 364)
        {
            positive = LookupTrits(-(int)value);
            negative = LookupTrits((int)value);
            return;
        }

        var swap = false;
        if (value > 0)
        {
            swap = true;
            value = -value;
        }

        negative = 0;
        positive = 0;
        var index = 0;

        while (value != 0 && index < 32)
        {
            value = Math.DivRem(value, 729, out var remainder);
            if (remainder < -364)
            {
                remainder += 729;
                value--;
            }

            negative |= (UInt32)LookupTrits((int)remainder) << index;
            positive |= (UInt32)LookupTrits(-(int)remainder) << index;
            index += 6;
        }

        if (swap) (positive, negative) = (negative, positive);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void To64Trits(Int64 value, out UInt64 negative, out UInt64 positive)
    {
        if (value is > -364 and < 364)
        {
            positive = LookupTrits(-(int)value);
            negative = LookupTrits((int)value);
            return;
        }

        var swap = false;
        if (value > 0)
        {
            swap = true;
            value = -value;
        }

        negative = 0;
        positive = 0;
        var index = 0;

        while (value != 0 && index < 64)
        {
            value = Math.DivRem(value, 729, out var remainder);
            if (remainder < -364)
            {
                remainder += 729;
                value--;
            }

            negative |= (UInt64)LookupTrits((int)remainder) << index;
            positive |= (UInt64)LookupTrits(-(int)remainder) << index;
            index += 6;
        }

        if (swap) (positive, negative) = (negative, positive);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void To64Trits(Int128 value, out UInt64 negative, out UInt64 positive)
    {
        if (value > -364 && value < 364)
        {
            positive = LookupTrits(-(int)value);
            negative = LookupTrits((int)value);
            return;
        }

        var swap = false;
        if (value > 0)
        {
            swap = true;
            value = -value;
        }

        negative = 0;
        positive = 0;
        var index = 0;

        while (value != 0 && index < 128)
        {
            var div = value / 729;
            var remainder = value - (div * 729);
            value = div;
            if (remainder < -364)
            {
                remainder += 729;
                value--;
            }

            negative |= (uint)LookupTrits((int)remainder) << index;
            positive |= (uint)LookupTrits(-(int)remainder) << index;
            index += 6;
        }

        if (swap) (positive, negative) = (negative, positive);
    }

    private static int GetLength(uint negative, uint positive)
    {
        var bits = negative | positive;
        if (bits < 8)
        {
            if (bits < 1) return 0;
            return bits < 4 ? 1 : 2;
        }
        if (bits < 64)
        {
            if (bits < 16) return 3;
            return bits < 32 ? 4 : 5;
        }
        throw new ArgumentOutOfRangeException(nameof(negative), "Value exceeds 6 bits");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ToTrits(BigInteger value, out List<ulong> negative, out List<ulong> positive, out int length)
    {
        // Each long holds 64 trits, so we fill them as we go.
        negative = [];
        positive = [];
        length = 0;

        if (value == 0) return;

        if (value > -364 && value < 364)
        {
            var pos = LookupTrits(-(int)value);
            var neg = LookupTrits((int)value);
            negative.Add(neg);
            positive.Add(pos);
            length = GetLength(neg, pos);
            return;
        }

        (var swap, value) = value > 0 ? (true, -value) : (false, value);

        ulong negWord = 0;
        ulong posWord = 0;
        var currentOffset = 0;

        while (value != 0)
        {
            int remainder;
            if (currentOffset < 60)
            {
                value = BigInteger.DivRem(value, 729, out var r);
                remainder = (int)r;
                if (remainder < -364)
                {
                    remainder += 729;
                    value -= 1;
                }

                if (AddTritsAndCheckIfDone(ref negative, ref positive, ref length, remainder)) return;
                currentOffset += 6;
            }
            else
            {
                value = BigInteger.DivRem(value, 81, out var r);
                remainder = (int)r;
                if (AddTritsAndCheckIfDone(ref negative, ref positive, ref length, remainder)) return;
                positive.Add(posWord);
                negative.Add(negWord);
                posWord = 0;
                negWord = 0;
                currentOffset = 0;
            }
        }

        if (swap)
        {
            (negative, positive) = (positive, negative);
        }

        bool AddTritsAndCheckIfDone(ref List<ulong> negative, ref List<ulong> positive, ref int length, int remainder)
        {
            var negBits = LookupTrits(remainder);
            var posBits = LookupTrits(-remainder);

            negWord |= (ulong)negBits << currentOffset;
            posWord |= (ulong)posBits << currentOffset;
            if (value != 0) return false;
            length += GetLength(negBits, posBits);;
            negative.Add(negWord);
            positive.Add(posWord);
            if (swap)
            {
                (negative, positive) = (positive, negative);
            }

            return true;
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int32 ToInt32(uint negative, uint positive)
    {
        var result = 0;
        var pow = 1;
        while (negative != 0 || positive != 0)
        {
            result += (LookupValue[positive & 0xff] - LookupValue[negative & 0xff]) * pow;
            negative >>= 8;
            positive >>= 8;
            pow *= 6561;
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int32 ToInt32(int negative, int positive)
    {
        var result = 0;
        var pow = 1;
        while (negative != 0 || positive != 0)
        {
            result += (LookupValue[positive & 0xff] - LookupValue[negative & 0xff]) * pow;
            negative >>= 8;
            positive >>= 8;
            pow *= 6561;
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int32 ToInt32(ushort negative, ushort positive)
    {
        var result = 0;
        var pow = 1;
        while (negative != 0 || positive != 0)
        {
            result += (LookupValue[positive & 0xff] - LookupValue[negative & 0xff]) * pow;
            negative >>= 8;
            positive >>= 8;
            pow *= 6561;
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int64 ToInt64(uint negative, uint positive)
    {
        Int64 result = 0;
        Int64 pow = 1;
        while (negative != 0 || positive != 0)
        {
            result += (LookupValue[positive & 0xff] - LookupValue[negative & 0xff]) * pow;
            negative >>= 8;
            positive >>= 8;
            pow *= 6561;
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int64 ToInt64(int negative, int positive)
    {
        Int64 result = 0;
        Int64 pow = 1;
        while (negative != 0 || positive != 0)
        {
            result += (LookupValue[positive & 0xff] - LookupValue[negative & 0xff]) * pow;
            negative >>= 8;
            positive >>= 8;
            pow *= 6561;
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int64 ToInt64(ulong negative, ulong positive)
    {
        Int64 result = 0;
        Int64 pow = 1;
        while (negative != 0 || positive != 0)
        {
            result += (LookupValue[positive & 0xff] - LookupValue[negative & 0xff]) * pow;
            negative >>= 8;
            positive >>= 8;
            pow *= 6561;
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int64 ToInt64(ushort negative, ushort positive)
    {
        Int64 result = 0;
        Int64 pow = 1;
        while (negative != 0 || positive != 0)
        {
            result += (LookupValue[positive & 0xff] - LookupValue[negative & 0xff]) * pow;
            negative >>= 8;
            positive >>= 8;
            pow *= 6561;
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int128 ToInt128(ulong negative, ulong positive)
    {
        Int128 result = 0;
        Int128 pow = 1;
        while (negative != 0 || positive != 0)
        {
            result += (LookupValue[positive & 0xff] - LookupValue[negative & 0xff]) * pow;
            negative >>= 8;
            positive >>= 8;
            pow *= 6561;
        }

        return result;
    }

    /// <summary>
    /// Converts an index in the range of -364 to 364 to a trit array representation.
    /// call Lookup(index) for the negative and Lookup(index) for the positive part.
    /// </summary>
    public static uint LookupTrits(int index) => index switch
    {
        -364 => 0b00111111,
        -363 => 0b00111110,
        -362 => 0b00111110,
        -361 => 0b00111101,
        -360 => 0b00111100,
        -359 => 0b00111100,
        -358 => 0b00111101,
        -357 => 0b00111100,
        -356 => 0b00111100,
        -355 => 0b00111011,
        -354 => 0b00111010,
        -353 => 0b00111010,
        -352 => 0b00111001,
        -351 => 0b00111000,
        -350 => 0b00111000,
        -349 => 0b00111001,
        -348 => 0b00111000,
        -347 => 0b00111000,
        -346 => 0b00111011,
        -345 => 0b00111010,
        -344 => 0b00111010,
        -343 => 0b00111001,
        -342 => 0b00111000,
        -341 => 0b00111000,
        -340 => 0b00111001,
        -339 => 0b00111000,
        -338 => 0b00111000,
        -337 => 0b00110111,
        -336 => 0b00110110,
        -335 => 0b00110110,
        -334 => 0b00110101,
        -333 => 0b00110100,
        -332 => 0b00110100,
        -331 => 0b00110101,
        -330 => 0b00110100,
        -329 => 0b00110100,
        -328 => 0b00110011,
        -327 => 0b00110010,
        -326 => 0b00110010,
        -325 => 0b00110001,
        -324 => 0b00110000,
        -323 => 0b00110000,
        -322 => 0b00110001,
        -321 => 0b00110000,
        -320 => 0b00110000,
        -319 => 0b00110011,
        -318 => 0b00110010,
        -317 => 0b00110010,
        -316 => 0b00110001,
        -315 => 0b00110000,
        -314 => 0b00110000,
        -313 => 0b00110001,
        -312 => 0b00110000,
        -311 => 0b00110000,
        -310 => 0b00110111,
        -309 => 0b00110110,
        -308 => 0b00110110,
        -307 => 0b00110101,
        -306 => 0b00110100,
        -305 => 0b00110100,
        -304 => 0b00110101,
        -303 => 0b00110100,
        -302 => 0b00110100,
        -301 => 0b00110011,
        -300 => 0b00110010,
        -299 => 0b00110010,
        -298 => 0b00110001,
        -297 => 0b00110000,
        -296 => 0b00110000,
        -295 => 0b00110001,
        -294 => 0b00110000,
        -293 => 0b00110000,
        -292 => 0b00110011,
        -291 => 0b00110010,
        -290 => 0b00110010,
        -289 => 0b00110001,
        -288 => 0b00110000,
        -287 => 0b00110000,
        -286 => 0b00110001,
        -285 => 0b00110000,
        -284 => 0b00110000,
        -283 => 0b00101111,
        -282 => 0b00101110,
        -281 => 0b00101110,
        -280 => 0b00101101,
        -279 => 0b00101100,
        -278 => 0b00101100,
        -277 => 0b00101101,
        -276 => 0b00101100,
        -275 => 0b00101100,
        -274 => 0b00101011,
        -273 => 0b00101010,
        -272 => 0b00101010,
        -271 => 0b00101001,
        -270 => 0b00101000,
        -269 => 0b00101000,
        -268 => 0b00101001,
        -267 => 0b00101000,
        -266 => 0b00101000,
        -265 => 0b00101011,
        -264 => 0b00101010,
        -263 => 0b00101010,
        -262 => 0b00101001,
        -261 => 0b00101000,
        -260 => 0b00101000,
        -259 => 0b00101001,
        -258 => 0b00101000,
        -257 => 0b00101000,
        -256 => 0b00100111,
        -255 => 0b00100110,
        -254 => 0b00100110,
        -253 => 0b00100101,
        -252 => 0b00100100,
        -251 => 0b00100100,
        -250 => 0b00100101,
        -249 => 0b00100100,
        -248 => 0b00100100,
        -247 => 0b00100011,
        -246 => 0b00100010,
        -245 => 0b00100010,
        -244 => 0b00100001,
        -243 => 0b00100000,
        -242 => 0b00100000,
        -241 => 0b00100001,
        -240 => 0b00100000,
        -239 => 0b00100000,
        -238 => 0b00100011,
        -237 => 0b00100010,
        -236 => 0b00100010,
        -235 => 0b00100001,
        -234 => 0b00100000,
        -233 => 0b00100000,
        -232 => 0b00100001,
        -231 => 0b00100000,
        -230 => 0b00100000,
        -229 => 0b00100111,
        -228 => 0b00100110,
        -227 => 0b00100110,
        -226 => 0b00100101,
        -225 => 0b00100100,
        -224 => 0b00100100,
        -223 => 0b00100101,
        -222 => 0b00100100,
        -221 => 0b00100100,
        -220 => 0b00100011,
        -219 => 0b00100010,
        -218 => 0b00100010,
        -217 => 0b00100001,
        -216 => 0b00100000,
        -215 => 0b00100000,
        -214 => 0b00100001,
        -213 => 0b00100000,
        -212 => 0b00100000,
        -211 => 0b00100011,
        -210 => 0b00100010,
        -209 => 0b00100010,
        -208 => 0b00100001,
        -207 => 0b00100000,
        -206 => 0b00100000,
        -205 => 0b00100001,
        -204 => 0b00100000,
        -203 => 0b00100000,
        -202 => 0b00101111,
        -201 => 0b00101110,
        -200 => 0b00101110,
        -199 => 0b00101101,
        -198 => 0b00101100,
        -197 => 0b00101100,
        -196 => 0b00101101,
        -195 => 0b00101100,
        -194 => 0b00101100,
        -193 => 0b00101011,
        -192 => 0b00101010,
        -191 => 0b00101010,
        -190 => 0b00101001,
        -189 => 0b00101000,
        -188 => 0b00101000,
        -187 => 0b00101001,
        -186 => 0b00101000,
        -185 => 0b00101000,
        -184 => 0b00101011,
        -183 => 0b00101010,
        -182 => 0b00101010,
        -181 => 0b00101001,
        -180 => 0b00101000,
        -179 => 0b00101000,
        -178 => 0b00101001,
        -177 => 0b00101000,
        -176 => 0b00101000,
        -175 => 0b00100111,
        -174 => 0b00100110,
        -173 => 0b00100110,
        -172 => 0b00100101,
        -171 => 0b00100100,
        -170 => 0b00100100,
        -169 => 0b00100101,
        -168 => 0b00100100,
        -167 => 0b00100100,
        -166 => 0b00100011,
        -165 => 0b00100010,
        -164 => 0b00100010,
        -163 => 0b00100001,
        -162 => 0b00100000,
        -161 => 0b00100000,
        -160 => 0b00100001,
        -159 => 0b00100000,
        -158 => 0b00100000,
        -157 => 0b00100011,
        -156 => 0b00100010,
        -155 => 0b00100010,
        -154 => 0b00100001,
        -153 => 0b00100000,
        -152 => 0b00100000,
        -151 => 0b00100001,
        -150 => 0b00100000,
        -149 => 0b00100000,
        -148 => 0b00100111,
        -147 => 0b00100110,
        -146 => 0b00100110,
        -145 => 0b00100101,
        -144 => 0b00100100,
        -143 => 0b00100100,
        -142 => 0b00100101,
        -141 => 0b00100100,
        -140 => 0b00100100,
        -139 => 0b00100011,
        -138 => 0b00100010,
        -137 => 0b00100010,
        -136 => 0b00100001,
        -135 => 0b00100000,
        -134 => 0b00100000,
        -133 => 0b00100001,
        -132 => 0b00100000,
        -131 => 0b00100000,
        -130 => 0b00100011,
        -129 => 0b00100010,
        -128 => 0b00100010,
        -127 => 0b00100001,
        -126 => 0b00100000,
        -125 => 0b00100000,
        -124 => 0b00100001,
        -123 => 0b00100000,
        -122 => 0b00100000,
        -121 => 0b00011111,
        -120 => 0b00011110,
        -119 => 0b00011110,
        -118 => 0b00011101,
        -117 => 0b00011100,
        -116 => 0b00011100,
        -115 => 0b00011101,
        -114 => 0b00011100,
        -113 => 0b00011100,
        -112 => 0b00011011,
        -111 => 0b00011010,
        -110 => 0b00011010,
        -109 => 0b00011001,
        -108 => 0b00011000,
        -107 => 0b00011000,
        -106 => 0b00011001,
        -105 => 0b00011000,
        -104 => 0b00011000,
        -103 => 0b00011011,
        -102 => 0b00011010,
        -101 => 0b00011010,
        -100 => 0b00011001,
        -99 => 0b00011000,
        -98 => 0b00011000,
        -97 => 0b00011001,
        -96 => 0b00011000,
        -95 => 0b00011000,
        -94 => 0b00010111,
        -93 => 0b00010110,
        -92 => 0b00010110,
        -91 => 0b00010101,
        -90 => 0b00010100,
        -89 => 0b00010100,
        -88 => 0b00010101,
        -87 => 0b00010100,
        -86 => 0b00010100,
        -85 => 0b00010011,
        -84 => 0b00010010,
        -83 => 0b00010010,
        -82 => 0b00010001,
        -81 => 0b00010000,
        -80 => 0b00010000,
        -79 => 0b00010001,
        -78 => 0b00010000,
        -77 => 0b00010000,
        -76 => 0b00010011,
        -75 => 0b00010010,
        -74 => 0b00010010,
        -73 => 0b00010001,
        -72 => 0b00010000,
        -71 => 0b00010000,
        -70 => 0b00010001,
        -69 => 0b00010000,
        -68 => 0b00010000,
        -67 => 0b00010111,
        -66 => 0b00010110,
        -65 => 0b00010110,
        -64 => 0b00010101,
        -63 => 0b00010100,
        -62 => 0b00010100,
        -61 => 0b00010101,
        -60 => 0b00010100,
        -59 => 0b00010100,
        -58 => 0b00010011,
        -57 => 0b00010010,
        -56 => 0b00010010,
        -55 => 0b00010001,
        -54 => 0b00010000,
        -53 => 0b00010000,
        -52 => 0b00010001,
        -51 => 0b00010000,
        -50 => 0b00010000,
        -49 => 0b00010011,
        -48 => 0b00010010,
        -47 => 0b00010010,
        -46 => 0b00010001,
        -45 => 0b00010000,
        -44 => 0b00010000,
        -43 => 0b00010001,
        -42 => 0b00010000,
        -41 => 0b00010000,
        -40 => 0b00001111,
        -39 => 0b00001110,
        -38 => 0b00001110,
        -37 => 0b00001101,
        -36 => 0b00001100,
        -35 => 0b00001100,
        -34 => 0b00001101,
        -33 => 0b00001100,
        -32 => 0b00001100,
        -31 => 0b00001011,
        -30 => 0b00001010,
        -29 => 0b00001010,
        -28 => 0b00001001,
        -27 => 0b00001000,
        -26 => 0b00001000,
        -25 => 0b00001001,
        -24 => 0b00001000,
        -23 => 0b00001000,
        -22 => 0b00001011,
        -21 => 0b00001010,
        -20 => 0b00001010,
        -19 => 0b00001001,
        -18 => 0b00001000,
        -17 => 0b00001000,
        -16 => 0b00001001,
        -15 => 0b00001000,
        -14 => 0b00001000,
        -13 => 0b00000111,
        -12 => 0b00000110,
        -11 => 0b00000110,
        -10 => 0b00000101,
        -9 => 0b00000100,
        -8 => 0b00000100,
        -7 => 0b00000101,
        -6 => 0b00000100,
        -5 => 0b00000100,
        -4 => 0b00000011,
        -3 => 0b00000010,
        -2 => 0b00000010,
        -1 => 0b00000001,
        0 => 0b00000000,
        1 => 0b00000000,
        2 => 0b00000001,
        3 => 0b00000000,
        4 => 0b00000000,
        5 => 0b00000011,
        6 => 0b00000010,
        7 => 0b00000010,
        8 => 0b00000001,
        9 => 0b00000000,
        10 => 0b00000000,
        11 => 0b00000001,
        12 => 0b00000000,
        13 => 0b00000000,
        14 => 0b00000111,
        15 => 0b00000110,
        16 => 0b00000110,
        17 => 0b00000101,
        18 => 0b00000100,
        19 => 0b00000100,
        20 => 0b00000101,
        21 => 0b00000100,
        22 => 0b00000100,
        23 => 0b00000011,
        24 => 0b00000010,
        25 => 0b00000010,
        26 => 0b00000001,
        27 => 0b00000000,
        28 => 0b00000000,
        29 => 0b00000001,
        30 => 0b00000000,
        31 => 0b00000000,
        32 => 0b00000011,
        33 => 0b00000010,
        34 => 0b00000010,
        35 => 0b00000001,
        36 => 0b00000000,
        37 => 0b00000000,
        38 => 0b00000001,
        39 => 0b00000000,
        40 => 0b00000000,
        41 => 0b00001111,
        42 => 0b00001110,
        43 => 0b00001110,
        44 => 0b00001101,
        45 => 0b00001100,
        46 => 0b00001100,
        47 => 0b00001101,
        48 => 0b00001100,
        49 => 0b00001100,
        50 => 0b00001011,
        51 => 0b00001010,
        52 => 0b00001010,
        53 => 0b00001001,
        54 => 0b00001000,
        55 => 0b00001000,
        56 => 0b00001001,
        57 => 0b00001000,
        58 => 0b00001000,
        59 => 0b00001011,
        60 => 0b00001010,
        61 => 0b00001010,
        62 => 0b00001001,
        63 => 0b00001000,
        64 => 0b00001000,
        65 => 0b00001001,
        66 => 0b00001000,
        67 => 0b00001000,
        68 => 0b00000111,
        69 => 0b00000110,
        70 => 0b00000110,
        71 => 0b00000101,
        72 => 0b00000100,
        73 => 0b00000100,
        74 => 0b00000101,
        75 => 0b00000100,
        76 => 0b00000100,
        77 => 0b00000011,
        78 => 0b00000010,
        79 => 0b00000010,
        80 => 0b00000001,
        81 => 0b00000000,
        82 => 0b00000000,
        83 => 0b00000001,
        84 => 0b00000000,
        85 => 0b00000000,
        86 => 0b00000011,
        87 => 0b00000010,
        88 => 0b00000010,
        89 => 0b00000001,
        90 => 0b00000000,
        91 => 0b00000000,
        92 => 0b00000001,
        93 => 0b00000000,
        94 => 0b00000000,
        95 => 0b00000111,
        96 => 0b00000110,
        97 => 0b00000110,
        98 => 0b00000101,
        99 => 0b00000100,
        100 => 0b00000100,
        101 => 0b00000101,
        102 => 0b00000100,
        103 => 0b00000100,
        104 => 0b00000011,
        105 => 0b00000010,
        106 => 0b00000010,
        107 => 0b00000001,
        108 => 0b00000000,
        109 => 0b00000000,
        110 => 0b00000001,
        111 => 0b00000000,
        112 => 0b00000000,
        113 => 0b00000011,
        114 => 0b00000010,
        115 => 0b00000010,
        116 => 0b00000001,
        117 => 0b00000000,
        118 => 0b00000000,
        119 => 0b00000001,
        120 => 0b00000000,
        121 => 0b00000000,
        122 => 0b00011111,
        123 => 0b00011110,
        124 => 0b00011110,
        125 => 0b00011101,
        126 => 0b00011100,
        127 => 0b00011100,
        128 => 0b00011101,
        129 => 0b00011100,
        130 => 0b00011100,
        131 => 0b00011011,
        132 => 0b00011010,
        133 => 0b00011010,
        134 => 0b00011001,
        135 => 0b00011000,
        136 => 0b00011000,
        137 => 0b00011001,
        138 => 0b00011000,
        139 => 0b00011000,
        140 => 0b00011011,
        141 => 0b00011010,
        142 => 0b00011010,
        143 => 0b00011001,
        144 => 0b00011000,
        145 => 0b00011000,
        146 => 0b00011001,
        147 => 0b00011000,
        148 => 0b00011000,
        149 => 0b00010111,
        150 => 0b00010110,
        151 => 0b00010110,
        152 => 0b00010101,
        153 => 0b00010100,
        154 => 0b00010100,
        155 => 0b00010101,
        156 => 0b00010100,
        157 => 0b00010100,
        158 => 0b00010011,
        159 => 0b00010010,
        160 => 0b00010010,
        161 => 0b00010001,
        162 => 0b00010000,
        163 => 0b00010000,
        164 => 0b00010001,
        165 => 0b00010000,
        166 => 0b00010000,
        167 => 0b00010011,
        168 => 0b00010010,
        169 => 0b00010010,
        170 => 0b00010001,
        171 => 0b00010000,
        172 => 0b00010000,
        173 => 0b00010001,
        174 => 0b00010000,
        175 => 0b00010000,
        176 => 0b00010111,
        177 => 0b00010110,
        178 => 0b00010110,
        179 => 0b00010101,
        180 => 0b00010100,
        181 => 0b00010100,
        182 => 0b00010101,
        183 => 0b00010100,
        184 => 0b00010100,
        185 => 0b00010011,
        186 => 0b00010010,
        187 => 0b00010010,
        188 => 0b00010001,
        189 => 0b00010000,
        190 => 0b00010000,
        191 => 0b00010001,
        192 => 0b00010000,
        193 => 0b00010000,
        194 => 0b00010011,
        195 => 0b00010010,
        196 => 0b00010010,
        197 => 0b00010001,
        198 => 0b00010000,
        199 => 0b00010000,
        200 => 0b00010001,
        201 => 0b00010000,
        202 => 0b00010000,
        203 => 0b00001111,
        204 => 0b00001110,
        205 => 0b00001110,
        206 => 0b00001101,
        207 => 0b00001100,
        208 => 0b00001100,
        209 => 0b00001101,
        210 => 0b00001100,
        211 => 0b00001100,
        212 => 0b00001011,
        213 => 0b00001010,
        214 => 0b00001010,
        215 => 0b00001001,
        216 => 0b00001000,
        217 => 0b00001000,
        218 => 0b00001001,
        219 => 0b00001000,
        220 => 0b00001000,
        221 => 0b00001011,
        222 => 0b00001010,
        223 => 0b00001010,
        224 => 0b00001001,
        225 => 0b00001000,
        226 => 0b00001000,
        227 => 0b00001001,
        228 => 0b00001000,
        229 => 0b00001000,
        230 => 0b00000111,
        231 => 0b00000110,
        232 => 0b00000110,
        233 => 0b00000101,
        234 => 0b00000100,
        235 => 0b00000100,
        236 => 0b00000101,
        237 => 0b00000100,
        238 => 0b00000100,
        239 => 0b00000011,
        240 => 0b00000010,
        241 => 0b00000010,
        242 => 0b00000001,
        243 => 0b00000000,
        244 => 0b00000000,
        245 => 0b00000001,
        246 => 0b00000000,
        247 => 0b00000000,
        248 => 0b00000011,
        249 => 0b00000010,
        250 => 0b00000010,
        251 => 0b00000001,
        252 => 0b00000000,
        253 => 0b00000000,
        254 => 0b00000001,
        255 => 0b00000000,
        256 => 0b00000000,
        257 => 0b00000111,
        258 => 0b00000110,
        259 => 0b00000110,
        260 => 0b00000101,
        261 => 0b00000100,
        262 => 0b00000100,
        263 => 0b00000101,
        264 => 0b00000100,
        265 => 0b00000100,
        266 => 0b00000011,
        267 => 0b00000010,
        268 => 0b00000010,
        269 => 0b00000001,
        270 => 0b00000000,
        271 => 0b00000000,
        272 => 0b00000001,
        273 => 0b00000000,
        274 => 0b00000000,
        275 => 0b00000011,
        276 => 0b00000010,
        277 => 0b00000010,
        278 => 0b00000001,
        279 => 0b00000000,
        280 => 0b00000000,
        281 => 0b00000001,
        282 => 0b00000000,
        283 => 0b00000000,
        284 => 0b00001111,
        285 => 0b00001110,
        286 => 0b00001110,
        287 => 0b00001101,
        288 => 0b00001100,
        289 => 0b00001100,
        290 => 0b00001101,
        291 => 0b00001100,
        292 => 0b00001100,
        293 => 0b00001011,
        294 => 0b00001010,
        295 => 0b00001010,
        296 => 0b00001001,
        297 => 0b00001000,
        298 => 0b00001000,
        299 => 0b00001001,
        300 => 0b00001000,
        301 => 0b00001000,
        302 => 0b00001011,
        303 => 0b00001010,
        304 => 0b00001010,
        305 => 0b00001001,
        306 => 0b00001000,
        307 => 0b00001000,
        308 => 0b00001001,
        309 => 0b00001000,
        310 => 0b00001000,
        311 => 0b00000111,
        312 => 0b00000110,
        313 => 0b00000110,
        314 => 0b00000101,
        315 => 0b00000100,
        316 => 0b00000100,
        317 => 0b00000101,
        318 => 0b00000100,
        319 => 0b00000100,
        320 => 0b00000011,
        321 => 0b00000010,
        322 => 0b00000010,
        323 => 0b00000001,
        324 => 0b00000000,
        325 => 0b00000000,
        326 => 0b00000001,
        327 => 0b00000000,
        328 => 0b00000000,
        329 => 0b00000011,
        330 => 0b00000010,
        331 => 0b00000010,
        332 => 0b00000001,
        333 => 0b00000000,
        334 => 0b00000000,
        335 => 0b00000001,
        336 => 0b00000000,
        337 => 0b00000000,
        338 => 0b00000111,
        339 => 0b00000110,
        340 => 0b00000110,
        341 => 0b00000101,
        342 => 0b00000100,
        343 => 0b00000100,
        344 => 0b00000101,
        345 => 0b00000100,
        346 => 0b00000100,
        347 => 0b00000011,
        348 => 0b00000010,
        349 => 0b00000010,
        350 => 0b00000001,
        351 => 0b00000000,
        352 => 0b00000000,
        353 => 0b00000001,
        354 => 0b00000000,
        355 => 0b00000000,
        356 => 0b00000011,
        357 => 0b00000010,
        358 => 0b00000010,
        359 => 0b00000001,
        360 => 0b00000000,
        361 => 0b00000000,
        362 => 0b00000001,
        363 => 0b00000000,
        364 => 0b00000000,
        _ => throw new ArgumentOutOfRangeException(nameof(index))
    };

    /// <summary>
    /// Converts the positive trit bytearray to the actual value.
    /// </summary>
    public static int[] LookupValue =
    [
        0,
        1,
        3,
        4,
        9,
        10,
        12,
        13,
        27,
        28,
        30,
        31,
        36,
        37,
        39,
        40,
        81,
        82,
        84,
        85,
        90,
        91,
        93,
        94,
        108,
        109,
        111,
        112,
        117,
        118,
        120,
        121,
        243,
        244,
        246,
        247,
        252,
        253,
        255,
        256,
        270,
        271,
        273,
        274,
        279,
        280,
        282,
        283,
        324,
        325,
        327,
        328,
        333,
        334,
        336,
        337,
        351,
        352,
        354,
        355,
        360,
        361,
        363,
        364,
        729,
        730,
        732,
        733,
        738,
        739,
        741,
        742,
        756,
        757,
        759,
        760,
        765,
        766,
        768,
        769,
        810,
        811,
        813,
        814,
        819,
        820,
        822,
        823,
        837,
        838,
        840,
        841,
        846,
        847,
        849,
        850,
        972,
        973,
        975,
        976,
        981,
        982,
        984,
        985,
        999,
        1000,
        1002,
        1003,
        1008,
        1009,
        1011,
        1012,
        1053,
        1054,
        1056,
        1057,
        1062,
        1063,
        1065,
        1066,
        1080,
        1081,
        1083,
        1084,
        1089,
        1090,
        1092,
        1093,
        2187,
        2188,
        2190,
        2191,
        2196,
        2197,
        2199,
        2200,
        2214,
        2215,
        2217,
        2218,
        2223,
        2224,
        2226,
        2227,
        2268,
        2269,
        2271,
        2272,
        2277,
        2278,
        2280,
        2281,
        2295,
        2296,
        2298,
        2299,
        2304,
        2305,
        2307,
        2308,
        2430,
        2431,
        2433,
        2434,
        2439,
        2440,
        2442,
        2443,
        2457,
        2458,
        2460,
        2461,
        2466,
        2467,
        2469,
        2470,
        2511,
        2512,
        2514,
        2515,
        2520,
        2521,
        2523,
        2524,
        2538,
        2539,
        2541,
        2542,
        2547,
        2548,
        2550,
        2551,
        2916,
        2917,
        2919,
        2920,
        2925,
        2926,
        2928,
        2929,
        2943,
        2944,
        2946,
        2947,
        2952,
        2953,
        2955,
        2956,
        2997,
        2998,
        3000,
        3001,
        3006,
        3007,
        3009,
        3010,
        3024,
        3025,
        3027,
        3028,
        3033,
        3034,
        3036,
        3037,
        3159,
        3160,
        3162,
        3163,
        3168,
        3169,
        3171,
        3172,
        3186,
        3187,
        3189,
        3190,
        3195,
        3196,
        3198,
        3199,
        3240,
        3241,
        3243,
        3244,
        3249,
        3250,
        3252,
        3253,
        3267,
        3268,
        3270,
        3271,
        3276,
        3277,
        3279,
        3280
    ];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Trit GetTrit(byte negative, byte positive, int index)
        => new(((positive >> index) & 1) - ((negative >> index) & 1));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Trit GetTrit(ushort negative, ushort positive, int index)
        => new(((positive >> index) & 1) - ((negative >> index) & 1));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Trit GetTrit(uint negative, uint positive, int index)
        => new((int)((positive >> index) & 1) - (int)((negative >> index) & 1));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Trit GetTrit(ulong negative, ulong positive, int index)
        => new((int)((positive >> index) & 1) - (int)((negative >> index) & 1));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Trit GetTrit(List<ulong> negative, List<ulong> positive, int index)
    {
        var longIndex = index / 64;
        var bitIndex = index % 64;
        var pos = (positive[longIndex] & (1UL << bitIndex)) != 0;
        var neg = (negative[longIndex] & (1UL << bitIndex)) != 0;
        return (Trit)((pos ? 1 : 0) - (neg ? 1 : 0));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetTrit(ref byte negative, ref byte positive, int index, Trit value)
    {
        var mask = 1 << index;
        switch (value.Value)
        {
            case 1:
                positive |= (byte)mask;
                negative &= (byte)~mask;
                break;
            case -1:
                positive &= (byte)~mask;
                negative |= (byte)mask;
                break;
            default: // case 0
                positive &= (byte)~mask;
                negative &= (byte)~mask;
                break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetTrit(ref ushort negative, ref ushort positive, int index, Trit value)
    {
        var mask = 1 << index;
        switch (value.Value)
        {
            case 1:
                positive |= (ushort)mask;
                negative &= (ushort)~mask;
                break;
            case -1:
                positive &= (ushort)~mask;
                negative |= (ushort)mask;
                break;
            default: // case 0
                positive &= (ushort)~mask;
                negative &= (ushort)~mask;
                break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetTrit(ref uint negative, ref uint positive, int index, Trit value)
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetTrit(ref List<ulong> negative, ref List<ulong> positive, int index, Trit value)
    {
        var longIndex = index / 64;
        var bitIndex = index % 64;
        switch (value.Value)
        {
            case 1:
                positive[longIndex] |= (1UL << bitIndex);
                negative[longIndex] &= ~(1UL << bitIndex);
                break;
            case -1:
                positive[longIndex] &= ~(1UL << bitIndex);
                negative[longIndex] |= (1UL << bitIndex);
                break;
            default:
                positive[longIndex] &= ~(1UL << bitIndex);
                negative[longIndex] &= ~(1UL << bitIndex);
                break;
        }
    }

    public static string FormatTrits(ulong negative, ulong positive, int length)
    {
        var space = (length - 1) / 9;
        var chars = new Span<char>(new char[length + space]);
        for (var i = length + space - 10; i > 0; i -= 10)
        {
            chars[i] = ' ';
        }

        for (var i = 0; i < length; i++)
        {
            chars[space + length - i - 1] = ((negative >> i) & 1, (positive >> i) & 1) switch
            {
                (0, 0) => '0',
                (0, 1) => '1',
                (1, 0) => 'T',
                _ => '?'
            };
            if (i % 9 == 8) space--;
        }

        return new(chars);
    }
}