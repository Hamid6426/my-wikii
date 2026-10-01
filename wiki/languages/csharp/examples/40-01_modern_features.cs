// Lesson 40: Modern C# Features (../40_modern_csharp_features.md)
// Primary constructors, collection expressions, field keyword
// Run: dotnet run 40-01_modern_features.cs

using Point = (int X, int Y);

Console.WriteLine(new Greeter("Ada").Greet());

int[] first = [1, 2, 3];
int[] second = [4, 5];
List<int> all = [..first, ..second, 6];
Console.WriteLine(string.Join(",", all));

Point p = (3, 4);
Console.WriteLine($"{p.X},{p.Y}");

var multiply = (int x, int factor = 2) => x * factor;
Console.WriteLine($"{multiply(5)} {multiply(5, 3)}");

Console.WriteLine(Sum(1, 2, 3, 4));

var user = new User { Name = "   Alice   " };
Console.WriteLine($"[{user.Name}]");

User? maybe = null;
maybe?.Name = "never assigned";
Console.WriteLine(maybe is null);

Console.WriteLine(Fib(20));

static int Fib(int n)
{
    return Loop(n, 0, 1);

    static int Loop(int n, int a, int b) => n == 0 ? a : Loop(n - 1, b, a + b);   // local function
}

static int Sum(params ReadOnlySpan<int> numbers)
{
    int total = 0;
    foreach (int n in numbers) total += n;
    return total;
}

class Greeter(string name)
{
    public string Greet() => $"Hello, {name}!";
}

class User
{
    public string Name
    {
        get;
        set => field = value.Trim();
    } = "";
}

// Expected output should be:
// Hello, Ada!
// 1,2,3,4,5,6
// 3,4
// 10 15
// 10
// [Alice]
// True
// 6765
