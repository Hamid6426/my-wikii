// Lesson 40: Modern Java Features (../40_modern_java_features.md)
// Text blocks java 15
// Run: java 40-01_text_blocks_java_15.java

public class TextBlocks {
    public static void main(String[] args) {
        String json = """
            {
              "name": "Alice",
              "age": 30
            }
            """;
        System.out.print(json);

        String sql = """
            SELECT name \
            FROM users \
            WHERE age > %d""".formatted(18);
        System.out.println(sql);
    }
}
