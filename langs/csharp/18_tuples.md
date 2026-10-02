# 18 - Tuples

## What is a Tuple

A small group of values bundled together without writing a class. Good for returning more than one value from a method.

```csharp
(string, int) person = ("Alice", 30);
Console.WriteLine(person.Item1);   // Alice
Console.WriteLine(person.Item2);   // 30
```

---

## Named Elements

```csharp
(string Name, int Age) person = ("Alice", 30);
Console.WriteLine(person.Name);

var p = (Name: "Bob", Age: 25);
Console.WriteLine(p.Age);
```

---

## Returning Multiple Values

```csharp
static (int Min, int Max) MinMax(int[] nums)
{
    return (nums.Min(), nums.Max());
}

var result = MinMax(new[] { 3, 1, 9 });
Console.WriteLine($"{result.Min} {result.Max}");   // 1 9
```

---

## Deconstruction

Unpack a tuple into separate variables.

```csharp
var (min, max) = MinMax(new[] { 3, 1, 9 });

(string name, int age) = ("Alice", 30);

// Ignore a value with a discard
var (_, maxOnly) = MinMax(new[] { 3, 1, 9 });
```

Swap two variables without a temp:

```csharp
(a, b) = (b, a);
```

---

## Deconstruct for Your Own Types

```csharp
class Point
{
    public int X { get; }
    public int Y { get; }
    public Point(int x, int y) => (X, Y) = (x, y);

    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
}

var (px, py) = new Point(1, 2);
```

---

## Equality

Tuples compare element by element.

```csharp
Console.WriteLine((1, "a") == (1, "a"));   // true
```

---

## Tuple vs Class vs Record

| Use          | When                                          |
| ------------ | --------------------------------------------- |
| Tuple        | Short-lived result inside one method or class |
| Record       | Named data that is passed around or stored    |
| Class/Struct | Data with behavior                            |

---

## Gotchas

- `(int, int)` is a `ValueTuple`, a struct, so it is copied on assignment
- Tuple element names exist only at compile time; they are not part of the runtime type
- Avoid tuples in public APIs; a record gives clearer names and a stable shape
- More than three or four elements is a sign you need a real type

---

## Examples

- [18-01](examples/18-01_tuples.cs): Tuples, named elements, and deconstruction

Run one with `dotnet run examples/18-01_tuples.cs`. See [Examples](examples/README.md).
