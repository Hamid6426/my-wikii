# Java

Tags: `java` `langs`

Notes for learning Java, the language and the JDK base library, in 67 short lessons. Start with [Getting Started](01_getting_started.md). Each lesson is one topic, most have a runnable [example](examples/README.md), and the [Glossary](67_glossary.md) explains the terms.

## Lessons

| No. | Lesson                                                                          |
| --- | ------------------------------------------------------------------------------- |
| 01  | [Getting Started](01_getting_started.md)                                        |
| 02  | [Projects and Build Tools](02_projects_and_build_tools.md)                      |
| 03  | [IDE For Java](03_ide_for_java.md)                                              |
| 04  | [Basics of a Program](04_basics_of_a_program.md)                                |
| 05  | [Console Input and Output](05_console_input_and_output.md)                      |
| 06  | [Packages and Imports](06_packages_and_imports.md)                              |
| 07  | [Variables and Constants](07_variables_and_constants.md)                        |
| 08  | [Primitive Data Types](08_primitive_data_types.md)                              |
| 09  | [Enums](09_enums.md)                                                            |
| 10  | [Type Conversion](10_type_conversion.md)                                        |
| 11  | [Operators](11_operators.md)                                                    |
| 12  | [Math, Random and Utility Classes](12_math_random_and_utilities.md)             |
| 13  | [Control Flow](13_control_flow.md)                                              |
| 14  | [Pattern Matching](14_pattern_matching.md)                                      |
| 15  | [Loops](15_loops.md)                                                            |
| 16  | [Methods](16_methods.md)                                                        |
| 17  | [Varargs](17_varargs.md)                                                        |
| 18  | [Arrays](18_arrays.md)                                                          |
| 19  | [Multidimensional Arrays](19_multidimensional_arrays.md)                        |
| 20  | [Strings](20_strings.md)                                                        |
| 21  | [Dates and Times](21_dates_and_times.md)                                        |
| 22  | [Reference Data Types](22_reference_data_types.md)                              |
| 23  | [Object and Wrapper Classes](23_object_and_wrapper_classes.md)                  |
| 24  | [Null and Optional](24_null_and_optional.md)                                    |
| 25  | [Records and Equality](25_records_and_equality.md)                              |
| 26  | [Classes and Objects](26_classes_and_objects.md)                                |
| 27  | [Static Members and Utility Classes](27_static_members_and_utility_classes.md)  |
| 28  | [Nested, Inner and Anonymous Classes](28_nested_inner_and_anonymous_classes.md) |
| 29  | [equals, hashCode and Comparable](29_equals_hashcode_and_comparable.md)         |
| 30  | [Inheritance](30_inheritance.md)                                                |
| 31  | [Polymorphism](31_polymorphism.md)                                              |
| 32  | [Interfaces and Abstract Classes](32_interfaces_and_abstract_classes.md)        |
| 33  | [AutoCloseable and try-with-resources](33_autocloseable.md)                     |
| 34  | [Memory and Garbage Collection](34_memory_and_garbage_collection.md)            |
| 35  | [JVM Internals](35_jvm_internals.md)                                            |
| 36  | [Collections](36_collections.md)                                                |
| 37  | [Queue, Deque and More Collections](37_queue_deque_and_more_collections.md)     |
| 38  | [Iterators and Iterable](38_iterators.md)                                       |
| 39  | [Lambdas and Functional Interfaces](39_lambdas_and_functional_interfaces.md)    |
| 40  | [Modern Java Features](40_modern_java_features.md)                              |
| 41  | [Streams](41_streams.md)                                                        |
| 42  | [Error Handling](42_error_handling.md)                                          |
| 43  | [Debugging](43_debugging.md)                                                    |
| 44  | [Logging](44_logging.md)                                                        |
| 45  | [File I/O: Read and Write](45_file_io_read_write.md)                            |
| 46  | [Serialization](46_serialization.md)                                            |
| 47  | [File I/O: Streams](47_file_io_streams.md)                                      |
| 48  | [JSON](48_json.md)                                                              |
| 49  | [Regular Expressions](49_regular_expressions.md)                                |
| 50  | [JDBC](50_jdbc.md)                                                              |
| 51  | [CompletableFuture and Async Code](51_completablefuture_and_async.md)           |
| 52  | [HTTP Client](52_http_client.md)                                                |
| 53  | [Threading](53_threading.md)                                                    |
| 54  | [Generics](54_generics.md)                                                      |
| 55  | [Annotations and Reflection](55_annotations_and_reflection.md)                  |
| 56  | [Dependency Injection](56_dependency_injection.md)                              |
| 57  | [Conventions and Clean Code](57_conventions_and_clean_code.md)                  |
| 58  | [Design Patterns](58_design_patterns.md)                                        |
| 59  | [Testing](59_testing.md)                                                        |
| 60  | [Mocking and Fixtures](60_mocking_and_fixtures.md)                              |
| 61  | [Performance Basics](61_performance_basics.md)                                  |
| 62  | [Security Basics](62_security_basics.md)                                        |
| 63  | [Packaging and Deployment](63_packaging_and_deployment.md)                      |
| 64  | [Java Version History](64_java_version_history.md)                              |
| 65  | [Common Frameworks](65_frameworks.md)                                           |
| 66  | [Common Libraries](66_libraries.md)                                             |
| 67  | [Glossary](67_glossary.md)                                                      |

## Practice

- [Top Interview 150](algorithms/README.md): LeetCode problems grouped by topic, with a checkbox for each.

---

## Adding Lessons

Only add a topic that is part of the foundation, core, or basics of Java.

- The topic is Java or the base library: things that ship with the JDK, such as the language, `java.*`, and the `javac`, `java` and `jar` tools
- A library or framework topic gets its own wiki folder, such as Spring or Hibernate. Only [Common Frameworks](65_frameworks.md) and [Common Libraries](66_libraries.md) point to them
- A library is allowed only for a core everyday skill: JSON, Dependency Injection, Testing, and Mocking. The lesson names the library it uses
- One topic per lesson. Split a lesson that covers two ideas, because short lessons are easier to read
- Place the lesson where its prerequisites come first. Insert it in order, then renumber the files and links after it

A new lesson needs:

- A file `NN_topic_name.md` with an `# NN - Title` heading, and a `---` line between every `##` section
- A row in the Lessons table above
- Terms the lesson introduces added to the [Glossary](67_glossary.md)
- A runnable example in [examples](examples/README.md), or a `NN-00_no_examples.txt` placeholder if the topic has no code
- Code that was compiled and run, with output written as "Expected output should be:" followed by a code block

---

## Related

- [Wiki home](../../README.md)
