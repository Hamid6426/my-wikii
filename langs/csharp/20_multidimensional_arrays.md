# 20 - Multidimensional Arrays

## 2D Array (Rectangular)

All rows have the same number of columns.

```csharp
// Declaration
int[,] matrix = new int[3, 4];  // 3 rows, 4 columns

// Initialization
int[,] grid = {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};
```

---

## Accessing Elements

```csharp
int[,] grid = {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

Console.WriteLine(grid[0, 0]);  // 1
Console.WriteLine(grid[1, 2]);  // 6
grid[2, 1] = 99;                // modify
```

---

## Dimensions and Length

```csharp
int[,] grid = new int[3, 4];

Console.WriteLine(grid.GetLength(0));  // 3: rows
Console.WriteLine(grid.GetLength(1));  // 4: columns
Console.WriteLine(grid.Length);        // 12: total elements
Console.WriteLine(grid.Rank);          // 2: number of dimensions
```

---

## Iterating a 2D Array

```csharp
int[,] grid = {
    { 1, 2, 3 },
    { 4, 5, 6 },
    { 7, 8, 9 }
};

for (int row = 0; row < grid.GetLength(0); row++)
{
    for (int col = 0; col < grid.GetLength(1); col++)
    {
        Console.Write($"{grid[row, col]} ");
    }
    Console.WriteLine();
}
```

---

## 3D Array

```csharp
int[,,] cube = new int[2, 3, 4];  // 2 × 3 × 4

cube[0, 1, 2] = 42;
Console.WriteLine(cube.GetLength(2));  // 4
```

---

## Jagged Arrays

Array of arrays: rows can have different lengths. More flexible, slight overhead.

```csharp
int[][] jagged = new int[3][];
jagged[0] = new int[] { 1, 2 };
jagged[1] = new int[] { 3, 4, 5, 6 };
jagged[2] = new int[] { 7 };
```

Shorthand initialization:

```csharp
int[][] jagged = {
    new int[] { 1, 2 },
    new int[] { 3, 4, 5 },
    new int[] { 6 }
};
```

---

## Iterating a Jagged Array

```csharp
foreach (int[] row in jagged)
{
    foreach (int val in row)
    {
        Console.Write($"{val} ");
    }
    Console.WriteLine();
}
```

---

## Rectangular vs Jagged

| Feature          | Rectangular `[,]` | Jagged `[][]`                    |
| ---------------- | ----------------- | -------------------------------- |
| Rows same length | Yes               | No                               |
| Memory layout    | Contiguous        | Separate allocations             |
| Performance      | Slightly faster   | Slight overhead                  |
| Index syntax     | `arr[i, j]`       | `arr[i][j]`                      |
| Common use       | Matrices, grids   | Triangular tables, variable rows |

---

## Gotchas

- `foreach` on a 2D array iterates all elements row by row, not row objects
- Jagged array rows must be explicitly allocated: `jagged[i] = new int[n]`
- `array.Length` on a rectangular array returns the total element count, not rows
- Deep copy of multidimensional arrays requires manual element-by-element copying

---

## Examples

- [20-01](examples/20-01_grid_and_jagged.cs): 2D and jagged arrays

Run one with `dotnet run examples/20-01_grid_and_jagged.cs`. See [Examples](examples/README.md).
