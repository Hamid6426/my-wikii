# 65 - Common Frameworks

A framework gives you the structure of a whole app and calls your code. A library is something you call. This page is a map only. Each framework gets its own folder in the wiki when it is studied. Check each site for the current version.

## Web and Services

| Framework        | What it is for                                                                                       | Site                                                                               |
| ---------------- | ---------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------- |
| Spring Boot      | The most used Java backend framework: web apps, REST APIs, batch jobs                                | [spring.io/projects/spring-boot](https://spring.io/projects/spring-boot)           |
| Spring Framework | The core under Spring Boot: dependency injection, web MVC, data access                               | [spring.io/projects/spring-framework](https://spring.io/projects/spring-framework) |
| Jakarta EE       | The standard enterprise APIs (servlets, REST, persistence), run on servers such as WildFly or Payara | [jakarta.ee](https://jakarta.ee/)                                                  |
| Quarkus          | Fast-starting, low-memory services, built for containers and native images                           | [quarkus.io](https://quarkus.io/)                                                  |
| Micronaut        | Services with dependency injection resolved at compile time                                          | [micronaut.io](https://micronaut.io/)                                              |
| Helidon          | Small microservices from Oracle, built on virtual threads                                            | [helidon.io](https://helidon.io/)                                                  |
| Javalin          | A tiny web framework: a few lines for an HTTP API                                                    | [javalin.io](https://javalin.io/)                                                  |
| Vert.x           | Event-driven, non-blocking network apps                                                              | [vertx.io](https://vertx.io/)                                                      |

---

## Data

| Framework     | What it is for                                                                                               | Site                                                                     |
| ------------- | ------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------ |
| JDBC          | Built into the JDK (`java.sql`): low-level database access the others sit on                                 | [JDBC tutorial](https://docs.oracle.com/javase/tutorial/jdbc/)           |
| Hibernate ORM | Object-relational mapper (ORM): use Java classes instead of SQL tables. Implements Jakarta Persistence (JPA) | [hibernate.org/orm](https://hibernate.org/orm/)                          |
| Spring Data   | Repositories generated from interfaces, for SQL and NoSQL databases                                          | [spring.io/projects/spring-data](https://spring.io/projects/spring-data) |
| jOOQ          | Type-safe SQL written in Java                                                                                | [jooq.org](https://www.jooq.org/)                                        |
| MyBatis       | Maps SQL you write by hand to objects                                                                        | [mybatis.org](https://mybatis.org/mybatis-3/)                            |

---

## Desktop

| Framework | What it is for                                                | Site                                                               |
| --------- | ------------------------------------------------------------- | ------------------------------------------------------------------ |
| JavaFX    | Modern desktop apps with FXML layouts and CSS styling         | [openjfx.io](https://openjfx.io/)                                  |
| Swing     | The older desktop toolkit, built into the JDK (`javax.swing`) | [Swing tutorial](https://docs.oracle.com/javase/tutorial/uiswing/) |
| SWT       | Native-looking widgets, used by the Eclipse IDE               | [eclipse.dev/eclipse/swt](https://eclipse.dev/eclipse/swt/)        |

---

## Mobile

| Framework   | What it is for                                             | Site                                                    |
| ----------- | ---------------------------------------------------------- | ------------------------------------------------------- |
| Android SDK | Android apps. Java works, but Google now leads with Kotlin | [developer.android.com](https://developer.android.com/) |
| Gluon       | JavaFX apps on Android and iOS                             | [gluonhq.com](https://gluonhq.com/)                     |

---

## Games and Graphics

| Framework     | What it is for                                | Site                                            |
| ------------- | --------------------------------------------- | ----------------------------------------------- |
| libGDX        | Cross-platform 2D and 3D games                | [libgdx.com](https://libgdx.com/)               |
| jMonkeyEngine | Open-source 3D game engine                    | [jmonkeyengine.org](https://jmonkeyengine.org/) |
| LWJGL         | Low-level access to OpenGL, Vulkan, and audio | [lwjgl.org](https://www.lwjgl.org/)             |

---

## Big Data and Messaging

| Framework    | What it is for                                          | Site                                                                       |
| ------------ | ------------------------------------------------------- | -------------------------------------------------------------------------- |
| Apache Kafka | Event streaming: services publish and read messages     | [kafka.apache.org](https://kafka.apache.org/)                              |
| Apache Spark | Large-scale data processing across many machines        | [spark.apache.org](https://spark.apache.org/)                              |
| Apache Flink | Real-time stream processing                             | [flink.apache.org](https://flink.apache.org/)                              |
| Apache Camel | Connects systems with ready-made integration routes     | [camel.apache.org](https://camel.apache.org/)                              |
| Spring Batch | Large scheduled jobs that read, process, and write data | [spring.io/projects/spring-batch](https://spring.io/projects/spring-batch) |

---

## AI and Machine Learning

| Framework   | What it is for                                        | Site                                                                 |
| ----------- | ----------------------------------------------------- | -------------------------------------------------------------------- |
| LangChain4j | Call large language models from Java                  | [docs.langchain4j.dev](https://docs.langchain4j.dev/)                |
| Spring AI   | AI model access in the Spring style                   | [spring.io/projects/spring-ai](https://spring.io/projects/spring-ai) |
| Tribuo      | Classic machine learning (classification, clustering) | [tribuo.org](https://tribuo.org/)                                    |

---

## Where to Start

- Web or backend work: Spring Boot, then Spring Data with Hibernate
- Small, fast services or containers: Quarkus or Micronaut
- Desktop: JavaFX
- Games: libGDX
- Android: the Android SDK (and learn Kotlin alongside Java)

The Java language lessons here apply to all of them. Learn the language first, then pick one framework. Libraries you call yourself are in [Common Libraries](66_libraries.md).
