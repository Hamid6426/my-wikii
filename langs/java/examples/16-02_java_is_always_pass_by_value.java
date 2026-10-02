// Lesson 16: Methods (../16_methods.md)
// Java is always pass by value
// Run: java 16-02_java_is_always_pass_by_value.java

import java.util.ArrayList;
import java.util.List;

public class PassByValue {
    static void doubleIt(int x) {
        x *= 2;                      // changes only the local copy
    }

    static void addItem(List<String> list) {
        list.add("new");             // changes the shared object
    }

    static void replace(List<String> list) {
        list = new ArrayList<>();    // points the local copy somewhere else
        list.add("lost");
    }

    public static void main(String[] args) {
        int n = 5;
        doubleIt(n);
        System.out.println(n);

        List<String> items = new ArrayList<>();
        addItem(items);
        replace(items);
        System.out.println(items);
    }
}
