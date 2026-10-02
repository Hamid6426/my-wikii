// Lesson 51: CompletableFuture and Async Code (../51_completablefuture_and_async.md)
// Handling errors
// Run: java 51-02_handling_errors.java

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
