// Lesson 15: Loops (../15_loops.md)
// Nested loops and labels
// Run: java 15-01_nested_loops_and_labels.java

public class FindPair {
    public static void main(String[] args) {
        int[][] grid = {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };

        outer:
        for (int row = 0; row < grid.length; row++) {
            for (int col = 0; col < grid[row].length; col++) {
                if (grid[row][col] == 5) {
                    System.out.println("found 5 at " + row + "," + col);
                    break outer;
                }
                System.out.println("checked " + grid[row][col]);
            }
        }
    }
}
