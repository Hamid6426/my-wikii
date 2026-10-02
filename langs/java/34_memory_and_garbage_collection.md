# 34 - Memory and Garbage Collection

## Stack and Heap

| Place | Holds                                                                 | Cleaned up by                       |
| ----- | --------------------------------------------------------------------- | ----------------------------------- |
| Stack | Local primitives, references, method call data (one stack per thread) | Automatically when a method returns |
| Heap  | Every object and array                                                | The garbage collector (GC)          |

A reference variable on the stack points to an object on the heap. Java has no `struct`: every object you create with `new` lives on the heap (the JIT compiler may skip the allocation when an object never leaves a method, which is called escape analysis).

---

## The Garbage Collector

The GC finds objects that nothing can reach any more and frees their memory. There is no `delete` and no `free`.

```java
void work() {
    byte[] data = new byte[1_000_000];   // on the heap
}   // nothing points to the array now; the GC frees it later
```

An object is **reachable** if a chain of references leads to it from a **GC root**: a local variable, a static field, or an active thread. Cycles (A points to B, B points to A) are freed too, once nothing outside reaches them.

You do not control when the GC runs.

---

## Generations

Most objects die young. So the heap is split by age, and the young part is collected often and cheaply.

| Area                   | Holds                                  | Collected           |
| ---------------------- | -------------------------------------- | ------------------- |
| Young: Eden            | Brand new objects                      | Very often          |
| Young: Survivor spaces | Objects that survived a collection     | Very often          |
| Old (tenured)          | Objects that survived many collections | Rarely              |
| Metaspace (not heap)   | Class metadata                         | When classes unload |

---

## Garbage Collectors

The JVM ships several collectors. Pick one with a flag.

| Collector  | Flag                   | Good for                                                                             |
| ---------- | ---------------------- | ------------------------------------------------------------------------------------ |
| G1         | `-XX:+UseG1GC`         | Default. A balance of speed and short pauses                                         |
| ZGC        | `-XX:+UseZGC`          | Very large heaps, pauses under a millisecond (generational by default since Java 23) |
| Parallel   | `-XX:+UseParallelGC`   | Batch jobs where total speed beats pause time                                        |
| Serial     | `-XX:+UseSerialGC`     | Small heaps and single-CPU containers                                                |
| Shenandoah | `-XX:+UseShenandoahGC` | Short pauses (in OpenJDK builds, not all vendors)                                    |

