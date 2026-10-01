import java.util.StringJoiner;

/**
 * This class demonstrates the use of various referenced data types in Java,
 * including String, StringBuilder, StringBuffer, and StringJoiner, Array,
 * ArrayList, LinkedList, HashMap, HashSet, and TreeSet. Each inner class
 * contains a main method that initializes and prints an example of the
 * respective data type.
 * 
 * Other than that enums, interfaces, and classes are also referenced data types
 * but they are not covered in this class.
 *
 * @author Hamid
 * @version 1.0
 * @since 2024-06-01
 */

public class Main {

  public static void main(String[] args) {
  }

  public static class StringExample {
    public static void main(String[] args) {
      String greeting = "Hello, World!";
      System.out.println("String value: " + greeting);
    }
  }

  public static class StringBuilderExample {
    public static void main(String[] args) {
      StringBuilder sb = new StringBuilder();
      sb.append("Hello");
      sb.append(", ");
      sb.append("World!");
      System.out.println("StringBuilder value: " + sb.toString());
    }
  }

  public static class StringBufferExample {
    public static void main(String[] args) {
      StringBuffer sb = new StringBuffer();
      sb.append("Hello");
      sb.append(", ");
      sb.append("World!");
      System.out.println("StringBuffer value: " + sb.toString());
    }
  }

  public static class StringJoinerExample {
    public static void main(String[] args) {
      StringJoiner sj = new StringJoiner(", ");
      sj.add("Hello");
      sj.add("World");
      System.out.println("StringJoiner value: " + sj.toString());
    }
  }

  public static class ArrayExample {
    public static void main(String[] args) {
      int[] numbers = { 1, 2, 3, 4, 5 };
      System.out.print("Array values: ");
      for (int number : numbers) {
        System.out.print(number + " ");
      }
      System.out.println();
    }
  }

  public static class ArrayListExample {
    public static void main(String[] args) {
      java.util.ArrayList<String> list = new java.util.ArrayList<>();
      list.add("Hello");
      list.add("World");
      System.out.println("ArrayList values: " + list);
    }
  }

  public static class LinkedListExample {
    public static void main(String[] args) {
      java.util.LinkedList<String> list = new java.util.LinkedList<>();
      list.add("Hello");
      list.add("World");
      System.out.println("LinkedList values: " + list);
    }
  }

  public static class HashMapExample {
    public static void main(String[] args) {
      java.util.HashMap<String, Integer> map = new java.util.HashMap<>();
      map.put("One", 1);
      map.put("Two", 2);
      System.out.println("HashMap values: " + map);
    }
  }

  public static class HashSetExample {
    public static void main(String[] args) {
      java.util.HashSet<String> set = new java.util.HashSet<>();
      set.add("Hello");
      set.add("World");
      System.out.println("HashSet values: " + set);
    }
  }

  public static class TreeSetExample {
    public static void main(String[] args) {
      java.util.TreeSet<String> set = new java.util.TreeSet<>();
      set.add("Hello");
      set.add("World");
      System.out.println("TreeSet values: " + set);
    }
  }
}
