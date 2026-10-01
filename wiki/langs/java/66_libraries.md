# 66 - Common Libraries

A library is code you call from your own program. Java libraries come as jars from Maven Central, the public repository. You add one with its coordinates: `groupId:artifactId:version`. This page lists the best-known ones by job. Check each site for the current version.

## Built Into the JDK (no install)

| Package                              | What it does                                    | Lesson                                           |
| ------------------------------------ | ----------------------------------------------- | ------------------------------------------------ |
| `java.util`                          | Lists, maps, sets, queues, `Optional`, `Random` | [Collections](36_collections.md)                 |
| `java.util.stream`                   | Process collections in pipelines                | [Streams](41_streams.md)                         |
| `java.time`                          | Dates, times, durations, time zones             | [Dates and Times](21_dates_and_times.md)         |
| `java.nio.file`, `java.io`           | Files, folders, and byte and text streams       | [File I/O](45_file_io_read_write.md)             |
| `java.util.regex`                    | Pattern matching on text                        | [Regular Expressions](49_regular_expressions.md) |
| `java.net.http`                      | Call web APIs                                   | [HTTP Client](52_http_client.md)                 |
| `java.util.concurrent`               | Thread pools, `CompletableFuture`, locks        | [Threading](53_threading.md)                     |
| `java.lang.reflect`                  | Inspect types at run time                       | [Reflection](55_annotations_and_reflection.md)   |
| `java.security`, `javax.crypto`      | Hashing, encryption, secure random numbers      | [Security Basics](62_security_basics.md)         |
| `java.sql`                           | JDBC: talk to databases through a driver        | [Common Frameworks](65_frameworks.md)            |
| `java.util.logging`, `System.Logger` | Basic logging                                   | [Debugging](43_debugging.md)                     |
| `java.lang.management`, `jdk.jfr`    | Memory, threads, and Flight Recorder            | [Performance Basics](61_performance_basics.md)   |

The JDK has no JSON parser. See [JSON](48_json.md) for the library choice.

---

## Adding a Library

```xml
<!-- Maven pom.xml -->
<dependency>
  <groupId>com.google.guava</groupId>
  <artifactId>guava</artifactId>
  <version>LATEST_VERSION</version>
</dependency>
```

```kotlin
// Gradle build.gradle.kts
dependencies {
    implementation("com.google.guava:guava:LATEST_VERSION")
}
```

Replace `LATEST_VERSION` with the number from the library's site or from [central.sonatype.com](https://central.sonatype.com/). See [Projects and Build Tools](02_projects_and_build_tools.md).

---

## Testing

