// Lesson 25: Records & Equality (../25_records_and_equality.md)
// Records, with-expressions, and value equality
// Run: dotnet run 25-01_records.cs

var a = new Person("Alice", 30);
var b = new Person("Alice", 30);
var c = a with { Age = 31 };

Console.WriteLine(a);
Console.WriteLine(c);
Console.WriteLine($"a == b: {a == b}");
Console.WriteLine($"a == c: {a == c}");
Console.WriteLine($"ReferenceEquals(a, b): {ReferenceEquals(a, b)}");

var (name, age) = a;
Console.WriteLine($"{name} {age}");

var origin = new Point(0, 0);
var moved = origin with { X = 5 };
Console.WriteLine($"{origin} -> {moved}");

var set = new HashSet<Person> { a, b, c };
Console.WriteLine($"Distinct people: {set.Count}");

record Person(string Name, int Age);
readonly record struct Point(int X, int Y);

// Expected output should be:
// Person { Name = Alice, Age = 30 }
// Person { Name = Alice, Age = 31 }
// a == b: True
// a == c: False
// ReferenceEquals(a, b): False
// Alice 30
// Point { X = 0, Y = 0 } -> Point { X = 5, Y = 0 }
// Distinct people: 2
