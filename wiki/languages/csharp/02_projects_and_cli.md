# 02 - Projects and the dotnet CLI

## What is a Project

A folder with a `.csproj` file and your source code. The `.csproj` tells the compiler what to build and which packages to use. One project builds into one program or library.

---

## Create a Project

```bash
dotnet new console -n MyApp
cd MyApp
```

| Option               | Meaning                                                  |
| -------------------- | -------------------------------------------------------- |
| `-n MyApp`           | Project name, and the name of the new folder             |
| `-o path`            | Output folder, if different from the name                |
| `--use-program-main` | Generate a `Main` method instead of top-level statements |

Without `-n`, the project is created in the current folder and takes that folder's name.

List other templates:

```bash
dotnet new list
```

Common ones: `console`, `classlib` (a library), `xunit` (tests), `web` (empty web app), `webapi`.

---

## Project Layout

Right after `dotnet new console`:

```
MyApp
├── MyApp.csproj
├── Program.cs
└── obj/
```

After `dotnet build`:

```
MyApp
├── MyApp.csproj
├── Program.cs
├── obj/
└── bin/
    └── Debug/
        └── net10.0/
```

| Item           | What it is                                                       |
| -------------- | ---------------------------------------------------------------- |
| `MyApp.csproj` | Project settings: target framework, packages, build options      |
| `Program.cs`   | Your code and the entry point                                    |
| `obj/`         | Temporary build files and the package restore results. Generated |
| `bin/`         | The built program. Generated                                     |

Do not commit `bin/` and `obj/`. Add them to `.gitignore`.

---

## The .csproj File

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

| Setting           | Meaning                                                                                        |
| ----------------- | ---------------------------------------------------------------------------------------------- |
| `OutputType`      | `Exe` for a program, omitted for a library                                                     |
| `TargetFramework` | Which .NET version to build for (`net10.0`)                                                    |
| `ImplicitUsings`  | Adds common `using` lines automatically (see [Namespaces](06_namespaces_and_packages.md))      |
| `Nullable`        | Turns on null-safety warnings (see [Nullable Reference Types](24_nullable_reference_types.md)) |

---

## What is in bin/

After a build, `bin/Debug/net10.0/` holds:

| File                       | What it is                                           |
| -------------------------- | ---------------------------------------------------- |
| `MyApp.dll`                | Your compiled code                                   |
| `MyApp` / `MyApp.exe`      | Launcher for the current OS (`.exe` on Windows)      |
| `MyApp.pdb`                | Debug symbols, so the debugger can show source lines |
| `MyApp.runtimeconfig.json` | Which .NET runtime version to use                    |
| `MyApp.deps.json`          | List of the packages the program needs               |

---

## Build and Run

```bash
dotnet build         # compile only
dotnet run           # build if needed, then run
```

`dotnet run` builds first when the code changed, so `dotnet build` is optional while developing.

Pass arguments to your program after `--`:

```bash
dotnet run -- hello world
```

---

## Clean and Rebuild

```bash
dotnet clean         # delete build output
dotnet build         # build again from scratch
```

Use it when the build behaves strangely after moving files or switching branches.

---

## Debug and Release

| Mode    | Command                   | Use for                            |
| ------- | ------------------------- | ---------------------------------- |
| Debug   | `dotnet build` (default)  | Development: slower, easy to debug |
| Release | `dotnet build -c Release` | What you ship: optimized, smaller  |

---

## Packages

Add libraries from [NuGet](https://www.nuget.org), the .NET package site.

```bash
dotnet add package Newtonsoft.Json
dotnet list package
dotnet remove package Newtonsoft.Json
```

This edits the `.csproj` and downloads the package. `dotnet restore` downloads again anything that is missing. It runs automatically on build.

---

## Publish

Package the app for deployment.

```bash
dotnet publish -c Release -o out
```

The `out/` folder holds everything needed to run the app on a machine with .NET installed. Add `--self-contained` to include the runtime too. See [Publishing and Deployment](60_publishing_and_deployment.md) for single-file, trimmed, and native builds.

---

## Solutions and Several Projects

A solution (`.sln` or `.slnx`) groups projects, for example an app plus its tests.

```bash
dotnet new sln -n MySolution
dotnet sln add MyApp/MyApp.csproj
dotnet sln add MyApp.Tests/MyApp.Tests.csproj
dotnet add MyApp.Tests reference MyApp/MyApp.csproj    # tests use the app
```

Running `dotnet build` in the solution folder builds every project.

---

## Command Reference

| Command                       | What it does                          |
| ----------------------------- | ------------------------------------- |
| `dotnet new <template>`       | Create a project from a template      |
| `dotnet restore`              | Download packages                     |
| `dotnet build`                | Compile                               |
| `dotnet run`                  | Build and run                         |
| `dotnet watch`                | Rebuild and rerun when a file changes |
| `dotnet test`                 | Run tests                             |
| `dotnet clean`                | Remove build output                   |
| `dotnet publish`              | Package for deployment                |
| `dotnet add package <name>`   | Add a NuGet package                   |
| `dotnet add reference <path>` | Reference another project             |
| `dotnet --help`               | Show all commands                     |
| `dotnet <command> --help`     | Help for one command                  |

---

## Gotchas

- Run commands from the folder with the `.csproj`, or point at it: `dotnet run --project MyApp`
- With several `.csproj` files in one folder, `dotnet` cannot choose and fails
- Editing files in `bin/` or `obj/` is pointless; the next build overwrites them
- `dotnet watch` is the fastest loop for learning: save the file and the program reruns
