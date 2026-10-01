// Lesson 25: Records & Equality (../25_records_and_equality.md)
// Equals, GetHashCode, IEquatable, and comparers
// Run: dotnet run 25-02_equality_in_depth.cs

// 1. A class uses reference equality by default
var a = new PlainPoint(1, 2);
var b = new PlainPoint(1, 2);
Console.WriteLine($"PlainPoint: a == b is {a == b}, Equals is {a.Equals(b)}");

// 2. A record compares by value automatically
var r1 = new RecordPoint(1, 2);
var r2 = new RecordPoint(1, 2);
Console.WriteLine($"RecordPoint: r1 == r2 is {r1 == r2}, same hash is {r1.GetHashCode() == r2.GetHashCode()}");

// 3. A class can opt in: IEquatable<T>, Equals, GetHashCode, and the operators
var c1 = new Money(5m, "USD");
var c2 = new Money(5m, "USD");
var c3 = new Money(5m, "EUR");
Console.WriteLine($"Money: c1 == c2 is {c1 == c2}, c1 == c3 is {c1 == c3}, c1 != c3 is {c1 != c3}");

// 4. Equality drives HashSet and Dictionary
var set = new HashSet<Money> { c1, c2, c3 };
Console.WriteLine($"HashSet holds {set.Count} items (c1 and c2 are the same value)");

var prices = new Dictionary<Money, string> { [c1] = "first" };
Console.WriteLine($"Lookup with an equal key: {prices[c2]}");

// 5. Forgetting GetHashCode breaks sets and dictionaries
var bad1 = new BadMoney(5m);
var bad2 = new BadMoney(5m);
Console.WriteLine($"BadMoney: Equals says {bad1.Equals(bad2)}, but a HashSet holds {new HashSet<BadMoney> { bad1, bad2 }.Count} items");

// 6. A record compares each member with that member's own Equals
var user = new User("Ann", ["admin"]);
var sameList = user with { Name = "Ann" };           // 'with' copies the reference to the same list
var otherList = new User("Ann", ["admin"]);          // a different list with the same content
Console.WriteLine($"Same list object: {user == sameList}");
Console.WriteLine($"Different list, same content: {user == otherList} (lists compare by reference)");

// 7. A custom comparer changes equality without changing the type
var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Alice", "ALICE", "bob" };
Console.WriteLine($"Case-insensitive set count: {names.Count}");

var byLength = new HashSet<string>(new LengthComparer()) { "cat", "dog", "bird" };
Console.WriteLine($"Equal when same length: {byLength.Count}");

class PlainPoint(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;
}

record RecordPoint(int X, int Y);

sealed class Money(decimal amount, string currency) : IEquatable<Money>
{
    public decimal Amount { get; } = amount;
    public string Currency { get; } = currency;

    public bool Equals(Money? other) =>
        other is not null && Amount == other.Amount && Currency == other.Currency;

    public override bool Equals(object? obj) => Equals(obj as Money);

    public override int GetHashCode() => HashCode.Combine(Amount, Currency);

    public static bool operator ==(Money? left, Money? right) => left is null ? right is null : left.Equals(right);
    public static bool operator !=(Money? left, Money? right) => !(left == right);
}

#pragma warning disable CS0659   // on purpose: this class shows what happens without GetHashCode
class BadMoney(decimal amount)
{
    public decimal Amount { get; } = amount;
    public override bool Equals(object? obj) => obj is BadMoney m && m.Amount == Amount;
    // GetHashCode is not overridden: equal objects get different hash codes
}
#pragma warning restore CS0659

record User(string Name, List<string> Roles);

class LengthComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y) => x?.Length == y?.Length;
    public int GetHashCode(string s) => s.Length;
}

// Expected output should be:
// PlainPoint: a == b is False, Equals is False
// RecordPoint: r1 == r2 is True, same hash is True
// Money: c1 == c2 is True, c1 == c3 is False, c1 != c3 is True
// HashSet holds 2 items (c1 and c2 are the same value)
// Lookup with an equal key: first
// BadMoney: Equals says True, but a HashSet holds 2 items
// Same list object: True
// Different list, same content: False (lists compare by reference)
// Case-insensitive set count: 2
// Equal when same length: 2
