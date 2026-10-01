# 50 - Threading and Synchronization

## Threads vs Tasks

| Tool     | What it is                                  | Use for                              |
| -------- | ------------------------------------------- | ------------------------------------ |
| `Thread` | One OS thread you start and manage yourself | Rare: long-running dedicated work    |
| `Task`   | A unit of work run on the thread pool       | Almost everything (see async lesson) |

This lesson covers what goes wrong when several threads touch the same data, and how to fix it.

---

## Creating a Thread

```csharp
var t = new Thread(() => Console.WriteLine("Hello from another thread"));
t.Start();
t.Join();   // wait for it to finish
```

---

## The Problem: Race Conditions

```csharp
int counter = 0;

Parallel.For(0, 100_000, _ => counter++);

Console.WriteLine(counter);   // less than 100000, and different each run
```

`counter++` is three steps (read, add, write). Two threads can read the same value and one update gets lost.

---

## lock

Only one thread at a time can run the block.

```csharp
private readonly object _gate = new();
private int _counter;

public void Increment()
{
    lock (_gate)
    {
        _counter++;
    }
}
```

In .NET 9+ use the dedicated `Lock` type:

```csharp
private readonly Lock _gate = new();
```

---

## Interlocked

Fast, lock-free operations for simple numbers.

```csharp
int counter = 0;
Parallel.For(0, 100_000, _ => Interlocked.Increment(ref counter));

Console.WriteLine(counter);   // 100000
```

---

## SemaphoreSlim

Limit how many threads or tasks run at once. It also works with `await`.

```csharp
var limit = new SemaphoreSlim(3);   // 3 at a time

async Task DownloadAsync(string url)
{
    await limit.WaitAsync();
    try
    {
        await client.GetStringAsync(url);
    }
    finally
    {
        limit.Release();
    }
}
```

`lock` cannot contain an `await`; use `SemaphoreSlim(1, 1)` instead.

---

## Thread-Safe Collections

```csharp
using System.Collections.Concurrent;

var dict = new ConcurrentDictionary<string, int>();
dict.AddOrUpdate("a", 1, (key, old) => old + 1);

var queue = new ConcurrentQueue<int>();
queue.Enqueue(1);
queue.TryDequeue(out int item);
```

---

## Parallel Loops and PLINQ

```csharp
Parallel.ForEach(files, file => Process(file));

await Parallel.ForEachAsync(urls, async (url, ct) =>
{
    await client.GetStringAsync(url, ct);
});

var squares = Enumerable.Range(1, 1000).AsParallel().Select(n => n * n).ToList();
```

Use them for CPU-heavy work on many items. They do not help with waiting on a network.

---

## Channels

A thread-safe pipe between a producer and a consumer.

```csharp
using System.Threading.Channels;

var channel = Channel.CreateUnbounded<int>();

_ = Task.Run(async () =>
{
    for (int i = 0; i < 3; i++) await channel.Writer.WriteAsync(i);
    channel.Writer.Complete();
});

await foreach (int n in channel.Reader.ReadAllAsync())
    Console.WriteLine(n);
```

---

## volatile

Threads may keep a variable in a CPU register or cache. A loop that waits for another thread to change a flag can then keep reading the old value forever. A `volatile` field is always read from and written to memory.

```csharp
class Worker
{
    private volatile bool _stop;
    public long Loops;

    public void Run()
    {
        while (!_stop) Loops++;
    }

    public void Stop() => _stop = true;
}

var worker = new Worker();
var thread = new Thread(worker.Run);
thread.Start();
Thread.Sleep(50);
worker.Stop();      // the loop sees this and ends
thread.Join();
```

`volatile` makes one read or write safe to see. It does not make `x++` atomic. For counters use `Interlocked`, and for several steps that must happen together use `lock`.

---

## Timers

### PeriodicTimer

An awaitable timer for loops. It fits async code and never overlaps ticks.

```csharp
using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(40));
int ticks = 0;

while (await timer.WaitForNextTickAsync())
{
    Console.WriteLine($"tick {++ticks}");
    if (ticks == 3) break;
}
```

Pass a `CancellationToken` to `WaitForNextTickAsync` to stop the loop from outside. If your work takes longer than the period, ticks are skipped, not queued.

### System.Threading.Timer

Calls a method on a pool thread at an interval.

```csharp
int fired = 0;
using var done = new ManualResetEventSlim();

using var timer = new Timer(_ =>
{
    if (Interlocked.Increment(ref fired) == 3) done.Set();
}, state: null, dueTime: 10, period: 20);   // first call after 10 ms, then every 20 ms

done.Wait();
```

| Timer                    | Use for                                             |
| ------------------------ | --------------------------------------------------- |
| `PeriodicTimer`          | An async loop that does work every so often         |
| `System.Threading.Timer` | A simple callback with no UI. Callbacks can overlap |
| `Task.Delay`             | Wait once                                           |
| `System.Timers.Timer`    | Older event-based timer. Prefer the two above       |

---

## Bounded Channels

An unbounded channel (shown earlier) lets a fast producer fill memory. A bounded channel makes the producer wait when the queue is full.

```csharp
using System.Threading.Channels;

var channel = Channel.CreateBounded<int>(new BoundedChannelOptions(2)
{
    FullMode = BoundedChannelFullMode.Wait
});

var producer = Task.Run(async () =>
{
    for (int i = 1; i <= 6; i++)
    {
        await channel.Writer.WriteAsync(i);   // waits while 2 items are already queued
    }
    channel.Writer.Complete();
});

int sum = 0;
await foreach (int item in channel.Reader.ReadAllAsync())
{
    await Task.Delay(5);                      // a slow consumer
    sum += item;
}
await producer;
Console.WriteLine(sum);   // 21
```

