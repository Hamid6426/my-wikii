// Lesson 50: Threading and Synchronization (../50_threading.md)
// PeriodicTimer, Timer, volatile, and bounded Channel
// Run: dotnet run 50-02_timers_volatile_channels.cs

using System.Threading.Channels;

// PeriodicTimer: an awaitable timer for loops
using var periodic = new PeriodicTimer(TimeSpan.FromMilliseconds(40));
int ticks = 0;
while (await periodic.WaitForNextTickAsync())
{
    Console.WriteLine($"tick {++ticks}");
    if (ticks == 3) break;
}

// System.Threading.Timer: runs a callback on a pool thread
int fired = 0;
using var done = new ManualResetEventSlim();
using var timer = new Timer(_ =>
{
    if (Interlocked.Increment(ref fired) == 3) done.Set();
}, null, dueTime: 10, period: 20);
done.Wait();
Console.WriteLine($"Timer callback ran {fired} times");

// volatile: one thread must see another thread's change to a flag
var worker = new Worker();
var thread = new Thread(worker.Run);
thread.Start();
Thread.Sleep(50);
worker.Stop();
thread.Join();
Console.WriteLine($"Worker stopped after doing work: {worker.Loops > 0}");

// Bounded channel: the producer waits when the queue is full
var channel = Channel.CreateBounded<int>(new BoundedChannelOptions(2) { FullMode = BoundedChannelFullMode.Wait });

var producer = Task.Run(async () =>
{
    for (int i = 1; i <= 6; i++)
    {
        await channel.Writer.WriteAsync(i);          // waits while 2 items are already queued
    }
    channel.Writer.Complete();
});

int sum = 0;
await foreach (int item in channel.Reader.ReadAllAsync())
{
    await Task.Delay(5);                             // a slow consumer
    sum += item;
}
await producer;
Console.WriteLine($"Consumer received everything, sum = {sum}");

class Worker
{
    private volatile bool _stop;          // volatile: reads and writes are not cached by one thread
    public long Loops;

    public void Run()
    {
        while (!_stop) Loops++;
    }

    public void Stop() => _stop = true;
}

// Expected output should be:
// tick 1
// tick 2
// tick 3
// Timer callback ran 3 times
// Worker stopped after doing work: True
// Consumer received everything, sum = 21
