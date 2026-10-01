// Lesson 58: Performance Basics (../58_performance_basics.md)
// Measure code with Stopwatch and GC counters
// Run: dotnet run 58-01_measure_with_stopwatch.cs

using System.Diagnostics;
using System.Text;

static double Best(Action action, int runs = 5)
{
    action();                                     // warm up
    double best = double.MaxValue;
    for (int i = 0; i < runs; i++)
    {
        var sw = Stopwatch.StartNew();
        action();
        sw.Stop();
        best = Math.Min(best, sw.Elapsed.TotalMilliseconds);
    }
    return best;
}

double concat = Best(() => { string s = ""; for (int i = 0; i < 20_000; i++) s += "x"; });
double builder = Best(() => { var sb = new StringBuilder(); for (int i = 0; i < 20_000; i++) sb.Append('x'); _ = sb.ToString(); });
Console.WriteLine($"String +=      {concat,8:F3} ms");
Console.WriteLine($"StringBuilder  {builder,8:F3} ms   ({concat / builder:F0}x faster)");

var list = Enumerable.Range(0, 10_000).ToList();
var set = list.ToHashSet();
double listTime = Best(() => { for (int i = 0; i < 1_000; i++) list.Contains(9_999); });
double setTime = Best(() => { for (int i = 0; i < 1_000; i++) set.Contains(9_999); });
Console.WriteLine($"List.Contains  {listTime,8:F3} ms");
Console.WriteLine($"HashSet        {setTime,8:F3} ms   ({listTime / setTime:F0}x faster)");

long before = GC.GetAllocatedBytesForCurrentThread();
_ = new byte[1_000_000];
long allocated = GC.GetAllocatedBytesForCurrentThread() - before;
Console.WriteLine($"One 1 MB array allocated about {allocated / 1000} KB");
Console.WriteLine("Timings change on every machine. Run in Release for real numbers: dotnet run -c Release 58-01_measure_with_stopwatch.cs");
