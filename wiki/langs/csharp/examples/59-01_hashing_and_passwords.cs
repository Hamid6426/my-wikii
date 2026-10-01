// Lesson 59: Security Basics (../59_security_basics.md)
// SHA-256, salted password hashes, HMAC
// Run: dotnet run 59-01_hashing_and_passwords.cs

using System.Security.Cryptography;
using System.Text;

byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes("hello"));
Console.WriteLine($"SHA-256 of 'hello': {Convert.ToHexString(hash)}");
Console.WriteLine($"Same input, same hash: {Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes("hello"))) == Convert.ToHexString(hash)}");

string stored = PasswordHasher.Hash("s3cret!");
string stored2 = PasswordHasher.Hash("s3cret!");
Console.WriteLine($"Stored value has 3 parts: {stored.Split('.').Length == 3}");
Console.WriteLine($"Two hashes of the same password differ (salt): {stored != stored2}");
Console.WriteLine($"Correct password: {PasswordHasher.Verify("s3cret!", stored)}");
Console.WriteLine($"Wrong password:   {PasswordHasher.Verify("guess", stored)}");

byte[] key = RandomNumberGenerator.GetBytes(32);
byte[] mac = HMACSHA256.HashData(key, Encoding.UTF8.GetBytes("pay 100 to Alice"));
byte[] tampered = HMACSHA256.HashData(key, Encoding.UTF8.GetBytes("pay 900 to Alice"));
Console.WriteLine($"Message intact:   {CryptographicOperations.FixedTimeEquals(mac, HMACSHA256.HashData(key, Encoding.UTF8.GetBytes("pay 100 to Alice")))}");
Console.WriteLine($"Message tampered: {CryptographicOperations.FixedTimeEquals(mac, tampered)}");

Console.WriteLine($"Token: {Convert.ToHexString(RandomNumberGenerator.GetBytes(8)).Length} hex characters");

static class PasswordHasher
{
    private const int Iterations = 600_000;

    public static string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, 32);
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public static bool Verify(string password, string stored)
    {
        string[] parts = stored.Split('.');
        byte[] salt = Convert.FromBase64String(parts[1]);
        byte[] expected = Convert.FromBase64String(parts[2]);
        byte[] actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, int.Parse(parts[0]), HashAlgorithmName.SHA256, expected.Length);
        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }
}

// Expected output should be:
// SHA-256 of 'hello': 2CF24DBA5FB0A30E26E83B2AC5B9E29E1B161E5C1FA7425E73043362938B9824
// Same input, same hash: True
// Stored value has 3 parts: True
// Two hashes of the same password differ (salt): True
// Correct password: True
// Wrong password:   False
// Message intact:   True
// Message tampered: False
// Token: 16 hex characters
