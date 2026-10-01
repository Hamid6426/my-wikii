# 10 - Type Conversion

## Widening: Automatic

Java converts a smaller type to a bigger one on its own. This is called **widening**.

```java
int i = 100;
long l = i;          // int to long
double d = l;        // long to double

System.out.println(l + " " + d);
```

Expected output should be:

```
100 100.0
```

---

## Narrowing: Casting

Going to a smaller type can lose data, so you must ask for it with a **cast**: the target type in parentheses.

```java
double price = 9.99;
int whole = (int) price;     // 9: the decimal part is cut off, not rounded

int a = (int) -9.99;         // -9: cut toward zero
byte b = (byte) 200;         // -56: only the low 8 bits are kept
int c = (int) 3_000_000_000L; // -1294967296: wraps around
int d = (int) 1e20;          // 2147483647: double to int stops at the limit
int e = (int) Double.NaN;    // 0
long r = Math.round(9.99);   // 10: use this to round instead of cut
```

Without the cast, `int whole = price;` does not compile: "possible lossy conversion from double to int".

---

## Widening Order

```
byte → short → int → long → float → double
              char → int
```

Any type converts automatically to one further right. Going left needs a cast. `char` widens to `int`, but `byte` and `short` do not widen to `char`.

---

## Widening That Loses Precision

`int` to `float` and `long` to `float` or `double` need no cast, yet they can lose digits.

```java
int bigInt = 123_456_789;
float f = bigInt;                 // no cast needed
System.out.println((int) f);      // 123456792: the last digits changed
```

A `float` holds about 7 digits and a `double` about 15. Larger whole numbers get rounded.

---

## Integer Promotion

Math on `byte`, `short`, and `char` always produces an `int`.

```java
byte x = 1, y = 2;
int sum = x + y;          // byte + byte is int
// byte bad = x + y;      // error: possible lossy conversion from int to byte

byte b = 10;
b += 5;                   // allowed: compound assignment adds a hidden cast
```

The same rule makes integer division drop the decimals:

```java
System.out.println(7 / 2);              // 3: both are int
System.out.println(7 / 2.0);            // 3.5: one double makes the result double
System.out.println((double) 7 / 2);     // 3.5: cast first, then divide
System.out.println((double) (7 / 2));   // 3.0: divided as int first, then cast
```

---

## Parsing Strings

Text to number goes through the wrapper classes.

```java
int n = Integer.parseInt("42");
long big = Long.parseLong("9000000000");
double r = Double.parseDouble("3.14");
boolean flag = Boolean.parseBoolean("TRUE");   // true, any case
boolean other = Boolean.parseBoolean("yes");   // false: only "true" counts
```

| Method                 | On bad input                                     |
| ---------------------- | ------------------------------------------------ |
| `Integer.parseInt`     | Throws `NumberFormatException`                   |
| `Double.parseDouble`   | Throws `NumberFormatException`                   |
| `Boolean.parseBoolean` | Returns `false`, never throws                    |
| `Integer.valueOf`      | Like `parseInt`, but returns an `Integer` object |

`Integer.parseInt("4.5")` and `Integer.parseInt(" 42")` both throw. Trim first, and parse decimals with `Double.parseDouble`.

---

## A Safe Parse Helper

Java has no `TryParse`. Write a small helper that returns an empty result instead of throwing.

```java
import java.util.OptionalInt;

public class SafeParse {
    static OptionalInt tryParseInt(String text) {
        try {
            return OptionalInt.of(Integer.parseInt(text.trim()));
        } catch (NumberFormatException e) {
            return OptionalInt.empty();
        }
    }

    public static void main(String[] args) {
        System.out.println(tryParseInt("12"));
        System.out.println(tryParseInt("abc"));
        System.out.println(tryParseInt("abc").orElse(0));
    }
}
```

Expected output should be:

```
OptionalInt[12]
OptionalInt.empty
0
```

See [Null and Optional](24_null_and_optional.md).

---

## To String

```java
String a = String.valueOf(42);          // "42": works for every type, gives "null" for null
String b = Integer.toString(42);        // "42"
String c = 42 + "";                     // "42": works, but reads like a trick

String hex = Integer.toString(255, 16); // "ff": any base
String h = Integer.toHexString(255);    // "ff"
String bits = Integer.toBinaryString(10); // "1010"
int back = Integer.parseInt("ff", 16);  // 255: back from hex
```

For formatted output (decimals, separators) see [Console Input and Output](05_console_input_and_output.md).

---

## Boxing and Unboxing

Java converts between a primitive and its wrapper class automatically.

```java
Integer boxed = 42;                   // autoboxing: int to Integer
int unboxed = boxed;                  // unboxing: Integer to int
Integer same = Integer.valueOf(42);   // the explicit form

Integer nothing = null;
int bad = nothing;                    // throws NullPointerException
```

Collections like `List<Integer>` hold wrappers, so boxing happens all the time. See [Primitive Data Types](08_primitive_data_types.md).

---

## char and int

```java
char c = 'A';
int code = c;                           // 65: char widens to int
char next = (char) (code + 1);          // 'B'

int digit = '7' - '0';                  // 7: digit character to number
int same = Character.getNumericValue('7');   // 7
char seven = Character.forDigit(7, 10); // '7': number to digit character
String text = String.valueOf(c);        // "A"
String bee = Character.toString(66);    // "B": from a code point
```

---

## Reference Casting

A cast on objects does not change the object. It tells the compiler to treat it as a more specific type, and the JVM checks at run time.

```java
Object obj = "hello";
String s = (String) obj;          // fine: it really is a String
Integer n = (Integer) obj;        // throws ClassCastException
```

Check first with `instanceof`. Since Java 16 it can declare a variable in the same step:

```java
if (obj instanceof String text) {
    System.out.println(text.toUpperCase());   // HELLO
}
```

See [Pattern Matching](14_pattern_matching.md).

---

## Gotchas

- A cast from `double` to `int` cuts toward zero; use `Math.round` to round
- Casting a too-big integer wraps around silently: `(byte) 200` is `-56`
- `int` to `float` and `long` to `double` compile with no warning but can change large values
- `byte b = x + y;` does not compile, but `b += y;` does, and can overflow silently
- Unboxing a `null` wrapper throws `NullPointerException`, often in a line that looks like plain math
- `Boolean.parseBoolean("yes")` is `false`, with no error
- `(double) (7 / 2)` is `3.0`, because the division happens first

---

## Examples

- [10-01](examples/10-01_a_safe_parse_helper.java): A safe parse helper

Run one with `java examples/10-01_a_safe_parse_helper.java`. See [Examples](examples/README.md).
