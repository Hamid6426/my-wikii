# 36 - Iterators

## What is an Iterator

A method that hands back values one at a time using `yield return`. The caller can loop over it with `foreach`.

```csharp
static IEnumerable<int> Count(int max)
{
    for (int i = 1; i <= max; i++)
    {
        yield return i;
    }
}

foreach (int n in Count(3))
{
    Console.WriteLine(n);   // 1, 2, 3
}
```

The method does not run all at once. It pauses at each `yield return` and resumes when the next value is asked for.

---

## Lazy Evaluation

```csharp
static IEnumerable<int> Numbers()
{
    Console.WriteLine("start");
    yield return 1;
    Console.WriteLine("middle");
    yield return 2;
}

var seq = Numbers();        // nothing printed yet
foreach (var n in seq) { }  // prints: start, middle
```

Nothing runs until something iterates the sequence. LINQ works the same way.

---

## yield break

Stop the sequence early.

```csharp
static IEnumerable<int> TakeUntilNegative(int[] nums)
{
    foreach (int n in nums)
    {
        if (n < 0) yield break;
        yield return n;
    }
}
```

---

## Infinite Sequences

```csharp
static IEnumerable<int> Naturals()
{
    int i = 0;
    while (true) yield return i++;
}

var firstFive = Naturals().Take(5);   // use Take or break out, or it never ends
```

---

## Implementing IEnumerable\<T\>

Put `GetEnumerator` on your own type so `foreach` works on it.

```csharp
class Deck : IEnumerable<string>
{
    private readonly string[] _cards = { "A", "K", "Q" };

    public IEnumerator<string> GetEnumerator()
    {
        foreach (var c in _cards) yield return c;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        => GetEnumerator();
}
```

---

## Under the Hood

The compiler turns an iterator into a hidden class that implements `IEnumerator<T>` as a state machine. You write a simple loop and get the state tracking for free.

---

## Gotchas

- Iterating the same lazy sequence twice runs the method twice; call `.ToList()` to run it once
- Arguments are not checked until the first `MoveNext`, so validation errors show up late; split into a public checker method and a private iterator
- You cannot use `yield` inside `try/catch` blocks (only `try/finally`), and not in `async` methods (use `IAsyncEnumerable<T>` there)
- If the source collection changes during iteration, `foreach` throws `InvalidOperationException`

---

## Examples

- [36-01](examples/36-01_iterators.cs): yield return, lazy sequences, custom enumerables

Run one with `dotnet run examples/36-01_iterators.cs`. See [Examples](examples/README.md).
