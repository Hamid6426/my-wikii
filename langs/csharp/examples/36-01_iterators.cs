// Lesson 36: Iterators (../36_iterators.md)
// yield return, lazy sequences, custom enumerables
// Run: dotnet run 36-01_iterators.cs

foreach (int n in Fibonacci().Take(10))
{
    Console.Write($"{n} ");
}
Console.WriteLine();

foreach (int n in Range(10, 40, 10))
{
    Console.Write($"{n} ");
}
Console.WriteLine();

var lazy = Noisy();
Console.WriteLine("Sequence created, nothing has run yet");
foreach (int n in lazy)
{
    Console.WriteLine($"got {n}");
}

var deck = new Deck();
foreach (string card in deck) Console.Write($"{card} ");
Console.WriteLine();

static IEnumerable<long> Fibonacci()
{
    long a = 0, b = 1;
    while (true)
    {
        yield return a;
        (a, b) = (b, a + b);
    }
}

static IEnumerable<int> Range(int start, int end, int step)
{
    for (int i = start; i <= end; i += step) yield return i;
}

static IEnumerable<int> Noisy()
{
    Console.WriteLine("  start");
    yield return 1;
    Console.WriteLine("  middle");
    yield return 2;
    Console.WriteLine("  end");
}

class Deck : IEnumerable<string>
{
    public IEnumerator<string> GetEnumerator()
    {
        foreach (string suit in new[] { "S", "H" })
            foreach (string rank in new[] { "A", "K", "Q" })
                yield return rank + suit;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}

// Expected output should be:
// 0 1 1 2 3 5 8 13 21 34
// 10 20 30 40
// Sequence created, nothing has run yet
//   start
// got 1
//   middle
// got 2
//   end
// AS KS QS AH KH QH
