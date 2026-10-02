// Lesson 54: Generics (../54_generics.md)
// Generic class
// Run: java 54-01_generic_class.java

import java.util.function.Function;

public class Generics {
    static class Box<T> {
        private final T value;

        Box(T value) { this.value = value; }

        T get() { return value; }

        <R> Box<R> map(Function<T, R> fn) {     // a generic method inside a generic class
            return new Box<>(fn.apply(value));
        }
    }

    record Pair<A, B>(A first, B second) {}     // records can be generic too

    public static void main(String[] args) {
        Box<String> box = new Box<>("hello");
        System.out.println(box.get().toUpperCase());
        System.out.println(box.map(String::length).get());

        var pair = new Pair<>("Alice", 30);
        System.out.println(pair.first() + ": " + pair.second());
        System.out.println(pair);
    }
}
