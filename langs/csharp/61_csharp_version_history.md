# 61 - C# Version History

What each version of C# added. Use it to read old code and to know which features your SDK supports.

## Timeline

| C#  | Year | .NET          | Main additions                                                                                             |
| --- | ---- | ------------- | ---------------------------------------------------------------------------------------------------------- |
| 1.0 | 2002 | Framework 1.0 | Classes, structs, interfaces, events, properties, delegates                                                |
| 2.0 | 2005 | Framework 2.0 | Generics, iterators (`yield`), partial classes, nullable value types, anonymous methods                    |
| 3.0 | 2007 | Framework 3.5 | LINQ, lambdas, extension methods, `var`, auto-properties, object and collection initializers               |
| 4.0 | 2010 | Framework 4.0 | `dynamic`, named and optional arguments, generic covariance                                                |
| 5.0 | 2012 | Framework 4.5 | `async` and `await`, caller info attributes                                                                |
| 6.0 | 2015 | Framework 4.6 | String interpolation, `?.`, expression-bodied members, `nameof`, exception filters, `using static`         |
| 7.x | 2017 | Framework 4.7 | Tuples, `out` variables, basic pattern matching, local functions, discards, `ref` returns                  |
| 8.0 | 2019 | .NET Core 3.0 | Nullable reference types, ranges and indices, switch expressions, async streams, default interface methods |
| 9.0 | 2020 | .NET 5        | Records, `init` setters, top-level statements, target-typed `new`, `and` / `or` / `not` patterns           |
| 10  | 2021 | .NET 6        | File-scoped namespaces, global usings, record structs                                                      |
| 11  | 2022 | .NET 7        | Raw string literals, required members, list patterns, generic math                                         |
| 12  | 2023 | .NET 8        | Primary constructors, collection expressions, alias any type, default lambda parameters                    |
| 13  | 2024 | .NET 9        | `params` collections, the `Lock` type, partial properties                                                  |
| 14  | 2025 | .NET 10 (LTS) | Extension members, the `field` keyword, null-conditional assignment                                        |

Each C# version is tied to a .NET version. The compiler picks the C# version from your target framework, so you get new syntax by upgrading the SDK and the target.

---

## Which Version Am I Using

```bash
dotnet --version
```

| SDK / target framework | Default C# |
| ---------------------- | ---------- |
| .NET 10 (`net10.0`)    | 14         |
| .NET 9 (`net9.0`)      | 13         |
| .NET 8 (`net8.0`)      | 12         |
| .NET 7 (`net7.0`)      | 11         |
| .NET 6 (`net6.0`)      | 10         |

Override it in the `.csproj` only when you must:

```xml
<PropertyGroup>
  <LangVersion>12</LangVersion>
</PropertyGroup>
```

---

## Where Each Feature Is Taught

| Feature                  | Version | Lesson                                                     |
| ------------------------ | ------- | ---------------------------------------------------------- |
| Generics                 | 2.0     | [Generics](51_generics.md)                                 |
| Iterators                | 2.0     | [Iterators](36_iterators.md)                               |
| LINQ                     | 3.0     | [LINQ](41_linq.md)                                         |
| Lambdas                  | 3.0     | [Delegates and Lambdas](37_delegates_and_lambdas.md)       |
| Extension methods        | 3.0     | [Extension Methods](39_extension_methods.md)               |
| `async` / `await`        | 5.0     | [Async / Await / Tasks](48_async_await_tasks.md)           |
| String interpolation     | 6.0     | [Console Input and Output](05_console_input_and_output.md) |
| Tuples                   | 7.0     | [Tuples](18_tuples.md)                                     |
| Local functions          | 7.0     | [Modern C# Features](40_modern_csharp_features.md)         |
| Nullable reference types | 8.0     | [Nullable Reference Types](24_nullable_reference_types.md) |
| Switch expressions       | 8.0     | [Pattern Matching](14_pattern_matching.md)                 |
| Records                  | 9.0     | [Records and Equality](25_records_and_equality.md)         |
| Top-level statements     | 9.0     | [Basics of a Program](04_basics_of_a_program.md)           |
| File-scoped namespaces   | 10      | [Namespaces and Packages](06_namespaces_and_packages.md)   |
| List patterns            | 11      | [Pattern Matching](14_pattern_matching.md)                 |
| Required members         | 11      | [Nullable Reference Types](24_nullable_reference_types.md) |
| Primary constructors     | 12      | [Modern C# Features](40_modern_csharp_features.md)         |
| Collection expressions   | 12      | [Modern C# Features](40_modern_csharp_features.md)         |
| `Lock` type              | 13      | [Threading and Synchronization](50_threading.md)           |
| Extension members        | 14      | [Extension Methods](39_extension_methods.md)               |
| `field` keyword          | 14      | [Modern C# Features](40_modern_csharp_features.md)         |

---

## Release Rhythm

- A new C# version ships every November, together with a new .NET version
- Even-numbered .NET versions (6, 8, 10) are LTS: supported for three years
- Odd-numbered versions (7, 9) are STS: shorter support, so use them only if you upgrade often
- Old features are rarely removed, so old code keeps compiling

---

## Reading Old Code

| You see                                | It is                                                  |
| -------------------------------------- | ------------------------------------------------------ |
| `new List<int>()` on both sides        | Pre-C# 9 style; today `List<int> x = new();`           |
| `namespace X { ... }` with braces      | Pre-C# 10 style; file-scoped `namespace X;` is shorter |
| `string.Format("{0}", x)`              | Pre-C# 6; use interpolation                            |
| `if (x != null && x.Y != null)`        | Pre-C# 6; use `x?.Y`                                   |
| Private field plus property with logic | Pre-C# 14 style; the `field` keyword removes the field |
| A `Main` method in a `Program` class   | Pre-C# 9; top-level statements are optional now        |

---

## Where to Follow Changes

- [What's new in C#](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/) on Microsoft Learn
- The C# language proposals on GitHub (`dotnet/csharplang`)
- The .NET blog, which posts a "What's new" article for each release

---

## Examples

- [61-01](examples/61-01_features_by_version.cs): One feature from each C# version

Run one with `dotnet run examples/61-01_features_by_version.cs`. See [Examples](examples/README.md).
