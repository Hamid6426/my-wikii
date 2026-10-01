
/**
 * Core Methods for arrays.
 * This class provides implementations of common array operations including
 * Access
 * - arr[i]
 * - first
 * - last
 * Traversal
 * - forward
 * - backward
 * - for-each
 * - iterator
 * - recursive
 * - stream
 * - parallel stream
 * Insertion
 * - append
 * - insert at index
 * - prepend
 * Deletion
 * - pop (end)
 * - remove at index
 * - remove by value
 * Search
 * - linear search
 * - binary search (sorted)
 * Update
 * - overwrite value at index
 * Sorting
 * - built-in sort
 * - custom comparator
 * Reverse
 * - in-place reverse
 * - two-pointer swap
 * Slicing
 * - subarray extraction
 * Merge / Concatenate
 * - join arrays
 * - merge sorted arrays
 * Aggregation
 * - sum
 * - min
 * - max
 * - frequency count
 * Transformation
 * - map
 * - filter
 * - reduce
 * Rotation
 * - left
 * - right rotate
 * Deduplication
 * - remove duplicates
 * Prefix operations
 * - prefix sum
 * - running total
 * Two pointers
 * - left pointer 
 * - right pointers
 * - fast
 * - slow
 * Sliding window
 * - fixed size
 * - variable size
 */
import java.util.*;

public class Main {

  // Access methods
  /**
   * Accesses the element at the specified index in the array.
   * 
   * @param arr   the array
   * @param index the index
   * @return the element at the index
   * @throws IndexOutOfBoundsException if index is out of bounds
   */
  public static int access(int[] arr, int index) {
    return arr[index];
  }

  /**
   * Gets the first element of the array.
   * 
   * @param arr the array
   * @return the first element
   * @throws IllegalArgumentException if array is empty
   */
  public static int first(int[] arr) {
    if (arr.length == 0)
      throw new IllegalArgumentException("Array is empty");
    return arr[0];
  }

  /**
   * Gets the last element of the array.
   * 
   * @param arr the array
   * @return the last element
   * @throws IllegalArgumentException if array is empty
   */
  public static int last(int[] arr) {
    if (arr.length == 0)
      throw new IllegalArgumentException("Array is empty");
    return arr[arr.length - 1];
  }

  // Traversal methods
  /**
   * Traverses the array forward and prints each element.
   * 
   * @param arr the array
   */
  public static void traverseForward(int[] arr) {
    for (int i = 0; i < arr.length; i++) {
      System.out.print(arr[i] + " ");
    }
    System.out.println();
  }

  /**
   * Traverses the array backward and prints each element.
   * 
   * @param arr the array
   */
  public static void traverseBackward(int[] arr) {
    for (int i = arr.length - 1; i >= 0; i--) {
      System.out.print(arr[i] + " ");
    }
    System.out.println();
  }

  // Insertion methods (using ArrayList for dynamic size)
  /**
   * Appends a value to the end of the list.
   * 
   * @param list  the list
   * @param value the value to append
   */
  public static void append(List<Integer> list, int value) {
    list.add(value);
  }

  /**
   * Inserts a value at the specified index in the list.
   * 
   * @param list  the list
   * @param index the index
   * @param value the value to insert
   */
  public static void insertAtIndex(List<Integer> list, int index, int value) {
    list.add(index, value);
  }

  /**
   * Prepends a value to the beginning of the list.
   * 
   * @param list  the list
   * @param value the value to prepend
   */
  public static void prepend(List<Integer> list, int value) {
    list.add(0, value);
  }

  // Deletion methods (using ArrayList)
  /**
   * Removes and returns the last element from the list.
   * 
   * @param list the list
   * @return the removed element
   * @throws IllegalArgumentException if list is empty
   */
  public static int pop(List<Integer> list) {
    if (list.isEmpty())
      throw new IllegalArgumentException("List is empty");
    return list.remove(list.size() - 1);
  }

  /**
   * Removes the element at the specified index from the list.
   * 
   * @param list  the list
   * @param index the index
   * @return the removed element
   */
  public static int removeAtIndex(List<Integer> list, int index) {
    return list.remove(index);
  }

  /**
   * Removes the first occurrence of the specified value from the list.
   * 
   * @param list  the list
   * @param value the value to remove
   * @return true if the value was removed, false otherwise
   */
  public static boolean removeByValue(List<Integer> list, int value) {
    return list.remove(Integer.valueOf(value));
  }

  // Search methods
  /**
   * Performs linear search for a target in the array.
   * 
   * @param arr    the array
   * @param target the target value
   * @return the index of the target, or -1 if not found
   */
  public static int linearSearch(int[] arr, int target) {
    for (int i = 0; i < arr.length; i++) {
      if (arr[i] == target)
        return i;
    }
    return -1;
  }

