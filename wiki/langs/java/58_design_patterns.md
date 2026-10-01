# 58 - Design Patterns

## What is a Design Pattern

A named, proven way to solve a problem that keeps coming up. Patterns give developers a shared vocabulary: saying "use a strategy here" explains the design in four words.

They are not libraries. You write them yourself with plain classes and interfaces. See [Conventions and Clean Code](57_conventions_and_clean_code.md) for the SOLID rules that most patterns follow.

| Group      | Question it answers           | Patterns in this lesson                             |
| ---------- | ----------------------------- | --------------------------------------------------- |
| Creational | How do I create objects?      | Singleton, Factory, Builder, Prototype              |
| Structural | How do I connect objects?     | Adapter, Decorator, Facade                          |
| Behavioral | How do objects work together? | Strategy, Observer, Command, Template Method, State |
| Other      | Common structure in Java apps | Repository, Null Object                             |

The snippets below show the types and then a few lines that use them. Put those lines inside a `main` method to run them.

---

## Singleton

One instance for the whole program. The simplest safe version in Java is an enum with one constant.

```java
enum AppSettings {
    INSTANCE;

    private final String env = "dev";

    String env() { return env; }
}

AppSettings a = AppSettings.INSTANCE;
AppSettings b = AppSettings.INSTANCE;
System.out.println(a == b);   // true
```

The JVM creates enum constants once, safely across threads. When the singleton must be a class, use the holder idiom:

```java
final class Registry {
    private Registry() { }

    private static class Holder {
        static final Registry INSTANCE = new Registry();   // created on first use
    }

    static Registry getInstance() { return Holder.INSTANCE; }
}
```

The nested `Holder` class loads only when `getInstance()` first runs, and class loading is thread-safe. In real apps, prefer one object created in `main` and passed around with [Dependency Injection](56_dependency_injection.md). It is easier to test.

---

## Factory

One place that decides which class to create.

```java
sealed interface Shape permits Circle, Square { double area(); }
record Circle(double radius) implements Shape { public double area() { return Math.PI * radius * radius; } }
record Square(double side) implements Shape { public double area() { return side * side; } }

final class Shapes {
    private Shapes() { }

    static Shape create(String kind, double size) {
        return switch (kind) {
            case "circle" -> new Circle(size);
            case "square" -> new Square(size);
            default -> throw new IllegalArgumentException("Unknown shape: " + kind);
        };
    }
}

Shape shape = Shapes.create("square", 3);
System.out.println(shape.area());   // 9.0
```

Callers depend on `Shape` and never name `Square` directly. Adding a shape changes only the factory. The JDK uses static factory methods everywhere: `List.of(...)`, `Path.of(...)`, `Optional.of(...)`.

---

## Builder

Build a complex object step by step. Each method returns the builder, so calls chain. The finished object can be immutable.

```java
import java.util.LinkedHashMap;
import java.util.Map;

final class Request {
    private final String url;
    private final String method;
    private final Map<String, String> headers;

    private Request(Builder b) {
        this.url = b.url;
        this.method = b.method;
        this.headers = Map.copyOf(b.headers);
    }

    static Builder builder(String url) { return new Builder(url); }

    @Override
    public String toString() { return method + " " + url + " " + headers; }

    static final class Builder {
        private final String url;
        private String method = "GET";
        private final Map<String, String> headers = new LinkedHashMap<>();

        private Builder(String url) { this.url = url; }

        Builder method(String method) { this.method = method; return this; }
        Builder header(String key, String value) { headers.put(key, value); return this; }
        Request build() { return new Request(this); }
    }
}

Request request = Request.builder("https://example.com")
        .method("POST")
        .header("Accept", "application/json")
        .build();
System.out.println(request);   // POST https://example.com {Accept=application/json}
```

Use it when a constructor would need many optional parameters. Java has no named or default arguments, so builders are common. See [Nested, Inner and Anonymous Classes](28_nested_inner_and_anonymous_classes.md).

---

## Prototype

Make a copy of an existing object and change a few things. For a record, write a small "wither" method that returns a changed copy.

```java
record Config(String env, int retries) {
    Config withRetries(int retries) { return new Config(env, retries); }
}

var dev = new Config("dev", 3);
var strict = dev.withRetries(5);

System.out.println(dev);      // Config[env=dev, retries=3]
System.out.println(strict);   // Config[env=dev, retries=5]
```

A built-in `with` expression for records has been proposed, but it is not part of Java 25. See [Records and Equality](25_records_and_equality.md).

---

## Adapter

Make an old or foreign class fit the interface your code expects.

```java
interface Logger { void log(String message); }

class LegacyLogger {
    void writeEntry(String level, String text) { System.out.println("[" + level + "] " + text); }
}

class LegacyLoggerAdapter implements Logger {
    private final LegacyLogger legacy;

    LegacyLoggerAdapter(LegacyLogger legacy) { this.legacy = legacy; }

    @Override
    public void log(String message) { legacy.writeEntry("INFO", message); }
}

Logger logger = new LegacyLoggerAdapter(new LegacyLogger());
logger.log("adapted");   // [INFO] adapted
```

