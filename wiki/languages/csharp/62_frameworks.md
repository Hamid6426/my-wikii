# 62 - Common Frameworks

A framework gives you the structure of a whole app and calls your code. A library is something you call. This page is a map only. Each framework gets its own folder in the wiki when it is studied.

## Web and Services

| Framework                 | What it is for                                                                             |
| ------------------------- | ------------------------------------------------------------------------------------------ |
| ASP.NET Core              | Web apps, REST APIs, and real-time apps. Each request passes through a chain of middleware |
| ASP.NET Core Minimal APIs | Small HTTP APIs with very little code                                                      |
| ASP.NET Core MVC          | Server-rendered websites with controllers and views                                        |
| Razor Pages               | Page-based server-rendered sites                                                           |
| Blazor                    | Web UI written in C# instead of JavaScript (server or WebAssembly)                         |
| SignalR                   | Real-time messaging (chat, live dashboards)                                                |
| gRPC for .NET             | Fast service-to-service calls                                                              |
| Azure Functions           | Small cloud functions that run on events                                                   |

---

## Data

| Framework             | What it is for                                                       |
| --------------------- | -------------------------------------------------------------------- |
| Entity Framework Core | Object-relational mapper (ORM): use C# classes instead of SQL tables |
| ADO.NET               | Low-level database access that EF Core sits on top of                |

---

## Desktop

| Framework     | What it is for                                      |
| ------------- | --------------------------------------------------- |
| Windows Forms | Simple, older drag-and-drop Windows apps            |
| WPF           | Windows desktop apps with XAML layouts              |
| WinUI 3       | Modern native Windows apps                          |
| Avalonia UI   | Cross-platform desktop apps (Windows, macOS, Linux) |
| Uno Platform  | One C# codebase for desktop, mobile, and web        |

---

## Mobile and Cross-Platform

| Framework | What it is for                                             |
| --------- | ---------------------------------------------------------- |
| .NET MAUI | Apps for Android, iOS, macOS, and Windows from one project |
| Xamarin   | The older mobile framework that MAUI replaced              |

---

## Games and Graphics

| Framework            | What it is for                          |
| -------------------- | --------------------------------------- |
| Unity                | Game engine that uses C# for scripting  |
| Godot (.NET version) | Open-source game engine with C# support |
| MonoGame             | Code-first 2D and 3D game framework     |

---

## Machine Learning and Data

| Framework             | What it is for                                |
| --------------------- | --------------------------------------------- |
| ML.NET                | Train and use machine learning models from C# |
| .NET for Apache Spark | Big data processing from C#                   |

---

## Hosting and Background Work

| Framework         | What it is for                                                        |
| ----------------- | --------------------------------------------------------------------- |
| .NET Generic Host | Startup, dependency injection, configuration, and logging for any app |
| Worker Services   | Long-running background programs and services                         |
| .NET Aspire       | Build and run cloud apps made of several services                     |

---

## Where to Start

- Web or backend work: ASP.NET Core, then Entity Framework Core
- Windows desktop: WPF, or Avalonia if it must run on Linux and macOS too
- Games: Unity
- Mobile: .NET MAUI

The C# language lessons here apply to all of them. Learn the language first, then pick one framework.
