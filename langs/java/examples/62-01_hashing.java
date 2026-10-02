// Lesson 62: Security Basics (../62_security_basics.md)
// Hashing
// Run: java 62-01_hashing.java

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
