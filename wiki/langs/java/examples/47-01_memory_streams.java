// Lesson 47: File I/O: Streams (../47_file_io_streams.md)
// Memory streams
// Run: java 47-01_memory_streams.java

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
