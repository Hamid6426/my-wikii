// Lesson 56: Dependency Injection (../56_dependency_injection.md)
// The solution constructor injection
// Run: java 56-01_the_solution_constructor_injection.java

public class Main {
    public static void main(String[] args) {
        EmailService email = new ConsoleEmailService();   // pick the real one here
        OrderService orders = new OrderService(email);     // and pass it in

        orders.placeOrder(new Order(7, "ann@example.com"));
    }
}

record Order(int id, String email) {}

interface EmailService {
    void send(String to, String subject);
}

class ConsoleEmailService implements EmailService {
    @Override
    public void send(String to, String subject) {
        System.out.println("Sending '" + subject + "' to " + to);
    }
}

class OrderService {
    private final EmailService email;

    OrderService(EmailService email) {   // injected
        this.email = email;
    }

    void placeOrder(Order order) {
        email.send(order.email(), "Order " + order.id() + " confirmed");
    }
}
