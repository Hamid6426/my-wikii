# 06 - Namespaces & Packages

## What is a Namespace

Namespaces organize code into logical groups and prevent naming conflicts across files and libraries.

```csharp
namespace MyApp.Models
{
    class User { }
}
```

---

## File-Scoped Namespaces (C# 10+)

Removes one level of indentation - preferred in modern C#.

```csharp
namespace MyApp.Models;

class User { }
```

---

## Nested Namespaces

```csharp
namespace MyApp
{
    namespace Services
    {
        class AuthService { }
    }
}
```

Shorthand:

```csharp
namespace MyApp.Services
{
    class AuthService { }
}
```

---

## using Directive

Imports a namespace so you don't need to fully qualify types.

```csharp
using System;
using System.Collections.Generic;

Console.WriteLine("Hello"); // instead of System.Console.WriteLine
```

---

## global using (.NET 6+)

Applied across the entire project - declare once, available everywhere.

```csharp
global using System;
global using System.Collections.Generic;
```

Typically placed in a `GlobalUsings.cs` file at project root.

---

## using static

Imports static members directly - no class prefix needed.

```csharp
using static System.Math;
using static System.Console;

WriteLine(Sqrt(16)); // instead of Console.WriteLine(Math.Sqrt(16))
```

---

## using Alias

Shortens or disambiguates long or conflicting namespace names.

```csharp
using Project = MyCompany.MyApp.Services;
using Dict = System.Collections.Generic.Dictionary<string, int>;
```

---

## Multi-File Namespace Organization

Files in the same namespace can span multiple files - the compiler merges them.

```
MyApp/
├── Models/
│   └── User.cs          → namespace MyApp.Models
├── Services/
│   └── AuthService.cs   → namespace MyApp.Services
└── Program.cs           → namespace MyApp
```

Convention: namespace should mirror folder structure.

---

## Installing Packages e.g. Pastel

### Using Terminal

1. Open the terminal in VSCode
2. Navigate to the project directory: `cd new_dotnet_project`
3. Install the Pastel package: `dotnet add package Pastel`

### Using Command Palette

1. Open the Command Palette (Ctrl+Shift+P)
2. Type `> .NET: Add Package` and select it
3. Enter the package name (e.g., `Pastel`) and select the desired version
4. The package will be added to your project, and you can start using it in your code files

---

## Examples

- [06-01](examples/06-01_namespaces_and_aliases.cs): Namespaces, using alias, and using static

Run one with `dotnet run examples/06-01_namespaces_and_aliases.cs`. See [Examples](examples/README.md).
