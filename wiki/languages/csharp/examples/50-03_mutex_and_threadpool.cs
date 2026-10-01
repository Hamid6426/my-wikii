// Lesson 50: Threading and Synchronization (../50_threading.md)
// ThreadPool, Mutex, and why not to mix Mutex with await
// Run: dotnet run 50-03_mutex_and_threadpool.cs

// 1. The thread pool: a shared set of ready-made threads
ThreadPool.GetMinThreads(out int minWorkers, out _);
ThreadPool.GetMaxThreads(out int maxWorkers, out _);
Console.WriteLine($"The pool can grow beyond its minimum: {maxWorkers > minWorkers}");

using var done = new CountdownEvent(3);
for (int i = 1; i <= 3; i++)
{
    int id = i;
    ThreadPool.QueueUserWorkItem(_ =>
    {
        Thread.Sleep(10 * id);
        done.Signal();
    });
}
done.Wait();
Console.WriteLine("3 work items finished on pool threads");

bool onPool = await Task.Run(() => Thread.CurrentThread.IsThreadPoolThread);
Console.WriteLine($"Task.Run runs on the pool: {onPool}");

var own = new Thread(() => Console.WriteLine($"A Thread you create is a pool thread: {Thread.CurrentThread.IsThreadPoolThread}"));
own.Start();
own.Join();

// 2. Mutex: a lock that works across threads and across processes
//    A mutex belongs to the thread that took it. Use it from plain threads, not around await.
var mutexDemo = new Thread(MutexDemo);
mutexDemo.Start();
mutexDemo.Join();

static void MutexDemo()
{
    const string name = "csharp-example-50-03";

    using var first = new Mutex(initiallyOwned: true, name, out bool createdNew);
    Console.WriteLine($"First copy created the mutex: {createdNew}  (a second program copy would see false and exit)");

    var other = new Thread(() =>
    {
        using var again = new Mutex(initiallyOwned: false, name);
        bool got = again.WaitOne(TimeSpan.FromMilliseconds(100));
        Console.WriteLine($"Another thread took it while it is held: {got}");
    });
    other.Start();
    other.Join();

    first.ReleaseMutex();                       // release on the same thread that took it

    var other2 = new Thread(() =>
    {
        using var again = new Mutex(initiallyOwned: false, name);
        bool got = again.WaitOne(TimeSpan.FromMilliseconds(100));
        Console.WriteLine($"Another thread took it after the release: {got}");
        if (got) again.ReleaseMutex();
    });
    other2.Start();
    other2.Join();
}

// Expected output should be:
// The pool can grow beyond its minimum: True
// 3 work items finished on pool threads
// Task.Run runs on the pool: True
// A Thread you create is a pool thread: False
// First copy created the mutex: True  (a second program copy would see false and exit)
// Another thread took it while it is held: False
// Another thread took it after the release: True
