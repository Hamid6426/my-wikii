# 31 - Polymorphism

## What is Polymorphism

Treating objects of different types through one shared type, while each object runs its own version of a method. The word means "many forms".

---

## Runtime Polymorphism (Overriding)

The JVM picks the method from the **real object**, not from the variable's declared type. This is called dynamic dispatch.

```java
abstract class Shape {
    abstract double area();
}

class Circle extends Shape {
    private final double r;
    Circle(double r) { this.r = r; }
    @Override double area() { return Math.PI * r * r; }
}

class Rectangle extends Shape {
    private final double w, h;
    Rectangle(double w, double h) { this.w = w; this.h = h; }
    @Override double area() { return w * h; }
}

Shape[] shapes = { new Circle(1), new Rectangle(4, 6) };
for (Shape s : shapes) {
    System.out.printf("%.2f%n", s.area());   // each runs its own area()
}
```

---

## Compile-Time Polymorphism (Overloading)

Same name, different parameter lists. The compiler picks one from the **declared** types of the arguments.

```java
class Printer {
    void print(int n)    { System.out.println("int: " + n); }
    void print(String s) { System.out.println("String: " + s); }
    void print(Object o) { System.out.println("Object: " + o); }
}

Printer p = new Printer();
Object text = "hi";
p.print(5);       // int: 5
p.print("hi");    // String: hi
p.print(text);    // Object: hi (declared type is Object)
```

---

## Overriding vs Overloading

| Feature    | Overriding                  | Overloading                       |
| ---------- | --------------------------- | --------------------------------- |
| Where      | Child class                 | Same class (or child)             |
| Parameters | Same                        | Different                         |
| Chosen     | At run time, by real object | At compile time, by declared type |
| Annotation | `@Override`                 | None                              |

---

## Interface Polymorphism

Unrelated classes can share a type through an interface.

```java
interface Drawable {
    void draw();
}

class Button implements Drawable {
    public void draw() { System.out.println("Drawing Button"); }
}

class Chart implements Drawable {
    public void draw() { System.out.println("Drawing Chart"); }
}

List<Drawable> items = List.of(new Button(), new Chart());
items.forEach(Drawable::draw);
```

---

## Upcasting and Downcasting

**Upcasting** (child to parent) is automatic and always safe.

```java
Dog dog = new Dog("Rex");
Animal animal = dog;            // upcast
```

**Downcasting** (parent to child) needs a cast and can fail at run time.

```java
Animal a = new Cat("Luna");

Dog d = (Dog) a;                // throws ClassCastException

if (a instanceof Dog safe) {    // safe: test first
    safe.bark();
}
```

---

## Switch over a Sealed Type

With a sealed parent, a `switch` can handle every subtype, and the compiler checks none are missing. See [Pattern Matching](14_pattern_matching.md).

```java
sealed interface Shape permits Circle, Square { }
record Circle(double r) implements Shape { }
record Square(double side) implements Shape { }

static double area(Shape s) {
    return switch (s) {
        case Circle c -> Math.PI * c.r() * c.r();
        case Square q -> q.side() * q.side();
    };                          // no default needed
}
```

This is the other way to get "many forms": the behavior lives in one `switch` instead of in each class. Use it when the set of types is fixed and you add operations often.

---

## Covariant Return Types

An override may return a more specific type than the parent method.

```java
class Animal {
    Animal copy() { return new Animal(); }
}

class Dog extends Animal {
    @Override
    Dog copy() { return new Dog(); }    // Dog, not Animal
}
```

---

## Full Example

```java
import java.util.List;

public class Main {
    public static void main(String[] args) {
        List<Animal> animals = List.of(new Dog("Rex"), new Cat("Luna"), new Dog("Max"));

        for (Animal a : animals) {
            a.speak();                          // runtime dispatch
        }

        Animal first = animals.get(0);
        if (first instanceof Dog d) {
            d.fetch();                          // only Dog has fetch()
        }

        describe(first);                        // overload picked by declared type
        describe((Dog) first);
    }

    static void describe(Animal a) { System.out.println("an animal"); }
    static void describe(Dog d)    { System.out.println("a dog"); }
}

abstract class Animal {
    protected final String name;
    Animal(String name) { this.name = name; }
    abstract void speak();
}

class Dog extends Animal {
    Dog(String name) { super(name); }
    @Override void speak() { System.out.println(name + ": Woof!"); }
    void fetch() { System.out.println(name + " fetches the ball"); }
}

class Cat extends Animal {
    Cat(String name) { super(name); }
    @Override void speak() { System.out.println(name + ": Meow!"); }
}
```

Expected output should be:

```
Rex: Woof!
Luna: Meow!
Max: Woof!
Rex fetches the ball
an animal
a dog
```

---

## Gotchas

- Overloads are chosen by declared type, overrides by real type; mixing the two surprises people (see `describe` above)
- Fields are not polymorphic: a field with the same name in the child hides the parent field, and the declared type decides which one you read
- `static` and `private` methods are never dispatched at run time
- A child method with a slightly different parameter type is an overload, not an override; `@Override` catches it
- Long `instanceof` chains usually mean a method belongs on the type, or the type should be sealed and switched on

---

## Examples

- [31-01](examples/31-01_polymorphism.java): Polymorphism

Run one with `java examples/31-01_polymorphism.java`. See [Examples](examples/README.md).
