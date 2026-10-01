// Lesson 34: Collections (../34_collections.md)
// List, Dictionary, HashSet
// Run: dotnet run 34-01_collections_tour.cs

var names = new List<string> { "Carol", "alice", "Bob" };
names.Add("Dave");
names.Remove("Bob");
names.Sort(StringComparer.OrdinalIgnoreCase);
Console.WriteLine(string.Join(", ", names));
Console.WriteLine($"Contains Carol: {names.Contains("Carol")}, index of Dave: {names.IndexOf("Dave")}");

var stock = new Dictionary<string, int> { ["apple"] = 5, ["pear"] = 0 };
stock["banana"] = 12;
stock["apple"] += 3;

foreach (var (fruit, qty) in stock.OrderBy(kv => kv.Key))
{
    Console.WriteLine($"{fruit,-7} {qty,3}");
}

if (stock.TryGetValue("kiwi", out int kiwi)) Console.WriteLine(kiwi);
else Console.WriteLine("No kiwi in stock");

var words = "the cat and the hat and the bat".Split(' ');
var freq = new Dictionary<string, int>();
foreach (string w in words)
{
    freq[w] = freq.GetValueOrDefault(w) + 1;
}
Console.WriteLine(string.Join(", ", freq.OrderByDescending(kv => kv.Value).ThenBy(kv => kv.Key).Select(kv => $"{kv.Key}={kv.Value}")));

var unique = new HashSet<string>(words);
Console.WriteLine($"{words.Length} words, {unique.Count} unique");

var a = new HashSet<int> { 1, 2, 3, 4 };
var b = new HashSet<int> { 3, 4, 5 };
Console.WriteLine($"union {string.Join(",", a.Union(b))}, intersect {string.Join(",", a.Intersect(b))}, except {string.Join(",", a.Except(b))}");

// Expected output should be:
// alice, Carol, Dave
// Contains Carol: True, index of Dave: 2
// apple     8
// banana   12
// pear      0
// No kiwi in stock
// the=3, and=2, bat=1, cat=1, hat=1
// 8 words, 5 unique
// union 1,2,3,4,5, intersect 3,4, except 1,2
