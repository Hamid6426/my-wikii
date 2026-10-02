# 61 - Performance Basics

## The Rules

1. **Measure first.** Programs are rarely slow where you expect
2. **Fix the biggest cost first.** Making a 1 ms step twice as fast does nothing if another step takes 2 seconds
3. **Measure again** to see that the change helped
4. **Keep the code clear.** Slower, readable code beats fast code nobody understands, unless a measurement says otherwise

---

## How the JVM Runs Your Code

The JVM (Java Virtual Machine) first interprets bytecode, then the JIT (Just-In-Time compiler) turns hot methods into optimized machine code while the program runs. A hot method is one that runs often.

| Effect        | What it means for timing                                           |
| ------------- | ------------------------------------------------------------------ |
| Warm-up       | The first calls are slow. Code gets faster after thousands of runs |
| Dead code     | If a result is never used, the JIT may remove the work entirely    |
| GC pauses     | The garbage collector can run in the middle of your measurement    |
| Class loading | The first use of a class pays a one-time cost                      |

There is no "Debug build" in Java. The same bytecode is optimized at run time, so always warm up before you time.

---

## Measuring with System.nanoTime

```java
long start = System.nanoTime();
doWork();
long micros = (System.nanoTime() - start) / 1_000;

System.out.println(micros + " us");
```

`System.nanoTime()` is for measuring elapsed time. `System.currentTimeMillis()` is wall-clock time and can jump when the clock is adjusted.

A small helper that avoids the common mistakes:

```java
static long sink;   // results go here so the JIT cannot drop the work

static long timeMicros(Runnable action, int runs) {
    action.run();                                // warm up: first run pays one-time costs
    long best = Long.MAX_VALUE;

    for (int i = 0; i < runs; i++) {
        long start = System.nanoTime();
        action.run();
        best = Math.min(best, System.nanoTime() - start);
    }

    return best / 1_000;
}
```

| Mistake                       | Why it misleads                                                        |
| ----------------------------- | ---------------------------------------------------------------------- |
| Timing one run                | The first call includes interpreting, JIT compiling, and class loading |
| Measuring something too small | A few nanoseconds is below what the clock can show; loop it many times |
| Result never used             | The JIT may remove code whose result is ignored                        |
| Other programs running        | Noise. Close them and repeat                                           |

For precise results use JMH (Java Microbenchmark Harness), the OpenJDK benchmarking tool. See [Common Libraries](66_libraries.md).

---

## Measuring Memory

Allocations are often the real cost, because the garbage collector has to clean them up. OpenJDK can count the bytes one thread allocated.

```java
import java.lang.management.ManagementFactory;
import java.util.ArrayList;
import java.util.List;

public class Alloc {
    public static void main(String[] args) {
        var threads = (com.sun.management.ThreadMXBean) ManagementFactory.getThreadMXBean();

        fill(new ArrayList<>());                     // warm up once
        long before = threads.getCurrentThreadAllocatedBytes();
        fill(new ArrayList<>());
        long middle = threads.getCurrentThreadAllocatedBytes();
        fill(new ArrayList<>(1_000_000));
        long after = threads.getCurrentThreadAllocatedBytes();

        System.out.println("no capacity:     " + (middle - before) / 1_000_000 + " MB");
        System.out.println("preset capacity: " + (after - middle) / 1_000_000 + " MB");
    }

    static void fill(List<Integer> list) {
        for (int i = 0; i < 1_000_000; i++) {
            list.add(i % 100);                       // small values are cached, so no new Integer
        }
    }
}
```

Expected output should be:

```
no capacity:     14 MB
preset capacity: 4 MB
```

`ArrayList` grows its inner array by half each time it fills, copying everything and leaving the old array as garbage. A preset capacity makes one array of the right size. See [Memory and Garbage Collection](34_memory_and_garbage_collection.md).

---

## Pick the Right Data Structure

The biggest wins usually come from this. One measured run (1,000 lookups in 10,000 items, warmed up):

| Operation              | ArrayList.contains | HashSet.contains |
| ---------------------- | ------------------ | ---------------- |
| Time for 1,000 lookups | about 2,500 us     | about 100 us     |

`ArrayList.contains` checks items one by one (O(n): time grows with the size). `HashSet` jumps straight to the item (O(1): time stays the same). Your numbers will differ, but the gap stays large. See [Collections](36_collections.md).

| Need                       | Use                  |
| -------------------------- | -------------------- |
| Fast "is this in the set?" | `HashSet<E>`         |
| Fast lookup by key         | `HashMap<K, V>`      |
| Process in arrival order   | `ArrayDeque<E>`      |
| Read by index, add at end  | `ArrayList<E>`       |
| Kept sorted                | `TreeMap`, `TreeSet` |

`LinkedList` is rarely faster than `ArrayList`, even for inserts. Measure before you pick it.

---

## Strings in Loops

