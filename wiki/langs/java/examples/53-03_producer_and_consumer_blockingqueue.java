// Lesson 53: Threading (../53_threading.md)
// Producer and consumer blockingqueue
// Run: java 53-03_producer_and_consumer_blockingqueue.java

import java.util.concurrent.ArrayBlockingQueue;
import java.util.concurrent.BlockingQueue;

public class ProducerConsumer {
    public static void main(String[] args) throws InterruptedException {
        BlockingQueue<Integer> queue = new ArrayBlockingQueue<>(2);   // holds at most 2

        Thread producer = Thread.ofVirtual().start(() -> {
            try {
                for (int i = 1; i <= 5; i++) queue.put(i);   // waits while the queue is full
                queue.put(-1);                              // a signal that means "done"
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
            }
        });

        int sum = 0;
        while (true) {
            int item = queue.take();                        // waits while the queue is empty
            if (item == -1) break;
            sum += item;
        }
        producer.join();
        System.out.println("sum " + sum);
    }
}