When the target interface has one method, a lambda is a complete adapter:

```java
var legacy = new LegacyLogger();
Logger quick = message -> legacy.writeEntry("WARN", message);
quick.log("lambda adapter");   // [WARN] lambda adapter
```

---

## Decorator

Wrap an object to add behavior without changing it. The wrapper has the same interface as what it wraps.

```java
public class Decorator {
    public static void main(String[] args) {
        MessageSender sender = new LoggingSender(new EmailSender());
        sender.send("hi");
    }
}

interface MessageSender {
    void send(String message);
}

class EmailSender implements MessageSender {
    @Override
    public void send(String message) {
        System.out.println("email: " + message);
    }
}

class LoggingSender implements MessageSender {
    private final MessageSender inner;

    LoggingSender(MessageSender inner) {
        this.inner = inner;
    }

    @Override
    public void send(String message) {
        System.out.println("sending...");
        inner.send(message);
        System.out.println("sent");
    }
}
```

Expected output should be:

```
sending...
email: hi
sent
```

Decorators stack: `new RetrySender(new LoggingSender(new EmailSender()))`. Java I/O is built this way: `new BufferedReader(new FileReader(file))`. See [File I/O: Streams](47_file_io_streams.md).

---

## Facade

One simple class in front of several complicated ones.

```java
class OrderFacade {
    private final Inventory inventory = new Inventory();
    private final Payment payment = new Payment();

    String place(String item) {
        return inventory.inStock(item) && payment.charge() ? "ordered " + item : "failed";
    }
}
```

Callers use `place` and know nothing about inventory or payment.

---

## Strategy

Swap an algorithm at run time by putting it behind an interface.

```java
interface Discount { double apply(double price); }

record PercentOff(int percent) implements Discount {
    public double apply(double price) { return price - price * percent / 100; }
}

class Cart {
    private final Discount discount;

    Cart(Discount discount) { this.discount = discount; }

    double total(double price) { return discount.apply(price); }
}

System.out.println(new Cart(new PercentOff(10)).total(200));   // 180.0
System.out.println(new Cart(price -> price).total(200));       // 200.0
System.out.println(new Cart(price -> price - 5).total(200));   // 195.0
```

`Discount` has one abstract method, so it is a functional interface and a lambda can be the strategy. `Comparator` is the strategy you will use most: `list.sort(Comparator.comparing(User::name))`. See [Lambdas and Functional Interfaces](39_lambdas_and_functional_interfaces.md).

---

## Observer

Objects subscribe to be told when something changes. In Java you keep a list of listeners and call each one.

```java
import java.util.ArrayList;
import java.util.List;
import java.util.function.DoubleConsumer;

public class Observer {
    public static void main(String[] args) {
        var stock = new Stock();
        stock.onPriceChanged(price -> System.out.println("price now " + price));

        stock.setPrice(10);
        stock.setPrice(10);   // no change, no event
        stock.setPrice(12);
    }
}

class Stock {
    private final List<DoubleConsumer> listeners = new ArrayList<>();
    private double price;

    void onPriceChanged(DoubleConsumer listener) {
        listeners.add(listener);
    }

    void setPrice(double newPrice) {
        if (newPrice == price) return;
        price = newPrice;
        listeners.forEach(listener -> listener.accept(newPrice));
    }
}
```

Expected output should be:

```
price now 10.0
price now 12.0
```

A **listener** is an observer with a name that fits the domain. The JDK convention, used by Swing and JavaBeans, is:

- the event class extends `java.util.EventObject`, which carries the publisher as `getSource()`
- the listener interface extends the marker interface `java.util.EventListener` (it has no methods, it only labels the type)
- a private `List` holds the listeners, with `addXListener` and `removeXListener` methods, so only the publisher can fire

Use `CopyOnWriteArrayList` for the listener list so it is safe to add and remove while events are being fired. Two traps: keep the reference when you register a lambda, because `remove` matches by `equals` and two identical-looking lambdas are different objects; and remove a listener you no longer need, or the publisher keeps it (and everything it references) alive, a common leak (see [Memory and Garbage Collection](34_memory_and_garbage_collection.md)).

For a stream of events between threads, use `java.util.concurrent.Flow` and see [Threading](53_threading.md).

---

## Command

Turn an action into an object. That lets you queue it, log it, or undo it.

```java
import java.util.ArrayDeque;
import java.util.Deque;

class Document { final StringBuilder text = new StringBuilder(); }

interface Command {
    void execute();
    void undo();
}

record AppendCommand(Document doc, String text) implements Command {
    public void execute() { doc.text.append(text); }
    public void undo() { doc.text.setLength(doc.text.length() - text.length()); }
}

var doc = new Document();
Deque<Command> history = new ArrayDeque<>();

Command cmd = new AppendCommand(doc, "Hello");
cmd.execute();
history.push(cmd);

cmd = new AppendCommand(doc, " World");
cmd.execute();
history.push(cmd);

System.out.println(doc.text);   // Hello World

history.pop().undo();
System.out.println(doc.text);   // Hello
```

