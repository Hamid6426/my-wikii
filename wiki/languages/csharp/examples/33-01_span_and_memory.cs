// Lesson 33: Memory and Garbage Collection (../33_memory_and_garbage_collection.md)
// Span, stackalloc, Lazy, and allocation counting
// Run: dotnet run 33-01_span_and_memory.cs

string date = "2026-10-01";

ReadOnlySpan<char> year = date.AsSpan(0, 4);
ReadOnlySpan<char> month = date.AsSpan(5, 2);
Console.WriteLine($"year {int.Parse(year)}, month {int.Parse(month)}");

int[] numbers = { 1, 2, 3, 4, 5 };
Span<int> middle = numbers.AsSpan(1, 3);
middle[0] = 99;
Console.WriteLine(string.Join(",", numbers));

Span<int> onStack = stackalloc int[4];
for (int i = 0; i < onStack.Length; i++) onStack[i] = i * i;
Console.WriteLine(string.Join(",", onStack.ToArray()));

// Warm up so one-time costs are not counted, then compare allocations
_ = int.Parse(date.Substring(0, 4));
_ = int.Parse(date.AsSpan(0, 4));

long Measure(Action action)
{
    long before = GC.GetAllocatedBytesForCurrentThread();
    action();
    return GC.GetAllocatedBytesForCurrentThread() - before;
}

Console.WriteLine($"Substring + Parse allocated {Measure(() => int.Parse(date.Substring(0, 4)))} bytes");
Console.WriteLine($"AsSpan + Parse allocated {Measure(() => int.Parse(date.AsSpan(0, 4)))} bytes");

var report = new Lazy<string>(() =>
{
    Console.WriteLine("  (building the report)");
    return "report text";
});
Console.WriteLine("Lazy created");
Console.WriteLine(report.Value);
Console.WriteLine(report.Value);

// Expected output should be:
// year 2026, month 10
// 1,99,3,4,5
// 0,1,4,9
// Substring + Parse allocated 32 bytes
// AsSpan + Parse allocated 0 bytes
// Lazy created
//   (building the report)
// report text
// report text
