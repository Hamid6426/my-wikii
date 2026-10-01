# 29 - Inheritance

## Basic Inheritance

A class can inherit from one base class using `:`.

```csharp
class Animal
{
    public string Name { get; set; }

    public void Eat() => Console.WriteLine($"{Name} is eating.");
}

class Dog : Animal
{
    public void Bark() => Console.WriteLine($"{Name} says Woof!");
}

var dog = new Dog { Name = "Rex" };
dog.Eat();   // inherited
dog.Bark();  // own method
```

---

## base Keyword

Call the base class constructor or method.

```csharp
class Animal
{
    public string Name { get; }

    public Animal(string name)
    {
        Name = name;
    }

    public virtual void Speak() => Console.WriteLine($"{Name} makes a sound.");
}

class Cat : Animal
{
    public Cat(string name) : base(name) { }  // calls Animal(name)

    public override void Speak() => Console.WriteLine($"{Name} says Meow!");
}
```

---

## virtual and override

`virtual` marks a method as overridable. `override` replaces it in a derived class.

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

---

## sealed

Prevents further inheritance or overriding.

```csharp
sealed class FinalClass { }   // no one can inherit from this

class Base
{
    public virtual void DoWork() { }
}

class Child : Base
{
    public sealed override void DoWork() { }  // cannot be overridden further
}
```

---

## new (Hiding)

Hides a base class member rather than overriding it. Base class variable still calls the original.

```csharp
class Parent
{
    public void Show() => Console.WriteLine("Parent");
}

class Child : Parent
{
    public new void Show() => Console.WriteLine("Child");
}

Child c = new Child();
c.Show();            // "Child"

Parent p = new Child();
p.Show();            // "Parent": binding is compile-time, not runtime
```

---

## Calling Base Method from Override

```csharp
class Logger : BaseLogger
{
    public override void Log(string msg)
    {
        base.Log(msg);                    // run base logic first
        Console.WriteLine("[Extended]");
    }
}
```

---

## Inheritance Chain

```csharp
class A { }
class B : A { }
class C : B { }   // C → B → A → object
```

All classes ultimately inherit from `object`.

---

## Checking Type

```csharp
Animal a = new Dog { Name = "Rex" };

bool isDog = a is Dog;             // true
bool isAnimal = a is Animal;       // true

if (a is Dog d)
{
    d.Bark();  // pattern-matched cast
}

Dog dog = a as Dog;   // null if not a Dog
```

---

## is-a Relationship

Use inheritance only for genuine "is-a" relationships. Prefer composition for "has-a".

---

## Gotchas

- C# supports only **single** class inheritance (a class can have one base class)
- `virtual` + `override` enables runtime polymorphism; `new` hides without polymorphism
- Calling `base.Method()` is optional; omit it when you want to completely replace behavior
- `sealed` on a class can enable JIT optimizations (de-virtualization)
- Do not use inheritance just for code reuse: use composition or extension methods instead

---

## Examples

- [29-01](examples/29-01_animal_hierarchy.cs): Inheritance, base, virtual, override, sealed

Run one with `dotnet run examples/29-01_animal_hierarchy.cs`. See [Examples](examples/README.md).
