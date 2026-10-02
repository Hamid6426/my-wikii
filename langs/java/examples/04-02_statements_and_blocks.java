// Lesson 04: Basics of a Program (../04_basics_of_a_program.md)
// Statements and blocks
// Run: java 04-02_statements_and_blocks.java

public class Blocks {
    public static void main(String[] args) {
        int a = 2;                 // a statement ends with a semicolon
        int b = 3;

        if (a < b) {               // a block is code inside { }
            System.out.println("a is smaller");
        }

        System.out.println(a + b); // whitespace and line breaks do not matter
    }
}
