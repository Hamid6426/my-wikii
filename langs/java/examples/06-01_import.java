// Lesson 06: Packages and Imports (../06_packages_and_imports.md)
// Import
// Run: java 06-01_import.java

import java.time.LocalDate;          // one class
import java.util.*;                  // every class in java.util

public class Imports {
    public static void main(String[] args) {
        List<String> names = new ArrayList<>(List.of("Bob", "Alice"));
        Collections.sort(names);
        System.out.println(names);

        LocalDate date = LocalDate.of(2025, 9, 16);
        System.out.println(date.getYear());
    }
}
