using FluentAssertions;

namespace Ternary3.Tests.Formatting;

using Ternary3.Formatting;

public class TernaryFormatTests
{
    [Fact]
    public void Constructor_CopiesFromOther()
    {
        var original = new TernaryFormat
        {
            NegativeTritDigit = 'A',
            ZeroTritDigit = 'B',
            PositiveTritDigit = 'C',
            Groups = new List<TritGroupDefinition> { new(",",2) },
            DecimalSeparator = ";",
            TernaryPadding = TernaryPadding.Group
        };
        var copy = new TernaryFormat(original);
        copy.NegativeTritDigit.Should().Be('A');
        copy.ZeroTritDigit.Should().Be('B');
        copy.PositiveTritDigit.Should().Be('C');
        copy.Groups[0].Size.Should().Be(2);
        copy.Groups[0].Separator.Should().Be(",");
        copy.DecimalSeparator.Should().Be(";");
        copy.TernaryPadding.Should().Be(TernaryPadding.Group);
    }

    [Fact]
    public void WithGroup_AddsGroup()
    {
        var format = new TernaryFormat();
        format.WithGroup(4, ":");
        format.Groups[^1].Size.Should().Be(4);
        format.Groups[^1].Separator.Should().Be(":");
    }

    [Fact]
    public void ClearGroups_RemovesAllGroups()
    {
        var format = new TernaryFormat();
        format.ClearGroups();
        format.Groups.Should().BeEmpty();
    }
}

