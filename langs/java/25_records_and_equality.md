# 25 - Records and Equality

## record (Java 16+)

A **record** is a class made only to carry data. You list the components once, and the compiler writes the rest.

```java
record Person(String name, int age) {}
```

That one line gives you:

| Generated                      | Example                          |
| ------------------------------ | -------------------------------- |
| `private final` fields         | `name`, `age`                    |
| A constructor taking all parts | `new Person("Alice", 30)`        |
| Accessor methods               | `p.name()`, `p.age()` (no `get`) |
| `equals` and `hashCode`        | Compare every component          |
| `toString`                     | `Person[name=Alice, age=30]`     |

---

## Records in Action

```java
import java.util.HashSet;
import java.util.Set;

public class Records {
    record Person(String name, int age) {}

    public static void main(String[] args) {
        Person p1 = new Person("Alice", 30);
        Person p2 = new Person("Alice", 30);

        System.out.println(p1);
        System.out.println(p1.name() + " is " + p1.age());
        System.out.println(p1 == p2);        // different objects
        System.out.println(p1.equals(p2));   // same values

        Set<Person> people = new HashSet<>();
        people.add(p1);
        people.add(p2);
        System.out.println(people.size());
    }
}
```

Expected output should be:

```
Person[name=Alice, age=30]
Alice is 30
false
true
1
```

Records work as `HashSet` items and `HashMap` keys out of the box, because `equals` and `hashCode` agree.

---

## Validating with a Compact Constructor

A **compact constructor** has no parameter list. It runs before the fields are set, so it can check or clean the values.

```java
record Person(String name, int age) {
    Person {
        Objects.requireNonNull(name);
        if (age < 0) throw new IllegalArgumentException("age < 0");
        name = name.strip();          // reassign the parameter, not this.name
    }
}

new Person("  Bob ", 4).name();   // "Bob"
new Person("Eve", -1);            // IllegalArgumentException
```

---

## Adding Methods

A record can have methods, static fields, static factory methods, and extra constructors. It cannot have extra instance fields.

```java
record Money(long cents, String currency) {
    static Money of(double amount, String currency) {
        return new Money(Math.round(amount * 100), currency);
    }

    Money plus(Money other) {
        if (!currency.equals(other.currency)) throw new IllegalArgumentException("currency");
        return new Money(cents + other.cents, currency);
    }

    @Override
    public String toString() {
        return "%d.%02d %s".formatted(cents / 100, cents % 100, currency);
    }
}

Money total = Money.of(1.50, "USD").plus(Money.of(2.25, "USD"));
System.out.println(total);   // 3.75 USD
```

---

## Changing a Record: Make a New One

Fields are `final`, so a record cannot change. Java has no `with` expression yet. Write a "wither" method that returns a copy:

```java
record Person(String name, int age) {
    Person withAge(int newAge) {
        return new Person(name, newAge);
    }
}

Person older = new Person("Alice", 30).withAge(31);
```

---

## Record Rules

| Rule                           | Why                                           |
| ------------------------------ | --------------------------------------------- |
| A record is `final`            | Nothing can extend it                         |
| It cannot extend another class | It already extends `java.lang.Record`         |
| It can implement interfaces    | `record Circle(double r) implements Shape {}` |
| No extra instance fields       | All state is in the header                    |
| Records can be nested or local | Declare one inside a class or method          |

Records also take apart in patterns: `if (o instanceof Person(String n, int a))`. See [Pattern Matching](14_pattern_matching.md).

---

## Two Kinds of Equality

| Check         | Question                    | Works on                  |
| ------------- | --------------------------- | ------------------------- |
| `a == b`      | Are these the same object?  | References and primitives |
| `a.equals(b)` | Do these hold equal values? | Objects                   |

A normal class inherits `equals` from `Object`, which is the same as `==`. So two `new Point(1, 2)` objects of a plain class are **not** equal until you write `equals` and `hashCode`. That is covered in [equals, hashCode and Comparable](29_equals_hashcode_and_comparable.md). A record writes both for you.

`Objects.equals(a, b)` is a null-safe version: it returns `true` when both are `null` and never throws.

---

## How Deep Is Record Equality

A record compares each component with that component's own `equals`.

```java
record Team(String name, List<String> members) {}
record Scores(int[] values) {}

new Team("a", List.of("x")).equals(new Team("a", List.of("x")));   // true: List compares contents

new Scores(new int[] {1}).equals(new Scores(new int[] {1}));       // false: arrays compare references
```

Lists, sets, and maps compare contents. Arrays do not. Use a `List` component, or override `equals` and `hashCode` with `Arrays.equals` and `Arrays.hashCode`.

---

## Record vs Class

| Use    | When                                                     |
| ------ | -------------------------------------------------------- |
| Record | Data that does not change: results, messages, keys, DTOs |
| Class  | Objects with changing state, or that need to be extended |

A **DTO** (data transfer object) is a plain object for moving data between parts of a program, such as a JSON body.

---

## Returning More Than One Value

Java has no tuples. A method returns exactly one value, so to send back several, wrap them in a record. That is the usual answer: named, typed, and compares by value.

```java
record MinMax(int min, int max) {}

static MinMax minMax(int[] nums) {
    int min = nums[0], max = nums[0];
    for (int n : nums) { min = Math.min(min, n); max = Math.max(max, n); }
    return new MinMax(min, max);
}
```

A **record pattern** (Java 21) unpacks the parts into variables. See [Pattern Matching](14_pattern_matching.md).

```java
if (minMax(data) instanceof MinMax(int min, int max)) {
    System.out.println(min + " to " + max);
}
```

For an older style, an array works when the values share a type (`return new int[] {min, max};`), and `Map.entry(key, value)` builds an unchangeable pair. Both lose the names, so prefer a record.

---

## Gotchas

- Accessors are `name()`, not `getName()`; some older libraries expect `get` methods
- Records are shallowly immutable: a `List` component can still be changed unless you copy it with `List.copyOf` in the compact constructor
- Array components break `equals`, `hashCode`, and `toString` (it prints `[I@...`)
- `==` on two records compares references, just like any other object
- In a compact constructor, assign the parameter (`name = ...`), not `this.name`; the field is set after the body runs

---

## Examples

- [25-01](examples/25-01_records_in_action.java): Records in action

Run one with `java examples/25-01_records_in_action.java`. See [Examples](examples/README.md).
