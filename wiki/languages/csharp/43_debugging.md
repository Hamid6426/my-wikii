# 43 - Debugging

## What is Debugging

Finding why a program does not do what you expect. A debugger lets you pause the program, look at the values inside it, and run it one line at a time. It is faster than adding `Console.WriteLine` everywhere.

---

## The Debugging Loop

1. Reproduce the bug. Find the smallest input that triggers it
2. Guess where the wrong value first appears
3. Pause there and inspect the values
4. Change one thing, run again, and compare
5. Write a test so the bug cannot return (see [Testing](56_testing.md))

---

## Reading a Stack Trace

When a program crashes, the first lines tell you what and where.

```csharp
class Program
{
    static void Main()
    {
        Console.WriteLine(Divide(4, 0));
    }

    static int Divide(int a, int b)
    {
        return a / b;
    }
}
```

Expected output should be:

```
Unhandled exception. System.DivideByZeroException: Attempted to divide by zero.
   at Program.Divide(Int32 a, Int32 b) in /home/you/MyApp/Program.cs:line 10
   at Program.Main() in /home/you/MyApp/Program.cs:line 5
```

| Part                                 | Meaning                        |
| ------------------------------------ | ------------------------------ |
| `System.DivideByZeroException`       | The type of error              |
| `Attempted to divide by zero.`       | The message                    |
| `at Program.Divide(...) ... line 10` | Where it was thrown (top line) |
| `at Program.Main() ... line 5`       | Who called it (read downward)  |

Start at the top line that is in your own code. See [Error Handling](42_error_handling.md).

---

## Start the Debugger

| Tool          | How                                                |
| ------------- | -------------------------------------------------- |
| VS Code       | Press `F5`, or open Run and Debug (`Ctrl+Shift+D`) |
| Visual Studio | Press `F5`                                         |
| Rider         | Press `Shift+F9`                                   |

`dotnet run` does not attach a debugger. Use the editor's Run command. Set up the editor in [IDE For C#](03_ide_for_csharp.md).

---

## Breakpoints

A breakpoint pauses the program just before a line runs.

| Action                     | VS Code / Visual Studio                                      |
| -------------------------- | ------------------------------------------------------------ |
| Add or remove a breakpoint | Click the left margin, or press `F9`                         |
| Conditional breakpoint     | Right-click the red dot, then **Add Conditional Breakpoint** |
| Logpoint (print, no pause) | Right-click the margin, then **Add Logpoint**                |

A conditional breakpoint stops only when a condition is true, such as `i == 500` inside a loop. A logpoint prints a message such as `i = {i}` without stopping and without editing the code.

---

## Stepping

When paused on a line, control the run:

| Action    | Key (VS Code) | What it does                                 |
| --------- | ------------- | -------------------------------------------- |
| Continue  | `F5`          | Run until the next breakpoint                |
| Step Over | `F10`         | Run this line, do not go inside method calls |
| Step Into | `F11`         | Go inside the method being called            |
| Step Out  | `Shift+F11`   | Finish this method and return to the caller  |
| Stop      | `Shift+F5`    | End the debug session                        |

---

## Inspecting Values

While paused:

| Panel         | Shows                                                       |
| ------------- | ----------------------------------------------------------- |
| Variables     | Local variables and their current values                    |
| Watch         | Expressions you type, such as `items.Count` or `a / b`      |
| Call Stack    | The chain of methods that led here. Click one to jump to it |
| Debug Console | Type an expression to evaluate it, for example `total * 2`  |

Hover over a variable in the code to see its value. You can also change a value in the Variables panel to test a fix without editing code.

---

## Break on Exceptions

Stop at the line that throws, before the program crashes. In VS Code, open the Breakpoints panel and tick **User-Unhandled Exceptions** (or **All Exceptions** to stop even on handled ones). In Visual Studio use **Debug > Windows > Exception Settings**.

---

