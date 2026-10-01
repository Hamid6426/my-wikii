# 03 - IDE For Java

## Editor or IDE

| Kind   | What you get                                                                           | Examples                         |
| ------ | -------------------------------------------------------------------------------------- | -------------------------------- |
| Editor | Fast, light. Java support comes from extensions                                        | VS Code, Neovim, Zed             |
| IDE    | Integrated Development Environment: editor, build, debugger, refactoring, all built in | IntelliJ IDEA, Eclipse, NetBeans |

Both work. Java code has long names and many imports, so code completion and auto-import save a lot of typing. Use one of the tools below, not a plain text editor.

---

## Which One to Pick

| Tool            | Cost                              | Best for                                    |
| --------------- | --------------------------------- | ------------------------------------------- |
| IntelliJ IDEA   | Free tier, paid Ultimate features | Most Java developers. Strongest refactoring |
| VS Code         | Free                              | You already use VS Code for other languages |
| Eclipse IDE     | Free, open source                 | Older enterprise projects, Eclipse plugins  |
| Apache NetBeans | Free, open source                 | Teaching, simple Maven projects             |

Not sure? Start with IntelliJ IDEA. Tool editions and prices change, so check each site for the current offer.

---

## IntelliJ IDEA

Download from [jetbrains.com/idea](https://www.jetbrains.com/idea/download/), or use the JetBrains Toolbox app to install and update it.

1. **New Project**, pick **Java**
2. Pick a **JDK**. IntelliJ can download one for you if none is installed
3. Pick a **build system**: Maven or Gradle (see [Projects and Build Tools](02_projects_and_build_tools.md))
4. Open `Main.java` and click the green arrow next to `main`

| Shortcut (Windows/Linux) | macOS           | Action                            |
| ------------------------ | --------------- | --------------------------------- |
| `Shift Shift`            | `Shift Shift`   | Search everywhere                 |
| `Alt+Enter`              | `Option+Return` | Quick fix (add import, fix error) |
| `Ctrl+Space`             | `Ctrl+Space`    | Code completion                   |
| `Shift+F10`              | `Ctrl+R`        | Run                               |
| `Shift+F9`               | `Ctrl+D`        | Debug                             |
| `Shift+F6`               | `Shift+F6`      | Rename everywhere                 |
| `Ctrl+Alt+L`             | `Option+Cmd+L`  | Reformat code                     |

Opening an existing project: **File > Open**, then pick the folder that holds `pom.xml` or `build.gradle.kts`. IntelliJ reads the build file and sets everything up.

---

## VS Code

1. Install [VS Code](https://code.visualstudio.com/)
2. Install the **Extension Pack for Java** (by Microsoft). It adds language support, a debugger, a test runner, and Maven and Gradle support
3. Open the command palette (`Ctrl+Shift+P`) and run **Java: Create Java Project**, or open a folder that has a `pom.xml` or `build.gradle.kts`
4. Click **Run** or **Debug** above `main`

```bash
code --install-extension vscjava.vscode-java-pack
```

| Command palette entry                      | Use                                        |
| ------------------------------------------ | ------------------------------------------ |
| Java: Configure Java Runtime               | See and change which JDK each project uses |
| Java: Clean Java Language Server Workspace | Fix strange errors after big changes       |
| Java: Organize Imports                     | Add missing imports and remove unused ones |

Single files work too: open any `.java` file with a `main` method and click **Run**. See the [VS Code Java docs](https://code.visualstudio.com/docs/languages/java).

---

## Eclipse and NetBeans

| Tool            | Get it                                                                                              | Note                                               |
| --------------- | --------------------------------------------------------------------------------------------------- | -------------------------------------------------- |
| Eclipse IDE     | [eclipse.org/downloads](https://www.eclipse.org/downloads/), pick "Eclipse IDE for Java Developers" | Uses a "workspace" folder that holds many projects |
| Apache NetBeans | [netbeans.apache.org](https://netbeans.apache.org/)                                                 | Maven support works with no setup                  |

Both import Maven and Gradle projects. Your code does not depend on the IDE.

---

## Terminal and Other Editors

Any editor that speaks LSP (Language Server Protocol, a standard way for editors to get completion and errors from a separate program) can use **jdtls**, the Eclipse Java language server. Neovim, Helix, Zed, and Emacs all support it.

You can always skip the editor's run button and use the terminal:

```bash
java Main.java        # single file
./mvnw package        # Maven project
./gradlew run         # Gradle project
```

---

## Checklist for Any Setup

- `java -version` and `javac -version` work in a terminal
- The editor uses the same JDK version as the build file
- Opening a `.java` file shows completion and red underlines on errors
- A breakpoint stops the program when you debug (see [Debugging](43_debugging.md))
- Format on save is on, so the code style stays the same

---

## Gotchas

- The IDE can use a different JDK from the terminal; check the project JDK setting when the two disagree
- After changing `pom.xml` or `build.gradle.kts`, reload the project (IntelliJ shows a small reload button) or new libraries stay red
- IDE folders (`.idea/`, `.vscode/`, `.settings/`, `.project`, `.classpath`) are usually kept out of Git, except shared settings your team agrees on
- Deleting `.idea/` or cleaning the language server workspace fixes many "it compiles in the terminal but not in the IDE" problems
