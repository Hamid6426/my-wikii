# 52 - Attributes and Reflection

## Attributes

Metadata you attach to code with square brackets. They do nothing on their own; something else reads them.

```csharp
[Obsolete("Use NewMethod instead")]
void OldMethod() { }

[Serializable]
class Data { }
```

---

## Common Built-in Attributes

| Attribute                 | Effect                                           |
| ------------------------- | ------------------------------------------------ |
| `[Obsolete]`              | Compiler warning (or error) when used            |
| `[Flags]`                 | Marks an enum as a set of bit flags              |
| `[Conditional("DEBUG")]`  | Call is removed unless the symbol is defined     |
| `[CallerMemberName]`      | Fills a parameter with the calling method's name |
| `[NotNull]` `[MaybeNull]` | Hints for nullable analysis                      |
| `[Fact]` `[Theory]`       | Mark xUnit tests                                 |
| `[JsonPropertyName]`      | Rename a property for JSON                       |

```csharp
void Log(string msg, [CallerMemberName] string caller = "")
    => Console.WriteLine($"{caller}: {msg}");
```

---

## Targets and Parameters

```csharp
[Obsolete("Old", error: true)]        // positional and named arguments
[assembly: InternalsVisibleTo("MyApp.Tests")]   // applies to the whole assembly
[field: NonSerialized] public string Temp { get; set; } = "";
```

---

## Writing Your Own Attribute

```csharp
[AttributeUsage(AttributeTargets.Property)]
class MaxLengthAttribute : Attribute
{
    public int Length { get; }
    public MaxLengthAttribute(int length) => Length = length;
}

class User
{
    [MaxLength(10)]
    public string Name { get; set; } = "";
}
```

The class name ends in `Attribute`, but you write it without that suffix.

---

## Reflection

Look at types and members while the program runs.

```csharp
Type t = typeof(User);
Console.WriteLine(t.Name);                        // User

foreach (var prop in t.GetProperties())
{
    Console.WriteLine($"{prop.Name}: {prop.PropertyType.Name}");
}
```

From an instance:

```csharp
var u = new User();
Type same = u.GetType();
```

---

## Reading Attributes

```csharp
foreach (var prop in typeof(User).GetProperties())
{
    var max = prop.GetCustomAttribute<MaxLengthAttribute>();
    if (max != null)
        Console.WriteLine($"{prop.Name} max {max.Length}");
}
```

`GetCustomAttribute<T>` needs `using System.Reflection;`.

---

## Reading and Setting Values

```csharp
var user = new User();
PropertyInfo name = typeof(User).GetProperty("Name")!;

name.SetValue(user, "Alice");
Console.WriteLine(name.GetValue(user));    // Alice
```

---

## Creating Objects and Calling Methods

```csharp
object? obj = Activator.CreateInstance(typeof(User));

MethodInfo? m = typeof(string).GetMethod("ToUpper", Type.EmptyTypes);
object? result = m?.Invoke("hello", null);   // HELLO
```

---

## Loading Types from an Assembly

```csharp
var types = typeof(User).Assembly
    .GetTypes()
    .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract);
```

This is how plugin systems and dependency injection containers find classes.

---

## Gotchas

- Reflection is slow compared to a normal call; cache `PropertyInfo` and `MethodInfo` objects
- It bypasses compile-time checks, so a typo in a name string fails at runtime; use `nameof(...)` where possible
- It can reach private members with `BindingFlags.NonPublic | BindingFlags.Instance`, which can break encapsulation
- Trimming and AOT compilation can remove members that only reflection uses; source generators avoid this
- Prefer a simple interface or generic over reflection when you can

---

## Examples

- [52-01](examples/52-01_reflection_and_attributes.cs): Custom attributes and reflection

Run one with `dotnet run examples/52-01_reflection_and_attributes.cs`. See [Examples](examples/README.md).
