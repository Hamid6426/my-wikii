// Lesson 21: Strings (../21_strings.md)
// IFormattable, custom format strings, and Convert.ChangeType
// Run: dotnet run 21-02_custom_formatting.cs

using System.Globalization;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;   // same output on every computer

var price = new Money(1234.5m, "USD");

Console.WriteLine(price);                         // ToString()
Console.WriteLine($"{price:G}");                  // general
Console.WriteLine($"{price:short}");              // custom format
Console.WriteLine($"{price:long}");
Console.WriteLine(string.Format("Total: {0:short}", price));
Console.WriteLine(price.ToString("long", new CultureInfo("de-DE")));

Console.WriteLine(Convert.ChangeType("42", typeof(int)));
Console.WriteLine(Convert.ChangeType(3.99, typeof(int)));
Console.WriteLine(((DateTime)Convert.ChangeType("2026-10-01", typeof(DateTime), CultureInfo.InvariantCulture)).ToString("yyyy-MM-dd"));
Console.WriteLine(((IConvertible)7).ToDouble(CultureInfo.InvariantCulture));

readonly record struct Money(decimal Amount, string Currency) : IFormattable
{
    public string ToString(string? format, IFormatProvider? provider)
    {
        provider ??= CultureInfo.CurrentCulture;
        return format switch
        {
            null or "" or "G" => $"{Amount.ToString("N2", provider)} {Currency}",
            "short" => Amount.ToString("N0", provider),
            "long" => $"{Amount.ToString("N2", provider)} {Currency} ({(Amount >= 1000 ? "large" : "small")})",
            _ => throw new FormatException($"Unknown format '{format}'")
        };
    }

    public override string ToString() => ToString("G", null);
}

// Expected output should be:
// 1,234.50 USD
// 1,234.50 USD
// 1,235
// 1,234.50 USD (large)
// Total: 1,235
// 1.234,50 USD (large)
// 42
// 4
// 2026-10-01
// 7
