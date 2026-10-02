# 54 - Generics

## What are Generics

**Generics** let you write a class or method once and use it with any type. The caller picks the type in angle brackets, and the compiler checks it.

```java
List<String> names = new ArrayList<>();
names.add("Alice");
names.add(42);                  // compile error: 42 is not a String

String first = names.get(0);    // no cast needed
```

`<>` on the right is the **diamond**: the compiler copies the type from the left (Java 7).

---

## Generic Class

`T` is a **type parameter**, a placeholder for the real type.

```java
import java.util.function.Function;

public class Generics {
    static class Box<T> {
        private final T value;

        Box(T value) { this.value = value; }

        T get() { return value; }

        <R> Box<R> map(Function<T, R> fn) {     // a generic method inside a generic class
            return new Box<>(fn.apply(value));
        }
    }

    record Pair<A, B>(A first, B second) {}     // records can be generic too

    public static void main(String[] args) {
        Box<String> box = new Box<>("hello");
        System.out.println(box.get().toUpperCase());
        System.out.println(box.map(String::length).get());

        var pair = new Pair<>("Alice", 30);
        System.out.println(pair.first() + ": " + pair.second());
        System.out.println(pair);
    }
}
```

Expected output should be:

```
HELLO
5
Alice: 30
Pair[first=Alice, second=30]
```

| Name     | Usual meaning           |
| -------- | ----------------------- |
| `T`      | Type                    |
| `E`      | Element of a collection |
| `K`, `V` | Key and value of a map  |
| `R`      | Return type             |

---

## Generic Method

Put the type parameter before the return type. The compiler works out `T` from the arguments.

```java
static <T extends Comparable<T>> T max(T a, T b) {
    return a.compareTo(b) >= 0 ? a : b;
}

max(3, 7)                 // 7
max(3.14, 2.71)           // 3.14
max("apple", "banana")    // banana
```

To give the type yourself, write it after the dot: `Collections.<String>emptyList()`.

---

## Generic Interface

```java
interface Repository<T, ID> {
    Optional<T> findById(ID id);
    List<T> findAll();
    void save(T item);
    void delete(ID id);
}

class UserRepository implements Repository<User, Integer> {
    // methods use User and Integer
}
```

---

## Bounded Types

A **bound** limits which types are allowed, and lets you call their methods.

| Bound                                | Meaning                       |
| ------------------------------------ | ----------------------------- |
| `<T extends Number>`                 | `T` is `Number` or a subclass |
| `<T extends Comparable<T>>`          | `T` can be compared to itself |
| `<T extends Number & Comparable<T>>` | Both. A class must come first |

`extends` is used for interfaces too.

```java
static <T extends Number & Comparable<T>> T largest(List<T> items) {
    return Collections.max(items);
}

largest(List.of(4, 9, 2))   // 9
```

---

## Wildcards

A `List<Integer>` is **not** a `List<Number>`, even though `Integer` is a `Number`. If it were, you could add a `Double` to a list of integers. A **wildcard** `?` makes a method accept related types.

```java
static double sum(List<? extends Number> numbers) {    // read Numbers from it
    double total = 0;
    for (Number n : numbers) total += n.doubleValue();
    return total;
}

sum(List.of(1, 2, 3))      // 6.0, a List<Integer>
sum(List.of(1.5, 2.5))     // 4.0, a List<Double>

static void fill(List<? super Integer> target) {       // write Integers into it
    for (int i = 1; i <= 3; i++) target.add(i);
}

fill(new ArrayList<Number>());   // works
fill(new ArrayList<Object>());   // works
```

| Wildcard      | You can           | Use when the list is a         |
| ------------- | ----------------- | ------------------------------ |
| `? extends T` | Read `T` out      | Producer (source)              |
| `? super T`   | Put `T` in        | Consumer (target)              |
| `?`           | Read `Object` out | You do not care about the type |

Memory aid: **PECS**, "producer extends, consumer super". `Collections.copy(List<? super T> dest, List<? extends T> src)` uses both.

---

## Type Erasure

The compiler checks generic types and then **erases** them: at runtime a `List<String>` is just a `List`. This is the biggest difference from C#.

```java
List<String> strings = new ArrayList<>();
List<Integer> ints = new ArrayList<>();

strings.getClass() == ints.getClass()   // true, both are plain ArrayList
```

Things erasure forbids:

| Not allowed                                        | Why                            | Instead                              |
| -------------------------------------------------- | ------------------------------ | ------------------------------------ |
| `new T()`                                          | `T` is unknown at runtime      | Pass a `Supplier<T>` or a `Class<T>` |
| `new T[10]`                                        | Same                           | Use a `List<T>`                      |
| `x instanceof List<String>`                        | The `String` part is gone      | `x instanceof List<?> list`          |
| `List<int>`                                        | Type arguments must be objects | `List<Integer>` (boxing)             |
| `static T field`                                   | One static field for all `T`   | Make it an instance field            |
| Overloads `f(List<String>)` and `f(List<Integer>)` | Both erase to `f(List)`        | Give them different names            |

Pass a **class token** when you need the type at runtime:

```java
static <T> T create(Class<T> type) throws ReflectiveOperationException {
    return type.getDeclaredConstructor().newInstance();
}

StringBuilder sb = create(StringBuilder.class);
```

The same idea is why Jackson needs a `TypeReference` for lists (see [JSON](48_json.md)).

---

## Raw Types

A **raw type** is a generic type used without `< >`, such as `List`. It exists only for code written before Java 5. It turns off type checks.

```java
List raw = new ArrayList();      // warning: raw type
raw.add("x");
raw.add(1);

List<String> unsafe = raw;       // warning: unchecked conversion
String s = unsafe.get(1);        // ClassCastException at runtime
```

Never write raw types in new code. Use `List<?>` when you do not know the type.

---

## Generics You Already Use

| Type                                            | Description                                                        |
| ----------------------------------------------- | ------------------------------------------------------------------ |
| `List<E>`, `Set<E>`, `Map<K, V>`                | Collections (see [Collections](36_collections.md))                 |
| `Optional<T>`                                   | A value that may be missing                                        |
| `Function<T, R>`, `Supplier<T>`, `Predicate<T>` | Lambdas (see [lesson 39](39_lambdas_and_functional_interfaces.md)) |
| `Stream<T>`                                     | Streams                                                            |
| `CompletableFuture<T>`                          | A result that arrives later                                        |
| `Comparable<T>`, `Comparator<T>`                | Ordering                                                           |
| `Class<T>`                                      | A type at runtime                                                  |

---

## Gotchas

- Type information is erased at runtime, so `T` cannot be used with `new`, `instanceof`, or arrays
- `List<Integer>` is not a `List<Number>`. Use `List<? extends Number>` in the parameter
- Primitives cannot be type arguments. `List<Integer>` boxes each value, which costs memory. `IntStream` and `int[]` avoid it
- Unchecked warnings mean the compiler cannot prove type safety. Fix them; do not hide them
- Java has no generic math like C#'s `INumber<T>`. A method that adds `T` values needs `T extends Number` and `doubleValue()`, or one overload per type
- Do not add bounds you do not need. Each bound makes the method accept fewer types

---

## Examples

- [54-01](examples/54-01_generic_class.java): Generic class

Run one with `java examples/54-01_generic_class.java`. See [Examples](examples/README.md).
