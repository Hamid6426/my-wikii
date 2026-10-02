# 17 - Parameters

## Value Parameters (default)

A copy of the value is passed. Changes inside the method do not affect the caller.

```csharp
static void Double(int x)
{
    x *= 2;  // only changes the local copy
}

int n = 5;
Double(n);
Console.WriteLine(n);  // still 5
```

---

## ref Parameter

Passes a reference to the variable. Changes inside the method affect the caller.

```csharp
static void Double(ref int x)
{
    x *= 2;
}

int n = 5;
Double(ref n);
Console.WriteLine(n);  // 10
```

Must be initialized before passing.

---

## out Parameter

Like `ref`, but the variable does not need to be initialized before the call. The method must assign it before returning.

```csharp
static bool TryParse(string s, out int result)
{
    return int.TryParse(s, out result);
}

if (TryParse("42", out int value))
{
    Console.WriteLine(value);  // 42
}
```

Inline declaration (C# 7+):

```csharp
int.TryParse("123", out int num);
```

---

## in Parameter (C# 7.2+)

Read-only reference. No copy, but the method cannot modify the value.

```csharp
static double Hypotenuse(in double a, in double b)
    => Math.Sqrt(a * a + b * b);

double result = Hypotenuse(3.0, 4.0);  // 5.0
```

Useful for large value types (`struct`) where copying is expensive.

---

## Default Parameters

Must appear after required parameters.

```csharp
static void Greet(string name, string greeting = "Hello")
{
    Console.WriteLine($"{greeting}, {name}!");
}

Greet("Alice");           // "Hello, Alice!"
Greet("Bob", "Hi");       // "Hi, Bob!"
```

---

## Named Arguments

Pass arguments in any order by specifying the parameter name.

```csharp
static int Range(int min, int max, int step = 1) { ... }

Range(max: 10, min: 0);
Range(min: 0, max: 10, step: 2);
```

---

## params: Variable Number of Arguments

```csharp
static int Sum(params int[] numbers)
{
    return numbers.Sum();
}

int total = Sum(1, 2, 3, 4, 5);  // 15
int also = Sum();                 // 0: empty array
```

- Only one `params` parameter per method, and it must be the last
- Caller can also pass an array directly: `Sum(new int[] { 1, 2, 3 })`

---

## ref vs out vs in

| Keyword | Must init before call | Method must assign | Can modify |
| ------- | --------------------- | ------------------ | ---------- |
| `ref`   | Yes                   | No                 | Yes        |
| `out`   | No                    | Yes                | Yes        |
| `in`    | Yes                   | No                 | No         |

---

## ref Returns and ref Locals

A method can return a reference to a variable instead of a copy of its value. The caller keeps that reference in a `ref` local, and changes through it affect the original.

```csharp
static ref int FindMax(int[] array)
{
    int index = 0;
    for (int i = 1; i < array.Length; i++)
    {
        if (array[i] > array[index]) index = i;
    }
    return ref array[index];
}

int[] scores = { 10, 20, 30 };

ref int best = ref FindMax(scores);   // ref local: an alias of scores[2]
best = 99;
Console.WriteLine(string.Join(",", scores));   // 10,20,99

int copy = FindMax(scores);           // no 'ref': the value is copied
copy = -1;
Console.WriteLine(string.Join(",", scores));   // 10,20,99 (unchanged)
```

Rules:

| Rule                                                                  | Why                                                                    |
| --------------------------------------------------------------------- | ---------------------------------------------------------------------- |
| Write `ref` in three places: return type, `return ref`, and the local | The compiler needs each one to be explicit                             |
| Return only things that outlive the method                            | Arrays, fields, and `ref` parameters are fine; a local variable is not |
| Use `ref readonly` to hand out a view without allowing changes        | Avoids copying large structs safely                                    |

```csharp
public ref readonly int First() => ref Values[0];

ref readonly int view = ref big.First();   // read-only alias, no copy
```

### A Practical Use: One Dictionary Lookup

```csharp
using System.Runtime.InteropServices;

var counts = new Dictionary<string, int>();
foreach (string word in "a b a c a b".Split(' '))
{
    ref int count = ref CollectionsMarshal.GetValueRefOrAddDefault(counts, word, out _);
    count++;   // finds the slot once, and updates it in place
}
```

The usual `counts[word] = counts.GetValueOrDefault(word) + 1` looks the key up twice.

Use `ref` returns for hot paths with large structs or arrays. For everyday code, normal return values are clearer.

---

## Gotchas

- Default values are baked into the call site at compile time: changing a default in a library requires recompiling callers
- `params` causes heap allocation; prefer `Span<T>` in hot paths
- `ref` and `out` cannot be used with `async` methods
- Named arguments improve readability but can break if parameters are renamed

---

## Examples

- [17-01](examples/17-01_parameter_modifiers.cs): ref, out, in, params, optional and named arguments
- [17-02](examples/17-02_ref_returns_and_locals.cs): ref locals, ref returns, and ref readonly

Run one with `dotnet run examples/17-01_parameter_modifiers.cs`. See [Examples](examples/README.md).
