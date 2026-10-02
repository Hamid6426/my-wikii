// Lesson 25: Records and Equality (../25_records_and_equality.md)
// Records in action
// Run: java 25-01_records_in_action.java

import java.util.HashSet;
import java.util.Set;

public class Records {
    record Person(String name, int age) {}

    public static void main(String[] args) {
        Person p1 = new Person("Alice", 30);
        Person p2 = new Person("Alice", 30);

        System.out.println(p1);
        System.out.println(p1.name() + " is " + p1.age());
        System.out.println(p1 == p2);        // different objects
        System.out.println(p1.equals(p2));   // same values

        Set<Person> people = new HashSet<>();
        people.add(p1);
        people.add(p2);
        System.out.println(people.size());
    }
}
