# 28 - Indexers and Operator Overloading

## Indexer

Lets an object be used like an array: `obj[key]`.

```csharp
class Playlist
{
    private readonly List<string> _songs = new();

    public string this[int index]
    {
        get => _songs[index];
        set => _songs[index] = value;
    }

    public void Add(string song) => _songs.Add(song);
}

var p = new Playlist();
p.Add("Song A");
Console.WriteLine(p[0]);    // Song A
p[0] = "Song B";
```

---

## Indexer with a String Key

```csharp
class Settings
{
    private readonly Dictionary<string, string> _values = new();

    public string this[string key]
    {
        get => _values.TryGetValue(key, out var v) ? v : "";
        set => _values[key] = value;
    }
}

var s = new Settings();
s["theme"] = "dark";
```

---

## Index and Range Support

Built-in arrays, strings, and lists support `^` (from the end) and `..` (range).

```csharp
int[] nums = { 1, 2, 3, 4, 5 };

Console.WriteLine(nums[^1]);        // 5
int[] part = nums[1..4];            // 2, 3, 4
```

---

## Operator Overloading

Define what `+`, `==`, and other operators do for your type.

```csharp
struct Money
{
    public decimal Amount { get; }
    public Money(decimal amount) => Amount = amount;

    public static Money operator +(Money a, Money b) => new(a.Amount + b.Amount);
    public static Money operator -(Money a, Money b) => new(a.Amount - b.Amount);
    public static bool operator >(Money a, Money b) => a.Amount > b.Amount;
    public static bool operator <(Money a, Money b) => a.Amount < b.Amount;

    public override string ToString() => $"{Amount:C}";
}

var total = new Money(5) + new Money(10);
```

Operators must be `public static`.

---

## Pairs You Must Overload Together

| If you overload | You must also overload |
| --------------- | ---------------------- |
| `==`            | `!=`                   |
| `<`             | `>`                    |
| `<=`            | `>=`                   |

When you overload `==`, also override `Equals` and `GetHashCode`.

---

## Implicit and Explicit Conversion

```csharp
struct Celsius
{
    public double Degrees { get; }
    public Celsius(double d) => Degrees = d;

    public static implicit operator double(Celsius c) => c.Degrees;       // no cast needed
    public static explicit operator Celsius(double d) => new(d);          // cast required
}

double d = new Celsius(20);            // implicit
Celsius c = (Celsius)36.6;             // explicit
```

---

## Overloadable Operators

| Group      | Operators                                  |
| ---------- | ------------------------------------------ |
| Unary      | `+` `-` `!` `~` `++` `--` `true` `false`   |
| Binary     | `+` `-` `*` `/` `%` `&` `\|` `^` `<<` `>>` |
| Comparison | `==` `!=` `<` `>` `<=` `>=` (in pairs)     |

`&&`, `||`, `=`, and `.` cannot be overloaded.

---

## Gotchas

- Only overload an operator when its meaning is obvious; `date + date` makes no sense
- Records give you `==` for free, so you rarely write it yourself
- Implicit conversions that lose data or can fail should be `explicit`
- An indexer with a bad index throws; decide whether your type returns a default or throws, and keep it consistent

---

## Examples

- [28-01](examples/28-01_indexers_and_operators.cs): Indexers and operator overloading

Run one with `dotnet run examples/28-01_indexers_and_operators.cs`. See [Examples](examples/README.md).
