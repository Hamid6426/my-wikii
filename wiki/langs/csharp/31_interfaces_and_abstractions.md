# 31 - Interfaces & Abstractions

## Interface

Defines a contract: what a type can do, without implementation.

```csharp
interface IShape
{
    double Area();
    double Perimeter();
}
```

---

## Implementing an Interface

```csharp
class Circle : IShape
{
    public double Radius { get; }
    public Circle(double r) => Radius = r;

    public double Area()      => Math.PI * Radius * Radius;
    public double Perimeter() => 2 * Math.PI * Radius;
}
```

A class can implement **multiple** interfaces:

```csharp
class Rectangle : IShape, IComparable<Rectangle>
{
    public double Width { get; }
    public double Height { get; }

    public Rectangle(double w, double h) { Width = w; Height = h; }

    public double Area()      => Width * Height;
    public double Perimeter() => 2 * (Width + Height);

    public int CompareTo(Rectangle other) => Area().CompareTo(other.Area());
}
```

---

## Interface as Type

```csharp
IShape s = new Circle(5);
Console.WriteLine(s.Area());

List<IShape> shapes = new() { new Circle(3), new Rectangle(4, 5) };
foreach (IShape shape in shapes)
{
    Console.WriteLine($"Area: {shape.Area():F2}");
}
```

---

## Default Interface Methods (C# 8+)

Provide a default implementation: implementing types can override it.

```csharp
interface ILogger
{
    void Log(string msg);
    void LogError(string msg) => Log($"[ERROR] {msg}");  // default
}
```

---

## Properties and Events in Interfaces

```csharp
interface IUser
{
    string Name { get; }
    event EventHandler LoggedIn;
}
```

---

## Abstract Class

Can have both abstract (no body) and concrete (with body) members. Cannot be instantiated.

```csharp
abstract class Animal
{
    public string Name { get; }
    public Animal(string name) => Name = name;

    public abstract void Speak();            // must override

    public void Sleep() => Console.WriteLine($"{Name} is sleeping.");  // shared
}

class Dog : Animal
{
    public Dog(string name) : base(name) { }
    public override void Speak() => Console.WriteLine("Woof!");
}
```

---

## Interface vs Abstract Class

| Feature                 | Interface             | Abstract Class        |
| ----------------------- | --------------------- | --------------------- |
| Multiple inheritance    | Yes (many interfaces) | No (one base class)   |
| Fields / state          | No (C# 8+ constants)  | Yes                   |
| Constructors            | No                    | Yes                   |
| Default implementations | Yes (C# 8+)           | Yes                   |
| Access modifiers        | `public` only         | Any                   |
| Use when                | Defining capability   | Sharing base behavior |

---

## Explicit Interface Implementation

When two interfaces have methods with the same name.

```csharp
interface IA { void Show(); }
interface IB { void Show(); }

class MyClass : IA, IB
{
    void IA.Show() => Console.WriteLine("IA.Show");
    void IB.Show() => Console.WriteLine("IB.Show");
}

MyClass obj = new MyClass();
((IA)obj).Show();  // "IA.Show"
((IB)obj).Show();  // "IB.Show"
```

---

## Common Built-in Interfaces

| Interface        | Purpose                               |
| ---------------- | ------------------------------------- |
| `IComparable<T>` | Sorting (`CompareTo`)                 |
| `IEquatable<T>`  | Equality (`Equals`)                   |
| `IEnumerable<T>` | Iteration (`foreach`)                 |
| `IDisposable`    | Resource cleanup (`Dispose`, `using`) |
| `ICloneable`     | Cloning (`Clone`)                     |

---

## Gotchas

- Interfaces define capability, not identity: prefer interfaces over base classes for dependency injection
- Avoid fat interfaces: split them by responsibility (Interface Segregation Principle)
- `abstract` class is appropriate when you have shared state or need a constructor
- Default interface methods break the assumption that all implementors have identical behavior: use with care

---

## Examples

- [31-01](examples/31-01_interfaces.cs): Interfaces, default methods, explicit implementation

Run one with `dotnet run examples/31-01_interfaces.cs`. See [Examples](examples/README.md).
