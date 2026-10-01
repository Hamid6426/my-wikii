# 44 - File I/O: Read & Write

## Namespace

```csharp
using System.IO;
```

---

## File Class: Quick Operations

One-liners for small files.

```csharp
// Write (creates or overwrites)
File.WriteAllText("notes.txt", "Hello, World!");
File.WriteAllLines("list.txt", new[] { "line1", "line2", "line3" });
File.WriteAllBytes("data.bin", byteArray);

// Append
File.AppendAllText("log.txt", "New entry\n");

// Read
string content = File.ReadAllText("notes.txt");
string[] lines = File.ReadAllLines("list.txt");
byte[]   bytes = File.ReadAllBytes("data.bin");
```

---

## StreamReader: Reading Line by Line

Efficient for large files: does not load the whole file into memory.

```csharp
using var reader = new StreamReader("notes.txt");

string line;
while ((line = reader.ReadLine()) != null)
{
    Console.WriteLine(line);
}
```

Reading all at once with `StreamReader`:

```csharp
using var reader = new StreamReader("notes.txt");
string content = reader.ReadToEnd();
```

---

## StreamWriter: Writing

```csharp
// Create or overwrite
using var writer = new StreamWriter("output.txt");
writer.WriteLine("First line");
writer.WriteLine("Second line");

// Append mode
using var appender = new StreamWriter("log.txt", append: true);
appender.WriteLine($"[{DateTime.Now}] Event occurred");
```

---

## File Existence and Info

```csharp
bool exists = File.Exists("notes.txt");
bool dirEx  = Directory.Exists("logs");

FileInfo info = new FileInfo("notes.txt");
Console.WriteLine(info.Length);         // size in bytes
Console.WriteLine(info.LastWriteTime); // last modified
Console.WriteLine(info.FullName);      // absolute path
```

---

## File Operations

```csharp
File.Copy("source.txt", "dest.txt");
File.Copy("source.txt", "dest.txt", overwrite: true);

File.Move("old.txt", "new.txt");

File.Delete("temp.txt");
```

---

## Directory Operations

```csharp
Directory.CreateDirectory("logs/2025");  // creates all needed directories

string[] files = Directory.GetFiles(".", "*.txt");
string[] dirs  = Directory.GetDirectories(".");

Directory.Delete("temp", recursive: true);
```

---

## Path Class

Platform-safe path manipulation.

```csharp
string full   = Path.Combine("C:", "Users", "Alice", "file.txt");
string dir    = Path.GetDirectoryName(full);   // "C:\Users\Alice"
string name   = Path.GetFileName(full);        // "file.txt"
string noExt  = Path.GetFileNameWithoutExtension(full);  // "file"
string ext    = Path.GetExtension(full);       // ".txt"
string temp   = Path.GetTempFileName();        // unique temp file
```

---

## Reading a CSV Example

```csharp
foreach (string line in File.ReadLines("data.csv"))
{
    string[] parts = line.Split(',');
    Console.WriteLine($"Name: {parts[0]}, Age: {parts[1]}");
}
```

`File.ReadLines` (note: not `ReadAllLines`) streams lazily: use it for large files.

---

## Encoding

```csharp
using var reader = new StreamReader("file.txt", System.Text.Encoding.UTF8);
using var writer = new StreamWriter("out.txt", false, System.Text.Encoding.UTF8);
```

---

## Gotchas

- `File.ReadAllLines` loads everything into memory: use `File.ReadLines` for large files
- Always use `using` with `StreamReader`/`StreamWriter` to ensure the file is closed
- `Path.Combine` handles directory separators automatically: avoid string concatenation for paths
- `File.Delete` throws `IOException` if the file is locked
- Writing without a full path uses the application's working directory, which may not be what you expect

---

## Examples

- [44-01](examples/44-01_read_and_write_files.cs): File, StreamReader, StreamWriter, and Path

Run one with `dotnet run examples/44-01_read_and_write_files.cs`. See [Examples](examples/README.md).
