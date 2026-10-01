// Lesson 46: Serialization (../46_serialization.md)
// Serialize a class and a transient field
// Run: java 46-01_serialize_and_transient.java

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;

public class SerializeDemo {
    static class Session implements Serializable {
        private static final long serialVersionUID = 1L;

        private final String user;
        private final int logins;
        private transient String sessionKey;   // not written, comes back null

        Session(String user, int logins, String sessionKey) {
            this.user = user;
            this.logins = logins;
            this.sessionKey = sessionKey;
        }

        @Override
        public String toString() {
            return "Session[user=" + user + ", logins=" + logins + ", key=" + sessionKey + "]";
        }
    }

    public static void main(String[] args) throws Exception {
        ByteArrayOutputStream bytes = new ByteArrayOutputStream();
        try (ObjectOutputStream out = new ObjectOutputStream(bytes)) {
            out.writeObject(new Session("Ana", 3, "abc123"));
        }
        System.out.println("bytes: " + bytes.size());

        try (ObjectInputStream in = new ObjectInputStream(new ByteArrayInputStream(bytes.toByteArray()))) {
            Session restored = (Session) in.readObject();
            System.out.println(restored);
        }
    }
}
