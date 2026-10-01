// Lesson 06: Namespaces & Packages (../06_namespaces_and_packages.md)
// Namespaces, using alias, and using static
// Run: dotnet run 06-01_namespaces_and_aliases.cs

using Shapes = MyApp.Geometry.Shapes;
using static System.Math;

Console.WriteLine(new Shapes.Circle(2).Area());
Console.WriteLine(new MyApp.Geometry.Shapes.Square(3).Area());
Console.WriteLine(Sqrt(16));
Console.WriteLine(Max(3, 9));

namespace MyApp.Geometry.Shapes
{
    class Circle(double radius)
    {
        public double Area() => Round(PI * radius * radius, 2);
    }

    class Square(double side)
    {
        public double Area() => side * side;
    }
}

// Expected output should be:
// 12.57
// 9
// 4
// 9
