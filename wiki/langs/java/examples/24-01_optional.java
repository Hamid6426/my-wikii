// Lesson 24: Null and Optional (../24_null_and_optional.md)
// Optional
// Run: java 24-01_optional.java

import java.util.List;
import java.util.Optional;

public class FindUser {
    record User(String name, String email) {}

    static final List<User> USERS = List.of(
        new User("alice", "alice@example.com"),
        new User("bob", null));

    static Optional<User> find(String name) {
        for (User u : USERS) {
            if (u.name().equals(name)) return Optional.of(u);
        }
        return Optional.empty();
    }

    public static void main(String[] args) {
        System.out.println(find("alice").isPresent());
        System.out.println(find("zoe").isEmpty());

        String email = find("alice").map(User::email).orElse("no email");
        System.out.println(email);

        String bobEmail = find("bob")
            .map(User::email)              // email is null, so the result is empty
            .orElse("no email");
        System.out.println(bobEmail);

        find("zoe").ifPresentOrElse(
            u -> System.out.println("found " + u.name()),
            () -> System.out.println("no such user"));
    }
}
