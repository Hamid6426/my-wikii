# 15 - Loops

## for Loop

Best when the number of iterations is known.

```java
for (int i = 0; i < 5; i++) {
    System.out.println(i);   // 0, 1, 2, 3, 4
}
```

Counting down:

```java
for (int i = 10; i >= 0; i--) {
    System.out.println(i);
}
```

Any of the three parts can hold more than one statement, separated by commas:

```java
for (int i = 0, j = 10; i < j; i++, j--) {
    System.out.println(i + " " + j);
}
```

---

## while Loop

Runs while the condition is true. The condition is checked before each iteration.

```java
int n = 0;
while (n < 5) {
    System.out.println(n);
    n++;
}
```

---

## do-while Loop

Runs at least once. The condition is checked after each iteration.

```java
int n = 0;
do {
    System.out.println(n);
    n++;
} while (n < 5);
```

---

## Enhanced for Loop

Also called the **for-each** loop. Walks over an array or anything that is `Iterable` (lists, sets). There is no index.

```java
String[] names = {"Alice", "Bob", "Carol"};
for (String name : names) {
    System.out.println(name);
}
```

Works with collections too (see [Collections](36_collections.md)):

```java
List<Integer> numbers = List.of(1, 2, 3, 4, 5);
for (int n : numbers) {
    System.out.println(n);
}
```

Walking a `Map`:

```java
Map<String, Integer> ages = Map.of("Alice", 30, "Bob", 25);
for (Map.Entry<String, Integer> e : ages.entrySet()) {
    System.out.println(e.getKey() + " is " + e.getValue());
}
```

---

## break

Exits the loop immediately.

```java
for (int i = 0; i < 10; i++) {
    if (i == 5) break;
    System.out.println(i);   // 0, 1, 2, 3, 4
}
```

---

## continue

Skips the rest of the current iteration and moves to the next.

```java
for (int i = 0; i < 5; i++) {
    if (i == 2) continue;
    System.out.println(i);   // 0, 1, 3, 4
}
```

---

## Nested Loops and Labels

```java
for (int i = 0; i < 3; i++) {
    for (int j = 0; j < 3; j++) {
        System.out.print("(" + i + "," + j + ") ");
    }
    System.out.println();
}
```

A plain `break` only leaves the innermost loop. Put a **label** (a name followed by `:`) on the outer loop to leave both at once.

```java
public class FindPair {
    public static void main(String[] args) {
        int[][] grid = {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };

        outer:
        for (int row = 0; row < grid.length; row++) {
            for (int col = 0; col < grid[row].length; col++) {
                if (grid[row][col] == 5) {
                    System.out.println("found 5 at " + row + "," + col);
                    break outer;
                }
                System.out.println("checked " + grid[row][col]);
            }
        }
    }
}
```

Expected output should be:

```
checked 1
checked 2
checked 3
checked 4
found 5 at 1,1
```

`continue outer;` works the same way: it jumps to the next iteration of the labeled loop.

---

## Infinite Loop

```java
Scanner in = new Scanner(System.in);
while (true) {
    String input = in.nextLine();
    if (input.equals("quit")) break;
    System.out.println("You typed " + input);
}
```

`for (;;)` is the same thing written another way.

---

## Need the Index Too

The enhanced `for` loop has no index. Use a classic `for` loop, or `IntStream.range` (see [Streams](41_streams.md)).

```java
List<String> names = List.of("Alice", "Bob");

for (int i = 0; i < names.size(); i++) {
    System.out.println(i + ": " + names.get(i));
}

IntStream.range(0, names.size())
         .forEach(i -> System.out.println(i + ": " + names.get(i)));
```

---

## forEach with a Lambda

Collections and maps have a `forEach` method that takes a lambda. See [Lambdas and Functional Interfaces](39_lambdas_and_functional_interfaces.md).

```java
names.forEach(name -> System.out.println(name));
names.forEach(System.out::println);           // same thing, shorter
ages.forEach((name, age) -> System.out.println(name + " " + age));
```

You cannot `break` or `continue` out of a `forEach` lambda. Use a normal loop when you need them.

---

## Gotchas

- A `for` loop variable exists only inside the loop, not after it
- Adding or removing items from a collection inside an enhanced `for` loop throws `ConcurrentModificationException`; use an `Iterator` and its `remove()`, or `removeIf` (see [Iterators and Iterable](38_iterators.md))
- Assigning to the loop variable of an enhanced `for` loop changes only the local copy, not the array
- `do-while` always runs the body at least once, even if the condition starts out false
- A lambda cannot use the counter of a classic `for` loop, because `i` changes; copy it to a local first (`int index = i;`)
- `Map.of` and `Set.of` have no fixed order, so the order you see when looping can change between runs

---

## Examples

- [15-01](examples/15-01_nested_loops_and_labels.java): Nested loops and labels

Run one with `java examples/15-01_nested_loops_and_labels.java`. See [Examples](examples/README.md).
