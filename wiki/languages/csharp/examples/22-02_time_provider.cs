// Lesson 22: Dates and Times (../22_dates_and_times.md)
// TimeProvider and a fake clock for testing
// Run: dotnet run 22-02_time_provider.cs

// Code that reads the clock directly is hard to test.
// Take a TimeProvider instead, and pass a fake one in tests.

var clock = new FakeClock(new DateTimeOffset(2026, 10, 1, 9, 0, 0, TimeSpan.Zero));
var trial = new Trial(clock, TimeSpan.FromDays(14));

Console.WriteLine($"Day 0 expired? {trial.IsExpired()}");
clock.Advance(TimeSpan.FromDays(13));
Console.WriteLine($"Day 13 expired? {trial.IsExpired()}");
clock.Advance(TimeSpan.FromDays(2));
Console.WriteLine($"Day 15 expired? {trial.IsExpired()}");

// The real clock
var real = new Trial(TimeProvider.System, TimeSpan.FromDays(14));
Console.WriteLine($"Real clock, new trial expired? {real.IsExpired()}");

// Measuring elapsed time with the provider
long start = TimeProvider.System.GetTimestamp();
Thread.Sleep(20);
Console.WriteLine($"Elapsed 20 ms or more? {TimeProvider.System.GetElapsedTime(start).TotalMilliseconds >= 20}");

class Trial(TimeProvider clock, TimeSpan length)
{
    private readonly DateTimeOffset _started = clock.GetUtcNow();

    public bool IsExpired() => clock.GetUtcNow() - _started > length;
}

class FakeClock(DateTimeOffset now) : TimeProvider
{
    private DateTimeOffset _now = now;

    public override DateTimeOffset GetUtcNow() => _now;
    public void Advance(TimeSpan by) => _now += by;
}

// Expected output should be:
// Day 0 expired? False
// Day 13 expired? False
// Day 15 expired? True
// Real clock, new trial expired? False
// Elapsed 20 ms or more? True
