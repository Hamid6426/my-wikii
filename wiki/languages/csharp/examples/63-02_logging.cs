#:package Microsoft.Extensions.Logging.Console@10.0.12
// Lesson 63: Common Libraries (../63_libraries.md)
// ILogger with the console provider
// Run: dotnet run 63-02_logging.cs

using Microsoft.Extensions.Logging;

using ILoggerFactory factory = LoggerFactory.Create(builder =>
    builder.AddSimpleConsole(o => o.SingleLine = true).SetMinimumLevel(LogLevel.Debug));

ILogger logger = factory.CreateLogger("Demo");

logger.LogDebug("Only developers need this");
logger.LogInformation("Order {OrderId} placed by {Customer}", 1001, "Alice");
logger.LogWarning("Disk is {Percent}% full", 91);

try
{
    throw new InvalidOperationException("payment gateway down");
}
catch (Exception ex)
{
    logger.LogError(ex, "Could not charge order {OrderId}", 1001);
}