## Debug Helpers in Code

```csharp
using System.Diagnostics;

Debug.WriteLine("Value is " + count);               // goes to the debugger's output
Debug.Assert(count >= 0, "count must not be negative");

if (Debugger.IsAttached)
{
    Debugger.Break();                               // act as a breakpoint
}
```

`Debug.WriteLine` and `Debug.Assert` exist only in Debug builds. The compiler removes them from Release builds (see [Projects and the dotnet CLI](02_projects_and_cli.md)).

When a `Debug.Assert` fails outside the debugger, a console app prints "Process terminated. Assertion failed." with your message and stops. This is on purpose: the assert marks a bug in your code, not a normal error.

For output that must stay in Release builds, use a logging library. See [Common Libraries](63_libraries.md).

---

## Make Objects Easy to Read

Control what the debugger shows for your type.

```csharp
using System.Diagnostics;

[DebuggerDisplay("{Name} ({Age})")]
class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
}
```

In the Variables panel the object now shows as `Alice (30)` instead of the type name.

| Attribute               | Effect                                        |
| ----------------------- | --------------------------------------------- |
| `[DebuggerDisplay]`     | Text shown for the object                     |
| `[DebuggerStepThrough]` | Step Into skips this method                   |
| `[DebuggerHidden]`      | Hides the method from the debugger completely |

---

## VS Code Launch Settings

C# Dev Kit builds and runs for you. To control it yourself, create `.vscode/launch.json`:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Launch",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/bin/Debug/net10.0/MyApp.dll",
      "cwd": "${workspaceFolder}",
      "console": "integratedTerminal"
    }
  ]
}
```

`"console": "integratedTerminal"` is needed when the program reads input with `Console.ReadLine`. The default debug console cannot take typed input.

---

## Hot Reload

Change code while the program runs and see the effect without restarting.

```bash
dotnet watch
```

`dotnet watch` rebuilds and reruns when you save. Many edits apply instantly without losing state. Edits that change a type's shape need a restart.

---

## Debugging Without an IDE

| Need                            | Tool                                                          |
| ------------------------------- | ------------------------------------------------------------- |
| Print values                    | `Console.WriteLine` or logging                                |
| Look at a crashed process       | `dotnet-dump` (install: `dotnet tool install -g dotnet-dump`) |
| See what a running app is doing | `dotnet-trace`, `dotnet-counters`                             |
| Command-line debugger           | `netcoredbg` (third-party)                                    |

---

## Common Bug Patterns

| Symptom                           | Usual cause                                                         |
| --------------------------------- | ------------------------------------------------------------------- |
| `NullReferenceException`          | Something is `null`. Check the variable named on that line          |
| `IndexOutOfRangeException`        | Loop goes one step too far (`<=` instead of `<`)                    |
| Value is always zero or default   | Integer division (`1 / 2` is `0`) or a missing assignment           |
| Works in Debug, fails in Release  | Code with side effects inside `Debug.Assert`, or timing bugs        |
| Works once, wrong the second time | Shared state or a static field that is never reset                  |
| Loop never ends                   | The loop variable never changes, or the condition never turns false |

---

## Gotchas

- Breakpoints do not hit if the code you run is not the code you edited. Rebuild, and check you did not set the breakpoint in a file that is not part of the project
- Do not put work with side effects inside `Debug.Assert(...)`. It disappears in Release
- Optimized (Release) builds can skip lines or hide variables while debugging. Debug in the Debug configuration
- Pausing at a breakpoint in async or multithreaded code pauses only one thread. Others keep running
- Changing a variable in the debugger changes only this run

---

## Examples

- [43-01](examples/43-01_find_the_bug.cs): A program with a bug to find using the debugger
- [43-02](examples/43-02_stack_trace.cs): Reading a stack trace

Run one with `dotnet run examples/43-01_find_the_bug.cs`. See [Examples](examples/README.md).
