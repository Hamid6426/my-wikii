// Lesson 11: Operators (../11_operators.md)
// Arithmetic, comparison, logical, bitwise, ternary
// Run: dotnet run 11-01_operators.cs

int a = 17, b = 5;
Console.WriteLine($"{a} + {b} = {a + b}");
Console.WriteLine($"{a} / {b} = {a / b} (integer division)");
Console.WriteLine($"{a} % {b} = {a % b}");
Console.WriteLine($"{a} / {b}.0 = {a / (double)b}");

int c = 10;
c += 5; c *= 2; c -= 1;
Console.WriteLine($"c = {c}");
int i = 5;
Console.WriteLine($"{i++} then {i}, {++i} then {i}");

Console.WriteLine($"{a > b} {a == b} {a != b}");
Console.WriteLine($"{(a > 10 && b < 10)} {(a > 100 || b < 10)} {!(a > b)}");

int flags = 0b_0101;
Console.WriteLine($"{flags & 0b_0100} {flags | 0b_0010} {flags ^ 0b_1111} {flags << 1} {flags >> 1}");

string label = a % 2 == 0 ? "even" : "odd";
string? name = null;
Console.WriteLine($"{label} {name ?? "unknown"} {name?.Length}");

// Expected output should be:
// 17 + 5 = 22
// 17 / 5 = 3 (integer division)
// 17 % 5 = 2
// 17 / 5.0 = 3.4
// c = 29
// 5 then 6, 7 then 7
// True False True
// True True False
// 4 7 10 10 2
// odd unknown
