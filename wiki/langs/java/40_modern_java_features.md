# 40 - Modern Java Features

Newer syntax that makes code shorter. Each feature lists the Java version that made it final (a standard, non-preview feature). The full version table is in [Java Version History](64_java_version_history.md).

A **preview feature** is shipped early for feedback. It needs the `--enable-preview` flag and can change. This lesson covers only final features.

## var (Java 10)

Let the compiler work out the type of a local variable from its value.

```java
var count = 10;                          // int
var names = new ArrayList<String>();     // ArrayList<String>
var ages = new HashMap<String, Integer>();

for (var entry : ages.entrySet()) {
    System.out.println(entry.getKey() + " " + entry.getValue());
}
```

`var` works only for local variables that have a value right away. It does not work for fields, parameters, or return types. The variable is still strongly typed: `count` is an `int` forever. See [Variables and Constants](07_variables_and_constants.md).

---

## Switch Expressions (Java 14)

A `switch` that returns a value. Arrow cases do not fall through, so there is no `break`.

```java
String kind = switch (day) {
    case SAT, SUN -> "weekend";
    default -> "weekday";
};

int letters = switch (day) {
    case MON, FRI, SUN -> 6;
    case TUE -> 7;
    default -> {
        String name = day.name();
        yield name.length();   // yield returns a value from a block
    }
};
```

A switch expression must cover every value. For an enum, list them all or add `default`.

---

## Text Blocks (Java 15)

A multi-line string between `"""` marks. No `\n` or escaped quotes needed.

```java
public class TextBlocks {
    public static void main(String[] args) {
        String json = """
            {
              "name": "Alice",
              "age": 30
            }
            """;
        System.out.print(json);

        String sql = """
            SELECT name \
            FROM users \
            WHERE age > %d""".formatted(18);
        System.out.println(sql);
    }
}
```

Expected output should be:

```
{
  "name": "Alice",
  "age": 30
}
SELECT name FROM users WHERE age > 18
```

- The indentation shared by all lines (set by the closing `"""`) is removed
- A `\` at the end of a line joins it with the next line

More in [Strings](20_strings.md).

---

## Records (Java 16)

A short class for plain data. The compiler writes the constructor, getters, `equals`, `hashCode`, and `toString`.

```java
record Point(int x, int y) {
    Point {                                   // compact constructor: validate here
        if (x < 0 || y < 0) throw new IllegalArgumentException("negative");
    }

    double distance() {
        return Math.sqrt(x * x + y * y);
    }
}

var p = new Point(3, 4);
System.out.println(p);                          // Point[x=3, y=4]
System.out.println(p.x());                      // 3 (getter has no "get" prefix)
System.out.println(p.distance());               // 5.0
System.out.println(p.equals(new Point(3, 4)));  // true
```

Fields are `final`. Full details in [Records and Equality](25_records_and_equality.md).

---

## Pattern Matching for instanceof (Java 16)

Test a type and get a typed variable in one step.

```java
Object o = "hello";

if (o instanceof String s && s.length() > 3) {
    System.out.println(s.toUpperCase());   // HELLO
}
```

More in [Pattern Matching](14_pattern_matching.md).

---

## Sealed Types (Java 17)

A sealed class or interface lists exactly which types may extend it. The compiler then knows every case.

```java
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
```

Expected output should be:

```
9.0
10.0
```

| Modifier on a permitted subtype | Meaning                          |
| ------------------------------- | -------------------------------- |
| `final`                         | No more subtypes (records are)   |
| `sealed`                        | Has its own `permits` list       |
| `non-sealed`                    | Open again: anyone may extend it |

Add a new `Shape` and every `switch` that misses it stops compiling. That is the point.

---

## Pattern Matching for switch and Record Patterns (Java 21)

`case` can test types, take records apart, and add conditions with `when`. The `Sealed` example above uses both. See [Pattern Matching](14_pattern_matching.md).

```java
String describe(Object o) {
    return switch (o) {
        case null -> "null";
        case Integer i when i > 100 -> "big number";
        case Integer i -> "number " + i;
        case String s -> "text " + s;
        default -> "something else";
    };
}
```

---

## Virtual Threads (Java 21)

A **virtual thread** is a very cheap thread managed by the JVM, not the operating system. You can run millions. Blocking calls such as `sleep` or a network read free the real thread underneath.

```java
import java.time.Duration;
import java.util.concurrent.Executors;
import java.util.concurrent.atomic.AtomicInteger;

