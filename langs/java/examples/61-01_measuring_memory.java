// Lesson 61: Performance Basics (../61_performance_basics.md)
// Measuring memory
// Run: java 61-01_measuring_memory.java

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
