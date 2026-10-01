# 48 - Async / Await / Tasks

## Why Async

Async programming lets work happen concurrently without blocking the calling thread, especially important for I/O operations (file access, HTTP calls, database queries).

---

## Task and Task\<T\>

`Task` represents an ongoing operation. `Task<T>` is a task that returns a value.

```csharp
Task t = Task.Run(() => Console.WriteLine("Running in background"));
await t;

Task<int> calc = Task.Run(() => 2 + 2);
int result = await calc;   // 4
```

---

## async / await

Mark a method `async` to use `await` inside it.

```csharp
static async Task<string> FetchDataAsync(string url)
{
    using var client = new HttpClient();
    string data = await client.GetStringAsync(url);
    return data;
}
```

```csharp
// Calling it
string result = await FetchDataAsync("https://api.example.com/data");
Console.WriteLine(result);
```

---

## Return Types

| Return type    | Use when                                      |
| -------------- | --------------------------------------------- |
| `Task`         | Async method with no return value             |
| `Task<T>`      | Async method that returns a value             |
| `void`         | Async event handlers only (not awaitable)     |
| `ValueTask<T>` | High-performance hot paths: avoids heap alloc |

```csharp
async Task DoWorkAsync()        { await Task.Delay(100); }
async Task<int> GetValueAsync() { await Task.Delay(100); return 42; }
```

---

## Task.Delay

Non-blocking pause: does not block the thread.

```csharp
await Task.Delay(1000);  // wait 1 second
```

---

## Running Tasks in Parallel

```csharp
Task<int> t1 = GetValueAsync(1);
Task<int> t2 = GetValueAsync(2);
Task<int> t3 = GetValueAsync(3);

int[] results = await Task.WhenAll(t1, t2, t3);
// all three run concurrently
```

Wait for the first to complete:

```csharp
Task<int> first = await Task.WhenAny(t1, t2, t3);
```

---

## CancellationToken

Cooperative cancellation: pass a token; check or await with it.

```csharp
static async Task DoWorkAsync(CancellationToken ct)
{
    for (int i = 0; i < 10; i++)
    {
        ct.ThrowIfCancellationRequested();
        await Task.Delay(200, ct);
        Console.WriteLine(i);
    }
}

var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));  // cancel after 1s
try
{
    await DoWorkAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Cancelled");
}
```

---

## ConfigureAwait

`ConfigureAwait(false)`: tells the runtime not to capture the current synchronization context. Use in library code to avoid deadlocks.

```csharp
string data = await client.GetStringAsync(url).ConfigureAwait(false);
```

---

## async Streams (C# 8+)

`IAsyncEnumerable<T>`: stream items asynchronously.

```csharp
async IAsyncEnumerable<int> GenerateAsync()
{
    for (int i = 0; i < 5; i++)
    {
        await Task.Delay(100);
        yield return i;
    }
}

await foreach (int n in GenerateAsync())
{
    Console.WriteLine(n);
}
```

---

## Exception Handling in Async

```csharp
try
{
    await DoRiskyWorkAsync();
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"HTTP error: {ex.Message}");
}
```

When several tasks fail, `await Task.WhenAll(...)` throws only the **first** exception. All of them are on the `WhenAll` task itself, inside an `AggregateException`:

```csharp
Task all = Task.WhenAll(t1, t2, t3);

try
{
    await all;
}
catch (Exception ex)
{
    // ex is the first exception only
    foreach (Exception inner in all.Exception!.InnerExceptions)
    {
        Console.WriteLine(inner.Message);   // every failure
    }
}
```

Calling `.Wait()` or `.Result` instead throws the `AggregateException` directly, which is one more reason to prefer `await`.

---

## TaskCompletionSource

`TaskCompletionSource<T>` creates a `Task` that you complete by hand. Use it to wrap older callback or event APIs so callers can `await` them, or to wait for a signal from another part of the program.

```csharp
// An old-style API that reports its result through a callback
static void DownloadWithCallback(string url, Action<string?, Exception?> done) { /* ... */ }

static Task<string> DownloadAsync(string url)
{
    var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

    DownloadWithCallback(url, (result, error) =>
    {
        if (error != null) tcs.SetException(error);
        else tcs.SetResult(result!);
    });

    return tcs.Task;
}

Console.WriteLine(await DownloadAsync("site.com/a"));   // content of site.com/a
```

