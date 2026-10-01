# 57 - Conventions and Clean Code

## Why Conventions Matter

Code is read far more than it is written. When everyone follows the same rules, any Java file looks familiar and mistakes stand out. Java naming rules are older than most languages' rules and almost every project follows them.

---

## Naming

| Kind                      | Style                      | Example                         |
| ------------------------- | -------------------------- | ------------------------------- |
| Class, record, enum       | PascalCase                 | `OrderService`, `Point`         |
| Interface                 | PascalCase, no `I` prefix  | `OrderRepository`, `Comparable` |
| Method                    | camelCase, usually a verb  | `calculateTotal`, `sendEmail`   |
| Local variable, parameter | camelCase                  | `orderCount`                    |
| Field                     | camelCase, no underscore   | `logger`, `maxSize`             |
| Constant (`static final`) | UPPER_SNAKE_CASE           | `MAX_RETRIES`                   |
| Enum constant             | UPPER_SNAKE_CASE           | `Status.IN_PROGRESS`            |
| Package                   | all lowercase, reverse DNS | `com.example.shop.order`        |
| Type parameter            | One capital letter         | `T`, `E`, `K`, `V`              |
| Getter, setter            | `get` / `set` + name       | `getName()`, `setName(...)`     |
| Boolean getter            | `is` / `has` / `can`       | `isActive()`, `hasItems()`      |
| Record component accessor | Just the name              | `point.x()`, not `point.getX()` |

PascalCase capitalizes every word (`OrderService`). camelCase does the same except the first word (`orderService`). See [Variables and Constants](07_variables_and_constants.md) and [Packages and Imports](06_packages_and_imports.md).

Good names say what a thing is or does:

```java
// Hard to read
int d;
boolean check(User u) { return u.d() > 30; }

// Clear
int daysSinceLogin;
boolean isEligibleForDiscount(User user) { return user.daysSinceLogin() > 30; }
```

| Rule                                        | Example                              |
| ------------------------------------------- | ------------------------------------ |
| Methods are verbs                           | `calculateTotal`, `sendEmail`        |
| Booleans read like a yes/no question        | `isValid`, `hasItems`, `canEdit`     |
| Collections are plural                      | `users`, `orderLines`                |
| No abbreviations unless everyone knows them | `customer`, not `cust`               |
| No type in the name                         | `users`, not `userList`              |
| Acronyms as words                           | `HttpClient`, `parseJson`, `XmlUtil` |

---

## Layout

