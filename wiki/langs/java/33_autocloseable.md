# 33 - AutoCloseable and try-with-resources

## The Problem

Files, sockets, and database connections must be closed, even when an exception is thrown. The old way needs `finally`:

```java
BufferedReader reader = new BufferedReader(new FileReader("data.txt"));
try {
    System.out.println(reader.readLine());
} finally {
    reader.close();            // runs even if readLine throws
}
```

---

## try-with-resources

Declare the resource in parentheses after `try`. Java calls `close()` for you at the end of the block, whether it ends normally or with an exception.

```java
try (var reader = new BufferedReader(new FileReader("data.txt"))) {
    System.out.println(reader.readLine());
}   // reader.close() runs here
```

Any object whose class implements `AutoCloseable` can go in the parentheses.

---

## AutoCloseable and Closeable

| Interface                 | Method                            | Notes                                               |
| ------------------------- | --------------------------------- | --------------------------------------------------- |
| `java.lang.AutoCloseable` | `void close() throws Exception`   | The base. Use it for your own types                 |
| `java.io.Closeable`       | `void close() throws IOException` | For I/O types. Calling `close()` twice must be safe |

Your own `close()` can declare a narrower exception, or none at all, which saves callers a `catch`.

```java
class Connection implements AutoCloseable {
    private boolean closed;

    void send(String msg) {
        if (closed) throw new IllegalStateException("connection is closed");
        System.out.println("sent " + msg);
    }

    @Override
    public void close() {          // no "throws": callers need no catch
        if (closed) return;        // a second close does nothing
        closed = true;
        System.out.println("closed");
    }
}
```

---

## Several Resources

Separate them with `;`. They close in **reverse** order, so a resource that depends on another closes first.

```java
try (var in = new FileInputStream("a.bin");
     var out = new FileOutputStream("b.bin")) {
    in.transferTo(out);
}   // out closes, then in
```

---

## Existing Variables (Java 9+)

A variable declared before the `try` can be used if it is final or effectively final (never reassigned).

```java
var conn = new Connection();
try (conn) {
    conn.send("hi");
}
```

---

## Suppressed Exceptions

If the body throws and `close()` also throws, the body's exception wins. The `close()` exception is attached to it as a **suppressed** exception instead of being lost.

```java
public class Main {
    public static void main(String[] args) {
        try (var a = new Res("A"); var b = new Res("B")) {
            System.out.println("body");
            throw new IllegalStateException("body failed");
        } catch (IllegalStateException e) {
            System.out.println("caught: " + e.getMessage());
            for (Throwable s : e.getSuppressed()) {
                System.out.println("suppressed: " + s.getMessage());
            }
        }
    }
}

class Res implements AutoCloseable {
    private final String name;

    Res(String name) {
        this.name = name;
        System.out.println("open " + name);
    }

    @Override
    public void close() {
        System.out.println("close " + name);
        throw new RuntimeException("close " + name + " failed");
    }
}
```

Expected output should be:

```
open A
open B
body
close B
close A
caught: body failed
suppressed: close B failed
suppressed: close A failed
```

The `catch` runs after every resource is closed.

---

## No Destructors, No finalize

Java has no destructor that runs at a known time. The old `finalize()` method is deprecated for removal (Java 18) because it might run late or never. Close resources yourself with try-with-resources.

---

## Cleaner: a Safety Net

`java.lang.ref.Cleaner` runs an action after an object becomes unreachable, if `close()` was never called. It is the modern replacement for `finalize()`.

```java
import java.lang.ref.Cleaner;

class NativeBuffer implements AutoCloseable {
    private static final Cleaner CLEANER = Cleaner.create();

    // Must not refer to the NativeBuffer itself, or it can never become unreachable
    private record State(long address) implements Runnable {
        @Override
        public void run() { System.out.println("free " + address); }
    }

    private final Cleaner.Cleanable cleanable;

    NativeBuffer(long address) {
        cleanable = CLEANER.register(this, new State(address));
    }

    @Override
    public void close() {
        cleanable.clean();       // runs State.run() once; later calls do nothing
    }
}
```

| Piece                 | Job                                                                |
| --------------------- | ------------------------------------------------------------------ |
| `Cleaner.create()`    | One cleaner thread, shared by the class                            |
| `register(obj, task)` | Run `task` once `obj` is unreachable                               |
| `State` record        | Holds only what cleanup needs, never a reference back to the owner |
| `cleanable.clean()`   | Runs the task now; it never runs twice                             |

---

## Which One Do I Write

| Your class holds                              | Write                                      |
| --------------------------------------------- | ------------------------------------------ |
| Nothing that needs closing                    | Nothing                                    |
| Other `AutoCloseable` objects                 | `close()` that closes them                 |
| A native resource (off-heap memory, a handle) | `close()` plus a `Cleaner` as a safety net |

For off-heap memory, the Foreign Function and Memory API's `Arena` is itself `AutoCloseable`. See [Memory and Garbage Collection](34_memory_and_garbage_collection.md).

---

## Gotchas

- Make `close()` safe to call twice, using a `closed` flag
- Throw `IllegalStateException` if someone uses the object after `close()`
- If a class owns an `AutoCloseable` field, the class should be `AutoCloseable` too and close that field
- `catch` and `finally` blocks attached to a try-with-resources run **after** the resources are closed
- Do not close objects you did not create (for example `System.out`); the caller owns them
- Wrapping streams: closing the outer `BufferedReader` closes the inner `FileReader` too, so declare only the outer one or both in order

---

## Examples

- [33-01](examples/33-01_suppressed_exceptions.java): Suppressed exceptions

Run one with `java examples/33-01_suppressed_exceptions.java`. See [Examples](examples/README.md).
