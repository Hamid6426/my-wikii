// Lesson 21: Dates and Times (../21_dates_and_times.md)
// Clock testable time
// Run: java 21-02_clock_testable_time.java

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
