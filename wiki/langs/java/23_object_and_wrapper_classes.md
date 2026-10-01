# 23 - Object and Wrapper Classes

## The Object Class

Every class implicitly extends `java.lang.Object`, so every value that is not a primitive has these methods.

| Method                              | Default behavior                                       |
| ----------------------------------- | ------------------------------------------------------ |
| `getClass()`                        | The runtime type, read-only                            |
| `toString()`                        | `ClassName@hexHashCode`                                |
| `equals(o)`                         | Same as `==` (identity)                                |
| `hashCode()`                        | Identity-based number                                  |
| `clone()`                           | Shallow copy; `protected`, needs `Cloneable`           |
| `wait()`, `notify()`, `notifyAll()` | Thread coordination (see [Threading](53_threading.md)) |

```java
class Money { }
Object o = new Money();
System.out.println(o.getClass().getName());   // Money (the full package name when it has one)
System.out.println(o.equals(o));              // true
```

Only `toString`, `equals`, and `hashCode` are commonly overridden. See [equals, hashCode and Comparable](29_equals_hashcode_and_comparable.md).

---

## Wrapper Classes

Each primitive has an object version in `java.lang`. Wrappers are objects, so they can be `null` and can be used where a type parameter or a collection element is needed.

| Primitive | Wrapper     | Note                           |
| --------- | ----------- | ------------------------------ |
| `byte`    | `Byte`      |                                |
| `short`   | `Short`     |                                |
| `int`     | `Integer`   | Most common; keys and counters |
| `long`    | `Long`      | Ids and timestamps             |
| `float`   | `Float`     |                                |
| `double`  | `Double`    |                                |
| `char`    | `Character` |                                |
| `boolean` | `Boolean`   |                                |

Wrappers are immutable, like `String`.

---

## Boxing and Unboxing

**Autoboxing** is the compiler wrapping a primitive in its wrapper automatically. **Unboxing** takes the primitive back out.

```java
Integer boxed = 42;         // autoboxing
int value = boxed;          // unboxing

List<Integer> nums = new ArrayList<>();
nums.add(1);                // autoboxing
int first = nums.get(0);    // unboxing
```

---

## The Wrapper Cache

`Integer.valueOf` caches the values -128 to 127, and `Boolean`, `Byte`, `Short`, and `Character` do the same for their range. So `==` is true for small wrappers and false above the cache, which is a trap.

```java
Integer a = 127, b = 127;
Integer c = 1000, d = 1000;
System.out.println(a == b);        // true, cached
System.out.println(c == d);        // false, different objects
System.out.println(c.equals(d));   // true, same value
```

Never compare wrappers with `==`; use `equals`, or unbox first.

---

## Useful Wrapper Methods

The wrapper classes are also where parsing and number helpers live.

| Call                          | Result                          |
| ----------------------------- | ------------------------------- |
| `Integer.parseInt("ff", 16)`  | `255`                           |
| `Integer.toBinaryString(5)`   | `"101"`                         |
| `Integer.MAX_VALUE`           | `2147483647`                    |
| `Double.isNaN(x)`             | Is the value NaN                |
| `Character.isLetter(c)`       | Is it a letter                  |
| `Boolean.parseBoolean("yes")` | `false` (only `"true"` is true) |
| `Long.compare(a, b)`          | Compare without boxing          |

The `Objects` class is a related helper, not a wrapper: `Objects.equals`, `Objects.hash`, `Objects.requireNonNull`.

---

## Primitive or Wrapper

| Use a primitive when              | Use a wrapper when                |
| --------------------------------- | --------------------------------- |
| Always present, and speed matters | Generic type or collection item   |
| Local math and loop counters      | The value may be missing (`null`) |
| Fields and parameters by default  | You need the helper methods       |

---

## Gotchas

- Unboxing a `null` wrapper throws `NullPointerException`
- `==` compares references for wrappers; use `equals` or unbox
- The cache covers -128 to 127 only, so `==` results are inconsistent
- Boxing in a tight loop allocates; keep primitives on hot paths (see [Performance Basics](61_performance_basics.md))
- `Boolean.parseBoolean` returns `false` for anything that is not `"true"`, including `"yes"`

---

## Full Example

```java
public class Wrappers {
    public static void main(String[] args) {
        Integer a = 127, b = 127;
        Integer c = 1000, d = 1000;
        System.out.println(a == b);        // cached
        System.out.println(c == d);        // not cached
        System.out.println(c.equals(d));   // same value

        System.out.println(Integer.parseInt("ff", 16));
        System.out.println(Integer.toBinaryString(5));

        Object o = new Wrappers();
        System.out.println(o.getClass().getSimpleName());
        System.out.println(o instanceof Object);
    }
}
```

Expected output should be:

```
true
false
true
255
101
Wrappers
true
```

---

## Examples

- [23-01](examples/23-01_object_and_wrapper_classes.java): Object and wrapper classes

Run one with `java examples/23-01_object_and_wrapper_classes.java`. See [Examples](examples/README.md).
