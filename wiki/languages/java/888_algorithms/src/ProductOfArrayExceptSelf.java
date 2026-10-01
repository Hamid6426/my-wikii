public class ProductOfArrayExceptSelf {
  public static void main(String[] args) {
    int[] nums = { 1, 2, 3, 4 };
    int[] result = productExceptSelf(nums);
    System.out.print("Product of array except self: ");
    for (int num : result) {
      System.out.print(num + " ");
    }
  }

  /**
   * Computes the product of all elements in the array except for the current
   * element.
   *
   * @param nums The input array of integers.
   * @return An array where each element is the product of all other elements in
   *         the input array.
   */
  public static int[] productExceptSelf(int[] nums) {
    int n = nums.length;
    int[] result = new int[n];
    int leftProduct = 1;

    // Calculate left products
    for (int i = 0; i < n; i++) {
      result[i] = leftProduct;
      leftProduct *= nums[i];
    }

    int rightProduct = 1;
    // Calculate right products and final result
    for (int i = n - 1; i >= 0; i--) {
      result[i] *= rightProduct;
      rightProduct *= nums[i];
    }

    return result;
  }
}