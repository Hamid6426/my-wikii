// Lesson 18: Arrays (../18_arrays.md)
// The arrays class
// Run: java 18-01_the_arrays_class.java

import java.util.Arrays;

public class ArrayTools {
    public static void main(String[] args) {
        int[] arr = {5, 2, 8, 1, 9, 3};

        System.out.println(arr);                    // not useful
        System.out.println(Arrays.toString(arr));

        Arrays.sort(arr);                           // sorts in place
        System.out.println(Arrays.toString(arr));

        int idx = Arrays.binarySearch(arr, 8);      // needs a sorted array
        System.out.println("8 is at " + idx);

        int[] firstThree = Arrays.copyOf(arr, 3);
        int[] middle = Arrays.copyOfRange(arr, 2, 5);   // index 2, 3, 4
        System.out.println(Arrays.toString(firstThree) + " " + Arrays.toString(middle));

        int[] filled = new int[4];
        Arrays.fill(filled, 7);
        System.out.println(Arrays.toString(filled));

        int[] other = {1, 2, 3, 5, 8, 9};
        System.out.println(arr == other);               // different objects
        System.out.println(Arrays.equals(arr, other));  // same contents

        System.out.println(Arrays.stream(arr).sum());
    }
}
