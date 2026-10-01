# 48 - JSON

## No JSON in the JDK

The JDK has no JSON parser. You add a library. **Jackson** is the most used one, and Spring uses it by default. This lesson uses **Jackson 3**, the current major version.

| Library | Package to add                                                   | Notes                                 |
| ------- | ---------------------------------------------------------------- | ------------------------------------- |
| Jackson | `tools.jackson.core:jackson-databind`                            | The usual choice. Used in this lesson |
| Gson    | `com.google.code.gson:gson`                                      | Simple, from Google                   |
| JSON-B  | `jakarta.json.bind:jakarta.json.bind-api` plus an implementation | The Jakarta EE standard               |

---

## Add the Dependency

Maven (`pom.xml`):

```xml
<dependency>
    <groupId>tools.jackson.core</groupId>
    <artifactId>jackson-databind</artifactId>
    <version>3.1.5</version>
</dependency>
```

Gradle (`build.gradle.kts`):

```kotlin
implementation("tools.jackson.core:jackson-databind:3.1.5")
```

Use the newest 3.x version from [Maven Central](https://central.sonatype.com/artifact/tools.jackson.core/jackson-databind). `jackson-databind` pulls in `jackson-core` and `jackson-annotations` for you. Build tools are covered in [Projects and Build Tools](02_projects_and_build_tools.md).

To try a single file without a build tool, put the three jars in a `libs` folder and run `java -cp "libs/*" Main.java`.

---

## Object to JSON and Back

**Serialize** means turn an object into JSON text. **Deserialize** means the opposite.

```java
import tools.jackson.databind.ObjectMapper;
import tools.jackson.databind.json.JsonMapper;

public class JsonRoundTrip {
    record Person(String name, int age) {}

    public static void main(String[] args) {
        ObjectMapper mapper = JsonMapper.builder().build();

        String json = mapper.writeValueAsString(new Person("Alice", 30));
        System.out.println(json);

        Person bob = mapper.readValue("""
            {"name":"Bob","age":25}
            """, Person.class);
        System.out.println(bob);
        System.out.println(bob.name());
    }
}
```

Expected output should be:

```
{"name":"Alice","age":30}
Person[name=Bob, age=25]
Bob
```

Records work with no extra setup. A normal class needs a no-argument constructor plus getters and setters, or the annotations below.

Create one mapper and share it. It is safe to use from many threads, and building one is slow.

---

## Pretty Printing

```java
String pretty = mapper.writerWithDefaultPrettyPrinter().writeValueAsString(person);
```

Expected output should be:

```
{
  "name" : "Alice",
  "age" : 30
}
```

---

## Mapper Settings

A Jackson 3 mapper cannot be changed after it is built. Set options on the builder.

```java
JsonMapper mapper = JsonMapper.builder()
    .enable(SerializationFeature.INDENT_OUTPUT)                 // always pretty print
    .enable(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES)  // reject extra fields
    .changeDefaultPropertyInclusion(incl -> incl.withValueInclusion(JsonInclude.Include.NON_NULL))  // skip nulls
    .build();
```

---

## Annotations

The annotations live in `com.fasterxml.jackson.annotation`, even in Jackson 3.

```java
import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;

@JsonInclude(JsonInclude.Include.NON_NULL)
record Product(
    @JsonProperty("product_id") int id,     // name in the JSON
    @JsonIgnore String internalNote,        // never written or read
    double price) {}

mapper.writeValueAsString(new Product(7, "secret", 9.5));   // {"product_id":7,"price":9.5}
```

| Annotation                                    | Effect                           |
| --------------------------------------------- | -------------------------------- |
| `@JsonProperty("x")`                          | Use `x` as the JSON name         |
| `@JsonIgnore`                                 | Skip this field                  |
| `@JsonInclude(Include.NON_NULL)`              | Leave out fields that are `null` |
| `@JsonIgnoreProperties(ignoreUnknown = true)` | Allow extra fields in the input  |
| `@JsonFormat(pattern = "dd/MM/yyyy")`         | Format for a date field          |

How annotations work is covered in [Annotations and Reflection](55_annotations_and_reflection.md).

---

## Lists and Maps

Generic types lose their type argument at runtime (see [Generics](54_generics.md)), so `List<Person>.class` does not exist. Pass a `TypeReference` instead.

```java
import tools.jackson.core.type.TypeReference;

List<Person> people = mapper.readValue(json, new TypeReference<List<Person>>() {});
Map<String, Integer> counts = mapper.readValue("""
    {"a":1,"b":2}
    """, new TypeReference<>() {});
```

---

## Dates

Jackson 3 handles `java.time` types out of the box and writes them as ISO text.

```java
record Event(String title, LocalDate date) {}

mapper.writeValueAsString(new Event("Launch", LocalDate.of(2026, 10, 1)));
// {"title":"Launch","date":"2026-10-01"}
```

See [Dates and Times](21_dates_and_times.md).

---

## Reading and Writing Files

```java
mapper.writeValue(Path.of("people.json"), people);

List<Person> loaded = mapper.readValue(Path.of("people.json"), new TypeReference<List<Person>>() {});
```

---

## Reading Without a Class: JsonNode

Read JSON as a tree when you need only a few values or do not know its shape.

```java
JsonNode root = mapper.readTree("""
    {"user":{"name":"Alice","tags":["x","y"]}}
    """);

root.get("user").get("name").asString()           // Alice
root.path("user").path("missing").asString("none")   // none, path() never returns null
root.at("/user/tags/1").asString()                // y, a JSON Pointer path
```

Edit a tree with `ObjectNode`:

```java
ObjectNode node = (ObjectNode) mapper.readTree("""
    {"count":1}
    """);
node.put("count", 2);
node.put("extra", "hi");
System.out.println(node);   // {"count":2,"extra":"hi"}
```

---

## Handling Bad Input

Jackson 3 errors extend `JacksonException`, which is unchecked.

```java
try {
    Person p = mapper.readValue(text, Person.class);
} catch (JacksonException e) {
    System.out.println("Bad JSON: " + e.getOriginalMessage());
}
```

| Input                           | Exception                  |
| ------------------------------- | -------------------------- |
| `{"name":"Bob",` (cut off)      | `StreamReadException`      |
| `{"age":"old"}` for an `int`    | `InvalidFormatException`   |
| `{"name":"Bob"}`, `age` missing | `MismatchedInputException` |

---

## Jackson 2

Much existing code still uses Jackson 2. The ideas are the same, but names differ.

| Jackson 2                                              | Jackson 3                                       |
| ------------------------------------------------------ | ----------------------------------------------- |
| Package `com.fasterxml.jackson.databind`               | `tools.jackson.databind`                        |
| Maven group `com.fasterxml.jackson.core`               | `tools.jackson.core`                            |
| `new ObjectMapper()`, then `configure(...)`            | `JsonMapper.builder()...build()`, cannot change |
| `JsonProcessingException`, checked                     | `JacksonException`, unchecked                   |
| `node.asText()`                                        | `node.asString()`                               |
| `java.time` needs the `jackson-datatype-jsr310` module | Built in                                        |
| Unknown properties fail by default                     | Unknown properties are ignored by default       |
| A missing `int` field becomes `0`                      | A missing `int` field fails                     |

---

## Gotchas

- Jackson is not part of the JDK. Without the dependency, the imports do not compile
- Do not mix Jackson 2 and Jackson 3 imports in one class. The types do not work together
- A missing `int` field fails in Jackson 3. Use `Integer` if the field is optional, since it can hold `null`
- `readValue` with the text `null` returns `null`, not an exception
- A class with only private fields and no getters serializes as `{}` with no error. Give it getters, or use a record
- Create the mapper once. A new one per call is slow
