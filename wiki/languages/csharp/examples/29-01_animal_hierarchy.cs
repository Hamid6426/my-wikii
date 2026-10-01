// Lesson 29: Inheritance (../29_inheritance.md)
// Inheritance, base, virtual, override, sealed
// Run: dotnet run 29-01_animal_hierarchy.cs

Animal[] zoo = { new Animal("Generic"), new Dog("Rex"), new Puppy("Bit") };

foreach (Animal a in zoo)
{
    Console.WriteLine($"{a.Name} says {a.Speak()} ({a.GetType().Name})");
}

Console.WriteLine(zoo[1] is Dog);
Console.WriteLine(zoo[2] is Dog);          // a Puppy is also a Dog
Console.WriteLine(zoo[0] is Dog);

class Animal(string name)
{
    public string Name { get; } = name;
    public virtual string Speak() => "...";
}

class Dog(string name) : Animal(name)
{
    public override string Speak() => "Woof";
}

sealed class Puppy(string name) : Dog(name)
{
    public override string Speak() => base.Speak() + " (squeaky)";
}

// Expected output should be:
// Generic says ... (Animal)
// Rex says Woof (Dog)
// Bit says Woof (squeaky) (Puppy)
// True
// True
// False
