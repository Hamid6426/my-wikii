// Lesson 39: Lambdas and Functional Interfaces (../39_lambdas_and_functional_interfaces.md)
// Lambdas and Functional Interfaces
// Run: java 39-01_lambdas_and_functional_interfaces.java

import java.util.List;
import java.util.function.*;

public class Main {
    public static void main(String[] args) {
        Function<Integer, Integer> plusOne = x -> x + 1;
        Function<Integer, Integer> twice = x -> x * 2;
        System.out.println(plusOne.andThen(twice).apply(3));

        Predicate<String> shortWord = s -> s.length() < 4;
        List<String> words = List.of("sun", "planet", "sky", "galaxy");
        for (String w : words) {
            if (shortWord.negate().test(w)) System.out.println("long: " + w);
        }

        Supplier<List<String>> make = () -> List.of("x", "y");
        make.get().forEach(System.out::println);

        int base = 10;
        IntUnaryOperator addBase = n -> n + base;
        System.out.println(addBase.applyAsInt(5));
    }
}
