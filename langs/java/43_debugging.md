# 43 - Debugging

## What is Debugging

Finding why a program does not do what you expect. A **debugger** lets you pause the program, look at the values inside it, and run it one line at a time. It is faster than adding `System.out.println` everywhere.

---

## The Debugging Loop

1. Reproduce the bug. Find the smallest input that triggers it
2. Guess where the wrong value first appears
3. Pause there and inspect the values
4. Change one thing, run again, and compare
5. Write a test so the bug cannot return (see [Testing](59_testing.md))

---

## Reading a Stack Trace

When a program crashes, the JVM prints a **stack trace**: the error, then the chain of method calls that led to it.

```java
public class Main {
    public static void main(String[] args) {
        System.out.println(divide(4, 0));
    }

    static int divide(int a, int b) {
        return a / b;
    }
}
```

Expected output should be:

```
Exception in thread "main" java.lang.ArithmeticException: / by zero
	at Main.divide(Main.java:7)
	at Main.main(Main.java:3)
```

| Part                            | Meaning                        |
| ------------------------------- | ------------------------------ |
| `Exception in thread "main"`    | The thread that crashed        |
| `java.lang.ArithmeticException` | The type of error              |
| `/ by zero`                     | The message                    |
| `at Main.divide(Main.java:7)`   | Where it was thrown (top line) |
| `at Main.main(Main.java:3)`     | Who called it (read downward)  |

Start at the top line that is in your own code. Lines from `java.base/...` are inside the JDK.

### Caused by

A wrapped exception (see [Error Handling](42_error_handling.md)) shows its cause below, starting with `Caused by:`. The real problem is usually the last `Caused by` in the trace. `... 5 more` means those lines are the same as in the trace above.

---

## Helpful NullPointerException Messages (Java 14)

The message names exactly what was `null`.

```java
public class Npe {
    record User(String name, Address address) {}
    record Address(String city) {}

    public static void main(String[] args) {
        var user = new User("Alice", null);
        System.out.println(user.address().city().length());
    }
}
```

Expected output should be:

```
Exception in thread "main" java.lang.NullPointerException: Cannot invoke "Npe$Address.city()" because the return value of "Npe$User.address()" is null
	at Npe.main(Npe.java:7)
```

`Npe$Address` is how the JVM names `Address` nested inside `Npe`.

---

## Start the Debugger

| Tool          | How                                                        |
| ------------- | ---------------------------------------------------------- |
| IntelliJ IDEA | Click the bug icon next to `main`, or press `Shift+F9`     |
| VS Code       | Install the Extension Pack for Java, then press `F5`       |
| Eclipse       | Right-click the file, then **Debug As > Java Application** |

`java Main.java` from a terminal does not attach a debugger. Use the editor's Debug command. Set up the editor in [IDE For Java](03_ide_for_java.md).

---

## Breakpoints

A **breakpoint** pauses the program just before a line runs.

| Action                     | IntelliJ IDEA                                                          | VS Code                                     |
| -------------------------- | ---------------------------------------------------------------------- | ------------------------------------------- |
| Add or remove a breakpoint | Click the left margin, or `Ctrl+F8`                                    | Click the left margin, or `F9`              |
| Conditional breakpoint     | Right-click the red dot, type a condition                              | Right-click, **Add Conditional Breakpoint** |
| Log without pausing        | Right-click the red dot, untick **Suspend**, tick **Evaluate and log** | Right-click, **Add Logpoint**               |

A conditional breakpoint stops only when a condition is true, such as `i == 500` inside a loop. A logpoint prints a message such as `i = {i}` without stopping and without editing the code.

---

## Stepping

When paused on a line, control the run:

| Action    | IntelliJ IDEA | VS Code     | What it does                                 |
| --------- | ------------- | ----------- | -------------------------------------------- |
| Resume    | `F9`          | `F5`        | Run until the next breakpoint                |
| Step Over | `F8`          | `F10`       | Run this line, do not go inside method calls |
| Step Into | `F7`          | `F11`       | Go inside the method being called            |
| Step Out  | `Shift+F8`    | `Shift+F11` | Finish this method and return to the caller  |
| Stop      | `Ctrl+F2`     | `Shift+F5`  | End the debug session                        |

Shortcuts are the Windows and Linux defaults. macOS uses `Cmd` in place of `Ctrl` for most of them.

---

## Inspecting Values

While paused:

| Panel               | Shows                                                       |
| ------------------- | ----------------------------------------------------------- |
| Variables           | Local variables and their current values                    |
| Watches             | Expressions you type, such as `items.size()` or `a / b`     |
| Frames (Call Stack) | The chain of methods that led here. Click one to jump to it |
| Evaluate Expression | Run any expression now (IntelliJ: `Alt+F8`)                 |

