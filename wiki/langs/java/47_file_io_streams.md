# 47 - File I/O: Streams

## What is an I/O Stream

An **I/O stream** is a one-way flow of data: you read from an input stream and write to an output stream. The source or target can be a file, memory, or a network connection. These are not the same as the data pipelines in [Streams](41_streams.md).

| For   | Read          | Write          |
| ----- | ------------- | -------------- |
| Bytes | `InputStream` | `OutputStream` |
| Text  | `Reader`      | `Writer`       |

All four live in `java.io`. Text classes turn bytes into characters with a charset (UTF-8 by default since Java 18).

---

## Reading and Writing Bytes in a File

```java
try (OutputStream out = Files.newOutputStream(path)) {
    out.write(new byte[] {72, 101, 108, 108, 111});   // "Hello"
}

try (InputStream in = Files.newInputStream(path)) {
    byte[] all = in.readAllBytes();                   // Java 9
    System.out.println(new String(all, StandardCharsets.UTF_8));   // Hello
}
```

`new FileInputStream(file)` and `new FileOutputStream(file)` do the same and appear in older code.

---

## Reading in Chunks

For a large file, read a block at a time. `read` returns how many bytes it got, or `-1` at the end.

```java
try (InputStream in = Files.newInputStream(path)) {
    byte[] buffer = new byte[8192];
    int count;
    while ((count = in.read(buffer)) != -1) {
        process(buffer, count);   // only the first `count` bytes are new
    }
}
```

---

## Memory Streams

`ByteArrayOutputStream` collects bytes in memory. `ByteArrayInputStream` reads from a byte array. No file needed, which is handy for tests.

```java
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.PrintStream;
import java.nio.charset.StandardCharsets;

public class MemoryStreams {
    public static void main(String[] args) throws IOException {
        var buffer = new ByteArrayOutputStream();
        try (var out = new PrintStream(buffer, true, StandardCharsets.UTF_8)) {
            out.println("In-memory data");
        }

        byte[] bytes = buffer.toByteArray();
        System.out.println(bytes.length + " bytes");

        InputStream in = new ByteArrayInputStream(bytes);
        System.out.print(new String(in.readAllBytes(), StandardCharsets.UTF_8));
    }
}
```

Expected output should be:

```
15 bytes
In-memory data
```

---

## Wrapping Streams

Streams are built in layers. Each wrapper adds one feature to the stream inside it. Closing the outer one closes them all.

```java
try (var reader = new BufferedReader(                 // adds readLine() and a buffer
        new InputStreamReader(                        // bytes to characters
            Files.newInputStream(path),               // bytes from a file
            StandardCharsets.UTF_8))) {
    System.out.println(reader.readLine());
}
```

| Wrapper                                       | Adds                                               |
| --------------------------------------------- | -------------------------------------------------- |
| `BufferedInputStream`, `BufferedOutputStream` | A buffer, so fewer slow calls to the disk          |
| `BufferedReader`, `BufferedWriter`            | A buffer, plus `readLine()` and `newLine()`        |
| `InputStreamReader`, `OutputStreamWriter`     | Bytes to text and back, with a charset             |
| `DataInputStream`, `DataOutputStream`         | Read and write `int`, `double`, and so on as bytes |
| `GZIPInputStream`, `GZIPOutputStream`         | Compression (`java.util.zip`)                      |
| `PrintStream`, `PrintWriter`                  | `println` and `printf`                             |

`Files.newBufferedReader` in [File I/O: Read and Write](45_file_io_read_write.md) builds the same chain for you.

---

## Data and GZIP Streams

```java
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.util.zip.GZIPInputStream;
import java.util.zip.GZIPOutputStream;

public class DataAndGzip {
    public static void main(String[] args) throws IOException {
        var bytes = new ByteArrayOutputStream();

        try (var out = new DataOutputStream(new GZIPOutputStream(bytes))) {
            out.writeInt(42);
            out.writeDouble(3.5);
            out.writeUTF("hello");
        }

        try (var in = new DataInputStream(new GZIPInputStream(new ByteArrayInputStream(bytes.toByteArray())))) {
            System.out.println(in.readInt());
            System.out.println(in.readDouble());
            System.out.println(in.readUTF());
        }
    }
}
```

Expected output should be:

```
42
3.5
hello
```

Read values back in the same order and with the same types you wrote them. A data stream does not record what each value was.

---

## Copying a Stream

`transferTo` (Java 9) copies everything from an input to an output.

```java
try (InputStream in = Files.newInputStream(source);
     OutputStream out = Files.newOutputStream(target)) {
    long copied = in.transferTo(out);
}
```

For whole files, `Files.copy(source, target)` is shorter. `Files.copy(inputStream, path)` saves any input stream, such as a download, to a file.

---

## Random Access

Plain streams only go forward. A `FileChannel` can jump to any **position** (a byte offset from the start of the file).

```java
try (FileChannel ch = FileChannel.open(path, StandardOpenOption.READ, StandardOpenOption.WRITE)) {
    ch.position(6);                                  // jump to byte 6
    ch.write(ByteBuffer.wrap("Java".getBytes()));    // overwrite 4 bytes there

    ch.position(0);
    ByteBuffer buf = ByteBuffer.allocate((int) ch.size());
    ch.read(buf);
    System.out.println(new String(buf.array()));
}
```

If the file held `Hello World`, this prints `Hello Javad`. Older code uses `RandomAccessFile` with `seek(position)` for the same job.

---

## Standard Streams

The console is a set of streams too.

| Field        | Type          | Use            |
| ------------ | ------------- | -------------- |
| `System.in`  | `InputStream` | Keyboard input |
| `System.out` | `PrintStream` | Normal output  |
| `System.err` | `PrintStream` | Error messages |

See [Console Input and Output](05_console_input_and_output.md).

---

## Stream Hierarchy

```
InputStream                      OutputStream
├── FileInputStream              ├── FileOutputStream
├── ByteArrayInputStream         ├── ByteArrayOutputStream
└── FilterInputStream            └── FilterOutputStream
    ├── BufferedInputStream          ├── BufferedOutputStream
    ├── DataInputStream              ├── DataOutputStream
    └── GZIPInputStream (via         ├── PrintStream
        InflaterInputStream)         └── GZIPOutputStream (via
                                         DeflaterOutputStream)

Reader                           Writer
├── BufferedReader               ├── BufferedWriter
├── InputStreamReader            ├── OutputStreamWriter
│   └── FileReader               │   └── FileWriter
└── StringReader                 ├── PrintWriter
                                 └── StringWriter
```

---

## Gotchas

- Always close streams with try-with-resources. An unclosed output stream may never write its last buffered bytes
- A `GZIPOutputStream` writes its ending only on `close()` or `finish()`. Reading before that fails with `EOFException`
- `read(buffer)` may fill less than the whole buffer even before the end. Always use the count it returns
- `new String(bytes)` and `getBytes()` without a charset use the default (UTF-8 since Java 18). Pass a charset when the data comes from elsewhere
- `FileReader` and `FileWriter` in older code used the system charset before Java 18. Prefer `Files.newBufferedReader` and `Files.newBufferedWriter`
- `ByteArrayOutputStream` keeps everything in memory, so it is not for very large data

---

## Examples

- [47-01](examples/47-01_memory_streams.java): Memory streams
- [47-02](examples/47-02_data_and_gzip_streams.java): Data and gzip streams

Run one with `java examples/47-01_memory_streams.java`. See [Examples](examples/README.md).
