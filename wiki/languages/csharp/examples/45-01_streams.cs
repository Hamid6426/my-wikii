// Lesson 45: File I/O: Streams (../45_file_io_streams.md)
// FileStream, MemoryStream, and copying
// Run: dotnet run 45-01_streams.cs

string path = Path.Combine(Path.GetTempPath(), "csharp-example-45.bin");

using (var output = new FileStream(path, FileMode.Create, FileAccess.Write))
{
    byte[] data = { 10, 20, 30, 40, 50 };
    output.Write(data, 0, data.Length);
}

using (var input = new FileStream(path, FileMode.Open, FileAccess.Read))
{
    Console.WriteLine($"Length {input.Length}, position {input.Position}");
    input.Seek(2, SeekOrigin.Begin);
    Console.WriteLine($"Byte at 2: {input.ReadByte()}");
}

using var memory = new MemoryStream();
using (var writer = new StreamWriter(memory, leaveOpen: true))
{
    writer.Write("stored in memory, not on disk");
}
Console.WriteLine($"{memory.Length} bytes in memory");

memory.Position = 0;
Console.WriteLine(new StreamReader(memory).ReadToEnd());

using (var source = File.OpenRead(path))
using (var copy = new MemoryStream())
{
    await source.CopyToAsync(copy);
    Console.WriteLine($"Copied {copy.Length} bytes: {string.Join(",", copy.ToArray())}");
}

File.Delete(path);

// Expected output should be:
// Length 5, position 0
// Byte at 2: 30
// 29 bytes in memory
// stored in memory, not on disk
// Copied 5 bytes: 10,20,30,40,50
