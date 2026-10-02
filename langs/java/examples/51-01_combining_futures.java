// Lesson 51: CompletableFuture and Async Code (../51_completablefuture_and_async.md)
// Combining futures
// Run: java 51-01_combining_futures.java

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
