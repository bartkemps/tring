namespace Examples;

using Ternary3;
using Ternary3.Formatting;

/// <summary>
/// Demonstrates formatting capabilities for ternary values using the Ternary3.Formatting namespace.
/// Shows how to use TernaryFormatter, TernaryFormat, and TernaryFormatProvider for custom display of ternary numbers.
/// </summary>
public static partial class FormattingDemo
{
    public static void Run()
    {
        Console.WriteLine($"\r\n\r\n{nameof(FormattingDemo)}");

        // EXAMPLE 1: Basic ternary formatting using the "ter" format specifier
        // ----------------------------------------------------------------
        Int27T value1 = 12345;
        Int9T value2 = ter101_T01;
        Int3T value3 = terT10;
        
        Console.WriteLine($"Basic ternary formatting:");
        Console.WriteLine($"  {value1} in ternary: {value1:ter}");
        Console.WriteLine($"  {value2} in ternary: {value2:ter}");
        Console.WriteLine($"  {value3} in ternary: {value3:ter}");

        // EXAMPLE 2: Using TernaryFormatProvider with string.Format
        // --------------------------------------------------------
        var provider = new TernaryFormatProvider();
        string formatted = string.Format(provider, "Value: {0:ter}", value1);
        Console.WriteLine($"\nUsing TernaryFormatProvider: {formatted}");

        // EXAMPLE 3: Custom TernaryFormat with different digit symbols
        // ----------------------------------------------------------
        var customFormat = new TernaryFormat()
        {
            NegativeTritDigit = '-',
            ZeroTritDigit = '0',
            PositiveTritDigit = '+'
        };
        
        Console.WriteLine($"\nCustom digit symbols:");
        Console.WriteLine($"  Standard: {value1:ter}");
        Console.WriteLine($"  Custom:   {((TritArray27)value1).ToString(customFormat)}");

        // EXAMPLE 4: Grouping trits for better readability
        // -----------------------------------------------
        var groupedFormat = new TernaryFormat();
        groupedFormat.Groups.Clear();
        groupedFormat.Groups.Add(new TritGroupDefinition("_", 3)); // Group every 3 trits with underscore
        groupedFormat.Groups.Add(new TritGroupDefinition(" ", 9)); // Group every 9 trits with space
        
        TritArray27 largeValue = 123456789;
        Console.WriteLine($"\nGrouped formatting:");
        Console.WriteLine($"  No grouping: {largeValue:ter}");
        Console.WriteLine($"  Grouped:     {largeValue.ToString(groupedFormat)}");

        // EXAMPLE 5: Using built-in formats
        // --------------------------------
        Console.WriteLine($"\nBuilt-in formats:");
        Console.WriteLine($"  Invariant: {((TritArray27)value1).ToString(TernaryFormat.Invariant)}");
        Console.WriteLine($"  Minimal:   {((TritArray27)value1).ToString(TernaryFormat.Minimal)}");

        // EXAMPLE 6: TernaryFormatter with mixed types
        // ------------------------------------------
        var formatter = new TernaryFormatter();
        var formatProvider = new TernaryFormatProvider();
        
        Console.WriteLine($"\nMixed type formatting:");
        var mixedValues = new object[] { value1, value2, value3, 42, -17 };
        foreach (var val in mixedValues)
        {
            string standardFormat = string.Format(formatProvider, "{0}", val);
            string ternaryFormat = string.Format(formatProvider, "{0:ter}", val);
            Console.WriteLine($"  {val.GetType().Name}: {standardFormat} -> {ternaryFormat}");
        }

        // EXAMPLE 7: Custom format with padding and decimal separator
        // ---------------------------------------------------------
        var paddedFormat = new TernaryFormat()
        {
            TernaryPadding = TernaryPadding.Full,
            DecimalSeparator = "."
        };
        
        Int9T smallValue = ter101;
        Console.WriteLine($"\nPadded formatting:");
        Console.WriteLine($"  Unpadded: {smallValue:ter}");
        Console.WriteLine($"  Padded:   {((TritArray9)smallValue).ToString(paddedFormat)}");

        // EXAMPLE 8: Hierarchical grouping
        // -------------------------------
        var hierarchicalFormat = new TernaryFormat();
        hierarchicalFormat.Groups.Clear();
        hierarchicalFormat.Groups.Add(new TritGroupDefinition(":", 3));  // Every 3 trits
        hierarchicalFormat.Groups.Add(new TritGroupDefinition("-", 9));  // Every 9 trits
        hierarchicalFormat.Groups.Add(new TritGroupDefinition(" | ", 18)); // Every 18 trits
        
        Console.WriteLine($"\nHierarchical grouping:");
        Console.WriteLine($"  {largeValue.ToString(hierarchicalFormat)}");

        // EXAMPLE 9: Using TernaryFormatter directly
        // ----------------------------------------
        var directFormatter = new TernaryFormatter(TernaryFormat.Minimal);
        
        Console.WriteLine($"\nDirect formatter usage:");
        Console.WriteLine($"  Formatted int: {directFormatter.Format("ter", 100, null)}");
        Console.WriteLine($"  Formatted TritArray: {directFormatter.Format("ter", (TritArray9)256, null)}");
        Console.WriteLine($"  Standard format: {directFormatter.Format("D", 100, null)}");

        // EXAMPLE 10: Error handling
        // -------------------------
        try
        {
            // This will throw FormatException because string cannot be formatted as ternary
            string result = directFormatter.Format("ter", "not a number", null);
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"\nError handling: {ex.Message}");
        }
    }
}
