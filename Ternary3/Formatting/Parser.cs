namespace Ternary3.Formatting;

using System.Numerics;
using System.Runtime.CompilerServices;

/// <summary>
/// Provides functionality for parsing ternary number strings into Int3T values.
/// </summary>
internal class Parser
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int3T ParseInt3T(string characters, ITernaryFormat? format = null, TritParseOptions options = TritParseOptions.Default)
    {
        var trits = GetNormalizedDigits(characters, format, options, 3);
        return TritsToNumber<sbyte>(trits);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int9T ParseInt9T(string characters, ITernaryFormat? format = null, TritParseOptions options = TritParseOptions.Default)
    {
        var trits = GetNormalizedDigits(characters, format, options, 9);
        return TritsToNumber<short>(trits);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int27T ParseInt27T(string characters, ITernaryFormat? format = null, TritParseOptions options = TritParseOptions.Default)
    {
        var trits = GetNormalizedDigits(characters, format, options, 27);
        return TritsToNumber<int>(trits);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TritArray3 ParseTritArray3(string characters, ITernaryFormat? format = null, TritParseOptions options = TritParseOptions.Default)
    {
        var trits = GetNormalizedDigits(characters, format, options, 3);
        TritsToTritArray<byte>(trits, out var negative, out var positive);
        return new (negative, positive);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TritArray9 ParseTritArray9(string characters, ITernaryFormat? format = null, TritParseOptions options = TritParseOptions.Default)
    {
        var trits = GetNormalizedDigits(characters, format, options, 3);
        TritsToTritArray<byte>(trits, out var negative, out var positive);
        return new (negative, positive);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TritArray27 ParseTritArray27(string characters, ITernaryFormat? format = null, TritParseOptions options = TritParseOptions.Default)
    {
        var trits = GetNormalizedDigits(characters, format, options, 3);
        TritsToTritArray<byte>(trits, out var negative, out var positive);
        return new (negative, positive);
    }

    private static sbyte[] GetNormalizedDigits(string characters, ITernaryFormat? format, TritParseOptions options, int count)
    {
        ArgumentNullException.ThrowIfNull(characters);
        format ??= TernaryFormat.Current;
        if (!TernaryFormatValidator.Validate(format, options))
        {
            throw new FormatException("The provided format is invalid for parsing.");
        }

        var result = new sbyte[count];
        var digitIndex = 0;
        var groups = characters.Split(format.DecimalSeparator);
        if (groups.Length > 2)
        {
            throw new FormatException("Input string contains more than one decimal separator.");
        }

        if (groups.Length == 2 && !options.HasFlag(TritParseOptions.AllowDecimal))
        {
            throw new FormatException("Input string contains a decimal separator but AllowDecimal option is not set.");
        }

        var actualCharacters =
            (options.HasFlag(TritParseOptions.AllowGroupSerparators), options.HasFlag(TritParseOptions.CaseInsensitive)) switch
            {
                (false, false) => groups[0],
                (false, true) => groups[0].ToUpperInvariant(),
                (true, false) => format.Groups.Aggregate(groups[0], (current, group) => current.Replace(group.Separator, string.Empty)),
                (true, true) => format.Groups.Aggregate(groups[0].ToUpperInvariant(), (current, group) => current.Replace(group.Separator, string.Empty, StringComparison.OrdinalIgnoreCase))
            };

        var done = false;
        var negative = format.NegativeTritDigit;
        var zero = format.ZeroTritDigit;
        var positive = format.PositiveTritDigit;
        if (options.HasFlag(TritParseOptions.CaseInsensitive))
        {
            negative = char.ToUpperInvariant(negative);
            zero = char.ToUpperInvariant(zero);
            positive = char.ToUpperInvariant(positive);
        }

        foreach (var c in actualCharacters.Reverse())
        {
            // Handle whitespace
            if (char.IsWhiteSpace(c) && options.HasFlag(TritParseOptions.AllowWhitespace)) continue;

            // Handle dashes
            if (c == '-' && options.HasFlag(TritParseOptions.AllowDashes) && c != format.NegativeTritDigit) continue;

            // Handle underscores
            if (c == '_' && options.HasFlag(TritParseOptions.AllowUnderscores)) continue;

            // Handle group separators
            if (options.HasFlag(TritParseOptions.AllowGroupSerparators) && format.Groups.Any(g => g.Separator.Contains(c))) continue;

            if (c == negative)
            {
                AddChar(-1);
                continue;
            }

            if (c == zero)
            {
                AddChar(0);
                continue;
            }

            if (c == positive)
            {
                AddChar(1);
                continue;
            }

            if (!options.HasFlag(TritParseOptions.AllowInvalidCharacters)) throw new FormatException($"Invalid character '{c}' in input string.");
        }

        if (digitIndex == 0 && !options.HasFlag(TritParseOptions.AllowAbsenceOfDigits))
        {
            throw new FormatException("Input string contains no valid trit digits.");
        }

        return result;

        void AddChar(sbyte value)
        {
            if (done)
            {
                if (options.HasFlag(TritParseOptions.AllowOverflow)) return; // Ignore the rest of the digits
                throw new FormatException("Input string contains more digits than the specified maximum number of trits.");
            }

            result[digitIndex++] = value;
            if (digitIndex == count) done = true;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T TritsToNumber<T>(sbyte[] trits) where T : IBinaryInteger<T>
    {
        T result = T.Zero;
        T power = T.One;
        foreach (var trit in trits)
        {
            if (trit != 0) result += T.CreateChecked(trit) * power;
            power *= T.CreateChecked(3);
        }
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void TritsToTritArray<T>(sbyte[] trits, out T negative, out T positive) where T : IBinaryInteger<T>
    {
        negative = T.Zero;
        positive = T.Zero;
        for (var i = 0; i < trits.Length; i++)
        {
            if (trits[i] == -1) negative |= T.One << i;
            else if (trits[i] == 1) positive |= T.One << i;
        }
    }
}