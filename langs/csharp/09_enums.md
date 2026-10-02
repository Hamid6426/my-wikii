# 09 - Enums

## What is an Enum

A named set of constants. Use it instead of magic numbers or strings.

```csharp
enum Status
{
    Pending,    // 0
    Active,     // 1
    Closed      // 2
}

Status s = Status.Active;
Console.WriteLine(s);        // Active
Console.WriteLine((int)s);   // 1
```

---

## Custom Values and Underlying Type

```csharp
enum HttpCode : short
{
    Ok = 200,
    NotFound = 404,
    ServerError = 500
}
```

The default underlying type is `int`. Allowed types: `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`.

---

## Converting

```csharp
Status s = (Status)1;                          // from int
Status p = Enum.Parse<Status>("Pending");      // from string, throws if invalid
bool ok = Enum.TryParse("Closed", out Status c);  // safe version
string name = s.ToString();                    // to string
```

---

## Looping Over Values

```csharp
foreach (Status s in Enum.GetValues<Status>())
{
    Console.WriteLine($"{s} = {(int)s}");
}
```

---

## Flags Enums

Combine several options in one value. Use powers of two.

```csharp
[Flags]
enum Permission
{
    None    = 0,
    Read    = 1,
    Write   = 2,
    Execute = 4,
    All     = Read | Write | Execute
}

var p = Permission.Read | Permission.Write;

Console.WriteLine(p);                              // Read, Write
Console.WriteLine(p.HasFlag(Permission.Write));    // true
p &= ~Permission.Write;                            // remove Write
```

---

## Enums in switch

```csharp
string label = status switch
{
    Status.Pending => "Waiting",
    Status.Active  => "Running",
    Status.Closed  => "Done",
    _              => "Unknown"
};
```

---

## Gotchas

- Any `int` can be cast to an enum, even one with no matching name: `(Status)99` compiles and runs
- Check with `Enum.IsDefined(typeof(Status), value)` before trusting outside input
- Without `[Flags]`, `ToString()` on a combined value prints the number, not the names
- Keep `None = 0` first in a flags enum so the default value means "nothing"
- Adding a member in the middle of a plain enum shifts the numbers after it, which breaks stored data

---

## Examples

- [09-01](examples/09-01_enums.cs): Enums and flags

Run one with `dotnet run examples/09-01_enums.cs`. See [Examples](examples/README.md).
