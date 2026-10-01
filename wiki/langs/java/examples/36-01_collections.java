// Lesson 36: Collections (../36_collections.md)
// Collections
// Run: java 36-01_collections.java

import java.util.*;

public class Main {
    public static void main(String[] args) {
        List<String> words = List.of("pear", "apple", "pear", "fig", "apple", "pear");

        Map<String, Integer> counts = new TreeMap<>();   // sorted keys for stable output
        for (String w : words) {
            counts.merge(w, 1, Integer::sum);
        }
        System.out.println(counts);

        Set<String> unique = new LinkedHashSet<>(words); // keeps first-seen order
        System.out.println(unique);

        List<String> sorted = new ArrayList<>(unique);
        sorted.sort(null);
        System.out.println(sorted.getFirst() + " .. " + sorted.getLast());
    }
}