`ArrayDeque` used with `push` and `pop` is a stack. See [Queue, Deque and More Collections](37_queue_deque_and_more_collections.md).

---

## Template Method

A base class fixes the steps. Child classes fill in some of them.

```java
abstract class Report {
    final void run() {          // final: children cannot change the order
        load();
        format();
        System.out.println("saved");
    }

    void load() { System.out.println("loaded"); }
    abstract void format();
}

class CsvReport extends Report {
    @Override
    void format() { System.out.println("csv"); }
}

new CsvReport().run();   // loaded, csv, saved
```

---

## State

An object changes behavior when its state changes. In Java, each enum constant can have its own method body.

```java
enum DoorState {
    CLOSED { DoorState press() { return OPEN; } },
    OPEN   { DoorState press() { return CLOSED; } };

    abstract DoorState press();
}

DoorState state = DoorState.CLOSED;
state = state.press();
System.out.println(state);   // OPEN
```

For many states with their own data, make each state a record that implements a `sealed` interface. See [Enums](09_enums.md).

---

## Repository

Hide where data is stored behind an interface. The rest of the app does not care if it is a file, a database, or memory.

```java
import java.util.HashMap;
import java.util.Map;
import java.util.Optional;

interface Entity { int id(); }

interface Repository<T extends Entity> {
    void add(T item);
    Optional<T> get(int id);
}

class InMemoryRepository<T extends Entity> implements Repository<T> {
    private final Map<Integer, T> items = new HashMap<>();

    public void add(T item) { items.put(item.id(), item); }
    public Optional<T> get(int id) { return Optional.ofNullable(items.get(id)); }
}

record User(int id, String name) implements Entity { }

Repository<User> repo = new InMemoryRepository<>();
repo.add(new User(1, "Ann"));
System.out.println(repo.get(1).map(User::name).orElse("none"));   // Ann
System.out.println(repo.get(2).map(User::name).orElse("none"));   // none
```

In tests you pass a fake repository. See [Mocking and Fixtures](60_mocking_and_fixtures.md) and [Generics](54_generics.md).

---

## Null Object

Instead of `null`, pass an object that does nothing. Callers need no `null` checks.

```java
interface Log {
    Log NONE = message -> { };   // the null object: does nothing
    void write(String message);
}

class Service {
    private final Log log;

    Service(Log log) { this.log = log; }

    String run() {
        log.write("running");   // safe even with Log.NONE
        return "ok";
    }
}

System.out.println(new Service(Log.NONE).run());   // ok
```

---

## Patterns You Already Used

| Pattern   | Where in Java                                                                    |
| --------- | -------------------------------------------------------------------------------- |
| Iterator  | `Iterator`, `Iterable`, and the for-each loop ([Iterators](38_iterators.md))     |
| Observer  | Listeners, such as Swing listeners and `PropertyChangeSupport`                   |
| Singleton | `Runtime.getRuntime()`, enum constants                                           |
| Factory   | `List.of`, `Path.of`, `Executors.newFixedThreadPool`                             |
| Strategy  | `Comparator`, any functional interface parameter                                 |
| Decorator | `BufferedReader` wraps a `Reader`, `Collections.unmodifiableList` wraps a `List` |
| Builder   | `StringBuilder`, `HttpRequest.newBuilder()` ([HTTP Client](52_http_client.md))   |
| Adapter   | `Arrays.asList` shows an array as a `List`                                       |

---

## Picking a Pattern

| Problem                                      | Try         |
| -------------------------------------------- | ----------- |
| Long `if/else` or `switch` choosing behavior | Strategy    |
| Which class to create depends on input       | Factory     |
| Constructor with many optional values        | Builder     |
| Add logging or retries around existing code  | Decorator   |
| Old API does not match your interface        | Adapter     |
| Many classes behind one entry point          | Facade      |
| Others must react when something changes     | Observer    |
| Need undo or a queue of actions              | Command     |
| `null` checks everywhere                     | Null Object |

---

## Gotchas

- Do not add a pattern before you feel the problem. Three lines of plain code beat an unused factory
- A pattern name is not a goal. If a simple `if` solves it, use the `if`
- Singletons are global state in disguise: they hide dependencies and make tests share data
- The classic "double-checked locking" singleton is easy to get wrong without `volatile`. Use the enum or holder idiom instead
- Many patterns exist because older Java lacked features. Lambdas, records, enums, and sealed types now replace whole class hierarchies
- Patterns from other books may use names that differ slightly. The idea matters more than the exact class names

---

## Examples

- [58-01](examples/58-01_decorator.java): Decorator
- [58-02](examples/58-02_observer.java): Observer

Run one with `java examples/58-01_decorator.java`. See [Examples](examples/README.md).
