# 36 - Collections

## The Collections Framework

The `java.util` package has interfaces that describe what a collection can do, and classes that implement them.

| Interface | Holds                        | Main class   | Other classes                 |
| --------- | ---------------------------- | ------------ | ----------------------------- |
| `List`    | Ordered items, duplicates ok | `ArrayList`  | `LinkedList`                  |
| `Set`     | Unique items                 | `HashSet`    | `LinkedHashSet`, `TreeSet`    |
| `Map`     | Key to value pairs           | `HashMap`    | `LinkedHashMap`, `TreeMap`    |
| `Queue`   | Items waiting in line        | `ArrayDeque` | `PriorityQueue`, `LinkedList` |

Queues and the sorted types are in [Queue, Deque and More Collections](37_queue_deque_and_more_collections.md).

Collections hold objects only. `List<int>` is not allowed; write `List<Integer>`, and Java boxes each `int` into an `Integer` for you.

---

## ArrayList

A resizable array. The most common collection.

```java
List<Integer> numbers = new ArrayList<>(List.of(1, 2, 3));

numbers.add(4);
numbers.addAll(List.of(5, 6));
numbers.add(0, 0);                   // insert at index 0
numbers.remove(Integer.valueOf(3));  // remove the value 3
numbers.remove(0);                   // remove at index 0
numbers.contains(5);                 // true
numbers.size();                      // count
numbers.get(0);                      // read by index
numbers.set(0, 99);                  // replace by index
numbers.sort(null);                  // natural order
numbers.clear();
```

---

## HashMap

Keys map to values. Keys are unique.

```java
Map<String, Integer> ages = new HashMap<>();

ages.put("Alice", 30);
ages.put("Bob", 25);
ages.put("Alice", 31);                        // replaces 30

int bob = ages.get("Bob");                    // 25
Integer dave = ages.get("Dave");              // null: no such key
int carol = ages.getOrDefault("Carol", 0);    // 0
ages.putIfAbsent("Carol", 28);
ages.containsKey("Bob");                      // true

for (Map.Entry<String, Integer> e : ages.entrySet()) {
    System.out.println(e.getKey() + ": " + e.getValue());
}
```

### Counting and Grouping

```java
Map<String, Integer> counts = new HashMap<>();
for (String w : List.of("a", "b", "a")) {
    counts.merge(w, 1, Integer::sum);         // add 1, or start at 1
}

Map<Character, List<String>> byLetter = new HashMap<>();
for (String w : List.of("apple", "avocado", "banana")) {
    byLetter.computeIfAbsent(w.charAt(0), k -> new ArrayList<>()).add(w);
}
```

---

## HashSet

Unique items, no order. Fast `contains`.

```java
Set<Integer> set = new HashSet<>(List.of(1, 2, 3));

set.add(4);
set.add(2);                // already there: returns false, nothing changes
set.remove(1);
set.contains(3);           // true

Set<Integer> other = Set.of(3, 4, 5);
set.addAll(other);         // union
set.retainAll(other);      // intersection
set.removeAll(other);      // difference
```

Sets and map keys rely on `equals` and `hashCode`. See [equals, hashCode and Comparable](29_equals_hashcode_and_comparable.md).

---

## Unmodifiable Collections

The `of` factories (Java 9) build collections that cannot change. Any change throws `UnsupportedOperationException`.

```java
List<String> colors = List.of("red", "green");
Set<Integer> primes = Set.of(2, 3, 5);
Map<String, Integer> ports = Map.of("http", 80, "https", 443);

List<String> copy = List.copyOf(someList);      // unmodifiable copy
```

`List.of` and friends reject `null` items. `Set.of` and `Map.of` throw on duplicates.

---

## Sequenced Collections (Java 21)

Collections with a defined order (`List`, `Deque`, `LinkedHashSet`, `LinkedHashMap`, `TreeSet`, `TreeMap`) share methods for both ends.

```java
List<String> list = new ArrayList<>(List.of("a", "b", "c"));

list.getFirst();          // "a"
list.getLast();           // "c"
list.addFirst("z");       // z, a, b, c
list.reversed();          // a reversed view: c, b, a, z
```

---

## Collection Types for APIs

Pick parameter and return types on purpose.

| Type            | Callers can                               |
| --------------- | ----------------------------------------- |
| `Iterable<T>`   | Loop                                      |
| `Collection<T>` | Loop, `size`, `contains`, and maybe `add` |
| `List<T>`       | Everything above, plus read by index      |
| `Map<K, V>`     | Look up by key                            |

- **Parameters: accept the widest type you need.** `Collection<Integer>` takes a list or a set.
- **Return values: return an unmodifiable view or copy** so callers cannot change your data.

```java
class Library {
    private final List<String> titles = new ArrayList<>();

    void add(String title) { titles.add(title); }

    List<String> titles() { return Collections.unmodifiableList(titles); }  // a read-only view
    List<String> snapshot() { return List.copyOf(titles); }                // a frozen copy
}
```

Java has no separate read-only list interface. An unmodifiable `List` still has `add`, which throws at run time.

---

## Full Example

```java
import java.util.*;

public class Main {
    public static void main(String[] args) {
        List<String> words = List.of("pear", "apple", "pear", "fig", "apple", "pear");

        Map<String, Integer> counts = new TreeMap<>();   // sorted keys for stable output
        for (String w : words) {
            counts.merge(w, 1, Integer::sum);
        }
        System.out.println(counts);

        Set<String> unique = new LinkedHashSet<>(words); // keeps first-seen order
        System.out.println(unique);

        List<String> sorted = new ArrayList<>(unique);
        sorted.sort(null);
        System.out.println(sorted.getFirst() + " .. " + sorted.getLast());
    }
}
```

Expected output should be:

```
{apple=2, fig=1, pear=3}
[pear, apple, fig]
apple .. pear
```

---

## Gotchas

- `list.remove(1)` on a `List<Integer>` removes **index** 1; use `list.remove(Integer.valueOf(1))` to remove the value
- Changing a collection inside a for-each loop throws `ConcurrentModificationException`; use `removeIf` or an iterator (see [Iterators](38_iterators.md))
- `HashMap` and `HashSet` have no stable order; use `LinkedHashMap` for insertion order or `TreeMap` for sorted order
- `Arrays.asList(array)` is fixed-size: `add` throws, but `set` writes through to the array
- `map.get(key)` returns `null` both for a missing key and for a key mapped to `null`; use `containsKey` to tell them apart
- `ArrayList.contains` checks every item; use a `HashSet` for fast lookups

---

## Examples

- [36-01](examples/36-01_collections.java): Collections

Run one with `java examples/36-01_collections.java`. See [Examples](examples/README.md).
