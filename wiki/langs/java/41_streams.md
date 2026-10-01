# 41 - Streams

## What is a Stream

A **stream** is a pipeline that filters, transforms, and sums up data from a source such as a list or an array. It does not store data, and it does not change the source.

```java
List<Integer> nums = List.of(1, 2, 3, 4, 5, 6);

List<Integer> squaresOfEvens = nums.stream()   // 1. source
    .filter(n -> n % 2 == 0)                   // 2. intermediate step
    .map(n -> n * n)                           // 2. intermediate step
    .toList();                                 // 3. terminal step

System.out.println(squaresOfEvens);   // [4, 16, 36]
```

| Part              | What it does                      | Examples                       |
| ----------------- | --------------------------------- | ------------------------------ |
| Source            | Where items come from             | `list.stream()`, `Stream.of()` |
| Intermediate step | Returns a new stream. Lazy        | `filter`, `map`, `sorted`      |
| Terminal step     | Runs the pipeline, gives a result | `toList`, `count`, `forEach`   |

The lambdas used here are covered in [Lambdas and Functional Interfaces](39_lambdas_and_functional_interfaces.md). Streams are not the same as the byte streams in [File I/O: Streams](47_file_io_streams.md).

---

## Creating Streams

```java
list.stream()                          // from a collection
Stream.of("a", "b", "c")               // from values
Arrays.stream(new int[] {3, 1, 2})     // from an array
IntStream.range(0, 5)                  // 0, 1, 2, 3, 4
IntStream.rangeClosed(1, 5)            // 1, 2, 3, 4, 5
Stream.iterate(1, n -> n * 2).limit(5) // 1, 2, 4, 8, 16
Stream.iterate(1, n -> n < 100, n -> n * 3)   // 1, 3, 9, 27, 81
"hello".chars()                        // IntStream of characters
Files.lines(path)                      // lines of a file, see lesson 45
```

---

## Filtering and Transforming

```java
nums.stream().filter(n -> n % 2 == 0)   // [2, 4, 6]
nums.stream().map(n -> n * n)           // [1, 4, 9, 16, 25, 36]
Stream.of(1, 1, 2, 3, 3).distinct()     // [1, 2, 3]
nums.stream().limit(3)                  // [1, 2, 3]
nums.stream().skip(2)                   // [3, 4, 5, 6]
nums.stream().skip(2).limit(3)          // [3, 4, 5], a page of results
Stream.of(1, 2, 3, 4).takeWhile(n -> n < 3)   // [1, 2]
```

### flatMap: Flatten Nested Data

```java
Stream.of("hello world", "foo bar")
    .flatMap(s -> Arrays.stream(s.split(" ")))
    .toList();   // [hello, world, foo, bar]
```

---

## Sorting

```java
Stream.of("pear", "fig", "apple").sorted()                                   // [apple, fig, pear]
Stream.of("pear", "fig", "apple").sorted(Comparator.reverseOrder())          // [pear, fig, apple]
Stream.of("pear", "fig", "apple").sorted(Comparator.comparing(String::length)) // [fig, pear, apple]

people.stream().sorted(Comparator.comparing(Person::age).reversed())
people.stream().sorted(Comparator.comparing(Person::dept).thenComparing(Person::name))
```

`Comparator` is covered in [equals, hashCode and Comparable](29_equals_hashcode_and_comparable.md).

---

## Terminal Steps

```java
nums.stream().count()                    // 6
nums.stream().reduce(0, Integer::sum)    // 21, combine all items into one
nums.stream().max(Integer::compare)      // Optional[6]
nums.stream().anyMatch(n -> n > 10)      // false, at least one matches
nums.stream().allMatch(n -> n > 0)       // true, every item matches
nums.stream().noneMatch(n -> n < 0)      // true
nums.stream().filter(n -> n > 4).findFirst()    // Optional[5]
nums.stream().filter(n -> n > 10).findFirst()   // Optional.empty
nums.stream().forEach(System.out::println)
```

Steps that may find nothing return an `Optional`. See [Null and Optional](24_null_and_optional.md).

---

## Primitive Streams

`IntStream`, `LongStream`, and `DoubleStream` hold plain numbers, so there is no boxing (wrapping each `int` in an `Integer` object). They add math methods.

```java
nums.stream().mapToInt(Integer::intValue).sum()                  // 21
nums.stream().mapToInt(Integer::intValue).average().orElse(0)    // 3.5
IntStream.range(0, 5).boxed().toList()                           // back to a Stream<Integer>

IntSummaryStatistics st = people.stream().mapToInt(Person::age).summaryStatistics();
// IntSummaryStatistics{count=4, sum=112, min=17, average=28.000000, max=40}
```

---

## Collectors

`collect(...)` builds a result with a **collector**, a recipe from the `Collectors` class.

