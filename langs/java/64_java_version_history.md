# 64 - Java Version History

What each version of Java added. Use it to read old code and to know which features your JDK supports. LTS (Long-Term Support) versions get fixes for many years; most companies run only those.

## Timeline

| Java   | Year | LTS | Headline features                                                                                      |
| ------ | ---- | --- | ------------------------------------------------------------------------------------------------------ |
| 1.0    | 1996 |     | First release: the JVM, applets, AWT                                                                   |
| 1.1    | 1997 |     | Inner classes, JDBC, reflection, JavaBeans                                                             |
| 1.2    | 1998 |     | Collections framework, Swing, a JIT compiler. Marketed as "Java 2"                                     |
| 1.3    | 2000 |     | HotSpot becomes the default JVM                                                                        |
| 1.4    | 2002 |     | `assert`, regular expressions, NIO, logging, chained exceptions                                        |
| 5      | 2004 |     | Generics, enums, annotations, autoboxing, varargs, for-each loop, `java.util.concurrent`               |
| 6      | 2006 |     | Scripting API, compiler API, JDBC 4                                                                    |
| 7      | 2011 |     | try-with-resources, diamond `<>`, strings in `switch`, multi-catch, `Path` and `Files`, fork/join      |
| **8**  | 2014 | Yes | Lambdas, streams, `java.time`, default methods, `Optional`, `CompletableFuture`                        |
| 9      | 2017 |     | Modules (JPMS), JShell, `List.of` / `Set.of` / `Map.of`, private interface methods                     |
| 10     | 2018 |     | `var` for local variables. Releases now come every six months                                          |
| **11** | 2018 | Yes | Standard `HttpClient`, run a `.java` file directly, new `String` methods (`isBlank`, `strip`, `lines`) |
| 12     | 2019 |     | Switch expressions (preview), `Collectors.teeing`                                                      |
| 13     | 2019 |     | Text blocks (preview)                                                                                  |
| 14     | 2020 |     | Switch expressions, helpful `NullPointerException` messages                                            |
| 15     | 2020 |     | Text blocks, ZGC ready for production                                                                  |
| 16     | 2021 |     | Records, pattern matching for `instanceof`, `Stream.toList()`, `jpackage`                              |
| **17** | 2021 | Yes | Sealed classes, strong encapsulation of JDK internals                                                  |
| 18     | 2022 |     | UTF-8 by default, simple web server (`jwebserver`), code snippets in Javadoc                           |
| 19     | 2022 |     | Virtual threads and record patterns (preview)                                                          |
| 20     | 2023 |     | Previews only: scoped values, record patterns, virtual threads                                         |
| **21** | 2023 | Yes | Virtual threads, pattern matching for `switch`, record patterns, sequenced collections                 |
| 22     | 2024 |     | Unnamed variables `_`, Foreign Function and Memory API, multi-file source programs                     |
| 23     | 2024 |     | Markdown in Javadoc comments, generational ZGC by default                                              |
| 24     | 2025 |     | Stream gatherers, Class-File API, AOT class loading, Security Manager disabled                         |
| **25** | 2025 | Yes | Compact source files and instance `main`, module imports, flexible constructor bodies, scoped values   |

A preview feature is finished but not final. It needs `--enable-preview` to compile and run, and can still change. This course uses only final features.

---

## Which Version Am I Using

```bash
java -version         # the runtime
javac -version        # the compiler
```

```java
public class Version {
    public static void main(String[] args) {
        System.out.println(Runtime.version().feature());   // for example 25
    }
}
```

Compile for an older target with `--release`, so the code runs on that version and cannot use newer APIs:

```bash
javac --release 21 Main.java
```

```xml
<!-- Maven pom.xml -->
<properties>
  <maven.compiler.release>21</maven.compiler.release>
</properties>
```

```kotlin
// Gradle build.gradle.kts
java {
    toolchain {
        languageVersion = JavaLanguageVersion.of(21)
    }
}
```

---

## Where Each Feature Is Taught

