# 38 - Events

## What is an Event

A way for one object (the publisher) to tell others (subscribers) that something happened. It is a delegate with extra safety: outsiders can subscribe and unsubscribe, but only the owner can raise it.

---

## A Basic Event

```csharp
class Button
{
    public event Action? Clicked;

    public void Press()
    {
        Clicked?.Invoke();
    }
}

var b = new Button();
b.Clicked += () => Console.WriteLine("Button was clicked");
b.Press();   // Button was clicked
```

---

## The Standard Pattern: EventHandler

Use `EventHandler<T>` so every event looks the same.

```csharp
class OrderEventArgs : EventArgs
{
    public int OrderId { get; }
    public OrderEventArgs(int id) => OrderId = id;
}

class OrderService
{
    public event EventHandler<OrderEventArgs>? OrderPlaced;

    public void Place(int id)
    {
        // ... do the work
        OnOrderPlaced(new OrderEventArgs(id));
    }

    protected virtual void OnOrderPlaced(OrderEventArgs e)
    {
        OrderPlaced?.Invoke(this, e);
    }
}
```

The handler signature is always `(object? sender, TEventArgs e)`.

---

## Subscribing and Unsubscribing

```csharp
var service = new OrderService();

void Handler(object? sender, OrderEventArgs e)
    => Console.WriteLine($"Order {e.OrderId} placed");

service.OrderPlaced += Handler;     // subscribe
service.Place(7);                   // Order 7 placed

service.OrderPlaced -= Handler;     // unsubscribe
```

---

## Event vs Delegate Field

| Feature                 | Public delegate field | `event` |
| ----------------------- | --------------------- | ------- |
| Others can `+=` / `-=`  | Yes                   | Yes     |
| Others can call it      | Yes                   | No      |
| Others can overwrite it | Yes (`=`)             | No      |

---

## Custom Add and Remove

Rarely needed, but you can control storage.

```csharp
private EventHandler? _changed;

public event EventHandler Changed
{
    add => _changed += value;
    remove => _changed -= value;
}
```

---

## Async Event Handlers

```csharp
button.Clicked += async (s, e) =>
{
    await SaveAsync();
};
```

`async void` is accepted here because event handlers cannot return a `Task`. Catch exceptions inside it.

---

## Gotchas

- Always raise with `?.Invoke(...)`; with no subscribers the event is `null`
- Forgetting `-=` keeps the subscriber alive as long as the publisher lives, which is a classic memory leak
- Handlers run one after another on the caller's thread; a slow handler blocks the publisher
- An exception in one handler stops the remaining handlers
- The event can only be raised from inside the class that declares it

---

## Examples

- [38-01](examples/38-01_events.cs): Events with EventHandler

Run one with `dotnet run examples/38-01_events.cs`. See [Examples](examples/README.md).
