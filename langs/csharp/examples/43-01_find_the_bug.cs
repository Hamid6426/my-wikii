// Lesson 43: Debugging (../43_debugging.md)
// A program with a bug to find using the debugger
// Run: dotnet run 43-01_find_the_bug.cs

// This program should print the average of the positive numbers.
// It has a bug. Run it, then use breakpoints to find it (see the Debugging lesson).

int[] readings = { 12, -5, 8, 0, 20, -1, 15 };

Console.WriteLine($"Average of positives: {AveragePositive(readings)}");

static double AveragePositive(int[] values)
{
    int sum = 0;
    int count = 0;

    for (int i = 0; i < values.Length; i++)
    {
        if (values[i] > 0)
        {
            sum += values[i];
        }
        count++;                      // BUG: counts every item, not only positive ones
    }

    return (double)sum / count;
}

// With the bug the program prints 7.857142857142857.
// Fix: move `count++;` inside the if-block. The correct answer is 13.75.
