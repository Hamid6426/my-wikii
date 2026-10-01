// Lesson 04: Basics of a Program (../04_basics_of_a_program.md)
// Preprocessor directives
// Run: dotnet run 04-02_preprocessor.cs

#define VERBOSE

#if DEBUG
Console.WriteLine("Debug build");
#else
Console.WriteLine("Release build");
#endif

#if VERBOSE
Console.WriteLine("VERBOSE is defined");
#endif

#region Helpers
static int Square(int x) => x * x;
#endregion

Console.WriteLine(Square(7));

// Expected output should be:
// Debug build
// VERBOSE is defined
// 49
