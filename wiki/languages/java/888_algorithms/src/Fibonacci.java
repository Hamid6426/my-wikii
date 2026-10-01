public class Fibonacci {
  public static void main(String[] args) {
    int n = 10; // Example: compute the 10th Fibonacci number
    long result = Fibonacci.fibonacci(n);
    System.out.println("The " + n + "th Fibonacci number is: " + result);
  }

  /**
   * Computes the nth Fibonacci number iteratively.
   * The Fibonacci sequence is a series of numbers in which each number is the sum
   * of the two preceding ones, usually starting with 0 and 1.
   * Time complexity is O(n), and space complexity is O(1).
   *
   * @param n The position in the Fibonacci sequence (0-indexed).
   * @return The nth Fibonacci number.
   * @throws IllegalArgumentException if n is negative.
   */
  public static long fibonacci(int n) {
    if (n < 0) {
      throw new IllegalArgumentException("Position must be non-negative.");
    }
    if (n == 0)
      return 0;
    if (n == 1)
      return 1;

    long prev1 = 0;
    long prev2 = 1;
    long current = 0;

    for (int i = 2; i <= n; i++) {
      current = prev1 + prev2;
      prev1 = prev2;
      prev2 = current;
    }

    return current;
  }
}
