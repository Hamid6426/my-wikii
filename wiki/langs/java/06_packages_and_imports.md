# 06 - Packages and Imports

## What is a Package

A named group of related classes. It keeps names from clashing and controls what other code can see.

```java
package com.example.shop;

public class Order {
}
```

The full name of this class is `com.example.shop.Order`. Two classes can both be called `Order` if they sit in different packages.

The `package` line must be the first line of code in the file. A file with no `package` line is in the unnamed (default) package, which is fine for quick tests only.

---

## Folders Must Match

The folders under the source root must match the package name, one folder per part.

```
src/main/java/
  com/example/shop/
    Main.java            package com.example.shop;
    Order.java           package com.example.shop;
    util/
      Money.java         package com.example.shop.util;
```

`com.example.shop.util` is a separate package, not part of `com.example.shop`. Nesting is only for names and folders. The two packages get no special access to each other.

---

## Naming Packages

| Rule                                  | Example                                    |
| ------------------------------------- | ------------------------------------------ |
| All lowercase                         | `com.example.shop`, not `com.example.Shop` |
| Start with a domain you own, reversed | `example.com` becomes `com.example`        |
| Then the project, then the area       | `com.example.shop.billing`                 |
| No `java.` or `javax.` at the start   | Those belong to the JDK                    |
| Replace hyphens and leading digits    | `my-app` becomes `my_app` or `myapp`       |

---

## import

`import` lets you use a class by its short name.

```java
import java.time.LocalDate;          // one class
import java.util.*;                  // every class in java.util

public class Imports {
    public static void main(String[] args) {
        List<String> names = new ArrayList<>(List.of("Bob", "Alice"));
        Collections.sort(names);
        System.out.println(names);

        LocalDate date = LocalDate.of(2025, 9, 16);
        System.out.println(date.getYear());
    }
}
```

Expected output should be:

```
[Alice, Bob]
2025
```

| Fact                                       | Detail                                                     |
| ------------------------------------------ | ---------------------------------------------------------- |
| `java.lang` is always imported             | `String`, `Math`, `System`, `Integer` need no `import`     |
| Classes in the same package need no import | `Main` can use `Order` directly                            |
| `*` imports one package, not sub-packages  | `import java.util.*;` does not import `java.util.function` |
| Imports cost nothing at run time           | They only tell the compiler where to find names            |

Most teams prefer one import per class. IDEs add and remove them for you.

---

## Name Clashes

When two packages have a class with the same name, import one and write the full name for the other.

```java
import java.util.Date;

public class Clash {
    public static void main(String[] args) {
        Date now = new Date(0);                          // java.util.Date
        java.sql.Date sqlDate = new java.sql.Date(0);    // full name, no import
        System.out.println(now.getClass().getName());
        System.out.println(sqlDate.getClass().getName());
    }
}
```

Expected output should be:

```
java.util.Date
java.sql.Date
```

Java has no import alias like `import X as Y`.

---

## import static

Use static members (fields and methods that belong to a class) without the class name.

```java
import static java.lang.Math.PI;
import static java.lang.Math.sqrt;

public class Static {
    public static void main(String[] args) {
        System.out.println(sqrt(16));             // instead of Math.sqrt(16)
        System.out.printf("%.4f%n", PI);          // instead of Math.PI
    }
}
```

Expected output should be:

```
4.0
3.1416
```

`import static java.lang.Math.*;` brings in all of them. Use it sparingly: readers can no longer tell where `sqrt` comes from. It is common in tests, such as `import static org.junit.jupiter.api.Assertions.*;`.

---

## Module Imports (Java 25+)

A **module** is a named group of packages (see below). `import module` imports every public class from every package the module exports.

```java
import module java.base;              // every public class in java.base

public class Mod {
    public static void main(String[] args) {
        List<Integer> nums = List.of(3, 1, 2);            // java.util
        Path file = Path.of("notes.txt");                 // java.nio.file
        LocalDate day = LocalDate.of(2025, 1, 1);         // java.time
        System.out.println(nums + " " + file + " " + day);
    }
}
```

