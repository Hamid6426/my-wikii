// Lesson 32: IDisposable (../32_idisposable.md)
// IDisposable, using, and async disposal
// Run: dotnet run 32-01_idisposable.cs

using (var first = new Resource("first"))
{
    Console.WriteLine("working with first");
}

using var second = new Resource("second");
Console.WriteLine("working with second");

try
{
    using var third = new Resource("third");
    throw new InvalidOperationException("something broke");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"caught: {ex.Message}");
}

await using (var logger = new AsyncLogger())
{
    Console.WriteLine("writing log");
}

Console.WriteLine("end of Main");

class Resource(string name) : IDisposable
{
    private bool _disposed;

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Console.WriteLine($"  [{name}] released");
    }
}

class AsyncLogger : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        await Task.Delay(10);
        Console.WriteLine("  [logger] flushed");
    }
}

// Expected output should be:
// working with first
//   [first] released
// working with second
//   [third] released
// caught: something broke
// writing log
//   [logger] flushed
// end of Main
//   [second] released
