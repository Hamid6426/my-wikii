// Lesson 12: Math, Random and Utility Types (../12_math_random_and_utilities.md)
// A tour of the Math class
// Run: dotnet run 12-01_math_tour.cs

Console.WriteLine($"Abs(-5) = {Math.Abs(-5)}");
Console.WriteLine($"Pow(2, 10) = {Math.Pow(2, 10)}");
Console.WriteLine($"Sqrt(144) = {Math.Sqrt(144)}");
Console.WriteLine($"Floor(3.7) = {Math.Floor(3.7)}, Ceiling(3.2) = {Math.Ceiling(3.2)}");
Console.WriteLine($"Round(2.5) = {Math.Round(2.5)}, Round(3.5) = {Math.Round(3.5)}");
Console.WriteLine($"Round(2.5, AwayFromZero) = {Math.Round(2.5, MidpointRounding.AwayFromZero)}");
Console.WriteLine($"Clamp(15, 0, 10) = {Math.Clamp(15, 0, 10)}");
Console.WriteLine($"Max(3, 9) = {Math.Max(3, 9)}, Min(3, 9) = {Math.Min(3, 9)}");

var (quotient, remainder) = Math.DivRem(17, 5);
Console.WriteLine($"17 / 5 = {quotient} remainder {remainder}");

double radius = 3;
Console.WriteLine($"Circle area, r=3: {Math.PI * radius * radius:F2}");
Console.WriteLine($"Sin(90 degrees) = {Math.Sin(90 * Math.PI / 180)}");
Console.WriteLine($"0.1 + 0.2 == 0.3 ? {0.1 + 0.2 == 0.3}");
Console.WriteLine($"decimal 0.1 + 0.2 == 0.3 ? {0.1m + 0.2m == 0.3m}");

// Expected output should be:
// Abs(-5) = 5
// Pow(2, 10) = 1024
// Sqrt(144) = 12
// Floor(3.7) = 3, Ceiling(3.2) = 4
// Round(2.5) = 2, Round(3.5) = 4
// Round(2.5, AwayFromZero) = 3
// Clamp(15, 0, 10) = 10
// Max(3, 9) = 9, Min(3, 9) = 3
// 17 / 5 = 3 remainder 2
// Circle area, r=3: 28.27
// Sin(90 degrees) = 1
// 0.1 + 0.2 == 0.3 ? False
// decimal 0.1 + 0.2 == 0.3 ? True
