// Lesson 61: C# Version History (../61_csharp_version_history.md)
// One feature from each C# version
// Run: dotnet run 61-01_features_by_version.cs

using System.Numerics;

// C# 3: LINQ, lambdas, var
var evens = Enumerable.Range(1, 10).Where(n => n % 2 == 0);
Console.WriteLine($"C# 3  LINQ:                {string.Join(",", evens)}");

// C# 5: async/await
Console.WriteLine($"C# 5  async/await:         {await Task.FromResult("done")}");

// C# 6: string interpolation, ?. and nameof
string? text = null;
Console.WriteLine($"C# 6  null-conditional:    {text?.Length ?? -1}, nameof gives {nameof(text)}");

// C# 7: tuples and out variables
var (q, r) = Math.DivRem(17, 5);
Console.WriteLine($"C# 7  tuples:              17 / 5 = {q} remainder {r}");

// C# 8: ranges and switch expressions
string word = "multiverse";
Console.WriteLine($"C# 8  ranges:              {word[5..]}, {word[..5]}");

// C# 9: records and target-typed new
Person p = new("Ada", 36);
Console.WriteLine($"C# 9  records:             {p with { Age = 37 }}");

// C# 10: file-scoped namespaces and global usings (this file uses implicit usings)
Console.WriteLine("C# 10 implicit usings:     no 'using System;' needed in this file");

// C# 11: raw strings and list patterns
string json = """{"language": "C#"}""";
int[] nums = { 1, 2, 3 };
Console.WriteLine($"C# 11 raw string, list:    {json}, {nums is [1, .., 3]}");

// C# 12: primary constructors and collection expressions
int[] merged = [..nums, 4, 5];
Console.WriteLine($"C# 12 collection expr:     {string.Join(",", merged)}");

// C# 13: params collections
Console.WriteLine($"C# 13 params span:         {Sum(1, 2, 3)}");

// C# 14: field keyword
var t = new Temperature { Celsius = -300 };
Console.WriteLine($"C# 14 field keyword:       {t.Celsius}");

static int Sum(params ReadOnlySpan<int> values)
{
    int total = 0;
    foreach (int v in values) total += v;
    return total;
}

record Person(string Name, int Age);

class Temperature
{
    public double Celsius
    {
        get;
        set => field = Math.Max(value, -273.15);
    }
}

// Expected output should be:
// C# 3  LINQ:                2,4,6,8,10
// C# 5  async/await:         done
// C# 6  null-conditional:    -1, nameof gives text
// C# 7  tuples:              17 / 5 = 3 remainder 2
// C# 8  ranges:              verse, multi
// C# 9  records:             Person { Name = Ada, Age = 37 }
// C# 10 implicit usings:     no 'using System;' needed in this file
// C# 11 raw string, list:    {"language": "C#"}, True
// C# 12 collection expr:     1,2,3,4,5
// C# 13 params span:         6
// C# 14 field keyword:       -273.15
