# 42 - Error Handling

## try / catch / finally

```csharp
try
{
    int result = 10 / 0;                    // throws DivideByZeroException
    string s = null;
    Console.WriteLine(s.Length);            // throws NullReferenceException
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"Division error: {ex.Message}");
}
catch (NullReferenceException ex)
{
    Console.WriteLine($"Null error: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
}
finally
{
    Console.WriteLine("Always runs: cleanup here");
}
```

- `catch` blocks are checked top-to-bottom: put specific before general
- `finally` runs whether or not an exception occurred

---

## Exception Properties

```csharp
catch (Exception ex)
{
    Console.WriteLine(ex.Message);        // short description
    Console.WriteLine(ex.StackTrace);     // call stack
    Console.WriteLine(ex.GetType().Name); // exception type name
    Console.WriteLine(ex.InnerException); // wrapped exception (if any)
}
```

---

## Throwing Exceptions

```csharp
static int Divide(int a, int b)
{
    if (b == 0)
        throw new ArgumentException("Divisor cannot be zero", nameof(b));
    return a / b;
}
```

Re-throw preserving stack trace:

```csharp
catch (Exception ex)
{
    Log(ex);
    throw;          // re-throw original, stack trace preserved
    // throw ex;    // avoid: resets stack trace to this line
}
```

---

## Common Exception Types

| Exception                     | When thrown                              |
| ----------------------------- | ---------------------------------------- |
| `NullReferenceException`      | Accessing member on null reference       |
| `ArgumentNullException`       | null argument where not allowed          |
| `ArgumentException`           | Invalid argument value                   |
| `ArgumentOutOfRangeException` | Argument outside valid range             |
| `InvalidOperationException`   | Method call invalid in current state     |
| `IndexOutOfRangeException`    | Array index out of bounds                |
| `DivideByZeroException`       | Integer division by zero                 |
| `FormatException`             | Bad string format in `Parse`             |
| `OverflowException`           | Arithmetic overflow in `checked` context |
| `IOException`                 | File/stream I/O failure                  |
| `NotImplementedException`     | Placeholder for unimplemented method     |

---

## Custom Exceptions

```csharp
class InsufficientFundsException : Exception
{
    public decimal Amount { get; }

    public InsufficientFundsException(decimal amount)
        : base($"Insufficient funds. Required: {amount}")
    {
        Amount = amount;
    }
}

// Usage
throw new InsufficientFundsException(100.50m);
```

---

## Exception Filters (C# 6+)

```csharp
catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
{
    Console.WriteLine("Resource not found");
}

catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
{
    Console.WriteLine("Unauthorized");
}
```

---

## using Statement for Cleanup

Automatically calls `Dispose()` when leaving the block.

```csharp
using (var reader = new StreamReader("file.txt"))
{
    string content = reader.ReadToEnd();
}  // reader.Dispose() called here, even if exception occurs

// C# 8+: without braces
using var writer = new StreamWriter("out.txt");
writer.WriteLine("Hello");
// writer.Dispose() called at end of enclosing scope
```

---

## checked / unchecked

```csharp
int max = int.MaxValue;

// unchecked (default): wraps silently
int overflow = max + 1;  // -2147483648

// checked: throws OverflowException
checked
{
    int safe = max + 1;  // throws
}
```

---

## Gotchas

- Never catch `Exception` without re-throwing or logging: it hides real bugs
- `throw ex` resets the stack trace; `throw` alone preserves it
- `finally` runs even if a `return` statement is hit inside `try`
- `using` is preferred over `try/finally` for resource cleanup
- Validate arguments at method entry and throw `ArgumentException` early: don't let bad input propagate deep

---

## Examples

- [42-01](examples/42-01_error_handling.cs): try, catch, finally, throw, custom exceptions

Run one with `dotnet run examples/42-01_error_handling.cs`. See [Examples](examples/README.md).
