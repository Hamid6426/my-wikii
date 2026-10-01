# 55 - Annotations and Reflection

## Annotations

An **annotation** is a label you attach to code with `@`. It is metadata (data about the code). It does nothing on its own: the compiler, a tool, or a framework reads it and acts.

```java
@Override
public String toString() { return "User"; }

@Deprecated(since = "2.0", forRemoval = true)
public void oldMethod() { }

@SuppressWarnings("unchecked")
List<String> list = (List<String>) raw;
```

---

## Common Built-in Annotations

| Annotation                 | Effect                                                                   |
| -------------------------- | ------------------------------------------------------------------------ |
| `@Override`                | Compile error if the method does not override anything                   |
| `@Deprecated`              | Warning when someone uses it. `forRemoval = true` warns louder           |
| `@SuppressWarnings("...")` | Hides a compiler warning, such as `"unchecked"`                          |
| `@FunctionalInterface`     | Compile error if the interface has more than one abstract method         |
| `@SafeVarargs`             | Promises a generic varargs method is safe (see [Varargs](17_varargs.md)) |
| `@Serial`                  | Marks serialization members so the compiler can check them (Java 14)     |

Annotations from libraries you will meet:

| Annotation                 | From                | Does                                                                          |
| -------------------------- | ------------------- | ----------------------------------------------------------------------------- |
| `@Test`                    | JUnit               | Marks a test method (see [Testing](59_testing.md))                            |
| `@JsonProperty`            | Jackson             | Renames a field in JSON (see [JSON](48_json.md))                              |
| `@Autowired`, `@Component` | Spring              | Dependency injection (see [Dependency Injection](56_dependency_injection.md)) |
| `@Entity`, `@Id`           | Jakarta Persistence | Maps a class to a database table                                              |

---

## Writing Your Own Annotation

Declare it with `@interface`. Its **elements** look like methods with no body.

```java
@Retention(RetentionPolicy.RUNTIME)     // keep it so reflection can see it
@Target(ElementType.FIELD)              // only allowed on fields
@interface MaxLength {
    int value();                        // a required element
    String message() default "too long";   // an optional element
}

class User {
    @MaxLength(5)                       // "value" can skip its name
    String name;

    @MaxLength(value = 10, message = "city name too long")
    String city;
}
```

Elements can be primitives, `String`, `Class`, an enum, another annotation, or an array of these. Not `Integer`, not `List`, and never `null`.

| `@Retention`              | Kept until                                                 |
| ------------------------- | ---------------------------------------------------------- |
| `RetentionPolicy.SOURCE`  | Compiling only. Then dropped (like `@Override`)            |
| `RetentionPolicy.CLASS`   | The `.class` file, but not visible at runtime. The default |
| `RetentionPolicy.RUNTIME` | Runtime. Needed for reflection                             |

| `@Target`          | Allowed on                          |
| ------------------ | ----------------------------------- |
| `TYPE`             | Classes, interfaces, enums, records |
| `FIELD`            | Fields                              |
| `METHOD`           | Methods                             |
| `PARAMETER`        | Method parameters                   |
| `CONSTRUCTOR`      | Constructors                        |
| `RECORD_COMPONENT` | Record components                   |

Leave out `@Target` and the annotation is allowed almost anywhere.

---

## Reflection

**Reflection** lets a program look at its own classes, fields, and methods while it runs. Everything starts with a `Class` object.

```java
Class<?> c1 = Account.class;                         // from the type
Class<?> c2 = account.getClass();                    // from an object
Class<?> c3 = Class.forName("java.util.ArrayList");  // from a name (throws if not found)

c1.getSimpleName()             // Account
c3.getSuperclass()             // class java.util.AbstractList
c1.isRecord()                  // false
List.class.isAssignableFrom(ArrayList.class)   // true, an ArrayList is a List
```

