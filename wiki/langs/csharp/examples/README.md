# C# Examples

Tags: `csharp` `examples` `langs`

Small runnable programs for the lessons in this folder. Each one is a single `.cs` file.

## Naming

`NN-MM_name.cs`: `NN` is the lesson number and `MM` is the example number within that lesson.

A lesson with no code has a one-line file `NN-00_no_examples.txt` that says so.

---

## Requirements

The .NET 10 SDK or newer. Check with `dotnet --version`. See [Getting Started](../01_getting_started.md).

---

## Run an Example

From this folder:

```bash
dotnet run 01-01_hello_world.cs
```

| Example needs          | How to run it                                         |
| ---------------------- | ----------------------------------------------------- |
| Nothing                | `dotnet run 12-01_math_tour.cs`                       |
| Command-line arguments | `dotnet run 05-03_args_and_exit_code.cs -- Alice Bob` |
| Typed input            | `dotnet run 05-01_greeting.cs`, then type when asked  |
| A NuGet package        | The same command. The first run downloads the package |

The expected output is in a comment at the end of each file. A few examples print values that change on every run, such as timings. Those files say so instead.

---

## How the Files Are Set Up

`dotnet run file.cs` builds and runs one file with no project. Lines that start with `#:` at the top of a file are settings for that build:

| Line                          | Meaning                                                               |
| ----------------------------- | --------------------------------------------------------------------- |
| `#:package Name@version`      | Use a NuGet package                                                   |
| `#:property PublishAot=false` | Turn off the ahead-of-time defaults. Needed for reflection-based JSON |

---

## Examples

