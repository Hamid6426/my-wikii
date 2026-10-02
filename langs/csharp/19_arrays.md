# 19 - Arrays

## Declaration and Initialization

```csharp
// Declare, then assign
int[] nums = new int[5];          // [0, 0, 0, 0, 0]: default values

// Declare with initializer
int[] scores = new int[] { 90, 85, 78, 92, 88 };

// Shorthand
int[] scores = { 90, 85, 78, 92, 88 };

// var
var names = new string[] { "Alice", "Bob", "Carol" };
```

---

## Accessing Elements

Zero-indexed.

```csharp
int[] nums = { 10, 20, 30, 40, 50 };

Console.WriteLine(nums[0]);   // 10
Console.WriteLine(nums[4]);   // 50

nums[2] = 99;                 // modify
```

---

## Length

```csharp
int[] arr = { 1, 2, 3, 4, 5 };
Console.WriteLine(arr.Length);  // 5
```

---

## Iterating

```csharp
int[] nums = { 1, 2, 3, 4, 5 };

// for loop
for (int i = 0; i < nums.Length; i++)
{
    Console.WriteLine(nums[i]);
}

// foreach
foreach (int n in nums)
{
    Console.WriteLine(n);
}
```

---

## Array Methods (System.Array)

```csharp
int[] arr = { 5, 2, 8, 1, 9, 3 };

Array.Sort(arr);              // [1, 2, 3, 5, 8, 9]
Array.Reverse(arr);           // [9, 8, 5, 3, 2, 1]
int idx = Array.IndexOf(arr, 5);  // index of value 5
Array.Copy(arr, dest, 3);    // copy first 3 elements to dest
```

---

## Index from End (C# 8+)

```csharp
int[] arr = { 10, 20, 30, 40, 50 };
Console.WriteLine(arr[^1]);  // 50 (last)
Console.WriteLine(arr[^2]);  // 40 (second-to-last)
```

---

## Range Slicing (C# 8+)

Returns a sub-array.

```csharp
int[] arr = { 10, 20, 30, 40, 50 };
int[] slice = arr[1..4];   // [20, 30, 40]: indices 1, 2, 3
int[] first3 = arr[..3];   // [10, 20, 30]
int[] last2  = arr[^2..];  // [40, 50]
```

---

## Array as Reference Type

Arrays are reference types: assigning copies the reference, not the data.

```csharp
int[] a = { 1, 2, 3 };
int[] b = a;          // both point to same array
b[0] = 99;
Console.WriteLine(a[0]);  // 99: a is also changed

// To copy: use Array.Copy or Clone
int[] c = (int[])a.Clone();
```

---

## Checking if Empty or Null

```csharp
int[] arr = {};

if (arr == null || arr.Length == 0)
{
    Console.WriteLine("Empty or null");
}
```

---

## Gotchas

- Array size is fixed at creation: cannot grow or shrink (use `List<T>` for dynamic size)
- Accessing out-of-bounds index throws `IndexOutOfRangeException`
- Arrays are reference types; assignment copies the reference, not the content
- `Array.Sort` modifies in place; it does not return a new array

---

## Examples

- [19-01](examples/19-01_array_basics.cs): Create, read, change, slice, sort arrays

Run one with `dotnet run examples/19-01_array_basics.cs`. See [Examples](examples/README.md).