  /**
   * Performs binary search for a target in a sorted array.
   * 
   * @param arr    the sorted array
   * @param target the target value
   * @return the index of the target, or -1 if not found
   */
  public static int binarySearch(int[] arr, int target) {
    int left = 0, right = arr.length - 1;
    while (left <= right) {
      int mid = left + (right - left) / 2;
      if (arr[mid] == target)
        return mid;
      if (arr[mid] < target)
        left = mid + 1;
      else
        right = mid - 1;
    }
    return -1;
  }

  // Update method
  /**
   * Overwrites the value at the specified index in the array.
   * 
   * @param arr   the array
   * @param index the index
   * @param value the new value
   */
  public static void updateAtIndex(int[] arr, int index, int value) {
    arr[index] = value;
  }

  // Sorting methods
  /**
   * Sorts the array using built-in sort.
   * 
   * @param arr the array to sort
   */
  public static void builtInSort(int[] arr) {
    Arrays.sort(arr);
  }

  /**
   * Sorts the array using a custom comparator (for demonstration, sorts in
   * descending order).
   * 
   * @param arr the array to sort
   */
  public static void customSort(int[] arr) {
    Arrays.sort(arr);
    reverseInPlace(arr); // Simple way to reverse for descending
  }

  // Reverse methods
  /**
   * Reverses the array in place.
   * 
   * @param arr the array to reverse
   */
  public static void reverseInPlace(int[] arr) {
    int left = 0, right = arr.length - 1;
    while (left < right) {
      int temp = arr[left];
      arr[left] = arr[right];
      arr[right] = temp;
      left++;
      right--;
    }
  }

  /**
   * Reverses the array using two-pointer swap (same as in-place).
   * 
   * @param arr the array to reverse
   */
  public static void reverseTwoPointer(int[] arr) {
    reverseInPlace(arr);
  }

  // Slicing method
  /**
   * Extracts a subarray from the given array.
   * 
   * @param arr   the original array
   * @param start the start index (inclusive)
   * @param end   the end index (exclusive)
   * @return the subarray
   */
  public static int[] subarray(int[] arr, int start, int end) {
    return Arrays.copyOfRange(arr, start, end);
  }

  // Merge/Concatenate methods
  /**
   * Concatenates two arrays.
   * 
   * @param arr1 the first array
   * @param arr2 the second array
   * @return the concatenated array
   */
  public static int[] concatenate(int[] arr1, int[] arr2) {
    int[] result = new int[arr1.length + arr2.length];
    System.arraycopy(arr1, 0, result, 0, arr1.length);
    System.arraycopy(arr2, 0, result, arr1.length, arr2.length);
    return result;
  }

  /**
   * Merges two sorted arrays into one sorted array.
   * 
   * @param arr1 the first sorted array
   * @param arr2 the second sorted array
   * @return the merged sorted array
   */
  public static int[] mergeSorted(int[] arr1, int[] arr2) {
    int[] result = new int[arr1.length + arr2.length];
    int i = 0, j = 0, k = 0;
    while (i < arr1.length && j < arr2.length) {
      if (arr1[i] <= arr2[j]) {
        result[k++] = arr1[i++];
      } else {
        result[k++] = arr2[j++];
      }
    }
    while (i < arr1.length)
      result[k++] = arr1[i++];
    while (j < arr2.length)
      result[k++] = arr2[j++];
    return result;
  }

  // Aggregation methods
  /**
   * Calculates the sum of the array elements.
   * 
   * @param arr the array
   * @return the sum
   */
  public static int sum(int[] arr) {
    int total = 0;
    for (int num : arr)
      total += num;
    return total;
  }

  /**
   * Finds the minimum value in the array.
   * 
   * @param arr the array
   * @return the minimum value
   * @throws IllegalArgumentException if array is empty
   */
  public static int min(int[] arr) {
    if (arr.length == 0)
      throw new IllegalArgumentException("Array is empty");
    int min = arr[0];
    for (int num : arr)
      if (num < min)
        min = num;
    return min;
  }

  /**
   * Finds the maximum value in the array.
   * 
   * @param arr the array
   * @return the maximum value
   * @throws IllegalArgumentException if array is empty
   */
  public static int max(int[] arr) {
    if (arr.length == 0)
      throw new IllegalArgumentException("Array is empty");
    int max = arr[0];
    for (int num : arr)
      if (num > max)
        max = num;
    return max;
  }

  /**
   * Counts the frequency of each element in the array.
   * 
   * @param arr the array
   * @return a map of value to frequency
   */
  public static Map<Integer, Integer> frequencyCount(int[] arr) {
    Map<Integer, Integer> freq = new HashMap<>();
    for (int num : arr) {
      freq.put(num, freq.getOrDefault(num, 0) + 1);
    }
    return freq;
  }

