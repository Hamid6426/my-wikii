# 14 - Pattern Matching

## What is Pattern Matching

Test a value's shape and pull data out of it in one step. It works with `instanceof` and `switch`.

A **pattern** is a test plus variables. `String s` is a pattern: "is it a `String`? If so, call it `s`".

| Feature                      | Since   |
| ---------------------------- | ------- |
| `instanceof` type pattern    | Java 16 |
| Type patterns in `switch`    | Java 21 |
| Record patterns              | Java 21 |
| Unnamed pattern `_`          | Java 22 |
| Primitive patterns (preview) | Java 23 |

---

## Type Pattern with instanceof

```java
Object o = "hello";

if (o instanceof String s) {
    System.out.println(s.length());   // 5: s is already a String here
}

if (!(o instanceof String s)) {
    return;
}
System.out.println(s.toUpperCase());  // s is in scope after the early return
```

Before Java 16 you had to test, then cast by hand: `if (o instanceof String) { String s = (String) o; }`.

The pattern variable can be used in the rest of the same condition:

```java
if (o instanceof String s && !s.isEmpty()) {
    System.out.println(s.charAt(0));
}
```

---

## Type Patterns in switch

A `switch` can match on the type of the value. The first case that matches wins.

```java
static String describe(Object o) {
    return switch (o) {
        case null                      -> "nothing";
        case Integer n when n < 0      -> "negative number";
        case Integer n                 -> "number " + n;
        case String s                  -> "text of length " + s.length();
        case int[] a when a.length == 0 -> "empty array";
        default                        -> "something else";
    };
}
```

| Part        | Meaning                                                                  |
| ----------- | ------------------------------------------------------------------------ |
| `case null` | Matches `null`. Without it, a `null` value throws `NullPointerException` |
| `when`      | A **guard**: an extra condition the case must also pass                  |
| `default`   | Matches anything left over                                               |

See [Control Flow](13_control_flow.md) for the arrow form of `switch`.

---

## Record Patterns

A **record pattern** takes a record apart into its components. Records are covered in [Records and Equality](25_records_and_equality.md).

```java
record Point(int x, int y) {}

Object o = new Point(3, 4);

if (o instanceof Point(int x, int y)) {
    System.out.println(x + y);   // 7
}
```

Patterns can nest:

```java
record Line(Point start, Point end) {}

if (o instanceof Line(Point(var x1, var y1), Point(var x2, var y2))) {
    System.out.println("from " + x1 + "," + y1 + " to " + x2 + "," + y2);
}
```

`var` in a pattern lets the compiler fill in the component type.

---

## Sealed Types and Exhaustive switch

A **sealed** interface lists every type allowed to implement it. The compiler then knows all the cases, so a `switch` needs no `default`.

```java
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
```

Expected output should be:

```
Circle[radius=1.0] -> 3.14
Square[side=2.0] -> 4.00
Rect[width=2.0, height=3.0] -> 6.00
```

Add a new `Shape` and every `switch` without a matching case stops compiling. That is the point: the compiler finds the places you forgot.

---

## Unnamed Pattern (Java 22+)

Use `_` for a part you do not need.

```java
if (o instanceof Point(int x, _)) {
    System.out.println("x is " + x);
}

String kind = switch (shape) {
    case Circle _          -> "round";
    case Square _, Rect _  -> "has corners";
};
```

---

## Primitive Patterns (Preview)

Java 23 to 25 have a preview feature that lets patterns test primitive values, such as `case int i when i > 0`. A **preview** feature is not final and needs `--enable-preview`. Do not use it in production code yet.

---

## Gotchas

- Order of cases matters: a case that can never match because an earlier one covers it is a compile error (`dominated` case)
- A pattern `switch` without `case null` throws `NullPointerException` on `null`
- `when` guards are not checked for coverage, so a `switch` that relies on them still needs a `default` or an unguarded case
- `instanceof` with a pattern never matches `null`, so `null instanceof String s` is `false`
- A pattern variable is only in scope where the compiler knows the match succeeded: `o instanceof String s && s.isEmpty()` compiles, but `o instanceof String s || s.isEmpty()` does not

---

## Examples

- [14-01](examples/14-01_sealed_types_and_exhaustive_switch.java): Sealed types and exhaustive switch

Run one with `java examples/14-01_sealed_types_and_exhaustive_switch.java`. See [Examples](examples/README.md).
