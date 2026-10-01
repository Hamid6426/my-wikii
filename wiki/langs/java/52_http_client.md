# 52 - HTTP Client

## What is HttpClient

The built-in class for calling web APIs over HTTP. It lives in `java.net.http` and arrived in Java 11. It speaks HTTP/1.1 and HTTP/2, and can work blocking or async.

| Type             | Job                                                   |
| ---------------- | ----------------------------------------------------- |
| `HttpClient`     | Sends requests. Create one and share it               |
| `HttpRequest`    | One request: URL, method, headers, body               |
| `HttpResponse`   | Status code, headers, and body                        |
| `BodyPublishers` | Turn your data into a request body                    |
| `BodyHandlers`   | Say how to read the response body (string, file, ...) |

The examples that call `https://api.example.com` need a network and a real server, so their output is an example only.

---

## GET

```java
HttpClient client = HttpClient.newHttpClient();

HttpRequest request = HttpRequest.newBuilder(URI.create("https://api.example.com/users/1"))
    .header("Accept", "application/json")
    .GET()                                     // the default, can be left out
    .build();

HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());

System.out.println(response.statusCode());     // for example 200
System.out.println(response.body());           // for example {"id":1,"name":"Alice"}
```

`send` blocks until the response arrives. It throws `IOException` and `InterruptedException`, both checked.

---

## A Complete Example

This program starts a tiny local server with `com.sun.net.httpserver.HttpServer` (part of the JDK), so it runs without the internet.

```java
import com.sun.net.httpserver.HttpServer;
import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.nio.charset.StandardCharsets;

public class LocalHttp {
    public static void main(String[] args) throws IOException, InterruptedException {
        // A tiny server that replies with the method and body it received
        HttpServer server = HttpServer.create(new InetSocketAddress("localhost", 0), 0);
        server.createContext("/echo", exchange -> {
            String body = new String(exchange.getRequestBody().readAllBytes(), StandardCharsets.UTF_8);
            byte[] reply = (exchange.getRequestMethod() + " [" + body + "]").getBytes(StandardCharsets.UTF_8);
            int status = exchange.getRequestURI().getPath().equals("/echo") ? 200 : 404;
            exchange.sendResponseHeaders(status, reply.length);
            exchange.getResponseBody().write(reply);
            exchange.close();
        });
        server.start();
        String base = "http://localhost:" + server.getAddress().getPort();

        try (HttpClient client = HttpClient.newHttpClient()) {
            HttpRequest get = HttpRequest.newBuilder(URI.create(base + "/echo")).build();
            HttpResponse<String> r1 = client.send(get, HttpResponse.BodyHandlers.ofString());
            System.out.println(r1.statusCode() + " " + r1.body());

            HttpRequest post = HttpRequest.newBuilder(URI.create(base + "/echo"))
                .header("Content-Type", "text/plain")
                .POST(HttpRequest.BodyPublishers.ofString("hello"))
                .build();
            HttpResponse<String> r2 = client.send(post, HttpResponse.BodyHandlers.ofString());
            System.out.println(r2.statusCode() + " " + r2.body());

            HttpRequest missing = HttpRequest.newBuilder(URI.create(base + "/echo/nope")).build();
            HttpResponse<String> r3 = client.send(missing, HttpResponse.BodyHandlers.ofString());
            System.out.println(r3.statusCode());
        } finally {
            server.stop(0);
        }
    }
}
```

Expected output should be:

```
200 GET []
200 POST [hello]
404
```

Port `0` asks the system for any free port. `HttpClient` is `AutoCloseable` since Java 21, so try-with-resources shuts it down.

---

## POST, PUT, DELETE

```java
HttpRequest post = HttpRequest.newBuilder(URI.create("https://api.example.com/users"))
    .header("Content-Type", "application/json")
    .POST(HttpRequest.BodyPublishers.ofString("""
        {"name":"Alice","age":30}
        """))
    .build();

HttpRequest put = HttpRequest.newBuilder(uri).PUT(HttpRequest.BodyPublishers.ofString(json)).build();
HttpRequest delete = HttpRequest.newBuilder(uri).DELETE().build();
HttpRequest patch = HttpRequest.newBuilder(uri).method("PATCH", HttpRequest.BodyPublishers.ofString(json)).build();
```

