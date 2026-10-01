# 63 - Common Libraries

A library is a package of code you call from your own program. Install one from NuGet with `dotnet add package <Name>`. This page lists the best-known ones by job.

## Built Into .NET (no install)

| Namespace                        | What it does                      | Lesson                                                                       |
| -------------------------------- | --------------------------------- | ---------------------------------------------------------------------------- |
| `System.Collections`             | Lists, dictionaries, sets, queues | [Collections](34_collections.md)                                             |
| `System.Linq`                    | Query collections                 | [LINQ](41_linq.md)                                                           |
| `System.IO`                      | Files and streams                 | [File I/O](44_file_io_read_write.md)                                         |
| `System.Text.Json`               | Read and write JSON               | [JSON](46_json.md)                                                           |
| `System.Text.RegularExpressions` | Pattern matching on text          | [Regex](47_regular_expressions.md)                                           |
| `System.Net.Http`                | Call web APIs                     | [HttpClient](49_http_client.md)                                              |
| `System.Threading`               | Threads and locks                 | [Threading](50_threading.md)                                                 |
| `System.Reflection`              | Inspect types at runtime          | [Reflection](52_attributes_and_reflection.md)                                |
| `System.Diagnostics`             | `Debug`, `Stopwatch`, `Process`   | [Debugging](43_debugging.md), [Performance Basics](58_performance_basics.md) |
| `System.Security.Cryptography`   | Hashing, encryption, random bytes | [Security Basics](59_security_basics.md)                                     |

---

## Microsoft.Extensions (common app plumbing)

| Package                                    | What it does                            |
| ------------------------------------------ | --------------------------------------- |
| `Microsoft.Extensions.DependencyInjection` | Dependency injection container          |
| `Microsoft.Extensions.Configuration`       | Read settings from JSON, env vars, args |
| `Microsoft.Extensions.Logging`             | Logging with `ILogger`                  |
| `Microsoft.Extensions.Options`             | Typed settings classes                  |
| `Microsoft.Extensions.Http`                | `IHttpClientFactory`                    |
| `Microsoft.Extensions.Caching.Memory`      | In-memory cache                         |

---

## Configuration, Logging, and Secrets (packages)

These are not part of the base library and have no core lesson. Each is a NuGet package, and each belongs to the framework-level topics that get their own wiki folders later. Here is the shortest working start for each.

### Configuration

```bash
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.Configuration.EnvironmentVariables
dotnet add package Microsoft.Extensions.Configuration.CommandLine
dotnet add package Microsoft.Extensions.Configuration.Binder
```

```json
{
  "AppName": "Demo",
  "Database": { "Host": "localhost", "Port": 5432 }
}
```

Save that as `appsettings.json`, and copy it to the output folder with `<None Update="appsettings.json" CopyToOutputDirectory="PreserveNewest" />` in the `.csproj`.

```csharp
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

Console.WriteLine(config["AppName"]);               // Demo
int port = config.GetValue<int>("Database:Port");   // 5432
var db = config.GetSection("Database").Get<DatabaseOptions>();

class DatabaseOptions { public string Host { get; set; } = ""; public int Port { get; set; } }
```

Later sources win. Running `dotnet run -- --AppName=FromCli` or setting an `APPNAME` environment variable overrides the file. Use `Database__Host` (double underscore) for nested keys in environment variables.

### User Secrets (development only)

Keep keys out of the project folder and out of git.

```bash
dotnet user-secrets init
dotnet user-secrets set "ApiKey" "abc123"
dotnet user-secrets list
```

Add `Microsoft.Extensions.Configuration.UserSecrets` and call `.AddUserSecrets<Program>()` in the builder. The values are stored in your user profile, not in the project. They are not encrypted, so they are for development. In production use environment variables or a secret manager from your hosting platform. See [Security Basics](59_security_basics.md).

### Options Pattern

Bind a configuration section to a class, then ask for it through `IOptions<T>`.

```bash
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Options.ConfigurationExtensions
```

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

IConfiguration config = new ConfigurationBuilder()
    .AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["Smtp:Host"] = "mail.example.com",
        ["Smtp:Port"] = "587",
        ["Smtp:UseTls"] = "true"
    })
    .Build();

var services = new ServiceCollection();
services.Configure<SmtpOptions>(config.GetSection("Smtp"));   // bind the section to the class
services.AddTransient<Mailer>();

class SmtpOptions
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 25;
    public bool UseTls { get; set; }
}

class Mailer(IOptions<SmtpOptions> options)
{
    public string Describe() => $"Sending through {options.Value.Host}:{options.Value.Port}";
}
```

Expected output should be:

```
Sending through mail.example.com:587
```

| Interface             | Reads settings                                      |
| --------------------- | --------------------------------------------------- |
| `IOptions<T>`         | Once, at first use. Same values for the whole run   |
| `IOptionsSnapshot<T>` | Again for each scope (for example each web request) |
| `IOptionsMonitor<T>`  | Live, and tells you when the file changes           |

Classes receive a small typed object, not the whole `IConfiguration`, so they are easy to test: create a `SmtpOptions` and pass `Options.Create(smtp)`. See [Dependency Injection](53_dependency_injection.md).

### Logging

```bash
dotnet add package Microsoft.Extensions.Logging.Console
```

```csharp
using Microsoft.Extensions.Logging;

