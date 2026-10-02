# 53 - Threading

## Threads

A **thread** is one path of execution. A program with several threads can do several things at once. Java has two kinds.

| Kind            | What it is                                                   | Cost                      |
| --------------- | ------------------------------------------------------------ | ------------------------- |
| Platform thread | Wraps one operating system thread                            | About 1 MB of memory each |
| Virtual thread  | Managed by the JVM, runs on a few platform threads (Java 21) | A few hundred bytes each  |

Use virtual threads for work that waits (network, files, databases). Use a small pool of platform threads for heavy CPU work. This lesson also covers what goes wrong when threads share data, and how to fix it.

---

## Creating a Thread

```java
Thread t = new Thread(() -> System.out.println("Hello from " + Thread.currentThread().getName()));
t.setName("worker");
t.start();      // run it on a new thread
t.join();       // wait for it to finish
// Hello from worker

Thread p = Thread.ofPlatform().name("plat").start(() -> doWork());   // builder style (Java 21)
Thread v = Thread.ofVirtual().start(() -> doWork());                 // a virtual thread
Thread.startVirtualThread(() -> doWork());                          // shortcut
```

Calling `run()` instead of `start()` runs the code on the current thread. That is a common mistake.

---

## ExecutorService

In real code you rarely create threads by hand. An **ExecutorService** runs tasks for you and gives back a `Future` for each result.

```java
try (ExecutorService pool = Executors.newFixedThreadPool(4)) {
    Future<Integer> answer = pool.submit(() -> 2 + 2);   // a Callable: returns a value
    pool.submit(() -> System.out.println("hi"));         // a Runnable: returns nothing

    System.out.println(answer.get());                    // 4, waits for the result

    List<Future<String>> all = pool.invokeAll(List.of(() -> "x", () -> "y"));   // run many, wait for all
}   // close() waits for every task, then shuts down (Java 19)
```

| Factory                                       | Gives                                         |
| --------------------------------------------- | --------------------------------------------- |
| `Executors.newVirtualThreadPerTaskExecutor()` | A new virtual thread per task                 |
| `Executors.newFixedThreadPool(n)`             | `n` platform threads that take turns on tasks |
| `Executors.newSingleThreadExecutor()`         | One thread: tasks run one by one, in order    |
| `Executors.newScheduledThreadPool(n)`         | Runs tasks after a delay or on a timer        |

Before Java 19, call `pool.shutdown()` and `pool.awaitTermination(...)` yourself in a `finally` block.

---

## Virtual Threads

One virtual thread per task, even for 10,000 tasks. While a virtual thread waits, the platform thread under it runs another one.

```java
import java.time.Duration;
import java.util.concurrent.Executors;
import java.util.concurrent.atomic.AtomicInteger;

public class VirtualThreads {
    public static void main(String[] args) {
        var finished = new AtomicInteger();
        long start = System.currentTimeMillis();

        try (var executor = Executors.newVirtualThreadPerTaskExecutor()) {
            for (int i = 0; i < 10_000; i++) {
                executor.submit(() -> {
                    Thread.sleep(Duration.ofSeconds(1));   // pretend to wait for a network call
                    return finished.incrementAndGet();
                });
            }
        }

        long seconds = (System.currentTimeMillis() - start) / 1000;
        System.out.println(finished.get() + " tasks in about " + seconds + " second(s)");
    }
}
```

Expected output should be:

```
10000 tasks in about 1 second(s)
```

With `newFixedThreadPool(100)` the same work takes about 100 seconds.

- Do not pool virtual threads. Create a new one per task
- They do not make CPU-heavy work faster. They help when tasks spend their time waiting
- Before Java 24, waiting inside a `synchronized` block **pinned** the virtual thread to its platform thread, which blocked others. Java 24 fixed this. On Java 21, prefer `ReentrantLock` around waits

---

## The Problem: Race Conditions

A **race condition** is a bug where the result depends on how threads happen to interleave.

```java
import java.util.concurrent.Executors;

public class Race {
    static int counter = 0;

    public static void main(String[] args) {
        try (var pool = Executors.newFixedThreadPool(4)) {
            for (int i = 0; i < 100_000; i++) {
                pool.submit(() -> counter++);
            }
        }
        System.out.println(counter);
    }
}
```

The output differs each run, for example:

```
99034
```

`counter++` is three steps (read, add, write). Two threads can read the same value, and one update is lost.

---

## synchronized

Only one thread at a time can run a `synchronized` block for the same lock object.

```java
private final Object lock = new Object();
private int counter;

public void increment() {
    synchronized (lock) {
        counter++;
    }
}

public synchronized int get() {   // a synchronized method locks on `this`
    return counter;
}
```

With this fix, the program above prints `100000` every time.

---

## Atomic Variables

Fast, lock-free operations for single values, in `java.util.concurrent.atomic`.

```java
var counter = new AtomicInteger();

counter.incrementAndGet();              // 1
counter.addAndGet(5);                   // 6
counter.compareAndSet(6, 10);           // true: set to 10 only if it is still 6
counter.updateAndGet(n -> n * 2);       // 20
```

`AtomicLong`, `AtomicBoolean`, and `AtomicReference<T>` work the same way. For a counter that many threads update very often, `LongAdder` is faster.

---

## ReentrantLock

A lock object with more options than `synchronized`.

```java
private final ReentrantLock lock = new ReentrantLock();

public void update() {
    lock.lock();
    try {
        // change shared state
    } finally {
        lock.unlock();                  // always unlock in finally
    }
}

if (lock.tryLock(1, TimeUnit.SECONDS)) {   // give up after a second
    try { /* ... */ } finally { lock.unlock(); }
}
```

`ReadWriteLock` lets many readers in at once but only one writer.

