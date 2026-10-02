// Lesson 30: Polymorphism (../30_polymorphism.md)
// Polymorphism with abstract classes and overloads
// Run: dotnet run 30-01_shapes_polymorphism.cs

List<Shape> shapes = [new Circle(1), new Rectangle(2, 3), new Triangle(3, 4)];

foreach (Shape s in shapes)
{
    Console.WriteLine($"{s.Name,-10} area {s.Area():F2}");
}

Console.WriteLine($"Total area: {shapes.Sum(s => s.Area()):F2}");

Shape biggest = shapes.MaxBy(s => s.Area())!;
Console.WriteLine($"Biggest: {biggest.Name}");

Shape first = shapes[0];
if (first is Circle c)                       // downcast
{
    Console.WriteLine($"Circle radius {c.Radius}");
}

abstract class Shape
{
    public abstract string Name { get; }
    public abstract double Area();
}

class Circle(double radius) : Shape
{
    public double Radius { get; } = radius;
    public override string Name => "Circle";
    public override double Area() => Math.PI * Radius * Radius;
}

class Rectangle(double w, double h) : Shape
{
    public override string Name => "Rectangle";
    public override double Area() => w * h;
}

class Triangle(double b, double h) : Shape
{
    public override string Name => "Triangle";
    public override double Area() => 0.5 * b * h;
}

// Expected output should be:
// Circle     area 3.14
// Rectangle  area 6.00
// Triangle   area 6.00
// Total area: 15.14
// Biggest: Rectangle
// Circle radius 1
