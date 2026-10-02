# 55 - Design Patterns

## What is a Design Pattern

A named, proven way to solve a problem that keeps coming up. Patterns give developers a shared vocabulary: saying "use a strategy here" explains the design in four words.

They are not libraries. You write them yourself with plain classes and interfaces. See [Conventions and Clean Code](54_conventions_and_clean_code.md) for the SOLID rules that most patterns follow.

| Group      | Question it answers           | Patterns in this lesson                             |
| ---------- | ----------------------------- | --------------------------------------------------- |
| Creational | How do I create objects?      | Singleton, Factory, Builder, Prototype              |
| Structural | How do I connect objects?     | Adapter, Decorator, Facade                          |
| Behavioral | How do objects work together? | Strategy, Observer, Command, Template Method, State |
| Other      | Common structure in C# apps   | Repository, Null Object                             |

---

## Singleton

One instance for the whole program.

```csharp
sealed class AppSettings
{
    private static readonly Lazy<AppSettings> _instance = new(() => new AppSettings());

    public static AppSettings Instance => _instance.Value;

    private AppSettings() { }
}

var a = AppSettings.Instance;
var b = AppSettings.Instance;
Console.WriteLine(ReferenceEquals(a, b));   // True
```

`Lazy<T>` creates it on first use and is safe across threads. In real apps, prefer registering a service as a singleton in [Dependency Injection](53_dependency_injection.md). It is easier to test.

---

## Factory

One place that decides which class to create.

```csharp
interface IShape { double Area(); }
class Circle : IShape { public double Area() => Math.PI; }
class Square : IShape { public double Area() => 4; }

static class ShapeFactory
{
    public static IShape Create(string kind) => kind switch
    {
        "circle" => new Circle(),
        "square" => new Square(),
        _ => throw new ArgumentException($"Unknown shape: {kind}")
    };
}

IShape shape = ShapeFactory.Create("circle");
```

Callers depend on `IShape` and never name `Circle` directly. Adding a shape changes only the factory.

---

## Builder

Build a complex object step by step. Each method returns the builder, so calls chain.

```csharp
class HttpRequest
{
    public string Url { get; set; } = "";
    public string Method { get; set; } = "GET";
    public Dictionary<string, string> Headers { get; } = new();
}

class RequestBuilder
{
    private readonly HttpRequest _request = new();

    public RequestBuilder Url(string url) { _request.Url = url; return this; }
    public RequestBuilder Method(string method) { _request.Method = method; return this; }
    public RequestBuilder Header(string key, string value) { _request.Headers[key] = value; return this; }
    public HttpRequest Build() => _request;
}

var request = new RequestBuilder()
    .Url("https://example.com")
    .Method("POST")
    .Header("Accept", "application/json")
    .Build();
```

Use it when a constructor would need many optional parameters.

---

## Prototype

Make a copy of an existing object and change a few things. Records do this with `with`.

```csharp
record Config(string Env, int Retries);

var dev = new Config("dev", 3);
var strict = dev with { Retries = 5 };

Console.WriteLine(dev);      // Config { Env = dev, Retries = 3 }
Console.WriteLine(strict);   // Config { Env = dev, Retries = 5 }
```

See [Records and Equality](25_records_and_equality.md).

---

## Adapter

Make an old or foreign class fit the interface your code expects.

```csharp
interface ILogger
{
    void Log(string message);
}

class LegacyLogger
{
    public void WriteEntry(string level, string text) => Console.WriteLine($"[{level}] {text}");
}

class LegacyLoggerAdapter(LegacyLogger legacy) : ILogger
{
    public void Log(string message) => legacy.WriteEntry("INFO", message);
}

ILogger logger = new LegacyLoggerAdapter(new LegacyLogger());
logger.Log("adapted");   // [INFO] adapted
```

---

## Decorator

Wrap an object to add behavior without changing it. The wrapper has the same interface as what it wraps.

```csharp
interface IMessageSender
{
    void Send(string message);
}

class EmailSender : IMessageSender
{
    public void Send(string message) => Console.WriteLine($"email: {message}");
}

class LoggingSender(IMessageSender inner) : IMessageSender
{
    public void Send(string message)
    {
        Console.WriteLine("sending...");
        inner.Send(message);
        Console.WriteLine("sent");
    }
}

IMessageSender sender = new LoggingSender(new EmailSender());
sender.Send("hi");
```

Expected output should be:

```
sending...
email: hi
sent
```

Decorators stack: `new Retry(new Logging(new Email()))`. This is the open/closed rule in practice.

---

## Facade

One simple class in front of several complicated ones.

```csharp
class OrderFacade
{
    private readonly Inventory _inventory = new();
    private readonly Payment _payment = new();

    public string Place(string item)
        => _inventory.InStock(item) && _payment.Charge() ? $"ordered {item}" : "failed";
}
```

Callers use `Place` and know nothing about inventory or payment.

---

## Strategy

Swap an algorithm at run time by putting it behind an interface.

```csharp
interface IDiscount
{
    decimal Apply(decimal price);
}

class PercentOff(int percent) : IDiscount
{
    public decimal Apply(decimal price) => price - price * percent / 100;
}

class NoDiscount : IDiscount
{
    public decimal Apply(decimal price) => price;
}

class Cart(IDiscount discount)
{
    public decimal Total(decimal price) => discount.Apply(price);
}

Console.WriteLine(new Cart(new PercentOff(10)).Total(200m));   // 180
Console.WriteLine(new Cart(new NoDiscount()).Total(200m));     // 200
```

In C#, a delegate is often a lighter strategy:

```csharp
class Cart2(Func<decimal, decimal> discount)
{
    public decimal Total(decimal price) => discount(price);
}

Console.WriteLine(new Cart2(p => p - 5).Total(200m));   // 195
```

