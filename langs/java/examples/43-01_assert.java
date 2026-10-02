// Lesson 43: Debugging (../43_debugging.md)
// Assert
// Run: java 43-01_assert.java

public class Asserts {
    public static void main(String[] args) {
        int count = -1;
        assert count >= 0 : "count must not be negative";
        System.out.println("count is " + count);
    }
}
