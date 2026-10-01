// Lesson 44: Logging (../44_logging.md)
// java.util.logging with a custom handler
// Run: java 44-01_java_util_logging.java

import java.util.logging.ConsoleHandler;
import java.util.logging.Formatter;
import java.util.logging.Level;
import java.util.logging.LogRecord;
import java.util.logging.Logger;

public class LogDemo {
    public static void main(String[] args) {
        Logger log = Logger.getLogger("demo");
        log.setUseParentHandlers(false);   // do not also write to the root handler
        log.setLevel(Level.ALL);

        ConsoleHandler handler = new ConsoleHandler();
        handler.setLevel(Level.ALL);
        handler.setFormatter(new Formatter() {
            @Override
            public String format(LogRecord record) {
                return record.getLevel() + ": " + record.getMessage() + System.lineSeparator();
            }
        });
        log.addHandler(handler);

        log.info("starting");
        log.warning("disk almost full");
        log.fine("detail shown only at FINE");
    }
}
