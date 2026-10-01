# 56 - Dependency Injection

## What is Dependency Injection

A dependency is an object your class needs to do its job, such as a mail sender or a database repository. Dependency injection (DI) means the class receives its dependencies from outside instead of creating them itself.

| Without DI                                  | With DI                                       |
| ------------------------------------------- | --------------------------------------------- |
| `new` inside the class                      | Passed in through the constructor             |
| Tied to one concrete class                  | Depends on an interface                       |
| Hard to test: real email gets sent          | Easy to test: pass a fake                     |
| Swapping the sender means editing the class | Swapping the sender means passing another one |

---

## The Problem Without DI

```java
class OrderService {
    private final SmtpEmailService email = new SmtpEmailService();   // tightly coupled

    void placeOrder(Order order) {
        // process...
        email.send(order.email(), "Order confirmed");
    }
}
```

`OrderService` always talks to a real mail server. A test cannot stop it, and you cannot swap in another sender.

---

## The Solution: Constructor Injection

Depend on an interface and take it in the constructor.

```java
public class Main {
    public static void main(String[] args) {
        EmailService email = new ConsoleEmailService();   // pick the real one here
        OrderService orders = new OrderService(email);     // and pass it in

        orders.placeOrder(new Order(7, "ann@example.com"));
    }
}

record Order(int id, String email) {}

interface EmailService {
    void send(String to, String subject);
}

class ConsoleEmailService implements EmailService {
    @Override
    public void send(String to, String subject) {
        System.out.println("Sending '" + subject + "' to " + to);
    }
}

class OrderService {
    private final EmailService email;

    OrderService(EmailService email) {   // injected
        this.email = email;
    }

    void placeOrder(Order order) {
        email.send(order.email(), "Order " + order.id() + " confirmed");
    }
}
```

Expected output should be:

```
Sending 'Order 7 confirmed' to ann@example.com
```

The field is `final`, so the dependency is set once and can never be `null` after construction. See [Interfaces and Abstract Classes](32_interfaces_and_abstract_classes.md).

---

## Wiring by Hand: the Composition Root

The composition root is the one place, usually `main`, that creates the real objects and connects them. Every other class only asks for what it needs.

```java
import java.util.ArrayList;
import java.util.List;

public class Root {
    public static void main(String[] args) {
        // Composition root: the one place that knows the concrete classes
        AppConfig config = new AppConfig("smtp.example.com");          // shared by everyone
        EmailService email = new SmtpEmailService(config);
        OrderRepository repo = new InMemoryOrderRepository();
        OrderService orders = new OrderService(repo, email);

        orders.placeOrder(new Order(1, "ann@example.com"));
        orders.placeOrder(new Order(2, "bob@example.com"));
        System.out.println(repo.count() + " orders saved");
    }
}

record AppConfig(String smtpHost) {}
record Order(int id, String email) {}

interface EmailService { void send(String to, String subject); }
interface OrderRepository { void save(Order order); int count(); }

class SmtpEmailService implements EmailService {
    private final AppConfig config;

    SmtpEmailService(AppConfig config) {
        this.config = config;
    }

    @Override
    public void send(String to, String subject) {
        System.out.println("[" + config.smtpHost() + "] " + subject + " -> " + to);
    }
}

class InMemoryOrderRepository implements OrderRepository {
    private final List<Order> orders = new ArrayList<>();

    @Override public void save(Order order) { orders.add(order); }
    @Override public int count() { return orders.size(); }
}

class OrderService {
    private final OrderRepository repo;
    private final EmailService email;

    OrderService(OrderRepository repo, EmailService email) {
        this.repo = repo;
        this.email = email;
    }

    void placeOrder(Order order) {
        repo.save(order);
        email.send(order.email(), "Order " + order.id() + " confirmed");
    }
}
```

Expected output should be:

```
[smtp.example.com] Order 1 confirmed -> ann@example.com
[smtp.example.com] Order 2 confirmed -> bob@example.com
2 orders saved
```

For small and medium programs, wiring by hand is enough. It is plain Java, and the compiler checks every connection.

---

## Lifetimes

A lifetime is how long one created object is reused.

| Lifetime      | By hand                                        | Use for                               |
| ------------- | ---------------------------------------------- | ------------------------------------- |
| Singleton     | Create once in `main`, pass the same object    | Config, caches, thread-safe services  |
| New each time | Pass a `Supplier<T>` and call `get()` each use | Short-lived, stateful helpers         |
| Per request   | Create at the start of each request            | Database transactions, per-user state |

