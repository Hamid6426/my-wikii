/**
 * Represents a basic calculator for mathematical operations.
 * This class provides methods for addition, subtraction, multiplication, and
 * division. This class provides methods for addition and handles basic
 * arithmetic logic.
 *
 * @author Hamid
 * @version 25.0.2
 * @since 2026-03-22
 */
public class Calculator {

    /**
     * Emtry point of every java application
     * Must be named public static void main(String[] args) {}
     *
     * public = JVM must be able to access it from outside the class.
     * static = JVM calls it without creating an object.
     * void = Returns nothing. JVM doesn’t expect a return value.
     * main = Fixed name. Not optional.
     * 
     * @param String[] Use to pass command line args
     * @return expect nothing
     */

    public static void main(String[] args) {
        Calculator calc = new Calculator();

        System.out.println("Add: " + calc.add(10, 20));
        System.out.println("Subtract: " + calc.subtract(20, 5));
    }

    /**
     * Adds two integers together.
     *
     * @param a The first integer to add.
     * @param b The second integer to add.
     * @return The sum of a and b.
     * @throws IllegalArgumentException if the result exceeds integer limits.
     */
    public int add(int a, int b) {
        long result = (long) a + b;

        if (result > Integer.MAX_VALUE || result < Integer.MIN_VALUE) {
            throw new IllegalArgumentException("Integer overflow");
        }

        return (int) result;
    }

    /**
     * Calculates the difference between two integers.
     *
     * @param a The value to be subtracted from (minuend).
     * @param b The value to subtract (subtrahend).
     * @return The result of a minus b.
     */
    public int subtract(int a, int b) {
        long result = (long) a - b;

        if (result > Integer.MAX_VALUE || result < Integer.MIN_VALUE) {
            throw new IllegalArgumentException("Integer overflow");
        }

        return (int) result;
    }

    /**
     * Multiplies two integers together.
     * 
     * @param a The first integer to multiply.
     * @param b The second integer to multiply.
     * @return The product of a and b.
     * @throws IllegalArgumentException if the result exceeds integer limits.
     */
    public int multiply(int a, int b) {
        long result = (long) a * b;

        if (result > Integer.MAX_VALUE || result < Integer.MIN_VALUE) {
            throw new IllegalArgumentException("Integer overflow");
        }

        return (int) result;
    }

    /**
     * Divides one integer by another.
     *
     * @param a The dividend (the number to be divided).
     * @param b The divisor (the number to divide by).
     * @return The quotient of a divided by b.
     * @throws IllegalArgumentException if b is zero (division by zero).
     */
    public int divide(int a, int b) {
        if (b == 0) {
            throw new IllegalArgumentException("Division by zero is not allowed");
        }

        return a / b;
    }
}