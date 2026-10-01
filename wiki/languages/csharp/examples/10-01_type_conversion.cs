// Lesson 10: Type Conversion (../10_type_conversion.md)
// Implicit, explicit, Convert, Parse, TryParse
// Run: dotnet run 10-01_type_conversion.cs

int small = 42;
long big = small;               // implicit: no data lost
double d = 9.99;
int cut = (int)d;               // explicit: decimals are dropped
Console.WriteLine($"{big} {cut}");

Console.WriteLine(Convert.ToInt32(9.5));    // rounds to even
Console.WriteLine(Convert.ToInt32(10.5));
Console.WriteLine(Convert.ToInt32("123"));

Console.WriteLine(int.Parse("456"));
Console.WriteLine(int.TryParse("12x", out int n) ? n : -1);

try
{
    long huge = 3_000_000_000L;
    int overflow = checked((int)huge);
    Console.WriteLine(overflow);
}
catch (OverflowException)
{
    Console.WriteLine("checked caught an overflow");
}

object o = "text";
if (o is string s) Console.WriteLine($"is string, length {s.Length}");
string? maybe = o as string;
int? none = o as int?;
Console.WriteLine($"{maybe} {none.HasValue}");

// Expected output should be:
// 42 9
// 10
// 10
// 123
// 456
// -1
// checked caught an overflow
// is string, length 4
// text False
