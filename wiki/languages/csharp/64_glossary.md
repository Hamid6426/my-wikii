# 64 - Glossary

Short definitions of terms used across these lessons, in alphabetical order within each group. Each entry points to the lesson that teaches it.

## A to C

| Term               | Meaning                                                                                                                  | Lesson                                                               |
| ------------------ | ------------------------------------------------------------------------------------------------------------------------ | -------------------------------------------------------------------- |
| Abstract class     | A class that cannot be created directly and can leave members for children to fill in                                    | [Interfaces and Abstractions](31_interfaces_and_abstractions.md)     |
| Abstraction        | Showing what something does and hiding how it does it                                                                    | [Interfaces and Abstractions](31_interfaces_and_abstractions.md)     |
| Access modifier    | A keyword (`public`, `private`, and so on) that controls who can use a member                                            | [Classes and Objects](26_classes_and_objects.md)                     |
| Alias              | A short name for a namespace or type, made with `using Name = ...;`                                                      | [Namespaces and Packages](06_namespaces_and_packages.md)             |
| Allocation         | Reserving memory for a new object, usually on the heap                                                                   | [Memory and Garbage Collection](33_memory_and_garbage_collection.md) |
| API                | Application Programming Interface: the public types and methods one piece of code offers to another                      | [Interfaces and Abstractions](31_interfaces_and_abstractions.md)     |
| Argument           | The actual value you pass to a method (the method declares a parameter)                                                  | [Methods](16_methods.md)                                             |
| Assembly           | A compiled `.dll` or `.exe`: the unit you build, ship, and load                                                          | [Projects and the dotnet CLI](02_projects_and_cli.md)                |
| async / await      | Keywords for code that waits (network, disk) without blocking a thread                                                   | [Async / Await / Tasks](48_async_await_tasks.md)                     |
| Attribute          | Metadata attached to code with `[Brackets]`                                                                              | [Attributes and Reflection](52_attributes_and_reflection.md)         |
| Base class         | The class another class inherits from, also called the parent class                                                      | [Inheritance](29_inheritance.md)                                     |
| BCL                | Base Class Library: the standard types that ship with .NET (`List<T>`, `File`, `Math`)                                   | [Common Libraries](63_libraries.md)                                  |
| Benchmark          | A repeatable measurement of how fast code runs                                                                           | [Performance Basics](58_performance_basics.md)                       |
| Boxing             | Wrapping a value type in an `object` on the heap                                                                         | [Reference Data Types](23_reference_data_types.md)                   |
| Breakpoint         | A marker that pauses the program at a line so you can inspect it                                                         | [Debugging](43_debugging.md)                                         |
| Buffer             | A block of memory that holds data on its way between two places                                                          | [File I/O: Streams](45_file_io_streams.md)                           |
| Cache              | Storing a result so it can be reused instead of computed again                                                           | [Performance Basics](58_performance_basics.md)                       |
| Cancellation token | An object passed to async work so the caller can ask it to stop                                                          | [Async / Await / Tasks](48_async_await_tasks.md)                     |
| Cast               | Converting a value to another type by writing the type in parentheses, such as `(int)x`                                  | [Type Conversion](10_type_conversion.md)                             |
| Channel            | A thread-safe queue that passes items from producers to consumers                                                        | [Threading and Synchronization](50_threading.md)                     |
| Checked            | A context where integer overflow throws an exception instead of wrapping silently                                        | [Type Conversion](10_type_conversion.md)                             |
| CIL / IL           | Common Intermediate Language: the code the C# compiler produces before it runs                                           | [Getting Started](01_getting_started.md)                             |
| CLI                | Command-line interface: using a tool by typing commands, such as `dotnet run`                                            | [Projects and the dotnet CLI](02_projects_and_cli.md)                |
| Closure            | A lambda that uses variables from the method around it                                                                   | [Delegates and Lambdas](37_delegates_and_lambdas.md)                 |
| CLR                | Common Language Runtime: the part of .NET that runs your program, manages memory, and compiles IL                        | [Memory and Garbage Collection](33_memory_and_garbage_collection.md) |
| Comparer           | An object that defines how two items are compared or ordered, separate from the item type                                | [Records and Equality](25_records_and_equality.md)                   |
| Compile time       | The moment the compiler builds your code. Errors found here appear before the program runs. The other moment is run time | [Getting Started](01_getting_started.md)                             |
| Compiler           | Turns C# source into IL. The .NET one is called Roslyn                                                                   | [Projects and the dotnet CLI](02_projects_and_cli.md)                |
| Configuration      | Settings kept outside the code (files, environment variables) so they can change without a rebuild                       | [Common Libraries](63_libraries.md)                                  |
| Constructor        | Special method that sets up a new object                                                                                 | [Classes and Objects](26_classes_and_objects.md)                     |
| Contravariance     | Letting a generic type that consumes a base type be used where one for a derived type is expected (`in`)                 | [Generics](51_generics.md)                                           |
| Covariance         | Letting a generic type that produces a derived type be used as one that produces its base type (`out`)                   | [Generics](51_generics.md)                                           |
| Culture            | The language and region settings that change how numbers and dates are formatted                                         | [Dates and Times](22_dates_and_times.md)                             |
| Debugger           | A tool that pauses a running program and lets you step through it                                                        | [Debugging](43_debugging.md)                                         |

