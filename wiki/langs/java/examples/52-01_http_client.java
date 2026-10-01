// Lesson 52: HTTP Client (../52_http_client.md)
// HTTP Client
// Run: java 52-01_http_client.java

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
