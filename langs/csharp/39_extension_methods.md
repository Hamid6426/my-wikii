# 39 - Extension Methods

## What is an Extension Method

Add a method to an existing type without changing its source or inheriting from it. It is how LINQ adds `Where` and `Select` to every collection.

---

## Writing One

Three rules: a `static` class, a `static` method, and `this` before the first parameter.

```csharp
static class StringExtensions
{
    public static bool IsNullOrBlank(this string? s)
        => string.IsNullOrWhiteSpace(s);

    public static string Truncate(this string s, int max)
        => s.Length <= max ? s : s[..max] + "...";
}
```

---

## Using One

```csharp
string title = "A very long article title";

Console.WriteLine(title.Truncate(10));    // A very lon...
Console.WriteLine(title.IsNullOrBlank()); // False
```

It reads like a normal method, but the compiler rewrites it to `StringExtensions.Truncate(title, 10)`.

---

## Extending Collections and Interfaces

```csharp
static class EnumerableExtensions
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : class
    {
        foreach (var item in source)
            if (item != null) yield return item;
    }
}

var names = new List<string?> { "a", null, "b" };
var clean = names.WhereNotNull().ToList();   // a, b
```

Extending an interface adds the method to every type that implements it.

---

## Extending Your Own Enums and Structs

```csharp
static class StatusExtensions
{
    public static string Label(this Status s) => s switch
    {
        Status.Active => "Running",
        Status.Closed => "Done",
        _ => "Waiting"
    };
}
```

---

## Where to Put Them

- One static class per extended type, named `TypeNameExtensions`
- Put it in the namespace where callers will look, or they need a `using` for it
- Keep them in a small, focused file

---

## Resolution Rules

An instance method always wins over an extension method with the same name and signature.

```csharp
class Dog { public string Speak() => "Woof"; }

static class DogExt { public static string Speak(this Dog d) => "Never called"; }
```

---

## Extension Members (C# 14)

C# 14 adds `extension` blocks that can also add properties and static members.

```csharp
static class StringExt
{
    extension(string s)
    {
        public bool IsBlank => string.IsNullOrWhiteSpace(s);
    }
}
```

The classic `this` syntax still works and is the most common form in existing code.

---

## Gotchas

- Extension methods cannot access private members of the type they extend
- They work on `null` references too, because they are plain static calls; check for `null` inside
- Too many extensions on common types like `string` or `object` make code hard to read
- A new instance method added to the type later silently takes priority over your extension
- They need the right `using` or the compiler says the method does not exist

---

## Examples

- [39-01](examples/39-01_extension_methods.cs): Extension methods on string, IEnumerable, and an enum

Run one with `dotnet run examples/39-01_extension_methods.cs`. See [Examples](examples/README.md).