| Example                                            | What it shows                                                    | Lesson                                                                             |
| -------------------------------------------------- | ---------------------------------------------------------------- | ---------------------------------------------------------------------------------- |
| [01-01](01-01_hello_world.cs)                      | Hello, World                                                     | [01 Getting Started](../01_getting_started.md)                                     |
| [04-01](04-01_program_structure.cs)                | Program structure with an explicit Main                          | [04 Basics of a Program](../04_basics_of_a_program.md)                             |
| [04-02](04-02_preprocessor.cs)                     | Preprocessor directives                                          | [04 Basics of a Program](../04_basics_of_a_program.md)                             |
| [05-01](05-01_greeting.cs)                         | Read a name and greet                                            | [05 Console Input and Output](../05_console_input_and_output.md)                   |
| [05-02](05-02_format_table.cs)                     | Format numbers and align columns                                 | [05 Console Input and Output](../05_console_input_and_output.md)                   |
| [05-03](05-03_args_and_exit_code.cs)               | Command-line arguments and an exit code                          | [05 Console Input and Output](../05_console_input_and_output.md)                   |
| [05-04](05-04_validated_input.cs)                  | Ask until the input is valid                                     | [05 Console Input and Output](../05_console_input_and_output.md)                   |
| [06-01](06-01_namespaces_and_aliases.cs)           | Namespaces, using alias, and using static                        | [06 Namespaces & Packages](../06_namespaces_and_packages.md)                       |
| [07-01](07-01_variables_and_constants.cs)          | Variables, var, const, readonly                                  | [07 Variables & Constants](../07_variable_and_constants.md)                        |
| [08-01](08-01_primitive_ranges.cs)                 | Sizes and ranges of primitive types                              | [08 Primitive Data Types](../08_primitive_data_types.md)                           |
| [09-01](09-01_enums.cs)                            | Enums and flags                                                  | [09 Enums](../09_enums.md)                                                         |
| [10-01](10-01_type_conversion.cs)                  | Implicit, explicit, Convert, Parse, TryParse                     | [10 Type Conversion](../10_type_conversion.md)                                     |
| [11-01](11-01_operators.cs)                        | Arithmetic, comparison, logical, bitwise, ternary                | [11 Operators](../11_operators.md)                                                 |
| [12-01](12-01_math_tour.cs)                        | A tour of the Math class                                         | [12 Math, Random and Utility Types](../12_math_random_and_utilities.md)            |
| [12-02](12-02_dice_roller.cs)                      | Seeded random numbers                                            | [12 Math, Random and Utility Types](../12_math_random_and_utilities.md)            |
| [12-03](12-03_guid_and_environment.cs)             | Guid, Environment, and BigInteger                                | [12 Math, Random and Utility Types](../12_math_random_and_utilities.md)            |
| [13-01](13-01_grade_calculator.cs)                 | if, else if, and switch                                          | [13 Control Flow](../13_control_flow.md)                                           |
| [14-01](14-01_shape_describer.cs)                  | Pattern matching with is, switch, and list patterns              | [14 Pattern Matching](../14_pattern_matching.md)                                   |
| [15-01](15-01_times_table_and_fizzbuzz.cs)         | for, while, do-while, foreach                                    | [15 Loops](../15_loops.md)                                                         |
| [16-01](16-01_methods_tour.cs)                     | Parameters, return values, overloads, expression bodies          | [16 Methods](../16_methods.md)                                                     |
| [16-02](16-02_static_local_functions.cs)           | Local functions and static local functions                       | [16 Methods](../16_methods.md)                                                     |
| [17-01](17-01_parameter_modifiers.cs)              | ref, out, in, params, optional and named arguments               | [17 Parameters](../17_params.md)                                                   |
| [17-02](17-02_ref_returns_and_locals.cs)           | ref locals, ref returns, and ref readonly                        | [17 Parameters](../17_params.md)                                                   |
| [18-01](18-01_tuples.cs)                           | Tuples, named elements, and deconstruction                       | [18 Tuples](../18_tuples.md)                                                       |
| [19-01](19-01_array_basics.cs)                     | Create, read, change, slice, sort arrays                         | [19 Arrays](../19_arrays.md)                                                       |
| [20-01](20-01_grid_and_jagged.cs)                  | 2D and jagged arrays                                             | [20 Multidimensional Arrays](../20_multidimensional_arrays.md)                     |
| [21-01](21-01_string_tools.cs)                     | Common string operations                                         | [21 Strings](../21_strings.md)                                                     |
| [21-02](21-02_custom_formatting.cs)                | IFormattable, custom format strings, and Convert.ChangeType      | [21 Strings](../21_strings.md)                                                     |
| [22-01](22-01_dates_and_times.cs)                  | DateTime, DateOnly, TimeSpan, formatting                         | [22 Dates and Times](../22_dates_and_times.md)                                     |
| [22-02](22-02_time_provider.cs)                    | TimeProvider and a fake clock for testing                        | [22 Dates and Times](../22_dates_and_times.md)                                     |
| [23-01](23-01_value_vs_reference.cs)               | Value types vs reference types, boxing                           | [23 Reference Data Types](../23_reference_data_types.md)                           |
| [24-01](24-01_nullable_references.cs)              | Nullable reference types and null operators                      | [24 Nullable Reference Types](../24_nullable_reference_types.md)                   |
| [25-01](25-01_records.cs)                          | Records, with-expressions, and value equality                    | [25 Records & Equality](../25_records_and_equality.md)                             |
| [25-02](25-02_equality_in_depth.cs)                | Equals, GetHashCode, IEquatable, and comparers                   | [25 Records & Equality](../25_records_and_equality.md)                             |
| [26-01](26-01_bank_account.cs)                     | A class with fields, properties, and a constructor               | [26 Classes & Objects](../26_classes_and_objects.md)                               |
| [27-01](27-01_static_and_partial.cs)               | Static classes, static members, and partial classes              | [27 Static and Partial Classes](../27_static_and_partial_classes.md)               |
| [28-01](28-01_indexers_and_operators.cs)           | Indexers and operator overloading                                | [28 Indexers and Operator Overloading](../28_indexers_and_operator_overloading.md) |
| [29-01](29-01_animal_hierarchy.cs)                 | Inheritance, base, virtual, override, sealed                     | [29 Inheritance](../29_inheritance.md)                                             |
| [30-01](30-01_shapes_polymorphism.cs)              | Polymorphism with abstract classes and overloads                 | [30 Polymorphism](../30_polymorphism.md)                                           |
| [31-01](31-01_interfaces.cs)                       | Interfaces, default methods, explicit implementation             | [31 Interfaces & Abstractions](../31_interfaces_and_abstractions.md)               |
| [32-01](32-01_idisposable.cs)                      | IDisposable, using, and async disposal                           | [32 IDisposable](../32_idisposable.md)                                             |
| [32-02](32-02_dispose_pattern.cs)                  | The full dispose pattern and SafeHandle                          | [32 IDisposable](../32_idisposable.md)                                             |
| [33-01](33-01_span_and_memory.cs)                  | Span, stackalloc, Lazy, and allocation counting                  | [33 Memory and Garbage Collection](../33_memory_and_garbage_collection.md)         |
| [34-01](34-01_collections_tour.cs)                 | List, Dictionary, HashSet                                        | [34 Collections](../34_collections.md)                                             |
| [34-02](34-02_collection_interfaces.cs)            | IEnumerable, IReadOnlyList, IList, and API design                | [34 Collections](../34_collections.md)                                             |
| [35-01](35-01_queue_stack_linkedlist.cs)           | Queue, Stack, LinkedList, SortedDictionary                       | [35 Queue, Stack & More Collections](../35_queue_stack_and_more_collections.md)    |
| [36-01](36-01_iterators.cs)                        | yield return, lazy sequences, custom enumerables                 | [36 Iterators](../36_iterators.md)                                                 |
| [37-01](37-01_delegates_and_lambdas.cs)            | Delegates, Func, Action, closures, and multicast                 | [37 Delegates and Lambdas](../37_delegates_and_lambdas.md)                         |
| [38-01](38-01_events.cs)                           | Events with EventHandler                                         | [38 Events](../38_events.md)                                                       |
| [39-01](39-01_extension_methods.cs)                | Extension methods on string, IEnumerable, and an enum            | [39 Extension Methods](../39_extension_methods.md)                                 |
| [40-01](40-01_modern_features.cs)                  | Primary constructors, collection expressions, field keyword      | [40 Modern C# Features](../40_modern_csharp_features.md)                           |
| [41-01](41-01_linq_queries.cs)                     | LINQ: filter, project, group, join, aggregate                    | [41 LINQ](../41_linq.md)                                                           |
| [42-01](42-01_error_handling.cs)                   | try, catch, finally, throw, custom exceptions                    | [42 Error Handling](../42_error_handling.md)                                       |
| [43-01](43-01_find_the_bug.cs)                     | A program with a bug to find using the debugger                  | [43 Debugging](../43_debugging.md)                                                 |
| [43-02](43-02_stack_trace.cs)                      | Reading a stack trace                                            | [43 Debugging](../43_debugging.md)                                                 |
| [44-01](44-01_read_and_write_files.cs)             | File, StreamReader, StreamWriter, and Path                       | [44 File I/O: Read & Write](../44_file_io_read_write.md)                           |
| [45-01](45-01_streams.cs)                          | FileStream, MemoryStream, and copying                            | [45 File I/O: Streams](../45_file_io_streams.md)                                   |
| [46-01](46-01_json_roundtrip.cs)                   | Serialize and deserialize JSON                                   | [46 JSON](../46_json.md)                                                           |
| [47-01](47-01_regex_examples.cs)                   | Match, extract, replace with regular expressions                 | [47 Regular Expressions](../47_regular_expressions.md)                             |
| [48-01](48-01_async_await.cs)                      | async, await, Task.WhenAll, cancellation                         | [48 Async / Await / Tasks](../48_async_await_tasks.md)                             |
| [48-02](48-02_task_completion_source.cs)           | TaskCompletionSource: turn a callback into a Task                | [48 Async / Await / Tasks](../48_async_await_tasks.md)                             |
| [48-03](48-03_configure_await.cs)                  | SynchronizationContext, ConfigureAwait, and the .Result deadlock | [48 Async / Await / Tasks](../48_async_await_tasks.md)                             |
| [49-01](49-01_http_client.cs)                      | HttpClient talking to a local HttpListener                       | [49 HttpClient](../49_http_client.md)                                              |
| [50-01](50-01_threads_and_locks.cs)                | Race conditions, lock, Interlocked, SemaphoreSlim                | [50 Threading and Synchronization](../50_threading.md)                             |
| [50-02](50-02_timers_volatile_channels.cs)         | PeriodicTimer, Timer, volatile, and bounded Channel              | [50 Threading and Synchronization](../50_threading.md)                             |
| [50-03](50-03_mutex_and_threadpool.cs)             | ThreadPool, Mutex, and why not to mix Mutex with await           | [50 Threading and Synchronization](../50_threading.md)                             |
| [51-01](51-01_generics.cs)                         | Generic classes, methods, and constraints                        | [51 Generics](../51_generics.md)                                                   |
| [51-02](51-02_generic_math_and_static_abstract.cs) | static abstract members and generic math                         | [51 Generics](../51_generics.md)                                                   |
| [52-01](52-01_reflection_and_attributes.cs)        | Custom attributes and reflection                                 | [52 Attributes and Reflection](../52_attributes_and_reflection.md)                 |
| [53-01](53-01_dependency_injection.cs)             | A DI container with a singleton and a transient                  | [53 Dependency Injection](../53_dependency_injection.md)                           |
| [54-01](54-01_before_and_after_refactor.cs)        | Refactoring a messy method into clean code                       | [54 Conventions and Clean Code](../54_conventions_and_clean_code.md)               |
| [55-01](55-01_strategy_and_decorator.cs)           | Strategy, Decorator, and Factory together                        | [55 Design Patterns](../55_design_patterns.md)                                     |
| [55-02](55-02_observer_command_builder.cs)         | Observer, Command with undo, and Builder                         | [55 Design Patterns](../55_design_patterns.md)                                     |
| [58-01](58-01_measure_with_stopwatch.cs)           | Measure code with Stopwatch and GC counters                      | [58 Performance Basics](../58_performance_basics.md)                               |
| [59-01](59-01_hashing_and_passwords.cs)            | SHA-256, salted password hashes, HMAC                            | [59 Security Basics](../59_security_basics.md)                                     |
| [59-02](59-02_encrypt_and_validate.cs)             | AES-GCM encryption, input validation, and safe paths             | [59 Security Basics](../59_security_basics.md)                                     |
| [61-01](61-01_features_by_version.cs)              | One feature from each C# version                                 | [61 C# Version History](../61_csharp_version_history.md)                           |
| [63-01](63-01_configuration.cs)                    | Configuration from JSON, environment variables, and arguments    | [63 Common Libraries](../63_libraries.md)                                          |
| [63-02](63-02_logging.cs)                          | ILogger with the console provider                                | [63 Common Libraries](../63_libraries.md)                                          |
| [63-03](63-03_options_pattern.cs)                  | The options pattern with IOptions                                | [63 Common Libraries](../63_libraries.md)                                          |
