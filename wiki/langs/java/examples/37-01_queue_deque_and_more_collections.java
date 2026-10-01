// Lesson 37: Queue, Deque and More Collections (../37_queue_deque_and_more_collections.md)
// Queue, Deque and More Collections
// Run: java 37-01_queue_deque_and_more_collections.java

import java.util.*;

public class Main {
    public static void main(String[] args) {
        Deque<String> stack = new ArrayDeque<>();
        for (String s : List.of("a", "b", "c")) stack.push(s);
        System.out.println("stack pop: " + stack.pop());

        Queue<String> queue = new ArrayDeque<>(List.of("a", "b", "c"));
        System.out.println("queue poll: " + queue.poll());

        PriorityQueue<Integer> pq = new PriorityQueue<>(List.of(5, 1, 4, 2));
        StringBuilder order = new StringBuilder();
        while (!pq.isEmpty()) order.append(pq.poll()).append(' ');
        System.out.println("priority: " + order.toString().trim());

        TreeMap<Integer, String> grades = new TreeMap<>(Map.of(90, "A", 80, "B", 70, "C"));
        System.out.println("85 gets " + grades.floorEntry(85).getValue());

        LinkedHashMap<String, Integer> lru = new LinkedHashMap<>(16, 0.75f, true) {
            @Override
            protected boolean removeEldestEntry(Map.Entry<String, Integer> e) {
                return size() > 2;
            }
        };
        lru.put("x", 1);
        lru.put("y", 2);
        lru.get("x");              // x is now the most recent
        lru.put("z", 3);           // y is dropped
        System.out.println("lru: " + lru.keySet());
    }
}