| Method                                        | Returns                                                 |
| --------------------------------------------- | ------------------------------------------------------- |
| `getFields()`, `getMethods()`                 | Public members, including inherited ones                |
| `getDeclaredFields()`, `getDeclaredMethods()` | All members of this class (also private), not inherited |
| `getField("x")`, `getMethod("f", int.class)`  | One public member, by name and parameter types          |
| `getDeclaredConstructor(...)`                 | A constructor                                           |
| `getRecordComponents()`                       | The parts of a record, in order                         |
| `getAnnotation(X.class)`                      | The annotation, or `null`                               |

The order of `getDeclaredFields()` and `getDeclaredMethods()` is not guaranteed.

---

## Reading Annotations: A Validator

```java
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
```

Expected output should be:

```
[]
[name is longer than 5]
```

This is how validation libraries such as Jakarta Bean Validation (`@NotNull`, `@Size`) work.

---

## Reading and Setting Values

```java
class Account {
    private double balance = 100;
    public void deposit(double amount) { balance += amount; }
}

Object acc = Account.class.getDeclaredConstructor().newInstance();   // new Account()

Method deposit = Account.class.getMethod("deposit", double.class);
deposit.invoke(acc, 50.0);                       // acc.deposit(50.0)

Field balance = Account.class.getDeclaredField("balance");
balance.setAccessible(true);                     // allow access to a private field
System.out.println(balance.get(acc));            // 150.0
balance.set(acc, 999.0);
```

| Problem                                      | Exception                                                       |
| -------------------------------------------- | --------------------------------------------------------------- |
| No member with that name                     | `NoSuchMethodException`, `NoSuchFieldException`                 |
| Wrong argument type in `invoke`              | `IllegalArgumentException: argument type mismatch`              |
| The method itself threw                      | `InvocationTargetException`. Call `getCause()` for the real one |
| Private member without `setAccessible(true)` | `IllegalAccessException`                                        |

---

## Records and Reflection

```java
record Point(int x, int y) {}

for (RecordComponent rc : Point.class.getRecordComponents()) {
    System.out.println(rc.getName() + " = " + rc.getAccessor().invoke(new Point(3, 4)));
}
// x = 3
// y = 4
```

This is how JSON libraries read records without getters. See [Records and Equality](25_records_and_equality.md).

---

## Modules Limit Reflection

Since Java 17, code cannot reach private members inside the JDK.

```java
Field f = String.class.getDeclaredField("value");
f.setAccessible(true);    // throws InaccessibleObjectException
```

Your own classes in the **classpath** (not in a named module) stay open. A named module must `opens` a package to let frameworks reflect on it. See [Packages and Imports](06_packages_and_imports.md).

---

## Method Handles

`java.lang.invoke.MethodHandle` is a faster, type-checked way to call a method found at runtime.

```java
MethodHandle upper = MethodHandles.lookup()
    .findVirtual(String.class, "toUpperCase", MethodType.methodType(String.class));

String result = (String) upper.invokeExact("hello");   // HELLO
```

Frameworks use it under the hood. In everyday code, plain reflection is easier to read.

---

## Annotation Processors

Some annotations are read by the **compiler**, not at runtime. An **annotation processor** runs during the build and generates new code. Lombok (`@Getter`), MapStruct, and Dagger work this way. They are faster at runtime than reflection, because the work is done before the program starts.

---

## Gotchas

- An annotation without `@Retention(RetentionPolicy.RUNTIME)` is invisible to reflection: `getAnnotation` returns `null`
- Reflection is slower than a normal call. Look up `Method` and `Field` objects once and reuse them
- It skips compile-time checks, so a typo in a name string fails only at runtime
- `setAccessible(true)` breaks encapsulation. Use it in tools and frameworks, not in normal code
- `getMethods()` does not include private methods, and `getDeclaredMethods()` does not include inherited ones
- Prefer an interface or generics over reflection when you can (see [Generics](54_generics.md))

---

## Examples

- [55-01](examples/55-01_reading_annotations_a_validator.java): Reading annotations a validator

Run one with `java examples/55-01_reading_annotations_a_validator.java`. See [Examples](examples/README.md).
