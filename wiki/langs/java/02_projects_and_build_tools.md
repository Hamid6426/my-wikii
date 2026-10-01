# 02 - Projects and Build Tools

## What is a Build Tool

A program that compiles your code, downloads the libraries it needs, runs the tests, and packages the result. You describe the project in one file, and the tool does the rest.

| Tool    | Project file                                           | Output folder |
| ------- | ------------------------------------------------------ | ------------- |
| Maven   | `pom.xml` (XML)                                        | `target/`     |
| Gradle  | `build.gradle.kts` (Kotlin) or `build.gradle` (Groovy) | `build/`      |
| By hand | none: you run `javac`, `java`, `jar` yourself          | any           |

Learn the by-hand commands once so you know what the tools do. Use Maven or Gradle for real projects.

---

## Several Files by Hand

```
hello/
  src/
    com/example/app/Main.java
    com/example/app/Greeter.java
```

```java
// src/com/example/app/Greeter.java
package com.example.app;

public class Greeter {
    public static String greet(String name) {
        return "Hello, " + name;
    }
}
```

```java
// src/com/example/app/Main.java
package com.example.app;

public class Main {
    public static void main(String[] args) {
        System.out.println(Greeter.greet("Java"));
    }
}
```

```bash
javac -d out src/com/example/app/*.java    # compile into out/
java -cp out com.example.app.Main          # run, using out/ as the class path
```

Expected output should be:

```
Hello, Java
```

| Option                 | Meaning                                                                                 |
| ---------------------- | --------------------------------------------------------------------------------------- |
| `-d out`               | Put the `.class` files in `out/`, in folders that match the packages                    |
| `-cp out`              | Class path: where the JVM looks for classes. Separate entries with `:` (`;` on Windows) |
| `com.example.app.Main` | The full class name, including the package, not a file path                             |

See [Packages and Imports](06_packages_and_imports.md) for why the folders match the package names.

---

## Run Several Files Without Compiling (Java 22+)

The source launcher finds the other files on its own:

```bash
java src/com/example/app/Main.java
```

Good for small programs. Real projects still use a build tool.

---

## JAR Files

A **JAR** (Java ARchive) is a zip file of `.class` files plus a manifest that can name the main class.

```bash
jar --create --file app.jar --main-class com.example.app.Main -C out .
java -jar app.jar
```

| Command                     | Effect                                    |
| --------------------------- | ----------------------------------------- |
| `jar --create --file x.jar` | Make a new JAR                            |
| `--main-class`              | Record the class `java -jar` should start |
| `-C out .`                  | Add everything inside `out/`              |
| `jar --list --file app.jar` | Show what is inside                       |
| `java -jar app.jar`         | Run the main class from the manifest      |

---

## Standard Project Layout

Maven and Gradle both expect this layout:

```
my-app/
  pom.xml or build.gradle.kts
  src/
    main/
      java/            app code, in package folders
      resources/       config files, copied next to the classes
    test/
      java/            test code
      resources/
```

Keep to it. Tools, IDEs, and other developers all assume it.

---

## Maven

Create a project from a template (called an archetype):

```bash
mvn archetype:generate -DgroupId=com.example -DartifactId=my-app \
    -DarchetypeArtifactId=maven-archetype-quickstart -DinteractiveMode=false
cd my-app
```

A small `pom.xml`:

```xml
<project xmlns="http://maven.apache.org/POM/4.0.0">
  <modelVersion>4.0.0</modelVersion>

  <groupId>com.example</groupId>
  <artifactId>my-app</artifactId>
  <version>1.0-SNAPSHOT</version>

  <properties>
    <maven.compiler.release>21</maven.compiler.release>
    <project.build.sourceEncoding>UTF-8</project.build.sourceEncoding>
  </properties>
</project>
```

| Command       | Effect                                                |
| ------------- | ----------------------------------------------------- |
| `mvn compile` | Compile `src/main/java` into `target/classes`         |
| `mvn test`    | Compile and run the tests                             |
| `mvn package` | Build the JAR in `target/`                            |
| `mvn clean`   | Delete `target/`                                      |
| `mvn verify`  | Run every check up to and including integration tests |

