// Lesson 32: Interfaces and Abstract Classes (../32_interfaces_and_abstract_classes.md)
// Interfaces and Abstract Classes
// Run: java 32-01_interfaces_and_abstract_classes.java

import java.util.List;

public class Main {
    public static void main(String[] args) {
        List<Shape> shapes = List.of(new Circle(1), new Square(2));
        for (Shape s : shapes) {
            System.out.printf("%s area %.2f, perimeter %.2f%n", s.name(), s.area(), s.perimeter());
        }
    }
}

interface Shape {
    double area();
    double perimeter();
    String name();
}

abstract class BaseShape implements Shape {
    @Override
    public String name() { return getClass().getSimpleName(); }   // shared code
}

class Circle extends BaseShape {
    private final double r;
    Circle(double r) { this.r = r; }
    @Override public double area()      { return Math.PI * r * r; }
    @Override public double perimeter() { return 2 * Math.PI * r; }
}

class Square extends BaseShape {
    private final double side;
    Square(double side) { this.side = side; }
    @Override public double area()      { return side * side; }
    @Override public double perimeter() { return 4 * side; }
}
