// Lesson 09: Enums (../09_enums.md)
// Enums and flags
// Run: dotnet run 09-01_enums.cs

Status status = Status.Active;
Console.WriteLine($"{status} = {(int)status}");

foreach (Status s in Enum.GetValues<Status>())
{
    Console.WriteLine($"  {s,-8} -> {Describe(s)}");
}

Console.WriteLine(Enum.TryParse("Closed", out Status parsed) ? $"Parsed {parsed}" : "No match");
Console.WriteLine(Enum.TryParse("Nope", out Status _) ? "Parsed" : "Nope is not a Status");

Permission perm = Permission.Read | Permission.Write;
Console.WriteLine(perm);
Console.WriteLine(perm.HasFlag(Permission.Write));
perm &= ~Permission.Write;
Console.WriteLine(perm);

static string Describe(Status s) => s switch
{
    Status.Pending => "Waiting",
    Status.Active => "Running",
    Status.Closed => "Done",
    _ => "Unknown"
};

enum Status { Pending, Active, Closed }

[Flags]
enum Permission { None = 0, Read = 1, Write = 2, Execute = 4 }

// Expected output should be:
// Active = 1
//   Pending  -> Waiting
//   Active   -> Running
//   Closed   -> Done
// Parsed Closed
// Nope is not a Status
// Read, Write
// True
// Read
