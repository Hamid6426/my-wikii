// Lesson 21: Strings (../21_strings.md)
// Common string operations
// Run: dotnet run 21-01_string_tools.cs

string text = "  The quick brown fox  ";

Console.WriteLine($"[{text.Trim()}]");
Console.WriteLine(text.Trim().ToUpper());
Console.WriteLine(text.Contains("quick"));
Console.WriteLine(text.Trim().Replace("quick", "slow"));
Console.WriteLine(text.Trim().Substring(4, 5));
Console.WriteLine(text.Trim().IndexOf("brown"));
Console.WriteLine(string.Join("|", text.Trim().Split(' ')));

string word = "level";
bool palindrome = word.SequenceEqual(word.Reverse());
Console.WriteLine($"{word} palindrome? {palindrome}");

string sentence = "the rain in spain";
string title = string.Join(" ", sentence.Split(' ').Select(w => char.ToUpper(w[0]) + w[1..]));
Console.WriteLine(title);

var counts = sentence.Where(char.IsLetter).GroupBy(c => c).OrderByDescending(g => g.Count()).ThenBy(g => g.Key).Take(3);
Console.WriteLine(string.Join(", ", counts.Select(g => $"{g.Key}:{g.Count()}")));

var sb = new System.Text.StringBuilder();
for (int i = 1; i <= 5; i++) sb.Append(i).Append(',');
sb.Length--;
Console.WriteLine(sb);

string path = @"C:\temp\file.txt";
string json = """{"name": "raw string"}""";
Console.WriteLine(path);
Console.WriteLine(json);

// Expected output should be:
// [The quick brown fox]
// THE QUICK BROWN FOX
// True
// The slow brown fox
// quick
// 10
// The|quick|brown|fox
// level palindrome? True
// The Rain In Spain
// i:3, n:3, a:2
// 1,2,3,4,5
// C:\temp\file.txt
// {"name": "raw string"}
