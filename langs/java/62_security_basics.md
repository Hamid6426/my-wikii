# 62 - Security Basics

Rules that prevent the most common mistakes. Everything here uses `java.security`, `javax.crypto`, and the base library, so no extra libraries are needed.

## The Core Rules

1. Never trust input from outside your program
2. Never store passwords. Store a salted hash
3. Never write your own encryption. Use the built-in classes
4. Never put secrets in source code
5. Give code and users the least access they need

---

## Hashing

A hash turns any data into a fixed-size fingerprint. It cannot be reversed, and the same input always gives the same output.

```java
import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.HexFormat;

public class Hash {
    public static void main(String[] args) throws NoSuchAlgorithmException {
        MessageDigest sha = MessageDigest.getInstance("SHA-256");
        byte[] hash = sha.digest("hello".getBytes(StandardCharsets.UTF_8));

        System.out.println(HexFormat.of().formatHex(hash));
    }
}
```

Expected output should be:

```
2cf24dba5fb0a30e26e83b2ac5b9e29e1b161e5c1fa7425e73043362938b9824
```

Hash a file without loading it all into memory:

```java
MessageDigest sha = MessageDigest.getInstance("SHA-256");

try (InputStream in = new DigestInputStream(Files.newInputStream(Path.of("download.zip")), sha)) {
    in.transferTo(OutputStream.nullOutputStream());   // reading feeds the digest
}
String fileHash = HexFormat.of().formatHex(sha.digest());
```

Use it to check that a download was not changed or corrupted. See [File I/O: Streams](47_file_io_streams.md).

| Algorithm        | Use                                      |
| ---------------- | ---------------------------------------- |
| SHA-256, SHA-512 | File checks, fingerprints, signatures    |
| SHA3-256         | A newer family, also fine                |
| MD5, SHA-1       | Do not use for security. They are broken |

---

## Passwords

A plain SHA-256 hash is too fast for passwords. Attackers try billions of guesses per second. Use a slow, salted key derivation function: an algorithm that turns a password into a key and is slow on purpose.

- **Salt**: random bytes added to each password, so two users with the same password get different hashes
- **Iterations**: how many times the work is repeated, to make each guess slow

```java
import java.security.GeneralSecurityException;
import java.security.MessageDigest;
import java.security.SecureRandom;
import java.util.Base64;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.PBEKeySpec;

public class Passwords {
    public static void main(String[] args) throws GeneralSecurityException {
        String stored = PasswordHasher.hash("s3cret".toCharArray());

        System.out.println(PasswordHasher.verify("s3cret".toCharArray(), stored));   // true
        System.out.println(PasswordHasher.verify("wrong".toCharArray(), stored));    // false
    }
}

final class PasswordHasher {
    private static final int SALT_SIZE = 16;
    private static final int HASH_BITS = 256;
    private static final int ITERATIONS = 600_000;
    private static final SecureRandom RANDOM = new SecureRandom();

    private PasswordHasher() { }

    static String hash(char[] password) throws GeneralSecurityException {
        byte[] salt = new byte[SALT_SIZE];
        RANDOM.nextBytes(salt);
        byte[] hash = pbkdf2(password, salt, ITERATIONS);

        var b64 = Base64.getEncoder();
        return ITERATIONS + "." + b64.encodeToString(salt) + "." + b64.encodeToString(hash);
    }

    static boolean verify(char[] password, String stored) throws GeneralSecurityException {
        String[] parts = stored.split("\\.");
        int iterations = Integer.parseInt(parts[0]);
        byte[] salt = Base64.getDecoder().decode(parts[1]);
        byte[] expected = Base64.getDecoder().decode(parts[2]);

        byte[] actual = pbkdf2(password, salt, iterations);
        return MessageDigest.isEqual(actual, expected);   // fixed-time comparison
    }

    private static byte[] pbkdf2(char[] password, byte[] salt, int iterations)
            throws GeneralSecurityException {
        var spec = new PBEKeySpec(password, salt, iterations, HASH_BITS);
        try {
            return SecretKeyFactory.getInstance("PBKDF2WithHmacSHA256")
                    .generateSecret(spec)
                    .getEncoded();
        } finally {
            spec.clearPassword();
        }
    }
}
```

Expected output should be:

```
true
false
```

The stored text holds the iteration count, the salt, and the hash. That lets you raise the iterations later and still verify old passwords. Check the current advice from OWASP (the Open Worldwide Application Security Project) for the iteration number, since it grows over time.

Passwords are passed as `char[]`, not `String`, so the array can be wiped after use. A `String` stays in memory until the garbage collector removes it.

---

