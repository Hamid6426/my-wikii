# 26 - Classes and Objects

## Defining a Class

A **class** is a blueprint. An **object** is one thing built from it, with its own copy of the fields.

```java
public class Person {
    // Fields: the data each object holds
    private String name;
    private int age;

    // Constructor: runs when the object is created
    public Person(String name, int age) {
        this.name = name;
        this.age = age;
    }

    // Methods: what the object can do
    public void introduce() {
        System.out.println("Hi, I'm " + name + ", age " + age + ".");
    }
}
```

A `public` class must live in a file with the same name: `Person.java`. See [Packages and Imports](06_packages_and_imports.md).

---

## Creating Objects

`new` creates an object and runs the constructor. The variable holds a reference to it.

```java
Person p = new Person("Alice", 30);
p.introduce();                    // Hi, I'm Alice, age 30.

var p2 = new Person("Bob", 25);   // var works too
```

---

## Fields

Fields get a default value if you do not set one: `0`, `false`, or `null`. Local variables do not, and must be set before use.

```java
class Counter {
    private int count;               // starts at 0
    private final String label;      // must be set once, in the constructor
    private static int total = 0;    // one copy shared by all Counter objects

    Counter(String label) { this.label = label; }
}
```

`final` on a field means "assigned exactly once". `static` members belong to the class, not to each object. See [Static Members and Utility Classes](27_static_members_and_utility_classes.md).

---

## Getters and Setters

Java has no properties. Keep fields `private` and expose methods. This is called **encapsulation**: the class controls how its data is read and changed.

```java
class Product {
    private double price;

    public double getPrice() {
        return price;
    }

    public void setPrice(double price) {
        if (price < 0) throw new IllegalArgumentException("Price cannot be negative");
        this.price = price;
    }
}
```

Leave out the setter for a read-only value. For a class that is only data, a [record](25_records_and_equality.md) is shorter.

---

## Constructors

```java
class Point {
    private final double x;
    private final double y;

    Point(double x, double y) {
        this.x = x;
        this.y = y;
    }

    Point() {
        this(0, 0);      // calls the other constructor
    }
}
```

- With no constructor written, Java adds an empty **default constructor**
- Once you write any constructor, the default one is gone
- `this(...)` calls another constructor of the same class

Before Java 25, `this(...)` or `super(...)` had to be the first statement. Java 25 allows code before it, as long as that code does not use the object being built. This is called **flexible constructor bodies**.

```java
Point(String text) {
    String[] parts = text.split(",");                                // Java 25+
    this(Double.parseDouble(parts[0]), Double.parseDouble(parts[1]));
}
```

---

## this Keyword

`this` means "the current object". Use it when a parameter has the same name as a field, or to return the object for **method chaining** (calling several methods in one expression).

```java
class Builder {
    private String name;
    private int size;

    Builder name(String name) {
        this.name = name;    // this.name is the field, name is the parameter
        return this;
    }

    Builder size(int size) {
        this.size = size;
        return this;
    }
}

new Builder().name("box").size(3);
```

---

## toString

Printing an object calls its `toString()`. The default prints the class name and a hash, such as `Person@1b6d3586`. Override it:

```java
@Override
public String toString() {
    return "Person[" + name + ", " + age + "]";
}
```

`@Override` asks the compiler to check that you really are replacing a method from the parent class. See [Inheritance](30_inheritance.md).

---

## A Full Example

```java
public class Bank {
    static class Account {
        private final String owner;
        private double balance;

        Account(String owner, double opening) {
            if (opening < 0) throw new IllegalArgumentException("opening < 0");
            this.owner = owner;
            this.balance = opening;
        }

        Account(String owner) {
            this(owner, 0);
        }

        void deposit(double amount) {
            if (amount <= 0) throw new IllegalArgumentException("amount <= 0");
            balance += amount;
        }

        boolean withdraw(double amount) {
            if (amount > balance) return false;
            balance -= amount;
            return true;
        }

        double getBalance() {
            return balance;
        }

        @Override
        public String toString() {
            return "%s: %.2f".formatted(owner, balance);
        }
    }

    public static void main(String[] args) {
        Account a = new Account("Alice", 100);
        Account b = new Account("Bob");

        a.deposit(50);
        System.out.println(a.withdraw(500));
        System.out.println(a.withdraw(30));
        b.deposit(20);

        System.out.println(a);
        System.out.println(b);
        System.out.println(a.getBalance() + b.getBalance());
    }
}
```

Expected output should be:

```
false
true
Alice: 120.00
Bob: 20.00
140.0
```

---

## Access Modifiers

| Modifier    | Visible from                                    |
| ----------- | ----------------------------------------------- |
| `public`    | Anywhere                                        |
| `protected` | Same package, plus subclasses in other packages |
| (none)      | Same package only. Called **package-private**   |
| `private`   | Inside the class only                           |

A top-level class can only be `public` or package-private. Modules can hide even `public` classes; see [Packaging and Deployment](63_packaging_and_deployment.md).

---

## No Destructors

Java frees objects automatically with the garbage collector (see [Memory and Garbage Collection](34_memory_and_garbage_collection.md)). There is no destructor.

`finalize()` is deprecated for removal since Java 18. To release files, sockets, or connections, implement `AutoCloseable` and use try-with-resources. See [AutoCloseable and try-with-resources](33_autocloseable.md).

---

## Gotchas

- With no modifier, a member is package-private, not `private` as in C#
- Writing any constructor removes the free default constructor, which can break code that calls `new Thing()`
- A `final` field must be set in every constructor, or the class does not compile
- Getters that return a mutable field (like a `List`) let callers change your object; return a copy or `List.copyOf`
- Calling an overridable method from a constructor can run subclass code before the subclass is ready
- Fields get default values, but local variables do not: using an unset local is a compile error

---

## Examples

- [26-01](examples/26-01_classes_and_objects.java): Classes and Objects

Run one with `java examples/26-01_classes_and_objects.java`. See [Examples](examples/README.md).
