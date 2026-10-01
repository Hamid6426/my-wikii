# 51 - Generics

## What are Generics

Write code that works with any type: the type is specified by the caller, checked at compile time, and avoids boxing.

---

## Generic Method

```csharp
static T Max<T>(T a, T b) where T : IComparable<T>
{
    return a.CompareTo(b) >= 0 ? a : b;
}

int    m1 = Max(3, 7);          // 7
double m2 = Max(3.14, 2.71);    // 3.14
string m3 = Max("apple", "banana"); // "banana"
```

---

## Generic Class

```csharp
class Stack<T>
{
    private List<T> _items = new();

    public void Push(T item) => _items.Add(item);

    public T Pop()
    {
        if (_items.Count == 0) throw new InvalidOperationException("Stack is empty");
        T item = _items[^1];
        _items.RemoveAt(_items.Count - 1);
        return item;
    }

    public int Count => _items.Count;
}

var stack = new Stack<int>();
stack.Push(1);
stack.Push(2);
int top = stack.Pop();  // 2
```

---

## Generic Interface

```csharp
interface IRepository<T>
{
    T GetById(int id);
    void Add(T item);
    void Delete(int id);
    IEnumerable<T> GetAll();
}
```

---

## Type Constraints

Restrict which types can be used for a type parameter.

