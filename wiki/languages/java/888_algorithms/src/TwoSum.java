public class TwoSum {
  public static void main(String[] args) {
    int[] nums = { 2, 7, 11, 15 };
    int target = 9;
    int[] result = twoSum(nums, target);
    if (result != null) {
      System.out.println("Indices: " + result[0] + ", " + result[1]);
    } else {
      System.out.println("No two sum solution found.");
    }
  }

  /**
   * Finds two numbers in the array that add up to the target and returns their
   * indices.
   *
   * @param nums   The array of integers.
   * @param target The target sum.
   * @return An array containing the indices of the two numbers that add up to
   *         the target, or null if no such pair exists.
   */
  public static int[] twoSum(int[] nums, int target) {
    for (int i = 0; i < nums.length; i++) {
      for (int j = i + 1; j < nums.length; j++) {
        if (nums[i] + nums[j] == target) {
          return new int[] { i, j };
        }
      }
    }
    return null; // No solution found
  }
}
