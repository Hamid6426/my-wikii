# 25 - Records & Equality

## record (C# 9+)

Reference type designed for immutable data with value-based equality.

```csharp
record Person(string Name, int Age);

var p1 = new Person("Alice", 30);
var p2 = new Person("Alice", 30);

Console.WriteLine(p1 == p2);   // true: compares values, not references
Console.WriteLine(p1);         // Person { Name = Alice, Age = 30 }

// Non-destructive mutation
var p3 = p1 with { Age = 31 };
```

---

## record struct (C# 10+)

Value-based equality with value type semantics.

```csharp
record struct Point(double X, double Y);
```

---

## Equality

```csharp
// Reference equality (same object in memory)
object.ReferenceEquals(a, b);

// Value equality (override of == or Equals)
a == b
a.Equals(b)
```

Classes use reference equality by default; records use value equality.

---

## Equals, GetHashCode, and IEquatable\<T\>

A class compares by reference unless you teach it otherwise. To compare by value, write these together:

| Member                | Job                                                            |
| --------------------- | -------------------------------------------------------------- |
| `Equals(T? other)`    | The typed comparison. Declared by `IEquatable<T>`              |
| `Equals(object? obj)` | The general version. Call the typed one                        |
| `GetHashCode()`       | A number used by `HashSet` and `Dictionary` to find items fast |
| `==` and `!=`         | Optional, but keep them consistent with `Equals`               |

```csharp
sealed class Money(decimal amount, string currency) : IEquatable<Money>
{
    public decimal Amount { get; } = amount;
    public string Currency { get; } = currency;

    public bool Equals(Money? other) =>
        other is not null && Amount == other.Amount && Currency == other.Currency;

    public override bool Equals(object? obj) => Equals(obj as Money);

    public override int GetHashCode() => HashCode.Combine(Amount, Currency);

    public static bool operator ==(Money? left, Money? right) => left is null ? right is null : left.Equals(right);
    public static bool operator !=(Money? left, Money? right) => !(left == right);
}

var c1 = new Money(5m, "USD");
var c2 = new Money(5m, "USD");
Console.WriteLine(c1 == c2);   // True
```

Rules:

- Two objects that are equal **must** return the same hash code. Different hash codes for equal objects break sets and dictionaries
- Build the hash from the same members `Equals` compares. `HashCode.Combine` does this
- Compare only members that never change, or the hash changes while the object sits in a collection and it becomes impossible to find
- Implement `IEquatable<T>` on structs, so comparing does not box the value

---

## Equality Drives Collections

`HashSet<T>`, `Dictionary<TKey, TValue>`, `Distinct()`, `Contains`, and `GroupBy` all use `GetHashCode` first and `Equals` second.

```csharp
var set = new HashSet<Money> { c1, c2, new Money(5m, "EUR") };
Console.WriteLine(set.Count);   // 2: c1 and c2 are the same value

var prices = new Dictionary<Money, string> { [c1] = "first" };
Console.WriteLine(prices[c2]);  // first: an equal key finds the entry
```

Forget `GetHashCode` and the collections misbehave:

```csharp
class BadMoney(decimal amount)
{
    public decimal Amount { get; } = amount;
    public override bool Equals(object? obj) => obj is BadMoney m && m.Amount == Amount;
    // no GetHashCode override: the compiler warns (CS0659)
}

var bad1 = new BadMoney(5m);
var bad2 = new BadMoney(5m);
Console.WriteLine(bad1.Equals(bad2));                              // True
Console.WriteLine(new HashSet<BadMoney> { bad1, bad2 }.Count);     // 2, not 1
```

The two objects are equal, but they get different default hash codes, so the set never compares them.

---

## What a Record Generates

A `record` writes the equality code for you:

- `Equals` and `==` / `!=`, comparing every member
- A matching `GetHashCode`
- `ToString`, `Deconstruct`, and the copy used by `with`

Each member is compared with **its own** `Equals`. A `string` or `int` compares by value, but a `List<T>` compares by reference, because lists do not override `Equals`.

```csharp
record User(string Name, List<string> Roles);

var user = new User("Ann", ["admin"]);
var sameList = user with { Name = "Ann" };    // `with` copies the reference to the same list
var otherList = new User("Ann", ["admin"]);   // a new list with the same content

Console.WriteLine(user == sameList);    // True
Console.WriteLine(user == otherList);   // False
```

More record details:

```csharp
record Animal(string Name);
record Dog(string Name) : Animal(Name);

Animal a = new Dog("x");
Animal b = new Animal("x");
Console.WriteLine(a == b);   // False: records also compare their exact type
```

```csharp
record struct Pt(int X, int Y);

Console.WriteLine(new Pt(1, 2) == new Pt(1, 2));   // True
```

You can replace the generated comparison. Change `Equals` and keep `GetHashCode` in step:

```csharp
record Tag(string Value)
{
    public virtual bool Equals(Tag? other) =>
        other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
}

Console.WriteLine(new Tag("Ab") == new Tag("aB"));   // True
```

A plain `struct` has no generated `==`. Its default `Equals` compares the fields but boxes the argument. Use a `record struct` or implement `IEquatable<T>`.

---

## IEqualityComparer\<T\>

When you cannot change the type, or want a different rule for one collection, pass a comparer. The type itself stays unchanged.

```csharp
var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Alice", "ALICE", "bob" };
Console.WriteLine(names.Count);   // 2

class LengthComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y) => x?.Length == y?.Length;
    public int GetHashCode(string s) => s.Length;
}

var byLength = new HashSet<string>(new LengthComparer()) { "cat", "dog", "bird" };
Console.WriteLine(byLength.Count);   // 2: "cat" and "dog" count as the same
```

Comparers work with `Dictionary`, `Distinct`, `GroupBy`, `Contains`, `ToHashSet`, and `Except`. `StringComparer` has ready-made ones: `Ordinal`, `OrdinalIgnoreCase`, and culture-based versions.

For ordering rather than equality, see `IComparable<T>` and `IComparer<T>` in [Interfaces and Abstractions](31_interfaces_and_abstractions.md).

---

## Gotchas

- `record` provides `==`, `!=`, `ToString`, and deconstruction for free

---

## Examples

- [25-01](examples/25-01_records.cs): Records, with-expressions, and value equality
- [25-02](examples/25-02_equality_in_depth.cs): Equals, GetHashCode, IEquatable, and comparers

Run one with `dotnet run examples/25-01_records.cs`. See [Examples](examples/README.md).
