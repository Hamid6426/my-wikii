# 11 - Operators

## Arithmetic Operators

```csharp
int a = 10, b = 3;
int sum  = a + b;   // 13
int diff = a - b;   // 7
int prod = a * b;   // 30
int quot = a / b;   // 3  (integer division, truncates)
int rem  = a % b;   // 1
```

Floating-point division:

```csharp
double result = 10.0 / 3;   // 3.3333...
```

---

## Assignment Operators

```csharp
int x = 10;
x += 5;   // x = x + 5  → 15
x -= 3;   // x = x - 3  → 12
x *= 2;   // x = x * 2  → 24
x /= 4;   // x = x / 4  → 6
x %= 4;   // x = x % 4  → 2
```

---

## Increment and Decrement

```csharp
int n = 5;
n++;   // post-increment: use n, then increment → n = 6
++n;   // pre-increment: increment, then use → n = 7
n--;   // post-decrement
--n;   // pre-decrement
```

Post vs pre matters in expressions:

```csharp
int a = 5;
int b = a++;  // b = 5, a = 6
int c = ++a;  // a = 7, c = 7
```

---

## Comparison Operators

Return `bool`.

```csharp
int x = 10, y = 20;
bool eq  = x == y;  // false
bool neq = x != y;  // true
bool gt  = x > y;   // false
bool lt  = x < y;   // true
bool gte = x >= y;  // false
bool lte = x <= y;  // true
```

---

## Logical Operators

```csharp
bool a = true, b = false;
bool and = a && b;   // false: short-circuits if a is false
bool or  = a || b;   // true: short-circuits if a is true
bool not = !a;       // false
```

---

## Bitwise Operators

```csharp
int a = 0b_1010;  // 10
int b = 0b_1100;  // 12

int and  = a & b;   // 0b_1000 = 8
int or   = a | b;   // 0b_1110 = 14
int xor  = a ^ b;   // 0b_0110 = 6
int not  = ~a;      // bitwise NOT
int lsh  = a << 1;  // 0b_10100 = 20
int rsh  = a >> 1;  // 0b_0101 = 5
```

---

## Ternary Operator

```csharp
int age = 20;
string result = age >= 18 ? "Adult" : "Minor";
```

---

## Null-Coalescing Operators

```csharp
string name = null;
string display = name ?? "Anonymous";       // "Anonymous"

string s = null;
s ??= "default";   // assigns only if s is null
```

---

## Null-Conditional Operator

```csharp
string s = null;
int? len = s?.Length;  // null, no NullReferenceException

// Chaining
int? count = list?.Count;
string first = list?[0]?.Name;
```

---

## typeof and sizeof

```csharp
Type t = typeof(int);           // System.Int32
Console.WriteLine(sizeof(int)); // 4 (bytes)
```

---

## Operator Precedence (high to low)

| Level | Operators                   |
| ----- | --------------------------- |
| 1     | `()` `[]` `.` `?.`          |
| 2     | `!` `~` `++` `--` (unary)   |
| 3     | `*` `/` `%`                 |
| 4     | `+` `-`                     |
| 5     | `<<` `>>`                   |
| 6     | `<` `>` `<=` `>=` `is` `as` |
| 7     | `==` `!=`                   |
| 8     | `&`                         |
| 9     | `^`                         |
| 10    | `\|`                        |
| 11    | `&&`                        |
| 12    | `\|\|`                      |
| 13    | `??`                        |
| 14    | `?:` (ternary)              |
| 15    | `=` `+=` `-=` etc.          |

---

## Gotchas

- Integer division truncates: `7 / 2 == 3`, not `3.5`
- `&&` and `||` short-circuit; `&` and `|` do not
- `==` on strings compares values; on objects compares references (unless overridden)
- Prefix `++n` returns the new value; postfix `n++` returns the old value

---

## Examples

- [11-01](examples/11-01_operators.cs): Arithmetic, comparison, logical, bitwise, ternary

Run one with `dotnet run examples/11-01_operators.cs`. See [Examples](examples/README.md).
