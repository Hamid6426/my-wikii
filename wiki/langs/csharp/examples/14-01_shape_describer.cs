// Lesson 14: Pattern Matching (../14_pattern_matching.md)
// Pattern matching with is, switch, and list patterns
// Run: dotnet run 14-01_shape_describer.cs

Console.WriteLine(Describe(null));
Console.WriteLine(Describe(-3));
Console.WriteLine(Describe(42));
Console.WriteLine(Describe("hello"));
Console.WriteLine(Describe(new int[0]));
Console.WriteLine(Describe(new Person("Alice", 17)));
Console.WriteLine(Describe(new Person("Bob", 30)));
Console.WriteLine(Describe(new[] { 1, 2, 3 }));

Console.WriteLine(Quadrant(2, 5));
Console.WriteLine(Quadrant(-2, 5));
Console.WriteLine(Quadrant(0, 4));

static string Describe(object? o) => o switch
{
    null => "nothing",
    int n when n < 0 => "negative number",
    int n => $"number {n}",
    string { Length: > 3 } s => $"long text: {s}",
    string s => $"short text: {s}",
    int[] { Length: 0 } => "empty array",
    int[] and [var first, .., var last] => $"array from {first} to {last}",
    Person { Age: < 18 } p => $"{p.Name} is a minor",
    Person p => $"{p.Name} is an adult",
    _ => "something else"
};

static string Quadrant(int x, int y) => (x, y) switch
{
    ( > 0, > 0) => "first quadrant",
    ( < 0, > 0) => "second quadrant",
    (0, _) or (_, 0) => "on an axis",
    _ => "another quadrant"
};

record Person(string Name, int Age);

// Expected output should be:
// nothing
// negative number
// number 42
// long text: hello
// empty array
// Alice is a minor
// Bob is an adult
// array from 1 to 3
// first quadrant
// second quadrant
// on an axis
