// Lesson 47: Regular Expressions (../47_regular_expressions.md)
// Match, extract, replace with regular expressions
// Run: dotnet run 47-01_regex_examples.cs

using System.Text.RegularExpressions;

string log = "2026-10-01 ERROR disk full; 2026-10-02 WARN low memory; 2026-10-03 ERROR timeout";

foreach (Match m in Regex.Matches(log, @"(?<date>\d{4}-\d{2}-\d{2}) ERROR (?<msg>[^;]+)"))
{
    Console.WriteLine($"{m.Groups["date"].Value}: {m.Groups["msg"].Value}");
}

Console.WriteLine(Regex.IsMatch("alice@example.com", @"^[^@\s]+@[^@\s]+\.[^@\s]+$"));
Console.WriteLine(Regex.IsMatch("not an email", @"^[^@\s]+@[^@\s]+\.[^@\s]+$"));

Console.WriteLine(Regex.Replace("call 555-123-4567 now", @"\d{3}-\d{3}-(\d{4})", "XXX-XXX-$1"));
Console.WriteLine(Regex.Replace("too    many   spaces", @"\s+", " "));
Console.WriteLine(string.Join("|", Regex.Split("a1b22c333d", @"\d+")));

Console.WriteLine(Digits().Matches("room 12, floor 3, door 456").Count);
Console.WriteLine(Regex.Replace("camelCaseToSnakeCase", "(?<!^)([A-Z])", "_$1").ToLower());

partial class Program
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex Digits();
}

// Expected output should be:
// 2026-10-01: disk full
// 2026-10-03: timeout
// True
// False
// call XXX-XXX-4567 now
// too many spaces
// a|b|c|d
// 3
// camel_case_to_snake_case
