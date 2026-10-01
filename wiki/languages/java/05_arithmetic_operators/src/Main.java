public class Main {
  // arethmetic operators are used to perform basic mathematical operations on
  // numerical values. The main arithmetic operators in Java are:
  // 1. Addition (+): Adds two operands together.
  // 2. Subtraction (-): Subtracts the right operand from the left operand.
  // 3. Multiplication (*): Multiplies two operands together.
  // 4. Division (/): Divides the left operand by the right operand.
  // 5. Modulus (%): Returns the remainder of the division of the left operand

  public static void main(String[] args) {
    int a = 10;
    int b = 3;

    // Addition
    int sum = a + b; // 13
    System.out.println("Addition: " + sum);

    // Subtraction
    int difference = a - b; // 7
    System.out.println("Subtraction: " + difference);

    // Multiplication
    int product = a * b; // 30
    System.out.println("Multiplication: " + product);

    // Division
    int quotient = a / b; // 3 (integer division)
    System.out.println("Division: " + quotient);

    // Modulus
    int remainder = a % b; // 1
    System.out.println("Modulus: " + remainder);
  }
}