public class Virtual {
    public static void main(String[] args) {
        var done = new AtomicInteger();

        try (var executor = Executors.newVirtualThreadPerTaskExecutor()) {
            for (int i = 0; i < 10_000; i++) {
                executor.submit(() -> {
                    Thread.sleep(Duration.ofMillis(100));   // blocking is cheap here
                    return done.incrementAndGet();
                });
            }
        }   // close() waits for every task

        System.out.println(done.get());
    }
}
```

Expected output should be:

```
10000
```

10,000 tasks that each sleep 100 ms finish in well under a second. See [Threading](53_threading.md).

---

## Sequenced Collections (Java 21)

Lists, deques, and ordered sets and maps share one set of methods for the first and last element.

```java
List<String> list = new ArrayList<>(List.of("a", "b", "c"));

list.getFirst();      // a
list.getLast();       // c
list.reversed();      // [c, b, a]
list.addFirst("z");   // [z, a, b, c]
```

Before this, the last item was `list.get(list.size() - 1)`. See [Collections](36_collections.md).

---

## Unnamed Variables (Java 22)

Write `_` for a variable you must declare but never use.

```java
for (var _ : List.of("a", "b", "c")) total++;

if (o instanceof Pair(String left, _)) {    // ignore the second part
    System.out.println(left);
}

try {
    Integer.parseInt("x");
} catch (NumberFormatException _) {
    System.out.println("not a number");
}
```

---

## Compact Source Files and Instance main (Java 25)

A small program no longer needs a class or `static`. The `IO` class gives simple console methods.

```java
void main() {
    String name = IO.readln("Name: ");
    IO.println("Hello, " + name);
}
```

Save as `Hello.java` and run `java Hello.java`. Typing `Bob` prints `Hello, Bob`. A compact file also imports all of `java.base` (the core module) for you, so `List` and `Map` need no import. See [Basics of a Program](04_basics_of_a_program.md).

---

## Flexible Constructor Bodies (Java 25)

Code can run before `super(...)` or `this(...)`, as long as it does not use the object being built. Good for checking arguments.

```java
class Employee extends Person {
    Employee(String name) {
        if (name.isBlank()) throw new IllegalArgumentException("name is blank");   // before super()
        super(name.strip());
    }
}
```

Before Java 25, `super(...)` had to be the first line.

---

## Module Imports (Java 25)

Import every public type a module exports in one line.

```java
import module java.base;   // java.util, java.io, java.time, and more

List<Path> files = new ArrayList<>();
```

If two packages have a class with the same name (such as `java.util.List` and `java.awt.List`), add a normal import to pick one.

---

## Features Covered Elsewhere

| Feature                                  | Version | Lesson                                                                       |
| ---------------------------------------- | ------- | ---------------------------------------------------------------------------- |
| Lambdas and method references            | 8       | [Lambdas and Functional Interfaces](39_lambdas_and_functional_interfaces.md) |
| Streams                                  | 8       | [Streams](41_streams.md)                                                     |
| `Optional`                               | 8       | [Null and Optional](24_null_and_optional.md)                                 |
| Default and static interface methods     | 8       | [Interfaces and Abstract Classes](32_interfaces_and_abstract_classes.md)     |
| try-with-resources on existing variables | 9       | [AutoCloseable and try-with-resources](33_autocloseable.md)                  |
| `List.of`, `Map.of`                      | 9       | [Collections](36_collections.md)                                             |
| `HttpClient`                             | 11      | [HTTP Client](52_http_client.md)                                             |
| Helpful `NullPointerException` messages  | 14      | [Debugging](43_debugging.md)                                                 |
| UTF-8 as the default charset             | 18      | [File I/O: Read and Write](45_file_io_read_write.md)                         |
| Stream gatherers                         | 24      | [Streams](41_streams.md)                                                     |
| Scoped values                            | 25      | [Threading](53_threading.md)                                                 |

---

## Gotchas

- `var` with a diamond gives a weak type: `var list = new ArrayList<>();` is an `ArrayList<Object>`. Put the type inside the brackets
- `var x = null;` and `var x;` do not compile, because there is no value to read the type from
- A switch expression over a sealed type needs no `default`. Adding one hides the compile error you want when a new subtype appears
- Record fields are `final`, but a field that holds a list can still have its contents changed. Copy it with `List.copyOf` in the compact constructor
- Check your Java version with `java -version`. A feature from a newer version fails to compile with a clear "not supported in -source N" error

---

## Examples

- [40-01](examples/40-01_text_blocks_java_15.java): Text blocks java 15
- [40-02](examples/40-02_sealed_types_java_17.java): Sealed types java 17
- [40-03](examples/40-03_virtual_threads_java_21.java): Virtual threads java 21

Run one with `java examples/40-01_text_blocks_java_15.java`. See [Examples](examples/README.md).
