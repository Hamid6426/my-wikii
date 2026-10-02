# 39 - Lambdas and Functional Interfaces

## Functional Interface

An interface with exactly **one** abstract method. A variable of that type can hold a lambda or a method reference, so you can pass behavior around like data. Java has no separate delegate type: functional interfaces fill that role.

```java
@FunctionalInterface
interface Operation {
    int apply(int a, int b);
}

Operation add = (a, b) -> a + b;
Operation multiply = (a, b) -> a * b;

System.out.println(add.apply(2, 3));        // 5
System.out.println(multiply.apply(2, 3));   // 6
```

`@FunctionalInterface` is optional. With it, the compiler fails if someone adds a second abstract method.

---

## Built-in Functional Interfaces

You rarely declare your own. `java.util.function` has these.

| Interface             | Method              | Shape                        |
| --------------------- | ------------------- | ---------------------------- |
| `Function<T, R>`      | `R apply(T)`        | One input, one result        |
| `BiFunction<T, U, R>` | `R apply(T, U)`     | Two inputs, one result       |
| `Supplier<T>`         | `T get()`           | No input, one result         |
| `Consumer<T>`         | `void accept(T)`    | One input, no result         |
| `BiConsumer<T, U>`    | `void accept(T, U)` | Two inputs, no result        |
| `Predicate<T>`        | `boolean test(T)`   | One input, `true` or `false` |
| `UnaryOperator<T>`    | `T apply(T)`        | Input and result same type   |
| `BinaryOperator<T>`   | `T apply(T, T)`     | Two inputs, all same type    |
| `Runnable`            | `void run()`        | No input, no result          |

```java
Function<String, Integer> length = s -> s.length();
Supplier<Double> random = () -> Math.random();
Consumer<String> print = s -> System.out.println(s);
Predicate<Integer> isEven = n -> n % 2 == 0;
BinaryOperator<Integer> max = (a, b) -> a > b ? a : b;
```

Primitive versions (`IntPredicate`, `IntFunction<R>`, `ToIntFunction<T>`, `IntBinaryOperator`, and more) avoid boxing `int` into `Integer`.

---

## Lambda Syntax

A lambda is a short method with no name. The `->` separates parameters from the body.

```java
Runnable hello = () -> System.out.println("Hi");     // no parameters

Function<Integer, Integer> square = x -> x * x;      // one parameter, no parentheses needed

BinaryOperator<Integer> larger = (a, b) -> {         // block body needs return
    if (a > b) return a;
    return b;
};

BiFunction<String, Integer, String> repeat =
    (String s, Integer n) -> s.repeat(n);            // explicit types
```

---

## Method References

When a lambda only calls one existing method, `::` names that method instead.

| Kind                     | Example               | Same as                      |
| ------------------------ | --------------------- | ---------------------------- |
| Static method            | `Integer::parseInt`   | `s -> Integer.parseInt(s)`   |
| Method on a given object | `System.out::println` | `x -> System.out.println(x)` |
| Method on the parameter  | `String::toUpperCase` | `s -> s.toUpperCase()`       |
| Constructor              | `ArrayList::new`      | `() -> new ArrayList<>()`    |

```java
List<String> words = List.of("a", "b");
words.forEach(System.out::println);
```

---

## Passing Behavior to Methods

```java
static List<Integer> filter(List<Integer> items, Predicate<Integer> keep) {
    List<Integer> result = new ArrayList<>();
    for (Integer i : items) {
        if (keep.test(i)) result.add(i);
    }
    return result;
}

List<Integer> evens = filter(List.of(1, 2, 3, 4), n -> n % 2 == 0);   // [2, 4]
```

Streams are built on exactly this. See [Streams](41_streams.md).

---

## Composing Functions

The built-in interfaces have default methods that combine them.

```java
Function<Integer, Integer> plusOne = x -> x + 1;
Function<Integer, Integer> twice = x -> x * 2;

plusOne.andThen(twice).apply(3);    // (3 + 1) * 2 = 8
plusOne.compose(twice).apply(3);    // (3 * 2) + 1 = 7

Predicate<String> empty = String::isEmpty;
Predicate<String> notEmpty = empty.negate();
Predicate<String> shortWord = notEmpty.and(s -> s.length() < 4);
```

---

## Capturing Variables

A lambda can use local variables from the method around it, but only if they are **effectively final** (assigned once and never changed).

```java
int factor = 3;
Function<Integer, Integer> times = x -> x * factor;   // ok

int count = 0;
Runnable r = () -> count++;                           // compile error: count changes
```

Java copies the value into the lambda. That is why the variable must not change: the copy and the original would disagree. Fields of objects are not covered by this rule, so `this.count++` is allowed.

---

## Unnamed Parameters (Java 22+)

Use `_` for a parameter you do not need.

```java
BiFunction<Integer, Integer, Integer> first = (a, _) -> a;
```

---

## Full Example

```java
import java.util.List;
import java.util.function.*;

public class Main {
    public static void main(String[] args) {
        Function<Integer, Integer> plusOne = x -> x + 1;
        Function<Integer, Integer> twice = x -> x * 2;
        System.out.println(plusOne.andThen(twice).apply(3));

        Predicate<String> shortWord = s -> s.length() < 4;
        List<String> words = List.of("sun", "planet", "sky", "galaxy");
        for (String w : words) {
            if (shortWord.negate().test(w)) System.out.println("long: " + w);
        }

        Supplier<List<String>> make = () -> List.of("x", "y");
        make.get().forEach(System.out::println);

        int base = 10;
        IntUnaryOperator addBase = n -> n + base;
        System.out.println(addBase.applyAsInt(5));
    }
}
```

Expected output should be:

```
8
long: planet
long: galaxy
x
y
15
```

---

## Gotchas

- Captured local variables must be effectively final; to count inside a lambda, use an `AtomicInteger` or a field
- Checked exceptions cannot be thrown from `Function`, `Consumer`, and friends; catch them inside or write your own interface that declares `throws`
- `this` inside a lambda means the enclosing object; inside an anonymous class it means the anonymous object
- Two functional interfaces with the same shape are still different types: a `Predicate<String>` is not a `Function<String, Boolean>`
- Boxed types (`Function<Integer, Integer>`) allocate in hot loops; use `IntUnaryOperator` and the other primitive versions
- There are no multicast delegates; to call several actions, keep a `List<Runnable>` or chain with `andThen`

---

## Examples

- [39-01](examples/39-01_lambdas_and_functional_interfaces.java): Lambdas and Functional Interfaces

Run one with `java examples/39-01_lambdas_and_functional_interfaces.java`. See [Examples](examples/README.md).
