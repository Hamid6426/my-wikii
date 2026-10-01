# 53 - Dependency Injection

## What is Dependency Injection

DI is a pattern where a class receives its dependencies from the outside rather than creating them itself. This decouples classes, makes them testable, and lets the container manage lifetimes.

---

## The Problem Without DI

```csharp
class OrderService
{
    private readonly EmailService _email = new EmailService();  // tightly coupled

    public void PlaceOrder(Order order)
    {
        // process...
        _email.Send(order.Email, "Order confirmed");
    }
}
```

`OrderService` is hard to test and cannot use a different email implementation.

---

## The Solution: Inject via Constructor

```csharp
interface IEmailService
{
    void Send(string to, string subject);
}

class EmailService : IEmailService
{
    public void Send(string to, string subject)
        => Console.WriteLine($"Sending '{subject}' to {to}");
}

class OrderService
{
    private readonly IEmailService _email;

    public OrderService(IEmailService email)  // injected
    {
        _email = email;
    }

    public void PlaceOrder(Order order)
    {
        _email.Send(order.Email, "Order confirmed");
    }
}
```

---

## .NET Built-in DI Container

Add to `Program.cs` (ASP.NET Core / .NET Generic Host):

```csharp
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddTransient<IEmailService, EmailService>();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddSingleton<IAppConfig, AppConfig>();
services.AddTransient<OrderService>();

ServiceProvider provider = services.BuildServiceProvider();

var orderService = provider.GetRequiredService<OrderService>();
```

---

## Service Lifetimes

| Lifetime    | Created                   | Use for                              |
| ----------- | ------------------------- | ------------------------------------ |
| `Transient` | Every time it's requested | Lightweight, stateless services      |
| `Scoped`    | Once per request/scope    | Database contexts, per-request state |
| `Singleton` | Once for the app lifetime | Config, caches, thread-safe services |

```csharp
services.AddTransient<IMyService, MyService>();
services.AddScoped<IMyService, MyService>();
services.AddSingleton<IMyService, MyService>();
```

---

## ASP.NET Core DI (Program.cs)

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();
```

Controllers and services receive dependencies automatically via constructor injection.

---

## Method Injection

Less common: inject dependency only into a single method.

```csharp
class ReportGenerator
{
    public void Generate(ILogger logger)
    {
        logger.Log("Generating report...");
    }
}
```

---

## Property Injection

Set optional dependencies via properties (less preferred; prefer constructor injection).

```csharp
class Service
{
    public ILogger Logger { get; set; } = NullLogger.Instance;
}
```

---

## Testing with DI

Replace real implementations with fakes during tests.

```csharp
class FakeEmailService : IEmailService
{
    public List<string> SentMessages = new();
    public void Send(string to, string subject) => SentMessages.Add(subject);
}

var fake = new FakeEmailService();
var svc  = new OrderService(fake);
svc.PlaceOrder(new Order { Email = "a@b.com" });

Assert.Contains("Order confirmed", fake.SentMessages);
```

---

## Gotchas

- Injecting a `Transient` into a `Singleton` is a captive dependency: the transient lives as long as the singleton (usually wrong)
- Constructor injection is preferred: dependencies are explicit and the class cannot be used without them
- Avoid service locator pattern (`provider.GetService<T>()` inline): it hides dependencies
- `GetRequiredService<T>` throws if not registered; `GetService<T>` returns null: prefer `GetRequired` in production code

---

## Examples

- [53-01](examples/53-01_dependency_injection.cs): A DI container with a singleton and a transient

Run one with `dotnet run examples/53-01_dependency_injection.cs`. See [Examples](examples/README.md).
