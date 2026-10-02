# 46 - Serialization

## What Serialization Is

**Serialization** turns an object into bytes so it can be stored or sent, and **deserialization** turns the bytes back into an object. Java has a built-in form for this.

Do not confuse it with JSON: see [JSON](48_json.md). Java's built-in serialization is binary, Java-only, and carries the class structure with the data.

---

## Serializable

A class opts in by implementing the marker interface `java.io.Serializable` (it has no methods). `ObjectOutputStream` writes and `ObjectInputStream` reads.

```java
import java.io.*;

record User(String name, int age) implements Serializable { }

ByteArrayOutputStream bytes = new ByteArrayOutputStream();
try (ObjectOutputStream out = new ObjectOutputStream(bytes)) {
    out.writeObject(new User("Ana", 30));
}

try (ObjectInputStream in = new ObjectInputStream(new ByteArrayInputStream(bytes.toByteArray()))) {
    System.out.println(in.readObject());   // User[name=Ana, age=30]
}
```

A record is serializable when it implements `Serializable`.

---

## serialVersionUID

Each serializable class has a version number, the **serialVersionUID**. If the class changes and the number no longer matches, reading old bytes fails with `InvalidClassException`. Declare it yourself so small changes do not break stored data.

```java
private static final long serialVersionUID = 1L;
```

---

## transient and static

`transient` and `static` fields are not serialized.

```java
class Session implements Serializable {
    private static final long serialVersionUID = 1L;
    private String user;
    private transient String sessionKey;   // not written
}
```

Use `transient` for caches, locks, streams, and anything that can be rebuilt on the other side. A `transient` field comes back as its default (`null`, `0`, `false`).

---

## Custom Form

- `writeObject` and `readObject` give you control over which fields are written, and a place to validate on read
- `Externalizable` writes the format entirely by hand
- `writeReplace` and `readResolve` swap in a proxy or enforce a singleton or enum identity

---

## Why It Is Risky

- Deserializing bytes from an untrusted source can run code, through a **gadget chain** in the class path; it is a well-known attack surface
- The exact class must exist on both sides, with matching fields and version
- It exposes private fields and the class layout, so it is brittle across releases
- It is not a data-interchange format; prefer JSON, or a schema format such as Protocol Buffers, for data you store or send

---

## Gotchas

- Never deserialize data you do not trust; this is the first rule of Java serialization
- A `transient` field stays default after a round trip
- Constructors do not run during deserialization, so validation must move into `readObject`
- Changing a class without a fixed `serialVersionUID` can break already stored bytes
- A `NotSerializableException` names the offending field, usually one you forgot to mark `transient`
- For long-lived data, a text or schema format survives version changes far better than Java serialization

---

## Full Example

```java
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
```

Expected output should be:

```
bytes: 89
Session[user=Ana, logins=3, key=null]
```

`user` and `logins` survive the round trip; `sessionKey` was `transient`, so it comes back `null`. The byte count depends on the class and the JVM, so yours may differ.

---

## Examples

- [46-01](examples/46-01_serialize_and_transient.java): Serialize a class and a transient field

Run one with `java examples/46-01_serialize_and_transient.java`. See [Examples](examples/README.md).
