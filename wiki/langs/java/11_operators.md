# 11 - Operators

## Arithmetic Operators

```java
int a = 17, b = 5;

System.out.println(a + b);     // 22
System.out.println(a - b);     // 12
System.out.println(a * b);     // 85
System.out.println(a / b);     // 3: int / int drops the decimals
System.out.println(a % b);     // 2: remainder
System.out.println(17.0 / 5);  // 3.4: one double makes it a double division
```

| Case                    | Result                                       |
| ----------------------- | -------------------------------------------- |
| `-17 % 5`               | `-2`: the sign follows the left side         |
| `Math.floorMod(-17, 5)` | `3`: always 0 or more for a positive divisor |
| `5.5 % 2`               | `1.5`: `%` works on decimals too             |
| `1 / 0`                 | Throws `ArithmeticException: / by zero`      |
| `1.0 / 0`               | `Infinity`, no exception                     |

---

## Assignment Operators

```java
int x = 10;
x += 5;    // 15  same as x = x + 5
x -= 3;    // 12
x *= 2;    // 24
x /= 4;    // 6
x %= 4;    // 2
x <<= 3;   // 16  shift left by 3
```

Compound operators include a hidden cast: `byte b = 1; b += 300;` compiles and overflows. See [Type Conversion](10_type_conversion.md).

`=` is also an expression, so `p = q = 7;` sets both.

---

## Increment and Decrement

```java
int i = 5;

System.out.println(i++);   // 5: prints, then adds 1
System.out.println(i);     // 6
System.out.println(++i);   // 7: adds 1, then prints
System.out.println(i--);   // 7: prints, then subtracts 1
System.out.println(--i);   // 5: subtracts 1, then prints
```

| Form  | Name    | Value used    |
| ----- | ------- | ------------- |
| `i++` | Postfix | The old value |
| `++i` | Prefix  | The new value |

Use them on their own line (`i++;`) and the difference does not matter.

---

## Comparison Operators

```java
System.out.println(5 == 5);   // true: equal
System.out.println(5 != 3);   // true: not equal
System.out.println(5 > 3);    // true
System.out.println(5 < 3);    // false
System.out.println(5 >= 5);   // true
System.out.println(5 <= 4);   // false
```

Each comparison gives a `boolean`, so you can use it in an `if` or store it in a variable.

`==` on objects checks whether both sides are the same object, not whether they hold the same value:

```java
String s1 = "hi";
String s2 = new String("hi");

System.out.println(s1 == s2);        // false: two different objects
System.out.println(s1.equals(s2));   // true: same text
```

Use `equals` for strings and other objects. See [Strings](20_strings.md) and [equals, hashCode and Comparable](29_equals_hashcode_and_comparable.md).

---

## Logical Operators

```java
int age = 20;
boolean hasTicket = false;

System.out.println(age >= 18 && hasTicket);   // false: AND
System.out.println(age >= 18 || hasTicket);   // true: OR
System.out.println(!hasTicket);               // true: NOT
System.out.println(true ^ true);              // false: XOR, true when the two differ
```

`&&` and `||` are **short-circuit**: they skip the right side when the left side already decides the answer. This makes null checks safe:

```java
if (name != null && name.length() > 3) {     // length() never runs on null
    System.out.println("long name");
}
```

`&` and `|` on booleans always run both sides. Use them only when the right side must run.

---

## Bitwise and Shift Operators

Work on the individual bits of an integer.

```java
int m = 0b1100, n = 0b1010;   // 12 and 10

System.out.println(m & n);    // 8   (1000) AND
System.out.println(m | n);    // 14  (1110) OR
System.out.println(m ^ n);    // 6   (0110) XOR
System.out.println(~m);       // -13 flip every bit
System.out.println(1 << 4);   // 16  shift left: multiply by 2 four times
System.out.println(-16 >> 2); // -4  shift right, keeps the sign
System.out.println(-16 >>> 28); // 15 shift right, fills with zeros
```