---

## D to I

| Term                 | Meaning                                                                                                                   | Lesson                                                               |
| -------------------- | ------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------- |
| Deadlock             | Two or more threads each waiting for the other to release something, so none can continue                                 | [Threading and Synchronization](50_threading.md)                     |
| Delegate             | A variable that holds a method                                                                                            | [Delegates and Lambdas](37_delegates_and_lambdas.md)                 |
| Dependency injection | Passing the objects a class needs in from outside instead of creating them inside                                         | [Dependency Injection](53_dependency_injection.md)                   |
| Design pattern       | A named, proven solution to a common design problem                                                                       | [Design Patterns](55_design_patterns.md)                             |
| Disposable           | A type with a `Dispose` method that frees resources                                                                       | [IDisposable](32_idisposable.md)                                     |
| Downcasting          | Treating a base-type reference as a more specific derived type. It can fail                                               | [Polymorphism](30_polymorphism.md)                                   |
| dynamic              | A type whose members are checked when the program runs, not when it compiles                                              | [Reference Data Types](23_reference_data_types.md)                   |
| Encapsulation        | Hiding a class's internals behind a small public surface                                                                  | [Classes and Objects](26_classes_and_objects.md)                     |
| Enum                 | A named set of constants                                                                                                  | [Enums](09_enums.md)                                                 |
| Event                | A notification an object raises that others can subscribe to                                                              | [Events](38_events.md)                                               |
| Exception            | An object that represents an error and unwinds the call stack                                                             | [Error Handling](42_error_handling.md)                               |
| Explicit conversion  | A conversion you must write with a cast because data might be lost                                                        | [Type Conversion](10_type_conversion.md)                             |
| Expression tree      | A lambda stored as data so a library can read and translate it                                                            | [LINQ](41_linq.md)                                                   |
| Extension method     | A method added to an existing type from outside                                                                           | [Extension Methods](39_extension_methods.md)                         |
| Fake                 | A simple stand-in object used in a test instead of the real one                                                           | [Mocking and Fixtures](57_mocking_and_fixtures.md)                   |
| Finalizer            | A method (`~Name`) the garbage collector runs before freeing an object, as a last-chance cleanup                          | [IDisposable](32_idisposable.md)                                     |
| Framework            | A large library that structures a whole app and calls your code                                                           | [Common Frameworks](62_frameworks.md)                                |
| Garbage collector    | The part of the runtime that frees memory nothing refers to                                                               | [Memory and Garbage Collection](33_memory_and_garbage_collection.md) |
| Generic              | Code written for a type chosen later, such as `List<T>`                                                                   | [Generics](51_generics.md)                                           |
| Generic math         | Writing one method for every number type by constraining to interfaces such as `INumber<T>`                               | [Generics](51_generics.md)                                           |
| Handle               | A reference to something the operating system owns, such as an open file                                                  | [IDisposable](32_idisposable.md)                                     |
| Hash                 | A fixed-size fingerprint of data that cannot be reversed                                                                  | [Security Basics](59_security_basics.md)                             |
| Hash code            | A number computed from an object, used by sets and dictionaries to find it fast. Equal objects must have equal hash codes | [Records and Equality](25_records_and_equality.md)                   |
| Heap                 | Memory for objects, cleaned by the garbage collector                                                                      | [Memory and Garbage Collection](33_memory_and_garbage_collection.md) |
| Hot path             | Code that runs very often, where small costs add up                                                                       | [Performance Basics](58_performance_basics.md)                       |
| HTTP                 | The protocol web browsers and web APIs use to send requests and responses                                                 | [HttpClient](49_http_client.md)                                      |
| IDE                  | Integrated Development Environment: editor, debugger, and tools in one app                                                | [IDE For C#](03_ide_for_csharp.md)                                   |
| Immutable            | Cannot change after it is created                                                                                         | [Records and Equality](25_records_and_equality.md)                   |
| Implicit conversion  | A conversion the compiler does for you because no data can be lost                                                        | [Type Conversion](10_type_conversion.md)                             |
| Inheritance          | A class taking the members of a parent class                                                                              | [Inheritance](29_inheritance.md)                                     |
| Injection            | An attack where user input becomes part of a command, such as SQL injection                                               | [Security Basics](59_security_basics.md)                             |
| Instance             | One object made from a class                                                                                              | [Classes and Objects](26_classes_and_objects.md)                     |
| Interface            | A contract: a list of members a type promises to have                                                                     | [Interfaces and Abstractions](31_interfaces_and_abstractions.md)     |
| Iterator             | A method that gives values one at a time with `yield return`                                                              | [Iterators](36_iterators.md)                                         |

---

## J to P

| Term             | Meaning                                                                               | Lesson                                                               |
| ---------------- | ------------------------------------------------------------------------------------- | -------------------------------------------------------------------- |
| JIT              | Just-In-Time compiler: turns IL into machine code while the program runs              | [Getting Started](01_getting_started.md)                             |
| JSON             | A text format for data, written with `{}`, `[]`, and `"key": value` pairs             | [JSON](46_json.md)                                                   |
| Lambda           | A short, nameless method written with `=>`                                            | [Delegates and Lambdas](37_delegates_and_lambdas.md)                 |
| Lazy evaluation  | Doing the work only when the result is needed, as with `yield` and LINQ queries       | [Iterators](36_iterators.md)                                         |
| Library          | Code packaged to be called from your program                                          | [Common Libraries](63_libraries.md)                                  |
| LINQ             | Language Integrated Query: query collections with `Where`, `Select`, and so on        | [LINQ](41_linq.md)                                                   |
| Literal          | A value written directly in code, such as `42`, `"text"`, or `3.14F`                  | [Primitive Data Types](08_primitive_data_types.md)                   |
| Lock             | A tool that lets only one thread run a block of code at a time                        | [Threading and Synchronization](50_threading.md)                     |
| Logging          | Recording what a program does, so problems can be found later                         | [Common Libraries](63_libraries.md)                                  |
| LTS              | Long-Term Support: a .NET version that gets fixes for three years                     | [C# Version History](61_csharp_version_history.md)                   |
| Marshalling      | Converting data between managed .NET types and native code types                      | [Memory and Garbage Collection](33_memory_and_garbage_collection.md) |
| Member           | Anything declared inside a type: a field, property, method, or event                  | [Classes and Objects](26_classes_and_objects.md)                     |
| Method           | A named block of code that can take inputs and return a value                         | [Methods](16_methods.md)                                             |
| Middleware       | A step in a web app's request pipeline that can handle or change a request            | [Common Frameworks](62_frameworks.md)                                |
| Mock             | A test object that records how it was used, so the test can check it                  | [Mocking and Fixtures](57_mocking_and_fixtures.md)                   |
| Mutable          | Can change after it is created                                                        | [Records and Equality](25_records_and_equality.md)                   |
| Mutex            | A lock that can work across processes, owned by one thread at a time                  | [Threading and Synchronization](50_threading.md)                     |
| Namespace        | A named group of types that prevents name clashes                                     | [Namespaces and Packages](06_namespaces_and_packages.md)             |
| Native AOT       | Compiling a program fully to machine code before it ships, with no JIT and no runtime | [Publishing and Deployment](60_publishing_and_deployment.md)         |
| NuGet            | The package manager and website for .NET libraries                                    | [Projects and the dotnet CLI](02_projects_and_cli.md)                |
| Nullable         | A type that may hold `null`: `int?`, `string?`                                        | [Nullable Reference Types](24_nullable_reference_types.md)           |
| Options pattern  | Binding a configuration section to a class and receiving it as `IOptions<T>`          | [Common Libraries](63_libraries.md)                                  |
| ORM              | Object-relational mapper: a library that maps database tables to classes              | [Common Frameworks](62_frameworks.md)                                |
| Overflow         | A number going past the largest or smallest value its type can hold                   | [Type Conversion](10_type_conversion.md)                             |
| Overload         | Several methods with the same name and different parameters                           | [Methods](16_methods.md)                                             |
| Override         | A child class replacing a parent's `virtual` method                                   | [Inheritance](29_inheritance.md)                                     |
| Package          | A library downloaded from NuGet                                                       | [Projects and the dotnet CLI](02_projects_and_cli.md)                |
| Parameter        | A variable in a method declaration that receives an argument                          | [Methods](16_methods.md)                                             |
| Parse            | Reading text and turning it into a value, such as `int.Parse("42")`                   | [Type Conversion](10_type_conversion.md)                             |
| Pattern matching | Testing a value's shape and pulling data out of it                                    | [Pattern Matching](14_pattern_matching.md)                           |
| Polymorphism     | Using different types through one common type or interface                            | [Polymorphism](30_polymorphism.md)                                   |
| Project          | A folder with a `.csproj` that builds into one assembly                               | [Projects and the dotnet CLI](02_projects_and_cli.md)                |
| Property         | A member that looks like a field but runs `get` and `set` code                        | [Classes and Objects](26_classes_and_objects.md)                     |
| Publish          | Producing a folder or file that can be deployed (`dotnet publish`)                    | [Publishing and Deployment](60_publishing_and_deployment.md)         |

---

## R to Z

| Term                    | Meaning                                                                                     | Lesson                                                               |
| ----------------------- | ------------------------------------------------------------------------------------------- | -------------------------------------------------------------------- |
| Race condition          | A bug where the result depends on the timing of threads                                     | [Threading and Synchronization](50_threading.md)                     |
| readonly                | A field that can be set only in its declaration or constructor                              | [Variables and Constants](07_variable_and_constants.md)              |
| Record                  | A type for data with value-based equality                                                   | [Records and Equality](25_records_and_equality.md)                   |
| Reference type          | A type stored on the heap; variables hold a reference to it (`class`)                       | [Reference Data Types](23_reference_data_types.md)                   |
| Reflection              | Reading type and member information while the program runs                                  | [Attributes and Reflection](52_attributes_and_reflection.md)         |
| REST                    | A style of web API that uses HTTP methods (`GET`, `POST`) on resource URLs                  | [HttpClient](49_http_client.md)                                      |
| RID                     | Runtime identifier: operating system plus CPU, such as `linux-x64`                          | [Publishing and Deployment](60_publishing_and_deployment.md)         |
| Runtime                 | The program that runs your compiled code (the CLR and its libraries)                        | [Getting Started](01_getting_started.md)                             |
| SafeHandle              | A class that owns an unmanaged handle and releases it reliably                              | [IDisposable](32_idisposable.md)                                     |
| Salt                    | Random bytes added to a password before hashing                                             | [Security Basics](59_security_basics.md)                             |
| SDK                     | Software Development Kit: the compiler and tools, including the `dotnet` command            | [Getting Started](01_getting_started.md)                             |
| Self-contained          | A published app that includes the .NET runtime                                              | [Publishing and Deployment](60_publishing_and_deployment.md)         |
| Serialization           | Turning an object into text or bytes (such as JSON) so it can be stored or sent             | [JSON](46_json.md)                                                   |
| Signature               | A method name plus its parameter types. Overloads differ in signature                       | [Methods](16_methods.md)                                             |
| Solution                | A file (`.sln` or `.slnx`) that groups projects                                             | [Projects and the dotnet CLI](02_projects_and_cli.md)                |
| Source generator        | A compiler add-on that writes code at build time                                            | [Regular Expressions](47_regular_expressions.md)                     |
| Span                    | A view over part of a block of memory, with no copy                                         | [Memory and Garbage Collection](33_memory_and_garbage_collection.md) |
| SQL                     | Structured Query Language: the language used to query relational databases                  | [Security Basics](59_security_basics.md)                             |
| Stack                   | Memory for local value types and method calls, freed when the method ends                   | [Memory and Garbage Collection](33_memory_and_garbage_collection.md) |
| Stack trace             | The list of method calls that led to an error                                               | [Debugging](43_debugging.md)                                         |
| static                  | Belongs to the type itself, not to any one instance                                         | [Static and Partial Classes](27_static_and_partial_classes.md)       |
| Static abstract member  | A static member an interface requires, called on the type itself, as in `T.Zero`            | [Generics](51_generics.md)                                           |
| Stream                  | A sequence of bytes you read or write one piece at a time                                   | [File I/O: Streams](45_file_io_streams.md)                           |
| String interpolation    | Putting values inside a string with `$"... {value} ..."`                                    | [Console Input and Output](05_console_input_and_output.md)           |
| Struct                  | A value type, copied on assignment                                                          | [Reference Data Types](23_reference_data_types.md)                   |
| Synchronization context | The rule that decides which thread code resumes on after an `await`, such as the UI thread  | [Async / Await / Tasks](48_async_await_tasks.md)                     |
| Target framework        | The .NET version a project builds for, such as `net10.0`                                    | [Projects and the dotnet CLI](02_projects_and_cli.md)                |
| Task                    | An object that stands for work that will finish later                                       | [Async / Await / Tasks](48_async_await_tasks.md)                     |
| TaskCompletionSource    | An object that creates a `Task` you finish by hand                                          | [Async / Await / Tasks](48_async_await_tasks.md)                     |
| Thread                  | One path of execution inside a program                                                      | [Threading and Synchronization](50_threading.md)                     |
| Thread pool             | A shared set of ready threads that run tasks and callbacks                                  | [Async / Await / Tasks](48_async_await_tasks.md)                     |
| Thread pool starvation  | Work waiting in the queue because every pool thread is blocked                              | [Threading and Synchronization](50_threading.md)                     |
| Top-level statements    | Code written in `Program.cs` without a class or `Main`                                      | [Basics of a Program](04_basics_of_a_program.md)                     |
| Trimming                | Removing unused code from a published app to make it smaller                                | [Publishing and Deployment](60_publishing_and_deployment.md)         |
| Tuple                   | A small group of values bundled together without a class                                    | [Tuples](18_tuples.md)                                               |
| Unboxing                | Taking a value type back out of an `object`                                                 | [Reference Data Types](23_reference_data_types.md)                   |
| Unmanaged resource      | Something the garbage collector cannot free, such as native memory or a file handle         | [IDisposable](32_idisposable.md)                                     |
| Upcasting               | Treating a derived-type reference as its base type. It always works                         | [Polymorphism](30_polymorphism.md)                                   |
| User secrets            | A development-only store for keys kept outside the project folder                           | [Common Libraries](63_libraries.md)                                  |
| Value type              | A type that holds its data directly (`int`, `struct`, `enum`)                               | [Reference Data Types](23_reference_data_types.md)                   |
| volatile                | A field modifier that makes reads and writes always go to memory, so other threads see them | [Threading and Synchronization](50_threading.md)                     |
| xUnit                   | The most common test framework for .NET                                                     | [Testing](56_testing.md)                                             |
