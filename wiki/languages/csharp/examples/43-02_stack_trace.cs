// Lesson 43: Debugging (../43_debugging.md)
// Reading a stack trace
// Run: dotnet run 43-02_stack_trace.cs

// Run this and read the stack trace from the top: the first line that is
// in your own code shows where the error was thrown.

try
{
    Report.Print(new[] { 10, 20 }, index: 5);
}
catch (Exception ex)
{
    Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
    foreach (string line in ex.StackTrace!.Split('\n'))
    {
        string text = line.Trim();
        int where = text.IndexOf(" in ");
        Console.WriteLine(where > 0 ? text[..where] : text);
    }
}

static class Report
{
    public static void Print(int[] data, int index)
    {
        Console.WriteLine(Pick(data, index));
    }

    static int Pick(int[] data, int index) => data[index];
}

// Expected output should be:
// IndexOutOfRangeException: Index was outside the bounds of the array.
// at Report.Pick(Int32[] data, Int32 index)
// at Report.Print(Int32[] data, Int32 index)
// at Program.<Main>$(String[] args)
