public class ContainDuplicate {
  public static void main(String[] args) {
    int[] nums = { 1, 2, 3, 1 };
    boolean result = containsDuplicate(nums);
    System.out.println("Contains duplicate: " + result);
  }

  /**
   * Checks if the array contains any duplicates.
   *
   * @param nums The array of integers to check for duplicates.
   * @return True if there are duplicates, false otherwise.
   */
  public static boolean containsDuplicate(int[] nums) {
    for (int i = 0; i < nums.length; i++) {
      for (int j = i + 1; j < nums.length; j++) {
        if (nums[i] == nums[j]) {
          return true;
        }
      }
    }
    return false; // No duplicates found
  }
}
