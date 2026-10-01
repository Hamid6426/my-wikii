// Lesson 04: Basics of a Program (../04_basics_of_a_program.md)
// Program structure with an explicit Main
// Run: dotnet run 04-01_program_structure.cs

namespace MyApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine($"Program started with {args.Length} argument(s)");
        Greet("C#");
    }

    static void Greet(string name)
    {
        Console.WriteLine($"Hello from {name}");
    }
}

// Expected output should be:
// Program started with 0 argument(s)
// Hello from C#
