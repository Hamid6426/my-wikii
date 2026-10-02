// Lesson 33: AutoCloseable and try-with-resources (../33_autocloseable.md)
// Suppressed exceptions
// Run: java 33-01_suppressed_exceptions.java

public class Main {
    public static void main(String[] args) {
        try (var a = new Res("A"); var b = new Res("B")) {
            System.out.println("body");
            throw new IllegalStateException("body failed");
        } catch (IllegalStateException e) {
            System.out.println("caught: " + e.getMessage());
            for (Throwable s : e.getSuppressed()) {
                System.out.println("suppressed: " + s.getMessage());
            }
        }
    }
}

class Res implements AutoCloseable {
    private final String name;

    Res(String name) {
        this.name = name;
        System.out.println("open " + name);
    }

    @Override
    public void close() {
        System.out.println("close " + name);
        throw new RuntimeException("close " + name + " failed");
    }
}
