// Lesson 22: Reference Data Types (../22_reference_data_types.md)
// Classes are shared primitives are copied
// Run: java 22-01_classes_are_shared_primitives_are_copied.java

public class References {
    static class Person {
        String name;
        Person(String name) { this.name = name; }
    }

    public static void main(String[] args) {
        int a = 1;
        int b = a;          // copy of the value
        b = 99;
        System.out.println(a);

        Person p1 = new Person("Alice");
        Person p2 = p1;     // copy of the reference: same object
        p2.name = "Bob";
        System.out.println(p1.name);

        Person p3 = new Person("Bob");
        System.out.println(p1 == p3);   // different objects
        System.out.println(p1 == p2);   // same object
    }
}
