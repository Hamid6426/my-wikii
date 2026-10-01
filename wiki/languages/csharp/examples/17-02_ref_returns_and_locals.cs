// Lesson 17: Parameters (../17_params.md)
// ref locals, ref returns, and ref readonly
// Run: dotnet run 17-02_ref_returns_and_locals.cs

using System.Runtime.InteropServices;

int[] scores = { 10, 20, 30 };

ref int best = ref FindMax(scores);        // ref local: an alias of the array element
best = 99;                                 // changes scores[2]
Console.WriteLine(string.Join(",", scores));

ref int first = ref scores[0];
first += 5;
Console.WriteLine(string.Join(",", scores));

int copy = FindMax(scores);                // without 'ref', the value is copied
copy = -1;
Console.WriteLine(string.Join(",", scores));

var big = new Big { Values = { [0] = 1 } };
ref readonly int view = ref big.First();    // read-only alias: no copy, no changes allowed
Console.WriteLine(view);

var counts = new Dictionary<string, int>();
foreach (string word in "a b a c a b".Split(' '))
{
    ref int count = ref CollectionsMarshal.GetValueRefOrAddDefault(counts, word, out _);
    count++;                               // one lookup instead of two
}
Console.WriteLine(string.Join(", ", counts.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key}={kv.Value}")));

static ref int FindMax(int[] array)
{
    int index = 0;
    for (int i = 1; i < array.Length; i++)
    {
        if (array[i] > array[index]) index = i;
    }
    return ref array[index];
}

class Big
{
    public int[] Values { get; } = new int[4];
    public ref readonly int First() => ref Values[0];
}

// Expected output should be:
// 10,20,99
// 15,20,99
// 15,20,99
// 1
// a=3, b=2, c=1
