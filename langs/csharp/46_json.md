# 46 - JSON

## System.Text.Json

Built into .NET. No package needed.

```csharp
using System.Text.Json;
```

---

## Object to JSON (Serialize)

```csharp
record Person(string Name, int Age);

var p = new Person("Alice", 30);

string json = JsonSerializer.Serialize(p);
Console.WriteLine(json);   // {"Name":"Alice","Age":30}
```

---

## JSON to Object (Deserialize)

```csharp
string json = """{"Name":"Bob","Age":25}""";

Person? p = JsonSerializer.Deserialize<Person>(json);
Console.WriteLine(p?.Name);   // Bob
```

---

## Options

```csharp
var options = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true,
    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
};

string pretty = JsonSerializer.Serialize(p, options);
```

Create one options object and reuse it. Building a new one per call is slow.

---

## Attributes

```csharp
using System.Text.Json.Serialization;

class Product
{
    [JsonPropertyName("product_id")]
    public int Id { get; set; }

    [JsonIgnore]
    public string InternalNote { get; set; } = "";

    public decimal Price { get; set; }
}
```

---

## Lists and Dictionaries

```csharp
var items = JsonSerializer.Deserialize<List<Product>>(json);
var map = JsonSerializer.Deserialize<Dictionary<string, int>>("""{"a":1,"b":2}""");
```

---

## Reading and Writing Files

```csharp
await using var write = File.Create("people.json");
await JsonSerializer.SerializeAsync(write, people);

await using var read = File.OpenRead("people.json");
var loaded = await JsonSerializer.DeserializeAsync<List<Person>>(read);
```

---

## Reading Without a Class: JsonDocument

```csharp
using JsonDocument doc = JsonDocument.Parse("""{"user":{"name":"Alice"}}""");

string? name = doc.RootElement
    .GetProperty("user")
    .GetProperty("name")
    .GetString();
```

For editing, use `JsonNode`:

```csharp
using System.Text.Json.Nodes;

JsonNode node = JsonNode.Parse("""{"count":1}""")!;
node["count"] = 2;
Console.WriteLine(node.ToJsonString());
```

---

## Handling Bad Input

```csharp
try
{
    var p = JsonSerializer.Deserialize<Person>(text);
}
catch (JsonException ex)
{
    Console.WriteLine($"Bad JSON: {ex.Message}");
}
```

---

## Gotchas

- By default property names are matched case-sensitively; set `PropertyNameCaseInsensitive = true` for outside data
- Fields are skipped unless you set `IncludeFields = true` or mark them; only public properties are used by default
- `Deserialize` returns `null` for the text `null`, so keep the `?` on the result type
- Missing properties keep their default value without an error; use `required` to enforce them
- Newtonsoft.Json (Json.NET) is a popular alternative with more features, but System.Text.Json is the default and faster

---

## Examples

- [46-01](examples/46-01_json_roundtrip.cs): Serialize and deserialize JSON

Run one with `dotnet run examples/46-01_json_roundtrip.cs`. See [Examples](examples/README.md).
