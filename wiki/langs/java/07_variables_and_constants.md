# 07 - Variables and Constants

## Variable Declaration

A variable is a named box that holds a value. Give it a type, a name, and usually a starting value.

```java
int age = 30;
String name = "Alice";
double price;          // declared now
price = 9.99;          // assigned later

System.out.println(name + " " + age + " " + price);
```

Expected output should be:

```
Alice 30 9.99
```

The type never changes. `age = "thirty";` is a compile error.

---

## var: Inferred Type (Java 10+)

`var` lets the compiler work out the type from the value. The variable is still strongly typed.

```java
var count = 10;                       // int
var title = "Java";                   // String
var items = new ArrayList<String>();  // ArrayList<String>
```

| Allowed                                      | Not allowed                             |
| -------------------------------------------- | --------------------------------------- |
| Local variables with a starting value        | `var x;` (no value to infer from)       |
| Loop variables: `for (var s : list)`         | `var n = null;` (no type to infer)      |
| Lambda parameters: `(var a, var b) -> a + b` | Fields, method parameters, return types |
|                                              | `var a = 1, b = 2;` (several at once)   |

Use `var` when the type is obvious from the right side. Write the type when it helps the reader.

---

## Multiple Declarations

```java
int x = 1, y = 2, z = 3;   // same type, one line
int a, b;
a = b = 5;                 // both become 5
```

One declaration per line is easier to read and to change.

---

## Definite Assignment

A local variable (one declared inside a method) has no default value. The compiler refuses to read it before it is assigned.

```java
int c;
System.out.println(c);   // error: variable c might not have been initialized
```

Fields of a class do get defaults (`0`, `false`, `null`). See [Primitive Data Types](08_primitive_data_types.md).

---

## Constants with final

`final` means "assign once". After that, the variable cannot change.

```java
final int limit = 3;
// limit = 4;           // error: cannot assign a value to final variable limit
```

A `final` local can be assigned later, as long as every path assigns it exactly once:

```java
final String greeting;
if (age >= 18) {
    greeting = "Hello";
} else {
    greeting = "Hi";
}
```

---

## static final: Class Constants

A constant shared by the whole program is a `static final` field, named in `UPPER_SNAKE_CASE`.

```java
public class Prices {
    public static final double TAX_RATE = 0.2;
    public static final int MAX_ITEMS = 100;
}
```

Other code uses it through the class name: `double tax = 50 * Prices.TAX_RATE;`.

| Kind                  | Example                                           | Value fixed when                                   |
| --------------------- | ------------------------------------------------- | -------------------------------------------------- |
| Compile-time constant | `static final int MAX = 100;`                     | Compile time. Copied into every class that uses it |
| Run-time constant     | `static final int MAX = readConfig();`            | When the class loads                               |
| Final field           | `private final String id;` set in the constructor | When the object is created                         |
| Final local           | `final int limit = 3;`                            | When that line runs                                |

Java has no `const` keyword in use. `const` is reserved but does nothing.

---

## final Does Not Freeze the Object

`final` locks the variable, not the object it points to.

```java
final List<String> names = new ArrayList<>();
names.add("Bob");               // fine: the list changes, the variable does not
// names = new ArrayList<>();   // error: the variable cannot point elsewhere

System.out.println(names);
```

Expected output should be:

```
[Bob]
```

For a list that cannot change, use `List.of("Bob")`. See [Collections](36_collections.md).

---

## Effectively Final

A local variable that is never reassigned is **effectively final**, even without the keyword. Lambdas (short inline functions) and inner classes can only use local variables that are final or effectively final.

```java
int base = 2;                                   // never reassigned
Runnable r = () -> System.out.println(base * 10);
r.run();                                        // 20
```

Add `base++;` anywhere and the lambda no longer compiles. See [Lambdas and Functional Interfaces](39_lambdas_and_functional_interfaces.md).

---

## Scope

A variable exists from its declaration to the end of the block `{ }` it is in.

```java
int total = 0;
{
    int inner = 5;
    total += inner;
}
// inner does not exist here
System.out.println(total);   // 5
```

A nested block cannot declare a local with the same name as one outside it. `int v = 1; { int v = 2; }` is an error.

---

## Unnamed Variables (Java 22+)

Use `_` for a variable you must declare but never use.

```java
int count = 0;
for (var _ : List.of(1, 2, 3)) {
    count++;
}

try {
    Integer.parseInt("abc");
} catch (NumberFormatException _) {
    System.out.println("Not a number");
}
```

---

## Naming Conventions

| Kind                           | Style               | Example                |
| ------------------------------ | ------------------- | ---------------------- |
| Local variable, field          | camelCase           | `orderTotal`           |
| Constant                       | UPPER_SNAKE_CASE    | `MAX_RETRIES`          |
| Class, interface, enum, record | PascalCase          | `OrderService`         |
| Method                         | camelCase, a verb   | `calculateTotal()`     |
| Package                        | all lowercase       | `com.example.shop`     |
| Boolean                        | reads as a question | `isActive`, `hasItems` |

Names may contain letters, digits, `_`, and `$`, but cannot start with a digit. Avoid `$`; generated code uses it. See [Conventions and Clean Code](57_conventions_and_clean_code.md).

---

## Gotchas

- `var` needs a starting value, and `var n = null;` does not compile
- `var list = new ArrayList<>();` infers `ArrayList<Object>`, so anything can go in it; put the type in the diamond or on the left
- A `final` reference to a list, array, or object still lets you change what is inside
- Changing a public compile-time constant does not update other JARs compiled against the old value until they are recompiled
- `0.2` is a `double`; see [Primitive Data Types](08_primitive_data_types.md) for why `3 * 0.2` prints `0.6000000000000001`
- Reading a local variable before assigning it is a compile error, not `0`
