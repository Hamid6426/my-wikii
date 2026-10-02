# 27 - Static and Partial Classes

## Static Class

A class with only static members. You cannot create an instance or inherit from it.

```csharp
static class MathHelper
{
    public const double Pi = 3.14159;

    public static double Square(double x) => x * x;
}

Console.WriteLine(MathHelper.Square(4));   // 16
```

Use it for stateless helpers. `Math`, `Console`, and `File` are static classes.

---

## Static Members in a Normal Class

```csharp
class Counter
{
    public static int Total;      // one copy shared by all instances
    public int Id;                // one copy per instance

    public Counter() => Total++;
}
```

---

## Static Constructor

Runs once, before the class is first used. It takes no parameters and no access modifier.

```csharp
class Config
{
    public static readonly Dictionary<string, string> Defaults;

    static Config()
    {
        Defaults = new() { ["env"] = "dev" };
    }
}
```

---

## Partial Class

Split one class across several files. The compiler joins them into one.

```csharp
// User.cs
public partial class User
{
    public string Name { get; set; } = "";
}

// User.Validation.cs
public partial class User
{
    public bool IsValid() => Name.Length > 0;
}
```

All parts need the `partial` keyword and the same name, namespace, and accessibility.

---

## Why Partial Exists

- Code generators write one file and you write the other, so regenerating never wipes your work
- Large classes can be split by concern
- Windows Forms, source generators, and EF Core scaffolding all rely on it

---

## Partial Methods

Declare in one part, implement in another.

```csharp
public partial class Order
{
    partial void OnCreated();            // declaration

    public Order() => OnCreated();
}

public partial class Order
{
    partial void OnCreated() => Console.WriteLine("created");
}
```

If no part implements it, the call is removed at compile time.

---

## Gotchas

- Static state lives for the whole program and is shared across threads, so protect it with care
- Static classes cannot implement interfaces and are hard to replace in tests; prefer dependency injection for anything that needs testing
- Overusing static helpers turns code into hidden global state
- Partial classes are not a fix for a class that is too big; if you need many parts, the class does too much

---

## Examples

- [27-01](examples/27-01_static_and_partial.cs): Static classes, static members, and partial classes

Run one with `dotnet run examples/27-01_static_and_partial.cs`. See [Examples](examples/README.md).
