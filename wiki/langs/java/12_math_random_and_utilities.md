# 12 - Math, Random and Utility Classes

## Math

`Math` (in `java.lang`, no import needed) has static methods for common math. Every method is called on the class: `Math.sqrt(16)`.

```java
System.out.println(Math.abs(-5));          // 5
System.out.println(Math.max(3, 7));        // 7
System.out.println(Math.min(3, 7));        // 3
System.out.println(Math.pow(2, 10));       // 1024.0: always a double
System.out.println(Math.sqrt(16));         // 4.0
System.out.println(Math.cbrt(27));         // 3.0: cube root
System.out.println(Math.hypot(3, 4));      // 5.0: sqrt(3*3 + 4*4)
System.out.println(Math.signum(-3.2));     // -1.0: the sign as -1, 0, or 1
System.out.println(Math.clamp(15, 0, 10)); // 10: keep in range (Java 21+)
System.out.println(Math.PI);               // 3.141592653589793
System.out.println(Math.E);                // 2.718281828459045
```

---

## Rounding Rules

```java
System.out.println(Math.round(2.5));    // 3: half rounds up, toward positive infinity
System.out.println(Math.round(-2.5));   // -2: also toward positive infinity
System.out.println(Math.rint(2.5));     // 2.0: half rounds to the even neighbour
System.out.println(Math.rint(3.5));     // 4.0
System.out.println(Math.floor(2.7));    // 2.0: down
System.out.println(Math.ceil(2.1));     // 3.0: up
System.out.println(Math.floor(-2.7));   // -3.0: down means more negative
```

| Method          | Returns                         | Rule                                             |
| --------------- | ------------------------------- | ------------------------------------------------ |
| `Math.round(x)` | `long` (or `int` for a `float`) | Nearest, half goes up                            |
| `Math.rint(x)`  | `double`                        | Nearest, half goes to even ("banker's rounding") |
| `Math.floor(x)` | `double`                        | Down                                             |
| `Math.ceil(x)`  | `double`                        | Up                                               |
| `(int) x`       | `int`                           | Cut toward zero                                  |

To round to a number of decimal places, use `BigDecimal` (below) or format the output with `String.format("%.2f", x)`.

---

## Exact and Floor Arithmetic

```java
Math.multiplyExact(1_000_000, 1_000_000);       // throws ArithmeticException: integer overflow
long ok = Math.multiplyExact(1_000_000L, 1_000_000L);   // 1000000000000
int small = Math.toIntExact(3_000_000_000L);     // throws: does not fit in an int

int q = Math.floorDiv(-7, 2);    // -4: rounds down (plain -7 / 2 is -3)
int m = Math.floorMod(-7, 2);    // 1: never negative here (plain -7 % 2 is -1)
```

`addExact`, `subtractExact`, `multiplyExact`, `incrementExact`, and `negateExact` all throw instead of wrapping around. Use them for money, sizes, and counters.

---

## Trigonometry and Logs

```java
System.out.println(Math.sin(Math.toRadians(30)));   // 0.49999999999999994
System.out.println(Math.cos(0));                    // 1.0
System.out.println(Math.toDegrees(Math.atan2(1, 1))); // 45.0
System.out.println(Math.log(Math.E));               // 1.0: natural log
System.out.println(Math.log10(1000));               // 3.0
System.out.println(Math.exp(1));                    // 2.718281828459045
System.out.println(Math.log(8) / Math.log(2));      // 3.0: log base 2
```

Angles are in radians. Convert with `Math.toRadians` and `Math.toDegrees`.

---

## Random

`Random` (in `java.util`) makes pseudo-random numbers: they look random but come from a formula.

```java
Random random = new Random();

int dice = random.nextInt(1, 7);        // 1 to 6: the upper bound is excluded (Java 17+)
int below10 = random.nextInt(10);       // 0 to 9
double fraction = random.nextDouble();  // 0.0 up to, not including, 1.0
boolean coin = random.nextBoolean();
long id = random.nextLong();
```

| Option                                      | When to use                                                  |
| ------------------------------------------- | ------------------------------------------------------------ |
| `new Random()`                              | Simple programs and games                                    |
| `ThreadLocalRandom.current().nextInt(1, 7)` | Code that runs on many threads                               |
| `RandomGenerator.getDefault()`              | The modern interface (Java 17+), lets you swap the algorithm |
| `Math.random()`                             | A quick `double` from 0.0 to 1.0; old style                  |
| `SecureRandom`                              | Passwords, tokens, keys (see below)                          |

