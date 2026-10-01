# 15 - Loops

## for Loop

Best when the number of iterations is known.

```csharp
for (int i = 0; i < 5; i++)
{
    Console.WriteLine(i);  // 0, 1, 2, 3, 4
}
```

Counting down:

```csharp
for (int i = 10; i >= 0; i--)
{
    Console.WriteLine(i);
}
```

---

## while Loop

Runs while condition is true. Condition checked before each iteration.

```csharp
int n = 0;
while (n < 5)
{
    Console.WriteLine(n);
    n++;
}
```

---

## do-while Loop

Runs at least once. Condition checked after each iteration.

```csharp
int n = 0;
do
{
    Console.WriteLine(n);
    n++;
} while (n < 5);
```

---

## foreach Loop

Iterates over any `IEnumerable`. No index access by default.

```csharp
string[] names = { "Alice", "Bob", "Carol" };
foreach (string name in names)
{
    Console.WriteLine(name);
}
```

Works with collections too:

```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
foreach (int n in numbers)
{
    Console.WriteLine(n);
}
```

---

## break

Exits the loop immediately.

```csharp
for (int i = 0; i < 10; i++)
{
    if (i == 5) break;
    Console.WriteLine(i);  // 0, 1, 2, 3, 4
}
```

---

## continue

Skips the rest of the current iteration and moves to the next.

```csharp
for (int i = 0; i < 5; i++)
{
    if (i == 2) continue;
    Console.WriteLine(i);  // 0, 1, 3, 4
}
```

---

## Nested Loops

```csharp
for (int i = 0; i < 3; i++)
{
    for (int j = 0; j < 3; j++)
    {
        Console.Write($"({i},{j}) ");
    }
    Console.WriteLine();
}
```

Breaking out of nested loops using a flag:

```csharp
bool found = false;
for (int i = 0; i < 5 && !found; i++)
{
    for (int j = 0; j < 5; j++)
    {
        if (i == 2 && j == 2) { found = true; break; }
    }
}
```

---

## Infinite Loop

```csharp
while (true)
{
    string input = Console.ReadLine();
    if (input == "quit") break;
}
```

---

## Loop with Index in foreach (C# 9+ / LINQ)

```csharp
foreach (var (item, index) in names.Select((v, i) => (v, i)))
{
    Console.WriteLine($"{index}: {item}");
}
```

---

## Gotchas

- `for` loop variable is scoped to the loop, not accessible after
- Modifying a collection inside `foreach` throws `InvalidOperationException`
- `do-while` always executes the body at least once, even if condition is initially false
- `break` in nested loops only breaks the innermost loop

---

## Examples

- [15-01](examples/15-01_times_table_and_fizzbuzz.cs): for, while, do-while, foreach

Run one with `dotnet run examples/15-01_times_table_and_fizzbuzz.cs`. See [Examples](examples/README.md).
