# Building Blocks

## Comments

There are 3 types of comments

### Inline Comment

```java
// This is a comment
String name = "Hamid"; // This is also a comment
```

### MultiLine Comment

```java
/* This is a MutiLine Comment
   I have written a simple code
*/
public class ProjectOne {
    public static void main(String[] args) {
        System.out.println(" is writing this code");
    }
}
```

### Documentation Comment

```java
/**
 * Represents a basic calculator for mathematical operations.
 * <p>
 * This class provides methods for addition and handles
 * basic arithmetic logic.
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
        return a + b;
    }

    /**
     * Calculates the difference between two integers.
     *
     * @param a The value to be subtracted from (minuend).
     * @param b The value to subtract (subtrahend).
     * @return The result of a minus b.
     */
    public int subtract(int a, int b) {
        return a - b;
    }
}
```

## Data Types

### Primitive Data Types

| Data Type | Use When                        | Usage Example                 | Code Example                   |
| --------- | ------------------------------- | ----------------------------- | ------------------------------ |
| byte      | Saving memory in large arrays   | small numbers (-128 to 127)   | byte age = 25;                 |
| short     | Slightly larger than byte       | small-range integers          | short temp = 3000;             |
| int       | Default integer type            | general integer operations    | int count = 100;               |
| long      | Large integer values            | big numbers (timestamps, IDs) | long population = 8000000000L; |
| float     | Decimal values (less precision) | scientific calculations       | float price = 10.5f;           |
| double    | Default decimal type            | precise decimal calculations  | double pi = 3.14159;           |
| char      | Single character                | storing letters/symbols       | char grade = 'A';              |
| boolean   | True/false conditions           | flags, conditions             | boolean isActive = true;       |

### Reference Data Types

| Data Type       | Use When                    | Usage Example             | Code Example               |
| --------------- | --------------------------- | ------------------------- | -------------------------- |
| String          | Working with text           | names, sentences          | String name = "Hamid";     |
| Array           | Storing multiple values     | list of items             | int[] nums = {1, 2, 3};    |
| Class           | Creating custom objects     | user-defined data         | class Car {}               |
| Object          | General-purpose reference   | base type for all classes | Object obj = new Object(); |
| Interface       | Defining contracts          | abstraction               | interface Animal {}        |
| Enum            | Fixed set of constants      | days, states              | enum Day { MON, TUE }      |
| Wrapper Classes | Using primitives as objects | collections, null support | Integer num = 10;          |
