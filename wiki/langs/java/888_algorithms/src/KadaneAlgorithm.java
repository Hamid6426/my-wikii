public class KadaneAlgorithm {
  public static void main(String[] args) {
    int[] arr = { -2, 1, -3, 4, -1, 2, 1, -5, 4 };
    int maxSum = kadane(arr);
    System.out.println("Maximum subarray sum: " + maxSum);
  }

  /**
   * Implements Kadane's algorithm to find the maximum sum of a contiguous
   * subarray.
   *
   * @param arr The input array of integers.
   * @return The maximum subarray sum.
   */
  public static int kadane(int[] arr) {
    int maxCurrent = arr[0];
    int maxGlobal = arr[0];
    for (int i = 1; i < arr.length; i++) {
      maxCurrent = Math.max(arr[i], maxCurrent + arr[i]);
      if (maxCurrent > maxGlobal) {
        maxGlobal = maxCurrent;
      }
    }
    return maxGlobal;
  }
}
