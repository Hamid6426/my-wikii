# 05 - Console Input and Output

## Writing Text

```java
System.out.println("Hello");   // text, then a new line
System.out.print("Hello ");    // text, no new line
System.out.print("World");
System.out.println();          // just a new line
```

Expected output should be:

```
Hello
Hello World
```

`System.out` is the standard output stream. In Java 25, `IO.println` and `IO.print` do the same with less typing.

---

## Joining Values

Use `+` to join text and values into one string.

```java
String name = "Alice";
int age = 30;

System.out.println(name + " is " + age + " years old");
System.out.println("Next year: " + (age + 1));
System.out.println("Sum: " + 1 + 2);
System.out.println(1 + 2 + " total");
```

Expected output should be:

```
Alice is 30 years old
Next year: 31
Sum: 12
3 total
```

`+` works left to right. Once one side is a `String`, the rest is joined as text, so wrap math in parentheses. See [Strings](20_strings.md).

---

## Formatted Output

`printf` takes a format string with placeholders that start with `%`, then the values to fill in.

```java
System.out.printf("%s is %d years old%n", name, age);

String line = String.format("%s is %d", name, age);   // same, but returns a String
String same = "%s is %d".formatted(name, age);        // same, called on the format
```

Expected output should be:

```
Alice is 30 years old
```

`%n` is a new line. `printf` does not add one on its own.

---

## Format Codes

```java
double price = 1234.5;

System.out.printf("%.2f%n", price);      // 1234.50
System.out.printf("%,.2f%n", price);     // 1,234.50
System.out.printf("%05d%n", 7);          // 00007
System.out.printf("%x%n", 255);          // ff
System.out.printf("%.1f%%%n", 25.6);     // 25.6%
```

| Code   | Meaning                         | Example input | Result         |
| ------ | ------------------------------- | ------------- | -------------- |
| `%s`   | Any value as text               | `"Bob"`       | `Bob`          |
| `%d`   | Whole number                    | `42`          | `42`           |
| `%f`   | Decimal number (6 places)       | `3.5`         | `3.500000`     |
| `%.2f` | Decimal, 2 places               | `3.14159`     | `3.14`         |
| `%,d`  | Whole number with separators    | `1234567`     | `1,234,567`    |
| `%05d` | Pad with zeros to 5 wide        | `7`           | `00007`        |
| `%x`   | Hexadecimal (`%X` for capitals) | `255`         | `ff`           |
| `%e`   | Scientific                      | `12345.0`     | `1.234500e+04` |
| `%b`   | Boolean                         | `true`        | `true`         |
| `%c`   | Character                       | `'J'`         | `J`            |
| `%%`   | A literal `%`                   |               | `%`            |
| `%n`   | New line for this system        |               |                |

The decimal point and the thousands separator depend on the computer's language settings (the locale). Pass one for fixed output:

```java
String.format(Locale.GERMANY, "%,.2f", 1234.5);   // 1.234,50
String.format(Locale.ROOT, "%,.2f", 1234.5);      // 1,234.50
```

`Locale` is in `java.util`.

---

## Alignment

A number after `%` sets the width. Plain is right-aligned, `-` is left-aligned.

```java
System.out.printf("%-10s%5s%n", "Name", "Age");
System.out.printf("%-10s%5d%n", "Alice", 30);
System.out.printf("%-10s%5d%n", "Bob", 4);
```

Expected output should be:

```
Name        Age
Alice        30
Bob           4
```

---

## Reading a Line

`Scanner` (in `java.util`) reads text from the keyboard.

```java
import java.util.Scanner;

public class Greet {
    public static void main(String[] args) {
        Scanner in = new Scanner(System.in);

        System.out.print("Your name: ");
        String name = in.nextLine();

        System.out.println("Hello, " + name);
    }
}
```

Expected output should be (typing `Alice`):

```
Your name: Alice
Hello, Alice
```

Create one `Scanner` for `System.in` and reuse it. `nextLine()` throws `NoSuchElementException` when input ends (Ctrl+D on Linux and macOS, Ctrl+Z then Enter on Windows).

---

## Reading Numbers

Input is always text. Convert it, and never trust that the user typed a number.

```java
Scanner in = new Scanner(System.in);
System.out.print("Age: ");
String text = in.nextLine();

try {
    int age = Integer.parseInt(text.trim());
    System.out.println("In 10 years: " + (age + 10));
} catch (NumberFormatException e) {
    System.out.println("That is not a number");
}
```

Java has no `TryParse`. Catch `NumberFormatException`, as above. See [Type Conversion](10_type_conversion.md) and [Error Handling](42_error_handling.md).

`Scanner` also has `nextInt()`, `nextDouble()`, and `hasNextInt()`. Mixing them with `nextLine()` causes a classic bug:

```java
System.out.print("Age: ");
int age = in.nextInt();          // reads "30", leaves the line break behind
System.out.print("Name: ");
String name = in.nextLine();     // reads the leftover empty line

System.out.println("[" + name + "] " + age);
```

