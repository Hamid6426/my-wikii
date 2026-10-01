# 26 - Classes & Objects

## Defining a Class

```csharp
class Person
{
    // Fields
    private string _name;
    private int _age;

    // Constructor
    public Person(string name, int age)
    {
        _name = name;
        _age = age;
    }

    // Properties
    public string Name => _name;
    public int Age => _age;

    // Method
    public void Introduce()
    {
        Console.WriteLine($"Hi, I'm {_name}, age {_age}.");
    }
}
```

---

## Creating Objects

```csharp
Person p = new Person("Alice", 30);
p.Introduce();

// C# 9+ target-typed new
Person p2 = new("Bob", 25);
```

---

## Fields

```csharp
class Counter
{
    private int _count = 0;          // instance field
    private static int _total = 0;   // shared across all instances
}
```

---

## Properties

Encapsulate fields: control read/write access.

```csharp
class Product
{
    private decimal _price;

    public decimal Price
    {
        get => _price;
        set
        {
            if (value < 0) throw new ArgumentException("Price cannot be negative");
            _price = value;
        }
    }
}
```

Auto-implemented property (compiler generates backing field):

```csharp
class User
{
    public string Name { get; set; }
    public string Email { get; private set; }  // read-only externally
    public int Id { get; init; }               // settable only in object initializer (C# 9+)
}
```

---

## Object Initializer

```csharp
var user = new User
{
    Name = "Alice",
    Email = "alice@example.com"
};
```

---

## Constructors

```csharp
class Point
{
    public double X { get; }
    public double Y { get; }

    // Parameterized constructor
    public Point(double x, double y) { X = x; Y = y; }

    // Constructor chaining
    public Point() : this(0, 0) { }
}
```

---

## Static Members

Belong to the type, not to any instance.

```csharp
class MathUtils
{
    public static double Pi = 3.14159;

    public static int Square(int n) => n * n;
}

MathUtils.Square(5);   // 25
```

---

## this Keyword

Refers to the current instance.

```csharp
class Builder
{
    private string _name;

    public Builder SetName(string name)
    {
        _name = name;
        return this;   // enables method chaining
    }
}
```

---

## Access Modifiers

| Modifier             | Accessible from                        |
| -------------------- | -------------------------------------- |
| `public`             | Anywhere                               |
| `private`            | Inside the class only (default)        |
| `protected`          | Class + derived classes                |
| `internal`           | Same assembly                          |
| `protected internal` | Same assembly or derived classes       |
| `private protected`  | Same assembly and only derived classes |

---

## Destructors / Finalizers

Called before garbage collection. Rarely needed with `IDisposable`.

```csharp
class Resource
{
    ~Resource()
    {
        // cleanup unmanaged resources
    }
}
```

---

## Gotchas

- Fields are `private` by default inside a class
- Properties are preferred over public fields: they allow validation and future change without breaking callers
- Static constructors run once before the type is first used
- Avoid mutable public fields; use properties with controlled setters

---

## Examples

- [26-01](examples/26-01_bank_account.cs): A class with fields, properties, and a constructor

Run one with `dotnet run examples/26-01_bank_account.cs`. See [Examples](examples/README.md).
