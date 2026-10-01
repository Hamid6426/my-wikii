# 20 - Strings

## String Basics

`String` is a class in `java.lang`, so it needs no import. Strings are **immutable**: no method changes a string, every one returns a new string.

```java
String name = "Alice";
String empty = "";
String nothing = null;        // no string at all
```

A `char` holds one character in single quotes: `'A'`. A `String` uses double quotes: `"A"`.

---

## Joining and Formatting

```java
String name = "Alice";
int age = 30;

String a = "Name: " + name + ", Age: " + age;              // + joins
String b = String.format("Name: %s, Age: %d", name, age);  // format codes
String c = "Name: %s, Age: %d".formatted(name, age);       // Java 15+, same thing

String pi = "%.2f".formatted(Math.PI);    // "3.14"
String padded = "%05d".formatted(42);     // "00042"
String hex = "%x".formatted(255);         // "ff"
```

| Code   | Meaning                     |
| ------ | --------------------------- |
| `%s`   | Any value, as text          |
| `%d`   | Whole number                |
| `%.2f` | Decimal with 2 places       |
| `%5d`  | Pad to width 5 on the left  |
| `%-5s` | Pad to width 5 on the right |
| `%n`   | Line break for this system  |

Java has no `$"..."` interpolation. String templates were previewed in Java 21 and 22 and then removed, so use `+` or `formatted`.

---

## Text Blocks (Java 15+)

A **text block** is a multi-line string between `"""` marks. Quotes need no escaping, and the common indentation is removed.

```java
String json = """
    {
        "name": "Alice",
        "age": 30
    }
    """;
```

The text starts on the line after the opening `"""`. Put the closing `"""` on its own line to end with a line break.

Escapes in normal strings: `\n` new line, `\t` tab, `\"` quote, `\\` backslash. Java has no `@"..."` raw strings, so Windows paths need `"C:\\Users\\Alice"`.

---

## Common String Methods

```java
import java.util.Arrays;

public class StringTools {
    public static void main(String[] args) {
        String s = "  Hello, World!  ";

        System.out.println(s.length());
        System.out.println("[" + s.strip() + "]");
        System.out.println("[" + s.stripLeading() + "]");
        System.out.println(s.toUpperCase());
        System.out.println(s.contains("World"));
        System.out.println(s.strip().startsWith("Hello"));
        System.out.println(s.replace("World", "Java"));
        System.out.println(s.strip().substring(7, 12));
        System.out.println(s.indexOf("World"));
        System.out.println(s.charAt(2));
        System.out.println(Arrays.toString(s.strip().split(", ")));
        System.out.println("ab".repeat(3));
        System.out.println(String.join("-", "a", "b", "c"));
    }
}
```

Expected output should be:

```
17
[Hello, World!]
[Hello, World!  ]
  HELLO, WORLD!  
true
true
  Hello, Java!  
World
9
H
[Hello, World!]
ababab
a-b-c
```

`substring(start, end)` takes a start index and an end index (not included), not a length.

`strip()` (Java 11+) removes all Unicode whitespace. The older `trim()` removes only characters up to code 32.

---

## Checking for Empty

```java
String s = "   ";

s.isEmpty();   // false: length is not 0
s.isBlank();   // true: only whitespace (Java 11+)

boolean missing = s == null || s.isBlank();   // the usual full check
```

There is no `string.IsNullOrEmpty`. Check for `null` first, or the call throws `NullPointerException`.

---

## Comparing Strings

`==` compares **references** (whether both variables point at the same object), not text. Always use `equals` for text.

```java
String a = "hello";
String b = new String("hello");

System.out.println(a == b);               // false: two objects
System.out.println(a.equals(b));          // true: same text
System.out.println(a.equalsIgnoreCase("HELLO"));   // true

System.out.println("apple".compareTo("banana"));   // negative: apple comes first
System.out.println("hello".equals(null));          // false, no exception
```

Put the known string first, `"quit".equals(input)`, so a `null` input does not throw.

---

## StringBuilder

A changeable string. Use it to build text in a loop, because `+` in a loop makes a new string every time.

```java
StringBuilder sb = new StringBuilder();
for (int i = 1; i <= 3; i++) {
    sb.append("item ").append(i).append('\n');
}
sb.insert(0, ">> ");
sb.setLength(sb.length() - 1);   // drop the last '\n'

String result = sb.toString();
```

`StringBuilder` also has `reverse()`, `replace(start, end, text)`, and `deleteCharAt(index)`.

---

## Characters

The `Character` class tests and changes single `char` values.

```java
String s = "Hello";
char c = s.charAt(0);              // 'H'

Character.isLetter('A');           // true
Character.isDigit('5');            // true
Character.isWhitespace(' ');       // true
Character.toUpperCase('a');        // 'A'

for (char ch : s.toCharArray()) {
    System.out.print(ch + " ");    // H e l l o
}
```

A `char` is a 16-bit code unit. Emoji and some other characters need two `char` values. Use `codePoints()` when that matters.

---

## Splitting and Joining Lines

```java
String csv = "a,b,,c,,";
csv.split(",");          // [a, b, , c]: trailing empty parts are dropped
csv.split(",", -1);      // [a, b, , c, , ]: keep them

"one\ntwo\nthree".lines().count();   // 3 (Java 11+)
```

`split` takes a **regular expression** (a text pattern, see [Regular Expressions](49_regular_expressions.md)). `"a.b".split(".")` returns an empty array because `.` means "any character". Write `split("\\.")`.

---

## Converting To and From Strings

```java
String s1 = String.valueOf(42);       // "42"
String s2 = Integer.toString(42);     // "42"
int n = Integer.parseInt("42");       // 42, throws NumberFormatException if not a number
double d = Double.parseDouble("3.5");
```

See [Type Conversion](10_type_conversion.md).

---

## Gotchas

- `==` on strings compares references; it can seem to work for literals (they are shared) and then fail for strings built at run time
- Strings are immutable: `s.toUpperCase()` alone does nothing, write `s = s.toUpperCase()`
- Calling any method on a `null` string throws `NullPointerException`
- `split` uses a regular expression, so `.`, `|`, `$`, and `*` need `\\` in front
- `substring(start, end)` uses an end index, not a length
- `length()` on a string has brackets, `length` on an array does not

---

## Examples

- [20-01](examples/20-01_common_string_methods.java): Common string methods

Run one with `java examples/20-01_common_string_methods.java`. See [Examples](examples/README.md).
