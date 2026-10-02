#:property PublishAot=false
// Lesson 49: HttpClient (../49_http_client.md)
// HttpClient talking to a local HttpListener
// Run: dotnet run 49-01_http_client.cs

using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

// A tiny web server on this computer, so the example needs no internet
string url = "http://localhost:5099/";
using var listener = new HttpListener();
listener.Prefixes.Add(url);
listener.Start();

var server = Task.Run(async () =>
{
    for (int i = 0; i < 3; i++)
    {
        var ctx = await listener.GetContextAsync();
        string path = ctx.Request.Url!.AbsolutePath;
        string body;

        if (path == "/hello")
        {
            body = "Hello from the server";
        }
        else if (path == "/user" && ctx.Request.HttpMethod == "POST")
        {
            string posted = await new StreamReader(ctx.Request.InputStream).ReadToEndAsync();
            body = JsonSerializer.Serialize(new { received = posted.Length, ok = true });
        }
        else
        {
            ctx.Response.StatusCode = 404;
            body = "not found";
        }

        byte[] bytes = Encoding.UTF8.GetBytes(body);
        await ctx.Response.OutputStream.WriteAsync(bytes);
        ctx.Response.Close();
    }
});

using var client = new HttpClient { BaseAddress = new Uri(url), Timeout = TimeSpan.FromSeconds(5) };

Console.WriteLine(await client.GetStringAsync("hello"));

var response = await client.PostAsJsonAsync("user", new { name = "Alice", age = 30 });
Console.WriteLine($"POST status: {(int)response.StatusCode}");
Console.WriteLine(await response.Content.ReadAsStringAsync());

var missing = await client.GetAsync("nothing");
Console.WriteLine($"GET /nothing: {(int)missing.StatusCode} {missing.StatusCode}");

await server;
listener.Stop();

// Expected output should be:
// Hello from the server
// POST status: 200
// {"received":25,"ok":true}
// GET /nothing: 404 NotFound
