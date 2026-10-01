# 59 - Security Basics

Rules that prevent the most common mistakes. Everything here uses `System.Security.Cryptography` and the base library, so no packages are needed.

## The Core Rules

1. Never trust input from outside your program
2. Never store passwords. Store a salted hash
3. Never write your own encryption. Use the built-in classes
4. Never put secrets in source code
5. Give code and users the least access they need

---

## Hashing

A hash turns any data into a fixed-size fingerprint. It cannot be reversed, and the same input always gives the same output.

```csharp
using System.Security.Cryptography;
using System.Text;

byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes("hello"));
Console.WriteLine(Convert.ToHexString(hash));
```

Expected output should be:

```
2CF24DBA5FB0A30E26E83B2AC5B9E29E1B161E5C1FA7425E73043362938B9824
```

Hash a file without loading it all into memory:

```csharp
using FileStream stream = File.OpenRead("download.zip");
string fileHash = Convert.ToHexString(SHA256.HashData(stream));
```

Use it to check that a download was not changed or corrupted.

| Algorithm        | Use                                      |
| ---------------- | ---------------------------------------- |
| SHA-256, SHA-512 | File checks, fingerprints, signatures    |
| MD5, SHA-1       | Do not use for security. They are broken |

---

## Passwords

A plain SHA-256 hash is too fast for passwords. Attackers try billions of guesses per second. Use a slow, salted key derivation function.

- **Salt**: random bytes added to each password, so two users with the same password get different hashes
- **Iterations**: how many times the work is repeated, to make each guess slow

```csharp
static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 600_000;

    public static string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);

        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public static bool Verify(string password, string stored)
    {
        string[] parts = stored.Split('.');
        int iterations = int.Parse(parts[0]);
        byte[] salt = Convert.FromBase64String(parts[1]);
        byte[] expected = Convert.FromBase64String(parts[2]);

        byte[] actual = Rfc2898DeriveBytes.Pbkdf2(
            password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);

        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }
}

string stored = PasswordHasher.Hash("s3cret");
Console.WriteLine(PasswordHasher.Verify("s3cret", stored));   // True
Console.WriteLine(PasswordHasher.Verify("wrong", stored));    // False
```

The stored text holds the iteration count, the salt, and the hash. That lets you raise the iterations later and still verify old passwords. Check the current advice from OWASP for the iteration number, since it grows over time.

---

## Compare Secrets in Fixed Time

`==` and `SequenceEqual` stop at the first difference, and the time they take can leak how much matched. Use a fixed-time comparison for hashes, tokens, and signatures.

```csharp
bool same = CryptographicOperations.FixedTimeEquals(actual, expected);
```

---

## Random Bytes for Tokens and Keys

`Random` is predictable. For anything secret, use the cryptographic generator.

```csharp
byte[] token = RandomNumberGenerator.GetBytes(32);
string text = Convert.ToHexString(token);

int dice = RandomNumberGenerator.GetInt32(1, 7);   // 1 to 6
```

See [Math, Random and Utility Types](12_math_random_and_utilities.md).

---

## Message Signatures (HMAC)

Proves a message came from someone who knows the key and was not changed.

```csharp
byte[] key = RandomNumberGenerator.GetBytes(32);
byte[] message = Encoding.UTF8.GetBytes("pay 100 to Alice");

byte[] mac = HMACSHA256.HashData(key, message);

bool valid = CryptographicOperations.FixedTimeEquals(
    mac, HMACSHA256.HashData(key, message));   // True
```

---

## Encryption

Encryption hides data and gives it back with the key. Use AES-GCM, which also detects tampering.

```csharp
byte[] key = RandomNumberGenerator.GetBytes(32);                       // keep this secret
byte[] nonce = RandomNumberGenerator.GetBytes(AesGcm.NonceByteSizes.MaxSize);   // new for every message
byte[] plain = Encoding.UTF8.GetBytes("secret text");

byte[] cipher = new byte[plain.Length];
byte[] tag = new byte[AesGcm.TagByteSizes.MaxSize];

using (var aes = new AesGcm(key, tag.Length))
{
    aes.Encrypt(nonce, plain, cipher, tag);
}

byte[] decrypted = new byte[cipher.Length];
using (var aes = new AesGcm(key, tag.Length))
{
    aes.Decrypt(nonce, cipher, tag, decrypted);   // throws if the data was changed
}

Console.WriteLine(Encoding.UTF8.GetString(decrypted));   // secret text
```

You must store the nonce and tag with the cipher text. Never reuse a nonce with the same key.

| Need                         | Use                                                  |
| ---------------------------- | ---------------------------------------------------- |
| Hide data and detect changes | `AesGcm`                                             |
| Prove who sent a message     | `HMACSHA256`, or digital signatures (`ECDsa`, `RSA`) |
| Check a file is unchanged    | `SHA256`                                             |
| Store a password             | `Rfc2898DeriveBytes.Pbkdf2`                          |

The hard part is key management, not the code. Where does the key live, and who can read it?

---

## Validate Input

