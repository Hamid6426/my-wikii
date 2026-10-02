# 28 - Nested, Inner and Anonymous Classes

## Kinds of Nested Classes

A **nested class** is a class declared inside another class. Java has four kinds, and the difference is whether the class needs an outer object.

| Kind          | Declared                        | Has access to outer `this` | Common use                        |
| ------------- | ------------------------------- | -------------------------- | --------------------------------- |
| Static nested | `static class X` inside a class | No                         | Helper types, builders, records   |
| Inner         | `class X` inside a class        | Yes                        | Objects tied to one outer object  |
| Local         | `class X` inside a method       | Yes, plus local variables  | Rare, one-off helpers             |
| Anonymous     | `new Interface() { ... }`       | Yes, plus local variables  | Old-style callbacks (now lambdas) |

The static member side of a class is covered in [Static Members and Utility Classes](27_static_members_and_utility_classes.md).

---

## Static Nested Class

Works like a normal class that happens to live inside another one. It does not need an outer object.

```java
class Shape {
    static class Point {
        final int x, y;
        Point(int x, int y) { this.x = x; this.y = y; }
    }
}

Shape.Point p = new Shape.Point(2, 3);
```

Nested records, enums, and interfaces are always static, even without the keyword.

---

## Inner Class

An inner class (non-static nested class) is tied to one outer object. It can read the outer object's fields directly.

```java
class Cart {
    private String owner = "Ana";

    class Line {
        String item;
        Line(String item) { this.item = item; }

        String describe() {
            return owner + " buys " + item;   // reads Cart's field
        }
    }
}

Cart cart = new Cart();
Cart.Line line = cart.new Line("pen");      // needs a Cart object
System.out.println(line.describe());        // Ana buys pen
```

Inside `Line`, `Cart.this` means the outer object, if a name is ambiguous.

---

## Local and Anonymous Classes

```java
void run() {
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
```

Both can use local variables only if they are **effectively final** (never reassigned). For a single-method interface, a lambda is shorter: see [Lambdas and Functional Interfaces](39_lambdas_and_functional_interfaces.md).

---

## When to Use Which

| Need                                        | Use                          |
| ------------------------------------------- | ---------------------------- |
| A helper type that stands on its own        | Static nested class          |
| An object that belongs to one outer object  | Inner class                  |
| A type used by one method only              | Local class                  |
| A quick implementation of a small interface | Anonymous class, or a lambda |

---

## Full Example

```java
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

        class Printer {
            void print(String s) { System.out.println(prefix + s); }
        }
        new Printer().print("local");

        Runnable r = new Runnable() {
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
```

Expected output should be:

```
Point: 2,3
Ana buys pen
>> local
>> anonymous
```

---

## Gotchas

- An inner class holds a hidden reference to its outer object, which can keep that object alive; make a nested class `static` unless it really needs the outer object
- A local or anonymous class can only use local variables that are effectively final
- Each anonymous class compiles to its own `Outer$1.class` file, which adds to the jar
- Prefer a lambda over an anonymous class when the interface has exactly one abstract method
- `this` inside an inner class means the inner object; write `Outer.this` for the outer one

---

## Examples

- [28-01](examples/28-01_nested_inner_and_anonymous_classes.java): Nested, inner and anonymous classes

Run one with `java examples/28-01_nested_inner_and_anonymous_classes.java`. See [Examples](examples/README.md).
