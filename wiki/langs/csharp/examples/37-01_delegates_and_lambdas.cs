// Lesson 37: Delegates and Lambdas (../37_delegates_and_lambdas.md)
// Delegates, Func, Action, closures, and multicast
// Run: dotnet run 37-01_delegates_and_lambdas.cs

Operation op = (a, b) => a + b;
Console.WriteLine(op(2, 3));
op = (a, b) => a * b;
Console.WriteLine(op(2, 3));

Func<int, int> square = x => x * x;
Func<int, bool> isEven = n => n % 2 == 0;
Action<string> shout = s => Console.WriteLine(s.ToUpper() + "!");

shout("hello");
Console.WriteLine(string.Join(",", Enumerable.Range(1, 6).Where(isEven).Select(square)));

Func<int, Func<int, int>> adder = x => y => x + y;
var addFive = adder(5);
Console.WriteLine(addFive(10));

int total = 0;
Action addToTotal = () => total += 10;
addToTotal();
addToTotal();
Console.WriteLine($"total = {total}");      // the lambda changed the outer variable

Action log = () => Console.WriteLine("A");
log += () => Console.WriteLine("B");
log();

var words = new List<string> { "pear", "fig", "banana" };
words.Sort((a, b) => a.Length.CompareTo(b.Length));
Console.WriteLine(string.Join(", ", words));

Console.WriteLine(Apply(5, x => x + 1));
Console.WriteLine(Apply(5, square));

static int Apply(int value, Func<int, int> f) => f(value);

delegate int Operation(int a, int b);

// Expected output should be:
// 5
// 6
// HELLO!
// 4,16,36
// 15
// total = 20
// A
// B
// fig, pear, banana
// 6
// 25
