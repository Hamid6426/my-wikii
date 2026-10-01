# 03 - IDE For C#

## Editor or IDE

| Term   | Meaning                                                                        |
| ------ | ------------------------------------------------------------------------------ |
| Editor | Edits text and adds features through extensions (VS Code)                      |
| IDE    | Integrated Development Environment: editor, debugger, project tools in one app |

You can write C# in any text editor and build from the terminal. An editor or IDE adds code completion, error underlines, and a debugger.

---

## Which One to Pick

| Tool          | Platform              | Cost                        | Best for                                 |
| ------------- | --------------------- | --------------------------- | ---------------------------------------- |
| VS Code       | Windows, macOS, Linux | Free                        | Learning, small projects, many languages |
| Visual Studio | Windows               | Community edition is free   | Large .NET projects, desktop apps        |
| Rider         | Windows, macOS, Linux | Free for non-commercial use | Full IDE on any OS                       |

The lessons use the terminal and work in all three. Start with VS Code unless you are on Windows and want a full IDE.

---

## VS Code

### Install

1. Download it from [code.visualstudio.com](https://code.visualstudio.com) and run the installer
2. Open it and press `Ctrl+Shift+X` to open the Extensions panel
3. Install **C# Dev Kit**. It also installs the **C#** and **.NET Install Tool** extensions

### Open a Project

1. Run `dotnet new console -n HelloWorld` in a terminal
2. In VS Code choose **File > Open Folder** and pick `HelloWorld`
3. Open `Program.cs`. If VS Code asks to add build and debug assets, accept

### Create a Project Inside VS Code

1. Press `Ctrl+Shift+P` to open the Command Palette
2. Run `.NET: New Project`
3. Choose **Console App**, then a folder and a name

### Run and Debug

| Action                 | Keys / command                                            |
| ---------------------- | --------------------------------------------------------- |
| Run without debugger   | `Ctrl+F5`, or `dotnet run` in the terminal (`` Ctrl+` ``) |
| Run with debugger      | `F5`                                                      |
| Breakpoint             | Click the left margin or press `F9`                       |
| Step over / into / out | `F10` / `F11` / `Shift+F11`                               |

---

## Visual Studio

Microsoft's full IDE for .NET on Windows. Visual Studio for Mac was retired in 2024, so use VS Code or Rider on macOS.

### Install

1. Download **Visual Studio Community** (free) from [visualstudio.microsoft.com](https://visualstudio.microsoft.com)
2. In the installer, select the **.NET desktop development** workload (add **ASP.NET and web development** for web work)
3. Click **Install**

### Create a Console App

1. Click **Create a new project**
2. Search for **Console App** and pick the C# one
3. Set the project name (for example `HelloWorld`) and a location
4. Choose the .NET version and click **Create**

### Run

| Action               | Keys           |
| -------------------- | -------------- |
| Run with debugger    | `F5`           |
| Run without debugger | `Ctrl+F5`      |
| Build                | `Ctrl+Shift+B` |

### Solution File

Visual Studio groups projects in a solution file (`.sln`, or `.slnx` in newer versions):

```
HelloWorld
├── HelloWorld.sln
└── HelloWorld
    ├── HelloWorld.csproj
    └── Program.cs
```

A solution can hold many projects, such as an app and its tests. See [Projects and the dotnet CLI](02_projects_and_cli.md).

### Main Features

| Feature           | What it does                               |
| ----------------- | ------------------------------------------ |
| IntelliSense      | Code completion and suggestions            |
| Debugger          | Breakpoints, variable inspection, stepping |
| Solution Explorer | File and project tree                      |
| NuGet Manager     | Add and remove packages with a UI          |
| Test Explorer     | Run and see tests                          |
| Live Share        | Code together in real time                 |

---

## JetBrains Rider

A full IDE that runs on Windows, macOS, and Linux.

1. Download it from [jetbrains.com/rider](https://www.jetbrains.com/rider/)
2. Choose **New Solution**, pick **Console**, set the name, and click **Create**
3. Run with the green arrow or `Shift+F10`, debug with `Shift+F9`

Strong refactoring and code analysis. Free for non-commercial use.

---

## Terminal and Other Editors

Any editor works because the build is just `dotnet build`. Neovim, Emacs, Helix, and Zed have C# language server support (`csharp-ls` or Roslyn based servers). You get completion and diagnostics but set it up yourself.

---

## Checklist for Any Setup

- Syntax colors and error underlines on `.cs` files
- Code completion while typing
- Format on save (`dotnet format` from the terminal does the same)
- A working debugger with breakpoints
- An integrated terminal to run `dotnet` commands

---

## Gotchas

- Install the .NET SDK first ([Getting Started](01_getting_started.md)); the editor does not replace it
- VS Code and Rider find the SDK on your path, so reopen them after installing it
- C# Dev Kit needs you to open the **folder** that holds the `.csproj`, not a single file
- C# Dev Kit has its own license terms, so check them before commercial use; the plain **C#** extension is the lighter option
