// Lesson 24: Nullable Reference Types (../24_nullable_reference_types.md)
// Nullable reference types and null operators
// Run: dotnet run 24-01_nullable_references.cs

string? maybeName = GetName(false);
Console.WriteLine(maybeName?.ToUpper() ?? "no name");
Console.WriteLine(GetName(true)?.ToUpper() ?? "no name");

string? cache = null;
cache ??= "loaded once";
cache ??= "ignored";
Console.WriteLine(cache);

int? number = null;
Console.WriteLine(number.HasValue ? number.Value : -1);
Console.WriteLine(number ?? 0);

var user = new User { Name = "Alice" };
Console.WriteLine($"{user.Name} {user.Nickname?.Length}");

try
{
    Save(null!);
}
catch (ArgumentNullException ex)
{
    Console.WriteLine($"Caught: {ex.ParamName}");
}

static string? GetName(bool exists) => exists ? "alice" : null;

static void Save(string path)
{
    ArgumentNullException.ThrowIfNull(path);
}

class User
{
    public required string Name { get; init; }
    public string? Nickname { get; init; }
}

// Expected output should be:
// no name
// ALICE
// loaded once
// -1
// 0
// Alice
// Caught: path
