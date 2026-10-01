# 37 - Delegates and Lambdas

## What is a Delegate

A variable that holds a method. It lets you pass behavior around like data.

```csharp
delegate int Operation(int a, int b);

static int Add(int a, int b) => a + b;
static int Multiply(int a, int b) => a * b;

Operation op = Add;
Console.WriteLine(op(2, 3));   // 5

op = Multiply;
Console.WriteLine(op(2, 3));   // 6
```

---

## Built-in Delegates

You rarely declare your own. Use these.

| Type           | Shape                       | Example                   |
| -------------- | --------------------------- | ------------------------- |
| `Action`       | No return value             | `Action<string> log`      |
| `Func<..., R>` | Returns a value (last type) | `Func<int, int, int> add` |
| `Predicate<T>` | Takes `T`, returns `bool`   | `Predicate<int> isEven`   |

```csharp
Action<string> print = s => Console.WriteLine(s);
Func<int, int, int> add = (a, b) => a + b;
Predicate<int> isEven = n => n % 2 == 0;
```

---

## Lambda Expressions

A short, nameless method. The `=>` reads as "goes to".

```csharp
Func<int, int> square = x => x * x;              // expression body

Func<int, int, int> max = (a, b) =>              // statement body
{
    if (a > b) return a;
    return b;
};

Action hello = () => Console.WriteLine("Hi");    // no parameters
```

---

## Passing Behavior to Methods

```csharp
static List<int> Filter(List<int> items, Func<int, bool> keep)
{
    var result = new List<int>();
    foreach (var i in items)
        if (keep(i)) result.Add(i);
    return result;
}

var evens = Filter(new() { 1, 2, 3, 4 }, n => n % 2 == 0);
```

This is how LINQ methods like `Where` and `Select` work.

---

## Method Groups

Pass a method name directly when its signature fits.

```csharp
var words = new List<string> { "a", "b" };
words.ForEach(Console.WriteLine);
```

---

## Closures

A lambda can use variables from the method around it.

```csharp
int factor = 3;
Func<int, int> times = x => x * factor;

factor = 10;
Console.WriteLine(times(2));   // 20, not 6: the lambda sees the variable, not a copy
```

---

## Static Lambdas and Discards

```csharp
Func<int, int> twice = static x => x * 2;       // cannot capture outer variables

Func<int, int, int> first = (a, _) => a;        // ignore a parameter
```

---

## Multicast Delegates

One delegate can hold several methods. All run in order.

```csharp
Action notify = () => Console.WriteLine("A");
notify += () => Console.WriteLine("B");

notify();   // A, then B
```

---

## Gotchas

- A closure captures the variable, not its value at that moment, so later changes show up inside the lambda
- Capturing a loop variable in an old-style `for` loop shares one variable across all lambdas; `foreach` variables are fresh per iteration
- Lambdas that capture variables allocate; use `static` lambdas in hot paths
- Invoking a null delegate throws; call it as `handler?.Invoke()`
- Multicast delegates return only the last method's result

---

## Examples

- [37-01](examples/37-01_delegates_and_lambdas.cs): Delegates, Func, Action, closures, and multicast

Run one with `dotnet run examples/37-01_delegates_and_lambdas.cs`. See [Examples](examples/README.md).
