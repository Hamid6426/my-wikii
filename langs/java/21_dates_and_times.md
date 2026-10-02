# 21 - Dates and Times

## The Types

All of these live in `java.time` (Java 8+). Every one is immutable: methods return a new value.

| Type             | Holds                                 | Use for                          |
| ---------------- | ------------------------------------- | -------------------------------- |
| `LocalDate`      | Date only                             | Birthdays, due dates             |
| `LocalTime`      | Time of day only                      | Opening hours, alarms            |
| `LocalDateTime`  | Date and time, no zone                | A meeting time "on the calendar" |
| `ZonedDateTime`  | Date, time, and a time zone           | Showing times to people          |
| `OffsetDateTime` | Date, time, and an offset from UTC    | Database and API timestamps      |
| `Instant`        | A point on the UTC timeline           | Logs, storing "when it happened" |
| `Duration`       | A length of time in seconds and nanos | Timeouts, elapsed time           |
| `Period`         | A length in years, months, and days   | "2 months from now", ages        |

"Local" means "no time zone attached", not "my computer's zone".

The old `java.util.Date` and `Calendar` are changeable and confusing. Do not use them in new code.

---

## Creating and Reading

```java
LocalDate today = LocalDate.now();
LocalDateTime now = LocalDateTime.now();
Instant stamp = Instant.now();                  // UTC

LocalDate d = LocalDate.of(2026, 10, 1);
LocalTime t = LocalTime.of(14, 30);
LocalDateTime dt = LocalDateTime.of(d, t);

System.out.println(d.getYear());        // 2026
System.out.println(d.getMonth());       // OCTOBER
System.out.println(d.getDayOfWeek());   // THURSDAY
System.out.println(dt);                 // 2026-10-01T14:30
```

Months are numbered 1 to 12. You can also write `Month.OCTOBER`.

---

## Math with Dates

```java
import java.time.*;
import java.time.temporal.ChronoUnit;

public class DateMath {
    public static void main(String[] args) {
        LocalDate start = LocalDate.of(2026, 10, 1);

        LocalDate next = start.plusDays(7).plusMonths(1);
        System.out.println(next);

        LocalDate christmas = LocalDate.of(2026, 12, 25);
        System.out.println(ChronoUnit.DAYS.between(start, christmas));

        Period gap = Period.between(start, christmas);
        System.out.println(gap.getMonths() + " months " + gap.getDays() + " days");

        Duration d = Duration.ofMinutes(90);
        System.out.println(d);
        System.out.println(d.toHours() + "h " + d.toMinutesPart() + "m");

        System.out.println(LocalDate.of(2026, 1, 31).plusMonths(1));
        System.out.println(start.isBefore(christmas));
    }
}
```

Expected output should be:

```
2026-11-08
85
2 months 24 days
PT1H30M
1h 30m
2026-02-28
true
```

`PT1H30M` is the ISO 8601 way to write "1 hour 30 minutes". Adding a month to January 31 gives the last valid day, February 28.

---

## Formatting

`toString()` gives ISO format (`2026-10-01`). For other formats, use a `DateTimeFormatter`.

```java
LocalDateTime dt = LocalDateTime.of(2026, 10, 1, 14, 30);

DateTimeFormatter f = DateTimeFormatter.ofPattern("dd/MM/yyyy HH:mm");
System.out.println(dt.format(f));          // 01/10/2026 14:30

DateTimeFormatter longer = DateTimeFormatter.ofPattern("EEEE d MMMM yyyy", Locale.ENGLISH);
System.out.println(dt.format(longer));     // Thursday 1 October 2026
```

| Code   | Meaning             |
| ------ | ------------------- |
| `yyyy` | 4-digit year        |
| `MM`   | Month number        |
| `MMM`  | Short month name    |
| `dd`   | Day of month        |
| `EEEE` | Day name            |
| `HH`   | Hour, 24-hour clock |
| `hh`   | Hour, 12-hour clock |
| `a`    | AM or PM            |
| `mm`   | Minutes             |
| `ss`   | Seconds             |

