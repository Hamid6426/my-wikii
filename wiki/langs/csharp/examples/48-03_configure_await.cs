// Lesson 48: Async / Await / Tasks (../48_async_await_tasks.md)
// SynchronizationContext, ConfigureAwait, and the .Result deadlock
// Run: dotnet run 48-03_configure_await.cs

// A console app has no synchronization context, so this example makes one:
// a single dedicated thread that runs every callback, like a UI thread does.
var context = new SingleThreadContext();
SynchronizationContext.SetSynchronizationContext(context);

// 1. Default: the code after 'await' goes back to the captured context
context.Run(async () =>
{
    await Task.Delay(20);
    Console.WriteLine($"After await (default):             on context thread? {Environment.CurrentManagedThreadId == context.ThreadId}");

    // 2. ConfigureAwait(false): continue on any pool thread
    await Task.Delay(20).ConfigureAwait(false);
    Console.WriteLine($"After ConfigureAwait(false):       on context thread? {Environment.CurrentManagedThreadId == context.ThreadId}");
});

// 3. Blocking on a task from the context thread deadlocks when the task needs the context
context.Run(() =>
{
    Task<string> task = LoadAsync();                        // needs the context to finish
    bool finished = task.Wait(TimeSpan.FromMilliseconds(300));
    Console.WriteLine($"Blocking with .Wait(): finished? {finished}   (a deadlock, rescued by the time-out)");

    Task<string> safe = LoadLibraryAsync();                 // uses ConfigureAwait(false) inside
    bool finished2 = safe.Wait(TimeSpan.FromMilliseconds(300));
    Console.WriteLine($"Library with ConfigureAwait(false): finished? {finished2}");
    return Task.CompletedTask;
});

// 4. Await without capturing, in one line (.NET 8+)
await Task.Delay(10).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
Console.WriteLine("Done");

static async Task<string> LoadAsync()
{
    await Task.Delay(20);               // wants to resume on the context, which is blocked
    return "data";
}

static async Task<string> LoadLibraryAsync()
{
    await Task.Delay(20).ConfigureAwait(false);   // does not need the context
    return "data";
}

class SingleThreadContext : SynchronizationContext
{
    private readonly System.Collections.Concurrent.BlockingCollection<(SendOrPostCallback, object?)> _queue = new();
    private readonly Thread _thread;

    public int ThreadId { get; private set; }

    public SingleThreadContext()
    {
        _thread = new Thread(() =>
        {
            ThreadId = Environment.CurrentManagedThreadId;
            SetSynchronizationContext(this);
            foreach (var (callback, state) in _queue.GetConsumingEnumerable()) callback(state);
        }) { IsBackground = true };
        _thread.Start();
        while (ThreadId == 0) Thread.Sleep(1);
    }

    public override void Post(SendOrPostCallback d, object? state) => _queue.Add((d, state));

    // Run work on the context thread and wait for it to finish
    public void Run(Func<Task> work)
    {
        var done = new ManualResetEventSlim();
        Post(async _ =>
        {
            await work();
            done.Set();
        }, null);
        done.Wait();
    }
}

// Expected output should be:
// After await (default):             on context thread? True
// After ConfigureAwait(false):       on context thread? False
// Blocking with .Wait(): finished? False   (a deadlock, rescued by the time-out)
// Library with ConfigureAwait(false): finished? True
// Done
