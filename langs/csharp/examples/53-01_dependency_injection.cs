#:package Microsoft.Extensions.DependencyInjection@10.0.12
// Lesson 53: Dependency Injection (../53_dependency_injection.md)
// A DI container with a singleton and a transient
// Run: dotnet run 53-01_dependency_injection.cs

using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<IClock, SystemClock>();
services.AddSingleton<IMessageStore, MemoryStore>();
services.AddTransient<GreetingService>();

using ServiceProvider provider = services.BuildServiceProvider();

var first = provider.GetRequiredService<GreetingService>();
var second = provider.GetRequiredService<GreetingService>();

first.Greet("Alice");
second.Greet("Bob");

Console.WriteLine($"Same GreetingService? {ReferenceEquals(first, second)}");
Console.WriteLine($"Messages stored: {provider.GetRequiredService<IMessageStore>().All.Count}");

using (IServiceScope scope = provider.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<GreetingService>().Greet("Carol");
}

foreach (string m in provider.GetRequiredService<IMessageStore>().All) Console.WriteLine(m);

interface IClock { string Now(); }
class SystemClock : IClock { public string Now() => "12:00"; }

interface IMessageStore
{
    List<string> All { get; }
}
class MemoryStore : IMessageStore { public List<string> All { get; } = []; }

class GreetingService(IClock clock, IMessageStore store)
{
    public void Greet(string name) => store.All.Add($"[{clock.Now()}] Hello, {name}");
}

// Expected output should be:
// Same GreetingService? False
// Messages stored: 2
// [12:00] Hello, Alice
// [12:00] Hello, Bob
// [12:00] Hello, Carol
