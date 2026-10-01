// Lesson 48: Async / Await / Tasks (../48_async_await_tasks.md)
// async, await, Task.WhenAll, cancellation
// Run: dotnet run 48-01_async_await.cs

using System.Diagnostics;

var sw = Stopwatch.StartNew();

string a = await FetchAsync("A", 300);
string b = await FetchAsync("B", 300);
long sequentialMs = sw.ElapsedMilliseconds;
Console.WriteLine($"Sequential: {a} {b} (two waits of 300 ms, one after the other)");

sw.Restart();
string[] results = await Task.WhenAll(FetchAsync("A", 300), FetchAsync("B", 300), FetchAsync("C", 300));
long parallelMs = sw.ElapsedMilliseconds;
Console.WriteLine($"Parallel: {string.Join(" ", results)} (three waits at the same time)");
Console.WriteLine($"Parallel was faster, even with more work: {parallelMs < sequentialMs}");

using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
try
{
    await FetchAsync("slow", 2000, cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Cancelled after the timeout");
}

try
{
    await FailAsync();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Caught from await: {ex.Message}");
}

int length = await Task.Run(() => string.Concat(Enumerable.Repeat("ab", 1000)).Length);
Console.WriteLine($"Computed on a pool thread: {length}");

static async Task<string> FetchAsync(string name, int delayMs, CancellationToken token = default)
{
    await Task.Delay(delayMs, token);
    return $"{name}-done";
}

static async Task FailAsync()
{
    await Task.Delay(10);
    throw new InvalidOperationException("async failure");
}

// Expected output should be:
// Sequential: A-done B-done (two waits of 300 ms, one after the other)
// Parallel: A-done B-done C-done (three waits at the same time)
// Parallel was faster, even with more work: True
// Cancelled after the timeout
// Caught from await: async failure
// Computed on a pool thread: 2000
