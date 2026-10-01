/**
 * Data Type Conversion Examples
 * 
 * This class demonstrates various examples of data type conversions in Java,
 * including conversions between double and int, int and double, int and char,
 * char and int, float and int, and int and float. Each inner class contains a
 * main method that performs a specific type conversion and prints the results.
 * 
 * Java has 8 primitive data types, and values can be converted from one type to
 * another. This process is called type conversion. There are two types in Java:
 * implicit (automatic, widening) and explicit (manual, narrowing/casting).
 * Implicit conversion occurs automatically when it is safe. Explicit conversion
 * (type casting) is performed by the programmer when needed.
 * 
 * There are 56 types conversions but there is a pattern to them. For example,
 * when converting from a larger type to a smaller type (narrowing), you need to
 * use explicit casting. When converting from a smaller type to a larger type
 * (widening), it happens automatically. For example, converting from int to
 * double is a widening conversion and happens automatically, while converting
 * from double to int is a narrowing conversion and requires explicit casting.
 * 
 * Explicit casting can lead to data loss if the value being converted is
 * outside the range of the target type. For example, when converting a double
 * to an int, the decimal part will be truncated, and if the double value is
 * larger than the maximum value of an int, it will wrap around to a negative
 * value. Therefore, it is important to be cautious when performing explicit
 * type conversions to avoid unintended consequences.
 * 
 * e.g. during explicity casting of double area_circle = 78.53981634d; to int,
 * the value will be truncated to 78, which is a loss of precision. Similarly,
 * when converting a char to an int, the ASCII code of the character will be
 * used, which may not always be the intended result.
 * 
 * Similarly for a short to byte conversion, if the short value is larger than
 * 127 or smaller than -128, it will wrap around and produce an incorrect byte
 * value. For example, if you try to convert a short value of 130 to a byte, it
 * will wrap around and result in a byte value of -126, which is not the
 * expected outcome. Therefore, it is crucial to ensure that the value being
 * converted is within the valid range of the target type to prevent data loss
 * and ensure accurate results.
 *
 * e.g. 129 int to byte = -127, because 129 is outside the range of byte (-128
 * to 127), it wraps around to -127.
 * 130 int to byte = -126, because 130 is outside the range of byte (-128 to
 * 127), it wraps around to -126.
 * 256 int to byte = 0, because 256 is outside the range of byte (-128 to 127),
 * it wraps around to 0.
 * 257 int to byte = 1, because 257 is outside the range of byte (-128 to 127),
 * it
 * So its a table of 256 values, where the first 128 values (0 to 127) are the
 * same as the byte values, and the next 128 values (128 to 255) wrap around to
 * -128 to -1.
 * 
 * For char to int conversion, the char 'A' has an ASCII code of 65, so when you
 * convert 'A' to int, it will result in 65. Similarly, the char 'B' has an
 * ASCII code of 66, so converting 'B' to int will yield 66. This can be used to
 * perform operations based on the ASCII values of characters, such as sorting
 * or comparing characters. However, it is important to remember that when
 * converting from char to int, the resulting integer value represents the ASCII
 * code of the character, which may not always be the intended outcome depending
 * on the context of the program..
 * 
 * @author Hamid
 * @version 1.0
 * @since 2024-06-01
 */

public class Main {
  public static void main(String[] args) {
    System.out.println("Data type conversion examples logs below!");
    DataDoubleToInt.main(args);
  }

  public class DataDoubleToInt {
    public static void main(String s[]) {
      double area_circle = 78.53981634d;
      System.out.println("Area of circle of 5cm radius is  " + area_circle);
      System.out.println("Area of circle of 5cm radius is  " + (int) area_circle);
    }
  }

  public class DataIntToDouble {
    public static void main(String s[]) {
      int radius = 5;
      double area_circle = 3.14159265 * radius * radius;
      System.out.println("Area of circle of 5cm radius is  " + area_circle);
    }
  }

  public class DataIntToChar {
    public static void main(String s[]) {
      int charCode = 65;
      char character = (char) charCode;
      System.out.println("The character for ASCII code 65 is: " + character);
    }
  }

  public class DataCharToInt {
    public static void main(String s[]) {
      char character = 'A';
      int charCode = (int) character;
      System.out.println("The ASCII code for character 'A' is: " + charCode);
    }
  }

  public class DataFloatToInt {
    public static void main(String s[]) {
      float pi = 3.14159265f;
      System.out.println("Value of pi is: " + pi);
      System.out.println("Value of pi as an integer is: " + (int) pi);
    }
  }

  public class DataIntToFloat {
    public static void main(String s[]) {
      int radius = 5;
      float area_circle = 3.14159265f * radius * radius;
      System.out.println("Area of circle of 5cm radius is  " + area_circle);
    }
  }
}
