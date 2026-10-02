# 63 - Packaging and Deployment

## Compile, Package, Deploy

| Step    | Tool                       | Output                                        |
| ------- | -------------------------- | --------------------------------------------- |
| Compile | `javac`                    | `.class` files with bytecode                  |
| Package | `jar`, Maven, Gradle       | A `.jar` file: a zip of classes plus metadata |
| Bundle  | `jlink`, `jpackage`        | A trimmed Java runtime, or an installer       |
| Deploy  | Copy, systemd, a container | The app running on a server or a user's PC    |

Bytecode is the portable instruction set the JVM runs. One `.jar` runs on any operating system that has a Java runtime of the right version. See [Projects and Build Tools](02_projects_and_build_tools.md).

---

## Run Without Packaging

For scripts and small tools, skip the build:

```bash
java Main.java              # compiles in memory and runs (Java 11+)
java Prog.java              # also finds other .java files next to it (Java 22+)
```

Two files in one folder, `Prog.java` and `Helper.java`:

```java
public class Prog {
    public static void main(String[] args) {
        System.out.println(Helper.greet("multi-file"));
    }
}
```

```java
public class Helper {
    static String greet(String name) {
        return "Hello, " + name;
    }
}
```

Run it with `java Prog.java`.

Expected output should be:

```
Hello, multi-file
```

---

## Compile and Make a JAR by Hand

```java
package com.example.app;

public class Main {
    public static void main(String[] args) {
        System.out.println("Hello from a jar");
    }
}
```

Saved as `src/com/example/app/Main.java`:

```bash
javac -d classes src/com/example/app/Main.java                    # compile into classes/
java -cp classes com.example.app.Main                             # run from the folder

jar --create --file app.jar --main-class com.example.app.Main -C classes .
java -jar app.jar                                                  # run the jar
```

Expected output should be:

```
Hello from a jar
```

The JAR was about 1 KB. `--main-class` writes the start class into `META-INF/MANIFEST.MF`, the metadata file inside every jar:

```
Manifest-Version: 1.0
Created-By: 25.0.4.1 (Red Hat, Inc.)
Main-Class: com.example.app.Main
```

| Option                 | Meaning                                             |
| ---------------------- | --------------------------------------------------- |
| `-d classes`           | Put compiled `.class` files in the folder `classes` |
| `-cp` / `--class-path` | Where the JVM looks for classes and jars            |
| `--release 21`         | Compile for Java 21, even with a newer JDK          |
| `-C classes .`         | Add everything from the folder `classes` to the jar |

---

## Build Tools Package for You

```bash
mvn package           # target/my-app-1.0.0.jar
./gradlew build       # build/libs/my-app-1.0.0.jar
```

That jar holds only your classes. Libraries it uses (from Maven Central) are separate jars, and the JVM must find them on the class path, the list of folders and jars it searches for classes.

| Way to ship dependencies | How                                                                              |
| ------------------------ | -------------------------------------------------------------------------------- |
| A `lib/` folder          | Copy the jars next to yours; run `java -cp "app.jar:lib/*" com.example.app.Main` |
| Fat jar (uber jar)       | One jar with every dependency inside. Maven Shade plugin, Gradle Shadow plugin   |
| Framework jar            | Spring Boot and Quarkus build their own runnable jar layout                      |

On Windows the class path separator is `;` instead of `:`.

---

## jlink: a Custom Runtime

`jlink` builds a small Java runtime that contains only the modules your app needs. A module is a named group of packages, declared in `module-info.java`. The target machine then needs no Java installed.

```bash
jdeps --print-module-deps app.jar          # which JDK modules does the app use?

jlink --add-modules java.base,java.net.http \
      --strip-debug --no-header-files --no-man-pages \
      --output runtime

./runtime/bin/java -jar app.jar
```

A runtime with only `java.base` is a few tens of MB, against a few hundred MB for a full JDK. It is built for one operating system and CPU. `jlink` needs the JDK's `jmods` folder, or a JDK built to link from its own run-time image (Java 24+).

---

## jpackage: Installers

`jpackage` wraps your app and a jlink runtime into a native installer or app folder.

```bash
jpackage --name MyApp --input dist --main-jar app.jar \
         --main-class com.example.app.Main --type app-image
```

