public class BubbleSort {
  public static void main(String[] args) {
    int[] arr = { 5, 2, 9, 1, 5, 6 };
    bubbleSort(arr);
    System.out.println("Sorted array: ");
    for (int num : arr) {
      System.out.print(num + " ");
    }
  }

  /**
   * Sorts an array of integers using the bubble sort algorithm.
   *
   * @param arr The array to be sorted.
   */
  public static void bubbleSort(int[] arr) {
    int n = arr.length;
    boolean swapped;
    for (int i = 0; i < n - 1; i++) {
      swapped = false;
      for (int j = 0; j < n - i - 1; j++) {
        if (arr[j] > arr[j + 1]) {
          // Swap arr[j] and arr[j+1]
          int temp = arr[j];
          arr[j] = arr[j + 1];
          arr[j + 1] = temp;
          swapped = true;
        }
      }
      // If no two elements were swapped in the inner loop, then the array is already
      // sorted
      if (!swapped) {
        break;
      }
    }
  }
}
