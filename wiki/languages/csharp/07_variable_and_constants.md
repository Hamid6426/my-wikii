# 07 - Variables & Constants

## Variable Declaration

```csharp
int age;           // declared, not initialized
int age = 25;      // declared + initialized
```

---

## var - Implicitly Typed

Compiler infers the type - type is still static, not dynamic.

```csharp
var name = "John";      // string
var age = 25;           // int
var price = 9.99M;      // decimal
```

- Only usable when initialized at declaration
- Avoid `var` when type is not obvious from the right-hand side

---

## Multiple Declarations

```csharp
int x = 1, y = 2, z = 3;
```

---

## Constants

Value set at compile time - cannot be changed.

```csharp
const double Pi = 3.14159;
const int MaxRetries = 3;
```

- Must be initialized at declaration
- Implicitly `static` - belongs to the type, not the instance
- Only works with primitive types and `string`

---

## readonly

Value set at runtime - can be assigned in constructor only.

```csharp
class Config
{
    readonly string _env;

    public Config(string env)
    {
        _env = env;   // only place you can assign readonly
    }
}
```

---

## const vs readonly

| Feature         | `const`             | `readonly`                |
| --------------- | ------------------- | ------------------------- |
| Set at          | Compile time        | Runtime (constructor)     |
| Can be `static` | Always static       | Can be instance or static |
| Types allowed   | Primitives + string | Any type                  |
| Changeable      | Never               | Only in constructor       |

---

## Naming Conventions

| Kind            | Convention  | Example             |
| --------------- | ----------- | ------------------- |
| Local variable  | camelCase   | `int userAge`       |
| Constant        | PascalCase  | `const int MaxSize` |
| Private field   | \_camelCase | `string _name`      |
| Public property | PascalCase  | `string FirstName`  |

---

## Gotchas

- `var` is not `dynamic` - type is fixed at compile time
- `const` values are inlined by the compiler - changing a `const` in a library requires recompiling all dependents
- Prefer `readonly` over `const` for complex or runtime-determined values

---

## Examples

- [07-01](examples/07-01_variables_and_constants.cs): Variables, var, const, readonly

Run one with `dotnet run examples/07-01_variables_and_constants.cs`. See [Examples](examples/README.md).
