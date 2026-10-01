
/**
 * This class demonstrates the use of various primitive data types in Java,
 * including byte, short, int, long, float, double, char, and boolean. Each
 * inner class contains a main method that initializes and prints an example of
 * the respective data type.
 *
 * @author Hamid
 * @version 1.0
 * @since 2024-06-01
 */

public class Main {
  public static void main(String[] args) {
    System.out.println("Data type examples logs below!");
    ByteExample.main(args);
    ShortExample.main(args);
    IntExample.main(args);
    LongExample.main(args);
    FloatExample.main(args);
    DoubleExample.main(args);
    CharExample.main(args);
    BooleanExample.main(args);
  }

  /**
   * This class demonstrates the use of the byte data type, which is an 8-bit
   * signed integer. It can hold values from -128 to 127.
   * 
   * In this example, we declare a byte variable named 'age' and assign it the
   * value of 30, which is within the valid range for a byte. We then print the
   * value of 'age' to the console.
   */
  public static class ByteExample {
    public static void main(String[] args) {
      byte age = 30;
      System.out.println("Byte value: " + age);
    }
  }

  /**
   * This class demonstrates the use of the short data type, which is a 16-bit
   * signed integer. It can hold values from -32,768 to 32,767.
   * 
   * In this example, we declare a short variable named 'year' and assign it the
   * value of 2024, which is within the valid range for a short. We then print the
   * value of 'year' to the console.
   */
  public static class ShortExample {
    public static void main(String[] args) {
      short year = 2024;
      System.out.println("Short value: " + year);
    }
  }

  /**
   * This class demonstrates the use of the int data type, which is a 32-bit
   * signed integer. It can hold values from -2,147,483,648 to 2,147,483,647.
   * 
   * In this example, we declare an int variable named 'population' and assign it
   * the value of 780000, which is within the valid range for an int. We then
   * print
   * the value of 'population' to the console.
   */
  public static class IntExample {
    public static void main(String[] args) {
      int population = 780000;
      System.out.println("Int value: " + population);
    }
  }

  /**
   * This class demonstrates the use of the long data type, which is a 64-bit
   * signed integer. It can hold values from -9,223,372,036,854,775,808 to
   * 9,223,372,036,854,775,807.
   * 
   * In this example, we declare a long variable named 'distanceToSun' and assign
   * it
   * the value of 149600000L (note the 'L' suffix to indicate a long literal),
   * which
   * is within the valid range for a long. We then print the value of
   * 'distanceToSun'
   * to the console.
   */
  public static class LongExample {
    public static void main(String[] args) {
      long distanceToSun = 149600000L;
      System.out.println("Long value: " + distanceToSun);
    }
  }

  /**
   * This class demonstrates the use of the float data type, which is a 32-bit
   * IEEE 754 floating-point number. It provides approximately 6-7 decimal digits
   * of precision.
   * 
   * In this example, we declare a float variable named 'pi' and assign it the
   * value of 3.14f (note the 'f' suffix to indicate a float literal). We then
   * print
   * the value of 'pi' to the console.
   */
  public static class FloatExample {
    public static void main(String[] args) {
      float pi = 3.14f;
      System.out.println("Float value: " + pi);
    }
  }

  /**
   * This class demonstrates the use of the double data type, which is a 64-bit
   * IEEE 754 floating-point number. It provides approximately 15 decimal digits
   * of precision and is the default choice for decimal numbers in Java.
   * 
   * In this example, we declare a double variable named 'e' and assign it the
   * value of 2.71828 (Euler's number). We then print the value of 'e' to the
   * console.
   */
  public static class DoubleExample {
    public static void main(String[] args) {
      double e = 2.71828;
      System.out.println("Double value: " + e);
    }
  }

  /**
   * This class demonstrates the use of the char data type, which is a 16-bit
   * Unicode character. It can represent any character from the Unicode character
   * set.
   * 
   * In this example, we declare a char variable named 'grade' and assign it the
   * character 'A'. We then print the value of 'grade' to the console.
   */
  public static class CharExample {
    public static void main(String[] args) {
      char grade = 'A';
      System.out.println("Char value: " + grade);
    }
  }

  /**
   * This class demonstrates the use of the boolean data type, which represents
   * a logical value. It can only be true or false.
   * 
   * In this example, we declare a boolean variable named 'isJavaFun' and assign
   * it the value true. We then print the value of 'isJavaFun' to the console.
   */
  public static class BooleanExample {
    public static void main(String[] args) {
      boolean isJavaFun = true;
      System.out.println("Boolean value: " + isJavaFun);
    }
  }
}
