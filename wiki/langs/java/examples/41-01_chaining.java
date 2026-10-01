// Lesson 41: Streams (../41_streams.md)
// Chaining
// Run: java 41-01_chaining.java

import java.util.Comparator;
import java.util.List;

public class Chaining {
    record Person(String name, int age) {}

    public static void main(String[] args) {
        var people = List.of(
            new Person("Alice", 30), new Person("Bob", 17),
            new Person("Cara", 25), new Person("Dan", 40));

        List<String> oldestAdults = people.stream()
            .filter(p -> p.age() >= 18)
            .sorted(Comparator.comparing(Person::age).reversed())
            .map(Person::name)
            .limit(2)
            .toList();

        System.out.println(oldestAdults);
    }
}