Expected output should be (typing `30`):

```
Age: 30
Name: [] 30
```

The program never waits for the name. Read whole lines with `nextLine()` and convert them yourself.

---

## Reading Until Valid

```java
Scanner in = new Scanner(System.in);

System.out.print("Age: ");
while (in.hasNextLine()) {
    String line = in.nextLine().trim();

    if (line.matches("\\d+")) {
        int age = Integer.parseInt(line);
        System.out.println("Age set to " + age);
        break;
    }

    System.out.print("Enter a whole number, 0 or more: ");
}
```

`hasNextLine()` returns `false` when input ends, so the loop stops instead of throwing. `matches("\\d+")` checks for digits only (see [Regular Expressions](49_regular_expressions.md)).

---

## Reading Many Lines Fast

`BufferedReader` is faster than `Scanner` for large input. `readLine()` returns `null` when input ends.

```java
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

public class Sum {
    public static void main(String[] args) throws IOException {
        var reader = new BufferedReader(new InputStreamReader(System.in));

        String line;
        int total = 0;
        while ((line = reader.readLine()) != null) {   // null when input ends
            total += Integer.parseInt(line.trim());
        }

        System.out.println("Total: " + total);
    }
}
```

```bash
printf '1\n2\n3\n' | java Sum.java
```

Expected output should be:

```
Total: 6
```

---

## The IO Class (Java 25+)

```java
void main() {
    String name = IO.readln("Your name: ");
    IO.println("Hello, " + name);
}
```

| Method              | Does                                                    |
| ------------------- | ------------------------------------------------------- |
| `IO.println(x)`     | Print `x` and a new line                                |
| `IO.print(x)`       | Print `x`, no new line                                  |
| `IO.readln(prompt)` | Print the prompt, read one line. `null` at end of input |
| `IO.readln()`       | Read one line, no prompt                                |

---

## Passwords and the Console

`System.console()` talks to the real terminal. It can hide what the user types.

```java
java.io.Console console = System.console();

if (console == null) {
    System.err.println("No terminal available");
} else {
    char[] password = console.readPassword("Password: ");
    System.out.println("Read " + password.length + " characters");
}
```

It returns `null` when input or output is redirected, and in some IDE run windows. Always check.

---

## Errors and Redirection

Programs have two output streams. Errors go to the second one, so users can separate them.

```java
System.out.println("Normal result");     // standard output
System.err.println("Something broke");   // standard error
```

```bash
java Main.java > result.txt       # only normal output goes to the file
java Main.java 2> errors.txt      # only errors go to the file
```

---

## Unicode

Java strings hold Unicode. Since Java 18 the default for files is UTF-8. The console uses the terminal's own encoding, which on older Windows setups may not be UTF-8, so symbols show as `?`. Force UTF-8 output with:

```bash
java -Dstdout.encoding=UTF-8 Main.java
```

On Windows, also run `chcp 65001` in the terminal first. The terminal font must contain the characters.

---

## Command-Line Arguments

Words typed after the program name arrive in `args`.

```java
public class Args {
    public static void main(String[] args) {
        if (args.length == 0) {
            System.err.println("Usage: java Args.java <name>");
            System.exit(1);              // non-zero means failure
        }

        System.out.println("Hello, " + args[0]);
        System.out.println("Argument count: " + args.length);
    }
}
```

```bash
java Args.java Alice Bob
```

Expected output should be:

```
Hello, Alice
Argument count: 2
```

With Maven or Gradle: `./gradlew run --args="Alice Bob"`.

---

## Exit Codes

A program reports success or failure to the shell with a number. `0` means success.

```bash
java Args.java
echo $?            # prints 1 (PowerShell: $LASTEXITCODE)
```

| Way                     | Exit code                                   |
| ----------------------- | ------------------------------------------- |
| `main` returns normally | `0`                                         |
| `System.exit(n)`        | `n`, and the program stops at once          |
| Uncaught exception      | `1`, with the stack trace on standard error |

`main` returns `void`, so `System.exit` is the only way to choose the code.

---

## Gotchas

- `"Total: " + 1 + 2` prints `Total: 12`; write `"Total: " + (1 + 2)`
- `printf` needs `%n` at the end, or the next output joins the same line
- A `%d` with a `double` throws `IllegalFormatConversionException` at run time, not a compile error
- `nextInt()` followed by `nextLine()` returns an empty string; read lines and parse them
- `Scanner.nextLine()` throws at end of input; check `hasNextLine()` first. `IO.readln` and `BufferedReader.readLine` return `null` instead
- `Integer.parseInt(" 42")` throws because of the space; call `trim()` first
- Do not close a `Scanner` on `System.in` and then create another one; closing it closes `System.in` for good
- `System.console()` is `null` in pipes and many IDEs

---

## Examples

- [05-01](examples/05-01_reading_many_lines_fast.java): Reading many lines fast

Run one with `java examples/05-01_reading_many_lines_fast.java`. See [Examples](examples/README.md).
