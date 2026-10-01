# 21 - Strings

## String Basics

`string` is an alias for `System.String`. Strings are **immutable**: every operation returns a new string.

```csharp
string name = "Alice";
string empty = "";
string nullStr = null;
```

---

## String Interpolation (C# 6+)

```csharp
string name = "Alice";
int age = 30;
string msg = $"Name: {name}, Age: {age}";
// "Name: Alice, Age: 30"

string formatted = $"Pi ≈ {Math.PI:F2}";  // "Pi ≈ 3.14"
```

---

## Verbatim Strings

`@` prefix: backslashes are literal, newlines preserved.

```csharp
string path = @"C:\Users\Alice\Documents";
string multiline = @"Line 1
Line 2
Line 3";
```

---

## Raw String Literals (C# 11+)

No escape sequences needed; delimited by `"""`.

```csharp
string json = """
{
    "name": "Alice",
    "age": 30
}
""";
```

---

## Common String Methods

```csharp
string s = "  Hello, World!  ";

s.Length                // 17
s.Trim()               // "Hello, World!"
s.TrimStart()          // "Hello, World!  "
s.TrimEnd()            // "  Hello, World!"
s.ToUpper()            // "  HELLO, WORLD!  "
s.ToLower()            // "  hello, world!  "
s.Contains("World")    // true
s.StartsWith("  He")   // true
s.EndsWith("!  ")      // true
s.Replace("World", "C#")  // "  Hello, C#!  "
s.Substring(2, 5)      // "Hello"
s.IndexOf("World")     // 9
s.Split(", ")          // ["  Hello", "World!  "]
```

---

## Checking Null or Empty

```csharp
string s = "";

string.IsNullOrEmpty(s)        // true if null or ""
string.IsNullOrWhiteSpace(s)   // true if null, "", or whitespace only
```

---

## String Concatenation

```csharp
string a = "Hello" + ", " + "World!";   // + operator
string b = string.Concat("Hello", ", ", "World!");
```

For many concatenations in a loop, use `StringBuilder`: `+` creates a new string each time.

---

## StringBuilder

Mutable string builder: efficient for repeated modifications.

```csharp
using System.Text;

var sb = new StringBuilder();
sb.Append("Hello");
sb.Append(", ");
sb.AppendLine("World!");
sb.Insert(0, ">> ");
sb.Replace("World", "C#");

string result = sb.ToString();
```

---

## String Comparison

```csharp
string a = "hello";
string b = "Hello";

bool eq1 = a == b;                                           // false
bool eq2 = a.Equals(b, StringComparison.OrdinalIgnoreCase); // true

int cmp = string.Compare(a, b, ignoreCase: true);  // 0 = equal
```

---

## String Formatting

```csharp
string s = string.Format("Name: {0}, Age: {1}", "Alice", 30);

// Format specifiers
double pi = 3.14159;
string f1 = pi.ToString("F2");   // "3.14": 2 decimal places
string f2 = pi.ToString("E2");   // "3.14E+000"
int n = 42;
string f3 = n.ToString("D5");    // "00042": padded
string f4 = n.ToString("X");     // "2A": hex
```

---

## Char Operations

```csharp
string s = "Hello";
char c = s[0];                // 'H'

char.IsLetter('A')            // true
char.IsDigit('5')             // true
char.IsWhiteSpace(' ')        // true
char.ToUpper('a')             // 'A'
char.ToLower('Z')             // 'z'
```

---

## Custom Formatting with IFormattable

Types such as `int` and `DateTime` accept format codes: `{price:N2}`, `{date:yyyy-MM-dd}`. Your own type can accept codes too by implementing `IFormattable`.

```csharp
using System.Globalization;

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

var price = new Money(1234.5m, "USD");
Console.WriteLine($"{price}");        // 1,234.50 USD
Console.WriteLine($"{price:short}");  // 1,235
Console.WriteLine($"{price:long}");   // 1,234.50 USD (large)
```

`string.Format`, interpolated strings, and `ToString(format)` all call your `ToString(string?, IFormatProvider?)`. The second argument carries the culture, so pass it on to the numbers inside. `ToString("long", new CultureInfo("de-DE"))` prints `1.234,50 USD (large)`.

---

## IConvertible and Convert.ChangeType

`IConvertible` is the interface the basic types implement so they can turn themselves into other basic types. You rarely implement it. You meet it through `Convert.ChangeType`, which converts a value to a type chosen at run time.

```csharp
Console.WriteLine(Convert.ChangeType("42", typeof(int)));     // 42
Console.WriteLine(Convert.ChangeType(3.99, typeof(int)));     // 4 (rounds, does not cut)
var date = (DateTime)Convert.ChangeType("2026-10-01", typeof(DateTime), CultureInfo.InvariantCulture);
```

It throws `InvalidCastException` or `FormatException` when the conversion is impossible. Use `int.TryParse` when the target type is known at compile time. See [Type Conversion](10_type_conversion.md).

---

## Gotchas

- Strings are immutable: `s.Replace(...)` does not change `s`, it returns a new string
- `string == null` is valid but `s.Length` on null throws `NullReferenceException`
- Use `StringBuilder` for building strings in loops (O(1) amortized append vs O(n²))
- `==` compares string content, not reference (unlike most reference types)
- `string.Empty` is preferred over `""` to express intent clearly

---

## Examples

- [21-01](examples/21-01_string_tools.cs): Common string operations
- [21-02](examples/21-02_custom_formatting.cs): IFormattable, custom format strings, and Convert.ChangeType

Run one with `dotnet run examples/21-01_string_tools.cs`. See [Examples](examples/README.md).
