# 32 - Interfaces and Abstract Classes

## Interface

A contract: a list of methods a type promises to have, usually without the code for them.

```java
interface Shape {
    double area();
    double perimeter();
}
```

Interface methods are `public` and `abstract` unless you say otherwise. Java names interfaces by what they are or can do (`Shape`, `Comparable`), with no `I` prefix.

---

## Implementing an Interface

```java
class Circle implements Shape {
    private final double r;
    Circle(double r) { this.r = r; }

    @Override public double area()      { return Math.PI * r * r; }
    @Override public double perimeter() { return 2 * Math.PI * r; }
}
```

The methods must be `public`. Leaving it off is a compile error, because it would make them less visible.

A class can implement **many** interfaces, and extend one class at the same time:

```java
class Rectangle extends Figure implements Shape, Comparable<Rectangle> {
    // ...
}
```

---

## Interface as a Type

Declare variables and parameters by the interface. Any implementing class fits.

```java
Shape s = new Circle(5);

List<Shape> shapes = List.of(new Circle(3), new Square(4));
for (Shape shape : shapes) {
    System.out.printf("Area: %.2f%n", shape.area());
}
```

---

## Constants in Interfaces

Fields in an interface are always `public static final`.

```java
interface Limits {
    int MAX_USERS = 100;       // a constant, not a per-object field
}
```

Interfaces cannot hold per-object state. Put shared constants in a class or enum unless they really belong to the contract.

---

## Abstract Class

A class marked `abstract` cannot be created with `new`. It can mix finished methods, unfinished (`abstract`) methods, fields, and constructors.

```java
abstract class Animal {
    protected final String name;

    Animal(String name) { this.name = name; }

    abstract void speak();                         // children must write this

    void sleep() { System.out.println(name + " is sleeping."); }   // shared
}

class Dog extends Animal {
    Dog(String name) { super(name); }

    @Override
    void speak() { System.out.println(name + ": Woof!"); }
}
```

A class with even one `abstract` method must itself be `abstract`.

---

## Interface vs Abstract Class

| Feature            | Interface                       | Abstract class              |
| ------------------ | ------------------------------- | --------------------------- |
| How many per class | Many (`implements A, B`)        | One (`extends`)             |
| Per-object fields  | No (only constants)             | Yes                         |
| Constructors       | No                              | Yes                         |
| Method bodies      | `default`, `static`, `private`  | Yes                         |
| Member visibility  | `public` (or `private` helpers) | Any                         |
| Use when           | Describing a capability         | Sharing state and base code |

## Methods With Bodies in an Interface

Since Java 8, an interface can hold real code, not only method signatures.

| Kind      | Since  | Called as                 | Can be overridden | Use for                                 |
| --------- | ------ | ------------------------- | ----------------- | --------------------------------------- |
| `default` | Java 8 | `obj.method()`            | Yes               | Behavior every implementer gets         |
| `static`  | Java 8 | `Interface.method()`      | No                | Factories and helpers for the interface |
| `private` | Java 9 | Only inside the interface | No                | Sharing code between default methods    |

```java
interface Greeter {
    String name();

    default String greet() { return "Hello, " + name(); }

    static Greeter of(String name) { return () -> name; }   // one abstract method, so a lambda fits

    private String label() { return "[" + name() + "]"; }   // only the default methods call this
}
```

A `default` method has a body, so every implementing class gets it for free and may override it. They let a library add methods to an interface without breaking the classes that already implement it. Java 8 used them to add `forEach` to `Iterable`, `removeIf` and `stream` to `Collection`, and `getOrDefault` and `merge` to `Map`.

A `static` interface method belongs to the interface. Call it through the interface name, as in `List.of`, `Map.entry`, and `Comparator.comparing`, never through an instance.

When a class inherits the same default method from two interfaces, it must override it and pick one with `Interface.super.method()`:

```java
interface Walker  { default String move() { return "walk"; } }
interface Swimmer { default String move() { return "swim"; } }

class Duck implements Walker, Swimmer {
    @Override
    public String move() { return Walker.super.move() + " and " + Swimmer.super.move(); }   // walk and swim
}
```

---

## Same Method in Two Interfaces

Java has no explicit interface implementation. If two interfaces declare the same method signature, one method satisfies both.

```java
interface Printable { String show(); }
interface Viewable  { String show(); }

class Report implements Printable, Viewable {
    @Override
    public String show() { return "report"; }    // serves both
}
```

If the two methods have the same parameters but different return types that do not fit together, the class cannot implement both.

---

## Sealed Interfaces

A sealed interface (Java 17) lists every type allowed to implement it. Records and sealed interfaces together model "one of these shapes" data.

```java
sealed interface Result permits Ok, Err { }
record Ok(String value) implements Result { }
record Err(String message) implements Result { }
```

---

## Full Example

```java
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
```

Expected output should be:

```
Circle area 3.14, perimeter 6.28
Square area 4.00, perimeter 8.00
```

The interface is the contract. The abstract class holds code every shape shares.

---

## Common Built-in Interfaces

| Interface       | Purpose                                                                            |
| --------------- | ---------------------------------------------------------------------------------- |
| `Comparable<T>` | Natural order (`compareTo`), see [lesson 29](29_equals_hashcode_and_comparable.md) |
| `Comparator<T>` | Any other order (`compare`)                                                        |
| `Iterable<T>`   | Lets `for (x : obj)` work, see [Iterators](38_iterators.md)                        |
| `AutoCloseable` | Cleanup with try-with-resources, see [lesson 33](33_autocloseable.md)              |
| `Runnable`      | A task with no result (`run`)                                                      |
| `Collection<T>` | Base of `List`, `Set`, `Queue`, see [Collections](36_collections.md)               |

---

## Gotchas

- Forgetting `public` on an implemented method is a compile error
- Fields in an interface are constants, never per-object state
- Avoid large interfaces; split them so classes implement only what they need (Interface Segregation Principle)
- Choose an abstract class when children share state or need a constructor; otherwise prefer an interface
- Adding an abstract method to a published interface breaks every class that implements it; add a `default` method instead
- A default method cannot override `equals`, `hashCode`, or `toString`; that is a compile error, because the class version from `Object` always wins
- Static interface methods are not inherited: `Circle.square(2)` does not compile, only `Shape.square(2)` does
- Default methods cannot use fields, because interfaces have no per-object state; they can only call other methods

---

## Examples

- [32-01](examples/32-01_interfaces_and_abstract_classes.java): Interfaces and Abstract Classes

Run one with `java examples/32-01_interfaces_and_abstract_classes.java`. See [Examples](examples/README.md).
