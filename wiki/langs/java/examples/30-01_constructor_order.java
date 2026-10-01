// Lesson 30: Inheritance (../30_inheritance.md)
// Constructor order
// Run: java 30-01_constructor_order.java

public class Main {
    public static void main(String[] args) {
        new C();
    }
}

class A { A() { System.out.println("A"); } }
class B extends A { B() { System.out.println("B"); } }
class C extends B { C() { System.out.println("C"); } }
