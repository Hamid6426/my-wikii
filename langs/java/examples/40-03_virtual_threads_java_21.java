// Lesson 40: Modern Java Features (../40_modern_java_features.md)
// Virtual threads java 21
// Run: java 40-03_virtual_threads_java_21.java

import java.time.Duration;
import java.util.concurrent.Executors;
import java.util.concurrent.atomic.AtomicInteger;

public class Virtual {
    public static void main(String[] args) {
        var done = new AtomicInteger();

        try (var executor = Executors.newVirtualThreadPerTaskExecutor()) {
            for (int i = 0; i < 10_000; i++) {
                executor.submit(() -> {
                    Thread.sleep(Duration.ofMillis(100));   // blocking is cheap here
                    return done.incrementAndGet();
                });
            }
        }   // close() waits for every task

        System.out.println(done.get());
    }
}