| Feature                       | Version | Lesson                                                                       |
| ----------------------------- | ------- | ---------------------------------------------------------------------------- |
| Generics                      | 5       | [Generics](54_generics.md)                                                   |
| Enums                         | 5       | [Enums](09_enums.md)                                                         |
| Annotations                   | 5       | [Annotations and Reflection](55_annotations_and_reflection.md)               |
| Varargs                       | 5       | [Varargs](17_varargs.md)                                                     |
| try-with-resources            | 7       | [AutoCloseable and try-with-resources](33_autocloseable.md)                  |
| `Path` and `Files`            | 7       | [File I/O: Read and Write](45_file_io_read_write.md)                         |
| Lambdas                       | 8       | [Lambdas and Functional Interfaces](39_lambdas_and_functional_interfaces.md) |
| Streams                       | 8       | [Streams](41_streams.md)                                                     |
| `java.time`                   | 8       | [Dates and Times](21_dates_and_times.md)                                     |
| `Optional`                    | 8       | [Null and Optional](24_null_and_optional.md)                                 |
| Default methods               | 8       | [Interfaces and Abstract Classes](32_interfaces_and_abstract_classes.md)     |
| `CompletableFuture`           | 8       | [CompletableFuture and Async Code](51_completablefuture_and_async.md)        |
| Modules                       | 9       | [Packages and Imports](06_packages_and_imports.md)                           |
| `var`                         | 10      | [Variables and Constants](07_variables_and_constants.md)                     |
| `HttpClient`                  | 11      | [HTTP Client](52_http_client.md)                                             |
| Switch expressions            | 14      | [Control Flow](13_control_flow.md)                                           |
| Text blocks                   | 15      | [Strings](20_strings.md)                                                     |
| Records                       | 16      | [Records and Equality](25_records_and_equality.md)                           |
| Sealed classes                | 17      | [Interfaces and Abstract Classes](32_interfaces_and_abstract_classes.md)     |
| Pattern matching for `switch` | 21      | [Pattern Matching](14_pattern_matching.md)                                   |
| Virtual threads               | 21      | [Threading](53_threading.md)                                                 |
| Sequenced collections         | 21      | [Collections](36_collections.md)                                             |
| Unnamed variables `_`         | 22      | [Modern Java Features](40_modern_java_features.md)                           |
| Compact source files          | 25      | [Basics of a Program](04_basics_of_a_program.md)                             |

---

## Release Rhythm

- A new Java version ships every March and September
- An LTS version comes every two years, each September: 17 (2021), 21 (2023), 25 (2025). The next is planned for 2027
- Non-LTS versions get updates only until the next release, six months later
- Features usually arrive as previews first, then become final one or more releases later
- Old code keeps compiling. Removals are rare and announced several releases ahead

The Java language and JVM are open source in the OpenJDK project. Oracle, Eclipse Temurin (Adoptium), Amazon Corretto, Azul Zulu, Microsoft, and Red Hat all ship builds of it. See [Getting Started](01_getting_started.md).

---

## Reading Old Code

| You see                                                   | It is                                                   |
| --------------------------------------------------------- | ------------------------------------------------------- |
| `List<String> list = new ArrayList<String>();`            | Pre-Java 7. Today: `new ArrayList<>()` or `var`         |
| Anonymous class `new Comparator<User>() { ... }`          | Pre-Java 8. Today: a lambda or `Comparator.comparing`   |
| `finally { if (in != null) in.close(); }`                 | Pre-Java 7. Today: try-with-resources                   |
| `Date`, `Calendar`, `SimpleDateFormat`                    | Pre-Java 8. Today: `java.time` (`LocalDate`, `Instant`) |
| `Arrays.asList(...)` or `Collections.unmodifiableList`    | Pre-Java 9. Today: `List.of(...)`                       |
| `if (o instanceof String) { String s = (String) o;`       | Pre-Java 16. Today: `if (o instanceof String s)`        |
| A class with fields, getters, `equals`, `hashCode`        | Pre-Java 16. Today: a `record`                          |
| `"line1\n" + "line2\n"` for long text                     | Pre-Java 15. Today: a text block `"""`                  |
| `public static void main(String[] args)` in a tiny script | Still fine. From Java 25, `void main()` works too       |

---

## Where to Follow Changes

- [JDK Project](https://openjdk.org/projects/jdk/): each release with its list of JEPs
- [JEP index](https://openjdk.org/jeps/0): every JDK Enhancement Proposal, the documents that describe each change
- [dev.java](https://dev.java/): the official learning site from the Java team
- [inside.java](https://inside.java/): news and articles from the people who build Java

---

## Examples

- [64-01](examples/64-01_which_version_am_i_using.java): Which version am i using

Run one with `java examples/64-01_which_version_am_i_using.java`. See [Examples](examples/README.md).
