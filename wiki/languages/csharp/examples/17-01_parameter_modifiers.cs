// Lesson 17: Parameters (../17_params.md)
// ref, out, in, params, optional and named arguments
// Run: dotnet run 17-01_parameter_modifiers.cs

int a = 1, b = 2;
Swap(ref a, ref b);
Console.WriteLine($"a={a}, b={b}");

if (TryDivide(10, 4, out double result))
{
    Console.WriteLine($"10 / 4 = {result}");
}
Console.WriteLine(TryDivide(1, 0, out _) ? "ok" : "cannot divide by zero");

Console.WriteLine(Sum());
Console.WriteLine(Sum(1, 2, 3, 4));

Console.WriteLine(Box(width: 3, height: 2));
Console.WriteLine(Box(5));

var p = new Point(3, 4);
Console.WriteLine(Length(in p));

static void Swap(ref int x, ref int y) => (x, y) = (y, x);

static bool TryDivide(int x, int y, out double quotient)
{
    quotient = 0;
    if (y == 0) return false;
    quotient = (double)x / y;
    return true;
}

static int Sum(params int[] numbers) => numbers.Sum();

static string Box(int width, int height = 1) => $"{width}x{height}";

static double Length(in Point p) => Math.Sqrt(p.X * p.X + p.Y * p.Y);

readonly record struct Point(double X, double Y);

// Expected output should be:
// a=2, b=1
// 10 / 4 = 2.5
// cannot divide by zero
// 0
// 10
// 3x2
// 5x1
// 5
