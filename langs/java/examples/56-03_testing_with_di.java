// Lesson 56: Dependency Injection (../56_dependency_injection.md)
// Testing with di
// Run: java 56-03_testing_with_di.java

import java.util.ArrayList;
import java.util.List;

public class FakeTest {
    public static void main(String[] args) {
        var fake = new FakeEmailService();
        var service = new OrderService(fake);

        service.placeOrder(new Order(7, "ann@example.com"));

        System.out.println(fake.sent);   // [Order 7 confirmed]
    }
}

record Order(int id, String email) {}

interface EmailService { void send(String to, String subject); }

class FakeEmailService implements EmailService {
    final List<String> sent = new ArrayList<>();

    @Override
    public void send(String to, String subject) {
        sent.add(subject);   // remember instead of sending
    }
}

class OrderService {
    private final EmailService email;
    OrderService(EmailService email) { this.email = email; }
    void placeOrder(Order order) { email.send(order.email(), "Order " + order.id() + " confirmed"); }
}
