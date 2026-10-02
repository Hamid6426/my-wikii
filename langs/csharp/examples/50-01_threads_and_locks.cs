// Lesson 50: Threading and Synchronization (../50_threading.md)
// Race conditions, lock, Interlocked, SemaphoreSlim
// Run: dotnet run 50-01_threads_and_locks.cs

int unsafeCounter = 0;
Parallel.For(0, 100_000, _ => unsafeCounter++);
Console.WriteLine($"Without protection: {(unsafeCounter == 100_000 ? "got lucky" : "lost updates")}");

int lockedCounter = 0;
var gate = new Lock();
Parallel.For(0, 100_000, _ =>
{
    lock (gate) { lockedCounter++; }
});
Console.WriteLine($"With lock: {lockedCounter}");

int atomicCounter = 0;
Parallel.For(0, 100_000, _ => Interlocked.Increment(ref atomicCounter));
Console.WriteLine($"With Interlocked: {atomicCounter}");

var limiter = new SemaphoreSlim(2);
int running = 0, maxRunning = 0;

async Task Work(int id)
{
    await limiter.WaitAsync();
    try
    {
        int now = Interlocked.Increment(ref running);
        InterlockedMax(ref maxRunning, now);
        await Task.Delay(50);
        Interlocked.Decrement(ref running);
    }
    finally
    {
        limiter.Release();
    }
}

await Task.WhenAll(Enumerable.Range(1, 6).Select(Work));
Console.WriteLine($"Most tasks running at once: {maxRunning}");

var words = new System.Collections.Concurrent.ConcurrentDictionary<string, int>();
Parallel.ForEach(new[] { "a", "b", "a", "c", "a", "b" }, w => words.AddOrUpdate(w, 1, (_, old) => old + 1));
Console.WriteLine(string.Join(", ", words.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key}={kv.Value}")));

static void InterlockedMax(ref int target, int value)
{
    int current;
    while (value > (current = Volatile.Read(ref target)))
    {
        if (Interlocked.CompareExchange(ref target, value, current) == current) break;
    }
}

// Expected output should be:
// Without protection: lost updates
// With lock: 100000
// With Interlocked: 100000
// Most tasks running at once: 2
// a=3, b=2, c=1
//
// The first line can differ between runs: lost updates depend on timing.
