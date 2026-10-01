// Lesson 64: Java Version History (../64_java_version_history.md)
// Which version am i using
// Run: java 64-01_which_version_am_i_using.java

public class Version {
    public static void main(String[] args) {
        System.out.println(Runtime.version().feature());   // for example 25
    }
}
