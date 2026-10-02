// Lesson 19: Multidimensional Arrays (../19_multidimensional_arrays.md)
// Iterating a 2d array
// Run: java 19-01_iterating_a_2d_array.java

public class Grid {
    public static void main(String[] args) {
        int[][] grid = {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };

        for (int row = 0; row < grid.length; row++) {
            for (int col = 0; col < grid[row].length; col++) {
                System.out.print(grid[row][col] + " ");
            }
            System.out.println();
        }

        int sum = 0;
        for (int[] row : grid) {
            for (int value : row) {
                sum += value;
            }
        }
        System.out.println("sum = " + sum);
    }
}