| Library        | What it does                                                   | Site                                                                                |
| -------------- | -------------------------------------------------------------- | ----------------------------------------------------------------------------------- |
| JUnit          | The test framework (see [Testing](59_testing.md))              | [junit.org](https://junit.org/)                                                     |
| Mockito        | Mocks (see [Mocking and Fixtures](60_mocking_and_fixtures.md)) | [site.mockito.org](https://site.mockito.org/)                                       |
| AssertJ        | Readable, chained assertions                                   | [assertj.github.io](https://assertj.github.io/doc/)                                 |
| Testcontainers | Run real databases in Docker for tests                         | [testcontainers.com](https://testcontainers.com/)                                   |
| ArchUnit       | Tests that check package and layer rules                       | [archunit.org](https://www.archunit.org/)                                           |
| JMH            | Precise micro-benchmarks, from OpenJDK                         | [openjdk.org/projects/code-tools/jmh](https://openjdk.org/projects/code-tools/jmh/) |

---

## JSON and Other Formats

| Library            | What it does                                    | Site                                                                                    |
| ------------------ | ----------------------------------------------- | --------------------------------------------------------------------------------------- |
| Jackson            | The most used JSON library; also YAML, XML, CSV | [github.com/FasterXML/jackson](https://github.com/FasterXML/jackson)                    |
| Gson               | Simple JSON from Google                         | [github.com/google/gson](https://github.com/google/gson)                                |
| SnakeYAML          | Read and write YAML                             | [bitbucket.org/snakeyaml](https://bitbucket.org/snakeyaml/snakeyaml)                    |
| Apache Commons CSV | Read and write CSV                              | [commons.apache.org/proper/commons-csv](https://commons.apache.org/proper/commons-csv/) |
| Protocol Buffers   | Compact binary messages from Google             | [protobuf.dev](https://protobuf.dev/)                                                   |
| jsoup              | Parse and clean HTML                            | [jsoup.org](https://jsoup.org/)                                                         |
| Apache POI         | Read and write Excel and Word files             | [poi.apache.org](https://poi.apache.org/)                                               |
| Apache PDFBox      | Create and read PDFs                            | [pdfbox.apache.org](https://pdfbox.apache.org/)                                         |

---

## Logging and Monitoring

| Library       | What it does                                               | Site                                                              |
| ------------- | ---------------------------------------------------------- | ----------------------------------------------------------------- |
| SLF4J         | A logging API. Code logs to it; a backend writes the lines | [slf4j.org](https://www.slf4j.org/)                               |
| Logback       | The usual SLF4J backend                                    | [logback.qos.ch](https://logback.qos.ch/)                         |
| Log4j 2       | Another popular logging backend from Apache                | [logging.apache.org/log4j](https://logging.apache.org/log4j/2.x/) |
| Micrometer    | App metrics for monitoring systems                         | [micrometer.io](https://micrometer.io/)                           |
| OpenTelemetry | Traces, metrics, and logs in one standard                  | [opentelemetry.io](https://opentelemetry.io/docs/languages/java/) |

---

## Data Access

| Library                                         | What it does                      | Site                                                                                     |
| ----------------------------------------------- | --------------------------------- | ---------------------------------------------------------------------------------------- |
| PostgreSQL JDBC, MySQL Connector/J, SQLite JDBC | Database drivers for `java.sql`   | [jdbc.postgresql.org](https://jdbc.postgresql.org/)                                      |
| HikariCP                                        | Fast database connection pool     | [github.com/brettwooldridge/HikariCP](https://github.com/brettwooldridge/HikariCP)       |
| Flyway, Liquibase                               | Versioned database schema changes | [flywaydb.org](https://flywaydb.org/), [liquibase.org](https://www.liquibase.org/)       |
| Jedis, Lettuce                                  | Redis clients                     | [redis.io/docs](https://redis.io/docs/latest/develop/clients/)                           |
| MongoDB Java Driver                             | MongoDB client                    | [mongodb.com/docs/drivers/java](https://www.mongodb.com/docs/drivers/java/sync/current/) |

---

## General Utilities

| Library             | What it does                                                        | Site                                                                                      |
| ------------------- | ------------------------------------------------------------------- | ----------------------------------------------------------------------------------------- |
| Guava               | Extra collections, caching, string and math helpers                 | [github.com/google/guava](https://github.com/google/guava)                                |
| Apache Commons Lang | Helpers for strings, numbers, and objects                           | [commons.apache.org/proper/commons-lang](https://commons.apache.org/proper/commons-lang/) |
| Caffeine            | Fast in-memory cache                                                | [github.com/ben-manes/caffeine](https://github.com/ben-manes/caffeine)                    |
| Resilience4j        | Retry, timeout, and circuit breaker policies                        | [resilience4j.readme.io](https://resilience4j.readme.io/)                                 |
| MapStruct           | Generates code that copies data between object types                | [mapstruct.org](https://mapstruct.org/)                                                   |
| Lombok              | Generates getters, constructors, and builders from annotations      | [projectlombok.org](https://projectlombok.org/)                                           |
| picocli             | Parse command-line arguments                                        | [picocli.info](https://picocli.info/)                                                     |
| Hibernate Validator | Rules on fields such as `@NotNull` and `@Size` (Jakarta Validation) | [hibernate.org/validator](https://hibernate.org/validator/)                               |

---

## HTTP and Dependency Injection

| Library           | What it does                                 | Site                                                             |
| ----------------- | -------------------------------------------- | ---------------------------------------------------------------- |
| OkHttp            | HTTP client from Square                      | [github.com/square/okhttp](https://github.com/square/okhttp)     |
| Retrofit          | Typed HTTP clients from interfaces           | [github.com/square/retrofit](https://github.com/square/retrofit) |
| Apache HttpClient | Long-standing, configurable HTTP client      | [hc.apache.org](https://hc.apache.org/)                          |
| Google Guice      | Dependency injection container               | [github.com/google/guice](https://github.com/google/guice)       |
| Dagger            | Dependency injection checked at compile time | [dagger.dev](https://dagger.dev/)                                |

See [Dependency Injection](56_dependency_injection.md).

---

## Picking a Library

- Prefer the JDK when it does the job: `HttpClient`, `java.time`, and records remove the need for many old libraries
- Check the last release date, open issues, and whether maintainers answer them
- Check the license, especially for commercial work
- Lombok changes how code compiles. Records now cover many of its uses
- Keep the number of dependencies small; every library is something to update and a possible security hole (see [Security Basics](62_security_basics.md))
