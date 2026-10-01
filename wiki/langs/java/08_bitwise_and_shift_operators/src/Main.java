public class Main {
  public static void main(String[] args) {
    int a = 5; // 0000 0101 in binary
    int b = 3; // 0000 0011 in binary

    // Bitwise AND
    int andResult = a & b; // 0000 0001 in binary, which is 1 in decimal
    System.out.println("Bitwise AND: " + andResult);

    // Bitwise OR
    int orResult = a | b; // 0000 0111 in binary,
    System.out.println("Bitwise OR: " + orResult);

    // Bitwise XOR
    int xorResult = a ^ b; // 0000 0110 in binary, which is 6 in decimal
    System.out.println("Bitwise XOR: " + xorResult);

    // Bitwise NOT
    int notResult = ~a; // 1111 1010 in binary, which is -6 in decimal (two's complement)
    System.out.println("Bitwise NOT: " + notResult);

    // Bitwise Left Shift
    int leftShiftResult = a << 1; // 0000 1010 in binary, which is 10 in decimal
    System.out.println("Left Shift: " + leftShiftResult);

    // Right Shift
    int rightShiftResult = a >> 1; // 0000 0010 in binary, which is 2 in decimal
    System.out.println("Right Shift: " + rightShiftResult);

    // Unsigned Right Shift
    int unsignedRightShiftResult = a >>> 1; // 0000 0010 in binary, which is 2 in decimal
    System.out.println("Unsigned Right Shift: " + unsignedRightShiftResult);

    // Bitwise AND Assignment
    a &= b; // a = a & b, which is 1
    System.out.println("Bitwise AND Assignment: " + a);

    // Bitwise OR Assignment
    a |= b; // a = a | b, which is 3
    System.out.println("Bitwise OR Assignment: " + a);

    // Bitwise XOR Assignment
    a ^= b; // a = a ^ b, which is 0
    System.out.println("Bitwise XOR Assignment: " + a);

    // Left Shift Assignment
    a <<= 1; // a = a << 1, which is 0
    System.out.println("Left Shift Assignment: " + a);

    // Right Shift Assignment
    a >>= 1; // a = a >> 1, which is 0
    System.out.println("Right Shift Assignment: " + a);

    // Unsigned Right Shift Assignment
    a >>>= 1; // a = a >>> 1, which is 0
    System.out.println("Unsigned Right Shift Assignment: " + a);
  }
}