---

## Thread-Safe Collections

Normal `ArrayList` and `HashMap` break when several threads change them. Use these from `java.util.concurrent`:

```java
var words = new ConcurrentHashMap<String, Integer>();
words.merge("apple", 1, Integer::sum);        // add 1, safely, even from many threads
words.computeIfAbsent("pear", k -> 0);

var queue = new ConcurrentLinkedQueue<Integer>();
var list = new CopyOnWriteArrayList<String>();   // for lists read often, changed rarely
```

`Collections.synchronizedList(list)` also works, but you must still lock the list yourself while looping over it. See [Collections](36_collections.md).

---

## Producer and Consumer: BlockingQueue

A **BlockingQueue** is a thread-safe queue. `put` waits while the queue is full, and `take` waits while it is empty.

```java
import java.util.concurrent.ArrayBlockingQueue;
import java.util.concurrent.BlockingQueue;

public class ProducerConsumer {
    public static void main(String[] args) throws InterruptedException {
        BlockingQueue<Integer> queue = new ArrayBlockingQueue<>(2);   // holds at most 2

        Thread producer = Thread.ofVirtual().start(() -> {
            try {
                for (int i = 1; i <= 5; i++) queue.put(i);   // waits while the queue is full
                queue.put(-1);                              // a signal that means "done"
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            }
        });

        int sum = 0;
        while (true) {
            int item = queue.take();                        // waits while the queue is empty
            if (item == -1) break;
            sum += item;
        }
        producer.join();
        System.out.println("sum " + sum);
    }
}
```

Expected output should be:

```
sum 15
```

A bounded queue such as `ArrayBlockingQueue(2)` slows a fast producer down, so it cannot fill memory. `LinkedBlockingQueue` with no size has no limit.

---

## Coordination Helpers

| Class            | Does                                                           |
| ---------------- | -------------------------------------------------------------- |
| `CountDownLatch` | Wait until a count reaches zero, such as "all 3 workers ready" |
| `Semaphore`      | Allow at most `n` threads into a section at once               |
| `CyclicBarrier`  | Make a group of threads wait for each other, then go together  |
| `Phaser`         | Like a barrier, but threads can join and leave                 |

```java
var latch = new CountDownLatch(3);
for (int i = 0; i < 3; i++) Thread.startVirtualThread(latch::countDown);
latch.await();                       // continues once countDown() ran 3 times

var limit = new Semaphore(3);        // 3 at a time, such as connections to a server
limit.acquire();
try {
    callServer();
} finally {
    limit.release();
}
```

---

## volatile

Each thread may keep its own cached copy of a field. A loop that waits for another thread to change a flag can then read the old value forever. A `volatile` field is always read from and written to main memory.

```java
private volatile boolean stop;

public void run() {
    while (!stop) {
        doWork();
    }
}

public void requestStop() { stop = true; }   // the loop sees this and ends
```

`volatile` makes one read or write visible. It does not make `x++` safe. Use an atomic for counters and a lock for several steps that must happen together.

---

## Scheduled Tasks

```java
try (ScheduledExecutorService timer = Executors.newSingleThreadScheduledExecutor()) {
    timer.schedule(() -> System.out.println("once, after 1 s"), 1, TimeUnit.SECONDS);
    timer.scheduleAtFixedRate(() -> System.out.println("tick"), 0, 500, TimeUnit.MILLISECONDS);
    Thread.sleep(2000);
    timer.shutdownNow();             // stop the repeating task
}
```

`scheduleAtFixedRate` keeps a steady beat. `scheduleWithFixedDelay` waits a fixed time after each run ends. Avoid the old `java.util.Timer`.

---

## Stopping a Thread: Interrupts

Java cannot kill a thread safely. You **interrupt** it: a polite request to stop. Waiting methods such as `sleep`, `join`, and `take` then throw `InterruptedException`.

```java
Thread worker = Thread.ofVirtual().start(() -> {
    while (!Thread.currentThread().isInterrupted()) {
        try {
            Thread.sleep(100);
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();   // set the flag again, then leave
            return;
        }
    }
});

worker.interrupt();
```

Throwing `InterruptedException` clears the interrupt flag. Set it again so code further up also sees the stop request.

---

## Per-Thread Data: ThreadLocal and ScopedValue

`ThreadLocal<T>` gives each thread its own copy of a value. `ScopedValue<T>` (final in Java 25) shares a read-only value with everything called inside a block, and is cheaper with many virtual threads.

```java
static final ScopedValue<String> USER = ScopedValue.newInstance();

ScopedValue.where(USER, "alice").run(() -> handleRequest());   // inside, USER.get() is "alice"
USER.isBound();                                                // false outside the block
```

---

## Gotchas

- Never call `run()` when you mean `start()`
- Shared data needs protection for reads too, not only writes
- Taking two locks in different orders in different places causes a **deadlock**: each thread waits for the other forever
- Keep the work inside a lock short, and do not call unknown code while holding it
- Do not lock on a `String`, a boxed number, or a public object. Lock on a `private final Object`
- An exception in a task submitted to an executor is stored in its `Future`. If you never call `get()`, you never see it
- Do not swallow `InterruptedException`. Restore the flag or rethrow it
- Prefer immutable data, such as records (see [Records and Equality](25_records_and_equality.md)), and message passing over shared mutable state

---

## Examples

- [53-01](examples/53-01_virtual_threads.java): Virtual threads
- [53-02](examples/53-02_the_problem_race_conditions.java): The problem race conditions
- [53-03](examples/53-03_producer_and_consumer_blockingqueue.java): Producer and consumer blockingqueue

Run one with `java examples/53-01_virtual_threads.java`. See [Examples](examples/README.md).
