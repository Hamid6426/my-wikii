# C#

Tags: `csharp` `langs`

Notes for learning C#, the language and the .NET base library, in 64 short lessons. Start with [Getting Started](01_getting_started.md). Each lesson is one topic, most have a runnable [example](examples/README.md), and the [Glossary](64_glossary.md) explains the terms.

## Lessons

| No. | Lesson                                                                       |
| --- | ---------------------------------------------------------------------------- |
| 01  | [Getting Started](01_getting_started.md)                                     |
| 02  | [Projects and the dotnet CLI](02_projects_and_cli.md)                        |
| 03  | [IDE For C#](03_ide_for_csharp.md)                                           |
| 04  | [Basics of a Program](04_basics_of_a_program.md)                             |
| 05  | [Console Input and Output](05_console_input_and_output.md)                   |
| 06  | [Namespaces & Packages](06_namespaces_and_packages.md)                       |
| 07  | [Variables & Constants](07_variable_and_constants.md)                        |
| 08  | [Primitive Data Types](08_primitive_data_types.md)                           |
| 09  | [Enums](09_enums.md)                                                         |
| 10  | [Type Conversion](10_type_conversion.md)                                     |
| 11  | [Operators](11_operators.md)                                                 |
| 12  | [Math, Random and Utility Types](12_math_random_and_utilities.md)            |
| 13  | [Control Flow](13_control_flow.md)                                           |
| 14  | [Pattern Matching](14_pattern_matching.md)                                   |
| 15  | [Loops](15_loops.md)                                                         |
| 16  | [Methods](16_methods.md)                                                     |
| 17  | [Parameters](17_params.md)                                                   |
| 18  | [Tuples](18_tuples.md)                                                       |
| 19  | [Arrays](19_arrays.md)                                                       |
| 20  | [Multidimensional Arrays](20_multidimensional_arrays.md)                     |
| 21  | [Strings](21_strings.md)                                                     |
| 22  | [Dates and Times](22_dates_and_times.md)                                     |
| 23  | [Reference Data Types](23_reference_data_types.md)                           |
| 24  | [Nullable Reference Types](24_nullable_reference_types.md)                   |
| 25  | [Records & Equality](25_records_and_equality.md)                             |
| 26  | [Classes & Objects](26_classes_and_objects.md)                               |
| 27  | [Static and Partial Classes](27_static_and_partial_classes.md)               |
| 28  | [Indexers and Operator Overloading](28_indexers_and_operator_overloading.md) |
| 29  | [Inheritance](29_inheritance.md)                                             |
| 30  | [Polymorphism](30_polymorphism.md)                                           |
| 31  | [Interfaces & Abstractions](31_interfaces_and_abstractions.md)               |
| 32  | [IDisposable](32_idisposable.md)                                             |
| 33  | [Memory and Garbage Collection](33_memory_and_garbage_collection.md)         |
| 34  | [Collections](34_collections.md)                                             |
| 35  | [Queue, Stack & More Collections](35_queue_stack_and_more_collections.md)    |
| 36  | [Iterators](36_iterators.md)                                                 |
| 37  | [Delegates and Lambdas](37_delegates_and_lambdas.md)                         |
| 38  | [Events](38_events.md)                                                       |
| 39  | [Extension Methods](39_extension_methods.md)                                 |
| 40  | [Modern C# Features](40_modern_csharp_features.md)                           |
| 41  | [LINQ](41_linq.md)                                                           |
| 42  | [Error Handling](42_error_handling.md)                                       |
| 43  | [Debugging](43_debugging.md)                                                 |
| 44  | [File I/O: Read & Write](44_file_io_read_write.md)                           |
| 45  | [File I/O: Streams](45_file_io_streams.md)                                   |
| 46  | [JSON](46_json.md)                                                           |
| 47  | [Regular Expressions](47_regular_expressions.md)                             |
| 48  | [Async / Await / Tasks](48_async_await_tasks.md)                             |
| 49  | [HttpClient](49_http_client.md)                                              |
| 50  | [Threading and Synchronization](50_threading.md)                             |
| 51  | [Generics](51_generics.md)                                                   |
| 52  | [Attributes and Reflection](52_attributes_and_reflection.md)                 |
| 53  | [Dependency Injection](53_dependency_injection.md)                           |
| 54  | [Conventions and Clean Code](54_conventions_and_clean_code.md)               |
| 55  | [Design Patterns](55_design_patterns.md)                                     |
| 56  | [Testing](56_testing.md)                                                     |
| 57  | [Mocking & Fixtures](57_mocking_and_fixtures.md)                             |
| 58  | [Performance Basics](58_performance_basics.md)                               |
| 59  | [Security Basics](59_security_basics.md)                                     |
| 60  | [Publishing and Deployment](60_publishing_and_deployment.md)                 |
| 61  | [C# Version History](61_csharp_version_history.md)                           |
| 62  | [Common Frameworks](62_frameworks.md)                                        |
| 63  | [Common Libraries](63_libraries.md)                                          |
| 64  | [Glossary](64_glossary.md)                                                   |

## Practice

- [Top Interview 150](algorithms/README.md): LeetCode problems grouped by topic, with a checkbox for each.

---

## Adding Lessons

Only add a topic that is part of the foundation, core, or basics of C#.

- The topic is C# or the base library: things that ship with the .NET SDK, such as the language, `System.*`, and the `dotnet` tool
- A package or framework topic gets its own wiki folder, such as ASP.NET Core or Entity Framework Core. Only [Common Frameworks](62_frameworks.md) and [Common Libraries](63_libraries.md) point to them
- A package is allowed only for a core everyday skill: Dependency Injection, Testing, and Mocking. The lesson names the package it uses
- One topic per lesson. Split a lesson that covers two ideas, because short lessons are easier to read
- Place the lesson where its prerequisites come first. Insert it in order, then renumber the files and links after it

A new lesson needs:

- A file `NN_topic_name.md` with an `# NN - Title` heading, and a `---` line between every `##` section
- A row in the Lessons table above
- A runnable example in [examples](examples/README.md), or a `NN-00_no_examples.txt` placeholder if the topic has no code
- Terms the lesson introduces added to the [Glossary](64_glossary.md)
- Code that was compiled and run, with output written as "Expected output should be:" followed by a code block

---

## Examples

Runnable programs for the lessons are in [examples](examples/README.md).

- File name: `NN-MM_name.cs`, the lesson number and then the example number
- A lesson with no code has a one-line `NN-00_no_examples.txt` file that says so
- Format: one `.cs` file, run with `dotnet run NN-MM_name.cs` (.NET 10 SDK)
- Output: recorded at the end of the file as "Expected output should be:"
- Each lesson that has examples ends with an `## Examples` section linking to them