- One top-level public class per file, and the file has the class name: `OrderService.java`
- The folder path matches the package: `com/example/shop/OrderService.java` holds `package com.example.shop;`
- Order inside a class: constants, static fields, instance fields, constructors, methods
- Opening brace on the same line (the Java style, unlike C#)
- Four spaces for indentation, no tabs (Google style uses two; follow your project)
- Use braces for every block that spans lines. Many teams require them even for a one-line `if`

```java
package com.example.shop;

import java.util.List;

public class OrderService {
    private static final int MAX_ITEMS = 50;

    private final OrderRepository repository;

    public OrderService(OrderRepository repository) {
        this.repository = repository;
    }

    public List<Order> findOpen() {
        if (repository == null) {
            return List.of();
        }
        return repository.findByStatus(Status.OPEN);
    }
}
```

Avoid wildcard imports such as `import java.util.*;`. They hide where a type comes from and can clash.

---

## Formatters and Checkers

The JDK has no built-in formatter. These tools are the common choice. Each is a separate download or build plugin.

| Tool               | What it does                                                   |
| ------------------ | -------------------------------------------------------------- |
| `.editorconfig`    | A file that tells every editor the indent size and line ends   |
| google-java-format | Rewrites code to the Google Java Style, no options to argue    |
| Spotless           | Maven and Gradle plugin that runs a formatter in the build     |
| Checkstyle         | Reports naming and layout rules you choose                     |
| PMD, SpotBugs      | Find likely bugs: unused code, empty `catch`, null mistakes    |
| Error Prone        | Compiler plugin from Google that turns common bugs into errors |

A minimal `.editorconfig` at the project root:

```ini
root = true

[*.java]
indent_style = space
indent_size = 4
end_of_line = lf
insert_final_newline = true
```

The compiler itself has useful warnings. Turn them all on:

```bash
javac -Xlint:all -Werror Main.java   # every warning, and fail the build on one
```

In Maven, set `<maven.compiler.showWarnings>true</maven.compiler.showWarnings>` and pass `-Xlint:all` through `compilerArgs`. See [Projects and Build Tools](02_projects_and_build_tools.md).

---

## Clean Code Habits

### Keep Methods Small

A method does one thing and fits on a screen. If you need a comment to explain a block, make that block its own method with a good name.

### Return Early

Use guard clauses so the main path is not nested. A guard clause is an `if` at the top that leaves the method when something is wrong.

```java
// Nested
if (user != null) {
    if (user.isActive()) {
        process(user);
    }
}

// Flat
if (user == null) return;
if (!user.isActive()) return;

process(user);
```

### No Magic Numbers

A magic number is an unexplained literal in the code.

```java
if (retries > 3) { }                     // why 3?

static final int MAX_RETRIES = 3;
if (retries > MAX_RETRIES) { }
```

### Comment the Why

```java
// Bad: repeats the code
i++;   // add one to i

// Good: explains a decision
// The API allows 10 requests per second, so wait 100 ms between calls.
Thread.sleep(100);
```

Use Javadoc (`/** ... */`) on public classes and methods. From Java 23 you can write it in Markdown with `///` lines.

### Prefer Immutable Data

Use `final` fields, `record`, and `List.of(...)` so values cannot change by accident. See [Records and Equality](25_records_and_equality.md).

### Do Not Return null for Collections

Return `List.of()` instead. Callers can loop without a `null` check. For a single value that may be missing, return `Optional<T>`. See [Null and Optional](24_null_and_optional.md).

### Do Not Repeat Yourself (DRY)

Copy-pasted code means every fix must be made twice. Move it into one method. Wait until you see the same code three times before you make an abstraction.

### Keep It Simple (KISS) and Do Not Build It Yet (YAGNI)

Write the simplest thing that works. Do not add options, layers, or interfaces for needs that do not exist yet.

---

## SOLID

Five design rules for classes. They push code toward small, replaceable parts. Named solutions built on them are in [Design Patterns](58_design_patterns.md).

### S: Single Responsibility

A class has one reason to change.

```java
// Does too much: stores, emails, and formats
class OrderManager { void save() { } void sendEmail() { } String toPdf() { return ""; } }

// Split by job
class OrderRepository { void save(Order o) { } }
class OrderNotifier   { void sendEmail(Order o) { } }
class OrderPdfWriter  { String toPdf(Order o) { return ""; } }
```

### O: Open/Closed

Add new behavior by adding code, not by editing working code.

```java
interface Discount { double apply(double price); }

class NoDiscount implements Discount { public double apply(double p) { return p; } }
class HalfPrice  implements Discount { public double apply(double p) { return p / 2; } }

double total(double price, Discount discount) { return discount.apply(price); }
```

A new discount is a new class. `total` never changes.

### L: Liskov Substitution

A child type must work anywhere its parent works. If `Penguin extends Bird` throws in `fly()`, the inheritance is wrong. See [Inheritance](30_inheritance.md).

### I: Interface Segregation

Many small interfaces beat one big one.

```java
interface Reader { String read(); }
interface Writer { void write(String text); }

class LogFile implements Reader, Writer { /* ... */ }
class ReadOnlyConfig implements Reader { /* ... */ }   // not forced to implement write
```

### D: Dependency Inversion

Depend on an interface, not on a concrete class, and pass it in.

```java
class OrderService {
    private final OrderRepository repo;   // an interface

    OrderService(OrderRepository repo) {  // gets what it needs from outside
        this.repo = repo;
    }
}
```

This is the idea behind [Dependency Injection](56_dependency_injection.md), and it makes [testing](59_testing.md) easy because you can pass a fake.

---

## Composition over Inheritance

Prefer a class that has another class to a class that is one. Inheritance locks the design; composition can be swapped.

```java
// Inheritance: a Car is an Engine? No.
class Car extends Engine { }

// Composition: a Car has an Engine
class Car {
    private final Engine engine;
    Car(Engine engine) { this.engine = engine; }
}
```

Make classes `final` unless you designed them to be extended. Use `sealed` when only a known list of subclasses is allowed. See [Interfaces and Abstract Classes](32_interfaces_and_abstract_classes.md).

---

## Review Checklist

- Can I read each method name and know what it does?
- Does each class have one job?
- Are there magic numbers or copy-pasted blocks?
- Is there deep nesting that a guard clause would remove?
- Could a `null` break this? Is `Optional` or an empty list better?
- Are resources closed with try-with-resources? (See [AutoCloseable and try-with-resources](33_autocloseable.md))
- Does the build pass with `-Xlint:all` and the project's formatter?

---

## Gotchas

- Rules are a guide. A tiny script does not need five interfaces
- Do not rename things only to match a style if the code is shared and stable; follow the style of the file you are in
- Over-abstraction is as bad as none: an interface with one implementation and no reason to swap it adds noise
- `IOrderRepository` and `OrderRepositoryImpl` are habits from other languages. In Java, name the interface `OrderRepository` and the class after how it works, such as `JdbcOrderRepository`
- Getters and setters for every field are not encapsulation. Expose behavior, or use a `record` for plain data
- Two style guides are common: Oracle's old Code Conventions and the Google Java Style Guide. Pick one per project and let a tool enforce it
