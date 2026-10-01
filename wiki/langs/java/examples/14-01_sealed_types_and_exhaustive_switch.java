// Lesson 14: Pattern Matching (../14_pattern_matching.md)
// Sealed types and exhaustive switch
// Run: java 14-01_sealed_types_and_exhaustive_switch.java

import java.util.List;

public class Shapes {
    sealed interface Shape permits Circle, Square, Rect {}
    record Circle(double radius) implements Shape {}
    record Square(double side) implements Shape {}
    record Rect(double width, double height) implements Shape {}

    static double area(Shape shape) {
        return switch (shape) {
            case Circle c                 -> Math.PI * c.radius() * c.radius();
            case Square(double side)      -> side * side;
            case Rect(double w, double h) -> w * h;
        };
    }

    public static void main(String[] args) {
        List<Shape> shapes = List.of(new Circle(1), new Square(2), new Rect(2, 3));
        for (Shape s : shapes) {
            System.out.printf("%s -> %.2f%n", s, area(s));
        }
    }
}
