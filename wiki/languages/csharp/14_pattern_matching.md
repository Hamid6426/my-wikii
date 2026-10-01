# 14 - Pattern Matching

## What is Pattern Matching

Test a value's shape and pull data out of it in one step. It works with `is` and `switch`.

---

## Type Pattern

```csharp
object o = "hello";

if (o is string s)
{
    Console.WriteLine(s.Length);   // s is already a string here
}

if (o is not null) { }
```

---

## Constant and Relational Patterns

```csharp
if (age is 18) { }
if (age is >= 18 and < 65) { }
if (c is 'a' or 'e' or 'i' or 'o' or 'u') { }
```

`and`, `or`, and `not` combine patterns.

---

## Property Pattern

```csharp
if (person is { Age: >= 18, Name: "Alice" })
{
    Console.WriteLine("Adult Alice");
}

// Nested
if (order is { Customer.Address.Country: "PK" }) { }
```

---

## Switch Expression

```csharp
string Describe(object? o) => o switch
{
    null            => "nothing",
    int n when n < 0 => "negative number",
    int n           => $"number {n}",
    string s        => $"text of length {s.Length}",
    int[] { Length: 0 } => "empty array",
    _               => "something else"
};
```

The `_` arm is the default. The compiler warns if your arms do not cover every case.

---

## Positional Pattern

Works on tuples and on types with a `Deconstruct` method (records have one).

```csharp
string Quadrant(int x, int y) => (x, y) switch
{
    (0, 0)           => "origin",
    ( > 0, > 0)      => "first",
    ( < 0, > 0)      => "second",
    ( < 0, < 0)      => "third",
    ( > 0, < 0)      => "fourth",
    _                => "on an axis"
};
```

---

## List Pattern (C# 11+)

```csharp
int[] nums = { 1, 2, 3 };

if (nums is [1, _, 3]) { }                  // exact shape
if (nums is [var first, .., var last]) { }  // first and last
if (nums is [_, ..]) { }                    // at least one item
```

---

## Gotchas

- Order of switch arms matters: the first match wins, and a later arm that can never match is a compile error
- `when` adds a condition to an arm, but the compiler cannot check it for coverage
- `is not null` is safer than `!= null` because it ignores an overloaded `!=`
- A switch expression with no match and no `_` arm throws `SwitchExpressionException`

---

## Examples

- [14-01](examples/14-01_shape_describer.cs): Pattern matching with is, switch, and list patterns

Run one with `dotnet run examples/14-01_shape_describer.cs`. See [Examples](examples/README.md).
