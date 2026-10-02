// Lesson 53: Threading (../53_threading.md)
// Virtual threads
// Run: java 53-01_virtual_threads.java

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
