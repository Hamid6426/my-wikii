# 41 - LINQ

## What is LINQ

Language-Integrated Query: query syntax built into C# for filtering, transforming, and aggregating data from any `IEnumerable<T>` source (collections, arrays, XML, databases).

---

## Two Syntaxes

### Query Syntax (SQL-like)

```csharp
int[] nums = { 1, 2, 3, 4, 5, 6 };

var evens = from n in nums
            where n % 2 == 0
            select n;
```

### Method Syntax (Lambda-based): preferred in modern C#

```csharp
var evens = nums.Where(n => n % 2 == 0);
```

Both produce the same result. Method syntax is more composable and widely used.

---

## Common LINQ Methods

### Filtering

```csharp
var adults = people.Where(p => p.Age >= 18);
```

### Projection

```csharp
var names = people.Select(p => p.Name);
var pairs = people.Select(p => new { p.Name, p.Age });  // anonymous type
```

### Ordering

```csharp
var sorted = people.OrderBy(p => p.Name);
var desc   = people.OrderByDescending(p => p.Age);
var multi  = people.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
```

### Aggregation

```csharp
int count  = nums.Count();
int sum    = nums.Sum();
double avg = nums.Average();
int min    = nums.Min();
int max    = nums.Max();
```

### First / Single / Last

```csharp
int first  = nums.First();                   // throws if empty
int firstN = nums.FirstOrDefault();          // 0 if empty
int single = nums.Single(n => n == 5);       // throws if not exactly one
int last   = nums.Last();
```

### Existence Checks

```csharp
bool any  = nums.Any(n => n > 10);          // at least one match
bool all  = nums.All(n => n > 0);           // all match
bool none = !nums.Any(n => n < 0);
```

### Take and Skip

```csharp
var first3 = nums.Take(3);          // [1, 2, 3]
var skip2  = nums.Skip(2);          // [3, 4, 5, 6]
var page   = nums.Skip(2).Take(3);  // [3, 4, 5]: pagination
```

### Distinct / GroupBy / Join

```csharp
var unique = nums.Distinct();

var groups = people.GroupBy(p => p.Department);
foreach (var group in groups)
{
    Console.WriteLine($"{group.Key}: {group.Count()}");
}

var result = orders.Join(
    customers,
    o => o.CustomerId,
    c => c.Id,
    (o, c) => new { c.Name, o.Total }
);
```

### SelectMany: flatten nested collections

```csharp
string[] words = { "hello world", "foo bar" };
var letters = words.SelectMany(w => w.Split(' '));
// ["hello", "world", "foo", "bar"]
```

---

## Converting Results

LINQ queries are lazy: they execute when iterated. Force evaluation:

```csharp
List<int> list = nums.Where(n => n > 2).ToList();
int[]     arr  = nums.Where(n => n > 2).ToArray();
Dictionary<int, string> dict = people.ToDictionary(p => p.Id, p => p.Name);
```

---

## Chaining

```csharp
var result = people
    .Where(p => p.Age > 18)
    .OrderBy(p => p.Name)
    .Select(p => p.Name)
    .Take(5)
    .ToList();
```

---

## LINQ on Strings

```csharp
string sentence = "the quick brown fox";
var words = sentence.Split(' ')
                    .Where(w => w.Length > 3)
                    .OrderBy(w => w)
                    .ToList();
```

---

## Expression Trees

A lambda stored as data instead of compiled code. A library can read it and translate it, for example into SQL. This is how Entity Framework turns `Where(u => u.Age > 18)` into a database query.

```csharp
using System.Linq.Expressions;

Func<int, bool> compiled = n => n > 5;             // runs as code
Expression<Func<int, bool>> tree = n => n > 5;     // kept as a tree of nodes

Console.WriteLine(tree.Body);                      // (n > 5)
Console.WriteLine(compiled(10));                   // True

Func<int, bool> run = tree.Compile();              // turn the tree into code
Console.WriteLine(run(10));                        // True
```

Inspect the parts:

```csharp
var body = (BinaryExpression)tree.Body;
Console.WriteLine(body.NodeType);      // GreaterThan
Console.WriteLine(body.Left);          // n
Console.WriteLine(body.Right);         // 5
```

Where you meet them:

| Type                     | Used by                                           |
| ------------------------ | ------------------------------------------------- |
| `IQueryable<T>` methods  | Database LINQ providers take `Expression<Func<>>` |
| `IEnumerable<T>` methods | In-memory LINQ takes plain `Func<>`               |

You rarely build trees by hand. The difference matters when a query works in memory but fails against a database because a method cannot be translated.

---

## Gotchas

- LINQ queries are **lazy**: they don't execute until iterated or materialized with `ToList()`/`ToArray()`
- Multiple iterations of a lazy query re-execute it: call `ToList()` if you need to iterate more than once
- `First()` throws on empty; `FirstOrDefault()` returns the default value
- `Single()` throws if there is more than one match
- LINQ on `IQueryable<T>` (e.g., Entity Framework) is translated to SQL; not all C# methods are supported

---

## Examples

- [41-01](examples/41-01_linq_queries.cs): LINQ: filter, project, group, join, aggregate

Run one with `dotnet run examples/41-01_linq_queries.cs`. See [Examples](examples/README.md).
