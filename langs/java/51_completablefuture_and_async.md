# 51 - CompletableFuture and Async Code

## Why Async

**Async** code starts slow work (a network call, a file read, a big calculation) and carries on without waiting for it. The result arrives later.

Java has no `async` and `await` keywords. It has two tools instead:

| Tool                | Style                                            | Since   |
| ------------------- | ------------------------------------------------ | ------- |
| `CompletableFuture` | Chain steps that run when a result is ready      | Java 8  |
| Virtual threads     | Write normal blocking code on very cheap threads | Java 21 |

This lesson covers `CompletableFuture`. Virtual threads are in [Threading](53_threading.md). For new code that waits on I/O, plain code on virtual threads is often simpler.

---

## Future: The Basic Idea

A `Future<T>` is a handle to a result that is not ready yet. An `ExecutorService` (a pool of threads that runs tasks) returns one when you submit a task.

```java
try (ExecutorService pool = Executors.newFixedThreadPool(2)) {
    Future<Integer> future = pool.submit(() -> slowSquare(6));

    System.out.println(future.isDone());   // false, still running
    System.out.println(future.get());      // 36, blocks until ready
}
```

A plain `Future` cannot say "when done, do this next". `CompletableFuture` can.

---

## Starting Work

```java
CompletableFuture<Integer> f = CompletableFuture.supplyAsync(() -> slowSquare(4));   // returns a value
CompletableFuture<Void> r = CompletableFuture.runAsync(() -> sendEmail());           // no value

int result = f.join();   // 16, waits for the result
```

By default the work runs on the **common pool**, a shared set of threads sized to your CPU cores.

---

## Chaining Steps

Each method returns a new future that runs when the previous one finishes.

```java
CompletableFuture.supplyAsync(() -> 21)
    .thenApply(n -> n * 2)                    // transform the value
    .thenApply(n -> "Answer: " + n)
    .thenAccept(System.out::println)          // use the value, return nothing
    .thenRun(() -> System.out.println("done"))   // run after, no value
    .join();
// Answer: 42
// done
```

| Method               | Gets the value | Returns                   | Like stream |
| -------------------- | -------------- | ------------------------- | ----------- |
| `thenApply(fn)`      | Yes            | A new value               | `map`       |
| `thenAccept(action)` | Yes            | Nothing                   | `forEach`   |
| `thenRun(action)`    | No             | Nothing                   |             |
| `thenCompose(fn)`    | Yes            | Another future, flattened | `flatMap`   |

Use `thenCompose` when the next step is itself async:

```java
CompletableFuture<User> user = findUserId("alice")              // CompletableFuture<Integer>
    .thenCompose(id -> loadUser(id));                         // loadUser returns CompletableFuture<User>
```

With `thenApply` you would get a `CompletableFuture<CompletableFuture<User>>`.

---

## Combining Futures

```java
var a = CompletableFuture.supplyAsync(() -> slowSquare(2));
var b = CompletableFuture.supplyAsync(() -> slowSquare(3));

int sum = a.thenCombine(b, Integer::sum).join();   // 13, both ran at the same time
```

### Wait for All, or the First

```java
import java.util.List;
import java.util.concurrent.CompletableFuture;

public class AllOf {
    static int slowSquare(int n) {
        try {
            Thread.sleep(100);
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
        }
        return n * n;
    }

    public static void main(String[] args) {
        List<CompletableFuture<Integer>> futures = List.of(1, 2, 3, 4, 5).stream()
            .map(n -> CompletableFuture.supplyAsync(() -> slowSquare(n)))
            .toList();

        CompletableFuture.allOf(futures.toArray(new CompletableFuture[0])).join();

        List<Integer> results = futures.stream().map(CompletableFuture::join).toList();
        System.out.println(results);
    }
}
```

Expected output should be:

```
[1, 4, 9, 16, 25]
```

The five tasks run at the same time, so on a machine with several cores this takes about 100 ms, not 500 ms.

`allOf` returns `CompletableFuture<Void>`, so read each result from its own future afterwards. `anyOf(...)` finishes when the first one does and returns its value as an `Object`.

---

## Handling Errors

An exception inside a step skips the following steps until something handles it.

