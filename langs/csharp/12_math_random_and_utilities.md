# 12 - Math, Random and Utility Types

Small built-in types you reach for all the time.

## Math

`Math` is a static class. No `new` needed.

```csharp
Math.Abs(-5);          // 5
Math.Max(3, 9);        // 9
Math.Min(3, 9);        // 3
Math.Pow(2, 10);       // 1024
Math.Sqrt(16);         // 4
Math.Floor(3.7);       // 3     rounds down
Math.Ceiling(3.2);     // 4     rounds up
Math.Truncate(-3.7);   // -3    drops the decimals
Math.Round(3.456, 2);  // 3.46
Math.Clamp(15, 0, 10); // 10    keeps a value in a range
Math.Sign(-8);         // -1
Console.WriteLine(Math.PI);
```

All of these return a `double` unless the inputs are integers (`Abs`, `Max`, `Min`, `Clamp`, `Sign`).

---

## Rounding Rules

`Math.Round` uses banker's rounding by default: halves go to the nearest even number.

```csharp
Math.Round(2.5);   // 2
Math.Round(3.5);   // 4

Math.Round(2.5, MidpointRounding.AwayFromZero);   // 3, what most people expect
```

For money, use `decimal` with `MidpointRounding.AwayFromZero`.

---

## Integer Division and Remainder

```csharp
int quotient = 17 / 5;    // 3
int remainder = 17 % 5;   // 2

var (q, r) = Math.DivRem(17, 5);   // both at once
```

---

## Trigonometry and Logs

```csharp
Math.Sin(Math.PI / 2);   // 1, angles are in radians
Math.Log(Math.E);        // 1, natural log
Math.Log10(1000);        // 3
```

Convert degrees to radians with `degrees * Math.PI / 180`.

---

## Random

```csharp
var rng = new Random();

rng.Next();            // 0 up to int.MaxValue - 1
rng.Next(10);          // 0 to 9
rng.Next(1, 7);        // 1 to 6 (the upper number is excluded)
rng.NextDouble();      // 0.0 up to, but not including, 1.0
```

Use the shared instance when you do not need a seed:

```csharp
int roll = Random.Shared.Next(1, 7);
```

---

## Seeds

A seed makes the sequence repeat, which is useful in tests.

```csharp
var a = new Random(42);
var b = new Random(42);

Console.WriteLine(a.Next(100) == b.Next(100));   // True
```

---

## Shuffle and Pick (.NET 8+)

```csharp
int[] cards = { 1, 2, 3, 4, 5 };
Random.Shared.Shuffle(cards);                      // shuffles in place

string[] names = { "Ann", "Bob", "Cy" };
string[] picked = Random.Shared.GetItems(names, 2);   // 2 random picks, repeats possible
```

---

## Random for Security

`Random` is predictable. For passwords, tokens, and keys use the cryptographic generator.

```csharp
using System.Security.Cryptography;

int secure = RandomNumberGenerator.GetInt32(1, 7);
byte[] token = RandomNumberGenerator.GetBytes(32);
string text = Convert.ToHexString(token);
```

---

## Guid

A 128-bit value that is practically unique. Used for IDs.

```csharp
Guid id = Guid.NewGuid();
Console.WriteLine(id);                       // for example 3f2504e0-4f89-11d3-9a0c-0305e82c3301

Guid parsed = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301");
bool ok = Guid.TryParse("not a guid", out _);   // false

Guid empty = Guid.Empty;                     // all zeros
string compact = id.ToString("N");           // no dashes
```

Random GUIDs make poor database keys because they scatter. .NET 9 adds `Guid.CreateVersion7()`, which is time-ordered and sorts better.

---

## Environment

Information about the machine and the running program.

```csharp
Console.WriteLine(Environment.MachineName);
Console.WriteLine(Environment.OSVersion);
Console.WriteLine(Environment.ProcessorCount);
Console.WriteLine(Environment.CurrentDirectory);
Console.WriteLine(Environment.NewLine == "\n" ? "Unix-style" : "Windows-style");

string? home = Environment.GetEnvironmentVariable("HOME");
string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
```

Use `System.OperatingSystem` to branch on the platform:

```csharp
if (OperatingSystem.IsWindows()) { }
if (OperatingSystem.IsLinux()) { }
```

---

## Starting Another Program

```csharp
using System.Diagnostics;

Process.Start("notepad.exe");                           // Windows

Process.Start(new ProcessStartInfo("https://example.com")
{
    UseShellExecute = true                              // opens the default browser
});
```

Run a command and read what it prints:

```csharp
var info = new ProcessStartInfo("dotnet", "--version")
{
    RedirectStandardOutput = true
};

using var process = Process.Start(info)!;
string output = process.StandardOutput.ReadToEnd();
process.WaitForExit();
```

---

## BigInteger

Whole numbers with no size limit.

```csharp
using System.Numerics;

BigInteger big = BigInteger.Pow(2, 200);
Console.WriteLine(big);

BigInteger factorial = 1;
for (int i = 2; i <= 30; i++) factorial *= i;
```

`long` overflows past about 9.2 quintillion; `BigInteger` does not, but it is slower.

---

## Other Small Types

| Type                      | Use for                                                                                          |
| ------------------------- | ------------------------------------------------------------------------------------------------ |
| `Version`                 | Compare version numbers like `1.2.3`                                                             |
| `Uri`                     | Parse and build web addresses                                                                    |
| `System.Numerics.Complex` | Complex numbers                                                                                  |
| `Stopwatch`               | Timing code (see [Dates and Times](22_dates_and_times.md))                                       |
| `Lazy<T>`                 | Delay expensive setup (see [Memory and Garbage Collection](33_memory_and_garbage_collection.md)) |

```csharp
var uri = new Uri("https://example.com:8080/docs?page=2");
Console.WriteLine(uri.Host);    // example.com
Console.WriteLine(uri.Port);    // 8080
Console.WriteLine(uri.Query);   // ?page=2
```

---

## Gotchas

- `Random.Next(1, 6)` never returns 6; the upper bound is excluded
- Creating `new Random()` in a tight loop on old .NET Framework gave the same numbers; use `Random.Shared`
- Do not use `Random` for passwords or tokens
- Floating-point math is approximate: `0.1 + 0.2 == 0.3` is `false`. Use `decimal` for money
- `Math.Round(2.5)` is `2`, not `3`
- `Math.Sqrt(-1)` returns `NaN`, not an exception
- Trig functions take radians, not degrees

---

## Examples

- [12-01](examples/12-01_math_tour.cs): A tour of the Math class
- [12-02](examples/12-02_dice_roller.cs): Seeded random numbers
- [12-03](examples/12-03_guid_and_environment.cs): Guid, Environment, and BigInteger

Run one with `dotnet run examples/12-01_math_tour.cs`. See [Examples](examples/README.md).
