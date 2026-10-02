// Lesson 41: Streams (../41_streams.md)
// Streams are lazy
// Run: java 41-02_streams_are_lazy.java

import java.util.stream.Stream;

public class Lazy {
    public static void main(String[] args) {
        Stream<String> pipeline = Stream.of("a", "b", "c")
            .map(s -> {
                System.out.println("map " + s);
                return s.toUpperCase();
            });

        System.out.println("nothing has run yet");

        String first = pipeline.filter(s -> !s.equals("A")).findFirst().orElse("none");
        System.out.println(first);
    }
}