Collectors change fast. Check the [HotSpot GC tuning guide](https://docs.oracle.com/en/java/javase/21/gctuning/) for your version.

---

## Heap Size

```bash
java -Xms256m -Xmx1g -jar app.jar        # start at 256 MB, never above 1 GB
java -XX:MaxRAMPercentage=75 -jar app.jar   # in containers: 75% of the memory limit
```

When the heap is full and the GC cannot free enough, the JVM throws `OutOfMemoryError`.

```java
Runtime rt = Runtime.getRuntime();
long usedMb = (rt.totalMemory() - rt.freeMemory()) / 1_048_576;
long maxMb = rt.maxMemory() / 1_048_576;
```

---

## Memory Leaks Still Happen

The GC frees only unreachable objects. Common leaks keep objects reachable by mistake:

- Listeners that are never removed (see [Design Patterns](58_design_patterns.md))
- Static collections that only grow
- Caches with no size limit
- An inner class instance that keeps its outer object alive (see [Nested, Inner and Anonymous Classes](28_nested_inner_and_anonymous_classes.md))
- `ThreadLocal` values never removed on pooled threads
- Resources never closed (see [AutoCloseable](33_autocloseable.md))

---

## Reference Types

The `java.lang.ref` package has references that do not keep an object alive the normal way.

| Type                  | The GC frees the object      | Use for                         |
| --------------------- | ---------------------------- | ------------------------------- |
| Strong (normal)       | Never, while reachable       | Everything                      |
| `SoftReference<T>`    | Only when memory runs low    | Memory-sensitive caches         |
| `WeakReference<T>`    | At the next collection       | Data attached to another object |
| `PhantomReference<T>` | Already freed; only a notice | Cleanup (used inside `Cleaner`) |

```java
import java.lang.ref.WeakReference;

var weak = new WeakReference<>(new byte[1000]);

byte[] data = weak.get();     // null if the GC already freed it
if (data != null) {
    // still alive, and now held strongly by 'data'
}
```

`WeakHashMap` holds its keys weakly: an entry disappears once its key is unreachable elsewhere.

---

## Lazy Creation

Create something expensive only when first needed. The holder class idiom is thread-safe with no locks, because the JVM loads a class only once, on first use.

```java
class Reports {
    private static class Holder {
        static final Report INSTANCE = buildReport();   // runs on first access
    }

    static Report get() { return Holder.INSTANCE; }
}
```

Java 25 adds `StableValue` as a preview feature for the same job. Preview features need `--enable-preview` and can still change, so check the docs for your version first.

---

## Native Memory and Native Calls

The Foreign Function and Memory API (FFM, final in Java 22) replaces `Unsafe` and JNI for most work. An `Arena` controls how long off-heap memory lives, and closing it frees the memory.

```java
import java.lang.foreign.*;
import java.lang.invoke.MethodHandle;

public class Main {
    public static void main(String[] args) throws Throwable {
        try (Arena arena = Arena.ofConfined()) {
            MemorySegment ints = arena.allocate(ValueLayout.JAVA_INT, 4);  // 4 ints, off-heap
            ints.setAtIndex(ValueLayout.JAVA_INT, 0, 42);
            System.out.println(ints.getAtIndex(ValueLayout.JAVA_INT, 0));
        }   // memory freed here

        Linker linker = Linker.nativeLinker();
        MethodHandle strlen = linker.downcallHandle(
            linker.defaultLookup().find("strlen").orElseThrow(),
            FunctionDescriptor.of(ValueLayout.JAVA_LONG, ValueLayout.ADDRESS));

        try (Arena arena = Arena.ofConfined()) {
            MemorySegment text = arena.allocateFrom("hello");    // a C string
            System.out.println((long) strlen.invokeExact(text));
        }
    }
}
```

Expected output should be:

```
42
5
```

Run it with `java --enable-native-access=ALL-UNNAMED Main.java`. Without the flag it still runs, but prints a warning about a restricted method.

| Need                              | Use                                                              |
| --------------------------------- | ---------------------------------------------------------------- |
| Part of an array, no copy         | `ByteBuffer.wrap(array, offset, len)` or `MemorySegment.ofArray` |
| Off-heap memory                   | `Arena` and `MemorySegment`                                      |
| Call a C function                 | `Linker.nativeLinker().downcallHandle`                           |
| Generate bindings from a C header | The `jextract` tool                                              |

---

## Tools

| Tool                                              | Shows                            |
| ------------------------------------------------- | -------------------------------- |
| `jcmd <pid> GC.heap_info`                         | Heap use of a running JVM        |
| `-Xlog:gc`                                        | One log line per collection      |
| Java Flight Recorder (`-XX:StartFlightRecording`) | Allocations, GC pauses, hot code |
| VisualVM, JDK Mission Control                     | Graphs of the above              |

---

## Gotchas

- Do not call `System.gc()` in normal code; it is only a hint and usually slows things down
- Many short-lived objects in a hot loop cause GC pressure; reuse buffers and avoid boxing (`Integer` instead of `int`)
- `-Xmx` limits the heap only; threads, metaspace, and direct buffers use memory on top of it
- `finalize()` is deprecated for removal; use try-with-resources and `Cleaner`
- Measure with Flight Recorder or a profiler before tuning GC flags

---

## Examples

- [34-01](examples/34-01_native_memory_and_native_calls.java): Native memory and native calls

Run one with `java examples/34-01_native_memory_and_native_calls.java`. See [Examples](examples/README.md).
