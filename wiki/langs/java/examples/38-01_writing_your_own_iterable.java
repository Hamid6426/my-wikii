// Lesson 38: Iterators and Iterable (../38_iterators.md)
// Writing your own iterable
// Run: java 38-01_writing_your_own_iterable.java

import java.util.Iterator;
import java.util.NoSuchElementException;

public class Main {
    public static void main(String[] args) {
        for (int n : new Range(1, 4)) {
            System.out.println(n);
        }
    }
}

record Range(int start, int end) implements Iterable<Integer> {
    @Override
    public Iterator<Integer> iterator() {
        return new Iterator<>() {
            private int current = start;

            @Override
            public boolean hasNext() { return current < end; }

            @Override
            public Integer next() {
                if (!hasNext()) throw new NoSuchElementException();
                return current++;
            }
        };
    }
}
