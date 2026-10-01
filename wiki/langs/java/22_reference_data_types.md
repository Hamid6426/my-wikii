# 22 - Reference Data Types

## Primitives vs References

Java has 8 **primitive** types (`int`, `double`, `boolean`, `char`, and the rest, see [Primitive Data Types](08_primitive_data_types.md)). Everything else is a **reference type**: classes, records, interfaces, enums, arrays, and `String`.

| Feature        | Primitive (`int`, `boolean`)    | Reference (`String`, arrays, objects)   |
| -------------- | ------------------------------- | --------------------------------------- |
| Variable holds | The value itself                | A reference (the address of an object)  |
| Object lives   | No object                       | On the heap (shared memory for objects) |
| Assignment     | Copies the value                | Copies the reference, not the object    |
| Default value  | `0`, `0.0`, `false`, `'\u0000'` | `null`                                  |
| Can be `null`  | No                              | Yes                                     |
| `==` compares  | Values                          | References (same object or not)         |
| Name style     | Lowercase: `int`                | Capitalized: `Integer`, `String`        |

---

## Classes Are Shared, Primitives Are Copied

```java
public class References {
    static class Person {
        String name;
        Person(String name) { this.name = name; }
    }

    public static void main(String[] args) {
        int a = 1;
        int b = a;          // copy of the value
        b = 99;
        System.out.println(a);

        Person p1 = new Person("Alice");
        Person p2 = p1;     // copy of the reference: same object
        p2.name = "Bob";
        System.out.println(p1.name);

        Person p3 = new Person("Bob");
        System.out.println(p1 == p3);   // different objects
        System.out.println(p1 == p2);   // same object
    }
}
```

Expected output should be:

```
1
Bob
false
true
```

---

## No User-Defined Value Types

Java has no `struct`. Every type you declare (class, record, enum) is a reference type. A record is the closest thing to a small value: it compares by value, but it is still an object on the heap. See [Records and Equality](25_records_and_equality.md).

Project Valhalla, a long-running OpenJDK project, is adding **value classes** (objects without identity). They are not part of Java 21 or 25.

---

## Wrapper Classes

Collections and generics only hold objects, so each primitive has a matching **wrapper class**.

| Primitive | Wrapper     |
| --------- | ----------- |
| `int`     | `Integer`   |
| `long`    | `Long`      |
| `double`  | `Double`    |
| `float`   | `Float`     |
| `boolean` | `Boolean`   |
| `char`    | `Character` |
| `byte`    | `Byte`      |
| `short`   | `Short`     |

```java
List<int> bad;                // compile error
List<Integer> numbers = new ArrayList<>();
numbers.add(5);               // the int is boxed into an Integer
int first = numbers.get(0);   // and unboxed back
```

---

## Autoboxing and Unboxing

**Boxing** wraps a primitive in its wrapper object. **Unboxing** takes it back out. The compiler does both for you (autoboxing).

```java
Integer boxed = 10;          // boxing: Integer.valueOf(10)
int plain = boxed;           // unboxing: boxed.intValue()

Integer missing = null;
int crash = missing;         // NullPointerException: nothing to unbox
```

Boxing creates objects. In a hot loop, a `Long` total instead of `long` can be many times slower.

```java
Long slow = 0L;
for (int i = 0; i < 1_000_000; i++) slow += i;    // a new Long every time

long fast = 0;
for (int i = 0; i < 1_000_000; i++) fast += i;    // no objects
```

---

## The Integer Cache Trap

`==` on two wrapper objects compares references. Java keeps one shared object for each value from -128 to 127, so small numbers seem to work and bigger ones do not.

```java
Integer a = 127, b = 127;
Integer c = 128, d = 128;

System.out.println(a == b);        // true: the same cached object
System.out.println(c == d);        // false: two different objects
System.out.println(c.equals(d));   // true: compares values
```

Compare wrappers with `equals`, or unbox them first.

---

## Object: The Base Type

Every class extends `java.lang.Object`, directly or indirectly. So any object fits in an `Object` variable.

```java
Object o1 = "hello";
Object o2 = 42;              // boxed to Integer
Object o3 = new int[] {1};   // arrays are objects too

if (o2 instanceof Integer n) {
    System.out.println(n + 1);   // 43
}
```

`Object` gives every class `equals`, `hashCode`, `toString`, and `getClass`. See [Pattern Matching](14_pattern_matching.md) for `instanceof`.

---

## var Is Not Dynamic

`var` (Java 10+) lets the compiler work out the type of a local variable. The type is still fixed at compile time.

```java
var list = new ArrayList<String>();   // list is ArrayList<String>
list.add("a");
// list = "text";                     // compile error: wrong type
```

Java has no `dynamic` type. The nearest thing is `Object` plus a cast or a pattern.

---

## Gotchas

- Assigning an object copies the reference, so both variables change the same object
- `==` on objects (including `Integer` and `String`) compares references; use `equals`
- Unboxing a `null` wrapper throws `NullPointerException`
- `Integer` values from -128 to 127 are cached, which hides `==` bugs in small tests
- A `final` reference cannot point somewhere else, but the object it points to can still change
- Boxing in loops creates many short-lived objects; use primitives or `IntStream` in hot code

---

## Examples

- [22-01](examples/22-01_classes_are_shared_primitives_are_copied.java): Classes are shared primitives are copied

Run one with `java examples/22-01_classes_are_shared_primitives_are_copied.java`. See [Examples](examples/README.md).
