// Lesson 55: Annotations and Reflection (../55_annotations_and_reflection.md)
// Reading annotations a validator
// Run: java 55-01_reading_annotations_a_validator.java

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;
import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.List;

public class Validate {
    @Retention(RetentionPolicy.RUNTIME)     // keep it so reflection can see it
    @Target(ElementType.FIELD)              // only allowed on fields
    @interface MaxLength {
        int value();
    }

    static class User {
        @MaxLength(5)
        String name;

        @MaxLength(10)
        String city;

        int age;

        User(String name, String city, int age) {
            this.name = name;
            this.city = city;
            this.age = age;
        }
    }

    static List<String> check(Object obj) throws IllegalAccessException {
        List<String> errors = new ArrayList<>();
        for (Field field : obj.getClass().getDeclaredFields()) {
            MaxLength max = field.getAnnotation(MaxLength.class);
            if (max == null) continue;

            String text = (String) field.get(obj);
            if (text != null && text.length() > max.value()) {
                errors.add(field.getName() + " is longer than " + max.value());
            }
        }
        return errors;
    }

    public static void main(String[] args) throws IllegalAccessException {
        System.out.println(check(new User("Alice", "Lahore", 30)));
        System.out.println(check(new User("Alexandra", "Lahore", 30)));
    }
}
