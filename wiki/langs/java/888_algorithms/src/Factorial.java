public class Factorial {
  public static void main(String[] args) {
    int number = 5; // Example input
    long result = factorial(number);
    System.out.println("Factorial of " + number + " is: " + result);
  }

  /**
   * Calculates the factorial of a given non-negative integer.
   * Factorial of n (n!) is the product of all positive integers less than or
   * equal to n.
   * Time complexity is O(n), and space complexity is O(n) due to the call stack.
   *
   * @param n The non-negative integer.
   * @return The factorial of n. Returns 1 if n is 0 or 1.
   * @throws IllegalArgumentException if n is negative.
   */
  public static long factorial(int n) {
    if (n < 0) {
      throw new IllegalArgumentException("Number must be non-negative.");
    }
    if (n == 0 || n == 1) {
      return 1;
    }
    return n * factorial(n - 1);
  }
}
