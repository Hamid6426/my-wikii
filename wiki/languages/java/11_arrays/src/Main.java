public class Main {
  // Arrays are used to store multiple values in a single variable, instead of
  // declaring separate variables for each value. An array is a data structure
  // that holds a fixed number of values of the same type. The length of an array
  // is established when the array is created and cannot be changed.
  // Each item in an array is called an element, and each element is accessed by
  // its numerical index. The index of the first element is 0, the second element
  // is 1, and so on.

  // we will do multi dimensional arrays in the next project, but for now we will
  // just do one dimensional

  public static void main(String[] args) {
    int[] nums = { 1, 2, 3, 4, 5 };
    int[] numbers = { 1, 2, 3, 3, 4, 5, 5, 5 };

    // length of the array is accessed using the length property
    int arrayLength = nums.length;
    System.out.println("Length of the array:" + arrayLength);

    // value of the first item of the array is accessed using index 0
    int left = nums[0];
    System.out.println("Value of the first item of the array:" + left);

    // value of the last item of the array is accessed using index length - 1
    int right = nums[arrayLength - 1];
    System.out.println("Value of the last item of the array:" + right);

    // mode of the array is the value that appears most frequently in the array. In
    // this example, there is no mode because all values appear only once. If there
    // were a mode, we would need to count the frequency of each value and find the
    // one that appears most frequently.
    // we will use a new array numbers for mode
    int mode = 0;
    int maxCount = 0;
    for (int i = 0; i < numbers.length; i++) {
      int count = 0;
      for (int j = 0; j < numbers.length; j++) {
        if (numbers[j] == numbers[i]) {
          count++;
        }
      }
      if (count > maxCount) {
        maxCount = count;
        mode = numbers[i];
      }
    }
    if (maxCount > 1) {
      System.out.println("Mode of the array:" + mode);
    } else {
      System.out.println("There is no mode in the array.");
    }

    // to calculate the mean of the array, we need to sum all the elements and then
    // divide by the length of the array
    int sum = 0;
    for (int i = 0; i < arrayLength; i++) {
      sum += nums[i];
    }
    double mean = (double) sum / arrayLength;
    System.out.println("Mean of the array:" + mean);

    // to calculate the median of the array, we need to sort the array and then find
    // the middle element. If the length of the array is even, we need to find the
    // average of the two middle elements.
    int[] sortedNums = nums.clone();
    java.util.Arrays.sort(sortedNums);
    double medianSorted;
    if (arrayLength % 2 == 0) {
      medianSorted = (sortedNums[arrayLength / 2 - 1] + sortedNums[arrayLength / 2]) / 2.0;
    } else {
      medianSorted = sortedNums[arrayLength / 2];
    }
    System.out.println("Median of the sorted array:" + medianSorted);
  }
}