// Lesson 20: Strings (../20_strings.md)
// Common string methods
// Run: java 20-01_common_string_methods.java

import java.util.Arrays;

public class StringTools {
    public static void main(String[] args) {
        String s = "  Hello, World!  ";

        System.out.println(s.length());
        System.out.println("[" + s.strip() + "]");
        System.out.println("[" + s.stripLeading() + "]");
        System.out.println(s.toUpperCase());
        System.out.println(s.contains("World"));
        System.out.println(s.strip().startsWith("Hello"));
        System.out.println(s.replace("World", "Java"));
        System.out.println(s.strip().substring(7, 12));
        System.out.println(s.indexOf("World"));
        System.out.println(s.charAt(2));
        System.out.println(Arrays.toString(s.strip().split(", ")));
        System.out.println("ab".repeat(3));
        System.out.println(String.join("-", "a", "b", "c"));
    }
}
