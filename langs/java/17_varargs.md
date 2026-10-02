# 17 - Varargs

## What is Varargs

**Varargs** (variable arguments) lets a method take any number of values of one type. Write `...` after the type.

```java
public class Sum {
    static int sum(int... numbers) {
        int total = 0;
        for (int n : numbers) {
            total += n;
        }
        return total;
    }

    public static void main(String[] args) {
        System.out.println(sum(1, 2, 3, 4, 5));
        System.out.println(sum(7));
        System.out.println(sum());
        System.out.println(sum(new int[] {10, 20}));
    }
}
```

Expected output should be:

```
15
7
0
30
```

Inside the method, `numbers` is a plain `int[]`. The compiler builds that array from the arguments at the call site.

---

## Rules

| Rule                                  | Example                                     |
| ------------------------------------- | ------------------------------------------- |
| Only one varargs parameter per method | `void f(int... a, String... b)` is an error |
| It must be the last parameter         | `void log(String level, Object... parts)`   |
| The caller may pass zero values       | `sum()` gets an empty array, not `null`     |
| The caller may pass an array instead  | `sum(new int[] {1, 2})`                     |

---

## Requiring at Least One Value

`int...` accepts zero values. To force at least one, take the first value as a normal parameter:

```java
static int max(int first, int... rest) {
    int best = first;
    for (int n : rest) {
        if (n > best) best = n;
    }
    return best;
}

max(4, 9, 2);   // 9
max(4);         // 4
// max();       // compile error
```

---

## Varargs in the JDK

You already use varargs methods:

```java
String.format("%s is %d", "Alice", 30);   // Object... args
List<String> names = List.of("a", "b", "c");
Path path = Path.of("home", "alice", "notes.txt");
int biggest = Collections.max(Arrays.asList(3, 8, 1));
```

---

## Varargs and Overloading

The compiler tries methods without varargs first. A varargs method is the last choice.

```java
static void show(int a, int b) { System.out.println("two ints"); }
static void show(int... a)     { System.out.println("varargs"); }

show(1, 2);      // two ints
show(1, 2, 3);   // varargs
```

Two varargs overloads that both fit cause an "ambiguous" compile error. Keep overloads of a varargs method few and clearly different.

---

## Generic Varargs and @SafeVarargs

A varargs parameter of a generic type such as `T...` makes the compiler warn about **heap pollution**: a generic array could end up holding the wrong type. If your method only reads the values, mark it `@SafeVarargs` to say it is safe.

```java
@SafeVarargs
static <T> List<T> listOf(T... items) {
    return new ArrayList<>(Arrays.asList(items));
}
```

`@SafeVarargs` is allowed only on methods that cannot be overridden: `static`, `final`, or `private` ones, and constructors. Generics are covered in [Generics](54_generics.md).

---

## Gotchas

- Passing `null` to `int...` passes a `null` array, and the loop throws `NullPointerException`
- `Arrays.asList(new int[] {1, 2})` gives a `List<int[]>` with one item, because `int[]` is not an array of objects; use `Integer[]` or `IntStream`
- Each call builds a new array, which costs a little in hot loops
- `Object... args` with a single `Object[]` argument passes the array as the whole list, not as one item; cast it to `(Object) arr` to pass it as one item
- Java has no `ref`, `out`, or default parameters; see [Methods](16_methods.md) for overloads instead

---

## Examples

- [17-01](examples/17-01_what_is_varargs.java): What is varargs

Run one with `java examples/17-01_what_is_varargs.java`. See [Examples](examples/README.md).
