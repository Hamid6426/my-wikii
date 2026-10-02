# 16 - Methods

## Basic Method

A **method** is a named block of code you can call. In Java every method lives inside a class. There are no free-standing functions.

```java
public class Hello {
    static void sayHello() {
        System.out.println("Hello!");
    }

    public static void main(String[] args) {
        sayHello();   // call it
    }
}
```

Expected output should be:

```
Hello!
```

---

## Return Type

```java
static int add(int a, int b) {
    return a + b;
}

int result = add(3, 4);   // 7
```

`void` means no return value. Every path through a non-`void` method must `return` a value, or the code does not compile.

---

## Parts of a Method

```java
public static int add(int a, int b) { ... }
```

| Part        | Here           | Meaning                                                               |
| ----------- | -------------- | --------------------------------------------------------------------- |
| Access      | `public`       | Who can call it. See [Classes and Objects](26_classes_and_objects.md) |
| `static`    | `static`       | Belongs to the class, not to an object                                |
| Return type | `int`          | What comes back, or `void`                                            |
| Name        | `add`          | A verb in camelCase                                                   |
| Parameters  | `int a, int b` | Inputs, each with a type                                              |

The name plus the parameter types form the **signature**. The return type is not part of it.

---

## Method Overloading

Same name, different parameter lists.

```java
static int add(int a, int b) { return a + b; }
static double add(double a, double b) { return a + b; }
static int add(int a, int b, int c) { return a + b + c; }

add(1, 2);       // int version
add(1.5, 2.0);   // double version
add(1, 2, 3);    // three-argument version
```

The compiler picks the version that fits the argument types best.

---

## No Default or Named Arguments

Java has no `greet(String name, String greeting = "Hello")`. Use an overload that calls the full version:

```java
static void greet(String name) {
    greet(name, "Hello");
}

static void greet(String name, String greeting) {
    System.out.println(greeting + ", " + name + "!");
}

greet("Alice");         // Hello, Alice!
greet("Bob", "Hi");     // Hi, Bob!
```

Arguments are always matched by position. For many optional settings, use a builder or a record (see [Design Patterns](58_design_patterns.md)).

---

## Static vs Instance Methods

**Static**: called on the class, no object needed.

```java
class MathHelper {
    static int square(int n) { return n * n; }
}

int result = MathHelper.square(5);   // 25
```

**Instance**: called on an object, and can use that object's fields.

```java
class Greeter {
    private final String prefix;

    Greeter(String prefix) { this.prefix = prefix; }

    void greet(String name) {
        System.out.println(prefix + ", " + name + "!");
    }
}

Greeter g = new Greeter("Hello");
g.greet("Alice");   // Hello, Alice!
```

---

## Java Is Always Pass-by-Value

A method gets a **copy** of each argument. For a primitive, that is a copy of the number. For an object, it is a copy of the **reference** (the address of the object), so both copies point at the same object.

```java
import java.util.ArrayList;
import java.util.List;

public class PassByValue {
    static void doubleIt(int x) {
        x *= 2;                      // changes only the local copy
    }

    static void addItem(List<String> list) {
        list.add("new");             // changes the shared object
    }

    static void replace(List<String> list) {
        list = new ArrayList<>();    // points the local copy somewhere else
        list.add("lost");
    }

    public static void main(String[] args) {
        int n = 5;
        doubleIt(n);
        System.out.println(n);

        List<String> items = new ArrayList<>();
        addItem(items);
        replace(items);
        System.out.println(items);
    }
}
```

Expected output should be:

```
5
[new]
```

Java has no `ref` or `out` parameters. To get more than one value back, see [Records and Equality](25_records_and_equality.md).

---

## final Parameters

`final` stops a parameter from being reassigned inside the method.

```java
static int area(final int width, final int height) {
    // width = 0;   // compile error
    return width * height;
}
```

It does not make the object itself read-only. A `final List` can still have items added.

---

## Recursion

A method that calls itself. It must have a **base case**, a point where it stops.

```java
static long factorial(int n) {
    if (n <= 1) return 1;
    return n * factorial(n - 1);
}

System.out.println(factorial(5));   // 120
```

---

## Helpers Inside a Method

Java has no local functions. For a helper that only one method needs, use a private method, or a lambda stored in a local variable:

```java
static int sumOfSquares(int[] numbers) {
    IntUnaryOperator square = x -> x * x;   // java.util.function.IntUnaryOperator
    int total = 0;
    for (int n : numbers) {
        total += square.applyAsInt(n);
    }
    return total;
}

System.out.println(sumOfSquares(new int[] {1, 2, 3, 4}));   // 30
```

A method can also declare a local `record` or `class` when it needs a small type of its own. See [Lambdas and Functional Interfaces](39_lambdas_and_functional_interfaces.md).

---

## Gotchas

- With no access modifier, a method is **package-private**: visible only to classes in the same package
- A `static` method cannot use instance fields or call instance methods without an object
- Overloads that differ only by return type do not compile
- Reassigning a parameter never affects the caller's variable, even when the parameter is an object
- Deep recursion throws `StackOverflowError`; Java does not optimize tail calls
- Name methods with verbs in camelCase: `getUser`, `calculateTotal`

---

## Examples

- [16-01](examples/16-01_basic_method.java): Basic method
- [16-02](examples/16-02_java_is_always_pass_by_value.java): Java is always pass by value

Run one with `java examples/16-01_basic_method.java`. See [Examples](examples/README.md).
