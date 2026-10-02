// Lesson 39: Extension Methods (../39_extension_methods.md)
// Extension methods on string, IEnumerable, and an enum
// Run: dotnet run 39-01_extension_methods.cs

Console.WriteLine("hello world".ToTitleCase());
Console.WriteLine("a-very-long-title".Truncate(9));
Console.WriteLine("   ".IsBlank());
Console.WriteLine("level".IsPalindrome());

var numbers = Enumerable.Range(1, 10);
Console.WriteLine(string.Join(",", numbers.Chunked(4).Select(c => $"[{string.Join(" ", c)}]")));
Console.WriteLine(numbers.Median());

Console.WriteLine(Priority.High.Label());

static class StringExtensions
{
    public static string ToTitleCase(this string s)
        => string.Join(" ", s.Split(' ').Select(w => w.Length == 0 ? w : char.ToUpper(w[0]) + w[1..]));

    public static string Truncate(this string s, int max)
        => s.Length <= max ? s : s[..max] + "...";

    public static bool IsBlank(this string? s) => string.IsNullOrWhiteSpace(s);

    public static bool IsPalindrome(this string s) => s.SequenceEqual(s.Reverse());
}

static class EnumerableExtensions
{
    public static IEnumerable<List<T>> Chunked<T>(this IEnumerable<T> source, int size)
    {
        var chunk = new List<T>(size);
        foreach (T item in source)
        {
            chunk.Add(item);
            if (chunk.Count == size)
            {
                yield return chunk;
                chunk = new List<T>(size);
            }
        }
        if (chunk.Count > 0) yield return chunk;
    }

    public static double Median(this IEnumerable<int> source)
    {
        int[] sorted = source.Order().ToArray();
        int mid = sorted.Length / 2;
        return sorted.Length % 2 == 0 ? (sorted[mid - 1] + sorted[mid]) / 2.0 : sorted[mid];
    }
}

enum Priority { Low, Medium, High }

static class PriorityExtensions
{
    public static string Label(this Priority p) => p switch
    {
        Priority.High => "Do it now",
        Priority.Medium => "Do it today",
        _ => "Do it later"
    };
}

// Expected output should be:
// Hello World
// a-very-lo...
// True
// True
// [1 2 3 4],[5 6 7 8],[9 10]
// 5.5
// Do it now