See [Delegates and Lambdas](37_delegates_and_lambdas.md).

---

## Observer

Objects subscribe to be told when something changes. C# has this built in as events.

```csharp
class Stock
{
    public event EventHandler<decimal>? PriceChanged;

    private decimal _price;
    public decimal Price
    {
        get => _price;
        set
        {
            if (_price == value) return;
            _price = value;
            PriceChanged?.Invoke(this, value);
        }
    }
}

var stock = new Stock();
stock.PriceChanged += (sender, price) => Console.WriteLine($"price now {price}");

stock.Price = 10;
stock.Price = 10;   // no change, no event
stock.Price = 12;
```

Expected output should be:

```
price now 10
price now 12
```

See [Events](38_events.md).

---

## Command

Turn an action into an object. That lets you queue it, log it, or undo it.

```csharp
class Document { public string Text = ""; }

interface ICommand
{
    void Do();
    void Undo();
}

class AppendCommand(Document doc, string text) : ICommand
{
    public void Do() => doc.Text += text;
    public void Undo() => doc.Text = doc.Text[..^text.Length];
}

var doc = new Document();
var history = new Stack<ICommand>();

ICommand cmd = new AppendCommand(doc, "Hello");
cmd.Do();
history.Push(cmd);

cmd = new AppendCommand(doc, " World");
cmd.Do();
history.Push(cmd);

Console.WriteLine(doc.Text);   // Hello World

history.Pop().Undo();
Console.WriteLine(doc.Text);   // Hello
```

---

## Template Method

A base class fixes the steps. Child classes fill in some of them.

```csharp
abstract class Report
{
    public void Run()
    {
        Load();
        Format();
        Console.WriteLine("saved");
    }

    protected virtual void Load() => Console.WriteLine("loaded");
    protected abstract void Format();
}

class CsvReport : Report
{
    protected override void Format() => Console.WriteLine("csv");
}

new CsvReport().Run();   // loaded, csv, saved
```

---

## State

An object changes behavior when its state changes. For small cases an enum and a `switch` expression are enough.

```csharp
enum DoorState { Closed, Open }

class Door
{
    public DoorState State { get; private set; } = DoorState.Closed;

    public void Press() => State = State switch
    {
        DoorState.Closed => DoorState.Open,
        _ => DoorState.Closed
    };
}
```

For many states with their own rules, make each state a class behind an interface.

---

## Repository

Hide where data is stored behind an interface. The rest of the app does not care if it is a file, a database, or memory.

```csharp
interface IEntity { int Id { get; } }

interface IRepository<T> where T : IEntity
{
    void Add(T item);
    T? Get(int id);
}

class InMemoryRepository<T> : IRepository<T> where T : IEntity
{
    private readonly Dictionary<int, T> _items = new();

    public void Add(T item) => _items[item.Id] = item;
    public T? Get(int id) => _items.GetValueOrDefault(id);
}

record User(int Id, string Name) : IEntity;

IRepository<User> repo = new InMemoryRepository<User>();
repo.Add(new User(1, "Ann"));
Console.WriteLine(repo.Get(1)?.Name);   // Ann
```

In tests you pass a fake repository. See [Mocking and Fixtures](57_mocking_and_fixtures.md).

---

## Null Object

Instead of `null`, pass an object that does nothing. Callers need no `null` checks.

```csharp
interface ILog { void Write(string message); }

class NullLog : ILog
{
    public static readonly NullLog Instance = new();
    public void Write(string message) { }
}

class Service(ILog log)
{
    public string Run()
    {
        log.Write("running");   // safe even with NullLog
        return "ok";
    }
}

Console.WriteLine(new Service(NullLog.Instance).Run());   // ok
```

---

## Patterns You Already Used

| Pattern   | Where in C#                                                                          |
| --------- | ------------------------------------------------------------------------------------ |
| Iterator  | `foreach` and `yield return` ([Iterators](36_iterators.md))                          |
| Observer  | Events                                                                               |
| Prototype | `record` with `with`                                                                 |
| Singleton | `Lazy<T>`, DI singleton registration                                                 |
| Strategy  | `Func<>` parameters, `IComparer<T>`                                                  |
| Decorator | `BufferedStream` wraps another `Stream` ([File I/O: Streams](45_file_io_streams.md)) |
| Builder   | `StringBuilder`                                                                      |

---

## Picking a Pattern

| Problem                                      | Try         |
| -------------------------------------------- | ----------- |
| Long `if/else` or `switch` choosing behavior | Strategy    |
| Which class to create depends on input       | Factory     |
| Constructor with many optional values        | Builder     |
| Add logging or retries around existing code  | Decorator   |
| Old API does not match your interface        | Adapter     |
| Many classes behind one entry point          | Facade      |
| Others must react when something changes     | Observer    |
| Need undo or a queue of actions              | Command     |
| `null` checks everywhere                     | Null Object |

---

## Gotchas

- Do not add a pattern before you feel the problem. Three lines of plain code beat an unused factory
- A pattern name is not a goal. If a simple `if` solves it, use the `if`
- Singletons are global state in disguise: they hide dependencies and make tests share data
- Many patterns exist because older languages lacked features. In C#, delegates, records, and events often replace a whole class hierarchy
- Patterns from other books may use names that differ slightly. The idea matters more than the exact class names

---

## Examples

- [55-01](examples/55-01_strategy_and_decorator.cs): Strategy, Decorator, and Factory together
- [55-02](examples/55-02_observer_command_builder.cs): Observer, Command with undo, and Builder

Run one with `dotnet run examples/55-01_strategy_and_decorator.cs`. See [Examples](examples/README.md).
