# 10 - Type Conversion

## Implicit Conversion

Automatic conversion when no data loss is possible.

```csharp
int x = 10;
long y = x;       // int → long (safe, no cast needed)
float f = x;      // int → float
double d = x;     // int → double
```

---

## Explicit Conversion (Casting)

Required when conversion may lose data or precision.

```csharp
double d = 9.99;
int i = (int)d;   // truncates to 9, not rounded

long big = 1_000_000_000_000L;
int small = (int)big;  // overflow: result is undefined/wrapped
```

---

## Convert Class

Safe conversions via `System.Convert`. Throws exceptions on invalid input.

```csharp
string s = "42";
int n = Convert.ToInt32(s);
double d = Convert.ToDouble("3.14");
bool b = Convert.ToBoolean(1);  // true
string str = Convert.ToString(99);
```

---

## Parse and TryParse

### Parse

Converts a string to a value type. Throws `FormatException` if invalid.

```csharp
int n = int.Parse("123");
double d = double.Parse("3.14");
```

### TryParse

Safe version: returns `false` instead of throwing.

```csharp
bool success = int.TryParse("abc", out int result);
// success = false, result = 0

if (int.TryParse("42", out int value))
{
    Console.WriteLine(value); // 42
}
```

---

## ToString

Every type has a `ToString()` method.

```csharp
int n = 42;
string s = n.ToString();        // "42"
double d = 3.14159;
string formatted = d.ToString("F2");  // "3.14"
```

---

## as and is (Reference Types)

### is: type check

```csharp
object obj = "hello";
if (obj is string)
{
    Console.WriteLine("It's a string");
}
```

### as: safe cast, returns null if fails

```csharp
object obj = "hello";
string s = obj as string;  // s = "hello"
object num = 42;
string fail = num as string;  // fail = null, no exception
```

---

## Pattern Matching Cast (C# 7+)

```csharp
object obj = "hello";
if (obj is string s)
{
    Console.WriteLine(s.Length);  // s is in scope here
}
```

---

## Numeric Widening Order

```
byte → short → int → long → float → double → decimal
```

Moving left requires explicit cast; moving right is implicit.

---

## Gotchas

- Casting `double` to `int` truncates, does not round
- `Convert.ToInt32` rounds, `(int)` truncates
- `int.Parse` throws on bad input; prefer `TryParse` for user input
- `as` only works with reference types and nullable types
- Overflow in explicit numeric casts wraps silently unless inside a `checked` block

---

## Examples

- [10-01](examples/10-01_type_conversion.cs): Implicit, explicit, Convert, Parse, TryParse

Run one with `dotnet run examples/10-01_type_conversion.cs`. See [Examples](examples/README.md).
