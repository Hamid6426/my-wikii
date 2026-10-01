# 32 - IDisposable

## IDisposable Pattern

Types that hold resources (files, connections) implement `IDisposable` so `using` can release them.

```csharp
class FileHandler : IDisposable
{
    private bool _disposed = false;

    public void Dispose()
    {
        if (!_disposed)
        {
            // release resources
            _disposed = true;
        }
    }
}

using (var handler = new FileHandler())
{
    // Dispose() is called automatically at end of block
}
```

---

## using Declaration (C# 8+)

No braces needed. `Dispose()` runs at the end of the enclosing block.

```csharp
void Save()
{
    using var handler = new FileHandler();
    // work with handler
}   // Dispose() is called here
```

---

## Async Disposal

Use `IAsyncDisposable` when cleanup needs `await`, such as flushing a stream.

```csharp
class Logger : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        await FlushAsync();
    }
}

await using var logger = new Logger();
```

---

## The Full Dispose Pattern

The simple version above is enough for a class that only holds other `IDisposable` objects. A class that owns an **unmanaged resource** (memory, a native handle) needs the full pattern, so the resource is freed even if the caller forgets `Dispose()`.

```csharp
using System.Runtime.InteropServices;

class BufferHolder : IDisposable
{
    private IntPtr _memory;                  // unmanaged resource
    private readonly List<string> _log = []; // managed resource
    private bool _disposed;

    public BufferHolder(int size) => _memory = Marshal.AllocHGlobal(size);

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);           // no need for the finalizer now
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _log.Clear();                    // managed: only when called from Dispose()
        }

        Marshal.FreeHGlobal(_memory);        // unmanaged: always release
        _memory = IntPtr.Zero;
        _disposed = true;
    }

    ~BufferHolder() => Dispose(disposing: false);   // safety net if Dispose() was forgotten

    public int Read(int offset)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return Marshal.ReadInt32(_memory, offset);
    }
}
```

| Piece                             | Job                                                                                                                                        |
| --------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------ |
| `Dispose()`                       | The public method. Calls `Dispose(true)` and suppresses the finalizer                                                                      |
| `Dispose(bool disposing)`         | `true`: called by you, so managed and unmanaged parts are safe to release. `false`: called by the finalizer, so touch only unmanaged parts |
| Finalizer `~BufferHolder()`       | Runs when the garbage collector frees the object. A last-chance cleanup                                                                    |
| `GC.SuppressFinalize(this)`       | Tells the collector the finalizer is not needed, because you already cleaned up                                                            |
| `_disposed` flag                  | Makes a second `Dispose()` do nothing                                                                                                      |
| `ObjectDisposedException.ThrowIf` | Rejects use after disposal                                                                                                                 |

---

## SafeHandle: Skip the Finalizer

Writing finalizers is easy to get wrong. A `SafeHandle` wraps one unmanaged handle, has a correct finalizer already, and is the recommended way to own native resources.

```csharp
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

class NativeMemory : SafeHandleZeroOrMinusOneIsInvalid
{
    public NativeMemory(int size) : base(ownsHandle: true) => SetHandle(Marshal.AllocHGlobal(size));

    protected override bool ReleaseHandle()
    {
        Marshal.FreeHGlobal(handle);
        return true;
    }
}

using (var memory = new NativeMemory(16))
{
    Marshal.WriteInt32(memory.DangerousGetHandle(), 7);
}   // ReleaseHandle() runs here
```

Many framework types already are safe handles. `File.OpenHandle(path)` returns a `SafeFileHandle`, so `using` releases it.

Your own class that holds a `SafeHandle` only needs the simple `IDisposable` version. It disposes the handle and has no finalizer.

---

## Which One Do I Write

| Your class holds                                     | Write                                                            |
| ---------------------------------------------------- | ---------------------------------------------------------------- |
| Nothing disposable                                   | Nothing                                                          |
| Other `IDisposable` objects                          | Simple `Dispose()` that disposes them. Seal the class if you can |
| A native handle                                      | A `SafeHandle` subclass, then simple `Dispose()` in the owner    |
| A raw native pointer, in a class others inherit from | The full pattern with a finalizer                                |

---

## Gotchas

- Calling `Dispose()` twice must be safe, which is why the example checks a `_disposed` flag
- Do not use an object after it is disposed; throw `ObjectDisposedException` if someone does
- If a class owns an `IDisposable` field, the class should be `IDisposable` too and dispose that field
- A `using` variable is read-only; you cannot point it at a different object

---

## Examples

- [32-01](examples/32-01_idisposable.cs): IDisposable, using, and async disposal
- [32-02](examples/32-02_dispose_pattern.cs): The full dispose pattern and SafeHandle

Run one with `dotnet run examples/32-01_idisposable.cs`. See [Examples](examples/README.md).
