// Lesson 55: Design Patterns (../55_design_patterns.md)
// Strategy, Decorator, and Factory together
// Run: dotnet run 55-01_strategy_and_decorator.cs

// Strategy: pick the algorithm at run time
IShippingStrategy[] strategies = [new Standard(), new Express(), new Pickup()];
foreach (var s in strategies)
{
    Console.WriteLine($"{s.Name,-9} {s.Cost(2.5m):N2}");
}

// Factory: create the right strategy from text
IShippingStrategy chosen = ShippingFactory.Create("express");
Console.WriteLine($"Chosen: {chosen.Name}");

// Decorator: add behavior without changing the class
IShippingStrategy logged = new LoggingShipping(new DiscountedShipping(chosen, 0.5m));
Console.WriteLine($"Final cost: {logged.Cost(2.5m):N2}");

interface IShippingStrategy
{
    string Name { get; }
    decimal Cost(decimal kg);
}

class Standard : IShippingStrategy
{
    public string Name => "Standard";
    public decimal Cost(decimal kg) => 3m + kg * 1m;
}

class Express : IShippingStrategy
{
    public string Name => "Express";
    public decimal Cost(decimal kg) => 8m + kg * 2m;
}

class Pickup : IShippingStrategy
{
    public string Name => "Pickup";
    public decimal Cost(decimal kg) => 0m;
}

static class ShippingFactory
{
    public static IShippingStrategy Create(string kind) => kind.ToLowerInvariant() switch
    {
        "standard" => new Standard(),
        "express" => new Express(),
        "pickup" => new Pickup(),
        _ => throw new ArgumentException($"Unknown shipping: {kind}")
    };
}

class DiscountedShipping(IShippingStrategy inner, decimal rate) : IShippingStrategy
{
    public string Name => inner.Name + " (discounted)";
    public decimal Cost(decimal kg) => inner.Cost(kg) * rate;
}

class LoggingShipping(IShippingStrategy inner) : IShippingStrategy
{
    public string Name => inner.Name;

    public decimal Cost(decimal kg)
    {
        decimal cost = inner.Cost(kg);
        Console.WriteLine($"  [log] {inner.Name} for {kg} kg costs {cost:N2}");
        return cost;
    }
}

// Expected output should be:
// Standard  5.50
// Express   13.00
// Pickup    0.00
// Chosen: Express
//   [log] Express (discounted) for 2.5 kg costs 6.50
// Final cost: 6.50