  // Transformation methods (using functional interfaces)
  /**
   * Maps each element of the array using the provided function.
   * 
   * @param arr    the array
   * @param mapper the mapping function
   * @return the mapped array
   */
  public static int[] map(int[] arr, java.util.function.IntUnaryOperator mapper) {
    int[] result = new int[arr.length];
    for (int i = 0; i < arr.length; i++) {
      result[i] = mapper.applyAsInt(arr[i]);
    }
    return result;
  }

  /**
   * Filters the array based on the predicate.
   * 
   * @param arr       the array
   * @param predicate the filter predicate
   * @return the filtered list
   */
  public static List<Integer> filter(int[] arr, java.util.function.IntPredicate predicate) {
    List<Integer> result = new ArrayList<>();
    for (int num : arr) {
      if (predicate.test(num))
        result.add(num);
    }
    return result;
  }

  /**
   * Reduces the array to a single value using the accumulator.
   * 
   * @param arr         the array
   * @param identity    the identity value
   * @param accumulator the accumulator function
   * @return the reduced value
   */
  public static int reduce(int[] arr, int identity, java.util.function.IntBinaryOperator accumulator) {
    int result = identity;
    for (int num : arr) {
      result = accumulator.applyAsInt(result, num);
    }
    return result;
  }

  // Rotation methods
  /**
   * Rotates the array to the left by k positions.
   * 
   * @param arr the array
   * @param k   the number of positions to rotate
   */
  public static void rotateLeft(int[] arr, int k) {
    k %= arr.length;
    reverseInPlace(Arrays.copyOfRange(arr, 0, k));
    reverseInPlace(Arrays.copyOfRange(arr, k, arr.length));
    reverseInPlace(arr);
  }

  /**
   * Rotates the array to the right by k positions.
   * 
   * @param arr the array
   * @param k   the number of positions to rotate
   */
  public static void rotateRight(int[] arr, int k) {
    k %= arr.length;
    rotateLeft(arr, arr.length - k);
  }

  // Deduplication method
  /**
   * Removes duplicates from the array (returns new array).
   * 
   * @param arr the array
   * @return the array with duplicates removed
   */
  public static int[] removeDuplicates(int[] arr) {
    Set<Integer> set = new LinkedHashSet<>();
    for (int num : arr)
      set.add(num);
    return set.stream().mapToInt(i -> i).toArray();
  }

  // Prefix operations
  /**
   * Computes the prefix sum array.
   * 
   * @param arr the array
   * @return the prefix sum array
   */
  public static int[] prefixSum(int[] arr) {
    int[] prefix = new int[arr.length];
    if (arr.length > 0)
      prefix[0] = arr[0];
    for (int i = 1; i < arr.length; i++) {
      prefix[i] = prefix[i - 1] + arr[i];
    }
    return prefix;
  }

  /**
   * Computes the running total (same as prefix sum).
   * 
   * @param arr the array
   * @return the running total array
   */
  public static int[] runningTotal(int[] arr) {
    return prefixSum(arr);
  }

  // Two pointers methods
  /**
   * Demonstrates left/right pointers (e.g., for reversing).
   * 
   * @param arr the array
   */
  public static void leftRightPointers(int[] arr) {
    reverseInPlace(arr);
  }

  /**
   * Demonstrates fast/slow pointers (e.g., for cycle detection in linked list,
   * but for array, find middle).
   * 
   * @param arr the array
   * @return the middle index
   */
  public static int fastSlowPointers(int[] arr) {
    int slow = 0, fast = 0;
    while (fast < arr.length - 1) {
      slow++;
      fast += 2;
    }
    return slow;
  }

  // Sliding window methods
  /**
   * Finds the maximum sum of a subarray of fixed size k.
   * 
   * @param arr the array
   * @param k   the window size
   * @return the maximum sum
   */
  public static int maxSumFixedWindow(int[] arr, int k) {
    int maxSum = 0;
    for (int i = 0; i < k; i++)
      maxSum += arr[i];
    int currentSum = maxSum;
    for (int i = k; i < arr.length; i++) {
      currentSum += arr[i] - arr[i - k];
      if (currentSum > maxSum)
        maxSum = currentSum;
    }
    return maxSum;
  }

  /**
   * Finds the minimum length subarray with sum >= target (variable size window).
   * 
   * @param arr    the array
   * @param target the target sum
   * @return the minimum length, or 0 if not found
   */
  public static int minLengthVariableWindow(int[] arr, int target) {
    int minLen = Integer.MAX_VALUE;
    int currentSum = 0;
    int left = 0;
    for (int right = 0; right < arr.length; right++) {
      currentSum += arr[right];
      while (currentSum >= target && left <= right) {
        minLen = Math.min(minLen, right - left + 1);
        currentSum -= arr[left];
        left++;
      }
    }
    return minLen == Integer.MAX_VALUE ? 0 : minLen;
  }

}
