# 05 - Console Input and Output

## Writing Text

```csharp
Console.WriteLine("Hello");   // text, then a new line
Console.Write("Hello ");      // text, no new line
Console.Write("World");
Console.WriteLine();          // just a new line
```

Expected output should be:

```
Hello
Hello World
```

---

## String Interpolation

Put `$` before the string and write values inside `{}`.

```csharp
string name = "Alice";
int age = 30;

Console.WriteLine($"{name} is {age} years old");
Console.WriteLine($"Next year: {age + 1}");
```

Expected output should be:

```
Alice is 30 years old
Next year: 31
```

Any expression works inside the braces.

---

## Formatting Values

Add a format after a colon: `{value:format}`.

```csharp
double price = 1234.5;
double ratio = 0.256;
int id = 7;

Console.WriteLine($"{price:C}");      // currency, depends on culture
Console.WriteLine($"{price:N2}");     // 1,234.50
Console.WriteLine($"{ratio:P1}");     // 25.6%
Console.WriteLine($"{id:D4}");        // 0007
Console.WriteLine($"{255:X}");        // FF (hex)
Console.WriteLine($"{price:F1}");     // 1234.5
```

| Format | Meaning             | Example input | Result      |
| ------ | ------------------- | ------------- | ----------- |
| `C`    | Currency            | `1234.5`      | `$1,234.50` |
| `N2`   | Number, 2 decimals  | `1234.5`      | `1,234.50`  |
| `F2`   | Fixed, 2 decimals   | `3.14159`     | `3.14`      |
| `P1`   | Percent, 1 decimal  | `0.256`       | `25.6%`     |
| `D4`   | Integer padded to 4 | `7`           | `0007`      |
| `X`    | Hexadecimal         | `255`         | `FF`        |
| `E2`   | Scientific          | `12345`       | `1.23E+004` |

The exact look of `C`, `N`, and `P` depends on the computer's language settings (the culture).

---

## Alignment

`{value,width}` pads to a width. Positive is right-aligned, negative is left-aligned.

```csharp
Console.WriteLine($"{"Name",-10}{"Age",5}");
Console.WriteLine($"{"Alice",-10}{30,5}");
Console.WriteLine($"{"Bob",-10}{4,5}");
```

Expected output should be:

```
Name        Age
Alice        30
Bob           4
```

---

## Older Formatting Styles

You will see these in older code.

```csharp
Console.WriteLine("Hello {0}, you are {1}", name, age);   // composite format
Console.WriteLine("Hello " + name);                       // concatenation
```

Prefer interpolation. It is easier to read.

---

## Reading a Line

```csharp
Console.Write("Your name: ");
string? name = Console.ReadLine();

Console.WriteLine($"Hello, {name}");
```

`ReadLine` returns `null` when input ends (for example Ctrl+D on Linux and macOS, Ctrl+Z on Windows), which is why the type is `string?`.

---

## Reading Numbers

`ReadLine` always gives text. Convert it, and never trust that the user typed a number.

```csharp
Console.Write("Age: ");
string? text = Console.ReadLine();

if (int.TryParse(text, out int age))
{
    Console.WriteLine($"In 10 years: {age + 10}");
}
else
{
    Console.WriteLine("That is not a number");
}
```

See [Type Conversion](10_type_conversion.md) for `Parse` and `TryParse`.

---

## Reading Until Valid

```csharp
int age;
string? line;

Console.Write("Age: ");
while ((line = Console.ReadLine()) != null)
{
    if (int.TryParse(line, out age) && age >= 0)
    {
        Console.WriteLine($"Age set to {age}");
        break;
    }

    Console.Write("Enter a whole number, 0 or more: ");
}
```

The `!= null` check ends the loop when input ends. Without it, a loop that retries on bad input never finishes when the input is closed.

---

## Reading a Single Key

```csharp
Console.WriteLine("Press any key to continue...");
ConsoleKeyInfo key = Console.ReadKey(intercept: true);   // true hides the key

if (key.Key == ConsoleKey.Y)
{
    Console.WriteLine("Yes");
}
```

---

## Colors and Screen Control

```csharp
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Success");
Console.ResetColor();

Console.BackgroundColor = ConsoleColor.Red;
Console.WriteLine("Error");
Console.ResetColor();

Console.Title = "My App";
Console.Clear();
```

Always call `ResetColor()` afterward, or the colors stay for the rest of the session and for your shell.

---

## Errors and Redirection

Programs have two output streams. Errors go to the second one, so users can separate them.

```csharp
Console.WriteLine("Normal result");        // standard output
Console.Error.WriteLine("Something broke"); // standard error
```

```bash
dotnet run > result.txt          # only normal output goes to the file
dotnet run 2> errors.txt         # only errors go to the file
```

---

## Unicode and Emoji

If symbols show as `?`, switch the output to UTF-8:

```csharp
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Café ✓");
```

The terminal font must also contain the characters.

---

## Command-Line Arguments

Arguments typed after the program name arrive in `args`.

```csharp
// Program.cs (top-level statements: `args` is available automatically)
if (args.Length == 0)
{
    Console.WriteLine("Usage: MyApp <name>");
    return;
}

Console.WriteLine($"Hello, {args[0]}");
```

```bash
dotnet run -- Alice
```

Expected output should be:

```
Hello, Alice
```

With an explicit `Main`, the parameter is declared: `static void Main(string[] args)`.

---

## Exit Codes

A program reports success or failure to the shell with a number. `0` means success.

```csharp
if (args.Length == 0)
{
    Console.Error.WriteLine("Missing argument");
    return 1;               // top-level statements can return an int
}

return 0;
```

```bash
dotnet run
echo $?            # prints the exit code (PowerShell: $LASTEXITCODE)
```

Elsewhere in the code, `Environment.Exit(2)` ends the program immediately with that code.

---

## Gotchas

- `Console.ReadLine()` can return `null`; handle it or use `TryParse`, which treats `null` as a failure
- A retry loop around `ReadLine` runs forever if the input ends, because `ReadLine` keeps returning `null`; stop the loop when it does
- `int.Parse` on bad text throws `FormatException`; use `TryParse` for user input
- `Console.ReadKey()` fails when input is redirected from a file or pipe
- `Console.Clear()` and colors may do nothing in some terminals and in output that is redirected
- `{}` inside an interpolated string needs doubling to print a brace: `$"{{literal}}"`
- Currency and decimal symbols change with the computer's culture; pass `CultureInfo.InvariantCulture` for fixed output

---

## Examples

- [05-01](examples/05-01_greeting.cs): Read a name and greet
- [05-02](examples/05-02_format_table.cs): Format numbers and align columns
- [05-03](examples/05-03_args_and_exit_code.cs): Command-line arguments and an exit code
- [05-04](examples/05-04_validated_input.cs): Ask until the input is valid

Run one with `dotnet run examples/05-01_greeting.cs`. See [Examples](examples/README.md).
