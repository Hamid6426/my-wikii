# 23 - Reference Data Types

## Value Types vs Reference Types

| Feature        | Value Type (`struct`, `int`, `bool`) | Reference Type (`class`, `string`, array) |
| -------------- | ------------------------------------ | ----------------------------------------- |
| Stored in      | Stack (usually)                      | Heap                                      |
| Variable holds | The actual value                     | A reference (pointer) to the object       |
| Assignment     | Copies the value                     | Copies the reference                      |
| Default value  | Zero/false/'\0'                      | `null`                                    |
| Nullable       | Only with `?` (`int?`)               | Already nullable                          |

---

## class (Reference Type)

```csharp
class Person
{
    public string Name;
    public int Age;
}

Person p1 = new Person { Name = "Alice", Age = 30 };
Person p2 = p1;        // p2 points to the same object
p2.Name = "Bob";

Console.WriteLine(p1.Name);  // "Bob": p1 was also changed
```

---

## struct (Value Type)

```csharp
struct Point
{
    public int X;
    public int Y;
}

Point a = new Point { X = 1, Y = 2 };
Point b = a;        // b is a copy
b.X = 99;

Console.WriteLine(a.X);  // 1: a is unchanged
```

---

## null and Nullable Types

Reference types can be `null`. Value types cannot by default.

```csharp
string s = null;       // valid

int x = null;          // compile error

int? y = null;         // nullable int: valid
y = 42;
int val = y ?? 0;      // use 0 if null
```

---

## Null Checks

```csharp
string s = GetName();

if (s != null) { ... }
if (s is not null) { ... }    // C# 9+ preferred

// Null-conditional
int? len = s?.Length;

// Null-coalescing
string display = s ?? "Unknown";
s ??= "default";              // assign if null
```

---

## object: The Base Type

Every type in C# derives from `object` (`System.Object`).

```csharp
object o = 42;        // boxing: int wrapped in object
int n = (int)o;       // unboxing: explicit cast required

object s = "hello";
object b = true;
```

---

## Boxing and Unboxing

```csharp
int x = 10;
object boxed = x;       // boxing: value copied to heap
int unboxed = (int)boxed;  // unboxing: copied back to stack
```

Boxing has performance cost: avoid in hot paths.

---

## dynamic

Type is resolved at runtime, not compile time. Skips static type checking.

```csharp
dynamic d = 42;
Console.WriteLine(d);    // 42

d = "now a string";
Console.WriteLine(d.Length);  // 13: no compile-time check
```

---

## Gotchas

- Assigning a class instance copies the reference: both variables share the same object
- `null` access throws `NullReferenceException`: always check before use
- Boxing/unboxing is implicit and easy to miss: can cause allocations in loops
- `dynamic` bypasses compile-time safety; errors appear at runtime instead

---

## Examples

- [23-01](examples/23-01_value_vs_reference.cs): Value types vs reference types, boxing

Run one with `dotnet run examples/23-01_value_vs_reference.cs`. See [Examples](examples/README.md).
