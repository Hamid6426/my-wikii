// Lesson 16: Methods (../16_methods.md)
// Local functions and static local functions
// Run: dotnet run 16-02_static_local_functions.cs

Console.WriteLine(SumOfSquares(new[] { 1, 2, 3, 4 }));
Console.WriteLine(Describe(7));
Console.WriteLine(string.Join(" ", FirstPrimes(8)));

static int SumOfSquares(int[] numbers)
{
    int total = 0;
    foreach (int n in numbers)
    {
        total += Square(n);
    }
    return total;

    static int Square(int x) => x * x;      // static: cannot use 'total' or 'numbers'
}

static string Describe(int n)
{
    string kind = IsOdd(n) ? "odd" : "even";
    return $"{n} is {kind}";

    bool IsOdd(int value) => value % 2 != 0;
}

static List<int> FirstPrimes(int count)
{
    var primes = new List<int>();
    for (int candidate = 2; primes.Count < count; candidate++)
    {
        if (IsPrime(candidate)) primes.Add(candidate);
    }
    return primes;

    static bool IsPrime(int n)
    {
        for (int i = 2; i * i <= n; i++)
        {
            if (n % i == 0) return false;
        }
        return true;
    }
}

// Expected output should be:
// 30
// 7 is odd
// 2 3 5 7 11 13 17 19