`MM` is month and `mm` is minutes. Mixing them up is the most common mistake.

---

## Parsing

```java
LocalDate a = LocalDate.parse("2026-10-01");     // ISO format needs no pattern

LocalDate b = LocalDate.parse("01/10/2026",
        DateTimeFormatter.ofPattern("dd/MM/yyyy"));

try {
    LocalDate.parse("not a date");
} catch (DateTimeParseException e) {
    System.out.println("bad date");
}
```

There is no `TryParse`. Catch `DateTimeParseException`.

---

## UTC and Time Zones

A `ZoneId` names a zone such as `Asia/Karachi`. It knows the daylight saving rules.

```java
Instant stamp = Instant.parse("2026-10-01T09:00:00Z");   // Z means UTC

ZonedDateTime karachi = stamp.atZone(ZoneId.of("Asia/Karachi"));
ZonedDateTime london = stamp.atZone(ZoneId.of("Europe/London"));

System.out.println(karachi);   // 2026-10-01T14:00+05:00[Asia/Karachi]
System.out.println(london);    // 2026-10-01T10:00+01:00[Europe/London]
```

Store and send times as `Instant` (UTC). Convert to a zone only when showing them to a person.

---

## Measuring Elapsed Time

```java
long start = System.nanoTime();
doWork();
long ms = (System.nanoTime() - start) / 1_000_000;
System.out.println(ms + " ms");
```

Use `System.nanoTime()`, not `Instant.now()`, to time code. The wall clock can jump when the system adjusts it.

---

## Clock: Testable Time

Code that calls `LocalDate.now()` directly is hard to test, because the test cannot control the clock. Take a `java.time.Clock` instead, and pass it to `now(clock)`.

```java
import java.time.*;

public class TrialDemo {
    record Trial(Clock clock, Instant started, Duration length) {
        Trial(Clock clock, Duration length) {
            this(clock, clock.instant(), length);
        }

        boolean isExpired() {
            return Duration.between(started, clock.instant()).compareTo(length) > 0;
        }
    }

    public static void main(String[] args) {
        Instant start = Instant.parse("2026-10-01T09:00:00Z");
        Clock fixed = Clock.fixed(start, ZoneOffset.UTC);
        Trial trial = new Trial(fixed, Duration.ofDays(14));
        System.out.println(trial.isExpired());

        Clock later = Clock.offset(fixed, Duration.ofDays(15));
        Trial moved = new Trial(later, trial.started(), trial.length());
        System.out.println(moved.isExpired());

        System.out.println(LocalDate.now(fixed));
    }
}
```

Expected output should be:

```
false
true
2026-10-01
```

| Clock                           | Use                                   |
| ------------------------------- | ------------------------------------- |
| `Clock.systemUTC()`             | The real clock, in production         |
| `Clock.fixed(instant, zone)`    | A clock that never moves, for tests   |
| `Clock.offset(clock, duration)` | Another clock shifted forward or back |

See [Testing](59_testing.md).

---

## Gotchas

- `LocalDateTime` has no zone, so it cannot say which moment it is; use `Instant` or `ZonedDateTime` for real timestamps
- Every type is immutable: `date.plusDays(1)` alone does nothing, write `date = date.plusDays(1)`
- `Period.getDays()` is only the days part; use `ChronoUnit.DAYS.between` for the total
- Pattern letters are case-sensitive: `MM` month, `mm` minute, `yyyy` year, `YYYY` week-based year (wrong near New Year)
- Day and month names depend on the default locale; pass `Locale.ENGLISH` for fixed output
- Use IANA zone IDs like `Europe/London`, not short codes like `PST`, which are ambiguous

---

## Examples

- [21-01](examples/21-01_math_with_dates.java): Math with dates
- [21-02](examples/21-02_clock_testable_time.java): Clock testable time

Run one with `java examples/21-01_math_with_dates.java`. See [Examples](examples/README.md).
