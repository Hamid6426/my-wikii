# 60 - Mocking and Fixtures

## Test Doubles

A test double is any object that stands in for a real dependency during a test. It keeps tests fast and free of email servers, databases, and networks. This only works if the class takes its dependencies from outside. See [Dependency Injection](56_dependency_injection.md).

| Kind | What it does                                                      |
| ---- | ----------------------------------------------------------------- |
| Fake | A small working version written by hand, such as an in-memory map |
| Stub | Returns fixed answers to calls                                    |
| Mock | Records how it was called, so the test can check it               |
| Spy  | Wraps a real object and records calls to it                       |

The examples below test this code:

```java
record Order(int id, String email) {}

interface EmailService {
    void send(String to, String subject);
}

class OrderService {
    private final EmailService email;

    OrderService(EmailService email) {
        this.email = email;
    }

    void placeOrder(Order order) {
        email.send(order.email(), "Order " + order.id() + " confirmed");
    }
}
```

---

## Mockito Setup

Mockito is the standard mocking library for Java. Its classes are in the package `org.mockito`. Add it next to JUnit ([Testing](59_testing.md)):

```xml
<dependency>
  <groupId>org.mockito</groupId>
  <artifactId>mockito-core</artifactId>
  <version>5.20.0</version>
  <scope>test</scope>
</dependency>
<dependency>
  <groupId>org.mockito</groupId>
  <artifactId>mockito-junit-jupiter</artifactId>
  <version>5.20.0</version>
  <scope>test</scope>
</dependency>
```

The version is an example; use the latest 5.x. `mockito-junit-jupiter` adds the JUnit 5 extension used further down.

---

## Mock and Verify

`mock(...)` creates an object that implements the interface and does nothing. `verify(...)` checks that a method was called with the given arguments.

```java
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.verifyNoMoreInteractions;

import org.junit.jupiter.api.Test;

class OrderServiceTest {

    @Test
    void placeOrder_sendsConfirmationEmail() {
        // Arrange
        EmailService email = mock(EmailService.class);
        var service = new OrderService(email);

        // Act
        service.placeOrder(new Order(7, "ann@example.com"));

        // Assert
        verify(email).send("ann@example.com", "Order 7 confirmed");
        verifyNoMoreInteractions(email);
    }
}
```

If the call did not happen, or happened with other arguments, the test fails and Mockito prints what it expected and what it saw.

---

## Stubbing Return Values

`when(...).thenReturn(...)` sets what a mock returns. Calls you did not set up return a default: `null`, `0`, `false`, an empty collection, or an empty `Optional`.

```java
interface OrderRepository {
    Optional<Order> findById(int id);
    int count();
}
```

```java
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

OrderRepository repo = mock(OrderRepository.class);

when(repo.findById(1)).thenReturn(Optional.of(new Order(1, "ann@example.com")));
when(repo.findById(99)).thenReturn(Optional.empty());
when(repo.count()).thenThrow(new IllegalStateException("db down"));

repo.findById(1);   // Optional[Order[id=1, email=ann@example.com]]
repo.findById(5);   // Optional.empty: not stubbed
```

---

## Argument Matchers and Call Counts

Matchers accept any value of a kind. They are static methods of `org.mockito.ArgumentMatchers`, also reachable through `Mockito`.

```java
import static org.mockito.Mockito.anyString;
import static org.mockito.Mockito.eq;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.times;
import static org.mockito.Mockito.verify;

verify(email, times(2)).send(anyString(), anyString());
verify(email, never()).send(eq("boss@example.com"), anyString());
```

| Matcher or mode       | Means                                   |
| --------------------- | --------------------------------------- |
| `any()`, `anyInt()`   | Any value of that type                  |
| `eq(value)`           | Exactly this value                      |
| `argThat(x -> ...)`   | Any value the lambda accepts            |
| `times(n)`, `never()` | Called exactly `n` times, or not at all |
| `atLeastOnce()`       | Called one or more times                |

If one argument uses a matcher, all of them must. Wrap plain values in `eq(...)`.

---

## Capturing Arguments

An `ArgumentCaptor` grabs the value a mock received, so you can check it in detail.

```java
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.mockito.Mockito.eq;
import static org.mockito.Mockito.verify;

import org.mockito.ArgumentCaptor;

ArgumentCaptor<String> subject = ArgumentCaptor.forClass(String.class);

verify(email).send(eq("ann@example.com"), subject.capture());

assertEquals("Order 7 confirmed", subject.getValue());
```

---

## The JUnit Extension

