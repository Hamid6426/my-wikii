# 24 - Null and Optional

## What null Means

`null` means "this reference points at no object". Any reference type can be `null`. Primitives such as `int` cannot.

```java
String name = null;      // fine
int count = null;        // compile error
Integer boxed = null;    // fine: Integer is a reference type
```

Java has no `string?` versus `string` in the type system. The compiler does not warn you about possible `null` values.

---

## NullPointerException

Calling a method or reading a field through `null` throws `NullPointerException` (NPE). Since Java 14, the message says exactly what was `null`.

```java
public class Npe {
    public static void main(String[] args) {
        String name = null;
        System.out.println(name.length());
    }
}
```

Expected output should be:

```
Exception in thread "main" java.lang.NullPointerException: Cannot invoke "String.length()" because "<local1>" is null
	at Npe.main(Npe.java:4)
```

`<local1>` appears because local variable names were not kept. A class compiled with `javac -g` (most build tools do this) shows `"name"` instead.

---

## Null Checks

Java has no `?.` or `??` operators. Write the check out, or use the `Objects` helpers.

```java
if (name != null) {
    System.out.println(name.length());
}

int len = (name != null) ? name.length() : 0;

String display = Objects.requireNonNullElse(name, "Unknown");          // Java 9+
String lazy = Objects.requireNonNullElseGet(name, () -> loadDefault());
```

| Other languages (C#) | Java                                       |
| -------------------- | ------------------------------------------ |
| `s?.Length`          | `s != null ? s.length() : null`            |
| `s ?? "default"`     | `Objects.requireNonNullElse(s, "default")` |
| `s ??= "default"`    | `if (s == null) s = "default";`            |

---

## Guard Clauses

Fail fast at the start of a method instead of deep inside it.

```java
void save(String path) {
    Objects.requireNonNull(path, "path must not be null");
    if (path.isBlank()) {
        throw new IllegalArgumentException("path must not be blank");
    }
    // ...
}
```

`requireNonNull` throws `NullPointerException` with your message, and returns the value, so it also works in constructors: `this.path = Objects.requireNonNull(path);`.

---

## Optional

`Optional<T>` (Java 8+) is a box that holds one value or nothing. A method that returns `Optional` tells the caller "there may be no result, handle it".

```java
import java.util.List;
import java.util.Optional;

public class FindUser {
    record User(String name, String email) {}

    static final List<User> USERS = List.of(
        new User("alice", "alice@example.com"),
        new User("bob", null));

    static Optional<User> find(String name) {
        for (User u : USERS) {
            if (u.name().equals(name)) return Optional.of(u);
        }
        return Optional.empty();
    }

    public static void main(String[] args) {
        System.out.println(find("alice").isPresent());
        System.out.println(find("zoe").isEmpty());

        String email = find("alice").map(User::email).orElse("no email");
        System.out.println(email);

        String bobEmail = find("bob")
            .map(User::email)              // email is null, so the result is empty
            .orElse("no email");
        System.out.println(bobEmail);

        find("zoe").ifPresentOrElse(
            u -> System.out.println("found " + u.name()),
            () -> System.out.println("no such user"));
    }
}
```

Expected output should be:

```
true
true
alice@example.com
no email
no such user
```

`map` turns a `null` result into an empty `Optional`, so a chain never throws on a missing part.

---

## Optional Methods

| Group     | Method                     | Does                                               |
| --------- | -------------------------- | -------------------------------------------------- |
| Create    | `Optional.of(v)`           | Wraps a value; throws if `v` is `null`             |
| Create    | `Optional.ofNullable(v)`   | Wraps a value, or empty if `null`                  |
| Create    | `Optional.empty()`         | Nothing                                            |
| Check     | `isPresent()`, `isEmpty()` | Is there a value                                   |
| Get       | `orElse(other)`            | The value, or `other`                              |
| Get       | `orElseGet(() -> ...)`     | The value, or runs the lambda only if empty        |
| Get       | `orElseThrow()`            | The value, or throws `NoSuchElementException`      |
| Get       | `orElseThrow(() -> ex)`    | The value, or throws your exception                |
| Transform | `map(f)`, `filter(p)`      | Changes or tests the value if there is one         |
| Transform | `flatMap(f)`               | Like `map`, when `f` already returns an `Optional` |
| Act       | `ifPresent(c)`             | Runs code only when there is a value               |
| Act       | `ifPresentOrElse(c, r)`    | Runs one of two blocks                             |

For primitives, use `OptionalInt`, `OptionalLong`, and `OptionalDouble`. Streams return them: `IntStream.of(3, 1).max()` is an `OptionalInt`.

---

## When to Use Optional

| Place                          | Use Optional? | Instead                                |
| ------------------------------ | ------------- | -------------------------------------- |
| Return type of a "find" method | Yes           |                                        |
| Field                          | No            | `null` or a sensible default value     |
| Method parameter               | No            | An overload without that parameter     |
| Collection result              | No            | An empty list, not `Optional<List<T>>` |

`Optional` itself must never be `null`. Return `Optional.empty()`.

To return a value or an error, throw an exception instead. To return several values at once, use a record. See [Error Handling](42_error_handling.md) and [Records and Equality](25_records_and_equality.md).

---

## Null Annotations

Annotations such as `@Nullable` and `@NonNull` mark which values may be `null`. The JDK does not check them, but IDEs and tools like NullAway do. JSpecify (`org.jspecify.annotations`) is the shared standard many libraries now use. See [Annotations and Reflection](55_annotations_and_reflection.md).

---

## Gotchas

- `optional.get()` on an empty `Optional` throws; prefer `orElse`, `orElseThrow`, or `ifPresent`
- `orElse(compute())` always runs `compute()`, even when there is a value; use `orElseGet(() -> compute())` for costly defaults
- `Optional.of(null)` throws `NullPointerException`; use `ofNullable` when the value may be `null`
- Unboxing a `null` `Integer` into an `int` throws NPE, which is easy to miss in `Map.get` results
- `Map.get` returns `null` for a missing key; use `getOrDefault` or `containsKey`
- `List.of`, `Set.of`, and `Map.of` reject `null` items and throw NPE

---

## Examples

- [24-01](examples/24-01_optional.java): Optional

Run one with `java examples/24-01_optional.java`. See [Examples](examples/README.md).