Hover over a variable in the code to see its value. You can also change a value in the Variables panel to test a fix without editing code.

The debugger shows an object using its `toString()`. Override `toString()` in your classes so they read well, such as `Alice (30)`. Records already have a useful one.

---

## Break on Exceptions

Stop at the line that throws, before the program crashes.

- IntelliJ: **Run > View Breakpoints** (`Ctrl+Shift+F8`), then **+ > Java Exception Breakpoints**, and pick a type such as `NullPointerException`
- VS Code: in the Breakpoints panel, tick **Uncaught Exceptions** (or **Caught Exceptions** to stop even on handled ones)

---

## assert

An `assert` checks something that must always be true. Asserts are **off by default** and only run with the `-ea` (enable assertions) flag.

```java
public class Asserts {
    public static void main(String[] args) {
        int count = -1;
        assert count >= 0 : "count must not be negative";
        System.out.println("count is " + count);
    }
}
```

`java Asserts.java` prints `count is -1`, because the assert is skipped.

`java -ea Asserts.java`. Expected output should be:

```
Exception in thread "main" java.lang.AssertionError: count must not be negative
	at Asserts.main(Asserts.java:4)
```

Use `assert` for bugs in your own code. Check user input with `if` and an exception, because that check must always run.

---

## HotSwap

Change code while the program is paused and keep debugging without a restart.

- IntelliJ: edit, then **Run > Debugging Actions > Reload Changed Classes**
- VS Code: save the file, and the change is applied

HotSwap can change the body of a method only. Adding a method or field, or changing a signature, needs a restart.

---

## Remote Debugging

Attach the debugger to a program started somewhere else, such as a server or a container.

```bash
java -agentlib:jdwp=transport=dt_socket,server=y,suspend=n,address=*:5005 -jar app.jar
```

Then create a **Remote JVM Debug** configuration (IntelliJ) or an `attach` configuration (VS Code) for port `5005`. `suspend=y` makes the program wait until the debugger connects. Never open this port on a public network.

---

## Debugging Without an IDE

These tools ship with the full JDK, not with a runtime-only install.

| Need                            | Tool                                                                                    |
| ------------------------------- | --------------------------------------------------------------------------------------- |
| Print values                    | `System.out.println`, or a logger (see [Common Libraries](66_libraries.md))             |
| List running Java programs      | `jps`                                                                                   |
| See what every thread is doing  | `jstack <pid>`, or `jcmd <pid> Thread.print`                                            |
| Save memory to a file           | `jcmd <pid> GC.heap_dump heap.hprof`                                                    |
| Record CPU, memory, and locks   | Java Flight Recorder: `java -XX:StartFlightRecording=duration=30s,filename=rec.jfr ...` |
| View a recording or a heap dump | JDK Mission Control, VisualVM (separate downloads)                                      |
| Command-line debugger           | `jdb`                                                                                   |
| Try a line of code quickly      | `jshell`                                                                                |

A **thread dump** (from `jstack`) is the first thing to take when a program hangs. It shows where each thread is stuck.

---

## Common Bug Patterns

| Symptom                                      | Usual cause                                                                                    |
| -------------------------------------------- | ---------------------------------------------------------------------------------------------- |
| `NullPointerException`                       | Something is `null`. The message names it                                                      |
| `ArrayIndexOutOfBoundsException`             | Loop goes one step too far (`<=` instead of `<`)                                               |
| Value is always zero                         | Integer division (`1 / 2` is `0`) or a missing assignment                                      |
| Two equal strings are not equal              | Compared with `==` instead of `equals`                                                         |
| Object not found in a `HashSet` or `HashMap` | `equals` overridden without `hashCode` (see [lesson 29](29_equals_hashcode_and_comparable.md)) |
| `ConcurrentModificationException`            | Removing from a list inside a for-each loop over it                                            |
| Works once, wrong the second time            | Shared state or a `static` field that is never reset                                           |
| Program hangs                                | A deadlock or an endless loop. Take a thread dump                                              |

---

## Gotchas

- Breakpoints do not hit if the code that runs is not the code you edited. Rebuild, and check the file is part of the project
- Do not put work with side effects inside `assert`. It does not run without `-ea`
- Evaluating an expression in the debugger runs it for real. `list.remove(0)` in a watch changes the list
- A breakpoint pauses one thread by default in VS Code, and all threads by default in IntelliJ. Others may keep running and change the timing
- Changing a variable in the debugger changes only this run

---

## Examples

- [43-01](examples/43-01_assert.java): Assert

Run one with `java examples/43-01_assert.java`. See [Examples](examples/README.md).
