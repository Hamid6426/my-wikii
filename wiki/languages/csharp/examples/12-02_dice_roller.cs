// Lesson 12: Math, Random and Utility Types (../12_math_random_and_utilities.md)
// Seeded random numbers
// Run: dotnet run 12-02_dice_roller.cs

var rng = new Random(42);          // a seed makes the sequence repeat
var counts = new int[7];

for (int i = 0; i < 600; i++)
{
    counts[rng.Next(1, 7)]++;      // upper bound is excluded
}

for (int face = 1; face <= 6; face++)
{
    Console.WriteLine($"{face}: {new string('#', counts[face] / 5)} {counts[face]}");
}

Console.WriteLine($"Total: {counts.Sum()} rolls");

// Expected output should be:
// 1: ####################### 116
// 2: ################### 98
// 3: ################# 89
// 4: ##################### 108
// 5: ################ 83
// 6: ##################### 106
// Total: 600 rolls