| Operator | Name                 | Use                                       |
| -------- | -------------------- | ----------------------------------------- |
| `&`      | AND                  | Test or clear bits: `(n & 1) == 1` is odd |
| `\|`     | OR                   | Set bits                                  |
| `^`      | XOR                  | Flip bits                                 |
| `~`      | NOT                  | Invert all bits                           |
| `<<`     | Left shift           | Multiply by a power of 2                  |
| `>>`     | Signed right shift   | Divide by a power of 2 (rounds down)      |
| `>>>`    | Unsigned right shift | Shift in zeros; for bit masks and hashes  |

`Integer.toBinaryString(m & n)` shows the bits: `"1000"`. For sets of options, prefer `EnumSet`. See [Enums](09_enums.md).

---

## Ternary Operator

A short `if`/`else` that produces a value: `condition ? valueIfTrue : valueIfFalse`.

```java
String status = age >= 18 ? "Adult" : "Minor";
```

Keep it to one simple condition. Nested ternaries are hard to read; use `if` or a `switch` expression instead (see [Control Flow](13_control_flow.md)).

---

## instanceof

Checks the type of an object at run time.

```java
Object o = "text";

System.out.println(o instanceof String);      // true
System.out.println(null instanceof String);   // false: never throws

if (o instanceof String s) {                  // test and declare in one step (Java 16+)
    System.out.println(s.length());           // 4
}
```

See [Pattern Matching](14_pattern_matching.md).

---

## String Concatenation

`+` with a `String` on either side joins text. It runs left to right.

```java
System.out.println("a" + 1 + 2);      // a12
System.out.println(1 + 2 + "a");      // 3a: the numbers add first
System.out.println('a' + 'b' + "c");  // 195c: two chars add as numbers first
System.out.println("" + 'a' + 'b');   // ab
```

---

## Java Has No

| Operator from other languages | Java way                                                                        |
| ----------------------------- | ------------------------------------------------------------------------------- |
| `??` (null-coalescing)        | `Objects.requireNonNullElse(x, fallback)`                                       |
| `?.` (null-conditional)       | An `if` check, or `Optional` (see [Null and Optional](24_null_and_optional.md)) |
| `**` (power)                  | `Math.pow(2, 10)`                                                               |
| `sizeof`                      | Fixed sizes: `Integer.BYTES` is `4`                                             |
| Operator overloading          | Methods such as `a.add(b)` on `BigDecimal`                                      |

---

## Operator Precedence (high to low)

| Level | Operators                               |
| ----- | --------------------------------------- |
| 1     | `x++` `x--`                             |
| 2     | `++x` `--x` `+x` `-x` `!` `~` `(cast)`  |
| 3     | `*` `/` `%`                             |
| 4     | `+` `-`                                 |
| 5     | `<<` `>>` `>>>`                         |
| 6     | `<` `>` `<=` `>=` `instanceof`          |
| 7     | `==` `!=`                               |
| 8     | `&`                                     |
| 9     | `^`                                     |
| 10    | `\|`                                    |
| 11    | `&&`                                    |
| 12    | `\|\|`                                  |
| 13    | `? :`                                   |
| 14    | `=` `+=` `-=` and the other assignments |

```java
System.out.println(2 + 3 * 4);       // 14
System.out.println((2 + 3) * 4);     // 20
System.out.println(10 - 4 - 3);      // 3: same level runs left to right
```

When in doubt, add parentheses. Bitwise operators sit below `==`, so `(x & 1) == 1` needs them.

---

## Gotchas

- `7 / 2` is `3`; make one side a `double` for `3.5`
- `-7 % 2` is `-1`, so `n % 2 == 1` misses negative odd numbers; test `n % 2 != 0`
- Integer overflow is silent: `Integer.MAX_VALUE + 1` is `-2147483648`
- `==` on `String` or `Integer` compares objects; `Integer` values from -128 to 127 are cached, so `==` seems to work until it does not
- `0.1 + 0.2 == 0.3` is `false`; compare with a tolerance: `Math.abs(a - b) < 1e-9`
- `count = count++;` leaves `count` unchanged
- `x & 1 == 1` does not compile, because `==` runs first; write `(x & 1) == 1`
