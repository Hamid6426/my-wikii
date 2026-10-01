# 38 - Iterators and Iterable

## Iterator

An `Iterator<T>` hands out the items of a collection one at a time.

| Method      | Does                                                      |
| ----------- | --------------------------------------------------------- |
| `hasNext()` | `true` if there is another item                           |
| `next()`    | Returns the next item, or throws `NoSuchElementException` |
| `remove()`  | Removes the last item `next()` returned (optional)        |

```java
List<String> names = List.of("Ana", "Ben", "Cy");

Iterator<String> it = names.iterator();
while (it.hasNext()) {
    System.out.println(it.next());   // Ana, Ben, Cy
}
```

---

## Iterable and the for-each Loop

Any object whose class implements `Iterable<T>` works in a for-each loop. The compiler turns the loop into the `iterator()` code above.

```java
for (String name : names) {          // same as the while loop above
    System.out.println(name);
}
```

All collections are `Iterable`. Arrays work in for-each too, but they are not `Iterable`.

---

## Removing While Looping

Changing a collection inside a for-each loop throws `ConcurrentModificationException`. Remove through the iterator, or use `removeIf`.

```java
List<Integer> nums = new ArrayList<>(List.of(1, 2, 3, 4));

Iterator<Integer> it = nums.iterator();
while (it.hasNext()) {
    if (it.next() % 2 == 0) it.remove();   // safe
}

nums.removeIf(n -> n > 2);                 // shorter, same idea
```

The collection iterators are **fail-fast**: they notice outside changes and throw instead of returning wrong data.

---

## Writing Your Own Iterable

Java has no `yield return`. To make a type work with for-each, implement `Iterable<T>` and return an `Iterator<T>` that keeps its own position.

```java
import java.util.Iterator;
import java.util.NoSuchElementException;

public class Main {
    public static void main(String[] args) {
        for (int n : new Range(1, 4)) {
            System.out.println(n);
        }
    }
}

record Range(int start, int end) implements Iterable<Integer> {
    @Override
    public Iterator<Integer> iterator() {
        return new Iterator<>() {
            private int current = start;

            @Override
            public boolean hasNext() { return current < end; }

            @Override
            public Integer next() {
                if (!hasNext()) throw new NoSuchElementException();
                return current++;
            }
        };
    }
}
```

Expected output should be:

```
1
2
3
```

Each call to `iterator()` returns a fresh iterator, so the same `Range` can be looped over many times.

---

## Lazy Sequences

An iterator computes each value only when `next()` is called, so a sequence can be endless.

```java
Iterator<Integer> naturals = new Iterator<>() {
    private int i = 0;
    public boolean hasNext() { return true; }      // never ends
    public Integer next() { return i++; }
};
```

For most lazy work, streams are shorter. See [Streams](41_streams.md).

```java
Stream.iterate(0, i -> i + 1)     // 0, 1, 2, ... computed on demand
      .limit(5)                   // stop, or it never ends
      .forEach(System.out::println);
```

---

## ListIterator

A `ListIterator<T>` (from `list.listIterator()`) can also go backwards and replace items.

```java
List<String> words = new ArrayList<>(List.of("a", "b", "c"));

ListIterator<String> li = words.listIterator();
while (li.hasNext()) {
    li.set(li.next().toUpperCase());   // A, B, C
}
while (li.hasPrevious()) {
    System.out.print(li.previous());   // CBA
}
```

---

## forEach and Spliterator

`Iterable` has a default `forEach` method that takes a lambda (see [Lambdas](39_lambdas_and_functional_interfaces.md)).

```java
names.forEach(n -> System.out.println(n));
```

A `Spliterator` is an iterator that can split its items into parts, so streams can process them in parallel. You rarely write one yourself.

---

## Gotchas

- Calling `next()` without `hasNext()` throws `NoSuchElementException` at the end
- `remove()` throws `IllegalStateException` if called twice in a row or before `next()`
- Iterators of `List.of(...)` and other unmodifiable collections throw `UnsupportedOperationException` on `remove()`
- An `Iterator` is single use; ask the `Iterable` for a new one to loop again
- A `Stream` is single use too, and is not `Iterable`
- Your own iterator should throw `NoSuchElementException` from `next()` when it is empty, as the contract says

---

## Examples

- [38-01](examples/38-01_writing_your_own_iterable.java): Writing your own iterable

Run one with `java examples/38-01_writing_your_own_iterable.java`. See [Examples](examples/README.md).
