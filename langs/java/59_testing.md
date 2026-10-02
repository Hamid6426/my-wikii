# 59 - Testing

## Why Test

A unit test is a small method that runs one piece of your code and checks the result. Tests catch regressions (old bugs coming back), document expected behavior, and give you confidence to refactor.

The standard test framework is JUnit 5, also called JUnit Jupiter. Its classes are in the package `org.junit.jupiter.api`. It is not part of the JDK, so you add it to the build. JUnit 6 keeps the same `org.junit.jupiter` API, so everything here works there too.

---

## Project Setup

Maven and Gradle expect tests in their own folder:

```
my-app/
  src/main/java/Calculator.java        # the code
  src/test/java/CalculatorTest.java    # the tests
```

Maven `pom.xml` (the version is an example; use the latest 5.x):

```xml
<dependency>
  <groupId>org.junit.jupiter</groupId>
  <artifactId>junit-jupiter</artifactId>
  <version>5.14.4</version>
  <scope>test</scope>
</dependency>
```

Gradle `build.gradle.kts`:

```kotlin
dependencies {
    testImplementation(platform("org.junit:junit-bom:5.14.4"))
    testImplementation("org.junit.jupiter:junit-jupiter")
    testRuntimeOnly("org.junit.platform:junit-platform-launcher")
}

tasks.test {
    useJUnitPlatform()
}
```

`junit-jupiter` pulls in the API, the parameterized tests, and the engine that runs them. See [Projects and Build Tools](02_projects_and_build_tools.md).

---

## A First Test

The class under test:

```java
public class Calculator {
    public int add(int a, int b) {
        return a + b;
    }

    public int divide(int a, int b) {
        if (b == 0) {
            throw new IllegalArgumentException("b must not be zero");
        }
        return a / b;
    }
}
```

The test, using `org.junit.jupiter.api`:

```java
import static org.junit.jupiter.api.Assertions.assertEquals;

import org.junit.jupiter.api.Test;

class CalculatorTest {

    @Test
    void add_returnsSum() {
        // Arrange
        var calc = new Calculator();

        // Act
        int result = calc.add(2, 3);

        // Assert
        assertEquals(5, result);
    }
}
```

`@Test` marks a method as a test. Test classes and methods do not need to be `public` in JUnit 5. A test passes when it finishes without an exception. A failed assertion throws one.

---

## Arrange, Act, Assert

The AAA pattern splits each test into three parts, as in the example above.

| Part    | What happens                             |
| ------- | ---------------------------------------- |
| Arrange | Create the objects and input             |
| Act     | Call the one method you are testing      |
| Assert  | Check the result, the state, or an error |

One test checks one behavior. If the name needs "and", split it.

---

## Common Assertions

All are static methods of `org.junit.jupiter.api.Assertions`. Import them with `import static org.junit.jupiter.api.Assertions.*;`.

```java
assertEquals(5, calc.add(2, 3));               // expected first, then actual
assertEquals(0.3, 0.1 + 0.2, 0.0001);          // doubles need a tolerance
assertNotEquals(1, 2);
assertTrue(list.contains(2));
assertFalse(list.isEmpty());
assertNull(nothing);
assertNotNull(obj);
assertSame(obj, obj);                          // same object, not just equal
assertArrayEquals(new int[] {1, 2}, arr);
assertIterableEquals(List.of(1, 2, 3), list);
assertInstanceOf(String.class, obj);
assertDoesNotThrow(() -> Integer.parseInt("42"));
assertTimeout(Duration.ofSeconds(1), () -> Thread.sleep(10));
```

Every assertion takes an optional last argument: a message shown when it fails.

```java
assertEquals(6, new Calculator().add(2, 3), "2 + 3");
```

Expected output should be:

```
org.opentest4j.AssertionFailedError: 2 + 3 ==> expected: <6> but was: <5>
```

---

## Testing Exceptions

`assertThrows` runs a lambda, checks that it throws the given type, and returns the exception so you can check it.

