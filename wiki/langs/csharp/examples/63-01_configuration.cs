#:package Microsoft.Extensions.Configuration.Json@10.0.12
#:package Microsoft.Extensions.Configuration.EnvironmentVariables@10.0.12
#:package Microsoft.Extensions.Configuration.CommandLine@10.0.12
#:package Microsoft.Extensions.Configuration.Binder@10.0.12
// Lesson 63: Common Libraries (../63_libraries.md)
// Configuration from JSON, environment variables, and arguments
// Run: dotnet run 63-01_configuration.cs

using Microsoft.Extensions.Configuration;

string folder = Path.Combine(Path.GetTempPath(), "csharp-example-63");
Directory.CreateDirectory(folder);
File.WriteAllText(Path.Combine(folder, "appsettings.json"), """
{
  "AppName": "Demo",
  "Database": { "Host": "localhost", "Port": 5432 }
}
""");

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(folder)
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables(prefix: "DEMO_")
    .AddCommandLine(args)
    .Build();

Console.WriteLine($"AppName:  {config["AppName"]}");
Console.WriteLine($"Host:     {config["Database:Host"]}");
Console.WriteLine($"Port:     {config.GetValue<int>("Database:Port")}");

var db = config.GetSection("Database").Get<DatabaseOptions>()!;
Console.WriteLine($"Bound:    {db.Host}:{db.Port}");

Console.WriteLine("Later sources win: run again with   -- --AppName=FromCli");

File.Delete(Path.Combine(folder, "appsettings.json"));
Directory.Delete(folder);

class DatabaseOptions
{
    public string Host { get; set; } = "";
    public int Port { get; set; }
}

// Expected output should be:
// AppName:  Demo
// Host:     localhost
// Port:     5432
// Bound:    localhost:5432
// Later sources win: run again with   -- --AppName=FromCli
