# 22 - Dates and Times

## The Types

| Type             | Holds                             | Use for                          |
| ---------------- | --------------------------------- | -------------------------------- |
| `DateTime`       | Date and time (no zone info kept) | Simple local times               |
| `DateTimeOffset` | Date, time, and offset from UTC   | Timestamps shared across systems |
| `DateOnly`       | Date only (.NET 6+)               | Birthdays, due dates             |
| `TimeOnly`       | Time of day only (.NET 6+)        | Opening hours, alarms            |
| `TimeSpan`       | A length of time                  | Durations, timeouts              |

---

## Creating and Reading

```csharp
DateTime now = DateTime.Now;          // local time
DateTime utc = DateTime.UtcNow;       // UTC time
DateTime d = new DateTime(2026, 10, 1, 14, 30, 0);

Console.WriteLine(d.Year);            // 2026
Console.WriteLine(d.DayOfWeek);       // Thursday
Console.WriteLine(d.Date);            // time part set to 00:00

var day = new DateOnly(2026, 10, 1);
var time = new TimeOnly(14, 30);
```

---

## Math with Dates

```csharp
DateTime next = d.AddDays(7).AddHours(2);

TimeSpan gap = new DateTime(2026, 12, 25) - d;
Console.WriteLine(gap.TotalDays);

TimeSpan t = TimeSpan.FromMinutes(90);
Console.WriteLine(t);                 // 01:30:00
```

---

## Formatting

```csharp
Console.WriteLine(d.ToString("yyyy-MM-dd"));          // 2026-10-01
Console.WriteLine(d.ToString("dd/MM/yyyy HH:mm"));    // 01/10/2026 14:30
Console.WriteLine(d.ToString("o"));                   // ISO 8601 round trip
```

| Code   | Meaning             |
| ------ | ------------------- |
| `yyyy` | 4-digit year        |
| `MM`   | Month number        |
| `MMM`  | Short month name    |
| `dd`   | Day of month        |
| `HH`   | Hour, 24-hour clock |
| `mm`   | Minutes             |
| `ss`   | Seconds             |

`MM` is month and `mm` is minutes. Mixing them up is the most common mistake.

---

## Parsing

```csharp
DateTime a = DateTime.Parse("2026-10-01");
bool ok = DateTime.TryParse("not a date", out DateTime b);   // false

DateTime c = DateTime.ParseExact("01/10/2026", "dd/MM/yyyy",
    System.Globalization.CultureInfo.InvariantCulture);
```

---

## UTC and Time Zones

```csharp
DateTimeOffset stamp = DateTimeOffset.UtcNow;

TimeZoneInfo pk = TimeZoneInfo.FindSystemTimeZoneById("Asia/Karachi");
DateTime local = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pk);
```

Store and send times in UTC. Convert to local time only when showing them to a person.

---

## Measuring Elapsed Time

```csharp
var sw = System.Diagnostics.Stopwatch.StartNew();
DoWork();
sw.Stop();
Console.WriteLine(sw.ElapsedMilliseconds);
```

Use `Stopwatch`, not `DateTime.Now`, to time code.

---

## TimeProvider

Code that calls `DateTime.UtcNow` directly is hard to test, because the test cannot control the clock. `TimeProvider` (.NET 8+) is a built-in clock you take as a parameter.

```csharp
class Trial(TimeProvider clock, TimeSpan length)
{
    private readonly DateTimeOffset _started = clock.GetUtcNow();

    public bool IsExpired() => clock.GetUtcNow() - _started > length;
}

var real = new Trial(TimeProvider.System, TimeSpan.FromDays(14));   // the real clock
```

In a test, pass a fake clock that you move by hand:

```csharp
class FakeClock(DateTimeOffset now) : TimeProvider
{
    private DateTimeOffset _now = now;

    public override DateTimeOffset GetUtcNow() => _now;
    public void Advance(TimeSpan by) => _now += by;
}

var clock = new FakeClock(new DateTimeOffset(2026, 10, 1, 9, 0, 0, TimeSpan.Zero));
var trial = new Trial(clock, TimeSpan.FromDays(14));

Console.WriteLine(trial.IsExpired());   // False
clock.Advance(TimeSpan.FromDays(15));
Console.WriteLine(trial.IsExpired());   // True
```

`TimeProvider` also measures elapsed time and creates timers, so tests never need to wait:

```csharp
long start = TimeProvider.System.GetTimestamp();
DoWork();
TimeSpan elapsed = TimeProvider.System.GetElapsedTime(start);
```

A ready-made `FakeTimeProvider` exists in the `Microsoft.Extensions.TimeProvider.Testing` package. See [Testing](56_testing.md).

---

## Gotchas

- `DateTime.Kind` can be `Local`, `Utc`, or `Unspecified`, and mixing them gives wrong results
- `DateTime.Now` changes with daylight saving time; `DateTime.UtcNow` does not
- `Parse` without a culture depends on the machine's settings; pass `CultureInfo.InvariantCulture` for fixed formats
- `TimeSpan.Days` is the whole-days part; use `TotalDays` for the full value
- Time zone IDs differ by OS (Windows names vs IANA names), though .NET 6+ converts between them

---

## Examples

- [22-01](examples/22-01_dates_and_times.cs): DateTime, DateOnly, TimeSpan, formatting
- [22-02](examples/22-02_time_provider.cs): TimeProvider and a fake clock for testing

Run one with `dotnet run examples/22-01_dates_and_times.cs`. See [Examples](examples/README.md).
