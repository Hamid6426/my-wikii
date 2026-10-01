// Lesson 42: Error Handling (../42_error_handling.md)
// try, catch, finally, throw, custom exceptions
// Run: dotnet run 42-01_error_handling.cs

Console.WriteLine(SafeDivide(10, 2));
Console.WriteLine(SafeDivide(1, 0));

try
{
    ParseAge("abc");
}
catch (FormatException ex)
{
    Console.WriteLine($"FormatException: {ex.Message}");
}

try
{
    Withdraw(balance: 50, amount: 80);
}
catch (InsufficientFundsException ex) when (ex.Shortfall > 10)
{
    Console.WriteLine($"Need {ex.Shortfall} more");
}
finally
{
    Console.WriteLine("finally always runs");
}

try
{
    try
    {
        int[] data = new int[2];
        data[5] = 1;
    }
    catch (IndexOutOfRangeException ex)
    {
        throw new InvalidOperationException("Could not store the value", ex);
    }
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"{ex.Message}, caused by {ex.InnerException!.GetType().Name}");
}

static string SafeDivide(int a, int b)
{
    try
    {
        return (a / b).ToString();
    }
    catch (DivideByZeroException)
    {
        return "cannot divide by zero";
    }
}

static int ParseAge(string text) => int.Parse(text);

static void Withdraw(decimal balance, decimal amount)
{
    if (amount > balance) throw new InsufficientFundsException(amount - balance);
}

class InsufficientFundsException(decimal shortfall) : Exception($"Short by {shortfall}")
{
    public decimal Shortfall { get; } = shortfall;
}

// Expected output should be:
// 5
// cannot divide by zero
// FormatException: The input string 'abc' was not in a correct format.
// Need 30 more
// finally always runs
// Could not store the value, caused by IndexOutOfRangeException
