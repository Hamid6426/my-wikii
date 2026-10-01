// Lesson 17: Varargs (../17_varargs.md)
// What is varargs
// Run: java 17-01_what_is_varargs.java

public class Sum {
    static int sum(int... numbers) {
        int total = 0;
        for (int n : numbers) {
            total += n;
        }
        return total;
    }

    public static void main(String[] args) {
        System.out.println(sum(1, 2, 3, 4, 5));
        System.out.println(sum(7));
        System.out.println(sum());
        System.out.println(sum(new int[] {10, 20}));
    }
}