---

## Seeds

A seed is the starting number for the formula. The same seed gives the same sequence every run, which helps tests and reproducible simulations.

```java
Random seeded = new Random(42);
System.out.println(seeded.nextInt(100) + " " + seeded.nextInt(100) + " " + seeded.nextInt(100));

Random again = new Random(42);
System.out.println(again.nextInt(100) + " " + again.nextInt(100) + " " + again.nextInt(100));
```

Expected output should be:

```
30 63 48
30 63 48
```

`java.util.Random` uses the same formula on every JVM, so these numbers are the same everywhere.

---

## Shuffle and Pick

```java
List<String> cards = new ArrayList<>(List.of("A", "B", "C", "D", "E"));

Collections.shuffle(cards, new Random(7));   // shuffles in place
System.out.println(cards);                   // [E, D, A, C, B]

Random random = new Random();
String pick = cards.get(random.nextInt(cards.size()));   // one random item
```

`Collections.shuffle(list)` without a `Random` uses a new random order each run. The list must be changeable: `List.of(...)` alone throws. See [Collections](36_collections.md).

---

## Random for Security

`Random` is predictable. For passwords, tokens, and session IDs use `SecureRandom` (in `java.security`).

```java
SecureRandom secure = new SecureRandom();

byte[] token = new byte[16];
secure.nextBytes(token);

String hex = HexFormat.of().formatHex(token);                              // 32 hex characters
String urlSafe = Base64.getUrlEncoder().withoutPadding().encodeToString(token); // 22 characters
int code = secure.nextInt(100_000, 1_000_000);                             // 6-digit code
```

See [Security Basics](62_security_basics.md).

---

## UUID

A UUID (universally unique identifier) is a 128-bit ID that is, in practice, never repeated. Good for IDs created on many machines with no shared counter.

```java
UUID id = UUID.randomUUID();                  // random (version 4)
System.out.println(id);                       // such as 3f1c2a9e-8b7d-4e21-9c6a-0d5b7e4f2a10

UUID parsed = UUID.fromString("123e4567-e89b-12d3-a456-426614174000");
String text = parsed.toString();              // back to the same text
```

The text form is always 36 characters: 32 hex digits and 4 hyphens.

---

## Environment and System

```java
String home = System.getenv("HOME");                          // null if not set
String mode = System.getenv().getOrDefault("APP_MODE", "dev"); // with a fallback

String version = System.getProperty("java.version");         // such as 25.0.1
String os = System.getProperty("os.name");                    // Linux, Windows 11, Mac OS X
String dir = System.getProperty("user.dir");                  // the current folder
String flag = System.getProperty("app.mode", "dev");          // set with: java -Dapp.mode=prod

int cpus = Runtime.getRuntime().availableProcessors();
int feature = Runtime.version().feature();                    // 25

long start = System.nanoTime();                               // for measuring time spent
long elapsedMs = (System.nanoTime() - start) / 1_000_000;
long now = System.currentTimeMillis();                        // wall clock, ms since 1970
```

| Source               | Set by                                | Read with                        |
| -------------------- | ------------------------------------- | -------------------------------- |
| Environment variable | The shell: `export APP_MODE=prod`     | `System.getenv("APP_MODE")`      |
| System property      | The `java` command: `-Dapp.mode=prod` | `System.getProperty("app.mode")` |

Use `nanoTime` to measure how long something takes. Use `java.time` for dates (see [Dates and Times](21_dates_and_times.md)).

---

## Starting Another Program

`ProcessBuilder` runs another program and reads its output.

```java
import java.io.IOException;

public class Run {
    public static void main(String[] args) throws IOException, InterruptedException {
        Process process = new ProcessBuilder("java", "-version")
                .redirectErrorStream(true)           // join error output into normal output
                .start();

        String output = new String(process.getInputStream().readAllBytes());
        int exitCode = process.waitFor();

        System.out.println("Exit code: " + exitCode);
        System.out.println(output.lines().findFirst().orElse(""));
    }
}
```

Expected output should be (the version line depends on your JDK):

