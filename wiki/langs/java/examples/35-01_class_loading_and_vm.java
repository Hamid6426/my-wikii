// Lesson 35: JVM Internals (../35_jvm_internals.md)
// Class loading and the VM
// Run: java 35-01_class_loading_and_vm.java

public class VmInfo {
    static class Loaded {
        static { System.out.println("Loaded class initialized"); }
        static int value = 7;
    }

    public static void main(String[] args) {
        System.out.println("bootstrap loader of String is: " + String.class.getClassLoader());
        System.out.println("our loader is not null: " + (VmInfo.class.getClassLoader() != null));

        System.out.println("before first use");
        System.out.println("value = " + Loaded.value);
        System.out.println("after first use");
    }
}
