// Lesson 58: Design Patterns (../58_design_patterns.md)
// Decorator
// Run: java 58-01_decorator.java

public class Decorator {
    public static void main(String[] args) {
        MessageSender sender = new LoggingSender(new EmailSender());
        sender.send("hi");
    }
}

interface MessageSender {
    void send(String message);
}

class EmailSender implements MessageSender {
    @Override
    public void send(String message) {
        System.out.println("email: " + message);
    }
}

class LoggingSender implements MessageSender {
    private final MessageSender inner;

    LoggingSender(MessageSender inner) {
        this.inner = inner;
    }

    @Override
    public void send(String message) {
        System.out.println("sending...");
        inner.send(message);
        System.out.println("sent");
    }
}