Each command runs every earlier step too: `package` also compiles and tests. See the [Maven docs](https://maven.apache.org/guides/getting-started/).

---

## Gradle

Create a project:

```bash
gradle init --type java-application
```

A small `build.gradle.kts`:

```kotlin
plugins {
    application
}

repositories {
    mavenCentral()
}

java {
    toolchain {
        languageVersion = JavaLanguageVersion.of(21)
    }
}

application {
    mainClass = "com.example.app.Main"
}
```

| Command           | Effect                       |
| ----------------- | ---------------------------- |
| `./gradlew build` | Compile, test, and package   |
| `./gradlew run`   | Build and run the main class |
| `./gradlew test`  | Run the tests                |
| `./gradlew clean` | Delete `build/`              |
| `./gradlew tasks` | List every task you can run  |

A **toolchain** tells Gradle which JDK to compile with, even if a different one runs Gradle. See the [Gradle docs](https://docs.gradle.org/current/userguide/building_java_projects.html).

---

## Wrappers

`mvnw` and `gradlew` are small scripts checked into the project. They download the exact tool version the project needs, so nobody has to install Maven or Gradle.

```bash
./mvnw package        # Maven wrapper
./gradlew build       # Gradle wrapper
```

On Windows run `mvnw.cmd` or `gradlew.bat`. Always use the wrapper when a project has one.

---

## Dependencies

Libraries come from [Maven Central](https://central.sonatype.com/), the main online store of Java libraries. A library is named by three parts: `groupId:artifactId:version`.

Maven, inside `<project>`:

```xml
<dependencies>
  <dependency>
    <groupId>com.google.code.gson</groupId>
    <artifactId>gson</artifactId>
    <version>2.13.1</version>
  </dependency>
</dependencies>
```

Gradle:

```kotlin
dependencies {
    implementation("com.google.code.gson:gson:2.13.1")
    testImplementation("org.junit.jupiter:junit-jupiter:5.13.4")
}
```

The tool downloads the JAR and every library it needs into a local cache (`~/.m2` for Maven, `~/.gradle` for Gradle). Search Maven Central for the current version.

| Scope (Maven) | Gradle               | Used for                                         |
| ------------- | -------------------- | ------------------------------------------------ |
| `compile`     | `implementation`     | App code (the default)                           |
| `test`        | `testImplementation` | Tests only                                       |
| `provided`    | `compileOnly`        | Compile only, the server provides it at run time |
| `runtime`     | `runtimeOnly`        | Run time only, such as a database driver         |

---

## Maven or Gradle

| Point          | Maven                      | Gradle                               |
| -------------- | -------------------------- | ------------------------------------ |
| Build file     | XML, fixed structure       | Kotlin or Groovy code, very flexible |
| Learning curve | Lower                      | Higher                               |
| Speed          | Good                       | Faster on big builds (cache, daemon) |
| Common in      | Enterprise and Spring apps | Android, large multi-module builds   |

Both use the same layout and the same Maven Central libraries. Pick what your team uses.

---

## Command Reference

| Task          | By hand                 | Maven                    | Gradle              |
| ------------- | ----------------------- | ------------------------ | ------------------- |
| Compile       | `javac -d out src/...`  | `mvn compile`            | `./gradlew classes` |
| Run           | `java -cp out pkg.Main` | `mvn exec:java` (plugin) | `./gradlew run`     |
| Test          | none                    | `mvn test`               | `./gradlew test`    |
| Package a JAR | `jar --create ...`      | `mvn package`            | `./gradlew jar`     |
| Clean         | `rm -r out`             | `mvn clean`              | `./gradlew clean`   |

---

## Gotchas

- `java -cp out Main` fails with `ClassNotFoundException` if `Main` is in a package; use the full name `com.example.app.Main`
- `java -jar app.jar` fails with "no main manifest attribute" if the JAR does not name a main class
- A plain JAR from `mvn package` does not include your dependencies; you need a plugin (Shade, Assembly) or a tool such as `jlink`; see [Packaging and Deployment](63_packaging_and_deployment.md)
- Set the Java version in the build file (`maven.compiler.release` or a toolchain), or the build uses whatever JDK happens to run it
- Commit the wrapper scripts and their `.mvn/` or `gradle/` folders; never commit `target/` or `build/`
