// Lesson 19: Arrays (../19_arrays.md)
// Create, read, change, slice, sort arrays
// Run: dotnet run 19-01_array_basics.cs

int[] scores = { 72, 95, 60, 88, 79 };

Console.WriteLine($"Length: {scores.Length}");
Console.WriteLine($"First: {scores[0]}, last: {scores[^1]}");
scores[2] = 65;

Console.WriteLine($"Max {scores.Max()}, min {scores.Min()}, average {scores.Average():F1}");
Console.WriteLine($"Index of 88: {Array.IndexOf(scores, 88)}");

int[] top3 = scores.OrderByDescending(s => s).Take(3).ToArray();
Console.WriteLine($"Top 3: {string.Join(", ", top3)}");

int[] slice = scores[1..4];
Console.WriteLine($"Slice [1..4]: {string.Join(", ", slice)}");

Array.Sort(scores);
Console.WriteLine($"Sorted: {string.Join(", ", scores)}");
Array.Reverse(scores);
Console.WriteLine($"Reversed: {string.Join(", ", scores)}");

int[] copy = (int[])scores.Clone();
copy[0] = -1;
Console.WriteLine($"{scores[0]} vs {copy[0]}");

int[] alias = scores;
alias[0] = 999;
Console.WriteLine($"{scores[0]} (alias changed the original)");

// Expected output should be:
// Length: 5
// First: 72, last: 79
// Max 95, min 65, average 79.8
// Index of 88: 3
// Top 3: 95, 88, 79
// Slice [1..4]: 95, 65, 88
// Sorted: 65, 72, 79, 88, 95
// Reversed: 95, 88, 79, 72, 65
// 95 vs -1
// 999 (alias changed the original)
