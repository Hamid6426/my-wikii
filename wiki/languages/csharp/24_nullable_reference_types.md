# 24 - Nullable Reference Types

## What They Do

Turn on compiler warnings for possible `null` mistakes. Without it, any reference can be `null` and nothing warns you.

Enable per project in the `.csproj` (on by default in new projects):

```xml
<PropertyGroup>
  <Nullable>enable</Nullable>
</PropertyGroup>
```

Or per file:

```csharp
#nullable enable
```

---

## Reading the Types

```csharp
string name = "Alice";     // never null
string? nick = null;       // may be null
```

With the feature on, `string` means "not null" and `string?` means "might be null".

---

## The Compiler Tracks It

```csharp
string? input = Console.ReadLine();

Console.WriteLine(input.Length);       // warning: input may be null

if (input != null)
{
    Console.WriteLine(input.Length);   // fine, compiler knows it is not null
}
```

---

## Null Operators

| Operator | Name                   | Example                  |
| -------- | ---------------------- | ------------------------ |
| `?.`     | Null-conditional       | `person?.Name`           |
| `??`     | Null-coalescing        | `nick ?? "none"`         |
| `??=`    | Null-coalescing assign | `cache ??= LoadCache();` |
| `!`      | Null-forgiving         | `input!.Length`          |

```csharp
int? len = person?.Name?.Length;        // null if person or Name is null
string display = nick ?? "anonymous";
```

---

## The Null-Forgiving Operator

`!` tells the compiler "trust me, this is not null". It removes the warning but adds no safety.

```csharp
string value = GetValue()!;
```

If you are wrong, you still get a `NullReferenceException` at runtime.

---

## Required Members (C# 11+)

Force callers to set a property when creating an object.

```csharp
class User
{
    public required string Name { get; init; }
    public string? Nickname { get; init; }
}

var u = new User { Name = "Alice" };   // ok
var bad = new User();                  // compile error: Name is required
```

---

## Guard Clauses

```csharp
void Save(string path)
{
    ArgumentNullException.ThrowIfNull(path);
    ArgumentException.ThrowIfNullOrEmpty(path);
}
```

---

## Gotchas

- The checks are compile-time warnings only; `null` can still reach your code from reflection, JSON, or older libraries
- Turn warnings into errors with `<WarningsAsErrors>nullable</WarningsAsErrors>` for strict projects
- Non-nullable properties with no value set give warning CS8618; set them in the constructor or mark them `required`
- `string?` and `string` are the same type at runtime; the `?` is only a compiler hint
- Nullable value types (`int?`) are different: they are a real `Nullable<int>` type

---

## Examples

- [24-01](examples/24-01_nullable_references.cs): Nullable reference types and null operators

Run one with `dotnet run examples/24-01_nullable_references.cs`. See [Examples](examples/README.md).
