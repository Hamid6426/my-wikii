// Lesson 45: File I/O: Read and Write (../45_file_io_read_write.md)
// File I/O: Read and Write
// Run: java 45-01_file_io_read_write.java

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
