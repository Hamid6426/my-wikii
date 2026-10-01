// Lesson 54: Conventions and Clean Code (../54_conventions_and_clean_code.md)
// Refactoring a messy method into clean code
// Run: dotnet run 54-01_before_and_after_refactor.cs

var order = new Order("Alice", [new("Pen", 3, 1.50m), new("Book", 1, 12.00m), new("Bag", 2, 20.00m)], "SAVE10");

Console.WriteLine($"Messy:  {MessyTotal(order)}");
Console.WriteLine($"Clean:  {OrderCalculator.Total(order):N2}");

// Before: one long method, magic numbers, and nested ifs
static decimal MessyTotal(Order o)
{
    decimal t = 0;
    foreach (var i in o.Items)
    {
        t += i.Qty * i.Price;
    }
    if (o.Code != null)
    {
        if (o.Code == "SAVE10")
        {
            t = t - t * 0.10m;
        }
    }
    if (t > 50)
    {
        t = t - 5;
    }
    return Math.Round(t, 2);
}

record Line(string Name, int Qty, decimal Price);
record Order(string Customer, List<Line> Items, string? Code);

// After: small methods, named constants, early returns
static class OrderCalculator
{
    private const decimal FreeShippingCredit = 5m;
    private const decimal FreeShippingThreshold = 50m;
    private static readonly Dictionary<string, decimal> DiscountCodes = new() { ["SAVE10"] = 0.10m };

    public static decimal Total(Order order)
    {
        decimal subtotal = Subtotal(order);
        decimal discounted = ApplyDiscount(subtotal, order.Code);
        return Math.Round(ApplyShippingCredit(discounted), 2);
    }

    private static decimal Subtotal(Order order) => order.Items.Sum(i => i.Qty * i.Price);

    private static decimal ApplyDiscount(decimal amount, string? code)
    {
        if (code is null) return amount;
        if (!DiscountCodes.TryGetValue(code, out decimal rate)) return amount;
        return amount - amount * rate;
    }

    private static decimal ApplyShippingCredit(decimal amount)
        => amount > FreeShippingThreshold ? amount - FreeShippingCredit : amount;
}

// Expected output should be:
// Messy:  45.85
// Clean:  45.85
