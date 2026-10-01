# 42 - Error Handling

## try / catch / finally

An **exception** is an object that reports an error. Throwing one stops normal flow until a matching `catch` handles it.

```java
public class TryCatch {
    public static void main(String[] args) {
        try {
            int[] items = {1, 2, 3};
            System.out.println(items[5]);              // throws ArrayIndexOutOfBoundsException
        } catch (ArrayIndexOutOfBoundsException e) {
            System.out.println("Bad index: " + e.getMessage());
        } catch (Exception e) {
            System.out.println("Unexpected: " + e.getMessage());
        } finally {
            System.out.println("Always runs: cleanup here");
        }
    }
}
```

Expected output should be:

```
Bad index: Index 5 out of bounds for length 3
Always runs: cleanup here
```

- `catch` blocks are checked top to bottom: put specific types before general ones
- `finally` runs whether or not an exception happened, even after a `return`

---

## Multi-catch

Handle several types with one block.

```java
try {
    int n = Integer.parseInt(text);
    System.out.println(10 / n);
} catch (NumberFormatException | ArithmeticException e) {
    System.out.println("Bad input: " + e.getMessage());
}
```

The types in one multi-catch cannot be parent and child of each other.

---

## Checked vs Unchecked

Java has two kinds of exceptions. This is the biggest difference from C#.

| Kind      | Extends                              | Compiler rule                                 | Example                                            |
| --------- | ------------------------------------ | --------------------------------------------- | -------------------------------------------------- |
| Checked   | `Exception` (not `RuntimeException`) | You must catch it or declare it with `throws` | `IOException`, `InterruptedException`              |
| Unchecked | `RuntimeException`                   | No rule. Usually a bug in the code            | `NullPointerException`, `IllegalArgumentException` |
| Error     | `Error`                              | Do not catch. The JVM is in trouble           | `OutOfMemoryError`, `StackOverflowError`           |

```java
// The caller must deal with IOException
static String readConfig(Path path) throws IOException {
    return Files.readString(path);
}
```

```
Throwable
├── Error                       (do not catch)
└── Exception                   (checked)
    ├── IOException
    ├── InterruptedException
    └── RuntimeException        (unchecked)
        ├── NullPointerException
        ├── IllegalArgumentException
        │   └── NumberFormatException
        ├── IllegalStateException
        ├── ArithmeticException
        └── IndexOutOfBoundsException
```

---

## Exception Methods

```java
catch (Exception e) {
    e.getMessage();                 // short description
    e.getClass().getSimpleName();   // type name, such as "IOException"
    e.getCause();                   // the wrapped exception, or null
    e.printStackTrace();            // type, message, and call stack to stderr
}
```

Reading a stack trace is covered in [Debugging](43_debugging.md).

---

## Throwing Exceptions

```java
static int divide(int a, int b) {
    if (b == 0) {
        throw new IllegalArgumentException("Divisor cannot be zero");
    }
    return a / b;
}
```

Check arguments at the start of a method with the helpers in `java.util.Objects`:

```java
this.name = Objects.requireNonNull(name, "name");        // throws NullPointerException with "name"
Objects.checkIndex(index, list.size());                   // throws IndexOutOfBoundsException
```

---

## Rethrowing and Wrapping

Rethrow the same object to keep its stack trace:

```java
catch (IOException e) {
    log(e);
    throw e;
}
```

Wrap a low-level error in your own type. Pass the original as the **cause** so its details are not lost:

```java
catch (IOException e) {
    throw new ConfigException("Could not load settings", e);
}
```

`UncheckedIOException` wraps an `IOException` when you cannot add `throws`, such as inside a lambda.

---

## Common Exception Types

| Exception                         | When thrown                                           |
| --------------------------------- | ----------------------------------------------------- |
| `NullPointerException`            | Using a member of a `null` reference                  |
| `IllegalArgumentException`        | A method got a bad argument                           |
| `IllegalStateException`           | The call is not valid right now                       |
| `IndexOutOfBoundsException`       | Bad index on a list, array, or string                 |
| `ArithmeticException`             | Integer division by zero, or `Math.addExact` overflow |
| `NumberFormatException`           | `Integer.parseInt("abc")`                             |
| `ClassCastException`              | Bad cast between object types                         |
| `UnsupportedOperationException`   | Changing an unmodifiable list, such as `List.of`      |
| `ConcurrentModificationException` | Changing a collection while looping over it           |
| `IOException`                     | File or network failure (checked)                     |
| `InterruptedException`            | A waiting thread was told to stop (checked)           |

---

## Custom Exceptions

```java
public class CustomException {
    static class InsufficientFundsException extends RuntimeException {
        private final long amount;

        InsufficientFundsException(long amount) {
            super("Insufficient funds. Required: " + amount);
            this.amount = amount;
        }

        long amount() { return amount; }
    }

    static void withdraw(long balance, long amount) {
        if (amount > balance) throw new InsufficientFundsException(amount - balance);
    }

    public static void main(String[] args) {
        try {
            withdraw(50, 80);
        } catch (InsufficientFundsException e) {
            System.out.println(e.getMessage());
            System.out.println("Short by " + e.amount());
        }
    }
}
```

Expected output should be:

```
Insufficient funds. Required: 30
Short by 30
```

Extend `RuntimeException` for unchecked, or `Exception` for checked. Most modern code uses unchecked.

---

## try-with-resources

Closes a resource automatically, even when an exception is thrown. Full details in [AutoCloseable and try-with-resources](33_autocloseable.md).

```java
try (BufferedReader reader = Files.newBufferedReader(Path.of("file.txt"))) {
    System.out.println(reader.readLine());
}   // reader.close() runs here
```

If `close()` also throws, that second exception is attached to the first one. Read it with `e.getSuppressed()`.

---

## Overflow

Integer math wraps around silently. The `Math.*Exact` methods throw instead.

```java
int max = Integer.MAX_VALUE;

int wrapped = max + 1;          // -2147483648, no error
int safe = Math.addExact(max, 1);   // throws ArithmeticException: integer overflow
```

See [Operators](11_operators.md).

---

## Gotchas

- An empty `catch` block hides bugs. At least log the exception
- Catching `Exception` also catches every `RuntimeException`, including real bugs like `NullPointerException`
- Never catch `Throwable` or `Error` in normal code
- `return` inside `finally` throws away any exception from `try`. Do not return from `finally`
- When you catch `InterruptedException`, call `Thread.currentThread().interrupt()` to keep the stop request (see [Threading](53_threading.md))
- Exceptions are slow to create. Do not use them for normal control flow, such as ending a loop

---

## Examples

- [42-01](examples/42-01_try_catch_finally.java): Try catch finally
- [42-02](examples/42-02_custom_exceptions.java): Custom exceptions

Run one with `java examples/42-01_try_catch_finally.java`. See [Examples](examples/README.md).