Expected output should be:

```
[3, 1, 2] notes.txt 2025-01-01
```

Compact source files (see [Basics of a Program](04_basics_of_a_program.md)) import `java.base` automatically. If two modules both have a class with the same name, such as `Date` in `java.base` and `java.sql`, add a normal `import` for the one you want.

---

## Packages and Access

The package is also a privacy boundary. A member with no access keyword is **package-private**: visible only inside its own package.

```java
// src/com/example/shop/util/Money.java
package com.example.shop.util;

public class Money {
    public static String format(int cents) {
        return "$" + (cents / 100) + "." + pad(cents % 100);
    }

    static String pad(int n) {            // no modifier: package-private
        return n < 10 ? "0" + n : "" + n;
    }
}
```

```java
// src/com/example/shop/Main.java
package com.example.shop;

import com.example.shop.util.Money;

public class Main {
    public static void main(String[] args) {
        System.out.println(Money.format(1205));
        // Money.pad(5);                  // error: pad is not public
    }
}
```

Expected output should be:

```
$12.05
```

| Keyword     | Same class | Same package | Subclass elsewhere | Anywhere |
| ----------- | ---------- | ------------ | ------------------ | -------- |
| `public`    | Yes        | Yes          | Yes                | Yes      |
| `protected` | Yes        | Yes          | Yes                | No       |
| none        | Yes        | Yes          | No                 | No       |
| `private`   | Yes        | No           | No                 | No       |

See [Classes and Objects](26_classes_and_objects.md) for more on access.

---

## Compiling and Running Packages

Run commands from the source root, and name the class by its full name.

```bash
javac -d out $(find src -name "*.java")    # compile every file
java -cp out com.example.shop.Main         # full class name, not a path
java src/com/example/shop/Main.java        # or the source launcher (Java 22+)
```

Build tools do this for you. See [Projects and Build Tools](02_projects_and_build_tools.md).

---

## Modules in Brief

A module groups packages and states which ones other code may use. It is described in a `module-info.java` file at the source root.

```java
module com.example.shop {
    requires java.sql;                // modules this one needs
    exports com.example.shop.api;     // packages others may use
}
```

| Fact                                       | Detail                                                 |
| ------------------------------------------ | ------------------------------------------------------ |
| Added in Java 9                            | Called the Java Platform Module System (JPMS)          |
| The JDK itself is split into modules       | `java.base`, `java.sql`, `java.net.http`, and more     |
| Your own code does not need to be a module | Without `module-info.java`, everything works as before |
| Useful for libraries and small runtimes    | `jlink` builds a trimmed JDK from the modules you use  |

See [Packaging and Deployment](63_packaging_and_deployment.md).

---

## Third-Party Packages

Libraries from other people are added through Maven or Gradle, then imported like any other package.

```java
import com.google.gson.Gson;      // after adding the gson dependency
```

See [Dependencies](02_projects_and_build_tools.md#dependencies) and [Common Libraries](66_libraries.md).

---

## Gotchas

- The folder path must match the `package` line, or `javac` and the IDE report the class in the wrong place
- `java com/example/shop/Main` (with slashes) fails; use dots: `com.example.shop.Main`
- `import java.util.*;` plus `import java.sql.*;` makes `Date` ambiguous; the error appears only where you use `Date`
- Unused imports are harmless but clutter the file; let the IDE organize them
- Classes in the default package cannot be imported by classes in named packages
- A sub-package is a separate package; package-private members are not visible from `com.example.shop.util` to `com.example.shop`

---

## Examples

- [06-01](examples/06-01_import.java): Import
- [06-02](examples/06-02_name_clashes.java): Name clashes
- [06-03](examples/06-03_import_static.java): Import static
- [06-04](examples/06-04_module_imports_java_25.java): Module imports java 25

Run one with `java examples/06-01_import.java`. See [Examples](examples/README.md).