| Body publisher                  | Sends             |
| ------------------------------- | ----------------- |
| `BodyPublishers.ofString(s)`    | Text              |
| `BodyPublishers.ofByteArray(b)` | Bytes             |
| `BodyPublishers.ofFile(path)`   | A file's contents |
| `BodyPublishers.noBody()`       | Nothing           |

The JDK has no JSON support, so build the text yourself or use Jackson (see [JSON](48_json.md)) and send `mapper.writeValueAsString(user)`.

---

## Reading the Response

| Body handler                   | `body()` returns                        |
| ------------------------------ | --------------------------------------- |
| `BodyHandlers.ofString()`      | `String`                                |
| `BodyHandlers.ofByteArray()`   | `byte[]`                                |
| `BodyHandlers.ofFile(path)`    | The `Path`, after saving the body to it |
| `BodyHandlers.ofLines()`       | `Stream<String>`, one line at a time    |
| `BodyHandlers.ofInputStream()` | `InputStream`, for large bodies         |
| `BodyHandlers.discarding()`    | Nothing, when only the status matters   |

```java
response.statusCode()                      // 200
response.headers().firstValue("X-Id")      // Optional[7]
response.uri()                             // the final URL, after redirects
```

---

## Async Requests

`sendAsync` returns a `CompletableFuture` at once. See [CompletableFuture and Async Code](51_completablefuture_and_async.md).

```java
List<URI> uris = List.of(URI.create("https://api.example.com/a"), URI.create("https://api.example.com/b"));

List<CompletableFuture<String>> pages = uris.stream()
    .map(uri -> client.sendAsync(HttpRequest.newBuilder(uri).build(), HttpResponse.BodyHandlers.ofString())
        .thenApply(HttpResponse::body))
    .toList();

List<String> bodies = pages.stream().map(CompletableFuture::join).toList();   // both requests ran at once
```

Calling `send` from many virtual threads works just as well and reads like normal code (see [Threading](53_threading.md)).

---

## Client Settings

```java
HttpClient client = HttpClient.newBuilder()
    .connectTimeout(Duration.ofSeconds(5))           // time to open the connection
    .followRedirects(HttpClient.Redirect.NORMAL)     // the default is NEVER
    .version(HttpClient.Version.HTTP_2)              // the default; falls back to 1.1
    .build();

HttpRequest request = HttpRequest.newBuilder(uri)
    .timeout(Duration.ofSeconds(10))                 // time to wait for the response
    .header("Authorization", "Bearer " + token)
    .build();
```

---

## Errors

```java
try {
    HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());
    if (response.statusCode() >= 400) {
        System.out.println("Server said " + response.statusCode());
    }
} catch (HttpTimeoutException e) {
    System.out.println("Timed out");
} catch (IOException e) {
    System.out.println("Request failed: " + e);     // such as ConnectException
} catch (InterruptedException e) {
    Thread.currentThread().interrupt();
}
```

`HttpTimeoutException` extends `IOException`, so catch it first.

---

## Gotchas

- A 404 or 500 does not throw. Always check `statusCode()`
- There is no default request timeout: without `.timeout(...)` a request can wait forever
- Redirects are not followed unless you set `followRedirects`
- Create one `HttpClient` and reuse it. Each one has its own connection pool and threads
- Build query strings with `URLEncoder.encode(value, StandardCharsets.UTF_8)`. A space or `&` in a value breaks the URL
- The older `HttpURLConnection` still exists but is harder to use. Prefer `HttpClient`

---

## Examples

- [52-01](examples/52-01_http_client.java): HTTP Client

Run one with `java examples/52-01_http_client.java`. See [Examples](examples/README.md).
