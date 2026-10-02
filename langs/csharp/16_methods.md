# 16 - Methods

## Basic Method

```csharp
static void SayHello()
{
    Console.WriteLine("Hello!");
}

SayHello();  // call it
```

---

## Return Type

```csharp
static int Add(int a, int b)
{
    return a + b;
}

int result = Add(3, 4);  // 7
```

`void` means no return value.

---

## Expression-Bodied Methods (C# 6+)

Single-expression methods: no braces or `return` keyword.

```csharp
static int Add(int a, int b) => a + b;
static void Greet(string name) => Console.WriteLine($"Hello, {name}!");
```

---

## Method Overloading

Same name, different parameter signatures.

```csharp
static int Add(int a, int b) => a + b;
static double Add(double a, double b) => a + b;
static int Add(int a, int b, int c) => a + b + c;
```

Compiler picks the right version based on argument types.

---

## Static vs Instance Methods

**Static**: called on the class, no object needed.

```csharp
class MathHelper
{
    public static int Square(int n) => n * n;
}

int result = MathHelper.Square(5);  // 25
```

**Instance**: called on an object.

```csharp
class Greeter
{
    private string _prefix;

    public Greeter(string prefix) => _prefix = prefix;

    public void Greet(string name) => Console.WriteLine($"{_prefix}, {name}!");
}

var g = new Greeter("Hello");
g.Greet("Alice");
```

---

## Calling Methods

```csharp
// positional
int result = Add(3, 4);

// named arguments (any order)
int result2 = Add(b: 4, a: 3);
```

---

## Recursion

A method that calls itself. Must have a base case.

```csharp
static int Factorial(int n)
{
    if (n <= 1) return 1;
    return n * Factorial(n - 1);
}

Console.WriteLine(Factorial(5));  // 120
```

---

## Local Functions (C# 7+)

Functions defined inside another method: not visible outside.

```csharp
static int ProcessData(int[] data)
{
    int Doubled(int x) => x * 2;

    return data.Sum(Doubled);
}
```

---

## Static Local Functions

Add `static` to a local function to forbid it from using the outer method's variables. The compiler then guarantees it depends only on its own parameters.

```csharp
static int SumOfSquares(int[] numbers)
{
    int total = 0;
    foreach (int n in numbers)
    {
        total += Square(n);
    }
    return total;

    static int Square(int x) => x * x;   // cannot use 'total' or 'numbers'
}

Console.WriteLine(SumOfSquares(new[] { 1, 2, 3, 4 }));   // 30
```

Without `static`, a local function can read and change outer variables. That is handy, but the compiler must create an extra object to hold them. A static one avoids that cost and cannot change outer state by accident.

Local functions are also a good place for a helper that only one method needs, such as the `IsPrime` check inside a method that collects primes.

---

## Gotchas

- Methods are `private` by default inside a class
- Method name should be a verb or verb phrase (`GetUser`, `CalculateTotal`)
- Avoid methods with side effects that also return values: choose one
- Recursion can cause `StackOverflowException` without a proper base case

---

## Examples

- [16-01](examples/16-01_methods_tour.cs): Parameters, return values, overloads, expression bodies
- [16-02](examples/16-02_static_local_functions.cs): Local functions and static local functions

Run one with `dotnet run examples/16-01_methods_tour.cs`. See [Examples](examples/README.md).
