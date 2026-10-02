// Lesson 34: Collections (../34_collections.md)
// IEnumerable, IReadOnlyList, IList, and API design
// Run: dotnet run 34-02_collection_interfaces.cs

var library = new Library();
library.Add("Dune");
library.Add("Emma");

IReadOnlyList<string> titles = library.Titles;      // callers can read, index, and count
Console.WriteLine($"{titles.Count} titles, first is {titles[0]}");

// titles.Add("x");                                 // does not compile: no Add on IReadOnlyList

// A caller can still cast back, so return a real read-only wrapper when it matters
if (titles is List<string> sneaky)
{
    sneaky.Add("Sneaked in");
}
Console.WriteLine($"After the cast trick: {library.Titles.Count} titles (the library was changed!)");

IReadOnlyList<string> safe = library.SafeTitles;
Console.WriteLine($"Safe wrapper is a List? {safe is List<string>}");

Console.WriteLine(Total(new List<int> { 1, 2, 3 }));
Console.WriteLine(Total(new[] { 4, 5 }));
Console.WriteLine(Total(Enumerable.Range(1, 4)));

IReadOnlyDictionary<string, int> ages = new Dictionary<string, int> { ["Ann"] = 30 };
Console.WriteLine(ages["Ann"]);

static int Total(IEnumerable<int> numbers) => numbers.Sum();   // accept the widest type you can

class Library
{
    private readonly List<string> _titles = [];

    public void Add(string title) => _titles.Add(title);

    public IReadOnlyList<string> Titles => _titles;                  // exposes the list itself
    public IReadOnlyList<string> SafeTitles => _titles.AsReadOnly();  // wrapper: cannot be cast back
}

// Expected output should be:
// 2 titles, first is Dune
// After the cast trick: 3 titles (the library was changed!)
// Safe wrapper is a List? False
// 6
// 9
// 10
// 30