| `--type`     | Gives                                    |
| ------------ | ---------------------------------------- |
| `app-image`  | A folder with a launcher, any OS         |
| `deb`, `rpm` | Linux packages                           |
| `msi`, `exe` | Windows installers (needs the WiX tools) |
| `dmg`, `pkg` | macOS installers                         |

It builds for the OS it runs on. To ship for three systems, run it on three systems, usually in CI.

---

## Faster Start-Up

| Option                   | What it does                                                            |
| ------------------------ | ----------------------------------------------------------------------- |
| AOT cache (Java 24+)     | A training run saves loaded and linked classes; later starts reuse them |
| CDS (class data sharing) | The older form of the same idea, on by default for JDK classes          |
| GraalVM Native Image     | Compiles the app to a native program with no JVM. Separate tool         |

The AOT cache in Java 25 takes two commands:

```bash
java -XX:AOTCacheOutput=app.aot -jar app.jar   # training run: writes app.aot
java -XX:AOTCache=app.aot -jar app.jar         # later runs start faster
```

For the hello-world jar the cache file was about 10 MB. Native Image starts in milliseconds and uses less memory, but reflection needs extra configuration and the build is slow. See [Annotations and Reflection](55_annotations_and_reflection.md).

---

## Version Your App

```xml
<groupId>com.example</groupId>
<artifactId>my-app</artifactId>
<version>1.2.3</version>
```

Use `MAJOR.MINOR.PATCH`: bump MAJOR for breaking changes, MINOR for new features, PATCH for fixes. A version ending in `-SNAPSHOT` means "still in development" to Maven.

---

## Publish a Library

A library is published as a jar plus a `pom.xml` to a Maven repository.

```bash
mvn install           # into ~/.m2/repository, for other projects on your machine
mvn deploy            # to a remote repository set in the pom
```

Maven Central, the public repository, also requires a sources jar, a Javadoc jar, and signed files. See the Central Portal guide at central.sonatype.org.

---

## Deploy

The jar (or runtime folder) is the deliverable. How it gets to a machine is separate:

| Method                  | How                                                                                               |
| ----------------------- | ------------------------------------------------------------------------------------------------- |
| Copy files              | `scp app.jar user@server:/opt/app/` and run `java -jar app.jar`                                   |
| Run as a service        | A systemd unit on Linux, or a Windows Service                                                     |
| Container               | Copy the jar into an image with a JRE and run it. See [Docker](../../wiki/containers/docker/README.md) |
| Cloud app services      | Upload the jar, or connect a git repository                                                       |
| Share with other people | A `jpackage` installer per OS                                                                     |

A minimal systemd unit:

```ini
[Unit]
Description=My App

[Service]
WorkingDirectory=/opt/app
ExecStart=/usr/bin/java -Xmx512m -jar /opt/app/app.jar
Restart=always
Environment=MY_API_KEY=value-from-a-secret-store

[Install]
WantedBy=multi-user.target
```

In a container, the JVM reads the container's memory limit. Use `-XX:MaxRAMPercentage=75` instead of a fixed `-Xmx`. Never put real secrets in a file that goes into git. See [Security Basics](62_security_basics.md).

---

## Release Checklist

- Tests pass (see [Testing](59_testing.md))
- Version number is bumped
- Compiled with `--release` set to the Java version the servers run
- No secrets, test data, or debug settings in the jar
- Tried the jar on a machine without your development tools
- If jlinked or native, ran the real scenarios, not only start-up
- The jar or installer is saved with its version number so you can roll back

---

## Gotchas

- `UnsupportedClassVersionError` means the jar was compiled for a newer Java than the one running it. Compile with `--release` set to the target version
- `no main manifest attribute` means the jar has no `Main-Class`. Add `--main-class`, or set it in the Maven or Gradle config
- `ClassNotFoundException` at start-up usually means a dependency jar is missing from the class path
- `java -jar` ignores `-cp`. Dependencies must be listed in the manifest's `Class-Path` or packed inside a fat jar
- Fat jars can clash when two libraries contain a file with the same name, such as service files under `META-INF/services`. The Shade and Shadow plugins have settings to merge them
- `jlink` and `jpackage` output is tied to one OS and CPU. Build once per target