| `FullMode`   | When the queue is full             |
| ------------ | ---------------------------------- |
| `Wait`       | The producer waits (back pressure) |
| `DropOldest` | The oldest item is discarded       |
| `DropNewest` | The new item is discarded          |
| `DropWrite`  | The write is ignored               |

---

## The Thread Pool

Creating a thread is expensive. The **thread pool** is a shared set of ready threads that run short pieces of work. `Task.Run`, `Parallel.For`, timer callbacks, and the code after most `await`s all run on it.

```csharp
ThreadPool.QueueUserWorkItem(_ =>
{
    Console.WriteLine(Thread.CurrentThread.IsThreadPoolThread);   // True
});

bool onPool = await Task.Run(() => Thread.CurrentThread.IsThreadPoolThread);   // True

var own = new Thread(() => Console.WriteLine(Thread.CurrentThread.IsThreadPoolThread));   // False
own.Start();
```

| Use                                                 | Choose                                                   |
| --------------------------------------------------- | -------------------------------------------------------- |
| Short work and async code                           | The pool (`Task.Run`, `await`)                           |
| Work that runs for minutes or for the whole program | Your own `Thread` (or `TaskCreationOptions.LongRunning`) |
| A thread you must name, prioritize, or stop         | Your own `Thread`                                        |

The pool starts with about one thread per CPU core and adds more slowly when all are busy.

### Thread Pool Starvation

If pool threads are blocked, new work waits in the queue even though the machine is idle. The pool adds a thread only about every half second.

```csharp
// Bad: each task holds a pool thread while it only waits
var tasks = Enumerable.Range(0, 100).Select(_ => Task.Run(() => Thread.Sleep(5000)));

// Good: the thread is released while waiting
var better = Enumerable.Range(0, 100).Select(_ => Task.Run(async () => await Task.Delay(5000)));
```

Rules to avoid starvation:

- Never block a pool thread with `Thread.Sleep`, `.Result`, or `.Wait()`. Use `await`
- Use `Task.Delay` and async I/O for waiting
- Give long-running work its own `Thread`

You can read the limits with `ThreadPool.GetMinThreads` and `GetMaxThreads`. Raising the minimum with `SetMinThreads` is a last resort, not a fix.

---

## Mutex

A `Mutex` is a lock that works **across processes**. A `lock` statement protects code inside one program. A named `Mutex` can protect something shared between programs, such as a file, or make sure only one copy of your program runs.

```csharp
const string name = @"Global\my-unique-app-name";

using var mutex = new Mutex(initiallyOwned: true, name, out bool createdNew);

if (!createdNew)
{
    Console.WriteLine("Another copy is already running");
    return;
}

// ... the program runs. The first copy owns the mutex.

mutex.ReleaseMutex();
```

Take and release it around a short critical section:

```csharp
using var mutex = new Mutex(initiallyOwned: false, "shared-file-lock");

if (mutex.WaitOne(TimeSpan.FromSeconds(2)))    // wait up to 2 seconds
{
    try
    {
        // work with the shared resource
    }
    finally
    {
        mutex.ReleaseMutex();                  // always release on the same thread
    }
}
else
{
    Console.WriteLine("Could not get the lock in time");
}
```

| Tool            | Protects across        | Cost   | Use for                                   |
| --------------- | ---------------------- | ------ | ----------------------------------------- |
| `lock`          | Threads in one program | Low    | Almost all shared data                    |
| `SemaphoreSlim` | Threads, and `await`   | Low    | Limiting how many run at once             |
| `Mutex`         | Processes              | Higher | One running copy, or a cross-program lock |

A `Mutex` belongs to the **thread** that took it, and only that thread may release it. Code after an `await` can resume on a different thread, so do not hold a `Mutex` across an `await`. Releasing from another thread throws `ApplicationException`. Use `SemaphoreSlim` when you need to wait asynchronously.

### Name Scope

A mutex name has a scope:

| Name            | Shared by                           |
| --------------- | ----------------------------------- |
| `my-app`        | Processes in the same login session |
| `Local\my-app`  | Same as above, written out          |
| `Global\my-app` | Every session on the machine        |

This was tested on Linux: two copies started from different sessions (with `setsid`) both reported they created a plain-named mutex, so neither saw the other. With the `Global\` prefix the second copy saw it and got `createdNew` as `false`.

For an "only one copy" check, use a `Global\` name, as in the first example above.

---

## Gotchas

- Never lock on `this`, a string, or a `Type`; lock on a private `readonly` object
- Taking two locks in different orders in different places causes a deadlock
- Keep the work inside a `lock` short
- Reads and writes of shared data both need protection, not only writes
- `Thread.Sleep` blocks a thread; in async code use `await Task.Delay`
- Prefer immutable data and message passing over shared mutable state

---

## Examples

- [50-01](examples/50-01_threads_and_locks.cs): Race conditions, lock, Interlocked, SemaphoreSlim
- [50-02](examples/50-02_timers_volatile_channels.cs): PeriodicTimer, Timer, volatile, and bounded Channel
- [50-03](examples/50-03_mutex_and_threadpool.cs): ThreadPool, Mutex, and why not to mix Mutex with await

Run one with `dotnet run examples/50-01_threads_and_locks.cs`. See [Examples](examples/README.md).