| Constraint                 | Meaning                                        |
| -------------------------- | ---------------------------------------------- |
| `where T : class`          | T must be a reference type                     |
| `where T : struct`         | T must be a value type                         |
| `where T : new()`          | T must have a public parameterless constructor |
| `where T : SomeClass`      | T must inherit from SomeClass                  |
| `where T : ISomeInterface` | T must implement ISomeInterface                |
| `where T : IComparable<T>` | T must implement IComparable<T>                |
| `where T : notnull`        | T must be non-nullable (C# 8+)                 |

Multiple constraints:

```csharp
static T Create<T>() where T : class, new()
{
    return new T();
}
```

---

## Generic with Multiple Type Parameters

```csharp
class Pair<TFirst, TSecond>
{
    public TFirst First  { get; }
    public TSecond Second { get; }

    public Pair(TFirst first, TSecond second)
    {
        First  = first;
        Second = second;
    }
}

var pair = new Pair<string, int>("Alice", 30);
Console.WriteLine($"{pair.First}: {pair.Second}");
```

---

## default Keyword with Generics

Returns the default value for any type.

```csharp
static T GetDefault<T>() => default;

int    n = GetDefault<int>();     // 0
string s = GetDefault<string>();  // null
bool   b = GetDefault<bool>();    // false
```

---

## Covariance and Contravariance

For interfaces and delegates with `out` (covariant) and `in` (contravariant).

```csharp
// Covariant: T can only appear as return type
interface IProducer<out T>
{
    T Produce();
}

// Contravariant: T can only appear as parameter type
interface IConsumer<in T>
{
    void Consume(T item);
}
```

```csharp
IProducer<Dog> dogProducer = ...;
IProducer<Animal> animalProducer = dogProducer;  // valid: Dog is an Animal
```

---

## Common Generic Types in .NET

| Type                       | Description            |
| -------------------------- | ---------------------- |
| `List<T>`                  | Dynamic array          |
| `Dictionary<TKey, TValue>` | Key-value map          |
| `HashSet<T>`               | Unique element set     |
| `Queue<T>`                 | FIFO collection        |
| `Stack<T>`                 | LIFO collection        |
| `Nullable<T>` / `T?`       | Optional value type    |
| `Task<T>`                  | Async operation result |
| `Func<T, TResult>`         | Generic delegate       |
| `Action<T>`                | Generic void delegate  |

---

## static abstract Members

An interface can require **static** members, which are called on the type itself and not on an object. Because the interface cannot be used as a plain type for this, you use it as a generic constraint.

```csharp
interface IAnimal<TSelf> where TSelf : IAnimal<TSelf>
{
    static abstract string Kind { get; }
    static abstract TSelf Create();
    string Speak();
}

class Dog : IAnimal<Dog>
{
    public static string Kind => "dog";
    public static Dog Create() => new Dog();
    public string Speak() => "Woof";
}

static string Describe<T>() where T : IAnimal<T> => T.Kind;       // call the static member on T
static T Make<T>() where T : IAnimal<T> => T.Create();

Console.WriteLine(Describe<Dog>());   // dog
Console.WriteLine(Make<Dog>().Speak());   // Woof
```

The `IAnimal<TSelf> where TSelf : IAnimal<TSelf>` shape, where a type passes itself as the type argument, is called the curiously recurring pattern. It lets `Create()` return the exact type, not the interface.

You can require operators the same way. That is how generic math works.

---

## Generic Math

The numeric types implement interfaces such as `INumber<T>` (in `System.Numerics`). One method now works for `int`, `double`, `decimal`, and every other number type.

```csharp
using System.Numerics;

static T Sum<T>(IEnumerable<T> items) where T : INumber<T>
{
    T total = T.Zero;                 // static abstract member
    foreach (T item in items) total += item;
    return total;
}

Console.WriteLine(Sum(new[] { 1, 2, 3 }));        // 6
Console.WriteLine(Sum(new[] { 1.5, 2.5 }));       // 4
Console.WriteLine(Sum(new[] { 10m, 20.5m }));     // 30.5

static T Clamp<T>(T value, T min, T max) where T : INumber<T> => T.Max(min, T.Min(max, value));

Console.WriteLine(Clamp(150, 0, 100));            // 100
Console.WriteLine(Clamp(-2.5, 0.0, 1.0));         // 0
```

Before this, you had to write `SumInt`, `SumDouble`, and `SumDecimal` separately.

### Pick the Smallest Interface You Need

`INumber<T>` is large. Constrain to just what the method uses:

| Interface                          | Gives you                                                  |
| ---------------------------------- | ---------------------------------------------------------- |
| `INumber<T>`                       | Everything: arithmetic, comparison, `Zero`, `One`, `Parse` |
| `IAdditionOperators<T, T, T>`      | `a + b`                                                    |
| `IMultiplyOperators<T, T, T>`      | `a * b`                                                    |
| `IParsable<T>`                     | `T.Parse(text, provider)`                                  |
| `IComparisonOperators<T, T, bool>` | `<`, `>`, `==`                                             |

```csharp
static T Square<T>(T x) where T : IMultiplyOperators<T, T, T> => x * x;

static T Parse<T>(string text) where T : IParsable<T> => T.Parse(text, null);

Console.WriteLine(Square(7));      // 49
Console.WriteLine(Square(1.5));    // 2.25
Console.WriteLine(Parse<int>("42") + Parse<int>("8"));   // 50
```

Your own types join in by implementing the interface and its operator:

```csharp
readonly record struct Meters(double Value) : IAdditionOperators<Meters, Meters, Meters>
{
    public static Meters operator +(Meters a, Meters b) => new(a.Value + b.Value);
}

static T Combine<T>(T a, T b) where T : IAdditionOperators<T, T, T> => a + b;

Console.WriteLine(Combine(new Meters(2), new Meters(3)));   // Meters { Value = 5 }
```

Converting between number types in generic code uses `T.CreateChecked(value)`, for example `Sum(items) / T.CreateChecked(items.Count)` to compute an average. See [Indexers and Operator Overloading](28_indexers_and_operator_overloading.md).

---

## Gotchas

- Generic type information is available at compile time and runtime (unlike Java type erasure)
- `T` cannot be used as an argument to `new T()` without the `new()` constraint
- Avoid constraints unless necessary: the more constraints, the less flexible the generic
- `default(T)` is null for reference types, zero for value types

---

## Examples

- [51-01](examples/51-01_generics.cs): Generic classes, methods, and constraints
- [51-02](examples/51-02_generic_math_and_static_abstract.cs): static abstract members and generic math

Run one with `dotnet run examples/51-01_generics.cs`. See [Examples](examples/README.md).
