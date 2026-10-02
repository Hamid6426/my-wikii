# 44 - Logging

## Why Log

`System.out.println` leaves no level, no timestamp, and no way to turn it off. **Logging** records what a program did, with a severity, so you can read it later and filter it.

---

## java.util.logging (JUL)

The JDK has a logger built in: `java.util.logging`. A **Logger** writes to one or more **Handlers**, each formatting through a **Formatter**.

```java
import java.util.logging.Logger;

public class App {
    private static final Logger log = Logger.getLogger(App.class.getName());

    public static void main(String[] args) {
        log.info("starting");
        log.warning("disk almost full");
    }
}
```

The default prints `INFO` and above to the console. Lower levels are filtered until you raise the logger level.

---

## The Levels

| Level                     | Use for                                    |
| ------------------------- | ------------------------------------------ |
| `SEVERE`                  | A failure the program cannot continue past |
| `WARNING`                 | Something wrong that is not fatal          |
| `INFO`                    | Normal, important events                   |
| `CONFIG`                  | Setup detail                               |
| `FINE`, `FINER`, `FINEST` | Debug detail, from coarse to very fine     |

`OFF` turns logging off and `ALL` turns every level on.

---

## Configuration

You can set levels in code, or with a `logging.properties` file passed to the JVM.

```bash
java -Djava.util.logging.config.file=logging.properties App
```

Handlers include `ConsoleHandler` and `FileHandler`. In a library, get the logger but do not attach handlers; let the application decide where logs go.

---

## SLF4J and a Backend

Most applications log through **SLF4J**, a facade, and pick a backend such as Logback, Log4j2, or JUL. The `{}` placeholders build the message only when the level is on.

```java
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

private static final Logger log = LoggerFactory.getLogger(App.class);

log.info("user {} logged in from {}", userId, ip);
```

SLF4J and the backends are not in the JDK. Add them with your build tool; see [Common Libraries](66_libraries.md).

---

## What to Log

- At `INFO`, the events you would want when something goes wrong: start, stop, a request, a decision
- At `DEBUG` (`FINE`), detail you need only while diagnosing
- An exception together with its stack trace, once, at the level that matches the impact
- A request or correlation id, so logs from different threads can be lined up

---

## Gotchas

- `log.fine("x = " + x)` builds the string even when `FINE` is off; guard with `isLoggable`, or use SLF4J `{}`
- Log an exception with `log.log(Level.SEVERE, "message", ex)`, not by printing `ex.getMessage()`; the message alone loses the stack trace
- Loggers form a hierarchy by dotted name, with the root logger at the top; a child inherits its parent's level and handlers
- Never log passwords, tokens, card numbers, or other secrets; see [Security Basics](62_security_basics.md)
- Logging inside a hot loop floods the file and slows the program; aggregate or sample

---

## Full Example

```java
import java.util.logging.ConsoleHandler;
import java.util.logging.Formatter;
import java.util.logging.Level;
import java.util.logging.LogRecord;
import java.util.logging.Logger;

public class LogDemo {
    public static void main(String[] args) {
        Logger log = Logger.getLogger("demo");
        log.setUseParentHandlers(false);   // do not also write to the root handler
        log.setLevel(Level.ALL);

        ConsoleHandler handler = new ConsoleHandler();
        handler.setLevel(Level.ALL);
        handler.setFormatter(new Formatter() {
            @Override
            public String format(LogRecord record) {
                return record.getLevel() + ": " + record.getMessage() + System.lineSeparator();
            }
        });
        log.addHandler(handler);

        log.info("starting");
        log.warning("disk almost full");
        log.fine("detail shown only at FINE");
    }
}
```

Expected output should be:

```
INFO: starting
WARNING: disk almost full
FINE: detail shown only at FINE
```

The default logger would drop `FINE`. Raising the logger and handler to `ALL` is what makes it appear.

---

## Examples

- [44-01](examples/44-01_java_util_logging.java): java.util.logging with a custom handler

Run one with `java examples/44-01_java_util_logging.java`. See [Examples](examples/README.md).
