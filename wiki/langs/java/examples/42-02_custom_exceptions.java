// Lesson 42: Error Handling (../42_error_handling.md)
// Custom exceptions
// Run: java 42-02_custom_exceptions.java

public class CustomException {
    static class InsufficientFundsException extends RuntimeException {
        private final long amount;

        InsufficientFundsException(long amount) {
            super("Insufficient funds. Required: " + amount);
            this.amount = amount;
        }

        long amount() { return amount; }
    }

    static void withdraw(long balance, long amount) {
        if (amount > balance) throw new InsufficientFundsException(amount - balance);
    }

    public static void main(String[] args) {
        try {
            withdraw(50, 80);
        } catch (InsufficientFundsException e) {
            System.out.println(e.getMessage());
            System.out.println("Short by " + e.amount());
        }
    }
}
