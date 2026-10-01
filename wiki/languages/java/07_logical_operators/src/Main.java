public class Main {
  // logical operators are used to combine multiple boolean expressions and return
  // a boolean result. The main logical operators in Java are:
  // 1. AND (&&): Returns true if both operands are true, otherwise returns false.
  // 2. OR (||): Returns true if at least one of the operands is true, otherwise
  // returns false.
  // 3. NOT (!): Returns true if the operand is false, and returns false if the
  // operand is true.

  public static void main(String[] args) {
    boolean a = true;
    boolean b = false;

    // AND operator
    boolean andResult = a && b; // false
    System.out.println("AND Result: " + andResult);

    // OR operator
    boolean orResult = a || b; // true
    System.out.println("OR Result: " + orResult);

    // NOT operator
    boolean notResult = !a; // false
    System.out.println("NOT Result: " + notResult);
  }
}
