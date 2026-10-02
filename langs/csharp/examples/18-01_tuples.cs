// Lesson 18: Tuples (../18_tuples.md)
// Tuples, named elements, and deconstruction
// Run: dotnet run 18-01_tuples.cs

var (min, max, average) = Stats(new[] { 4, 8, 15, 16, 23, 42 });
Console.WriteLine($"min={min} max={max} average={average:F2}");

(string Name, int Age) person = ("Alice", 30);
Console.WriteLine($"{person.Name} is {person.Age}");

var (name, age) = person;
Console.WriteLine($"{name}/{age}");

int x = 1, y = 2;
(x, y) = (y, x);
Console.WriteLine($"x={x} y={y}");

Console.WriteLine((1, "a") == (1, "a"));

var point = new Point(3, 4);
var (px, py) = point;
Console.WriteLine($"{px},{py}");

static (int Min, int Max, double Average) Stats(int[] nums)
    => (nums.Min(), nums.Max(), nums.Average());

class Point(int x, int y)
{
    public void Deconstruct(out int px, out int py) => (px, py) = (x, y);
}

// Expected output should be:
// min=4 max=42 average=18.00
// Alice is 30
// Alice/30
// x=2 y=1
// True
// 3,4
