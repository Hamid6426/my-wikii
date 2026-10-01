// Lesson 59: Security Basics (../59_security_basics.md)
// AES-GCM encryption, input validation, and safe paths
// Run: dotnet run 59-02_encrypt_and_validate.cs

using System.Security.Cryptography;
using System.Text;

byte[] key = RandomNumberGenerator.GetBytes(32);
byte[] nonce = RandomNumberGenerator.GetBytes(AesGcm.NonceByteSizes.MaxSize);
byte[] plain = Encoding.UTF8.GetBytes("meet at noon");
byte[] cipher = new byte[plain.Length];
byte[] tag = new byte[AesGcm.TagByteSizes.MaxSize];

using (var aes = new AesGcm(key, tag.Length)) aes.Encrypt(nonce, plain, cipher, tag);
Console.WriteLine($"Cipher text differs from plain text: {!cipher.SequenceEqual(plain)}");

byte[] decrypted = new byte[cipher.Length];
using (var aes = new AesGcm(key, tag.Length)) aes.Decrypt(nonce, cipher, tag, decrypted);
Console.WriteLine($"Decrypted: {Encoding.UTF8.GetString(decrypted)}");

cipher[0] ^= 1;                                   // an attacker changes one bit
try
{
    using var aes = new AesGcm(key, tag.Length);
    aes.Decrypt(nonce, cipher, tag, decrypted);
}
catch (CryptographicException)
{
    Console.WriteLine("Tampering was detected");
}

foreach (string age in new[] { "42", "-1", "abc", "200" })
{
    Console.WriteLine($"age '{age}' valid? {IsValidAge(age)}");
}
foreach (string name in new[] { "alice_01", "x", "bob; DROP TABLE users" })
{
    Console.WriteLine($"username '{name}' valid? {IsValidUsername(name)}");
}

string uploads = Path.Combine(Path.GetTempPath(), "uploads");
foreach (string requested in new[] { "report.txt", "../../etc/passwd", "/etc/passwd" })
{
    Console.WriteLine($"'{requested}' inside uploads? {IsInside(uploads, requested)}");
}

static bool IsValidAge(string? input) => int.TryParse(input, out int age) && age is >= 0 and <= 150;

static bool IsValidUsername(string? input)
    => input is { Length: >= 3 and <= 20 } && input.All(c => char.IsAsciiLetterOrDigit(c) || c == '_');

static bool IsInside(string baseDir, string userInput)
{
    string full = Path.GetFullPath(Path.Combine(baseDir, userInput));
    string root = Path.GetFullPath(baseDir) + Path.DirectorySeparatorChar;
    return full.StartsWith(root, StringComparison.Ordinal);
}

// Expected output should be:
// Cipher text differs from plain text: True
// Decrypted: meet at noon
// Tampering was detected
// age '42' valid? True
// age '-1' valid? False
// age 'abc' valid? False
// age '200' valid? False
// username 'alice_01' valid? True
// username 'x' valid? False
// username 'bob; DROP TABLE users' valid? False
// 'report.txt' inside uploads? True
// '../../etc/passwd' inside uploads? False
// '/etc/passwd' inside uploads? False
