// Lesson 47: File I/O: Streams (../47_file_io_streams.md)
// Data and gzip streams
// Run: java 47-02_data_and_gzip_streams.java

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
