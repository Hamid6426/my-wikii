// Lesson 27: Static and Partial Classes (../27_static_and_partial_classes.md)
// Static classes, static members, and partial classes
// Run: dotnet run 27-01_static_and_partial.cs

Console.WriteLine(TemperatureConverter.CelsiusToFahrenheit(100));
Console.WriteLine(TemperatureConverter.FahrenheitToCelsius(98.6));

var a = new Ticket();
var b = new Ticket();
var c = new Ticket();
Console.WriteLine($"Ticket numbers: {a.Number}, {b.Number}, {c.Number}");
Console.WriteLine($"Issued: {Ticket.Issued}");

var user = new User { First = "Ada", Last = "Lovelace" };
Console.WriteLine(user.FullName());
Console.WriteLine(user.IsComplete());

static class TemperatureConverter
{
    public static double CelsiusToFahrenheit(double c) => c * 9 / 5 + 32;
    public static double FahrenheitToCelsius(double f) => Math.Round((f - 32) * 5 / 9, 1);
}

class Ticket
{
    public static int Issued { get; private set; }
    public int Number { get; } = ++Issued;
}

partial class User
{
    public string First { get; set; } = "";
    public string Last { get; set; } = "";
}

partial class User
{
    public string FullName() => $"{First} {Last}";
    public bool IsComplete() => First.Length > 0 && Last.Length > 0;
}

// Expected output should be:
// 212
// 37
// Ticket numbers: 1, 2, 3
// Issued: 3
// Ada Lovelace
// True
