// Lesson 26: Classes and Objects (../26_classes_and_objects.md)
// Classes and Objects
// Run: java 26-01_classes_and_objects.java

public class Bank {
    static class Account {
        private final String owner;
        private double balance;

        Account(String owner, double opening) {
            if (opening < 0) throw new IllegalArgumentException("opening < 0");
            this.owner = owner;
            this.balance = opening;
        }

        Account(String owner) {
            this(owner, 0);
        }

        void deposit(double amount) {
            if (amount <= 0) throw new IllegalArgumentException("amount <= 0");
            balance += amount;
        }

        boolean withdraw(double amount) {
            if (amount > balance) return false;
            balance -= amount;
            return true;
        }

        double getBalance() {
            return balance;
        }

        @Override
        public String toString() {
            return "%s: %.2f".formatted(owner, balance);
        }
    }

    public static void main(String[] args) {
        Account a = new Account("Alice", 100);
        Account b = new Account("Bob");

        a.deposit(50);
        System.out.println(a.withdraw(500));
        System.out.println(a.withdraw(30));
        b.deposit(20);

        System.out.println(a);
        System.out.println(b);
        System.out.println(a.getBalance() + b.getBalance());
    }
}
