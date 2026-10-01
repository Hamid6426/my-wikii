public class Main {
  // unary operators are used to perform operations on a single operand. The main
  // unary operators in Java are:
  // 1. Unary plus (+): Indicates a positive value. It does not change the value
  // of the operand.
  // 2. Unary minus (-): Negates the value of the operand, changing its sign.
  // 3. Increment (++) : Increases the value of the operand by 1.
  // 4. Decrement (--) : Decreases the value of the operand by 1.

  // Ternary operator (?:) is a shorthand for an if-else statement. It takes three
  // operands:
  // 1. A boolean expression that evaluates to true or false.
  // 2. An expression that is evaluated and returned if the boolean expression is
  // true.
  // 3. An expression that is evaluated and returned if the boolean expression is
  // false.

  public static void main(String[] args) {
    int a = 5;

    // Unary plus
    int unaryPlus = +a; // 5
    System.out.println("Unary plus: " + unaryPlus);

    // Unary minus
    int unaryMinus = -a; // -5
    System.out.println("Unary minus: " + unaryMinus);

    // Increment
    int increment = a++; // Post-increment: returns a (5), then increments a to 6
    System.out.println("Post-increment: " + increment);
    System.out.println("Value of a after post-increment: " + a); // 6

    a = 5; // Reset a to 5 for pre-increment
    increment = ++a; // Pre-increment: increments a to 6, then returns a (6)
    System.out.println("Pre-increment: " + increment);
    System.out.println("Value of a after pre-increment: " + a); // 6

    a = 5; // Reset a to 5 for post-decrement
    int decrement = a--; // Post-decrement: returns a (5), then decrements a to 4
    System.out.println("Post-decrement: " + decrement);
    System.out.println("Value of a after post-decrement: " + a); // 4

    a = 5; // Reset a to 5 for pre-decrement
    decrement = --a; // Pre-decrement: decrements a to 4, then returns a (4)
    System.out.println("Pre-decrement: " + decrement);
    System.out.println("Value of a after pre-decrement: " + a); // 4

    // Ternary operator
    int x = 10;
    int y = 20;
    int max = (x > y) ? x : y; // If x is greater than y, return x; otherwise, return y
    System.out.println("Maximum value: " + max); // 20
  }
}