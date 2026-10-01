# 58 - Performance Basics

## The Rules

1. **Measure first.** Programs are rarely slow where you expect
2. **Fix the biggest cost first.** Making a 1 ms step twice as fast does nothing if another step takes 2 seconds
3. **Measure again** to see that the change helped
4. **Keep the code clear.** Slower, readable code beats fast code nobody understands, unless a measurement says otherwise

---

## Build in Release Mode

Debug builds skip optimizations. Never time a Debug build.

```bash
dotnet run -c Release
```

---

## Measuring with Stopwatch

```csharp
using System.Diagnostics;

var sw = Stopwatch.StartNew();
DoWork();
sw.Stop();

Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
Console.WriteLine(sw.Elapsed);        // a TimeSpan, for example 00:00:00.0123456
```

A small helper that avoids the common mistakes:

```csharp
static long TimeMicroseconds(Action action, int runs = 5)
{
    action();                                  // warm up: first run pays one-time costs
    long best = long.MaxValue;

    for (int i = 0; i < runs; i++)
    {
        var sw = Stopwatch.StartNew();
        action();
        sw.Stop();
        best = Math.Min(best, sw.ElapsedTicks);
    }

    return best * 1_000_000 / Stopwatch.Frequency;
}
```

| Mistake                       | Why it misleads                                                        |
| ----------------------------- | ---------------------------------------------------------------------- |
| Timing one run                | The first call includes JIT compiling and cache setup                  |
| Timing a Debug build          | Optimizations are off                                                  |
| Measuring something too small | A few nanoseconds is below what the clock can show; loop it many times |
| Result never used             | The compiler may remove code whose result is ignored                   |
| Other programs running        | Noise. Close them and repeat                                           |

For precise results use a benchmarking library. See [Common Libraries](63_libraries.md).

---

## Measuring Memory

Allocations are often the real cost, because the garbage collector has to clean them up.

```csharp
long before = GC.GetAllocatedBytesForCurrentThread();
DoWork();
long after = GC.GetAllocatedBytesForCurrentThread();

Console.WriteLine($"{after - before} bytes allocated");
Console.WriteLine($"Gen 0 collections so far: {GC.CollectionCount(0)}");
```

Run `DoWork()` once before measuring. The first call can allocate extra memory for one-time setup. See [Memory and Garbage Collection](33_memory_and_garbage_collection.md).

---

## Pick the Right Data Structure

The biggest wins usually come from this. One measured run (1,000 lookups in 10,000 items, Release build):

| Operation              | List\<T\>.Contains | HashSet\<T\>.Contains |
| ---------------------- | ------------------ | --------------------- |
| Time for 1,000 lookups | about 1,400 us     | about 30 us           |

`List.Contains` checks items one by one (O(n)). `HashSet` jumps straight to the item (O(1)). Your numbers will differ, but the gap stays large. See [Collections](34_collections.md).

| Need                       | Use                        |
| -------------------------- | -------------------------- |
| Fast "is this in the set?" | `HashSet<T>`               |
| Fast lookup by key         | `Dictionary<TKey, TValue>` |
| Process in arrival order   | `Queue<T>`                 |
| Read by index, add at end  | `List<T>`                  |

---

## Strings in Loops

```csharp
// Slow: every += creates a new string and copies the old one
string s = "";
for (int i = 0; i < 20_000; i++) s += "x";

// Fast: one buffer that grows
var sb = new StringBuilder();
for (int i = 0; i < 20_000; i++) sb.Append('x');
string result = sb.ToString();
```

In one measured run the `+=` loop took about 27 ms and `StringBuilder` about 0.04 ms, more than 500 times faster. See [Strings](21_strings.md).

---

## Avoid Boxing

Putting a value type in an `object` allocates on the heap.

```csharp
var old = new ArrayList();        // stores object: every int is boxed
for (int i = 0; i < 100_000; i++) old.Add(i);

var good = new List<int>();       // stores int directly
for (int i = 0; i < 100_000; i++) good.Add(i);
```

