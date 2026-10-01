// Lesson 05: Console Input and Output (../05_console_input_and_output.md)
// Read a name and greet
// Run: dotnet run 05-01_greeting.cs   (type the input when asked)

Console.Write("Your name: ");
string? name = Console.ReadLine();

if (string.IsNullOrWhiteSpace(name))
{
    name = "stranger";
}

Console.WriteLine($"Hello, {name}!");

// Expected output should be:
// Your name: Alice
// Hello, Alice!
