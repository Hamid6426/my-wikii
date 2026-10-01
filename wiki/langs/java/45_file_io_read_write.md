# 45 - File I/O: Read and Write

## Packages

Modern file code uses **NIO.2** (new I/O, Java 7): the `Path` type for a location and the `Files` class for actions.

```java
import java.nio.file.Files;
import java.nio.file.Path;
```

The older `java.io.File` class still works, but `Files` has better error messages and more features. Convert with `file.toPath()` and `path.toFile()`.

---

## Path

A `Path` is a file or folder location. Creating one does not touch the disk.

```java
Path p = Path.of("data", "notes.txt");      // data/notes.txt on Linux, data\notes.txt on Windows

p.getFileName()            // notes.txt
p.getParent()              // data
p.toAbsolutePath()         // /home/you/project/data/notes.txt
p.resolve("more.txt")      // data/notes.txt/more.txt, joins paths
p.resolveSibling("x.txt")  // data/x.txt
Path.of(System.getProperty("user.home"))   // your home folder
```

`Path.of` arrived in Java 11. Older code uses `Paths.get`, which does the same.

---

## Files: Quick Operations

One-liners for small files. They open and close the file for you.

```java
// Write (creates or overwrites)
Files.writeString(path, "Hello, World!");
Files.write(path, List.of("line1", "line2", "line3"));
Files.write(path, bytes);

// Append
Files.writeString(log, "New entry\n", StandardOpenOption.CREATE, StandardOpenOption.APPEND);

// Read
String content = Files.readString(path);
List<String> lines = Files.readAllLines(path);
byte[] bytes = Files.readAllBytes(path);
```

| `StandardOpenOption` | Meaning                                                       |
| -------------------- | ------------------------------------------------------------- |
| `CREATE`             | Create the file if it does not exist                          |
| `CREATE_NEW`         | Create the file, fail if it exists                            |
| `APPEND`             | Write at the end                                              |
| `TRUNCATE_EXISTING`  | Empty the file first. Used by default when you pass no option |

---

## A Complete Example

```java
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.StandardOpenOption;
import java.util.List;

public class ReadWrite {
    public static void main(String[] args) throws IOException {
        Path dir = Files.createTempDirectory("demo");
        Path file = dir.resolve("notes.txt");

        Files.write(file, List.of("first", "second"));
        Files.writeString(file, "third\n", StandardOpenOption.APPEND);

        List<String> lines = Files.readAllLines(file);
        System.out.println(lines);
        System.out.println(Files.size(file) + " bytes");
        System.out.println(Files.exists(file));

        Files.delete(file);
        Files.delete(dir);
        System.out.println(Files.exists(file));
    }
}
```

Expected output should be:

```
[first, second, third]
19 bytes
true
false
```

`Files.write` ends each line with the system line separator, so on Windows the size is 21 bytes.

Most `Files` methods throw `IOException`, a checked exception, so `main` declares `throws IOException`. See [Error Handling](42_error_handling.md).

---

## Reading Line by Line

`readAllLines` loads the whole file into memory. For large files, read one line at a time.

```java
try (BufferedReader reader = Files.newBufferedReader(path)) {
    String line;
    while ((line = reader.readLine()) != null) {
        System.out.println(line);
    }
}
```

Or as a stream (see [Streams](41_streams.md)):

```java
try (Stream<String> lines = Files.lines(path)) {
    long errors = lines.filter(l -> l.contains("ERROR")).count();
}
```

Both need try-with-resources so the file is closed. See [AutoCloseable and try-with-resources](33_autocloseable.md).

---

## Writing Line by Line

```java
try (BufferedWriter writer = Files.newBufferedWriter(path)) {
    writer.write("First line");
    writer.newLine();
    writer.write("Second line");
    writer.newLine();
}

// Append mode
try (BufferedWriter writer = Files.newBufferedWriter(log, StandardOpenOption.CREATE, StandardOpenOption.APPEND)) {
    writer.write("[" + LocalDateTime.now() + "] Event occurred");
    writer.newLine();
}
```

`PrintWriter` wraps a writer and adds `println` and `printf`:

```java
try (var out = new PrintWriter(Files.newBufferedWriter(path))) {
    out.printf("%s is %d%n", "Alice", 30);
}
```

---

## File Information

```java
Files.exists(path)
Files.isDirectory(path)
Files.isReadable(path)
Files.size(path)                     // bytes
Files.getLastModifiedTime(path)      // a FileTime
```

---

## Copy, Move, and Delete

```java
Files.copy(source, target);                                          // FileAlreadyExistsException if target exists
Files.copy(source, target, StandardCopyOption.REPLACE_EXISTING);
Files.move(oldPath, newPath, StandardCopyOption.REPLACE_EXISTING);   // also renames

Files.delete(path);              // throws NoSuchFileException if missing
Files.deleteIfExists(path);      // returns false if missing
```

---

## Directories

```java
Files.createDirectories(Path.of("logs/2026"));   // creates every missing folder

try (Stream<Path> entries = Files.list(Path.of("."))) {          // one level
    entries.forEach(System.out::println);
}

try (Stream<Path> all = Files.walk(Path.of("src"))) {             // every level
    List<Path> javaFiles = all.filter(p -> p.toString().endsWith(".java")).toList();
}

Path temp = Files.createTempFile("report", ".txt");              // unique temp file
```

`Files.delete` on a folder works only when it is empty. To delete a tree, walk it and delete the deepest paths first.

---

## Reading a CSV Example

```java
try (Stream<String> lines = Files.lines(Path.of("data.csv"))) {
    lines.skip(1)                                  // skip the header row
         .map(line -> line.split(","))
         .forEach(parts -> System.out.println("Name: " + parts[0] + ", Age: " + parts[1]));
}
```

A real CSV can have commas inside quotes. Use a CSV library for outside data.

---

## Encoding

Since Java 18 the default **charset** (how characters turn into bytes) is UTF-8 on every system. The `Files` methods always used UTF-8. Pass a charset to use another:

```java
String text = Files.readString(path, StandardCharsets.ISO_8859_1);
Files.writeString(path, text, StandardCharsets.UTF_8);
```

---

## Gotchas

- `readAllLines` and `readString` load the whole file into memory. Use `newBufferedReader` or `Files.lines` for large files
- `Files.lines`, `Files.list`, and `Files.walk` keep the file or folder open. Always close them with try-with-resources
- A relative path is relative to the **working directory** (where you ran `java`), not to the source file
- `Files.readString` throws `MalformedInputException` if the file is not valid UTF-8. Pass the right charset
- On Windows, deleting or moving a file that is still open fails. Close it first

---

## Examples

- [45-01](examples/45-01_file_io_read_write.java): File I/O: Read and Write

Run one with `java examples/45-01_file_io_read_write.java`. See [Examples](examples/README.md).
