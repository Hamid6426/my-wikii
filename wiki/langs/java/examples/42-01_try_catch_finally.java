// Lesson 42: Error Handling (../42_error_handling.md)
// Try catch finally
// Run: java 42-01_try_catch_finally.java

public class TryCatch {
    public static void main(String[] args) {
        try {
            int[] items = {1, 2, 3};
            System.out.println(items[5]);              // throws ArrayIndexOutOfBoundsException
        } catch (ArrayIndexOutOfBoundsException e) {
            System.out.println("Bad index: " + e.getMessage());
        } catch (Exception e) {
            System.out.println("Unexpected: " + e.getMessage());
        } finally {
            System.out.println("Always runs: cleanup here");
        }
    }
}
