// Lesson 15: Loops (../15_loops.md)
// for, while, do-while, foreach
// Run: dotnet run 15-01_times_table_and_fizzbuzz.cs

for (int row = 1; row <= 4; row++)
{
    for (int col = 1; col <= 4; col++)
    {
        Console.Write($"{row * col,4}");
    }
    Console.WriteLine();
}

Console.WriteLine();
for (int i = 1; i <= 15; i++)
{
    Console.Write(i switch
    {
        _ when i % 15 == 0 => "FizzBuzz",
        _ when i % 3 == 0 => "Fizz",
        _ when i % 5 == 0 => "Buzz",
        _ => i.ToString()
    });
    Console.Write(i < 15 ? " " : "\n");
}

int n = 27, steps = 0;
while (n != 1)
{
    n = n % 2 == 0 ? n / 2 : 3 * n + 1;
    steps++;
}
Console.WriteLine($"Collatz(27) reaches 1 in {steps} steps");

int guess = 0;
do
{
    guess += 7;
} while (guess % 5 != 0);
Console.WriteLine($"First multiple of 7 that is also a multiple of 5: {guess}");

foreach (char c in "loop")
{
    if (c == 'p') break;
    Console.Write(char.ToUpper(c));
}
Console.WriteLine();

// Expected output should be:
//    1   2   3   4
//    2   4   6   8
//    3   6   9  12
//    4   8  12  16
//
// 1 2 Fizz 4 Buzz Fizz 7 8 Fizz Buzz 11 Fizz 13 14 FizzBuzz
// Collatz(27) reaches 1 in 111 steps
// First multiple of 7 that is also a multiple of 5: 35
// LOO
