// Lesson 06: Packages and Imports (../06_packages_and_imports.md)
// Name clashes
// Run: java 06-02_name_clashes.java

import java.util.Date;

public class Clash {
    public static void main(String[] args) {
        Date now = new Date(0);                          // java.util.Date
        java.sql.Date sqlDate = new java.sql.Date(0);    // full name, no import
        System.out.println(now.getClass().getName());
        System.out.println(sqlDate.getClass().getName());
    }
}
