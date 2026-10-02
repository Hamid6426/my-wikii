// Lesson 21: Dates and Times (../21_dates_and_times.md)
// Math with dates
// Run: java 21-01_math_with_dates.java

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
