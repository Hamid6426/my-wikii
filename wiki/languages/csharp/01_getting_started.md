# 01 - Getting Started

## What You Will Build

A small console program that prints text, running on your own machine in about five minutes.

---

## C# and .NET

| Name | What it is                                                                        |
| ---- | --------------------------------------------------------------------------------- |
| C#   | The programming language you write                                                |
| .NET | The platform that runs it: the runtime, the standard library, and the build tools |
| SDK  | The kit you install to build and run C# programs (includes the `dotnet` command)  |

C# runs on Windows, macOS, and Linux.

---

## How Your Code Runs

1. You write C# source code (`.cs` files)
2. The compiler turns it into **IL** (Intermediate Language) inside a `.dll` file
3. When you run the program, the **CLR** (Common Language Runtime) starts
4. The **JIT** (Just-In-Time compiler) inside the CLR turns IL into machine code for your CPU as the program runs
5. The CLR also manages memory and cleans up unused objects (the garbage collector)

Because the middle step is IL, the same `.dll` runs on any system that has the .NET runtime.

---

## Prerequisites

- A computer running Windows, macOS, or Linux
- A terminal (PowerShell, Terminal, or any Linux shell)
- A text editor (see [IDE For C#](03_ide_for_csharp.md); any editor works for now)
- Basic computer skills. No programming experience is needed.

---

## Install the .NET SDK

Download the installer for the latest LTS version from [dotnet.microsoft.com/download](https://dotnet.microsoft.com/en-us/download), or use a package manager:

| System  | Command                                  |
| ------- | ---------------------------------------- |
| Windows | `winget install Microsoft.DotNet.SDK.10` |
| macOS   | `brew install --cask dotnet-sdk`         |
| Fedora  | `sudo dnf install dotnet-sdk-10.0`       |
| Ubuntu  | `sudo apt install dotnet-sdk-10.0`       |

Package names carry the version number. Check the [install guide](https://learn.microsoft.com/en-us/dotnet/core/install/) for the current one.

LTS means long-term support: the version gets fixes for three years.

---

## Check the Install

Open a new terminal and run:

```bash
dotnet --version
```

It prints a version number such as `10.0.100`. An error like "command not found" means the SDK is not installed, or the terminal was opened before the install finished.

Other useful checks:

```bash
dotnet --list-sdks       # every installed SDK
dotnet --info            # full details about your setup
```

---

## Your First Program

```bash
dotnet new console -n HelloWorld
cd HelloWorld
dotnet run
```

Expected output should be:

```
Hello, World!
```

What each line did:

| Command                            | Effect                                                          |
| ---------------------------------- | --------------------------------------------------------------- |
| `dotnet new console -n HelloWorld` | Creates a folder `HelloWorld` with a ready-made console project |
| `cd HelloWorld`                    | Moves into that folder                                          |
| `dotnet run`                       | Builds the project and runs it                                  |

Open `Program.cs` to see the code:

```csharp
Console.WriteLine("Hello, World!");
```

Change the text, save, and run `dotnet run` again.

---

## Run a Single File (.NET 10+)

No project needed for quick experiments. Save this as `hello.cs`:

```csharp
Console.WriteLine("Hello from a single file");
```

Then run it:

```bash
dotnet run hello.cs
```

---

## If Something Goes Wrong

| Problem                            | Fix                                                               |
| ---------------------------------- | ----------------------------------------------------------------- |
| `dotnet: command not found`        | Close and reopen the terminal, then check the install again       |
| `Couldn't find a project to run`   | Run `dotnet run` from inside the folder that holds the `.csproj`  |
| Old version printed by `--version` | Another SDK is first on the path; check with `dotnet --list-sdks` |

---

## Next Steps

1. [Projects and the dotnet CLI](02_projects_and_cli.md): what the generated files are and the commands you will use every day
2. [IDE For C#](03_ide_for_csharp.md): pick an editor that gives you code completion and debugging
3. [Basics of a Program](04_basics_of_a_program.md): how a C# program is put together

---

## Examples

- [01-01](examples/01-01_hello_world.cs): Hello, World

Run one with `dotnet run examples/01-01_hello_world.cs`. See [Examples](examples/README.md).
