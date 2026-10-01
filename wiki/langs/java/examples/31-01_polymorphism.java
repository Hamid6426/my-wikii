// Lesson 31: Polymorphism (../31_polymorphism.md)
// Polymorphism
// Run: java 31-01_polymorphism.java

import java.util.List;

public class Main {
    public static void main(String[] args) {
        List<Animal> animals = List.of(new Dog("Rex"), new Cat("Luna"), new Dog("Max"));

        for (Animal a : animals) {
            a.speak();                          // runtime dispatch
        }

        Animal first = animals.get(0);
        if (first instanceof Dog d) {
            d.fetch();                          // only Dog has fetch()
        }

        describe(first);                        // overload picked by declared type
        describe((Dog) first);
    }

    static void describe(Animal a) { System.out.println("an animal"); }
    static void describe(Dog d)    { System.out.println("a dog"); }
}

abstract class Animal {
    protected final String name;
    Animal(String name) { this.name = name; }
    abstract void speak();
}

class Dog extends Animal {
    Dog(String name) { super(name); }
    @Override void speak() { System.out.println(name + ": Woof!"); }
    void fetch() { System.out.println(name + " fetches the ball"); }
}

class Cat extends Animal {
    Cat(String name) { super(name); }
    @Override void speak() { System.out.println(name + ": Meow!"); }
}
