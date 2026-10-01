# 54 - Conventions and Clean Code

## Why Conventions Matter

Code is read far more than it is written. When everyone follows the same rules, any C# file looks familiar and mistakes stand out.

---

## Naming

| Kind                      | Style           | Example                |
| ------------------------- | --------------- | ---------------------- |
| Class, struct, record     | PascalCase      | `OrderService`         |
| Interface                 | `I` + Pascal    | `IOrderRepository`     |
| Method, property, event   | PascalCase      | `GetTotal`, `IsActive` |
| Local variable, parameter | camelCase       | `orderCount`           |
| Private field             | `_camelCase`    | `_logger`              |
| Constant                  | PascalCase      | `MaxRetries`           |
| Enum type and members     | PascalCase      | `Status.Active`        |
| Async method              | ends in `Async` | `LoadUserAsync`        |
| Type parameter            | `T` + name      | `TKey`, `TValue`       |

More on variables in [Variables and Constants](07_variable_and_constants.md).

Good names say what a thing is or does:

```csharp
// Hard to read
int d;
bool Check(User u) { ... }

// Clear
int daysSinceLogin;
bool IsEligibleForDiscount(User user) { ... }
```

| Rule                                        | Example                          |
| ------------------------------------------- | -------------------------------- |
| Methods are verbs                           | `CalculateTotal`, `SendEmail`    |
| Booleans read like a yes/no question        | `isValid`, `hasItems`, `canEdit` |
| Collections are plural                      | `users`, `orderLines`            |
| No abbreviations unless everyone knows them | `customer`, not `cust`           |
| No type in the name                         | `users`, not `userList`          |

---

## Layout

- One type per file, and the file has the type's name: `OrderService.cs`
- Match folders to namespaces
- Order inside a class: constants, fields, constructors, properties, public methods, private methods
- Braces on their own lines (the .NET style)
- Four spaces for indentation, no tabs

---

## .editorconfig

A file at the project root that sets the style for every editor and for the build.

```ini
root = true

[*.cs]
indent_style = space
indent_size = 4
end_of_line = lf
insert_final_newline = true

# Naming: private fields start with an underscore
dotnet_diagnostic.IDE1006.severity = warning
dotnet_naming_rule.private_fields_underscore.severity = warning
dotnet_naming_rule.private_fields_underscore.symbols = private_fields
dotnet_naming_rule.private_fields_underscore.style = underscore_camel

dotnet_naming_symbols.private_fields.applicable_kinds = field
dotnet_naming_symbols.private_fields.applicable_accessibilities = private

dotnet_naming_style.underscore_camel.required_prefix = _
dotnet_naming_style.underscore_camel.capitalization = camel_case

# Code style
csharp_style_var_for_built_in_types = false:suggestion
csharp_style_namespace_declarations = file_scoped:warning
csharp_prefer_braces = true:warning
```

Create one with `dotnet new editorconfig`. Check it into git so the whole team shares it.

The `dotnet_diagnostic.IDE1006.severity` line makes `dotnet build` report naming violations. Without it, they appear only in the editor and in `dotnet format style --verify-no-changes`.

---

## Format and Analyze from the Command Line

```bash
dotnet format                     # fix style and whitespace
dotnet format --verify-no-changes # fail if anything needs fixing (for CI)
```

Turn on stronger checks in the `.csproj`:

```xml
<PropertyGroup>
  <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
  <AnalysisLevel>latest-recommended</AnalysisLevel>
  <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
</PropertyGroup>
```

Analyzers are rules built into the SDK (and into packages such as StyleCop.Analyzers) that report problems while you type.

---

## Clean Code Habits

### Keep Methods Small

A method does one thing and fits on a screen. If you need a comment to explain a block, make that block its own method with a good name.

### Return Early

Use guard clauses so the main path is not nested.

```csharp
// Nested
if (user != null)
{
    if (user.IsActive)
    {
        Process(user);
    }
}

// Flat
if (user is null) return;
if (!user.IsActive) return;

Process(user);
```

