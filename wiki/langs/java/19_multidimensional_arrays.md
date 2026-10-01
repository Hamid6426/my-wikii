# 19 - Multidimensional Arrays

## Arrays of Arrays

Java has no true grid type. A 2D array is an array whose items are arrays. Each row is its own array object.

```java
// 3 rows, 4 columns, all zero
int[][] matrix = new int[3][4];

// With values
int[][] grid = {
    {1, 2, 3},
    {4, 5, 6},
    {7, 8, 9}
};
```

---

## Accessing Elements

`grid[row][col]`: first pick the row, then the item in that row.

```java
System.out.println(grid[0][0]);   // 1
System.out.println(grid[1][2]);   // 6
grid[2][1] = 99;                  // change a value

int[] middleRow = grid[1];        // a whole row is a normal int[]
```

---

## Dimensions and Length

There is no single "total size". Ask each level for its `length`.

```java
int[][] grid = new int[3][4];

System.out.println(grid.length);      // 3: rows
System.out.println(grid[0].length);   // 4: columns in row 0
```

---

## Iterating a 2D Array

```java
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
```

Expected output should be:

```
1 2 3 
4 5 6 
7 8 9 
sum = 45
```

Use `grid[row].length` in the inner loop, not `grid[0].length`, so rows of different lengths still work.

---

## Jagged Arrays

A **jagged** array has rows of different lengths. Leave the second size empty, then create each row.

```java
int[][] jagged = new int[3][];
jagged[0] = new int[] {1, 2};
jagged[1] = new int[] {3, 4, 5, 6};
jagged[2] = new int[] {7};
```

Shorthand:

```java
int[][] triangle = {
    {1},
    {1, 1},
    {1, 2, 1},
    {1, 3, 3, 1}
};
```

Until a row is created, it is `null`.

---

## Printing and Comparing

`Arrays.toString` shows only the row references. Use the `deep` versions.

```java
int[][] a = {{1, 2}, {3, 4}};
int[][] b = {{1, 2}, {3, 4}};

System.out.println(Arrays.toString(a));       // [[I@..., [I@...]
System.out.println(Arrays.deepToString(a));   // [[1, 2], [3, 4]]

System.out.println(Arrays.equals(a, b));      // false: compares row references
System.out.println(Arrays.deepEquals(a, b));  // true: compares values
```

---

## Copying

`clone()` copies only the outer array. Both copies share the same rows.

```java
int[][] original = {{1, 2}, {3, 4}};

int[][] shallow = original.clone();
shallow[0][0] = 99;
System.out.println(original[0][0]);   // 99: same row arrays

int[][] deep = new int[original.length][];
for (int i = 0; i < original.length; i++) {
    deep[i] = original[i].clone();     // copy each row
}
```

---

## 3D and Beyond

Add one more `[]` per level.

```java
int[][][] cube = new int[2][3][4];   // 2 x 3 x 4

cube[0][1][2] = 42;
System.out.println(cube[0][1].length);   // 4
```

More than two levels is rare. A class or record with clear names often reads better.

---

## Java vs Other Languages

| Feature          | Java `int[][]`               | C# `int[,]`  |
| ---------------- | ---------------------------- | ------------ |
| Rows same length | Not required                 | Always       |
| Memory layout    | One array per row            | One block    |
| Index syntax     | `arr[i][j]`                  | `arr[i, j]`  |
| Total item count | Add up `row.length` yourself | `arr.Length` |

---

## Gotchas

- `new int[3][]` creates 3 `null` rows; using one before creating it throws `NullPointerException`
- `grid.length` is the number of rows, not the number of items
- `clone()` and `Arrays.copyOf` are shallow for 2D arrays; copy each row for a real copy
- `Arrays.equals` and `Arrays.toString` do not look inside rows; use `deepEquals` and `deepToString`
- Looping column by column (`grid[col][row]` order) is slower than row by row for big arrays, because each row is a separate block of memory

---

## Examples

- [19-01](examples/19-01_iterating_a_2d_array.java): Iterating a 2d array

Run one with `java examples/19-01_iterating_a_2d_array.java`. See [Examples](examples/README.md).
