// Lesson 10: Type Conversion (../10_type_conversion.md)
// A safe parse helper
// Run: java 10-01_a_safe_parse_helper.java

import java.util.OptionalInt;

public class SafeParse {
    static OptionalInt tryParseInt(String text) {
        try {
            return OptionalInt.of(Integer.parseInt(text.trim()));
        } catch (NumberFormatException e) {
            return OptionalInt.empty();
        }
    }

    public static void main(String[] args) {
        System.out.println(tryParseInt("12"));
        System.out.println(tryParseInt("abc"));
        System.out.println(tryParseInt("abc").orElse(0));
    }
}
