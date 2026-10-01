// Lesson 06: Packages and Imports (../06_packages_and_imports.md)
// Import static
// Run: java 06-03_import_static.java

import static java.lang.Math.PI;
import static java.lang.Math.sqrt;

public class Static {
    public static void main(String[] args) {
        System.out.println(sqrt(16));             // instead of Math.sqrt(16)
        System.out.printf("%.4f%n", PI);          // instead of Math.PI
    }
}