```java
@Test
void divide_byZero_throws() {
    var calc = new Calculator();

    IllegalArgumentException ex = assertThrows(
            IllegalArgumentException.class,
            () -> calc.divide(10, 0));

    assertEquals("b must not be zero", ex.getMessage());
}
```

See [Error Handling](42_error_handling.md).

---

## Several Checks at Once

`assertAll` runs every check and reports all failures together, instead of stopping at the first.

```java
@Test
void divide_checksSeveralThings() {
    var calc = new Calculator();

    assertAll(
            () -> assertEquals(5, calc.divide(10, 2)),
            () -> assertEquals(3, calc.divide(10, 3)),
            () -> assertEquals(-2, calc.divide(-10, 5)));
}
```

---

## Parameterized Tests

One test method, many data sets. These live in `org.junit.jupiter.params`, which `junit-jupiter` already includes.

```java
import static org.junit.jupiter.api.Assertions.assertEquals;

import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import org.junit.jupiter.params.provider.ValueSource;

class CalculatorParamTest {

    @ParameterizedTest
    @CsvSource({
        "2, 3, 5",
        "-1, 1, 0",
        "0, 0, 0"
    })
    void add_returnsExpected(int a, int b, int expected) {
        assertEquals(expected, new Calculator().add(a, b));
    }

    @ParameterizedTest
    @ValueSource(ints = {1, 2, 3})
    void add_zero_keepsNumber(int n) {
        assertEquals(n, new Calculator().add(n, 0));
    }
}
```

| Source          | Gives                                                |
| --------------- | ---------------------------------------------------- |
| `@ValueSource`  | One value per run: `ints`, `strings`, and more       |
| `@CsvSource`    | Several values per run, as comma-separated text      |
| `@EnumSource`   | Each constant of an enum                             |
| `@MethodSource` | A static method that returns a `Stream` of arguments |
| `@NullSource`   | A `null` value, for edge cases                       |

---

## Organizing Tests

| Annotation     | Does                                                  |
| -------------- | ----------------------------------------------------- |
| `@DisplayName` | A readable name in reports: `"divide by zero throws"` |
| `@Disabled`    | Skips a test. Give a reason: `@Disabled("bug #42")`   |
| `@Tag("slow")` | Labels a test so the build can include or skip it     |
| `@Nested`      | An inner class that groups related tests              |

```java
class StackTest {
    @Nested
    class WhenEmpty {
        @Test
        void hasSizeZero() {
            assertEquals(0, List.of().size());
        }
    }
}
```

Setup and cleanup methods (`@BeforeEach`, `@AfterEach`) are in [Mocking and Fixtures](60_mocking_and_fixtures.md).

---

## Running Tests

```bash
mvn test                                   # run all
mvn test -Dtest=CalculatorTest             # one class
mvn test -Dtest='CalculatorTest#add*'      # matching methods

./gradlew test                             # run all
./gradlew test --tests CalculatorTest      # one class
./gradlew test --tests '*add*'             # matching names
```

Your IDE also runs tests with a click next to each method. See [IDE For Java](03_ide_for_java.md).

A failing test makes the build fail. Reports are written to `target/surefire-reports/` (Maven) or `build/reports/tests/` (Gradle).

Code coverage shows which lines the tests ran. JaCoCo is the usual tool, added as a Maven or Gradle plugin.

---

## Gotchas

- `assertEquals(expected, actual)`: the expected value comes first. Swapping them gives a confusing failure message
- Compare `double` values with a tolerance (the third argument), never exactly
- Tests must be independent. JUnit creates a new test class instance per test method, but `static` fields are shared
- Test order is not guaranteed. Never let one test depend on another having run
- Name tests clearly: `method_condition_expectedResult`, or use `@DisplayName`
- Do not test private methods directly. Test through the public methods
- Avoid `Thread.sleep` to wait for async work. Wait on the `CompletableFuture` with a timeout instead (see [CompletableFuture and Async Code](51_completablefuture_and_async.md))
- JUnit 4 (`org.junit.Test`, `@Before`) is a different, older API. Do not mix its imports with JUnit 5
