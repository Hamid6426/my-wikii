# 40 - Modern C# Features

Newer syntax that makes code shorter. Each feature lists the C# version that added it. The version table is in [C# Version History](61_csharp_version_history.md).

## Local Functions (C# 7)

A function declared inside another method. Only that method can call it.

```csharp
int SumOfSquares(int[] nums)
{
    int total = 0;
    foreach (int n in nums) total += Square(n);
    return total;

    int Square(int x) => x * x;   // local function
}
```

Add `static` to stop it from capturing outer variables (C# 8):

```csharp
static int Square(int x) => x * x;
```

---

## Target-Typed new (C# 9)

Leave out the type on the right when the left side already names it.

```csharp
List<string> names = new();
Dictionary<string, int> ages = new() { ["Alice"] = 30 };
Person p = new("Alice", 30);
```

---

## Primary Constructors (C# 12)

Put constructor parameters right after the class name. They are available in the whole class body.

```csharp
class Person(string name, int age)
{
    public string Name { get; } = name;
    public string Describe() => $"{Name} is {age}";   // age is used directly
}

var p = new Person("Alice", 30);
```

For classes and structs, the parameters are not properties. You make properties yourself, as `Name` above. Records are different: their primary constructor parameters become properties automatically.

```csharp
record Point(int X, int Y);   // X and Y are properties
```

Common use: dependency injection.

```csharp
class OrderService(IOrderRepository repo, ILogger<OrderService> logger)
{
    public Order? Find(int id) => repo.GetById(id);
}
```

---

## Collection Expressions (C# 12)

One square-bracket syntax for arrays, lists, and spans.

```csharp
int[] array = [1, 2, 3];
List<int> list = [1, 2, 3];
Span<int> span = [1, 2, 3];
int[] empty = [];
```

Spread with `..` to join collections:

```csharp
int[] a = [1, 2];
int[] b = [3, 4];

int[] all = [..a, ..b, 5];       // 1, 2, 3, 4, 5
```

---

## Alias Any Type (C# 12)

`using` aliases can name tuples, arrays, and generic types.

```csharp
using Point = (int X, int Y);
using Scores = System.Collections.Generic.Dictionary<string, int[]>;

Point p = (3, 4);
Console.WriteLine(p.X);
```

---

## Default Lambda Parameters (C# 12)

```csharp
var greet = (string name, string greeting = "Hello") => $"{greeting}, {name}";

Console.WriteLine(greet("Alice"));           // Hello, Alice
Console.WriteLine(greet("Alice", "Hi"));     // Hi, Alice
```

---

## params Collections (C# 13)

`params` works with more types than arrays, such as `List<T>` and `ReadOnlySpan<T>`. A span avoids creating an array.

```csharp
static int Sum(params ReadOnlySpan<int> numbers)
{
    int total = 0;
    foreach (int n in numbers) total += n;
    return total;
}

Console.WriteLine(Sum(1, 2, 3));   // 6
```

---

## The field Keyword (C# 14)

Write a property with custom logic without declaring a backing field yourself. `field` is the hidden one.

```csharp
class User
{
    public string Name
    {
        get;
        set => field = value?.Trim() ?? "";
    } = "";
}
```

Before C# 14 this needed a `private string _name;` and a full getter.

If your class already has a member named `field`, write `@field` or `this.field` to mean your own member.

---

## Null-Conditional Assignment (C# 14)

Assign only if the left side is not null.

```csharp
user?.Name = "Alice";       // skipped when user is null
list?[0] = 10;
```

---

## readonly struct (C# 7.2)

A struct that can never change after creation. The compiler can skip defensive copies.

```csharp
readonly struct Money(decimal amount, string currency)
{
    public decimal Amount { get; } = amount;
    public string Currency { get; } = currency;

    public Money Add(decimal x) => new(Amount + x, Currency);
}
```

Use it for small value types such as points, money, and ranges.

---

## Features Covered Elsewhere

| Feature                              | Lesson                                                                       |
| ------------------------------------ | ---------------------------------------------------------------------------- |
| Top-level statements                 | [Basics of a Program](04_basics_of_a_program.md)                             |
| File-scoped namespaces, global using | [Namespaces and Packages](06_namespaces_and_packages.md)                     |
| Records, `with`                      | [Records and Equality](25_records_and_equality.md)                           |
| Pattern matching                     | [Pattern Matching](14_pattern_matching.md)                                   |
| `required` members, nullable types   | [Nullable Reference Types](24_nullable_reference_types.md)                   |
| Extension members                    | [Extension Methods](39_extension_methods.md)                                 |
| Raw string literals                  | [Strings](21_strings.md)                                                     |
| Indices and ranges                   | [Indexers and Operator Overloading](28_indexers_and_operator_overloading.md) |

---

## Gotchas

- Primary constructor parameters in classes are captured by the compiler and can be changed by any method; copy them into `readonly` fields or properties when they must not change
- A collection expression needs a target type: write `int[] x = [1, 2];`, not `var x = [1, 2];`
- The `field` keyword and null-conditional assignment need C# 14 (.NET 10 SDK); older SDKs report a syntax error
- Check the language version with `dotnet --version`, or set `<LangVersion>` in the `.csproj`

---

## Examples

- [40-01](examples/40-01_modern_features.cs): Primary constructors, collection expressions, field keyword

Run one with `dotnet run examples/40-01_modern_features.cs`. See [Examples](examples/README.md).
