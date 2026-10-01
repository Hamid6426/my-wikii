// Lesson 06: Packages and Imports (../06_packages_and_imports.md)
// Module imports java 25
// Run: java 06-04_module_imports_java_25.java

import module java.base;              // every public class in java.base

public class Mod {
    public static void main(String[] args) {
        List<Integer> nums = List.of(3, 1, 2);            // java.util
        Path file = Path.of("notes.txt");                 // java.nio.file
        LocalDate day = LocalDate.of(2025, 1, 1);         // java.time
        System.out.println(nums + " " + file + " " + day);
    }
}