## Compare Secrets in Fixed Time

`Arrays.equals` and `String.equals` stop at the first difference, and the time they take can leak how much matched. Use a fixed-time comparison for hashes, tokens, and signatures.

```java
boolean same = MessageDigest.isEqual(actual, expected);
```

---

## Random Bytes for Tokens and Keys

`Random` and `Math.random()` are predictable. For anything secret, use `SecureRandom`.

```java
SecureRandom random = new SecureRandom();

byte[] token = new byte[32];
random.nextBytes(token);
String text = HexFormat.of().formatHex(token);   // 64 hex characters

int dice = random.nextInt(1, 7);                 // 1 to 6
```

Create one `SecureRandom` and reuse it. See [Math, Random and Utility Classes](12_math_random_and_utilities.md).

---

## Message Signatures (HMAC)

HMAC (hash-based message authentication code) proves a message came from someone who knows the key and was not changed.

```java
byte[] key = new byte[32];
random.nextBytes(key);
byte[] message = "pay 100 to Alice".getBytes(StandardCharsets.UTF_8);

Mac hmac = Mac.getInstance("HmacSHA256");
hmac.init(new SecretKeySpec(key, "HmacSHA256"));
byte[] mac = hmac.doFinal(message);

boolean valid = MessageDigest.isEqual(mac, hmac.doFinal(message));   // true
```

`Mac` is in `javax.crypto`, and `SecretKeySpec` in `javax.crypto.spec`.

---

## Encryption

Encryption hides data and gives it back with the key. Use AES-GCM, which also detects tampering.

```java
KeyGenerator generator = KeyGenerator.getInstance("AES");
generator.init(256);
SecretKey key = generator.generateKey();                  // keep this secret

byte[] iv = new byte[12];                                 // new for every message
random.nextBytes(iv);
byte[] plain = "secret text".getBytes(StandardCharsets.UTF_8);

Cipher cipher = Cipher.getInstance("AES/GCM/NoPadding");
cipher.init(Cipher.ENCRYPT_MODE, key, new GCMParameterSpec(128, iv));
byte[] encrypted = cipher.doFinal(plain);                 // cipher text plus a 16-byte tag

cipher.init(Cipher.DECRYPT_MODE, key, new GCMParameterSpec(128, iv));
byte[] decrypted = cipher.doFinal(encrypted);             // throws if the data was changed

System.out.println(new String(decrypted, StandardCharsets.UTF_8));   // secret text
```

The IV (initialization vector, also called a nonce) must be stored with the cipher text. It is not secret, but never reuse an IV with the same key. If anyone changes one byte, decryption throws `AEADBadTagException` with the message `Tag mismatch`.

| Need                             | Use                                                    |
| -------------------------------- | ------------------------------------------------------ |
| Hide data and detect changes     | `Cipher` with `AES/GCM/NoPadding`                      |
| Prove who sent a message         | `Mac` with `HmacSHA256`, or `Signature` with `Ed25519` |
| Check a file is unchanged        | `MessageDigest` with `SHA-256`                         |
| Store a password                 | `SecretKeyFactory` with `PBKDF2WithHmacSHA256`         |
| Derive keys from a shared secret | `KDF` with `HKDF-SHA256` (Java 25)                     |

Never use `Cipher.getInstance("AES")` on its own. It picks ECB mode, which leaks patterns in the data. The hard part is key management, not the code. Where does the key live, and who can read it?

---

## Validate Input

Treat everything from a user, a file, a network call, or a command line as hostile.

```java
static boolean isValidAge(String input) {
    try {
        int age = Integer.parseInt(input);
        return age >= 0 && age <= 150;
    } catch (NumberFormatException e) {
        return false;
    }
}

static boolean isValidUsername(String input) {
    return input != null && input.matches("[A-Za-z0-9_]{3,20}");
}
```

| Rule                        | Meaning                                                                        |
| --------------------------- | ------------------------------------------------------------------------------ |
| Allow list, not block list  | Say what is allowed (letters, digits, `_`). Do not try to list every bad thing |
| Check type, range, length   | Parse, then check minimum, maximum, and length                                 |
| Validate on the server side | Checks in a browser or app screen can be skipped by an attacker                |
| Reject, do not repair       | Refuse bad input instead of silently "cleaning" it                             |

See [Type Conversion](10_type_conversion.md) for parsing and [Regular Expressions](49_regular_expressions.md) for patterns. Keep patterns simple when the text is untrusted: some patterns take very long on crafted input.

---

## Injection

Injection happens when input becomes part of a command. The fix is always the same: keep data and commands apart.

