// Lesson 16: Methods (../16_methods.md)
// Parameters, return values, overloads, expression bodies
// Run: dotnet run 16-01_methods_tour.cs

Console.WriteLine(Calc.Add(2, 3));
Console.WriteLine(Calc.Add(2.5, 3.5));
Console.WriteLine(Calc.Add(1, 2, 3));
Console.WriteLine(Greet("Alice"));
Console.WriteLine(Greet("Alice", greeting: "Welcome"));
Console.WriteLine(Square(9));
Console.WriteLine(Factorial(10));

Console.WriteLine(Calc.IsPrime(97));
var counter = new Counter();
counter.Increment();
counter.Increment();
Console.WriteLine(counter.Value);

static string Greet(string name, string greeting = "Hello") => $"{greeting}, {name}!";

static int Square(int x) => x * x;

static long Factorial(int n)
{
    if (n <= 1) return 1;
    return n * Factorial(n - 1);
}

static class Calc
{
    // Overloads: same name, different parameters
    public static int Add(int a, int b) => a + b;
    public static double Add(double a, double b) => a + b;
    public static int Add(int a, int b, int c) => a + b + c;

    public static bool IsPrime(int n)
    {
        if (n < 2) return false;
        for (int i = 2; i * i <= n; i++)
        {
            if (n % i == 0) return false;
        }
        return true;
    }
}

class Counter
{
    public int Value { get; private set; }
    public void Increment() => Value++;
}

// Expected output should be:
// 5
// 6
// 6
// Hello, Alice!
// Welcome, Alice!
// 81
// 3628800
// True
// 2
