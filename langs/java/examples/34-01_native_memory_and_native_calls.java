// Lesson 34: Memory and Garbage Collection (../34_memory_and_garbage_collection.md)
// Native memory and native calls
// Run: java 34-01_native_memory_and_native_calls.java

import java.lang.foreign.*;
import java.lang.invoke.MethodHandle;

public class Main {
    public static void main(String[] args) throws Throwable {
        try (Arena arena = Arena.ofConfined()) {
            MemorySegment ints = arena.allocate(ValueLayout.JAVA_INT, 4);  // 4 ints, off-heap
            ints.setAtIndex(ValueLayout.JAVA_INT, 0, 42);
            System.out.println(ints.getAtIndex(ValueLayout.JAVA_INT, 0));
        }   // memory freed here

        Linker linker = Linker.nativeLinker();
        MethodHandle strlen = linker.downcallHandle(
            linker.defaultLookup().find("strlen").orElseThrow(),
            FunctionDescriptor.of(ValueLayout.JAVA_LONG, ValueLayout.ADDRESS));

        try (Arena arena = Arena.ofConfined()) {
            MemorySegment text = arena.allocateFrom("hello");    // a C string
            System.out.println((long) strlen.invokeExact(text));
        }
    }
}
