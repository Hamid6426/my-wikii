// Lesson 26: Classes & Objects (../26_classes_and_objects.md)
// A class with fields, properties, and a constructor
// Run: dotnet run 26-01_bank_account.cs

var account = new BankAccount("Alice", 100m);
account.Deposit(50m);
account.Withdraw(30m);
Console.WriteLine(account);

try
{
    account.Withdraw(500m);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.WriteLine($"Accounts created: {BankAccount.Count}");
var second = new BankAccount("Bob");
Console.WriteLine($"Accounts created: {BankAccount.Count}");

class BankAccount
{
    private decimal _balance;

    public string Owner { get; }
    public decimal Balance => _balance;
    public static int Count { get; private set; }

    public BankAccount(string owner, decimal opening = 0m)
    {
        Owner = owner;
        _balance = opening;
        Count++;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        _balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount > _balance) throw new InvalidOperationException("Insufficient funds");
        _balance -= amount;
    }

    public override string ToString() => $"{Owner}: {_balance:N2}";
}

// Expected output should be:
// Alice: 120.00
// Error: Insufficient funds
// Accounts created: 1
// Accounts created: 2
