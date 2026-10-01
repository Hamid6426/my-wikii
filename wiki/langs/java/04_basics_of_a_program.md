# 04 - Basics of a Program

## Entry Point

Every Java program starts at a method called `main`. All code lives inside a class.

```java
public class Main {
    public static void main(String[] args) {
        System.out.println("Hello, World!");
    }
}
```

Expected output should be:

```
Hello, World!
```

| Part                 | Meaning                                                                                     |
| -------------------- | ------------------------------------------------------------------------------------------- |
| `public class Main`  | A class named `Main` that any code can use                                                  |
| `public`             | The JVM can call the method from outside the class                                          |
| `static`             | The method belongs to the class, so no object is needed to call it                          |
| `void`               | The method returns nothing                                                                  |
| `main`               | The name the JVM looks for                                                                  |
| `String[] args`      | The command-line arguments (see [Console Input and Output](05_console_input_and_output.md)) |
| `System.out.println` | Print a line to the console                                                                 |

---

## File and Class Names

A `public` class must live in a file with the same name: `public class Main` goes in `Main.java`.

| Rule                                       | Example                                 |
| ------------------------------------------ | --------------------------------------- |
| One `public` top-level class per file      | `Main.java` holds `public class Main`   |
| Other classes in the file are not `public` | `class Helper { }` can sit below `Main` |
| Names are case-sensitive                   | `main.java` does not match `Main`       |
| Class names use PascalCase                 | `OrderService`, not `order_service`     |

---

## Compact Source Files (Java 25+)

For small programs and learning, Java 25 drops the ceremony. The class is created for you, and `main` can be a plain method.

```java
void main() {
    String name = "Java";
    IO.println("Hello, " + name);
}
```

Expected output should be:

```
Hello, Java
```

| Feature           | Meaning                                                                 |
| ----------------- | ----------------------------------------------------------------------- |
| No class line     | The file becomes an unnamed class automatically                         |
| `void main()`     | No `public`, no `static`, and `args` is optional                        |
| `IO.println`      | Print a line. `IO` lives in `java.lang`, so no import is needed         |
| Automatic imports | Every public class in the `java.base` module is usable without `import` |

This course uses the full form, because most real code and Java 21 need it. Compact files are fine for quick tests.

---

## Statements and Blocks

```java
public class Blocks {
    public static void main(String[] args) {
        int a = 2;                 // a statement ends with a semicolon
        int b = 3;

        if (a < b) {               // a block is code inside { }
            System.out.println("a is smaller");
        }

        System.out.println(a + b); // whitespace and line breaks do not matter
    }
}
```

Expected output should be:

```
a is smaller
5
```

- Every statement ends with `;`
- `{ }` groups statements into a block. Variables declared in a block exist only inside it
- Java is case-sensitive: `System` works, `system` does not

---

## Packages

A package groups related classes, like a folder. The `package` line comes first in the file.

```java
package com.example.app;

public class Main {
    public static void main(String[] args) {
        System.out.println("Inside a package");
    }
}
```

The file must sit in a matching folder: `com/example/app/Main.java`. See [Packages and Imports](06_packages_and_imports.md).

---

## How to Run

```bash
java Main.java             # single file, compiled in memory
javac Main.java            # compile to Main.class
java Main                  # run the compiled class
./mvnw compile exec:java   # Maven project (needs the exec plugin)
./gradlew run              # Gradle project
```

See [Projects and Build Tools](02_projects_and_build_tools.md).

---

## Comments

```java
// Single-line comment

/*
   Multi-line comment
*/

/**
 * Javadoc comment: documents the method below it.
 * Tools turn these into HTML pages, and IDEs show them on hover.
 *
 * @param a the first number
 * @param b the second number
 * @return the sum of a and b
 */
public static int add(int a, int b) {
    return a + b;
}
```

| Javadoc tag   | Use                                     |
| ------------- | --------------------------------------- |
| `@param`      | Describe a parameter                    |
| `@return`     | Describe the return value               |
| `@throws`     | Describe an exception the method throws |
| `{@code x}`   | Show `x` as code                        |
| `{@link Foo}` | Link to another class or method         |

Build the HTML pages with `javadoc -d docs -sourcepath src -subpackages com`, or `mvn javadoc:javadoc`.

---

## Markdown Doc Comments (Java 23+)

Java 23 added `///` comments that use Markdown instead of HTML.

```java
/// Returns the sum of two numbers.
///
/// - `a`: the first number
/// - `b`: the second number
///
/// @return the sum
public static int add(int a, int b) {
    return a + b;
}
```

---

## Gotchas

- A missing `;` gives an error on the next line, not the line that is missing it
- `public static void main(String[] args)` must be spelled exactly; `Main` or `String args` makes a normal method the JVM does not find
- `javac` refuses a `public` class whose name does not match the file name
- A compact source file cannot have a `package` line, and other classes cannot use it by name
- Comments do not nest: `/* outer /* inner */ still outer */` ends at the first `*/`

---

## Examples

- [04-01](examples/04-01_entry_point.java): Entry point
- [04-02](examples/04-02_statements_and_blocks.java): Statements and blocks

Run one with `java examples/04-01_entry_point.java`. See [Examples](examples/README.md).
