# 27 - Static Members and Utility Classes

## Static Members

A `static` member belongs to the class, not to any one object. There is one copy, shared by all instances.

```java
class Counter {
    static int total;      // one copy for the whole class
    int id;                // one copy per object

    Counter() {
        total++;
        id = total;
    }
}

new Counter();
new Counter();
System.out.println(Counter.total);   // 2
```

Call static members through the class name: `Counter.total`, `Math.max(1, 2)`.

---

## Static Methods

A static method has no `this`. It can only use its parameters and other static members.

```java
class Temperature {
    static double toFahrenheit(double celsius) {
        return celsius * 9 / 5 + 32;
    }
}

double f = Temperature.toFahrenheit(100);   // 212.0
```

`main` is static, so the JVM can call it before any object exists.

---

## Utility Classes

Java has no `static class` keyword at the top level. A helper class full of static methods is written like this:

```java
final class MathUtil {
    private MathUtil() { }              // nobody can create one

    static final double GOLDEN = 1.618;

    static int square(int x) {
        return x * x;
    }
}
```

| Piece                 | Why                                                                       |
| --------------------- | ------------------------------------------------------------------------- |
| `final`               | Nobody can extend it                                                      |
| `private` constructor | Nobody can write `new MathUtil()`                                         |
| `static final` field  | A constant (see [Variables and Constants](07_variables_and_constants.md)) |

`Math`, `Collections`, `Arrays`, and `Objects` in the JDK follow this pattern.

---

## Static Initializer Block

Runs once, when the class is first used. Use it when a static field needs more than one line to set up.

```java
import java.util.HashMap;
import java.util.Map;

class Config {
    static final Map<String, String> DEFAULTS = new HashMap<>();

    static {
        DEFAULTS.put("env", "dev");
        DEFAULTS.put("port", "8080");
    }
}
```

For simple cases, `Map.of("env", "dev", "port", "8080")` is shorter and returns a map that cannot change.

---

## No Partial Classes

Java has no `partial` keyword. One top-level class lives in one file. Code generators extend a generated base class or write a separate class instead.

---

## Full Example

```java
public class Main {
    public static void main(String[] args) {
        new Counter();
        new Counter();
        System.out.println("Counters: " + Counter.total);
        System.out.println("100C is " + Temperature.toFahrenheit(100) + "F");
        System.out.println("square(5) = " + MathUtil.square(5));
    }
}

class Counter {
    static int total;
    Counter() { total++; }
}

class Temperature {
    static double toFahrenheit(double celsius) { return celsius * 9 / 5 + 32; }
}

final class MathUtil {
    private MathUtil() { }
    static int square(int x) { return x * x; }
}
```

Expected output should be:

```
Counters: 2
100C is 212.0F
square(5) = 25
```

---

## Gotchas

- Static state lives as long as the program and is shared across threads, so guard it with care
- Static methods cannot be overridden and are hard to swap out in tests; prefer passing objects in (see [Dependency Injection](56_dependency_injection.md))
- Calling a static member through an instance (`counter.total`) compiles but misleads readers; use the class name
- A static field is set up once per class loader, not once per object, so tests that change it affect each other
- A static initializer runs when the class is first touched, so keep it short and free of heavy work

---

## Examples

- [27-01](examples/27-01_static_members_and_utility_classes.java): Static members and utility classes

Run one with `java examples/27-01_static_members_and_utility_classes.java`. See [Examples](examples/README.md).