using ILoggerFactory factory = LoggerFactory.Create(builder =>
    builder.AddSimpleConsole().SetMinimumLevel(LogLevel.Debug));

ILogger logger = factory.CreateLogger("Demo");

logger.LogInformation("Started {App}", "Demo");
logger.LogWarning("Disk at {Percent}%", 91);

try { throw new InvalidOperationException("boom"); }
catch (Exception ex) { logger.LogError(ex, "Failed for user {UserId}", 42); }
```

Expected output should be:

```
info: Demo[0]
      Started Demo
warn: Demo[0]
      Disk at 91%
fail: Demo[0]
      Failed for user 42
      System.InvalidOperationException: boom
         at Program.<Main>$(String[] args) in ...
```

| Level            | Use for                                 |
| ---------------- | --------------------------------------- |
| `Trace`, `Debug` | Detail for developers                   |
| `Information`    | Normal events: started, saved           |
| `Warning`        | Something odd that did not stop the app |
| `Error`          | An operation failed                     |
| `Critical`       | The app cannot continue                 |

Write `{Name}` placeholders, not string interpolation, so log tools can search by field. Serilog and NLog (listed below) add files and other outputs.

### Benchmarking

`BenchmarkDotNet` gives precise, repeatable timings. Use `Stopwatch` first ([Performance Basics](58_performance_basics.md)) and move to it when you need exact numbers.

### Databases and Web

Entity Framework Core, Dapper, database drivers, and ASP.NET Core need packages or the web SDK. See [Common Frameworks](62_frameworks.md).

---

## Testing

| Library                    | What it does                                  |
| -------------------------- | --------------------------------------------- |
| xUnit                      | Test framework (see [Testing](56_testing.md)) |
| NUnit, MSTest              | Other test frameworks                         |
| Moq, NSubstitute           | Mocking                                       |
| FluentAssertions, Shouldly | Readable assertions                           |
| AutoFixture, Bogus         | Generate test data                            |
| Testcontainers             | Run real databases in Docker for tests        |
| BenchmarkDotNet            | Measure how fast code runs                    |

---

## Data and Mapping

| Library                                      | What it does                                       |
| -------------------------------------------- | -------------------------------------------------- |
| Dapper                                       | Small, fast SQL helper                             |
| Npgsql, MySql.Data, Microsoft.Data.SqlClient | Database drivers for PostgreSQL, MySQL, SQL Server |
| Microsoft.Data.Sqlite                        | SQLite driver                                      |
| AutoMapper, Mapster                          | Copy data between object types                     |
| StackExchange.Redis                          | Redis client                                       |
| MongoDB.Driver                               | MongoDB client                                     |

---

## Serialization and Formats

| Library                       | What it does                     |
| ----------------------------- | -------------------------------- |
| Newtonsoft.Json               | Older, feature-rich JSON library |
| CsvHelper                     | Read and write CSV               |
| YamlDotNet                    | Read and write YAML              |
| protobuf-net, Google.Protobuf | Protocol Buffers                 |
| HtmlAgilityPack, AngleSharp   | Parse HTML                       |
| ClosedXML, EPPlus             | Read and write Excel files       |
| QuestPDF, iText               | Create PDFs                      |

---

## Logging and Monitoring

| Library       | What it does                                 |
| ------------- | -------------------------------------------- |
| Serilog       | Structured logging with many outputs         |
| NLog          | Another popular logging library              |
| OpenTelemetry | Traces and metrics                           |
| Polly         | Retry, timeout, and circuit breaker policies |

---

## Validation, Patterns, and Messaging

| Library                  | What it does                          |
| ------------------------ | ------------------------------------- |
| FluentValidation         | Rules for validating objects          |
| MediatR                  | In-process requests and notifications |
| MassTransit, NServiceBus | Messaging between services            |
| Hangfire, Quartz.NET     | Background jobs and scheduling        |
| Scrutor, Autofac         | Extra dependency injection features   |

---

## Console and Utilities

| Library                               | What it does                                     |
| ------------------------------------- | ------------------------------------------------ |
| Spectre.Console                       | Rich console output: tables, colors, prompts     |
| Pastel                                | Colored console text                             |
| CommandLineParser, System.CommandLine | Parse command line arguments                     |
| Humanizer                             | Turn values into readable text ("3 minutes ago") |
| NodaTime                              | Better date and time handling                    |

---

## Imaging and Misc

| Library    | What it does                       |
| ---------- | ---------------------------------- |
| ImageSharp | Load, edit, and save images        |
| SkiaSharp  | 2D drawing                         |
| Refit      | Typed HTTP clients from interfaces |

---

## Picking a Library

- Check the NuGet page for download count, last update date, and license
- Prefer the built-in option when it does the job
- Look at open issues and whether the maintainers answer them
- Keep the number of packages small; every package is something to update later

---

## Examples

- [63-01](examples/63-01_configuration.cs): Configuration from JSON, environment variables, and arguments
- [63-02](examples/63-02_logging.cs): ILogger with the console provider
- [63-03](examples/63-03_options_pattern.cs): The options pattern with IOptions

Run one with `dotnet run examples/63-01_configuration.cs`. See [Examples](examples/README.md).
