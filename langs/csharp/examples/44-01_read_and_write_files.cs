// Lesson 44: File I/O: Read & Write (../44_file_io_read_write.md)
// File, StreamReader, StreamWriter, and Path
// Run: dotnet run 44-01_read_and_write_files.cs

string folder = Path.Combine(Path.GetTempPath(), "csharp-example-44");
Directory.CreateDirectory(folder);
string file = Path.Combine(folder, "notes.txt");

File.WriteAllText(file, "first line\nsecond line\n");
File.AppendAllText(file, "third line\n");
Console.WriteLine(File.ReadAllText(file).TrimEnd());

string[] lines = File.ReadAllLines(file);
Console.WriteLine($"{lines.Length} lines, longest: {lines.MaxBy(l => l.Length)}");

using (var writer = new StreamWriter(file, append: false))
{
    for (int i = 1; i <= 3; i++) writer.WriteLine($"row {i}");
}

using (var reader = new StreamReader(file))
{
    string? line;
    while ((line = reader.ReadLine()) != null)
    {
        Console.WriteLine($"read: {line}");
    }
}

foreach (string l in File.ReadLines(file).Where(l => l.EndsWith('2') || l.EndsWith('3')))
{
    Console.WriteLine($"filtered: {l}");
}

Console.WriteLine(Path.GetFileName(file));
Console.WriteLine(Path.GetExtension(file));
Console.WriteLine(File.Exists(file));
File.Delete(file);
Console.WriteLine(File.Exists(file));
Directory.Delete(folder);

// Expected output should be:
// first line
// second line
// third line
// 3 lines, longest: second line
// read: row 1
// read: row 2
// read: row 3
// filtered: row 2
// filtered: row 3
// notes.txt
// .txt
// True
// False
