#:property PublishAot=false
// Lesson 46: JSON (../46_json.md)
// Serialize and deserialize JSON
// Run: dotnet run 46-01_json_roundtrip.cs

using System.Text.Json;
using System.Text.Json.Serialization;

var order = new Order
{
    Id = 1001,
    Customer = "Alice",
    Items = [new("Pen", 3, 1.5m), new("Notebook", 1, 4.25m)],
    Note = null
};

var options = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

string json = JsonSerializer.Serialize(order, options);
Console.WriteLine(json);

Order? back = JsonSerializer.Deserialize<Order>(json, options);
Console.WriteLine($"{back!.Customer} ordered {back.Items.Count} items, total {back.Items.Sum(i => i.Qty * i.Price)}");

using JsonDocument doc = JsonDocument.Parse(json);
string firstItem = doc.RootElement.GetProperty("items")[0].GetProperty("name").GetString()!;
Console.WriteLine($"First item: {firstItem}");

try
{
    JsonSerializer.Deserialize<Order>("{ not json");
}
catch (JsonException)
{
    Console.WriteLine("Invalid JSON was rejected");
}

class Order
{
    public int Id { get; set; }
    public string Customer { get; set; } = "";
    public List<Item> Items { get; set; } = [];
    public string? Note { get; set; }
}

record Item(string Name, int Qty, decimal Price);

// Expected output should be:
// {
//   "id": 1001,
//   "customer": "Alice",
//   "items": [
//     {
//       "name": "Pen",
//       "qty": 3,
//       "price": 1.5
//     },
//     {
//       "name": "Notebook",
//       "qty": 1,
//       "price": 4.25
//     }
//   ]
// }
// Alice ordered 2 items, total 8.75
// First item: Pen
// Invalid JSON was rejected