```java
import java.util.function.Supplier;

class ReportJob {
    private final Supplier<ReportWriter> writers;   // a factory, not one writer

    ReportJob(Supplier<ReportWriter> writers) {
        this.writers = writers;
    }

    void run() {
        ReportWriter writer = writers.get();        // a fresh writer each run
        writer.write("report");
    }
}

interface ReportWriter { void write(String text); }

// In main: new ReportJob(() -> new FileReportWriter());
```

`Supplier<T>` is a built-in functional interface with one method, `get()`. See [Lambdas and Functional Interfaces](39_lambdas_and_functional_interfaces.md).

---

## Other Ways to Inject

| Kind        | How                                     | When                                          |
| ----------- | --------------------------------------- | --------------------------------------------- |
| Constructor | Parameter of the constructor            | The default. Required dependencies            |
| Setter      | A `setX(...)` method called after `new` | Optional dependencies with a safe default     |
| Method      | A parameter of one method only          | Only one method needs it, or it changes a lot |

```java
class ReportGenerator {
    private Logger logger = Logger.NONE;      // setter injection with a safe default

    void setLogger(Logger logger) {
        this.logger = logger;
    }

    void generate(Clock clock) {              // method injection
        logger.log("Report at " + clock.instant());
    }
}

interface Logger {
    Logger NONE = message -> { };             // does nothing
    void log(String message);
}
```

Here `Clock` is `java.time.Clock`. Passing it in lets a test fix the time. See [Dates and Times](21_dates_and_times.md).

---

## Testing with DI

Replace the real dependency with a fake: a small class written for the test.

```java
import java.util.ArrayList;
import java.util.List;

public class FakeTest {
    public static void main(String[] args) {
        var fake = new FakeEmailService();
        var service = new OrderService(fake);

        service.placeOrder(new Order(7, "ann@example.com"));

        System.out.println(fake.sent);   // [Order 7 confirmed]
    }
}

record Order(int id, String email) {}

interface EmailService { void send(String to, String subject); }

class FakeEmailService implements EmailService {
    final List<String> sent = new ArrayList<>();

    @Override
    public void send(String to, String subject) {
        sent.add(subject);   // remember instead of sending
    }
}

class OrderService {
    private final EmailService email;
    OrderService(EmailService email) { this.email = email; }
    void placeOrder(Order order) { email.send(order.email(), "Order " + order.id() + " confirmed"); }
}
```

No mail is sent, and the test can read what would have been sent. Real tests use JUnit ([Testing](59_testing.md)) and often Mockito ([Mocking and Fixtures](60_mocking_and_fixtures.md)).

---

## DI Frameworks: Spring and Guice

The JDK has no DI container. A container is a library that reads your classes, creates the objects, and wires them for you. Use one when the object graph gets large or a framework already brings one.

| Framework    | Package                        | Marks a dependency with                       |
| ------------ | ------------------------------ | --------------------------------------------- |
| Spring       | `org.springframework` (Spring) | `@Component` or `@Service` on the class       |
| Google Guice | `com.google.inject` (Guice)    | `@Inject` on the constructor, plus a `Module` |
| Jakarta CDI  | `jakarta.inject` (Jakarta EE)  | `@Inject` on the constructor                  |

```java
import org.springframework.stereotype.Service;

@Service
class OrderService {
    private final EmailService email;

    OrderService(EmailService email) {   // Spring finds an EmailService bean and passes it
        this.email = email;
    }
}
```

The class still uses constructor injection. Only the wiring in `main` moves into the framework. Spring Boot is covered in [Common Frameworks](65_frameworks.md).

---

## Gotchas

- Constructor injection is the default: the dependencies are visible, and the object cannot exist without them
- A singleton that holds a short-lived object keeps it alive forever (a captive dependency). Pass a `Supplier` instead
- Avoid a service locator: a global `Registry.get(EmailService.class)` call inside a class hides what it needs
- A constructor with seven parameters is a sign the class does too much. Split it. See [Conventions and Clean Code](57_conventions_and_clean_code.md)
- Do not inject plain values like `int` everywhere. Group settings into one `record` such as `AppConfig`
- Field injection (`@Autowired` on a field in Spring) works but hides dependencies and blocks `final`. Prefer the constructor

---

## Examples

- [56-01](examples/56-01_the_solution_constructor_injection.java): The solution constructor injection
- [56-02](examples/56-02_wiring_by_hand_the_composition_root.java): Wiring by hand the composition root
- [56-03](examples/56-03_testing_with_di.java): Testing with di

Run one with `java examples/56-01_the_solution_constructor_injection.java`. See [Examples](examples/README.md).
