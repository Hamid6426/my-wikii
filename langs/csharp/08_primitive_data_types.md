# 08 - Primitive Data Types

## What are Primitive Data Types

Value types stored directly on the stack. In C#, they map to .NET base types.

---

## Integer Types

| Type     | Size   | Range                                                   | .NET Type       |
| -------- | ------ | ------------------------------------------------------- | --------------- |
| `sbyte`  | 8-bit  | -128 to 127                                             | `System.SByte`  |
| `byte`   | 8-bit  | 0 to 255                                                | `System.Byte`   |
| `short`  | 16-bit | -32,768 to 32,767                                       | `System.Int16`  |
| `ushort` | 16-bit | 0 to 65,535                                             | `System.UInt16` |
| `int`    | 32-bit | -2,147,483,648 to 2,147,483,647                         | `System.Int32`  |
| `uint`   | 32-bit | 0 to 4,294,967,295                                      | `System.UInt32` |
| `long`   | 64-bit | -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807 | `System.Int64`  |
| `ulong`  | 64-bit | 0 to 18,446,744,073,709,551,615                         | `System.UInt64` |

```csharp
int age = 25;
long population = 8_000_000_000L;  // L suffix for long
uint positiveOnly = 42U;
```

---

## Floating-Point Types

| Type      | Size    | Precision     | Suffix | .NET Type        |
| --------- | ------- | ------------- | ------ | ---------------- |
| `float`   | 32-bit  | ~6-9 digits   | `F`    | `System.Single`  |
| `double`  | 64-bit  | ~15-17 digits | `D`    | `System.Double`  |
| `decimal` | 128-bit | 28-29 digits  | `M`    | `System.Decimal` |

```csharp
float price = 9.99F;
double pi = 3.14159265358979;
decimal salary = 95_000.50M;  // use decimal for money - no floating-point errors
```

---

## Boolean

```csharp
bool isActive = true;
bool isDeleted = false;
```

---

## Character

```csharp
char grade = 'A';
char symbol = '\n';   // escape sequence
char unicode = '\u0041'; // 'A'
```

---

## Gotchas

- **Never use `float` or `double` for money** - use `decimal`
- `int` is the default for whole numbers, `double` for decimals unless specified
- Numeric literals support `_` separator for readability: `1_000_000`
- `long` requires `L` suffix, `float` requires `F`, `decimal` requires `M` - without suffix the compiler infers `int` or `double`
- `char` uses single quotes `' '`, `string` uses double quotes `" "` - not interchangeable

---

## Default Values

Uninitialized value types in a class default to:

| Type                         | Default |
| ---------------------------- | ------- |
| `int`, `long`                | `0`     |
| `float`, `double`, `decimal` | `0.0`   |
| `bool`                       | `false` |
| `char`                       | `'\0'`  |

---

## Examples

- [08-01](examples/08-01_primitive_ranges.cs): Sizes and ranges of primitive types

Run one with `dotnet run examples/08-01_primitive_ranges.cs`. See [Examples](examples/README.md).
