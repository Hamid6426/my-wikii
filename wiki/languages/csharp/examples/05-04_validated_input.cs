// Lesson 05: Console Input and Output (../05_console_input_and_output.md)
// Ask until the input is valid
// Run: dotnet run 05-04_validated_input.cs   (type the input when asked)

int age = 0;
string? line;

Console.Write("Age: ");
while ((line = Console.ReadLine()) != null)
{
    if (int.TryParse(line, out age) && age is >= 0 and <= 150)
    {
        break;
    }

    Console.Write("Enter a whole number from 0 to 150: ");
}

Console.WriteLine();
Console.WriteLine($"Age set to {age}. In ten years: {age + 10}");

// Expected output should be:
// Age: abc
// Enter a whole number from 0 to 150: -4
// Enter a whole number from 0 to 150: 200
// Enter a whole number from 0 to 150: 32
//
// Age set to 32. In ten years: 42
