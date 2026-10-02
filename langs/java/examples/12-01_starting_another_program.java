// Lesson 12: Math, Random and Utility Classes (../12_math_random_and_utilities.md)
// Starting another program
// Run: java 12-01_starting_another_program.java

import java.io.IOException;

public class Run {
    public static void main(String[] args) throws IOException, InterruptedException {
        Process process = new ProcessBuilder("java", "-version")
                .redirectErrorStream(true)           // join error output into normal output
                .start();

        String output = new String(process.getInputStream().readAllBytes());
        int exitCode = process.waitFor();

        System.out.println("Exit code: " + exitCode);
        System.out.println(output.lines().findFirst().orElse(""));
    }
}
