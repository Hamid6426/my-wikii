// Lesson 05: Console Input and Output (../05_console_input_and_output.md)
// Reading many lines fast
// Run: java 05-01_reading_many_lines_fast.java

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

public class Sum {
    public static void main(String[] args) throws IOException {
        var reader = new BufferedReader(new InputStreamReader(System.in));

        String line;
        int total = 0;
        while ((line = reader.readLine()) != null) {   // null when input ends
            total += Integer.parseInt(line.trim());
        }

        System.out.println("Total: " + total);
    }
}
