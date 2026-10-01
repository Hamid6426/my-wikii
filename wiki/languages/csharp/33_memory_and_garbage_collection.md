# 33 - Memory and Garbage Collection

## Stack and Heap

| Place | Holds                               | Cleaned up by                  |
| ----- | ----------------------------------- | ------------------------------ |
| Stack | Local value types, method call data | Automatically when method ends |
| Heap  | Objects (class instances), arrays   | The garbage collector (GC)     |

A reference variable lives on the stack and points to an object on the heap.

---

## The Garbage Collector

The GC finds heap objects nothing points to anymore and frees them. You never call `delete`.

```csharp
void Work()
{
    var data = new byte[1_000_000];   // allocated on the heap
}   // 'data' goes out of scope; the GC frees the array later
```

You do not control when it runs.

---

## Generations

Objects are grouped by age so the GC can check young objects often and old ones rarely.

| Generation | Holds                         | Collected  |
| ---------- | ----------------------------- | ---------- |
| Gen 0      | New, short-lived objects      | Very often |
| Gen 1      | Survivors of one collection   | Sometimes  |
| Gen 2      | Long-lived objects            | Rarely     |
| LOH        | Large objects (85,000+ bytes) | With Gen 2 |

---

## Memory Leaks Still Happen

The GC only frees objects nothing references. Common leaks:

- Event handlers never unsubscribed (the publisher keeps the subscriber alive)
- Static collections that only grow
- Caches with no size limit
- Unmanaged resources never disposed (see [IDisposable](32_idisposable.md))

---

## Finalizers vs Dispose

| Feature   | Runs when                   | Use for                  |
| --------- | --------------------------- | ------------------------ |
| `Dispose` | You call it or `using` ends | Releasing resources now  |
| Finalizer | The GC gets to it, sometime | Safety net for unmanaged |

Prefer `using` and `Dispose`. Do not write a finalizer unless you hold raw unmanaged handles.

---

## Span\<T\>

A view over a slice of memory with no copy and no heap allocation.

```csharp
string text = "2026-10-01";

ReadOnlySpan<char> year = text.AsSpan(0, 4);
int y = int.Parse(year);            // no substring created

int[] nums = { 1, 2, 3, 4, 5 };
Span<int> middle = nums.AsSpan(1, 3);
middle[0] = 99;                     // changes nums[1]
```

`Span<T>` is a `ref struct`: it lives on the stack only, so it cannot be a field of a class or used across an `await`.

---

## Lazy\<T\>

Delay creating something expensive until first use.

```csharp
private readonly Lazy<Report> _report = new(() => BuildReport());

public Report Report => _report.Value;   // built on first access, then reused
```

---

## WeakReference

Points to an object without keeping it alive. Useful for caches.

```csharp
var weak = new WeakReference<byte[]>(new byte[1000]);

if (weak.TryGetTarget(out var data))
{
    // still alive
}
```

---

## Unsafe Code and Interop

Most C# never needs these. They exist for speed and for talking to native libraries.

### unsafe and Pointers

Pointers read and write memory directly, with no safety checks. Enable them in the `.csproj`:

```xml
<AllowUnsafeBlocks>true</AllowUnsafeBlocks>
```

```csharp
unsafe
{
    int value = 42;
    int* p = &value;     // address of value
    *p = 100;            // write through the pointer
    Console.WriteLine(value);   // 100
}
```

### stackalloc

Allocate a small array on the stack, so the garbage collector never sees it. Use it with `Span<T>` and no `unsafe` is needed.

```csharp
Span<int> buffer = stackalloc int[16];
buffer[0] = 1;
```

Keep it small (a few kilobytes). A stack overflow cannot be caught.

### P/Invoke

Call a function from a native library (a `.dll` on Windows, `.so` on Linux, `.dylib` on macOS).

```csharp
using System.Runtime.InteropServices;

Console.WriteLine(Native.GetProcessId());

static partial class Native
{
    [LibraryImport("libc", EntryPoint = "getpid")]
    internal static partial int GetProcessId();
}
```

This example runs on Linux. The class and method must be `static partial`, and the project needs `<AllowUnsafeBlocks>true</AllowUnsafeBlocks>` (the generated code is unsafe), or the build fails with error SYSLIB1062.

`LibraryImport` (.NET 7+) generates the marshalling code at build time. `DllImport` is the older form that still works.

The library name differs per system. The Windows equivalent of the call above:

```csharp
[LibraryImport("kernel32.dll")]
internal static partial uint GetCurrentProcessId();
```

### Safe Alternatives First

| Need                     | Use before `unsafe`         |
| ------------------------ | --------------------------- |
| Work on part of an array | `Span<T>` and `Memory<T>`   |
| Small temporary buffer   | `stackalloc` with `Span<T>` |
| Reuse big buffers        | `ArrayPool<T>.Shared`       |
| Call native code         | `LibraryImport`             |

---

## Gotchas

- Do not call `GC.Collect()` in normal code; it usually makes programs slower
- Allocating many short-lived objects in a hot loop creates GC pressure; reuse buffers or use `Span<T>`
- Big arrays (85,000+ bytes) go on the Large Object Heap, which is expensive to clean
- `struct` copies can cost more than a reference when the struct is large
- Measure with a profiler or `dotnet-counters` before optimizing memory

---

## Examples

- [33-01](examples/33-01_span_and_memory.cs): Span, stackalloc, Lazy, and allocation counting

Run one with `dotnet run examples/33-01_span_and_memory.cs`. See [Examples](examples/README.md).