```java
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.CompletionException;

public class Errors {
    public static void main(String[] args) {
        CompletableFuture<Integer> failed = CompletableFuture.supplyAsync(() -> 10 / 0);

        int fallback = failed.exceptionally(e -> -1).join();
        System.out.println(fallback);

        String report = failed.handle((value, e) ->
            e == null ? "ok " + value : "failed: " + e.getCause().getMessage()).join();
        System.out.println(report);

        try {
            failed.join();
        } catch (CompletionException e) {
            System.out.println("join threw: " + e.getCause());
        }
    }
}
```

Expected output should be:

```
-1
failed: / by zero
join threw: java.lang.ArithmeticException: / by zero
```

| Method                       | Runs when    | Use for                                   |
| ---------------------------- | ------------ | ----------------------------------------- |
| `exceptionally(fn)`          | Failure only | Replace the error with a fallback value   |
| `handle((v, e) -> ..)`       | Always       | Turn success or failure into one result   |
| `whenComplete((v, e) -> ..)` | Always       | Log or clean up, keep the result as it is |

The error you receive is usually wrapped in a `CompletionException`. Call `getCause()` for the real one. See [Error Handling](42_error_handling.md).

---

## join vs get

| Method                     | Throws on failure                 | Checked exceptions          |
| -------------------------- | --------------------------------- | --------------------------- |
| `join()`                   | `CompletionException` (unchecked) | None                        |
| `get()`                    | `ExecutionException` (checked)    | Also `InterruptedException` |
| `get(5, TimeUnit.SECONDS)` | Also `TimeoutException`           | Yes                         |

Prefer `join()` inside lambdas and streams. Both block the calling thread, so call them once, at the end.

---

## Timeouts (Java 9)

```java
slow.completeOnTimeout("fallback", 100, TimeUnit.MILLISECONDS).join();   // "fallback" if too slow

slow.orTimeout(100, TimeUnit.MILLISECONDS).join();   // fails with a TimeoutException cause
```

---

## Choosing the Thread Pool

Every `...Async` method takes an optional `Executor` as its last argument. Pass one for blocking I/O work, so the small common pool is not tied up.

```java
try (ExecutorService pool = Executors.newVirtualThreadPerTaskExecutor()) {
    CompletableFuture<String> page = CompletableFuture.supplyAsync(() -> download(url), pool);
    System.out.println(page.join());
}
```

`thenApply` runs on whichever thread finished the previous step. `thenApplyAsync` hands the step to a pool instead.

---

## Completing a Future by Hand

`new CompletableFuture<T>()` creates a future that you finish yourself. Use it to turn a callback-style API into a future.

```java
static CompletableFuture<String> downloadAsync(String url) {
    var future = new CompletableFuture<String>();

    downloadWithCallback(url, (result, error) -> {     // an old callback API
        if (error != null) future.completeExceptionally(error);
        else future.complete(result);
    });

    return future;
}
```

| Method                                 | Effect                                                 |
| -------------------------------------- | ------------------------------------------------------ |
| `complete(value)`                      | Finishes with a value. Returns `false` if already done |
| `completeExceptionally(e)`             | Finishes as failed                                     |
| `cancel(true)`                         | Finishes as cancelled                                  |
| `CompletableFuture.completedFuture(v)` | A future that is already done                          |

---

## Structured Concurrency (Preview)

`StructuredTaskScope` treats a group of subtasks as one unit: if one fails, the others are cancelled, and none outlive the block. It is a preview feature in Java 21 to 25 and its API has changed between versions, so check the docs for your JDK before using it.

---

## Gotchas

- `cancel(true)` marks the future as cancelled but does not stop the code already running inside it
- Forgetting `join()` or a handler means an exception in a step is lost silently
- The common pool is small. Blocking I/O in it slows every other `supplyAsync` in the program. Pass your own executor
- Calling `join()` right after `supplyAsync` gives you no speedup. Start all the work first, then wait
- Common pool threads are daemon threads: the JVM can exit before they finish if `main` returns without waiting
- A long chain of `thenCompose` calls is hard to read. Plain blocking code on a virtual thread is often clearer

---

## Examples

- [51-01](examples/51-01_combining_futures.java): Combining futures
- [51-02](examples/51-02_handling_errors.java): Handling errors

Run one with `java examples/51-01_combining_futures.java`. See [Examples](examples/README.md).
