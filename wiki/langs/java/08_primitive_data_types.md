# 08 - Primitive Data Types

## What are Primitive Data Types

Java has eight primitive types. They hold a plain value directly: a number, a character, or `true`/`false`. They are written in lowercase.

Everything else (`String`, arrays, your own classes) is a reference type, which holds a pointer to an object. See [Reference Data Types](22_reference_data_types.md).

Every primitive has the same size on every computer. An `int` is always 32 bits.

---

## Integer Types

| Type    | Size    | Range                             | Literal      |
| ------- | ------- | --------------------------------- | ------------ |
| `byte`  | 8 bits  | -128 to 127                       | `(byte) 10`  |
| `short` | 16 bits | -32,768 to 32,767                 | `(short) 10` |
| `int`   | 32 bits | about -2.1 billion to 2.1 billion | `10`         |
| `long`  | 64 bits | about -9.2 × 10^18 to 9.2 × 10^18 | `10L`        |

Use `int` by default. Use `long` for large counts, file sizes, and time in milliseconds.

```java
int million = 1_000_000;        // underscores for readability
long big = 9_000_000_000L;      // L suffix: too big for int
int hex = 0xFF;                 // hexadecimal: 255
int binary = 0b1010;            // binary: 10
int octal = 017;                // a leading 0 means octal: 15

System.out.println(million + " " + big + " " + hex + " " + binary + " " + octal);
```

Expected output should be:

```
1000000 9000000000 255 10 15
```

Each type has a wrapper class with its limits: `Integer.MAX_VALUE`, `Long.MIN_VALUE`, `Byte.MAX_VALUE`.

---

## Floating-Point Types

| Type     | Size    | Precision          | Literal |
| -------- | ------- | ------------------ | ------- |
| `float`  | 32 bits | about 7 digits     | `3.14f` |
| `double` | 64 bits | about 15-16 digits | `3.14`  |

```java
System.out.println(0.1 + 0.2);      // 0.30000000000000004
System.out.println(1.0f / 3);       // 0.33333334
System.out.println(1.0 / 3);        // 0.3333333333333333
System.out.println(1.0 / 0);        // Infinity
System.out.println(0.0 / 0);        // NaN (not a number)
```

Use `double` by default. A decimal literal without `f` is a `double`, so `float f = 3.14;` does not compile.

Floating-point numbers are stored in binary, so most decimals are close, not exact. Use `BigDecimal` for money. See [Math, Random and Utility Classes](12_math_random_and_utilities.md).

---

## Boolean

```java
boolean ready = true;
boolean done = false;
boolean bigger = 5 > 3;      // true
```

Only `true` and `false`. Unlike C, `0` and `1` are not booleans: `if (1)` does not compile.

---

## Character

`char` is one 16-bit UTF-16 code unit, written in single quotes.

```java
char letter = 'A';
char unicode = 'B';            // 'B'
char newline = '\n';                // escape for a new line

System.out.println(letter + 1);             // 66: char + int is an int
System.out.println((char) (letter + 1));    // B
```

| Escape   | Meaning             |
| -------- | ------------------- |
| `\n`     | New line            |
| `\t`     | Tab                 |
| `\'`     | Single quote        |
| `\"`     | Double quote        |
| `\\`     | Backslash           |
| `\uXXXX` | Unicode by hex code |

Emoji and some rare characters need two `char` values. See [Strings](20_strings.md).

---

## Overflow

Integer math wraps around silently when it goes past the limit.

```java
int max = Integer.MAX_VALUE;
System.out.println(max + 1);        // -2147483648

Math.addExact(max, 1);              // throws ArithmeticException: integer overflow
```

Use `long`, the `Math.*Exact` methods, or `BigInteger` when a value can get large.

---

## No Unsigned Types

Java has no `uint` or unsigned byte. Every integer type is signed. Helper methods treat the bits as unsigned:

```java
byte raw = (byte) 200;
System.out.println(raw);                           // -56
System.out.println(Byte.toUnsignedInt(raw));       // 200
System.out.println(Integer.toUnsignedString(-1));  // 4294967295
```

---

## Wrapper Classes

Each primitive has a class version, used where an object is needed, such as in a `List`.

| Primitive | Wrapper     |
| --------- | ----------- |
| `byte`    | `Byte`      |
| `short`   | `Short`     |
| `int`     | `Integer`   |
| `long`    | `Long`      |
| `float`   | `Float`     |
| `double`  | `Double`    |
| `char`    | `Character` |
| `boolean` | `Boolean`   |

Java converts between the two automatically (**autoboxing** and **unboxing**). See [Type Conversion](10_type_conversion.md).

---

## Default Values

Fields and array elements start with a default. Local variables do not; they must be assigned before use.

| Type                           | Default                         |
| ------------------------------ | ------------------------------- |
| `byte`, `short`, `int`, `long` | `0`                             |
| `float`, `double`              | `0.0`                           |
| `boolean`                      | `false`                         |
| `char`                         | `'\u0000'` (the zero character) |
| Any reference type             | `null`                          |

---

## Gotchas

- `0.1 + 0.2 == 0.3` is `false`; compare doubles with a small tolerance, or use `BigDecimal`
- `int` overflow does not throw; it wraps to a negative number
- A leading zero makes an octal number: `010` is `8`
- `long big = 3_000_000_000;` does not compile; the literal needs the `L` suffix
- `char + char` is an `int`, so `'a' + 'b'` is `195`, not `"ab"`
- `NaN == NaN` is `false`; use `Double.isNaN(x)`
- `Integer` values compared with `==` compare objects; `127 == 127` happens to be `true` but `128 == 128` is `false`. Use `equals`
