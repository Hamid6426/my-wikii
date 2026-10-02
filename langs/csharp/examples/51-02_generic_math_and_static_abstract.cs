// Lesson 51: Generics (../51_generics.md)
// static abstract members and generic math
// Run: dotnet run 51-02_generic_math_and_static_abstract.cs

using System.Numerics;

// 1. Generic math: one method for every numeric type
Console.WriteLine(Sum(new[] { 1, 2, 3 }));
Console.WriteLine(Sum(new[] { 1.5, 2.5 }));
Console.WriteLine(Sum(new[] { 10m, 20.5m }));
Console.WriteLine(Average(new[] { 4, 8, 15, 16 }));
Console.WriteLine(Clamp(150, 0, 100));
Console.WriteLine(Clamp(-2.5, 0.0, 1.0));
Console.WriteLine(Square(7) + " " + Square(1.5));

// 2. Your own interface with static abstract members
Console.WriteLine(Describe<Dog>());
Console.WriteLine(Describe<Cat>());
Console.WriteLine(Create<Dog>().Speak() + " / " + Create<Cat>().Speak());

// 3. Operators in an interface
var total = Combine(new Meters(2), new Meters(3));
Console.WriteLine(total);
Console.WriteLine(Parse<int>("42") + Parse<int>("8"));

static T Sum<T>(IEnumerable<T> items) where T : INumber<T>
{
    T total = T.Zero;
    foreach (T item in items) total += item;
    return total;
}

static T Average<T>(IReadOnlyCollection<T> items) where T : INumber<T>
    => Sum(items) / T.CreateChecked(items.Count);

static T Clamp<T>(T value, T min, T max) where T : INumber<T>
    => T.Max(min, T.Min(max, value));

static T Square<T>(T x) where T : IMultiplyOperators<T, T, T> => x * x;

static string Describe<T>() where T : IAnimal<T> => $"{T.Kind} that says {T.Sound}";

static T Create<T>() where T : IAnimal<T> => T.Create();

static T Combine<T>(T a, T b) where T : IAdditionOperators<T, T, T> => a + b;

static T Parse<T>(string text) where T : IParsable<T> => T.Parse(text, null);

interface IAnimal<TSelf> where TSelf : IAnimal<TSelf>
{
    static abstract string Kind { get; }
    static abstract string Sound { get; }
    static abstract TSelf Create();
    string Speak();
}

class Dog : IAnimal<Dog>
{
    public static string Kind => "dog";
    public static string Sound => "Woof";
    public static Dog Create() => new Dog();
    public string Speak() => Sound;
}

class Cat : IAnimal<Cat>
{
    public static string Kind => "cat";
    public static string Sound => "Meow";
    public static Cat Create() => new Cat();
    public string Speak() => Sound;
}

readonly record struct Meters(double Value) : IAdditionOperators<Meters, Meters, Meters>
{
    public static Meters operator +(Meters a, Meters b) => new(a.Value + b.Value);
    public override string ToString() => $"{Value} m";
}

// Expected output should be:
// 6
// 4
// 30.5
// 10
// 100
// 0
// 49 2.25
// dog that says Woof
// cat that says Meow
// Woof / Meow
// 5 m
// 50