| Method                     | Effect on the task                                               |
| -------------------------- | ---------------------------------------------------------------- |
| `SetResult(value)`         | Finishes with a value                                            |
| `SetException(ex)`         | Finishes as faulted; `await` throws `ex`                         |
| `SetCanceled()`            | Finishes as cancelled                                            |
| `TrySetResult` and friends | Same, but return `false` instead of throwing if already finished |

### Waiting for a Signal

The non-generic form carries no value:

```csharp
var ready = new TaskCompletionSource();

_ = Task.Run(async () =>
{
    await Task.Delay(30);
    ready.SetResult();
});

await ready.Task;   // continues when SetResult() is called
```

Always pass `TaskCreationOptions.RunContinuationsAsynchronously`. Without it, the code after `await` can run inside `SetResult()` on the caller's thread, which can cause deadlocks and surprises.

---

## Synchronization Context

A **synchronization context** decides where code runs after an `await`. UI apps (WinForms, WPF, MAUI) have one that sends work back to the UI thread. A console app and ASP.NET Core have none, so code after `await` continues on any pool thread.

By default `await` captures the current context and returns to it. That is what lets a UI app update a label after `await LoadAsync()`.

`ConfigureAwait(false)` says "I do not need the original context". The rest of the method then continues on a pool thread.

```csharp
// Inside code that runs on a context thread (for example a UI thread)
await Task.Delay(20);
// continues on the context thread

await Task.Delay(20).ConfigureAwait(false);
// continues on a pool thread, and the context is not involved
```

| Where your code runs          | `ConfigureAwait(false)`                                             |
| ----------------------------- | ------------------------------------------------------------------- |
| Library code                  | Use it on every `await`. A library cannot know its caller's context |
| UI event handlers             | Do not use it where the next line touches the UI                    |
| ASP.NET Core and console apps | No effect, because there is no context. Optional                    |

Run the example `examples/48-03_configure_await.cs` to see it. It builds a one-thread context like a UI thread and shows where each continuation runs.

---

## The .Result and .Wait() Deadlock

Blocking on a task from a thread that owns a context can freeze the program:

1. The context thread calls `task.Wait()` and blocks, waiting for the task
2. The task finishes its work and wants to continue on the context thread
3. That thread is blocked, so the continuation never runs, and the task never finishes

```csharp
// Called on a context thread
Task<string> task = LoadAsync();
task.Wait();                        // never returns: a deadlock
```

Ways out:

- Do not block. `await` the task instead. This is the real fix
- In a library, use `ConfigureAwait(false)` inside, so the library does not need the blocked thread
- If you must block in a console app, there is no context, so it works, but it still wastes a thread

```csharp
static async Task<string> LoadLibraryAsync()
{
    await Task.Delay(20).ConfigureAwait(false);   // does not need the context
    return "data";
}
```

---

## ConfigureAwaitOptions (.NET 8+)

`ConfigureAwait` also takes a set of options:

| Option                      | Meaning                                                             |
| --------------------------- | ------------------------------------------------------------------- |
| `None`                      | Do not capture the context. Same as `ConfigureAwait(false)`         |
| `ContinueOnCapturedContext` | Capture the context. Same as `ConfigureAwait(true)`                 |
| `SuppressThrowing`          | Do not throw if the task failed. Only for `Task`, not `Task<T>`     |
| `ForceYielding`             | Always return to the caller first, even if the task is already done |

```csharp
await Task.Delay(10).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
```

`SuppressThrowing` is useful for cleanup, where you want to wait for a task to finish but ignore its failure. You lose the exception, so log it first if it matters.

---

## Gotchas

- `async void` cannot be awaited and swallows exceptions: only use for event handlers
- Do not use `.Result` or `.Wait()` on tasks in async code: causes deadlocks in UI/ASP.NET contexts
- `await Task.WhenAll` runs tasks concurrently; `await t1; await t2;` runs them sequentially
- Not every slow operation benefits from `async`: CPU-bound work belongs in `Task.Run`, not bare `async`
- Always pass `CancellationToken` in library methods to allow callers to cancel long operations

---

## Examples

- [48-01](examples/48-01_async_await.cs): async, await, Task.WhenAll, cancellation
- [48-02](examples/48-02_task_completion_source.cs): TaskCompletionSource: turn a callback into a Task
- [48-03](examples/48-03_configure_await.cs): SynchronizationContext, ConfigureAwait, and the .Result deadlock

Run one with `dotnet run examples/48-01_async_await.cs`. See [Examples](examples/README.md).
