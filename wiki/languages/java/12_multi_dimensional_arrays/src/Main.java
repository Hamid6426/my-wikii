/**
 * Multi-dimensional arrays are arrays of arrays. They are used to store data in
 * a tabular format, such as a matrix or a grid. A multi-dimensional array can
 * have two or more dimensions.
 * 
 * The most common multi-dimensional array is a two-dimensional array, which is
 * often used to represent a table of data with rows and columns. In Java,
 * multi-dimensional arrays are declared using multiple sets of square brackets.
 * 
 * For example, a two-dimensional array of integers can be declared as int[][]
 * matrix = new int[3][4]; This creates a two-dimensional array with 3 rows and
 * 4 columns.
 * 
 * Each element in the array can be accessed using two indices: the row index
 * and the column index. For example, matrix[0][0] refers to the element in the
 * first row and first column, while matrix[2][3] refers to the element in the
 * third row and fourth column.
 * 
 * Multi-dimensional arrays can also be initialized with values at the time of
 * declaration.
 * 
 * For example, int[][] matrix = { {1, 2, 3}, {4, 5, 6}, {7, 8, 9} };
 * 
 * This creates a two-dimensional array with 3 rows and 3 columns, and
 * initializes it with the values from 1 to 9. Multi-dimensional arrays can be
 * used in various applications, such as representing images, storing data in a
 * spreadsheet, or implementing algorithms that require a grid-like structure.
 */
public class Main {
  public static void main(String[] args) {
    int[][] matrix = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };

    // Accessing elements of the multi-dimensional array
    System.out.println("Element at row 0, column 0: " + matrix[0][0]); // 1
    System.out.println("Element at row 1, column 2: " + matrix[1][2]); // 6
    System.out.println("Element at row 2, column 1: " + matrix[2][1]); // 8

    // Iterating through the multi-dimensional array
    System.out.println("Elements of the multi-dimensional array:");
    for (int i = 0; i < matrix.length; i++) {
      for (int j = 0; j < matrix[i].length; j++) {
        System.out.print(matrix[i][j] + " ");
      }
      System.out.println();
    }
  }
}