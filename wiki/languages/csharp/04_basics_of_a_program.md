# 04 - Basics of a Program

## Entry Point

In C#, execution starts at `Main()` inside a class.

```csharp
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}
```

Although, .NET 6+ supports top-level statements - no class or `Main()` required:

```csharp
Console.WriteLine("Hello, World!");
```

---

## Class Wrapper

Every C# file belongs to a class. The class wraps your logic.

```csharp
class MyApp
{
    static void Main()
    {
        // code here
    }
}
```

---

## Namespaces

Namespaces organize code and prevent naming conflicts.

```csharp
namespace MyProject
{
    class Program
    {
        static void Main() { }
    }
}
```

`using` imports a namespace:

```csharp
using System;
using System.Collections.Generic;
```

.NET 6+ implicit usings - common namespaces are auto-imported, no need to declare them manually.

---

## How to Run

```bash
dotnet new console -n HelloWorld
cd HelloWorld
dotnet run
```

---

## Comments

### Single-line

```csharp
// This is a single-line comment
int x = 10; // inline comment
```

### Multi-line

```csharp
/*
  This spans
  multiple lines
*/
```

### XML Doc Comments

Used by IDEs for tooltips and documentation generation.

```csharp
/// <summary>
/// Calculates the sum of two integers.
/// </summary>
/// <param name="a">First number</param>
/// <param name="b">Second number</param>
/// <returns>Sum of a and b</returns>
static int Add(int a, int b) => a + b;
```

---

## Preprocessor Directives

Lines that start with `#`. The compiler reads them before compiling and they control which code is built.

```csharp
#define VERBOSE

#if DEBUG
Console.WriteLine("Debug build");
#elif VERBOSE
Console.WriteLine("Verbose build");
#else
Console.WriteLine("Release build");
#endif
```

`DEBUG` is set automatically for Debug builds. Define your own with `#define` at the top of the file or `<DefineConstants>` in the `.csproj`.

| Directive                        | What it does                                           |
| -------------------------------- | ------------------------------------------------------ |
| `#if` `#elif` `#else` `#endif`   | Include code only when a symbol is defined             |
| `#define` `#undef`               | Define or remove a symbol (top of the file only)       |
| `#region` `#endregion`           | Name a block so editors can collapse it                |
| `#nullable enable`               | Turn null-safety warnings on or off for part of a file |
| `#pragma warning disable CS0168` | Silence one compiler warning                           |
| `#warning` `#error`              | Print your own warning or stop the build               |

```csharp
#region Helpers
static int Square(int x) => x * x;
#endregion

#pragma warning disable CS0219   // variable assigned but never used
int unused = 5;
#pragma warning restore CS0219
```

Prefer `[Conditional("DEBUG")]` on a method over `#if` around its calls. See [Attributes and Reflection](52_attributes_and_reflection.md).

---

## Examples

- [04-01](examples/04-01_program_structure.cs): Program structure with an explicit Main
- [04-02](examples/04-02_preprocessor.cs): Preprocessor directives

Run one with `dotnet run examples/04-01_program_structure.cs`. See [Examples](examples/README.md).
