// Lesson 58: Design Patterns (../58_design_patterns.md)
// Observer
// Run: java 58-02_observer.java

import java.util.ArrayList;
import java.util.List;
import java.util.function.DoubleConsumer;

public class Observer {
    public static void main(String[] args) {
        var stock = new Stock();
        stock.onPriceChanged(price -> System.out.println("price now " + price));

        stock.setPrice(10);
        stock.setPrice(10);   // no change, no event
        stock.setPrice(12);
    }
}

class Stock {
    private final List<DoubleConsumer> listeners = new ArrayList<>();
    private double price;

    void onPriceChanged(DoubleConsumer listener) {
        listeners.add(listener);
    }

    void setPrice(double newPrice) {
        if (newPrice == price) return;
        price = newPrice;
        listeners.forEach(listener -> listener.accept(newPrice));
    }
}
