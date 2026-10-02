// Lesson 27: Static Members and Utility Classes (../27_static_members_and_utility_classes.md)
// Static members and utility classes
// Run: java 27-01_static_members_and_utility_classes.java

public class Main {
    public static void main(String[] args) {
        new Counter();
        new Counter();
        System.out.println("Counters: " + Counter.total);

        System.out.println("100C is " + Temperature.toFahrenheit(100) + "F");
        System.out.println("square(5) = " + MathUtil.square(5));
    }
}

class Counter {
    static int total;
    Counter() { total++; }
}

class Temperature {
    static double toFahrenheit(double celsius) { return celsius * 9 / 5 + 32; }
}

final class MathUtil {
    private MathUtil() { }
    static int square(int x) { return x * x; }
}
