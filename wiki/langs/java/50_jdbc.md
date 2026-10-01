# 50 - JDBC

## What JDBC Is

**JDBC** (Java Database Connectivity) is the JDK API for talking to a relational database. It lives in the `java.sql` package. The API is in the JDK; the **driver** that speaks a particular database's protocol is not, and is added as a jar.

---

## The Pieces

| Piece                           | Role                                                  |
| ------------------------------- | ----------------------------------------------------- |
| `Driver`                        | Talks the database protocol (a jar on the class path) |
| `DriverManager` `DataSource`    | Gets a `Connection`                                   |
| `Connection`                    | One session with the database                         |
| `Statement` `PreparedStatement` | Sends SQL                                             |
| `ResultSet`                     | Reads rows                                            |
| `SQLException`                  | Reports a database error                              |

---

## Connecting

The URL is driver-specific. The driver registers itself through service loading when it is on the class path, so `Class.forName("...Driver")` is no longer needed (old code still does it).

```java
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

String url = "jdbc:example:mem";   // format depends on the driver
try (Connection conn = DriverManager.getConnection(url, "user", "password")) {
    // use conn
} catch (SQLException ex) {
    // handle the failure
}
```

---

## PreparedStatement

Always use `?` placeholders and set the values. Never build SQL by joining strings, because that is a **SQL injection** hole.

```java
try (PreparedStatement ps = conn.prepareStatement("insert into users(name) values (?)")) {
    ps.setString(1, "Ana");
    ps.executeUpdate();
}

try (PreparedStatement ps = conn.prepareStatement("select id, name from users where id = ?")) {
    ps.setInt(1, 7);
    try (ResultSet rs = ps.executeQuery()) {
        while (rs.next()) {
            System.out.println(rs.getLong("id") + " " + rs.getString("name"));
        }
    }
}
```

A `PreparedStatement` also lets the database reuse the query plan, so it is faster when run in a loop.

---

## Transactions

By default each statement is committed on its own. For several statements that must all succeed, turn that off.

```java
conn.setAutoCommit(false);
try {
    // ... several statements ...
    conn.commit();
} catch (SQLException ex) {
    conn.rollback();
    throw ex;
} finally {
    conn.setAutoCommit(true);
}
```

Closing a connection does not roll back work you already committed.

---

## DataSource and Pools

In an application you get connections from a pool (HikariCP is common) behind a `javax.sql.DataSource`, not from `DriverManager`. A pool reuses a small set of open connections, which matters because opening one is slow and each is limited. See [Dependency Injection](56_dependency_injection.md) and [Threading](53_threading.md).

---

## Rows to Objects

A `ResultSet` is a cursor, not a collection. Read each row and build a record or object; an ORM such as JPA or Hibernate automates exactly this mapping.

---

## Gotchas

- The driver jar must be on the class path; the JDK has only the API (`java.sql`)
- String-joined SQL is a SQL injection hole; use `PreparedStatement` with `?`
- Close `Connection`, `Statement`, and `ResultSet`; try-with-resources does it in the right order (see [AutoCloseable and try-with-resources](33_autocloseable.md))
- `ResultSet` column indexes are 1-based, not 0-based
- A `Connection` is not thread-safe; use a pool, or one per thread
- `SQLException` is checked, so it must be handled, and it often wraps the real cause worth logging

