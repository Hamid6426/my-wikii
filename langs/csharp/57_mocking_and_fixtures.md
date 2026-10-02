# 57 - Mocking & Fixtures

## Mocking with Moq

Install: `dotnet add package Moq`

```csharp
using Moq;

[Fact]
public void PlaceOrder_SendsConfirmationEmail()
{
    // Arrange
    var mockEmail = new Mock<IEmailService>();
    var service = new OrderService(mockEmail.Object);
    var order = new Order { Email = "a@b.com" };

    // Act
    service.PlaceOrder(order);

    // Assert
    mockEmail.Verify(e => e.Send("a@b.com", "Order confirmed"), Times.Once);
}
```

Setting up return values:

```csharp
var mockRepo = new Mock<IOrderRepository>();
mockRepo.Setup(r => r.GetById(1)).Returns(new Order { Id = 1 });
```

---

## Test Lifecycle in xUnit

```csharp
public class MyTests : IDisposable
{
    private readonly MyService _service;

    public MyTests()
    {
        _service = new MyService();  // runs before each test
    }

    public void Dispose()
    {
        _service.Dispose();          // runs after each test
    }
}
```

---

## Shared Context (IClassFixture)

Expensive setup shared across all tests in a class.

```csharp
public class DatabaseFixture : IDisposable
{
    public DatabaseContext Db { get; } = new DatabaseContext(":memory:");

    public void Dispose() => Db.Dispose();
}

public class UserTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public UserTests(DatabaseFixture fixture) => _fixture = fixture;

    [Fact]
    public void GetUser_ReturnsUser() { ... }
}
```

---

## Gotchas

- Mocking frameworks mock interfaces and virtual methods: they cannot mock sealed classes or non-virtual methods without workarounds