Treat everything from a user, a file, a network call, or a command line as hostile.

```csharp
static bool IsValidAge(string? input)
    => int.TryParse(input, out int age) && age is >= 0 and <= 150;

static bool IsValidUsername(string? input)
    => input is { Length: >= 3 and <= 20 }
       && input.All(c => char.IsAsciiLetterOrDigit(c) || c == '_');
```

| Rule                        | Meaning                                                                        |
| --------------------------- | ------------------------------------------------------------------------------ |
| Allow list, not block list  | Say what is allowed (letters, digits, `_`). Do not try to list every bad thing |
| Check type, range, length   | `TryParse`, minimum and maximum, maximum text length                           |
| Validate on the server side | Checks in a browser or app screen can be skipped by an attacker                |
| Reject, do not repair       | Refuse bad input instead of silently "cleaning" it                             |

See [Type Conversion](10_type_conversion.md) for `TryParse` and [Regular Expressions](47_regular_expressions.md) for patterns (set a timeout when the text is untrusted).

---

## Injection

Injection happens when input becomes part of a command. The fix is always the same: keep data and commands apart.

| Attack            | Bad                                                 | Good                                                |
| ----------------- | --------------------------------------------------- | --------------------------------------------------- |
| SQL injection     | `"SELECT * FROM Users WHERE Name = '" + name + "'"` | A parameterized query: `WHERE Name = @name`         |
| Command injection | `Process.Start("sh", "-c ping " + host)`            | Pass arguments as separate values, validate `host`  |
| HTML injection    | Putting user text straight into a page              | Encode it: `System.Net.WebUtility.HtmlEncode(text)` |

Database drivers all support parameters. Never build SQL by joining strings with user input.

---

## Path Traversal

A user sends `../../etc/passwd` as a file name. Check that the final path stays inside your folder.

```csharp
static bool IsInside(string baseDir, string userInput)
{
    string full = Path.GetFullPath(Path.Combine(baseDir, userInput));
    string root = Path.GetFullPath(baseDir) + Path.DirectorySeparatorChar;

    return full.StartsWith(root, StringComparison.Ordinal);
}

IsInside("/data/uploads", "report.txt");          // True
IsInside("/data/uploads", "../../etc/passwd");    // False
IsInside("/data/uploads", "/etc/passwd");         // False
```

`Path.Combine` throws away the first part when the second is an absolute path, so `Path.Combine("/data/uploads", "/etc/passwd")` gives `/etc/passwd`. That is why the check works on the final full path.

---

## Secrets

A secret is a password, API key, token, or connection string.

| Do                                                | Do not                                                 |
| ------------------------------------------------- | ------------------------------------------------------ |
| Read from environment variables or a secret store | Write them in `.cs` files or `appsettings.json` in git |
| Add secret files to `.gitignore`                  | Commit `.env` files                                    |
| Use different secrets per environment             | Share one key across dev and production                |
| Rotate a secret after it leaks                    | Assume deleting the commit removes it                  |
| Keep secrets out of logs and error messages       | Print a connection string "for debugging"              |

```csharp
string? apiKey = Environment.GetEnvironmentVariable("MY_API_KEY");

if (string.IsNullOrEmpty(apiKey))
{
    throw new InvalidOperationException("MY_API_KEY is not set");
}
```

Hide most of a value when you must show it:

```csharp
static string Mask(string card) => new string('*', card.Length - 4) + card[^4..];

Console.WriteLine(Mask("4111111111111111"));   // ************1111
```

Once a secret is in git history, treat it as public. Change it. For app settings and secret stores, see [Common Libraries](63_libraries.md).

---

## Other Habits

- Use HTTPS for every network call. `HttpClient` rejects bad certificates by default. Never turn that check off to "make it work"
- Do not deserialize data from untrusted sources into types that run code. `System.Text.Json` is safe for plain data. Avoid `BinaryFormatter`, which was removed because it cannot be made safe
- Keep packages updated. `dotnet list package --vulnerable` shows packages with known problems
- Show users a short, generic error. Log the details privately (see [Error Handling](42_error_handling.md))
- Run programs with the least permissions they need
- Do not log passwords, tokens, or full card numbers

---

## Gotchas

- A plain hash (`SHA256`) of a password is not safe storage. Use `Pbkdf2` with a salt
- Hashing is not encryption. You cannot get the original back from a hash
- `MD5` and `SHA1` are acceptable only to detect accidental file corruption, not for security
- `Random` and `Guid.NewGuid()` are not meant to be secret tokens. Use `RandomNumberGenerator`
- Encryption with a key stored next to the data protects nothing
- Client-side validation is for user comfort. The server must check again
- Security is more than code. Review access, backups, and who holds the keys

---

## Examples

- [59-01](examples/59-01_hashing_and_passwords.cs): SHA-256, salted password hashes, HMAC
- [59-02](examples/59-02_encrypt_and_validate.cs): AES-GCM encryption, input validation, and safe paths

Run one with `dotnet run examples/59-01_hashing_and_passwords.cs`. See [Examples](examples/README.md).
