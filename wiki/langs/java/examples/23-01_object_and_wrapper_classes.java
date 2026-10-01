// Lesson 23: Object and Wrapper Classes (../23_object_and_wrapper_classes.md)
// Object and wrapper classes
// Run: java 23-01_object_and_wrapper_classes.java

public class Wrappers {
    public static void main(String[] args) {
        Integer a = 127, b = 127;
        Integer c = 1000, d = 1000;
        System.out.println(a == b);        // cached
        System.out.println(c == d);        // not cached
        System.out.println(c.equals(d));   // same value

        System.out.println(Integer.parseInt("ff", 16));
        System.out.println(Integer.toBinaryString(5));

        Object o = new Wrappers();
        System.out.println(o.getClass().getSimpleName());
        System.out.println(o instanceof Object);
    }
}
