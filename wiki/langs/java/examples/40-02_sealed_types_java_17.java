// Lesson 40: Modern Java Features (../40_modern_java_features.md)
// Sealed types java 17
// Run: java 40-02_sealed_types_java_17.java

public class Sealed {
    sealed interface Shape permits Circle, Square, Rect {}
    record Circle(double r) implements Shape {}
    record Square(double side) implements Shape {}
    record Rect(double w, double h) implements Shape {}

    static double area(Shape s) {
        return switch (s) {
            case Circle c -> Math.PI * c.r() * c.r();
            case Square q -> q.side() * q.side();
            case Rect(double w, double h) -> w * h;   // record pattern
        };   // no default needed: the compiler knows every Shape
    }

    public static void main(String[] args) {
        System.out.println(area(new Square(3)));    // 9.0
        System.out.println(area(new Rect(2, 5)));   // 10.0
    }
}
