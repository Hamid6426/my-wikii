// Lesson 22: Dates and Times (../22_dates_and_times.md)
// DateTime, DateOnly, TimeSpan, formatting
// Run: dotnet run 22-01_dates_and_times.cs

using System.Globalization;

var date = new DateTime(2026, 10, 1, 14, 30, 0);
Console.WriteLine(date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
Console.WriteLine(date.DayOfWeek);
Console.WriteLine(date.AddDays(45).ToString("dddd, d MMMM yyyy", CultureInfo.InvariantCulture));

var christmas = new DateTime(2026, 12, 25);
TimeSpan left = christmas - date;
Console.WriteLine($"{left.Days} days and {left.Hours} hours until Christmas");

var birthday = new DateOnly(1990, 5, 17);
var today = new DateOnly(2026, 10, 1);
int age = today.Year - birthday.Year - (today < birthday.AddYears(today.Year - birthday.Year) ? 1 : 0);
Console.WriteLine($"Age on {today:yyyy-MM-dd}: {age}");

Console.WriteLine(TimeSpan.FromMinutes(135));
Console.WriteLine(DateTime.ParseExact("01/10/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("o"));
Console.WriteLine(DateTime.TryParse("not a date", out _));

var utc = new DateTimeOffset(2026, 10, 1, 12, 0, 0, TimeSpan.Zero);
Console.WriteLine(utc.ToOffset(TimeSpan.FromHours(5)).ToString("yyyy-MM-dd HH:mm zzz", CultureInfo.InvariantCulture));

// Expected output should be:
// 2026-10-01 14:30
// Thursday
// Sunday, 15 November 2026
// 84 days and 9 hours until Christmas
// Age on 2026-10-01: 36
// 02:15:00
// 2026-10-01T00:00:00.0000000
// False
// 2026-10-01 17:00 +05:00
