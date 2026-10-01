#:package Microsoft.Extensions.DependencyInjection@10.0.12
#:package Microsoft.Extensions.Configuration@10.0.12
#:package Microsoft.Extensions.Options.ConfigurationExtensions@10.0.12
// Lesson 63: Common Libraries (../63_libraries.md)
// The options pattern with IOptions
// Run: dotnet run 63-03_options_pattern.cs

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
services.Configure<SmtpOptions>(config.GetSection("Smtp"));     // bind the section to the class
services.AddTransient<Mailer>();

using ServiceProvider provider = services.BuildServiceProvider();
Console.WriteLine(provider.GetRequiredService<Mailer>().Describe());

class SmtpOptions
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 25;
    public bool UseTls { get; set; }
}

class Mailer(IOptions<SmtpOptions> options)
{
    private readonly SmtpOptions _smtp = options.Value;

    public string Describe() => $"Sending through {_smtp.Host}:{_smtp.Port}, TLS {(_smtp.UseTls ? "on" : "off")}";
}

// Expected output should be:
// Sending through mail.example.com:587, TLS on
