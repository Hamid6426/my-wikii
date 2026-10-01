# 30 - Inheritance

## Basic Inheritance

A class inherits from one other class with `extends`. The child gets the parent's fields and methods.

```java
class Animal {
    protected String name;

    void eat() { System.out.println(name + " is eating."); }
}

class Dog extends Animal {
    void bark() { System.out.println(name + " says Woof!"); }
}

Dog dog = new Dog();
dog.name = "Rex";
dog.eat();    // inherited
dog.bark();   // its own
```

The parent is also called the **superclass**, and the child the **subclass**.

---

## super: Calling the Parent

`super(...)` calls the parent constructor. It must be the first statement in the constructor (Java 25 relaxes this, see below).

```java
class Animal {
    protected final String name;

    Animal(String name) { this.name = name; }

    void speak() { System.out.println(name + " makes a sound."); }
}

class Cat extends Animal {
    Cat(String name) { super(name); }        // calls Animal(name)

    @Override
    void speak() { System.out.println(name + " says Meow!"); }
}
```

If you write no `super(...)`, Java inserts `super()` for you. That fails to compile when the parent has no no-argument constructor.

---

## Overriding and @Override

Every non-static, non-private, non-final method in Java can be overridden. There is no `virtual` keyword: methods are virtual by default.

```java
class Shape {
    double area() { return 0; }
}

class Circle extends Shape {
    private final double radius;
    Circle(double radius) { this.radius = radius; }

    @Override
    double area() { return Math.PI * radius * radius; }
}
```

`@Override` is optional, but always use it. The compiler then fails if the method does not really override anything (a typo, or a wrong parameter type).

---

## Calling the Parent Method

```java
class TimestampLogger extends Logger {
    @Override
    void log(String msg) {
        super.log(msg);                     // run the parent logic first
        System.out.println("[at " + java.time.LocalTime.now() + "]");
    }
}
```

---

## final: Stop Inheritance

```java
final class Token { }                      // no class can extend Token

class Base {
    final void audit() { }                 // no subclass can override audit
}
```

`String`, `Integer`, and all records are `final`.

---

## sealed: Only These Children

A **sealed** class (Java 17) lists exactly which classes may extend it. Each child must be `final`, `sealed`, or `non-sealed`.

```java
sealed abstract class Payment permits Card, Cash { }

final class Card extends Payment { }
final class Cash extends Payment { }
```

The compiler then knows every subtype, which makes `switch` checks complete. See [Pattern Matching](14_pattern_matching.md).

---

## Access Modifiers and Inheritance

| Modifier    | Same class | Same package | Subclass | Everyone |
| ----------- | ---------- | ------------ | -------- | -------- |
| `private`   | Yes        | No           | No       | No       |
| (none)      | Yes        | Yes          | No       | No       |
| `protected` | Yes        | Yes          | Yes      | No       |
| `public`    | Yes        | Yes          | Yes      | Yes      |

An override cannot be less visible than the method it replaces: a `public` method stays `public`.

---

## Constructor Order

Parent constructors run before child constructors, all the way up.

```java
public class Main {
    public static void main(String[] args) {
        new C();
    }
}

class A { A() { System.out.println("A"); } }
class B extends A { B() { System.out.println("B"); } }
class C extends B { C() { System.out.println("C"); } }
```

Expected output should be:

```
A
B
C
```

Every class ends up inheriting from `Object`, which gives `equals`, `hashCode`, and `toString`.

---

## Statements Before super (Java 25)

Java 25 lets you run code before `super(...)`, as long as it does not touch `this`. Handy for checking arguments first.

```java
class Square extends Rectangle {
    Square(double side) {
        if (side <= 0) throw new IllegalArgumentException("side");
        super(side, side);
    }
}
```

On Java 21, move the check into a static helper method and call `super(check(side), side)`.

---

## Hiding Is Not Overriding

Static methods and fields are **hidden**, not overridden. Which one runs depends on the declared type, not the real object.

```java
class Parent { static String who() { return "Parent"; } }
class Child extends Parent { static String who() { return "Child"; } }

Parent p = new Child();
// p.who() would return "Parent": static calls use the declared type
System.out.println(Child.who());   // Child
```

Java has no `new` keyword for hiding instance methods. An instance method with the same signature always overrides.

---

## Checking the Type

```java
Animal a = new Dog();

boolean isDog = a instanceof Dog;        // true

if (a instanceof Dog d) {                // test and cast in one step
    d.bark();
}
```

---

## Composition over Inheritance

Use `extends` only for a real "is-a" link (a `Dog` is an `Animal`). For "has-a", keep the other object in a field.

```java
class Car {
    private final Engine engine = new Engine();   // a Car has an Engine

    void start() { engine.start(); }
}
```

---

## Gotchas

- A class can extend only **one** class; use interfaces for more (see [Interfaces and Abstract Classes](32_interfaces_and_abstract_classes.md))
- Methods are overridable by default; mark ones you do not design for extension `final`
- Calling an overridable method from a constructor runs the child's version before the child's fields are set
- `private` methods are not inherited, so a child method with the same name is a new method, not an override
- Changing a parent class can break every child (the "fragile base class" problem); keep hierarchies shallow

---

## Examples

- [30-01](examples/30-01_constructor_order.java): Constructor order

Run one with `java examples/30-01_constructor_order.java`. See [Examples](examples/README.md).
