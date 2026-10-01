public class Main {
  // assignment operators are used to assign values to variables. The main
  // assignment operators in Java are:
  // 1. Assignment (=): Assigns the value on the right to the variable on the
  // left.
  // 2. Addition assignment (+=): Adds the value on the right to the variable
  // on the left and assigns the result to the variable on the left.
  // 3. Subtraction assignment (-=): Subtracts the value on the right from the
  // variable on the left and assigns the result to the variable on the left.
  // 4. Multiplication assignment (*=): Multiplies the variable on the left by
  // the value on the right and assigns the result to the variable on the left.
  // 5. Division assignment (/=): Divides the variable on the left by the
  // value on the right and assigns the result to the variable on the left.
  // 6. Modulus assignment (%=): Takes the modulus of the variable on the left
  // by the value on the right and assigns the result to the variable on the left.

  public static void main(String[] args) {
    int a = 10; // Assignment operator
    a += 5; // Addition assignment operator (a = a + 5)
    System.out.println("After addition assignment: " + a); // 15

    a -= 3; // Subtraction assignment operator (a = a - 3)
    System.out.println("After subtraction assignment: " + a); // 12

    a *= 2; // Multiplication assignment operator (a = a * 2)
    System.out.println("After multiplication assignment: " + a); // 24

    a /= 4; // Division assignment operator (a = a / 4)
    System.out.println("After division assignment: " + a); // 6

    a %= 5; // Modulus assignment operator (a = a % 5)
    System.out.println("After modulus assignment: " + a); // 1
  }
}