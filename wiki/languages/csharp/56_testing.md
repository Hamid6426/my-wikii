# 56 - Testing

## Why Test

Tests catch regressions, document expected behavior, and give confidence to refactor.

---

## Test Project Setup

```bash
dotnet new xunit -n MyApp.Tests
cd MyApp.Tests
dotnet add reference ../MyApp/MyApp.csproj
dotnet test
```

Popular frameworks: **xUnit** (recommended), NUnit, MSTest.

---

## xUnit Basics

```csharp
using Xunit;

public class CalculatorTests
{
    [Fact]
    public void Add_ReturnsCorrectSum()
    {
        // Arrange
        var calc = new Calculator();

        // Act
        int result = calc.Add(2, 3);

        // Assert
        Assert.Equal(5, result);
    }
}
```

`[Fact]`: a single test case with no parameters.

---

## Parameterized Tests

```csharp
public class MathTests
{
    [Theory]
    [InlineData(2, 3, 5)]
    [InlineData(-1, 1, 0)]
    [InlineData(0, 0, 0)]
    public void Add_ReturnsExpectedResult(int a, int b, int expected)
    {
        var calc = new Calculator();
        Assert.Equal(expected, calc.Add(a, b));
    }
}
```

`[Theory]` + `[InlineData]`: one test method, many data sets.

---

## Common xUnit Assertions

```csharp
Assert.Equal(expected, actual);
Assert.NotEqual(a, b);
Assert.True(condition);
Assert.False(condition);
Assert.Null(obj);
Assert.NotNull(obj);
Assert.Contains(item, collection);
Assert.DoesNotContain(item, collection);
Assert.Empty(collection);
Assert.NotEmpty(collection);
Assert.IsType<ExpectedType>(obj);
Assert.Throws<ArgumentException>(() => BadMethod());
```

---

## Testing Exceptions

```csharp
[Fact]
public void Divide_ByZero_ThrowsArgumentException()
{
    var calc = new Calculator();

    Assert.Throws<ArgumentException>(() => calc.Divide(10, 0));
}

// Async version
[Fact]
public async Task FetchAsync_BadUrl_ThrowsHttpException()
{
    await Assert.ThrowsAsync<HttpRequestException>(
        () => _client.FetchAsync("bad-url"));
}
```

---

## Arrange-Act-Assert (AAA) Pattern

```csharp
[Fact]
public void ProcessOrder_SetsStatusToConfirmed()
{
    // Arrange
    var order = new Order { Id = 1, Total = 50m };
    var service = new OrderService();

    // Act
    service.Process(order);

    // Assert
    Assert.Equal(OrderStatus.Confirmed, order.Status);
}
```

---

## Running Tests

```bash
dotnet test                          # run all
dotnet test --filter "Add"           # filter by name
dotnet test --logger "console;verbosity=detailed"
dotnet test /p:CollectCoverage=true  # with code coverage
```

---

## Gotchas

- Tests should be independent: no shared mutable state between tests
- Name tests clearly: `MethodName_Condition_ExpectedResult`
- Do not test private methods directly: test through public API
- Avoid `Thread.Sleep` in tests; use async/await with `Task.Delay` if needed