```
Exit code: 0
openjdk version "25.0.4.1" 2026-08-18
```

To let the child print straight to your console, use `.inheritIO()` instead of reading the output.

Pass each argument as its own string: `new ProcessBuilder("git", "commit", "-m", message)`. No shell runs in between, so spaces and quotes in `message` are safe.

---

## BigInteger and BigDecimal

Both are in `java.math`. They have no size limit and no rounding surprises, but they are slower and use methods instead of operators.

```java
BigInteger factorial = BigInteger.ONE;
for (int i = 2; i <= 30; i++) {
    factorial = factorial.multiply(BigInteger.valueOf(i));
}
System.out.println(factorial);                     // 265252859812191058636308480000000
System.out.println(BigInteger.TWO.pow(100));       // 1267650600228229401496703205376
```

`BigDecimal` stores exact decimal numbers. Use it for money.

```java
BigDecimal price = new BigDecimal("19.99");
BigDecimal total = price.multiply(BigDecimal.valueOf(3));
System.out.println(total);                                             // 59.97

System.out.println(new BigDecimal("0.1").add(new BigDecimal("0.2")));  // 0.3
System.out.println(new BigDecimal("2.345").setScale(2, RoundingMode.HALF_UP));   // 2.35
System.out.println(new BigDecimal("2.345").setScale(2, RoundingMode.HALF_EVEN)); // 2.34
System.out.println(new BigDecimal("10").divide(new BigDecimal("3"), 4, RoundingMode.HALF_UP)); // 3.3333
```

| Pitfall                                                           | Fix                                                                  |
| ----------------------------------------------------------------- | -------------------------------------------------------------------- |
| `new BigDecimal(0.1)` is `0.1000000000000000055511...`            | Pass a string: `new BigDecimal("0.1")`, or `BigDecimal.valueOf(0.1)` |
| `divide` throws when the answer never ends, such as 10 / 3        | Give a scale and a `RoundingMode`                                    |
| `new BigDecimal("2.0").equals(new BigDecimal("2.00"))` is `false` | Use `compareTo(...) == 0`                                            |
| Values never change: `price.add(x)` alone does nothing            | Keep the result: `price = price.add(x)`                              |

---

## Other Small Helpers

| Class       | Method                                     | Result                                                          |
| ----------- | ------------------------------------------ | --------------------------------------------------------------- |
| `Integer`   | `Integer.compare(3, 7)`                    | `-1`: negative, zero, or positive                               |
| `Integer`   | `Integer.sum(3, 4)`                        | `7`: handy as a method reference                                |
| `Integer`   | `Integer.bitCount(255)`                    | `8`: how many 1 bits                                            |
| `Character` | `Character.isDigit('7')`                   | `true`                                                          |
| `Character` | `Character.isLetter('x')`                  | `true`                                                          |
| `Character` | `Character.toUpperCase('q')`               | `'Q'`                                                           |
| `Objects`   | `Objects.equals(a, b)`                     | `equals` that is safe with `null`                               |
| `Objects`   | `Objects.requireNonNull(x, "message")`     | Throws `NullPointerException` with the message if `x` is `null` |
| `Objects`   | `Objects.requireNonNullElse(x, "default")` | `x`, or the default if `x` is `null`                            |
| `Objects`   | `Objects.toString(x, "none")`              | Text, with a fallback for `null`                                |

`Objects` is in `java.util`. `Integer`, `Character`, and `Math` are in `java.lang`.

---

## Gotchas

- `Math.pow` returns a `double`; `(int) Math.pow(10, 2)` is `100`, but very large results lose precision
- `Math.round(-2.5)` is `-2`, not `-3`
- `Math.abs(Integer.MIN_VALUE)` is still negative, because the positive value does not fit in an `int`
- `random.nextInt(1, 7)` never returns `7`; the upper bound is excluded
- Creating a `new Random()` inside a loop is wasteful; create one and reuse it
- Never use `Random` or `Math.random()` for passwords or tokens; use `SecureRandom`
- `System.getenv` returns `null` for a missing variable; give a default
- Read a child process's output, or it can block when its output buffer fills up
- Build `BigDecimal` from strings, not doubles

---

## Examples

- [12-01](examples/12-01_starting_another_program.java): Starting another program

Run one with `java examples/12-01_starting_another_program.java`. See [Examples](examples/README.md).