| Attack            | Bad                                                 | Good                                                   |
| ----------------- | --------------------------------------------------- | ------------------------------------------------------ |
| SQL injection     | `"SELECT * FROM users WHERE name = '" + name + "'"` | A `PreparedStatement` with `?` placeholders            |
| Command injection | `Runtime.getRuntime().exec("sh -c ping " + host)`   | `new ProcessBuilder("ping", host)` and validate `host` |
| HTML injection    | Putting user text straight into a page              | Encode it with your web framework or template engine   |

```java
String sql = "SELECT id, email FROM users WHERE name = ?";

try (PreparedStatement statement = connection.prepareStatement(sql)) {
    statement.setString(1, name);                  // sent as data, never as SQL
    try (ResultSet rows = statement.executeQuery()) {
        while (rows.next()) {
            System.out.println(rows.getString("email"));
        }
    }
}
```

`PreparedStatement` is part of JDBC (`java.sql`), the JDK's database API. Never build SQL by joining strings with user input.

---

## Path Traversal

A user sends `../../etc/passwd` as a file name. Check that the final path stays inside your folder.

```java
static boolean isInside(Path baseDir, String userInput) {
    Path root = baseDir.toAbsolutePath().normalize();
    Path full = root.resolve(userInput).normalize();

    return full.startsWith(root);
}

isInside(Path.of("/data/uploads"), "report.txt");          // true
isInside(Path.of("/data/uploads"), "../../etc/passwd");    // false
isInside(Path.of("/data/uploads"), "/etc/passwd");         // false
isInside(Path.of("/data/uploads"), "../uploads2/x");       // false
```

`resolve` throws away the base when the input is an absolute path, so `Path.of("/data/uploads").resolve("/etc/passwd")` gives `/etc/passwd`. `normalize` removes `..` parts. `Path.startsWith` compares whole folder names, so `/data/uploads2` does not count as inside `/data/uploads`. A `String.startsWith` check would get that wrong.

---

## Secrets

A secret is a password, API key, token, or connection string.

| Do                                                | Do not                                                         |
| ------------------------------------------------- | -------------------------------------------------------------- |
| Read from environment variables or a secret store | Write them in `.java` files or `application.properties` in git |
| Add secret files to `.gitignore`                  | Commit `.env` files                                            |
| Use different secrets per environment             | Share one key across dev and production                        |
| Rotate a secret after it leaks                    | Assume deleting the commit removes it                          |
| Keep secrets out of logs and error messages       | Print a connection string "for debugging"                      |

```java
String apiKey = System.getenv("MY_API_KEY");

if (apiKey == null || apiKey.isBlank()) {
    throw new IllegalStateException("MY_API_KEY is not set");
}
```

Hide most of a value when you must show it:

```java
static String mask(String card) {
    return "*".repeat(card.length() - 4) + card.substring(card.length() - 4);
}

System.out.println(mask("4111111111111111"));   // ************1111
```

Once a secret is in git history, treat it as public. Change it.

---

## Other Habits

- Use HTTPS for every network call. `HttpClient` checks certificates by default. Never install a "trust all" `TrustManager` to "make it work" (see [HTTP Client](52_http_client.md))
- Do not deserialize untrusted data with `ObjectInputStream`. Java serialization can run code while reading. Use JSON with plain records instead (see [JSON](48_json.md)), or set a serialization filter
- Keep libraries updated. Tools such as OWASP Dependency-Check and GitHub Dependabot report libraries with known problems
- Show users a short, generic error. Log the details privately (see [Error Handling](42_error_handling.md))
- Run programs with the least operating system permissions they need. The old Security Manager was disabled in Java 24, so the JVM no longer sandboxes code for you
- Do not log passwords, tokens, or full card numbers

---

## Gotchas

- A plain hash (`SHA-256`) of a password is not safe storage. Use PBKDF2 with a salt
- Hashing is not encryption. You cannot get the original back from a hash
- `MD5` and `SHA-1` are acceptable only to detect accidental file corruption, not for security
- `Random`, `Math.random()`, and `UUID.randomUUID().toString()` as a session token are poor choices. Use `SecureRandom` bytes
- `Cipher.getInstance("AES")` means AES in ECB mode. Always name the mode: `AES/GCM/NoPadding`
- Encryption with a key stored next to the data protects nothing
- Client-side validation is for user comfort. The server must check again
- Security is more than code. Review access, backups, and who holds the keys

---

## Examples

- [62-01](examples/62-01_hashing.java): Hashing
- [62-02](examples/62-02_passwords.java): Passwords

Run one with `java examples/62-01_hashing.java`. See [Examples](examples/README.md).
