// Lesson 12: Math, Random and Utility Types (../12_math_random_and_utilities.md)
// Guid, Environment, and BigInteger
// Run: dotnet run 12-03_guid_and_environment.cs

using System.Numerics;

Guid id = Guid.NewGuid();
Console.WriteLine($"New id has {id.ToString("N").Length} characters");
Console.WriteLine(Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"));

Console.WriteLine($"OS: {Environment.OSVersion.Platform}");
Console.WriteLine($"CPU cores: {Environment.ProcessorCount}");
Console.WriteLine($"New line is {(Environment.NewLine == "\n" ? "LF" : "CRLF")}");
Console.WriteLine($"Linux? {OperatingSystem.IsLinux()}  Windows? {OperatingSystem.IsWindows()}");

BigInteger factorial = 1;
for (int i = 2; i <= 30; i++) factorial *= i;
Console.WriteLine($"30! = {factorial}");
Console.WriteLine($"2^200 = {BigInteger.Pow(2, 200)}");
