# 01 - Getting Started

## What You Will Build

A small console program that prints text, running on your own machine in about five minutes.

---

## Java, the JDK and the JVM

| Name     | What it is                                                                          |
| -------- | ----------------------------------------------------------------------------------- |
| Java     | The programming language you write                                                  |
| JVM      | Java Virtual Machine: the program that runs compiled Java code                      |
| JDK      | Java Development Kit: the kit you install. It has the JVM, the library, and tools   |
| JRE      | Java Runtime Environment: an older name for "just the JVM and library", no compiler |
| Bytecode | The compiled form of your code, stored in `.class` files                            |

Install the JDK. It includes everything, so you never need a separate JRE.

Java runs on Windows, macOS, and Linux.

---

## How Your Code Runs

1. You write Java source code (`.java` files)
2. The compiler `javac` turns it into **bytecode** inside `.class` files
3. The `java` command starts the **JVM** and loads those classes
4. The **JIT** (Just-In-Time compiler) inside the JVM turns hot bytecode into machine code for your CPU as the program runs
5. The JVM also manages memory and cleans up unused objects (the garbage collector)

Because the middle step is bytecode, the same `.class` files run on any system that has a JVM. This is the "write once, run anywhere" idea.

---

## Versions and LTS

A new Java version ships every six months (March and September). Every two years one of them is an **LTS** (long-term support) version that gets fixes for many years.

| Version | Type | Notes                                                |
| ------- | ---- | ---------------------------------------------------- |
| 21      | LTS  | Records, pattern matching in switch, virtual threads |
| 25      | LTS  | Compact source files, `IO` class, module imports     |

Pick the newest LTS for new work. This course targets Java 21 and notes where a feature needs 25. See [Java Version History](64_java_version_history.md).

---

## Prerequisites

- A computer running Windows, macOS, or Linux
- A terminal (PowerShell, Terminal, or any Linux shell)
- A text editor (see [IDE For Java](03_ide_for_java.md); any editor works for now)
- Basic computer skills. No programming experience is needed.

---

## Install a JDK

Many companies build the same OpenJDK source code. Any of these is fine:

| Build           | From      |
| --------------- | --------- |
| Eclipse Temurin | Adoptium  |
| Oracle OpenJDK  | Oracle    |
| Amazon Corretto | Amazon    |
| Microsoft Build | Microsoft |
| Red Hat build   | Red Hat   |

Download from [adoptium.net](https://adoptium.net/), or use a package manager:

| System  | Command                                                 |
| ------- | ------------------------------------------------------- |
| Windows | `winget install EclipseAdoptium.Temurin.25.JDK`         |
| macOS   | `brew install --cask temurin`                           |
| Fedora  | `sudo dnf install java-25-openjdk-devel`                |
| Ubuntu  | `sudo apt install openjdk-25-jdk`                       |
| Any     | `sdk install java` (with [SDKMAN!](https://sdkman.io/)) |

Package names carry the version number. Check your system's package list for the current one. On Linux, the package without `-devel` or `-jdk` is only the runtime and has no `javac`.

---

## Check the Install

Open a new terminal and run:

```bash
java -version
javac -version
```

Both print a version number such as `25.0.1`. An error like "command not found" means the JDK is not installed, or the terminal was opened before the install finished.

Many tools find the JDK through the `JAVA_HOME` environment variable. Set it to the JDK folder if a tool complains:

```bash
export JAVA_HOME=/usr/lib/jvm/java-25-openjdk    # Linux example, put it in ~/.bashrc
echo $JAVA_HOME
```

---

## Windows: Installer and JAVA_HOME

Use this when you prefer the `.msi` installer to `winget`.

1. On the download page, pick the LTS tab, then your OS, and download the `.msi`
2. Check the file against the SHA256 value shown on the page (PowerShell):

```powershell
Get-FileHash $HOME\Downloads\jdk-25_windows-x64_bin.msi -Algorithm SHA256
```

The hash is not case sensitive. If it matches the page, the download is intact.

3. Run the installer with the default folder, then open a new terminal and run `java -version`
4. If tools still complain, set `JAVA_HOME`: press the Windows key, search "Environment Variables", choose "Edit the system environment variables", then "Environment Variables"
5. Under System variables, add `JAVA_HOME` with the JDK folder (for example `C:\Program Files\Java\jdk-25`)
6. Edit `Path` and add `%JAVA_HOME%\bin`, then click OK on every window
7. Open a new terminal and check:

```powershell
echo $env:JAVA_HOME
```

---

## Your First Program

Save this as `Hello.java`:

```java
public class Hello {
    public static void main(String[] args) {
        System.out.println("Hello, World!");
    }
}
```

Run it straight from the source file:

```bash
java Hello.java
```

Expected output should be:

```
Hello, World!
```

`java Hello.java` compiles the file in memory and runs it. No `.class` file is written. This works for quick experiments.

---

## Compile, Then Run

The classic two steps, which is what build tools do for you:

```bash
javac Hello.java     # writes Hello.class
java Hello           # runs the class (no .java at the end)
```

| Command            | Effect                                               |
| ------------------ | ---------------------------------------------------- |
| `javac Hello.java` | Compiles source code into `Hello.class`              |
| `java Hello`       | Starts the JVM and runs the `main` method of `Hello` |
| `java Hello.java`  | Compiles in memory and runs, in one step             |

See [Projects and Build Tools](02_projects_and_build_tools.md) for programs with many files.

---

## Compact Source Files (Java 25+)

Java 25 lets a small program skip the class and the long `main` line. Save this as `hi.java`:

```java
void main() {
    IO.println("Hello from a compact source file");
}
```

```bash
java hi.java
```

Expected output should be:

```
Hello from a compact source file
```

`IO.println` is a short form of `System.out.println`, added in Java 25. See [Basics of a Program](04_basics_of_a_program.md).

---

## If Something Goes Wrong

| Problem                                                                             | Fix                                                                                 |
| ----------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- |
| `java: command not found`                                                           | Close and reopen the terminal, then check the install again                         |
| `javac: command not found`, but `java` works                                        | Only the runtime is installed. Install the JDK package                              |
| `javac` says `class Hello is public, should be declared in a file named Hello.java` | Rename the file to match the class name                                             |
| `UnsupportedClassVersionError`                                                      | The code was compiled with a newer JDK than the one running it. Upgrade `java`      |
| Old version printed by `java -version`                                              | Another JDK is first on the `PATH`; on Fedora use `sudo alternatives --config java` |

---

## Next Steps

1. [Projects and Build Tools](02_projects_and_build_tools.md): `javac`, `jar`, Maven and Gradle
2. [IDE For Java](03_ide_for_java.md): pick an editor that gives you code completion and debugging
3. [Basics of a Program](04_basics_of_a_program.md): how a Java program is put together

---

## Examples

- [01-01](examples/01-01_your_first_program.java): Your first program

Run one with `java examples/01-01_your_first_program.java`. See [Examples](examples/README.md).
