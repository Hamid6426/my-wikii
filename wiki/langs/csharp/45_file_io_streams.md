# 45 - File I/O: Streams

## What is a Stream

An abstraction over a sequence of bytes: reading from or writing to files, network connections, memory, etc. All streams derive from `System.IO.Stream`.

---

## FileStream

Low-level byte-level access to files.

```csharp
using var fs = new FileStream("data.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite);

// Write bytes
byte[] data = { 72, 101, 108, 108, 111 };  // "Hello"
fs.Write(data, 0, data.Length);

// Seek back to start
fs.Seek(0, SeekOrigin.Begin);

// Read bytes
byte[] buffer = new byte[fs.Length];
fs.Read(buffer, 0, buffer.Length);
Console.WriteLine(System.Text.Encoding.UTF8.GetString(buffer));
```

---

## FileMode Options

| Mode           | Behavior                                  |
| -------------- | ----------------------------------------- |
| `Create`       | Create or overwrite                       |
| `CreateNew`    | Create only: fails if file exists         |
| `Open`         | Open existing: fails if not found         |
| `OpenOrCreate` | Open if exists, create if not             |
| `Append`       | Open and seek to end; create if not found |
| `Truncate`     | Open and truncate to zero length          |

---

## MemoryStream

Stream backed by in-memory byte array: no file needed.

```csharp
using var ms = new MemoryStream();

using var writer = new StreamWriter(ms, leaveOpen: true);
writer.WriteLine("In-memory data");
writer.Flush();

ms.Seek(0, SeekOrigin.Begin);

using var reader = new StreamReader(ms);
string content = reader.ReadToEnd();
Console.WriteLine(content);
```

---

## BufferedStream

Wraps another stream to add buffering: reduces the number of read/write calls.

```csharp
using var fs = new FileStream("large.bin", FileMode.Open);
using var bs = new BufferedStream(fs, bufferSize: 4096);

byte[] buf = new byte[4096];
int bytesRead;
while ((bytesRead = bs.Read(buf, 0, buf.Length)) > 0)
{
    // process buf[0..bytesRead]
}
```

---

## CryptoStream

Wraps a stream with encryption/decryption.

```csharp
using System.Security.Cryptography;

using var aes = Aes.Create();
using var fs  = new FileStream("encrypted.bin", FileMode.Create);
using var cs  = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write);
using var sw  = new StreamWriter(cs);

sw.WriteLine("Secret message");
```

---

## Async Stream Operations

All stream read/write methods have async versions.

```csharp
using var fs = new FileStream("large.txt", FileMode.Open, FileAccess.Read,
    FileShare.Read, bufferSize: 4096, useAsync: true);

byte[] buffer = new byte[4096];
int bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length);
```

---

## Stream Copying

```csharp
using var input  = new FileStream("source.dat", FileMode.Open);
using var output = new FileStream("copy.dat",   FileMode.Create);

await input.CopyToAsync(output);  // async copy, good for large files
```

---

## Stream Properties

```csharp
Stream s = new FileStream("file.dat", FileMode.Open);

s.CanRead;      // true
s.CanWrite;     // false if read-only
s.CanSeek;      // true for FileStream, false for NetworkStream
s.Length;       // total length in bytes
s.Position;     // current cursor position
```

---

## Stream Hierarchy

```
Stream
├── FileStream
├── MemoryStream
├── NetworkStream
├── CryptoStream
└── BufferedStream

TextReader / TextWriter (wrap streams for text)
├── StreamReader / StreamWriter
└── StringReader / StringWriter
```

---

## Gotchas

- Always `Flush()` or `Dispose()` (`using`) to ensure buffered data is written to disk
- `MemoryStream` grows as needed but is not suitable for very large data (lives in RAM)
- `NetworkStream.CanSeek` is `false`: you cannot seek on a network stream
- `FileStream` opened without `useAsync: true` uses sync I/O even when awaited: always set it for async usage
- `CopyToAsync` is more efficient than manually looping with `Read`/`Write`

---

## Examples

- [45-01](examples/45-01_streams.cs): FileStream, MemoryStream, and copying

Run one with `dotnet run examples/45-01_streams.cs`. See [Examples](examples/README.md).
