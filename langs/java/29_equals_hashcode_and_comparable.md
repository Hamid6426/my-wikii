# 29 - equals, hashCode and Comparable

## No Operator Overloading

Java does not let you redefine `+`, `==`, or `<` for your own types. Named methods do that job instead.

| You want            | C# style        | Java                             |
| ------------------- | --------------- | -------------------------------- |
| Same value          | `a == b`        | `a.equals(b)`                    |
| Hash for sets, maps | `GetHashCode()` | `a.hashCode()`                   |
| Order (less, more)  | `a < b`         | `a.compareTo(b) < 0`             |
| Add two values      | `a + b`         | `a.plus(b)` (a method you write) |

`BigDecimal` and `BigInteger` work this way: `a.add(b)`, `a.compareTo(b)`.

---

## == vs equals

`==` on objects checks whether two references point to the **same object**. `equals` checks whether they hold the **same value**.

```java
String a = new String("hi");
String b = new String("hi");

System.out.println(a == b);        // false: two objects
System.out.println(a.equals(b));   // true: same text
```

The default `equals` from `Object` is the same as `==`. Override it to compare values. See [Reference Data Types](22_reference_data_types.md).

---

## Overriding equals

```java
import java.util.Objects;

final class Money {
    private final long cents;
    private final String currency;

    Money(long cents, String currency) {
        this.cents = cents;
        this.currency = currency;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;                         // same object
        if (!(o instanceof Money other)) return false;      // null or other type
        return cents == other.cents
            && currency.equals(other.currency);
    }

    @Override
    public int hashCode() {
        return Objects.hash(cents, currency);
    }
}
```

The parameter must be `Object`, not `Money`. `equals(Money m)` is an overload, not an override, and collections never call it. `@Override` catches that mistake.

---

## The hashCode Contract

`HashMap` and `HashSet` use `hashCode` to pick a bucket, then `equals` to confirm a match.

| Rule                                                  | Breaks if you get it wrong                |
| ----------------------------------------------------- | ----------------------------------------- |
| Equal objects must have equal hash codes              | `set.contains(x)` returns `false` wrongly |
| Hash code stays the same while the object is in a set | The object gets lost inside the set       |
| Unequal objects may share a hash code                 | Nothing breaks, it is only slower         |

Always override `equals` and `hashCode` together, from the same fields.

---

## Helpers in Objects

| Method                  | Does                                        |
| ----------------------- | ------------------------------------------- |
| `Objects.equals(a, b)`  | `equals` that is safe when either is `null` |
| `Objects.hash(a, b, c)` | Combines field hashes into one              |
| `Objects.hashCode(a)`   | `0` for `null`, else `a.hashCode()`         |

---

## Records Do It for You

A record writes `equals`, `hashCode`, and `toString` from its fields. Use one when a class is just data. See [Records and Equality](25_records_and_equality.md).

```java
record Point(int x, int y) { }

System.out.println(new Point(1, 2).equals(new Point(1, 2)));   // true
```

---

## Comparable: Natural Order

`Comparable<T>` gives a type one built-in order. `compareTo` returns a negative number, zero, or a positive number.

```java
record Version(int major, int minor) implements Comparable<Version> {
    @Override
    public int compareTo(Version other) {
        int byMajor = Integer.compare(major, other.major);
        return byMajor != 0 ? byMajor : Integer.compare(minor, other.minor);
    }
}
```

| `a.compareTo(b)` | Means           |
| ---------------- | --------------- |
| `< 0`            | `a` comes first |
| `0`              | Same position   |
| `> 0`            | `b` comes first |

`Collections.sort`, `List.sort(null)`, `TreeSet`, and `TreeMap` all use it.

---

## Comparator: Any Other Order

A `Comparator<T>` is an order kept outside the class. Build one from small parts.

```java
import java.util.Comparator;

record Person(String name, int age) { }

Comparator<Person> byAge = Comparator.comparingInt(Person::age);
Comparator<Person> byAgeThenName = byAge.thenComparing(Person::name);
Comparator<Person> oldestFirst = byAge.reversed();
```

`Person::age` is a method reference, covered in [Lambdas and Functional Interfaces](39_lambdas_and_functional_interfaces.md).

---

## Full Example

```java
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Objects;
import java.util.Set;

public class Main {
    public static void main(String[] args) {
        Money a = new Money(500, "USD");
        Money b = new Money(500, "USD");
        System.out.println("== " + (a == b));
        System.out.println("equals " + a.equals(b));

        Set<Money> set = new HashSet<>();
        set.add(a);
        System.out.println("contains " + set.contains(b));

        List<Money> list = new ArrayList<>(List.of(
            new Money(900, "USD"), new Money(100, "USD"), new Money(500, "USD")));
        list.sort(null);                   // null means natural order
        System.out.println(list);
        System.out.println(a.plus(b));
    }
}

final class Money implements Comparable<Money> {
    private final long cents;
    private final String currency;

    Money(long cents, String currency) {
        this.cents = cents;
        this.currency = currency;
    }

    Money plus(Money other) {
        if (!currency.equals(other.currency)) throw new IllegalArgumentException("currency");
        return new Money(cents + other.cents, currency);
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (!(o instanceof Money other)) return false;
        return cents == other.cents && currency.equals(other.currency);
    }

    @Override
    public int hashCode() {
        return Objects.hash(cents, currency);
    }

    @Override
    public int compareTo(Money other) {
        return Long.compare(cents, other.cents);
    }

    @Override
    public String toString() {
        return currency + " " + cents / 100 + "." + String.format("%02d", cents % 100);
    }
}
```

Expected output should be:

```
== false
equals true
contains true
[USD 1.00, USD 5.00, USD 9.00]
USD 10.00
```

Remove the `hashCode` method and `contains` prints `false` (almost always): the two equal objects get different hash codes and land in different buckets.

---

## Gotchas

- Overriding `equals` without `hashCode` breaks `HashSet` and `HashMap` silently
- Do not use mutable fields in `hashCode` for objects stored in a hash set or as map keys
- `compareTo` should return `0` only when `equals` is `true`; `BigDecimal` breaks this (`2.0` and `2.00` compare equal but are not `equals`), so a `TreeSet` and a `HashSet` disagree on it
- Never compare with `a - b` in `compareTo`: it overflows for large values; use `Integer.compare(a, b)`
- `==` on boxed numbers like `Integer` compares references and only seems to work for small values (-128 to 127, which are cached)
- Comparing `float` or `double` fields in `equals`: use `Double.compare(a, b) == 0`, which handles `NaN` and `-0.0`

---

## Examples

- [29-01](examples/29-01_equals_hashcode_and_comparable.java): equals, hashCode and Comparable

Run one with `java examples/29-01_equals_hashcode_and_comparable.java`. See [Examples](examples/README.md).