### No Magic Numbers

```csharp
if (retries > 3) { }               // why 3?

const int MaxRetries = 3;
if (retries > MaxRetries) { }
```

### Comment the Why

```csharp
// Bad: repeats the code
i++;   // add one to i

// Good: explains a decision
// The API rate limit is 10 requests per second, so wait 100 ms between calls.
await Task.Delay(100);
```

### Prefer Immutable Data

Use `readonly`, `init`, and `record` so values cannot change by accident.

### Do Not Repeat Yourself (DRY)

Copy-pasted code means every fix must be made twice. Move it into one method. Wait until you see the same code three times before you make an abstraction.

### Keep It Simple (KISS) and Do Not Build It Yet (YAGNI)

Write the simplest thing that works. Do not add options, layers, or interfaces for needs that do not exist yet.

---

## SOLID

Five design rules for classes. They push code toward small, replaceable parts. Named solutions built on them are in [Design Patterns](55_design_patterns.md).

### S: Single Responsibility

A class has one reason to change.

```csharp
// Does too much: stores, emails, and formats
class OrderManager { void Save() { } void SendEmail() { } string ToPdf() { } }

// Split by job
class OrderRepository { void Save(Order o) { } }
class OrderNotifier   { void SendEmail(Order o) { } }
class OrderPdfWriter  { string ToPdf(Order o) => ""; }
```

### O: Open/Closed

Add new behavior by adding code, not by editing working code.

```csharp
interface IDiscount { decimal Apply(decimal price); }

class NoDiscount : IDiscount      { public decimal Apply(decimal p) => p; }
class HalfPrice : IDiscount       { public decimal Apply(decimal p) => p / 2; }

decimal Total(decimal price, IDiscount discount) => discount.Apply(price);
```

A new discount is a new class. `Total` never changes.

### L: Liskov Substitution

A child type must work anywhere its parent works. If `Penguin : Bird` throws in `Fly()`, the inheritance is wrong. See [Inheritance](29_inheritance.md).

### I: Interface Segregation

Many small interfaces beat one big one.

```csharp
interface IReader { string Read(); }
interface IWriter { void Write(string text); }

class LogFile : IReader, IWriter { /* ... */ }
class ReadOnlyConfig : IReader { /* ... */ }   // not forced to implement Write
```

### D: Dependency Inversion

Depend on an interface, not on a concrete class, and pass it in.

```csharp
class OrderService(IOrderRepository repo)   // gets what it needs from outside
{
    public void Place(Order order) => repo.Save(order);
}
```

This is the idea behind [Dependency Injection](53_dependency_injection.md), and it makes [testing](56_testing.md) easy because you can pass a fake.

---

## Composition over Inheritance

Prefer a class that has another class to a class that is one. Inheritance locks the design; composition can be swapped.

```csharp
// Inheritance: a Car is-a Engine? No.
class Car : Engine { }

// Composition: a Car has-an Engine
class Car(IEngine engine) { }
```

---

## Review Checklist

- Can I read each method name and know what it does?
- Does each class have one job?
- Are there magic numbers or copy-pasted blocks?
- Is there deep nesting that a guard clause would remove?
- Could a missing `null` check break this? (See [Nullable Reference Types](24_nullable_reference_types.md))
- Does `dotnet format --verify-no-changes` pass?

---

## Gotchas

- Rules are a guide. A tiny script does not need five interfaces
- Do not rename things only to match a style if the code is shared and stable; follow the style of the file you are in
- Over-abstraction is as bad as none: an interface with one implementation and no reason to swap it adds noise
- The underscore prefix for private fields is a common team choice, not a Microsoft rule; follow your project's `.editorconfig`

---

## Examples

- [54-01](examples/54-01_before_and_after_refactor.cs): Refactoring a messy method into clean code

Run one with `dotnet run examples/54-01_before_and_after_refactor.cs`. See [Examples](examples/README.md).
