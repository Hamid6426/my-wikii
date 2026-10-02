// Lesson 20: Multidimensional Arrays (../20_multidimensional_arrays.md)
// 2D and jagged arrays
// Run: dotnet run 20-01_grid_and_jagged.cs

int[,] grid = new int[3, 4];
for (int r = 0; r < grid.GetLength(0); r++)
{
    for (int c = 0; c < grid.GetLength(1); c++)
    {
        grid[r, c] = r * 10 + c;
    }
}

for (int r = 0; r < grid.GetLength(0); r++)
{
    for (int c = 0; c < grid.GetLength(1); c++)
    {
        Console.Write($"{grid[r, c],4}");
    }
    Console.WriteLine();
}
Console.WriteLine($"Rows {grid.GetLength(0)}, columns {grid.GetLength(1)}, total {grid.Length}");

int[][] triangle = new int[5][];
for (int i = 0; i < triangle.Length; i++)
{
    triangle[i] = new int[i + 1];
    triangle[i][0] = triangle[i][i] = 1;
    for (int j = 1; j < i; j++)
    {
        triangle[i][j] = triangle[i - 1][j - 1] + triangle[i - 1][j];
    }
}

foreach (int[] row in triangle)
{
    Console.WriteLine(string.Join(" ", row).PadLeft(8 + string.Join(" ", row).Length / 2));
}

// Expected output should be:
//    0   1   2   3
//   10  11  12  13
//   20  21  22  23
// Rows 3, columns 4, total 12
//        1
//       1 1
//      1 2 1
//     1 3 3 1
//    1 4 6 4 1
