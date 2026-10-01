# 37 - Queue, Deque and More Collections

## Queue

FIFO: first in, first out. Each operation comes in two forms, one that throws and one that returns a special value.

| Job            | Throws on failure | Returns `false` or `null` |
| -------------- | ----------------- | ------------------------- |
| Add to back    | `add(x)`          | `offer(x)`                |
| Take the front | `remove()`        | `poll()`                  |
| Look at front  | `element()`       | `peek()`                  |

```java
Queue<String> queue = new ArrayDeque<>();

queue.offer("first");
queue.offer("second");
queue.offer("third");

String next = queue.poll();   // "first": removed
String peek = queue.peek();   // "second": not removed
queue.size();                 // 2
```

---

## Deque: Both Ends

A deque (double-ended queue, said "deck") adds and removes at both ends. `ArrayDeque` is the class to use for both queues and stacks.

```java
Deque<Integer> deque = new ArrayDeque<>();

deque.offerFirst(1);
deque.offerLast(2);
deque.pollFirst();     // 1
deque.pollLast();      // 2
```

---

## Stack (LIFO)

LIFO: last in, first out. Use `ArrayDeque` with `push`, `pop`, and `peek`.

```java
Deque<Integer> stack = new ArrayDeque<>();

stack.push(1);
stack.push(2);
stack.push(3);

int top = stack.pop();    // 3: removed
int peek = stack.peek();  // 2: not removed
```

The old `java.util.Stack` class still exists, but it is synchronized (slower) and extends `Vector`. Prefer `ArrayDeque`.

---

## PriorityQueue

Always hands out the smallest item first, by natural order or by a `Comparator`. It is a binary heap, not a sorted list.

```java
Queue<Integer> pq = new PriorityQueue<>(List.of(5, 1, 3));
pq.poll();   // 1
pq.poll();   // 3

Queue<String> longestFirst =
    new PriorityQueue<>(Comparator.comparingInt(String::length).reversed());
```

Printing a `PriorityQueue` or looping over it does **not** show sorted order. Only `poll()` does.

---

## TreeMap and TreeSet

Kept sorted by key at all times (a red-black tree). They also answer "nearest" questions.

```java
TreeMap<Integer, String> grades = new TreeMap<>(Map.of(90, "A", 80, "B", 70, "C"));

grades.firstKey();           // 70
grades.floorKey(85);         // 80: largest key <= 85
grades.ceilingKey(85);       // 90: smallest key >= 85
grades.headMap(80);          // {70=C}: keys below 80
grades.descendingMap();      // {90=A, 80=B, 70=C}

TreeSet<String> names = new TreeSet<>(List.of("cy", "al", "bo"));
names.first();               // "al"
```

---

## LinkedHashMap and LinkedHashSet

Like `HashMap` and `HashSet`, but they remember insertion order. `LinkedHashMap` can also be an LRU cache (least recently used items are dropped first).

```java
class LruCache<K, V> extends LinkedHashMap<K, V> {
    private final int max;

    LruCache(int max) {
        super(16, 0.75f, true);     // true: order by last access, not insertion
        this.max = max;
    }

    @Override
    protected boolean removeEldestEntry(Map.Entry<K, V> eldest) {
        return size() > max;        // drop the oldest when over the limit
    }
}
```

---

## EnumMap and EnumSet

Fast, compact collections for enum keys. See [Enums](09_enums.md).

```java
enum Day { MON, TUE, WED, THU, FRI, SAT, SUN }

EnumSet<Day> weekend = EnumSet.of(Day.SAT, Day.SUN);
EnumSet<Day> weekdays = EnumSet.complementOf(weekend);

EnumMap<Day, Integer> hours = new EnumMap<>(Day.class);
hours.put(Day.MON, 8);
```

---

## Full Example

```java
import java.util.*;

public class Main {
    public static void main(String[] args) {
        Deque<String> stack = new ArrayDeque<>();
        for (String s : List.of("a", "b", "c")) stack.push(s);
        System.out.println("stack pop: " + stack.pop());

        Queue<String> queue = new ArrayDeque<>(List.of("a", "b", "c"));
        System.out.println("queue poll: " + queue.poll());

        PriorityQueue<Integer> pq = new PriorityQueue<>(List.of(5, 1, 4, 2));
        StringBuilder order = new StringBuilder();
        while (!pq.isEmpty()) order.append(pq.poll()).append(' ');
        System.out.println("priority: " + order.toString().trim());

        TreeMap<Integer, String> grades = new TreeMap<>(Map.of(90, "A", 80, "B", 70, "C"));
        System.out.println("85 gets " + grades.floorEntry(85).getValue());

        LinkedHashMap<String, Integer> lru = new LinkedHashMap<>(16, 0.75f, true) {
            @Override
            protected boolean removeEldestEntry(Map.Entry<String, Integer> e) {
                return size() > 2;
            }
        };
        lru.put("x", 1);
        lru.put("y", 2);
        lru.get("x");              // x is now the most recent
        lru.put("z", 3);           // y is dropped
        System.out.println("lru: " + lru.keySet());
    }
}
```

Expected output should be:

```
stack pop: c
queue poll: a
priority: 1 2 4 5
85 gets B
lru: [x, z]
```

---

## Collection Comparison

| Collection      | Order               | Duplicates | Add / find           |
| --------------- | ------------------- | ---------- | -------------------- |
| `ArrayList`     | Insertion           | Yes        | O(1) / O(n)          |
| `LinkedList`    | Insertion           | Yes        | O(1) at ends / O(n)  |
| `ArrayDeque`    | Both ends           | Yes        | O(1) at ends         |
| `PriorityQueue` | Smallest first      | Yes        | O(log n) / O(1) peek |
| `HashSet`       | None                | No         | O(1) / O(1)          |
| `LinkedHashSet` | Insertion           | No         | O(1) / O(1)          |
| `TreeSet`       | Sorted              | No         | O(log n) / O(log n)  |
| `HashMap`       | None                | Keys: no   | O(1) / O(1)          |
| `LinkedHashMap` | Insertion or access | Keys: no   | O(1) / O(1)          |
| `TreeMap`       | Sorted by key       | Keys: no   | O(log n) / O(log n)  |

For collections shared between threads, use `java.util.concurrent` (`ConcurrentHashMap`, `ConcurrentLinkedQueue`, `LinkedBlockingQueue`). See [Threading](53_threading.md).

---

## Gotchas

- `ArrayDeque` does not accept `null`; `poll()` uses `null` to mean "empty"
- `remove()`, `pop()`, and `element()` throw `NoSuchElementException` on an empty collection; `poll()` and `peek()` return `null`
- Iterating a `PriorityQueue` gives heap order, not sorted order
- `LinkedList` is rarely faster than `ArrayList` or `ArrayDeque` in practice, because its nodes are scattered in memory
- A `TreeMap` or `TreeSet` uses `compareTo`, not `equals`, to find duplicates

---

## Examples

- [37-01](examples/37-01_queue_deque_and_more_collections.java): Queue, Deque and More Collections

Run one with `java examples/37-01_queue_deque_and_more_collections.java`. See [Examples](examples/README.md).
