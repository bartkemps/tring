namespace Ternary3.Tests.Formatting;

using FluentAssertions;
using Ternary3.Formatting;


public class ParserTests
{
    private static readonly TernaryFormat? format = new()
    {
        DecimalSeparator = "--",
        NegativeTritDigit = ' ',
        ZeroTritDigit = '+',
        PositiveTritDigit = 'x',
        Groups = new List<TritGroupDefinition>
        {
            new ("X", 3),
            new ("O", 2)
        },
        TernaryPadding = TernaryPadding.Full
    };

    public static object[][] ParseTestData =
    [
        // Format: string, ITernaryFormat, TritParseOptions
        ["zzz", null, TritParseOptions.AllowInvalidCharacters],
        ["zzz", null, TritParseOptions.AllowAbsenceOfDigits],
        ["zzz", null, TritParseOptions.AllowAbsenceOfDigits | TritParseOptions.AllowInvalidCharacters],
        ["0", null, TritParseOptions.Default],
        ["1", null, TritParseOptions.Default],
        ["T", null, TritParseOptions.Default],
        ["10", null, TritParseOptions.Default],
        ["T0", null, TritParseOptions.Default],
        ["1T", null, TritParseOptions.Default],
        ["TT", null, TritParseOptions.Default],
        ["1_0", null, TritParseOptions.AllowUnderscores],
        [" 1 0 ", null, TritParseOptions.AllowWhitespace],
        ["1-0", null, TritParseOptions.AllowDashes],
        ["1,0", null, TritParseOptions.AllowGroupSerparators],
        ["1.0", null, TritParseOptions.AllowDecimal],
        ["t0", null, TritParseOptions.CaseInsensitive],
        ["T0", null, TritParseOptions.CaseInsensitive],
        ["1T0", null, TritParseOptions.Default],
        ["TTT", null, TritParseOptions.Default],
        ["111", null, TritParseOptions.Default],
        ["T1T", null, TritParseOptions.Default],
        ["0T1", null, TritParseOptions.Default],
        ["T10", null, TritParseOptions.Default],
        ["T1T0", null, TritParseOptions.Default],
        ["1T0T", null, TritParseOptions.Default],
        ["T0T1", null, TritParseOptions.Default],
        ["1T0T1", null, TritParseOptions.Default],
        ["T1T0T", null, TritParseOptions.Default],
        ["T0T1T", null, TritParseOptions.Default],
        [" +xX0+xO +xX0+x", format, TritParseOptions.Default],
        [" +xX0+xO +xX0+x", format, TritParseOptions.None]
    ];
    
    [Theory]
    [MemberData(nameof(ParseTestData))]
    public void Int3T_Parse_Equals_TritArray3_Parse(string s, ITernaryFormat? format, TritParseOptions options)
    {
        Int3T val1 = default;
        Int3T val2 = default;
        Exception? ex1 = null;
        Exception? ex2 = null;
        try { val1 = Int3T.Parse(s, format, options); }
        catch (Exception ex) { ex1 = ex; }
        try { val2 = TritArray3.Parse(s, format, options); }
        catch (Exception ex) { ex2 = ex; }

        val1.Should().Be(val2);
        ex1?.GetType().Should().Be(ex2?.GetType());
        ex1?.Message.Should().Be(ex2?.Message);
    }

    [Theory]
    [MemberData(nameof(ParseTestData))]
    public void Int9T_Parse_Equals_TritArray9_Parse(string s, ITernaryFormat format, TritParseOptions options)
    {
        Int9T val1 = default;
        Int9T val2 = default;
        Exception ex1 = null!;
        Exception ex2 = null!;
        try { val1 = Int9T.Parse(s, format, options); }
        catch (Exception ex) { ex1 = ex; }
        try { val2 = TritArray9.Parse(s, format, options); }
        catch (Exception ex) { ex2 = ex; }

        val1.Should().Be(val2);
        ex1?.GetType().Should().Be(ex2?.GetType());
        ex1?.Message.Should().Be(ex2?.Message);
    }

    [Theory]
    [MemberData(nameof(ParseTestData))]
    public void Int27T_Parse_Equals_TritArray27_Parse(string s, ITernaryFormat format, TritParseOptions options)
    {
        Int27T val1 = default;
        Int27T val2 = default;
        Exception ex1 = null!;
        Exception ex2 = null!;
        try { val1 = Int27T.Parse(s, format, options); }
        catch (Exception ex) { ex1 = ex; }
        try { val2 = TritArray27.Parse(s, format, options); }
        catch (Exception ex) { ex2 = ex; }

        val1.Should().Be(val2);
        ex1?.GetType().Should().Be(ex2?.GetType());
        ex1?.Message.Should().Be(ex2?.Message);
    }
}