In one run the `List<int>` version was about 7 times faster. Use generic collections. See [Generics](51_generics.md).

---

## Set a Capacity

A `List<T>` doubles its internal array when full, copying everything each time. If you know the size, say so.

```csharp
var list = new List<int>(1_000_000);   // no regrowing
```

In one run, adding one million items took about half the time with a preset capacity.

---

## Span Instead of Copies

`Substring` makes a new string. `AsSpan` looks at the same memory.

```csharp
string text = "2026-10-01";

int a = int.Parse(text.Substring(0, 4));        // allocates a new 4-character string
int b = int.Parse(text.AsSpan(0, 4));           // allocates nothing
```

Measured after a warm-up: `Substring` allocated 32 bytes per call and `AsSpan` allocated 0. One call is cheap. Millions of calls add up.

---

## Reuse Buffers

```csharp
using System.Buffers;

byte[] buffer = ArrayPool<byte>.Shared.Rent(1024);
try
{
    // use buffer. It may be larger than 1024 bytes
}
finally
{
    ArrayPool<byte>.Shared.Return(buffer);
}
```

A pool hands out arrays that were already created, so the garbage collector has less to do. Do not use a buffer after returning it.

---

## LINQ and Loops

LINQ is clear and fast enough for most code. In tight loops over large data, a plain `foreach` avoids allocating enumerators and delegates.

```csharp
int count = nums.Where(n => n % 2 == 0).Count();   // clear

int count2 = 0;                                     // faster in a hot path
foreach (int n in nums)
{
    if (n % 2 == 0) count2++;
}
```

On 100,000 numbers the loop was about three times faster in a Release run. Change to a loop only when a measurement shows LINQ is the problem. See [LINQ](41_linq.md).

---

## Struct or Class

| Type     | Cost                                                          |
| -------- | ------------------------------------------------------------- |
| `class`  | Heap allocation, garbage collected, passed by reference       |
| `struct` | No heap allocation when local, but copied on every assignment |

A small, short-lived, immutable value (a point, a range) can be a `readonly struct`. A large struct copied around is slower than a class. See [Modern C# Features](40_modern_csharp_features.md).

---

## Async and ValueTask

`async` does not make code faster. It frees a thread while waiting, so more work can run at once.

```csharp
async ValueTask<int> GetCountAsync(bool cached)
{
    if (cached) return 42;              // no Task allocated on this path
    return await LoadFromDiskAsync();
}
```

`ValueTask<T>` avoids allocating a `Task` when the answer is often ready immediately. Use it for hot paths that usually finish synchronously, not everywhere. Never `await` the same `ValueTask` twice. See [Async / Await / Tasks](48_async_await_tasks.md).

---

## Do Less Work

The fastest code is code that does not run.

| Technique         | Example                                              |
| ----------------- | ---------------------------------------------------- |
| Stop early        | `Any()` instead of `Count() > 0`; `break` when found |
| Cache results     | Keep a computed value in a field or `Lazy<T>`        |
| Avoid repeats     | Move work that does not change out of a loop         |
| Fewer round trips | One call that returns 100 items, not 100 calls       |
| Do it later       | Lazy loading, deferred LINQ                          |

---

## Gotchas

- Debug build timings are not real. Compare Release to Release
- Results change between runs and machines. Compare two versions on the same machine, and repeat
- Do not optimize code that runs once. Optimize code that runs a million times
- `GC.Collect()` rarely helps and often makes programs slower
- Faster code that is wrong is worthless. Keep your tests, and run them after every change (see [Testing](56_testing.md))
- Network, disk, and database calls usually cost far more than any C# code. Reduce those first

---

## Examples

- [58-01](examples/58-01_measure_with_stopwatch.cs): Measure code with Stopwatch and GC counters

Run one with `dotnet run examples/58-01_measure_with_stopwatch.cs`. See [Examples](examples/README.md).