```java
record Person(String name, int age, String dept) {}

var people = List.of(
    new Person("Alice", 30, "IT"),
    new Person("Bob", 17, "HR"),
    new Person("Cara", 25, "IT"),
    new Person("Dan", 40, "HR"));
```

| Collector                                                   | Result for `people`                       |
| ----------------------------------------------------------- | ----------------------------------------- |
| `joining(", ")` on names                                    | `Alice, Bob, Cara, Dan`                   |
| `groupingBy(Person::dept, counting())`                      | `{HR=2, IT=2}`                            |
| `groupingBy(Person::dept, mapping(Person::name, toList()))` | `{HR=[Bob, Dan], IT=[Alice, Cara]}`       |
| `partitioningBy(p -> p.age() >= 18)`                        | Map with keys `false` and `true`          |
| `toMap(Person::name, Person::age)`                          | `{Alice=30, Bob=17, ...}` in no set order |
| `averagingInt(Person::age)`                                 | `28.0`                                    |

```java
import static java.util.stream.Collectors.*;

Map<String, List<String>> byDept = people.stream()
    .collect(groupingBy(Person::dept, TreeMap::new, mapping(Person::name, toList())));
// {HR=[Bob, Dan], IT=[Alice, Cara]}

Map<Boolean, List<String>> adults = people.stream()
    .collect(partitioningBy(p -> p.age() >= 18, mapping(Person::name, toList())));
// {false=[Bob], true=[Alice, Cara, Dan]}
```

`TreeMap::new` keeps the keys sorted. The plain `groupingBy` returns a `HashMap` with no set order.

---

## Chaining

```java
import java.util.Comparator;
import java.util.List;

public class Chaining {
    record Person(String name, int age) {}

    public static void main(String[] args) {
        var people = List.of(
            new Person("Alice", 30), new Person("Bob", 17),
            new Person("Cara", 25), new Person("Dan", 40));

        List<String> oldestAdults = people.stream()
            .filter(p -> p.age() >= 18)
            .sorted(Comparator.comparing(Person::age).reversed())
            .map(Person::name)
            .limit(2)
            .toList();

        System.out.println(oldestAdults);
    }
}
```

Expected output should be:

```
[Dan, Alice]
```

---

## Streams Are Lazy

Intermediate steps do nothing until a terminal step runs. Then each item goes through the whole pipeline before the next one starts.

```java
import java.util.stream.Stream;

public class Lazy {
    public static void main(String[] args) {
        Stream<String> pipeline = Stream.of("a", "b", "c")
            .map(s -> {
                System.out.println("map " + s);
                return s.toUpperCase();
            });

        System.out.println("nothing has run yet");

        String first = pipeline.filter(s -> !s.equals("A")).findFirst().orElse("none");
        System.out.println(first);
    }
}
```

Expected output should be:

```
nothing has run yet
map a
map b
B
```

`c` is never mapped, because `findFirst` stopped once it had an answer.

---

## Gatherers (Java 24)

A **gatherer** is a custom intermediate step, used with `gather(...)`. The `Gatherers` class has ready-made ones.

```java
Stream.of(1, 2, 3, 4, 5, 6, 7).gather(Gatherers.windowFixed(3)).toList();
// [[1, 2, 3], [4, 5, 6], [7]]
```

| Gatherer               | Does                                           |
| ---------------------- | ---------------------------------------------- |
| `windowFixed(n)`       | Groups items into lists of `n`                 |
| `windowSliding(n)`     | Overlapping groups: `[1,2]`, `[2,3]`, ...      |
| `fold(init, fn)`       | Like `reduce`, but stays a stream              |
| `scan(init, fn)`       | Running totals                                 |
| `mapConcurrent(n, fn)` | Runs `fn` on up to `n` virtual threads at once |

---

## Parallel Streams

`parallelStream()` splits the work across CPU cores.

```java
long count = bigList.parallelStream().filter(this::isPrime).count();
```

It helps only for large, CPU-heavy work with no shared state. For small lists it is slower, because splitting the work has a cost.

---

## Gotchas

- A stream can be used only once. A second terminal step throws `IllegalStateException: stream has already been operated upon or closed`
- `toList()` (Java 16) returns a list you cannot change. Use `collect(Collectors.toList())` or `new ArrayList<>(...)` if you need to add items
- `toMap` throws `IllegalStateException` on a duplicate key. Pass a merge function as the third argument: `toMap(k, v, (a, b) -> a)`
- Do not change outside variables or the source list from inside a lambda. Use `collect` or `reduce` instead
- `Files.lines` and other streams backed by a file must be closed. Use try-with-resources
- A plain loop is often clearer for simple work or when you need `break`

---

## Examples

- [41-01](examples/41-01_chaining.java): Chaining
- [41-02](examples/41-02_streams_are_lazy.java): Streams are lazy

Run one with `java examples/41-01_chaining.java`. See [Examples](examples/README.md).
