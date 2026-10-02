// Lesson 29: equals, hashCode and Comparable (../29_equals_hashcode_and_comparable.md)
// equals, hashCode and Comparable
// Run: java 29-01_equals_hashcode_and_comparable.java

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Objects;
import java.util.Set;

public class Main {
    public static void main(String[] args) {
        Money a = new Money(500, "USD");
        Money b = new Money(500, "USD");
        System.out.println("== " + (a == b));
        System.out.println("equals " + a.equals(b));

        Set<Money> set = new HashSet<>();
        set.add(a);
        System.out.println("contains " + set.contains(b));

        List<Money> list = new ArrayList<>(List.of(
            new Money(900, "USD"), new Money(100, "USD"), new Money(500, "USD")));
        list.sort(null);                   // null means natural order
        System.out.println(list);
        System.out.println(a.plus(b));
    }
}

final class Money implements Comparable<Money> {
    private final long cents;
    private final String currency;

    Money(long cents, String currency) {
        this.cents = cents;
        this.currency = currency;
    }

    Money plus(Money other) {
        if (!currency.equals(other.currency)) throw new IllegalArgumentException("currency");
        return new Money(cents + other.cents, currency);
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (!(o instanceof Money other)) return false;
        return cents == other.cents && currency.equals(other.currency);
    }

    @Override
    public int hashCode() {
        return Objects.hash(cents, currency);
    }

    @Override
    public int compareTo(Money other) {
        return Long.compare(cents, other.cents);
    }

    @Override
    public String toString() {
        return currency + " " + cents / 100 + "." + String.format("%02d", cents % 100);
    }
}
