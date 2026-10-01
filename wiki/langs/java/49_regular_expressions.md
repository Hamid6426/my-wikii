# 49 - Regular Expressions

## What is a Regex

A **regular expression** (regex) is a pattern for finding, checking, or replacing text. Java has it built in.

```java
import java.util.regex.Matcher;
import java.util.regex.Pattern;
```

| Class     | Job                                              |
| --------- | ------------------------------------------------ |
| `Pattern` | The compiled pattern. Build once, use many times |
| `Matcher` | One search of a pattern over one piece of text   |

---

## Backslashes

Java has no raw strings, so every `\` in a pattern is written `\\` in code. The regex `\d+` becomes `"\\d+"`.

| Regex | Java string |
| ----- | ----------- |
| `\d+` | `"\\d+"`    |
| `\s`  | `"\\s"`     |
| `\.`  | `"\\."`     |
| `\\`  | `"\\\\"`    |

Text blocks (see [Strings](20_strings.md)) process escapes too, so they need `\\` as well.

---

## Check for a Match

```java
"123".matches("\\d+")                               // true
"abc123".matches("\\d+")                            // false, must match the WHOLE text
Pattern.compile("\\d+").matcher("abc123").find()    // true, finds it anywhere
```

| Method        | Matches when                           |
| ------------- | -------------------------------------- |
| `matches()`   | The whole text fits the pattern        |
| `lookingAt()` | The start of the text fits             |
| `find()`      | Any part fits. Call again for the next |

---

## Find a Match

```java
Matcher m = Pattern.compile("#(\\d+)").matcher("Order #4521 shipped");

if (m.find()) {
    System.out.println(m.group());    // #4521, the whole match
    System.out.println(m.group(1));   // 4521, the first group in ( )
    System.out.println(m.start());    // 6, where it begins
}
```

---

## Find All Matches

```java
Matcher m = Pattern.compile("\\d+").matcher("a1 b22 c333");
while (m.find()) {
    System.out.println(m.group());   // 1, then 22, then 333
}
```

As a stream (Java 9):

```java
List<String> numbers = Pattern.compile("\\d+").matcher("a1 b22 c333")
    .results()
    .map(MatchResult::group)
    .toList();   // [1, 22, 333]
```

---

## Replace and Split

```java
"a   b    c".replaceAll("\\s+", " ")                          // a b c
"a1b22c".split("\\d+")                                        // [a, b, c]
"4111111111111111".replaceAll("\\d(?=\\d{4})", "*")           // ************1111
"2026-10-01".replaceAll("(\\d{4})-(\\d{2})-(\\d{2})", "$3/$2/$1")   // 01/10/2026, $1 is group 1
```

Replace with code (Java 9):

```java
Pattern.compile("\\d+").matcher("x 5 y 12")
    .replaceAll(r -> String.valueOf(Integer.parseInt(r.group()) * 2));   // x 10 y 24
```

---

## Named Groups

```java
Matcher m = Pattern.compile("(?<year>\\d{4})-(?<month>\\d{2})-(?<day>\\d{2})")
    .matcher("2026-10-01");

if (m.matches()) {
    System.out.println(m.group("year"));    // 2026
    System.out.println(m.group("month"));   // 10
}
```

In a replacement string, use `${year}`.

---

## Common Pieces

| Pattern   | Meaning                          |
| --------- | -------------------------------- |
| `.`       | Any character except newline     |
| `\d`      | A digit                          |
| `\w`      | Letter, digit, or underscore     |
| `\s`      | Whitespace                       |
| `\b`      | A word boundary                  |
| `^` `$`   | Start and end of the text        |
| `*`       | Zero or more                     |
| `+`       | One or more                      |
| `?`       | Zero or one                      |
| `{3,5}`   | Between 3 and 5                  |
| `[abc]`   | One of a, b, or c                |
| `[^abc]`  | Any character except a, b, or c  |
| `(a\|b)`  | Group with a choice              |
| `(?:...)` | Group that does not capture      |
| `(?=...)` | Followed by, without using it up |

---

## Flags

```java
Pattern.compile("hello", Pattern.CASE_INSENSITIVE).matcher("HELLO").matches()   // true
Pattern.compile("^b$", Pattern.MULTILINE).matcher("a\nb").find()                // true
Pattern.compile("a.b", Pattern.DOTALL).matcher("a\nb").matches()                // true
Pattern.compile("(?i)hello").matcher("HeLLo").matches()                         // true, flag inside the pattern
```

| Flag               | Inline | Effect                               |
| ------------------ | ------ | ------------------------------------ |
| `CASE_INSENSITIVE` | `(?i)` | Ignore upper and lower case          |
| `MULTILINE`        | `(?m)` | `^` and `$` match at each line       |
| `DOTALL`           | `(?s)` | `.` also matches a newline           |
| `COMMENTS`         | `(?x)` | Ignore spaces and allow `#` comments |

Combine flags with `|`: `Pattern.CASE_INSENSITIVE | Pattern.MULTILINE`.

---

## Reuse and Speed

`String.matches`, `replaceAll`, and `split` compile the pattern on every call. Compile once if you use it many times.

```java
public class Validator {
    private static final Pattern EMAIL = Pattern.compile("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$");

    public static boolean isEmail(String input) {
        return EMAIL.matcher(input).matches();
    }
}
```

A `Pattern` is safe to share between threads. A `Matcher` is not, so make a new one per use.

Handy helpers on `Pattern`:

```java
Pattern.compile(",").splitAsStream("a,b,c").toList()      // [a, b, c]
Pattern.compile("\\d").asPredicate().test("ab1")          // true, like find()
Pattern.compile("\\d").asMatchPredicate().test("ab1")     // false, like matches()
```

---

## Matching Literal Text

```java
"a.b.c".split(".")                     // [] (empty): "." means any character
"a.b.c".split("\\.")                   // [a, b, c]
"1+1=2".split(Pattern.quote("+"))      // [1, 1=2], quote() escapes everything
"price".replaceAll("price", Matcher.quoteReplacement("$5"))   // $5
```

`$` and `\` are special in a replacement string. `Matcher.quoteReplacement` makes them plain.

---

## Gotchas

- `String.matches` must match the whole text. Use `find()` to search inside it
- `split` takes a regex, so `"."`, `"|"`, and `"+"` need escaping
- `group()` before a successful `find()` or `matches()` throws `IllegalStateException: No match found`
- A bad pattern throws `PatternSyntaxException` when compiled, such as "Unclosed group" for `"(abc"`
- Java has no match timeout. Patterns with nested repeats like `(a+)+` can run for a very long time on bad input, so do not run patterns from users on untrusted text
- Do not parse HTML or JSON with regex. Use a real parser (see [JSON](48_json.md))
- Simple checks like `startsWith` or `contains` are clearer and faster than a regex
