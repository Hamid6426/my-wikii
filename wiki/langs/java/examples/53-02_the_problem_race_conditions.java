// Lesson 53: Threading (../53_threading.md)
// The problem race conditions
// Run: java 53-02_the_problem_race_conditions.java

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