`MockitoExtension` creates the mocks for you. `@Mock` makes a mock. `@InjectMocks` builds the class under test and passes the mocks to its constructor.

```java
import static org.mockito.Mockito.verify;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

@ExtendWith(MockitoExtension.class)
class OrderServiceExtensionTest {

    @Mock
    EmailService email;

    @InjectMocks
    OrderService service;

    @Test
    void placeOrder_sendsConfirmationEmail() {
        service.placeOrder(new Order(7, "ann@example.com"));

        verify(email).send("ann@example.com", "Order 7 confirmed");
    }
}
```

The extension also runs in strict mode: a `when(...)` that no test code used fails the test with `UnnecessaryStubbingException`. That keeps tests free of dead setup.

---

## Spies

A spy wraps a real object. Real methods run, and calls are recorded.

```java
import static org.mockito.Mockito.spy;
import static org.mockito.Mockito.verify;

List<String> list = spy(new ArrayList<>());
list.add("a");

verify(list).add("a");
System.out.println(list.size());   // 1: the real method ran
```

Use spies rarely. Needing one often means the class does too much.

---

## Fixtures: Setup and Cleanup

A fixture is the known starting state a test runs against: objects, files, or data. JUnit 5 (`org.junit.jupiter.api`) has four lifecycle annotations.

```java
import static org.junit.jupiter.api.Assertions.assertEquals;

import java.util.ArrayList;
import java.util.List;
import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.MethodOrderer;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.TestMethodOrder;

@TestMethodOrder(MethodOrderer.MethodName.class)
class LifecycleTest {
    private List<String> items;

    @BeforeAll
    static void startAll() { System.out.println("before all"); }

    @BeforeEach
    void setUp() {
        System.out.println("before each");
        items = new ArrayList<>();   // a fresh list for every test
    }

    @Test
    void first() {
        System.out.println("  test 1");
        items.add("a");
        assertEquals(1, items.size());
    }

    @Test
    void second() {
        System.out.println("  test 2");
        assertEquals(0, items.size());   // not affected by test 1
    }

    @AfterEach
    void tearDown() { System.out.println("after each"); }

    @AfterAll
    static void stopAll() { System.out.println("after all"); }
}
```

Expected output should be:

```
before all
before each
  test 1
after each
before each
  test 2
after each
after all
```

| Annotation    | Runs                  | Use for                                      |
| ------------- | --------------------- | -------------------------------------------- |
| `@BeforeAll`  | Once, before any test | Expensive shared setup. Must be `static`     |
| `@BeforeEach` | Before every test     | Fresh objects for each test                  |
| `@AfterEach`  | After every test      | Close resources, delete temporary data       |
| `@AfterAll`   | Once, after all tests | Shut down shared resources. Must be `static` |

`@TestMethodOrder` is only here to make the output stable. Normal tests should not care about order.

---

## Temporary Folders

`@TempDir` gives each test a new empty folder and deletes it afterwards.

```java
import static org.junit.jupiter.api.Assertions.assertEquals;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.io.TempDir;

class TempDirTest {
    @TempDir
    Path dir;   // a new empty folder per test, deleted afterwards

    @Test
    void writesAndReadsFile() throws IOException {
        Path file = dir.resolve("notes.txt");
        Files.writeString(file, "hello");

        assertEquals("hello", Files.readString(file));
    }
}
```

See [File I/O: Read and Write](45_file_io_read_write.md).

---

## Mock or Fake

| Use a mock when                                | Use a hand-written fake when                 |
| ---------------------------------------------- | -------------------------------------------- |
| You must check that a call happened            | You only need the dependency to work         |
| The interface is large and you need one method | The same fake is reused by many tests        |
| Setting up a reply in one line is enough       | The behavior has state, such as a repository |

A fake like the `FakeEmailService` in [Dependency Injection](56_dependency_injection.md) needs no library at all.

---

## Gotchas

- Mock interfaces you own. Do not mock value types such as `String`, records, or `List`. Create real ones
- Mockito 5 can mock `final` classes, but on Java 21 and newer it prints a warning about loading an agent at run time. Follow the Mockito docs to add it as a `-javaagent` in Maven or Gradle
- Too many `verify` calls make tests break on every refactor. Check results first, and calls only when the call is the behavior
- A `@BeforeAll` method must be `static`, unless the class is marked `@TestInstance(Lifecycle.PER_CLASS)`
- State set in a `static` field leaks between tests. Reset it in `@BeforeEach` or avoid it
- `when(mock.method())` on a spy calls the real method. Use `doReturn(value).when(spy).method()` for spies
