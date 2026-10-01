// Lesson 62: Security Basics (../62_security_basics.md)
// Passwords
// Run: java 62-02_passwords.java

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
