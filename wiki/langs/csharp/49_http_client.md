# 49 - HttpClient

## What is HttpClient

The built-in class for calling web APIs over HTTP. It lives in `System.Net.Http`.

```csharp
using var client = new HttpClient();
string html = await client.GetStringAsync("https://example.com");
```

---

## GET

```csharp
HttpResponseMessage response = await client.GetAsync("https://api.example.com/users/1");

response.EnsureSuccessStatusCode();           // throws on 4xx or 5xx
string body = await response.Content.ReadAsStringAsync();
```

---

## GET and Read JSON in One Step

```csharp
using System.Net.Http.Json;

User? user = await client.GetFromJsonAsync<User>("https://api.example.com/users/1");
```

---

## POST, PUT, DELETE

```csharp
var newUser = new User("Alice", 30);

HttpResponseMessage created = await client.PostAsJsonAsync("https://api.example.com/users", newUser);
await client.PutAsJsonAsync("https://api.example.com/users/1", newUser);
await client.DeleteAsync("https://api.example.com/users/1");
```

Send form or raw text with `StringContent`:

```csharp
var content = new StringContent("hello", System.Text.Encoding.UTF8, "text/plain");
await client.PostAsync(url, content);
```

---

## Headers, Base Address, and Timeout

```csharp
var client = new HttpClient
{
    BaseAddress = new Uri("https://api.example.com/"),
    Timeout = TimeSpan.FromSeconds(10)
};

client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Authorization =
    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

var user = await client.GetFromJsonAsync<User>("users/1");   // relative to BaseAddress
```

---

## Checking the Status

```csharp
if (response.StatusCode == System.Net.HttpStatusCode.NotFound) { }
if (!response.IsSuccessStatusCode) { }
```

---

## Cancellation and Errors

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

try
{
    var text = await client.GetStringAsync(url, cts.Token);
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Request failed: {ex.Message}");
}
catch (TaskCanceledException)
{
    Console.WriteLine("Timed out or cancelled");
}
```

---

## Reuse the Client

Create one `HttpClient` and share it, or get one from `IHttpClientFactory`.

```csharp
// Good: one shared instance
private static readonly HttpClient Client = new();

// Better in apps with dependency injection
services.AddHttpClient();
```

---

## Gotchas

- Creating a new `HttpClient` per request and disposing it can exhaust sockets; share one instance
- A long-lived static client can miss DNS changes; `IHttpClientFactory` handles this
- `GetAsync` does not throw on 404 or 500; call `EnsureSuccessStatusCode()` or check the status yourself
- The default timeout is 100 seconds, which is usually too long
- Always `await` the calls; blocking with `.Result` can deadlock (see [Async / Await / Tasks](48_async_await_tasks.md))

---

## Examples

- [49-01](examples/49-01_http_client.cs): HttpClient talking to a local HttpListener

Run one with `dotnet run examples/49-01_http_client.cs`. See [Examples](examples/README.md).
