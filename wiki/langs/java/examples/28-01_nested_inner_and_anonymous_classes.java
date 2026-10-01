// Lesson 28: Nested, Inner and Anonymous Classes (../28_nested_inner_and_anonymous_classes.md)
// Nested, inner and anonymous classes
// Run: java 28-01_nested_inner_and_anonymous_classes.java

public class Main {
    public static void main(String[] args) {
        Shape.Point p = new Shape.Point(2, 3);
        System.out.println("Point: " + p.x + "," + p.y);

        Cart cart = new Cart("Ana");
        Cart.Line line = cart.new Line("pen");
        System.out.println(line.describe());

        run();
    }

    static void run() {
        String prefix = ">> ";

        class Printer {                          // local class
            void print(String s) { System.out.println(prefix + s); }
        }
        new Printer().print("local");

        Runnable r = new Runnable() {            // anonymous class
            @Override
            public void run() { System.out.println(prefix + "anonymous"); }
        };
        r.run();
    }
}

class Shape {
    static class Point {
        final int x, y;
        Point(int x, int y) { this.x = x; this.y = y; }
    }
}

class Cart {
    private final String owner;
    Cart(String owner) { this.owner = owner; }

    class Line {
        private final String item;
        Line(String item) { this.item = item; }
        String describe() { return owner + " buys " + item; }
    }
}
