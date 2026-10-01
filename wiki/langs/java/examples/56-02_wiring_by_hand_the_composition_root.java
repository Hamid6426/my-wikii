// Lesson 56: Dependency Injection (../56_dependency_injection.md)
// Wiring by hand the composition root
// Run: java 56-02_wiring_by_hand_the_composition_root.java

import java.util.ArrayList;
import java.util.List;

public class Root {
    public static void main(String[] args) {
        // Composition root: the one place that knows the concrete classes
        AppConfig config = new AppConfig("smtp.example.com");          // shared by everyone
        EmailService email = new SmtpEmailService(config);
        OrderRepository repo = new InMemoryOrderRepository();
        OrderService orders = new OrderService(repo, email);

        orders.placeOrder(new Order(1, "ann@example.com"));
        orders.placeOrder(new Order(2, "bob@example.com"));
        System.out.println(repo.count() + " orders saved");
    }
}

record AppConfig(String smtpHost) {}
record Order(int id, String email) {}

interface EmailService { void send(String to, String subject); }
interface OrderRepository { void save(Order order); int count(); }

class SmtpEmailService implements EmailService {
    private final AppConfig config;

    SmtpEmailService(AppConfig config) {
        this.config = config;
    }

    @Override
    public void send(String to, String subject) {
        System.out.println("[" + config.smtpHost() + "] " + subject + " -> " + to);
    }
}

class InMemoryOrderRepository implements OrderRepository {
    private final List<Order> orders = new ArrayList<>();

    @Override public void save(Order order) { orders.add(order); }
    @Override public int count() { return orders.size(); }
}

class OrderService {
    private final OrderRepository repo;
    private final EmailService email;

    OrderService(OrderRepository repo, EmailService email) {
        this.repo = repo;
        this.email = email;
    }

    void placeOrder(Order order) {
        repo.save(order);
        email.send(order.email(), "Order " + order.id() + " confirmed");
    }
}
