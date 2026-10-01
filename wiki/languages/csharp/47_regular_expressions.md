# 47 - Regular Expressions

## What is a Regex

A pattern for finding, checking, or replacing text. C# has it built in.

```csharp
using System.Text.RegularExpressions;
```

---

## Check for a Match

```csharp
bool ok = Regex.IsMatch("abc123", @"\d+");   // true
```

Use a verbatim string (`@"..."`) or a raw string so backslashes do not need doubling.

---

## Find a Match

```csharp
Match m = Regex.Match("Order #4521 shipped", @"#(\d+)");

if (m.Success)
{
    Console.WriteLine(m.Value);              // #4521
    Console.WriteLine(m.Groups[1].Value);    // 4521
}
```

---

## Find All Matches

```csharp
foreach (Match m in Regex.Matches("a1 b22 c333", @"\d+"))
{
    Console.WriteLine(m.Value);   // 1, 22, 333
}
```

---

## Replace and Split

```csharp
string clean = Regex.Replace("a   b    c", @"\s+", " ");     // a b c
string[] parts = Regex.Split("a1b22c", @"\d+");              // a, b, c

string masked = Regex.Replace("4111111111111111", @"\d(?=\d{4})", "*");
```

---

## Named Groups

```csharp
var m = Regex.Match("2026-10-01", @"(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})");

Console.WriteLine(m.Groups["year"].Value);    // 2026
Console.WriteLine(m.Groups["month"].Value);   // 10
```

---

## Common Pieces

| Pattern  | Meaning                      |
| -------- | ---------------------------- |
| `.`      | Any character except newline |
| `\d`     | A digit                      |
| `\w`     | Letter, digit, or underscore |
| `\s`     | Whitespace                   |
| `^` `$`  | Start and end of the text    |
| `*`      | Zero or more                 |
| `+`      | One or more                  |
| `?`      | Zero or one                  |
| `{3,5}`  | Between 3 and 5              |
| `[abc]`  | One of a, b, or c            |
| `(a\|b)` | Group with a choice          |

---

## Options

```csharp
Regex.IsMatch("HELLO", "hello", RegexOptions.IgnoreCase);              // true
Regex.IsMatch("a\nb", "^b$", RegexOptions.Multiline);                  // true
```

---

## Reuse and Speed

Build the regex once if you use it many times.

```csharp
private static readonly Regex Email = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

bool valid = Email.IsMatch(input);
```

Source-generated regex (.NET 7+) is faster still and checked at build time:

```csharp
public static partial class Patterns
{
    [GeneratedRegex(@"^\d{4}-\d{2}-\d{2}$")]
    public static partial Regex IsoDate();
}

bool ok = Patterns.IsoDate().IsMatch("2026-10-01");
```

---

## Gotchas

- Do not parse HTML or full email rules with regex; use a real parser
- Patterns with nested repeats like `(a+)+` can run for a very long time on bad input; pass a timeout when matching untrusted text: `new Regex(pattern, RegexOptions.None, TimeSpan.FromSeconds(1))`
- `.` does not match a newline unless you set `RegexOptions.Singleline`
- `Regex.Match` returns a `Match` even on failure; check `Success` before using `Value`
- Simple checks like `StartsWith` or `Contains` are clearer and faster than a regex

---

## Examples

- [47-01](examples/47-01_regex_examples.cs): Match, extract, replace with regular expressions

Run one with `dotnet run examples/47-01_regex_examples.cs`. See [Examples](examples/README.md).
