// Lesson 05: Console Input and Output (../05_console_input_and_output.md)
// Command-line arguments and an exit code
// Run: dotnet run 05-03_args_and_exit_code.cs -- Alice Bob

if (args.Length == 0)
{
    Console.Error.WriteLine("Usage: dotnet run 05-03_args_and_exit_code.cs -- <name> [name...]");
    return 1;
}

foreach (string name in args)
{
    Console.WriteLine($"Hello, {name}");
}

return 0;

// Expected output should be:
// Hello, Alice
// Hello, Bob
