# 34 - Collections

## List\<T\>

Dynamic array: grows automatically. Most common collection.

```csharp
var numbers = new List<int> { 1, 2, 3 };

numbers.Add(4);
numbers.AddRange(new[] { 5, 6 });
numbers.Insert(0, 0);          // insert at index 0
numbers.Remove(3);             // remove first occurrence of 3
numbers.RemoveAt(0);           // remove by index
numbers.Contains(5);           // true
numbers.Count;                 // length
numbers.Sort();
numbers.Reverse();
numbers.Clear();
```

---

## Dictionary\<TKey, TValue\>

Key-value pairs. Keys must be unique.

```csharp
var ages = new Dictionary<string, int>
{
    ["Alice"] = 30,
    ["Bob"]   = 25
};

ages["Carol"] = 28;
ages["Alice"] = 31;              // update

bool exists = ages.ContainsKey("Bob");
ages.TryGetValue("Dave", out int age);  // safe lookup

foreach (var kvp in ages)
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}
```

---

## HashSet\<T\>

Unique elements, unordered. Fast membership testing.

```csharp
var set = new HashSet<int> { 1, 2, 3 };

set.Add(4);
set.Add(2);           // duplicate: ignored
set.Remove(1);
set.Contains(3);      // true

var other = new HashSet<int> { 3, 4, 5 };
set.UnionWith(other);        // set ∪ other
set.IntersectWith(other);    // set ∩ other
set.ExceptWith(other);       // set \ other
```

---

## Collection Initializers

```csharp
var list = new List<string> { "a", "b", "c" };
var dict = new Dictionary<int, string> { { 1, "one" }, { 2, "two" } };
```

---

## IEnumerable\<T\> and ICollection\<T\>

Programming to an interface makes code more flexible.

```csharp
IEnumerable<int> nums = new List<int> { 1, 2, 3 };
foreach (int n in nums) Console.WriteLine(n);
```

---

## Collection Interfaces for APIs

Choose the interface for each parameter and return type on purpose. It decides what callers can do and what you can change later.

| Interface                           | Callers can                                     |
| ----------------------------------- | ----------------------------------------------- |
| `IEnumerable<T>`                    | Loop once. Nothing else                         |
| `IReadOnlyCollection<T>`            | Loop and read `Count`                           |
| `IReadOnlyList<T>`                  | Loop, read `Count`, and read by index `list[i]` |
| `ICollection<T>`                    | Loop, `Count`, `Add`, `Remove`, `Clear`         |
| `IList<T>`                          | Everything above, plus insert and index         |
| `IReadOnlyDictionary<TKey, TValue>` | Look up by key, no changes                      |

Two rules of thumb:

- **Parameters: accept the widest type you need.** `IEnumerable<int>` accepts a list, an array, and a LINQ query.
- **Return values: return the narrowest type that is still useful.** A read-only interface tells callers they must not change your data.

```csharp
static int Total(IEnumerable<int> numbers) => numbers.Sum();

Console.WriteLine(Total(new List<int> { 1, 2, 3 }));   // 6
Console.WriteLine(Total(new[] { 4, 5 }));              // 9
Console.WriteLine(Total(Enumerable.Range(1, 4)));      // 10
```

### Hiding a List

```csharp
class Library
{
    private readonly List<string> _titles = [];

    public void Add(string title) => _titles.Add(title);

    public IReadOnlyList<string> Titles => _titles;                  // exposes the list itself
    public IReadOnlyList<string> SafeTitles => _titles.AsReadOnly(); // a wrapper
}
```

`Titles` has no `Add`, so ordinary code cannot change the list. But the object behind it is still a `List<string>`, and a caller can cast it back and change it:

```csharp
if (library.Titles is List<string> sneaky)
{
    sneaky.Add("Sneaked in");   // works, and the library changes
}
```

`AsReadOnly()` returns a wrapper that cannot be cast back to a `List<T>`. Use it when callers are not trusted. For data that never changes, the immutable collections in `System.Collections.Immutable` (`ImmutableList<T>`, `ImmutableArray<T>`) cannot be changed by anyone.

Do not use `IList<T>` for a read-only result. It promises `Add`, which would then throw at run time.

---

## Gotchas

- `List<T>` index access is O(1), but `Contains` is O(n): use `HashSet` for fast lookups
- Modifying a collection while iterating with `foreach` throws `InvalidOperationException`
- `Dictionary` does not guarantee iteration order

---

## Examples

- [34-01](examples/34-01_collections_tour.cs): List, Dictionary, HashSet
- [34-02](examples/34-02_collection_interfaces.cs): IEnumerable, IReadOnlyList, IList, and API design

Run one with `dotnet run examples/34-01_collections_tour.cs`. See [Examples](examples/README.md).
