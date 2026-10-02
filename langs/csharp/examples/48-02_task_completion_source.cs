// Lesson 48: Async / Await / Tasks (../48_async_await_tasks.md)
// TaskCompletionSource: turn a callback into a Task
// Run: dotnet run 48-02_task_completion_source.cs

// An old-style API that reports its result through a callback
static void DownloadWithCallback(string url, Action<string?, Exception?> done)
{
    var timer = new Timer(_ =>
    {
        if (url.Contains("bad")) done(null, new InvalidOperationException("Server refused"));
        else done($"content of {url}", null);
    }, null, 50, Timeout.Infinite);
    GC.KeepAlive(timer);
}

// Wrap it so callers can use await
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

Console.WriteLine(await DownloadAsync("site.com/a"));

try
{
    await DownloadAsync("site.com/bad");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}

// A task you complete yourself: wait for a signal
var ready = new TaskCompletionSource();
_ = Task.Run(async () =>
{
    await Task.Delay(30);
    ready.SetResult();
});

Console.WriteLine("Waiting for the signal...");
await ready.Task;
Console.WriteLine("Signal received");

// Expected output should be:
// content of site.com/a
// Caught: Server refused
// Waiting for the signal...
// Signal received
