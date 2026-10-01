# 35 - JVM Internals

## What the JVM Does

The `java` command starts a **JVM**. It loads `.class` files, checks them, runs them, and manages memory. The same bytecode runs on any machine with a JVM, which is what "write once, run anywhere" means.

| Step       | What happens                                                     |
| ---------- | ---------------------------------------------------------------- |
| Load       | A class loader reads `.class` bytes from the class path or a jar |
| Link       | Verify, prepare (defaults), and resolve references               |
| Initialize | Run the static initializers, once                                |
| Run        | Execute bytecode, interpreted first, compiled later              |

---

## Class Loading

Three built-in loaders, each with a parent, and a class is loaded the first time it is used, not at startup.

| Loader      | Loads                                                                      |
| ----------- | -------------------------------------------------------------------------- |
| Bootstrap   | Core JDK classes such as `java.lang.String` (`getClassLoader()` is `null`) |
| Platform    | Other JDK modules                                                          |
| Application | Your class path and jars                                                   |

A loader first asks its parent, then looks itself. Identity is `(class, loader)`, so the same class name loaded by two loaders gives two different classes.

---

## Bytecode

`javac` turns source into **bytecode**, the instructions in a `.class` file. The JVM is stack-based: bytecode pushes and pops values. `javap` disassembles it.

```bash
javap -c -p MyClass          # show bytecode
javap -verbose MyClass       # plus the constant pool and class version
```

You rarely read bytecode by hand, but it explains some behavior, such as when a lambda becomes a synthetic method.

---

## Interpreter and JIT

A fresh JVM interprets bytecode. When a method runs often ("hot"), the **JIT** compiler turns it into native machine code, with two tiers (C1 fast, C2 optimizing). It can inline small methods and remove code that cannot be reached.

- Warm-up: the first calls are slow while methods are still interpreted
- Peak speed comes after the hot paths are compiled
- Microbenchmarks on cold code mislead; use JMH. See [Performance Basics](61_performance_basics.md)

---

## Memory Areas

The JVM splits memory into areas. See [Memory and Garbage Collection](34_memory_and_garbage_collection.md) for how the heap is collected.

| Area         | Holds                                                        |
| ------------ | ------------------------------------------------------------ |
| Heap         | All objects, shared by every thread (see the GC lesson)      |
| Stack        | One frame per method call, per thread: locals and references |
| Metaspace    | Class metadata (since Java 8, off-heap)                      |
| PC register  | The current instruction address per thread                   |
| Native stack | Frames for native (JNI) calls                                |

`OutOfMemoryError: Java heap space` is the heap; `StackOverflowError` is a stack, usually deep recursion.

---

## Looking Inside

The JDK ships tools to see what the JVM is doing.

| Tool                     | Shows                                                                                 |
| ------------------------ | ------------------------------------------------------------------------------------- |
| `javap`                  | Bytecode and class structure                                                          |
| `jcmd <pid> <command>`   | Thread dumps, heap info, VM flags                                                     |
| `jstack`                 | Thread stacks, for deadlocks                                                          |
| `jmap`                   | Heap summary and dumps                                                                |
| JDK Flight Recorder      | Low-overhead recording of events (see [Performance Basics](61_performance_basics.md)) |
| `java -XshowSettings:vm` | The JVM's own settings                                                                |

---

## Not Only HotSpot

**HotSpot** is the default JVM. **OpenJ9** targets a smaller footprint and faster startup. **GraalVM** adds an ahead-of-time compiler (`native-image`) that turns a jar into a native executable, trading peak throughput for startup and memory. The bytecode you compile does not change.

---

## Gotchas

- Class loading is lazy; a static initializer runs on first use, which can surprise you with a slow "first call"
- Startup is dominated by class loading and verification, not by your code
- `-Xmx` sets the max heap and `-Xms` the initial heap; the default is a fraction of the machine's RAM
- The JIT only optimizes methods after they are hot, so a quick run never reaches peak speed
- Bytecode is portable, not safe by itself; the verifier checks each class before it runs
- Two loaders loading the same class name produce two incompatible classes, the cause of many `ClassCastException` and `LinkageError` surprises

---

## Full Example

```java
public class VmInfo {
    static class Loaded {
        static { System.out.println("Loaded class initialized"); }
        static int value = 7;
    }

    public static void main(String[] args) {
        System.out.println("bootstrap loader of String is: " + String.class.getClassLoader());
        System.out.println("our loader is not null: " + (VmInfo.class.getClassLoader() != null));

        System.out.println("before first use");
        System.out.println("value = " + Loaded.value);
        System.out.println("after first use");
    }
}
```

Expected output should be:

```
bootstrap loader of String is: null
our loader is not null: true
before first use
Loaded class initialized
value = 7
after first use
```

The second line before `value` shows that the nested class is initialized on first use, not at startup.

---

## Examples

- [35-01](examples/35-01_class_loading_and_vm.java): Class loading and the VM

Run one with `java examples/35-01_class_loading_and_vm.java`. See [Examples](examples/README.md).
