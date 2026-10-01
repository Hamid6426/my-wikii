# 30 - Polymorphism

## What is Polymorphism

The ability to treat objects of different types through a common interface, with each type providing its own implementation.

---

## Runtime Polymorphism (virtual / override)

The method called is determined at runtime based on the actual object type.

```csharp
class Shape
{
    public virtual double Area() => 0;
}

class Circle : Shape
{
    public double Radius { get; }
    public Circle(double r) => Radius = r;
    public override double Area() => Math.PI * Radius * Radius;
}

class Rectangle : Shape
{
    public double Width { get; }
    public double Height { get; }
    public Rectangle(double w, double h) { Width = w; Height = h; }
    public override double Area() => Width * Height;
}
```

Using them polymorphically:

```csharp
Shape[] shapes = {
    new Circle(5),
    new Rectangle(4, 6),
    new Circle(3)
};

foreach (Shape s in shapes)
{
    Console.WriteLine($"Area: {s.Area():F2}");  // calls the right override
}
```

---

## Compile-Time Polymorphism (Overloading)

Same method name, different parameter lists: resolved at compile time.

```csharp
class Printer
{
    public void Print(int n)    => Console.WriteLine($"Int: {n}");
    public void Print(string s) => Console.WriteLine($"String: {s}");
    public void Print(double d) => Console.WriteLine($"Double: {d}");
}
```

---

## Interface Polymorphism

Multiple types implementing the same interface: all treated as the same type.

```csharp
interface IDrawable
{
    void Draw();
}

class Circle : IDrawable
{
    public void Draw() => Console.WriteLine("Drawing Circle");
}

class Square : IDrawable
{
    public void Draw() => Console.WriteLine("Drawing Square");
}

List<IDrawable> drawables = new() { new Circle(), new Square() };
foreach (var d in drawables) d.Draw();
```

---

## Abstract Class Polymorphism

Force derived classes to implement specific behavior.

```csharp
abstract class Animal
{
    public string Name { get; }
    public Animal(string name) => Name = name;

    public abstract void Speak();          // must override

    public void Sleep() => Console.WriteLine($"{Name} is sleeping.");  // shared
}

class Dog : Animal
{
    public Dog(string name) : base(name) { }
    public override void Speak() => Console.WriteLine($"{Name}: Woof!");
}

class Cat : Animal
{
    public Cat(string name) : base(name) { }
    public override void Speak() => Console.WriteLine($"{Name}: Meow!");
}
```

---

## Upcasting and Downcasting

**Upcasting**: implicit, always safe.

```csharp
Dog dog = new Dog("Rex");
Animal animal = dog;    // upcast: Dog → Animal
animal.Sleep();         // works: Sleep is on Animal
```

**Downcasting**: explicit, can fail.

```csharp
Animal a = new Dog("Rex");
Dog d = (Dog)a;         // safe here: a really is a Dog
d.Speak();

Animal a2 = new Cat("Whiskers");
Dog d2 = (Dog)a2;       // throws InvalidCastException!

// Safe downcast
Dog d3 = a2 as Dog;    // null if not a Dog
if (a2 is Dog safeD) safeD.Speak();   // pattern match
```

---

## Polymorphism with Collections

```csharp
List<Animal> animals = new()
{
    new Dog("Rex"),
    new Cat("Luna"),
    new Dog("Max")
};

foreach (var animal in animals)
{
    animal.Speak();  // each calls its own override
}
```

---

## Gotchas

- A `virtual` method without `override` in a derived class still calls the base version
- `new` hides a method but does not achieve runtime polymorphism: base-typed reference calls the base version
- Abstract classes cannot be instantiated directly
- Prefer interfaces over abstract classes when only behavior (no shared state) is needed

---

## Examples

- [30-01](examples/30-01_shapes_polymorphism.cs): Polymorphism with abstract classes and overloads

Run one with `dotnet run examples/30-01_shapes_polymorphism.cs`. See [Examples](examples/README.md).