```java
// Slow: every += creates a new String and copies the old one
String s = "";
for (int i = 0; i < 20_000; i++) s += "x";

// Fast: one buffer that grows
var sb = new StringBuilder();
for (int i = 0; i < 20_000; i++) sb.append('x');
String result = sb.toString();
```

In one measured run the `+=` loop took about 27 ms and `StringBuilder` about 0.2 ms, more than 100 times faster. A single `a + b + c` outside a loop is fine; the compiler already optimizes it. See [Strings](20_strings.md).

---

## Avoid Boxing

Boxing wraps a primitive such as `int` in an object such as `Integer`. Collections can only hold objects, so `List<Integer>` boxes every number.

```java
List<Integer> boxed = new ArrayList<>();   // one Integer object per number
for (int i = 0; i < 1_000_000; i++) boxed.add(i);

int[] plain = new int[1_000_000];          // numbers stored directly
for (int i = 0; i < plain.length; i++) plain[i] = i;
```

Filling and summing one million numbers took about 13 ms with `List<Integer>` and about 1 ms with `int[]` in one run. Storing 100,000 numbers above 127 allocated about 2 MB as `Integer` objects and 0.4 MB as an `int[]`.

| Instead of                        | Use                                                   |
| --------------------------------- | ----------------------------------------------------- |
| `List<Integer>`                   | `int[]` when the size is known                        |
| `Stream<Integer>`                 | `IntStream`, `LongStream`, `DoubleStream`             |
| `Map<Integer, ...>` in a hot path | An array indexed by the number, if the range is small |

See [Primitive Data Types](08_primitive_data_types.md) and [Generics](54_generics.md).

---

## Streams and Loops

Streams are clear and fast enough for most code. In tight loops over large data, a plain loop avoids creating lambdas and stream objects.

```java
long count = Arrays.stream(nums).filter(n -> n % 2 == 0).count();   // clear

int count2 = 0;                                                      // faster in a hot path
for (int n : nums) {
    if (n % 2 == 0) count2++;
}
```

On 100,000 numbers the loop was about two to three times faster after warm-up. Change to a loop only when a measurement shows the stream is the problem. `parallel()` helps only for large, CPU-heavy work, and can make small jobs slower. See [Streams](41_streams.md).

---

## Threads and Waiting

Virtual threads (Java 21) do not make code faster. They make waiting cheap, so one program can handle many slow network or disk calls at once.

| Work type                          | Helps                                     |
| ---------------------------------- | ----------------------------------------- |
| Waiting on network, disk, database | Virtual threads, `CompletableFuture`      |
| Heavy CPU work                     | A pool with about one thread per CPU core |

See [Threading](53_threading.md).

---

## JVM Options and Tools

```bash
java -Xmx2g -jar app.jar                         # maximum heap size
java -XX:+UseZGC -jar app.jar                    # low-pause garbage collector
java -XX:StartFlightRecording:filename=rec.jfr -jar app.jar   # record a profile
jcmd <pid> GC.heap_info                          # ask a running JVM about its heap
```

| Tool                      | Use                                                                 |
| ------------------------- | ------------------------------------------------------------------- |
| JDK Flight Recorder (JFR) | Records CPU, memory, locks, and GC with very low overhead. Built in |
| JDK Mission Control (JMC) | Opens `.jfr` files and shows where time goes. Separate download     |
| `jcmd`, `jstat`           | Command-line tools in the JDK to inspect a running JVM              |
| VisualVM, async-profiler  | Free profilers. A profiler shows which methods use the most time    |

The default collector, G1, suits most apps. Change it only when a measurement shows long pauses.

---

## Do Less Work

The fastest code is code that does not run.

| Technique         | Example                                                 |
| ----------------- | ------------------------------------------------------- |
| Stop early        | `anyMatch` instead of `count() > 0`; `break` when found |
| Cache results     | Keep a computed value in a field or a `Map`             |
| Avoid repeats     | Move work that does not change out of a loop            |
| Fewer round trips | One call that returns 100 items, not 100 calls          |
| Reuse objects     | Compile a `Pattern` once, not on every call             |

See [Regular Expressions](49_regular_expressions.md) for `Pattern.compile`.

---

## Gotchas

- Timings without warm-up measure the interpreter, not your code. Run the code many times first
- Results change between runs and machines. Compare two versions on the same machine, and repeat
- Do not optimize code that runs once. Optimize code that runs a million times
- `System.gc()` is only a hint and rarely helps. Do not call it in normal code
- Faster code that is wrong is worthless. Keep your tests, and run them after every change (see [Testing](59_testing.md))
- Network, disk, and database calls usually cost far more than any Java code. Reduce those first
- `Integer` values from -128 to 127 are cached, so small numbers box for free. That can hide boxing costs in a quick test

---

## Examples

- [61-01](examples/61-01_measuring_memory.java): Measuring memory

Run one with `java examples/61-01_measuring_memory.java`. See [Examples](examples/README.md).
