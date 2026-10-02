# 09 - Enums

## What is an Enum

A type with a fixed set of named values. Use it instead of magic numbers or strings.

```java
enum Status {
    PENDING,
    ACTIVE,
    CLOSED
}

Status s = Status.ACTIVE;
System.out.println(s);             // ACTIVE
System.out.println(s.ordinal());   // 1 (position, starting at 0)
System.out.println(s.name());      // ACTIVE
```

Constants are written in `UPPER_SNAKE_CASE`. An enum can sit in its own file (`Status.java`) or inside a class.

---

## Enums are Classes

In Java an enum is a full class. Each constant is one object, created once. It can have fields, a constructor, and methods.

```java
enum HttpCode {
    OK(200, "Ok"),
    NOT_FOUND(404, "Not Found"),
    SERVER_ERROR(500, "Server Error");   // semicolon before the members

    private final int code;
    private final String text;

    HttpCode(int code, String text) {     // constructor is always private
        this.code = code;
        this.text = text;
    }

    int code() { return code; }
    String text() { return text; }

    boolean isError() { return code >= 400; }
}

HttpCode nf = HttpCode.NOT_FOUND;
System.out.println(nf.code() + " " + nf.text() + " " + nf.isError());
```

Expected output should be:

```
404 Not Found true
```

There is no `new HttpCode(...)`. The constants are the only instances.

---

## Converting

```java
Status p = Status.valueOf("PENDING");    // from a name, exact case
String name = p.name();                  // to a name: "PENDING"
int index = p.ordinal();                 // to its position: 0
Status third = Status.values()[2];       // from a position: CLOSED
```

`valueOf` throws `IllegalArgumentException` when no constant has that name, including a different case such as `"pending"`. Java has no `TryParse`; catch the exception or loop over `values()`.

Lookup by your own field:

```java
static HttpCode fromCode(int code) {      // inside the HttpCode enum
    for (HttpCode c : values()) {
        if (c.code == code) return c;
    }
    throw new IllegalArgumentException("Unknown code: " + code);
}
```

`HttpCode.fromCode(500)` returns `SERVER_ERROR`.

---

## Looping Over Values

```java
for (Status s : Status.values()) {
    System.out.println(s + " = " + s.ordinal());
}
```

Expected output should be:

```
PENDING = 0
ACTIVE = 1
CLOSED = 2
```

`values()` returns a new array each call, in declaration order.

---

## Enums in switch

```java
String label = switch (status) {
    case PENDING -> "Waiting";
    case ACTIVE  -> "Running";
    case CLOSED  -> "Done";
};
```

A switch expression that covers every constant needs no `default`. If someone adds a constant later, the compiler points at every switch that misses it. See [Control Flow](13_control_flow.md).

---

## Different Behavior per Constant

Each constant can have its own body for a method.

```java
enum Operation {
    ADD {
        int apply(int a, int b) { return a + b; }
    },
    MULTIPLY {
        int apply(int a, int b) { return a * b; }
    };

    abstract int apply(int a, int b);
}

System.out.println(Operation.ADD.apply(2, 3));        // 5
System.out.println(Operation.MULTIPLY.apply(2, 3));   // 6
```

A switch inside one method is often easier to read. Use this when each constant has a lot of its own logic.

---

## EnumSet Instead of Flags

Java has no `[Flags]` enums. Use `EnumSet` (in `java.util`) to hold several constants at once. It is stored as bits, so it is as fast as flags.

```java
enum Permission { READ, WRITE, EXECUTE }

Set<Permission> perms = EnumSet.of(Permission.READ, Permission.WRITE);
System.out.println(perms);                          // [READ, WRITE]
System.out.println(perms.contains(Permission.WRITE)); // true
perms.remove(Permission.WRITE);
System.out.println(perms);                          // [READ]

Set<Permission> all = EnumSet.allOf(Permission.class);   // [READ, WRITE, EXECUTE]
Set<Permission> none = EnumSet.noneOf(Permission.class); // []
```

---

## EnumMap

A map with enum keys, kept in declaration order.

```java
Map<Status, Integer> counts = new EnumMap<>(Status.class);
counts.put(Status.CLOSED, 4);
counts.put(Status.PENDING, 1);
System.out.println(counts);     // {PENDING=1, CLOSED=4}
```

See [Collections](36_collections.md).

---

## Comparing

```java
boolean same = s == Status.ACTIVE;                           // true: there is only one ACTIVE
boolean before = Status.PENDING.compareTo(Status.CLOSED) < 0; // true: compares declaration order
```

`==` is the normal way to compare enums. It never throws, even when `s` is `null`.

---

## Gotchas

- `valueOf` is case-sensitive and throws on unknown names; check user input before calling it
- `switch` on a `null` enum throws `NullPointerException`
- Storing `ordinal()` in a database or file breaks when someone reorders or inserts a constant; store `name()` or your own code field
- `values()` copies the array each time; cache it in a `static final` field if you call it in a hot loop
- An enum constructor runs once per constant, when the enum class first loads; it cannot be `public`
- Enums cannot extend another class, because they already extend `java.lang.Enum`; they can implement interfaces
