# 13 - Control Flow

## if / else if / else

```csharp
int score = 75;

if (score >= 90)
{
    Console.WriteLine("A");
}
else if (score >= 80)
{
    Console.WriteLine("B");
}
else if (score >= 70)
{
    Console.WriteLine("C");
}
else
{
    Console.WriteLine("F");
}
```

---

## switch Statement

```csharp
int day = 3;

switch (day)
{
    case 1:
        Console.WriteLine("Monday");
        break;
    case 2:
        Console.WriteLine("Tuesday");
        break;
    case 3:
    case 4:
        Console.WriteLine("Mid-week");  // fall-through via empty case
        break;
    default:
        Console.WriteLine("Other");
        break;
}
```

---

## switch Expression (C# 8+)

Returns a value: more concise.

```csharp
string dayName = day switch
{
    1 => "Monday",
    2 => "Tuesday",
    3 => "Wednesday",
    _ => "Unknown"   // _ is the default arm
};
```

---

## Pattern Matching in switch (C# 7+)

```csharp
object obj = 42;

string result = obj switch
{
    int n when n > 0 => "Positive int",
    int n            => "Non-positive int",
    string s         => $"String: {s}",
    null             => "Null",
    _                => "Other"
};
```

---

## Ternary Operator

Inline conditional: returns a value.

```csharp
int age = 20;
string status = age >= 18 ? "Adult" : "Minor";
```

---

## Null Check Patterns

```csharp
string name = GetName();

// Traditional
if (name == null) { ... }

// Modern
if (name is null) { ... }
if (name is not null) { ... }
```

---

## goto (avoid in production)

Jumps to a labelled statement. Rarely used: signals a design problem.

```csharp
goto end;
Console.WriteLine("This is skipped");
end:
Console.WriteLine("Jumped here");
```

---

## Gotchas

- `switch` requires `break` or `return` in each case (no implicit fall-through unlike C)
- Empty cases (no code before `break`) can fall through
- `switch` expressions must be exhaustive: use `_` as the catch-all or the compiler warns
- `is null` is preferred over `== null` for pattern matching contexts

---

## Examples

- [13-01](examples/13-01_grade_calculator.cs): if, else if, and switch

Run one with `dotnet run examples/13-01_grade_calculator.cs`. See [Examples](examples/README.md).
