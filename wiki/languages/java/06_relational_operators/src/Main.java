public class Main {
  // relational operators are used to compare two values and return a boolean
  // result. The
  // main relational operators in Java are:
  // 1. Equal to (==): Returns true if the operands are equal, otherwise returns
  // false.
  // 2. Not equal to (!=): Returns true if the operands are not equal, otherwise
  // returns false.
  // 3. Greater than (>): Returns true if the left operand is greater than the
  // right operand, otherwise returns false.
  // 4. Less than (<): Returns true if the left operand is less than the right
  // operand, otherwise returns false.
  // 5. Greater than or equal to (>=): Returns true if the left operand is greater
  // than or equal to the right operand, otherwise returns false.
  // 6. Less than or equal to (<=): Returns true if the left operand is less than
  // or equal to the right operand, otherwise returns false.

  public static void main(String[] args) {
    int a = 10;
    int b = 20;

    // Equal to
    boolean equalTo = (a == b); // false
    System.out.println("Equal to: " + equalTo);

    // Not equal to
    boolean notEqualTo = (a != b); // true
    System.out.println("Not equal to: " + notEqualTo);

    // Greater than
    boolean greaterThan = (a > b); // false
    System.out.println("Greater than: " + greaterThan);

    // Less than
    boolean lessThan = (a < b); // true
    System.out.println("Less than: " + lessThan);

    // Greater than or equal to
    boolean greaterThanOrEqualTo = (a >= b); // false
    System.out.println("Greater than or equal to: " + greaterThanOrEqualTo);

    // Less than or equal to
    boolean lessThanOrEqualTo = (a <= b); // true
    System.out.println("Less than or equal to: " + lessThanOrEqualTo);
  }
}
