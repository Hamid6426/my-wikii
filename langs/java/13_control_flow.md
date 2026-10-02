# 13 - Control Flow

## if / else if / else

```java
int score = 75;

if (score >= 90) {
    System.out.println("A");
} else if (score >= 80) {
    System.out.println("B");
} else if (score >= 70) {
    System.out.println("C");
} else {
    System.out.println("F");
}
```

Expected output should be:

```
C
```

The condition must be a `boolean`. `if (count)` or `if (x = 5)` does not compile.

---

## Always Use Braces

Without braces, only the next single statement belongs to the `if`.

```java
if (loggedIn)
    System.out.println("Welcome");
    System.out.println("Loading...");   // always runs: the indent lies
```

Write `{ }` even for one line. It prevents this bug when someone adds a line later.

---

## switch Statement: Classic Form

The old `case ...:` form **falls through**: without `break`, it keeps running the next cases.

```java
int day = 3;

switch (day) {
    case 1:
        System.out.println("Monday");
        break;
    case 2:
        System.out.println("Tuesday");
        break;
    case 3:
    case 4:
        System.out.println("Mid-week");   // 3 falls through the empty case to here
        break;
    default:
        System.out.println("Other");
}
```

Forgetting `break` is a classic bug:

```java
int level = 1;

switch (level) {
    case 1:
        System.out.println("one");
    case 2:
        System.out.println("two");       // runs too: no break above
    default:
        System.out.println("default");   // and this
}
```

Expected output should be:

```
one
two
default
```

---

## switch Statement: Arrow Form (Java 14+)

`case ... ->` never falls through. Several values share one case with commas. Prefer this form.

```java
switch (day) {
    case 1 -> System.out.println("Monday");
    case 2 -> System.out.println("Tuesday");
    case 3, 4 -> System.out.println("Mid-week");
    default -> System.out.println("Other");
}
```

---

## switch Expression (Java 14+)

A `switch` can produce a value.

```java
String dayName = switch (day) {
    case 1 -> "Monday";
    case 2 -> "Tuesday";
    case 3 -> "Wednesday";
    default -> "Unknown";
};
```

When a case needs several lines, use a block and give the value with `yield`:

```java
String size = switch (score / 10) {
    case 10, 9 -> "top";
    case 8, 7 -> {
        String note = "good";
        yield note.toUpperCase();     // the value of this case
    }
    default -> "keep going";
};

System.out.println(size);   // GOOD (score is 75, so score / 10 is 7)
```

A switch expression must cover every possible value. For numbers and strings that means a `default`. For enums and sealed types, listing every case is enough (see [Enums](09_enums.md)).

---

## What switch Accepts

| Type                                              | Since                                       |
| ------------------------------------------------- | ------------------------------------------- |
| `int`, `short`, `byte`, `char` and their wrappers | Always                                      |
| Enums                                             | Java 5                                      |
| `String`                                          | Java 7, compared with `equals`              |
| Any object, using type patterns                   | Java 21                                     |
| `long`, `float`, `double`, `boolean`              | Not yet; a preview feature in Java 23 to 25 |

```java
String command = "stop";

switch (command) {
    case "start" -> System.out.println("Starting");
    case "stop" -> System.out.println("Stopping");
    default -> System.out.println("Unknown command");
}
```

---

## Pattern Matching in switch (Java 21+)

A case can test the type of the value and name it in one step. `when` adds an extra condition.

```java
Object obj = 42;

String result = switch (obj) {
    case Integer n when n > 0 -> "Positive int";
    case Integer n            -> "Non-positive int";
    case String s             -> "String: " + s;
    case null                 -> "Null";
    default                   -> "Other";
};

System.out.println(result);   // Positive int
```

Cases are checked top to bottom, so put the more specific ones first. See [Pattern Matching](14_pattern_matching.md) for records and sealed types.

---

## Ternary Operator

An inline `if`/`else` that returns a value.

```java
int age = 20;
String status = age >= 18 ? "Adult" : "Minor";
```

See [Operators](11_operators.md).

---

## Null Checks

```java
String name = null;

if (name == null) {
    System.out.println("no name");
}

String safe = Objects.requireNonNullElse(name, "Guest");   // fallback value
System.out.println(safe);                                  // Guest
```

A `switch` on a `null` value throws `NullPointerException`, unless it has a `case null` (Java 21+). See [Null and Optional](24_null_and_optional.md).

---

## Combining Conditions

`&&` stops early, so a later check can rely on an earlier one. `instanceof` can declare a variable that the rest of the condition uses.

```java
Object text = "hello";

if (text instanceof String s && s.length() > 3) {
    System.out.println("long: " + s);    // long: hello
}
```

---

## No goto

`goto` is a reserved word in Java but does nothing; you cannot use it. To leave nested loops early, use a labeled `break`. See [Loops](15_loops.md).

---

## Gotchas

- The classic `case ...:` form falls through without `break`; use the arrow form `case ... ->` instead
- A switch expression on an `int` or `String` needs a `default`, or it does not compile
- `switch` on a `null` value throws `NullPointerException` unless there is a `case null`
- `switch` cannot use `long`, `float`, `double`, or `boolean` without preview features
- Compare strings in an `if` with `equals`, not `==`; `switch` on a `String` already uses `equals`
- An `if` without braces covers only one statement, whatever the indentation says
- `if (x = 5)` does not compile for an `int`, but `if (flag = true)` compiles for a `boolean` and is always true
