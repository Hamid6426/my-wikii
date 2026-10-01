# 18 - Arrays

## Declaration and Initialization

An **array** is a fixed-size row of values of one type.

```java
// Declare with a size: every slot gets the default value
int[] nums = new int[5];                 // [0, 0, 0, 0, 0]

// Declare with values
int[] scores = new int[] {90, 85, 78, 92, 88};

// Shorthand, only allowed in a declaration
int[] marks = {90, 85, 78, 92, 88};

// var needs the full form
var names = new String[] {"Alice", "Bob", "Carol"};
```

| Element type                | Default value |
| --------------------------- | ------------- |
| `int`, `long`, `double` ... | `0`, `0.0`    |
| `boolean`                   | `false`       |
| `char`                      | `'\u0000'`    |
| Any object (`String` ...)   | `null`        |

`int nums[]` also compiles (a C habit), but `int[] nums` is the Java style.

---

## Accessing Elements

Indexes start at 0.

```java
int[] nums = {10, 20, 30, 40, 50};

System.out.println(nums[0]);                 // 10
System.out.println(nums[4]);                 // 50
System.out.println(nums[nums.length - 1]);   // 50, the last one

nums[2] = 99;                                // change a value
```

Java has no `nums[-1]` or `nums[^1]`. Use `nums.length - 1`.

---

## Length

`length` is a field, not a method, so it has no `()`.

```java
int[] arr = {1, 2, 3, 4, 5};
System.out.println(arr.length);   // 5
```

---

## Iterating

```java
int[] nums = {1, 2, 3, 4, 5};

// classic for: you get the index
for (int i = 0; i < nums.length; i++) {
    System.out.println(i + ": " + nums[i]);
}

// enhanced for: values only
for (int n : nums) {
    System.out.println(n);
}
```

See [Loops](15_loops.md).

---

## The Arrays Class

`java.util.Arrays` holds the helper methods. Arrays themselves have almost none.

```java
import java.util.Arrays;

public class ArrayTools {
    public static void main(String[] args) {
        int[] arr = {5, 2, 8, 1, 9, 3};

        System.out.println(arr);                    // not useful
        System.out.println(Arrays.toString(arr));

        Arrays.sort(arr);                           // sorts in place
        System.out.println(Arrays.toString(arr));

        int idx = Arrays.binarySearch(arr, 8);      // needs a sorted array
        System.out.println("8 is at " + idx);

        int[] firstThree = Arrays.copyOf(arr, 3);
        int[] middle = Arrays.copyOfRange(arr, 2, 5);   // index 2, 3, 4
        System.out.println(Arrays.toString(firstThree) + " " + Arrays.toString(middle));

        int[] filled = new int[4];
        Arrays.fill(filled, 7);
        System.out.println(Arrays.toString(filled));

        int[] other = {1, 2, 3, 5, 8, 9};
        System.out.println(arr == other);               // different objects
        System.out.println(Arrays.equals(arr, other));  // same contents

        System.out.println(Arrays.stream(arr).sum());
    }
}
```

Expected output should be:

```
[I@501edcf1
[5, 2, 8, 1, 9, 3]
[1, 2, 3, 5, 8, 9]
8 is at 4
[1, 2, 3] [3, 5, 8]
[7, 7, 7, 7]
false
true
28
```

Printing an array directly shows its type code (`[I` means "array of int") and a hash that changes each run, not the values. Always use `Arrays.toString`.

---

## Sorting in Reverse or by a Rule

`Arrays.sort` with a `Comparator` needs an array of objects, not primitives.

```java
Integer[] boxed = {5, 2, 8};
Arrays.sort(boxed, Comparator.reverseOrder());   // [8, 5, 2]

String[] words = {"pear", "fig", "banana"};
Arrays.sort(words, Comparator.comparing(String::length));   // [fig, pear, banana]
```

---

## Arrays Are Reference Types

The variable holds a reference to the array. Assigning copies the reference, not the data. See [Reference Data Types](22_reference_data_types.md).

```java
int[] a = {1, 2, 3};
int[] b = a;                  // both point to the same array
b[0] = 99;
System.out.println(a[0]);     // 99: a changed too

int[] c = a.clone();          // a real copy
int[] d = Arrays.copyOf(a, a.length);   // also a copy
```

---

## Arrays and Lists

An array cannot grow. When the size changes, use `ArrayList` (see [Collections](36_collections.md)).

```java
String[] arr = {"a", "b"};

List<String> fixed = Arrays.asList(arr);        // a view of the array: set works, add throws
List<String> copy = new ArrayList<>(List.of(arr));  // a separate, growable list

String[] back = copy.toArray(String[]::new);    // list to array
```

---

## Checking for null or Empty

```java
int[] arr = {};

if (arr == null || arr.length == 0) {
    System.out.println("Empty or null");
}
```

Return an empty array instead of `null` from your own methods, so callers can always loop.

---

## Gotchas

- Size is fixed at creation: `new int[5]` stays 5 long forever
- A bad index throws `ArrayIndexOutOfBoundsException`
- `==` and `equals` on arrays compare references; use `Arrays.equals`
- `System.out.println(arr)` prints something like `[I@501edcf1`; use `Arrays.toString(arr)`
- `Arrays.sort` changes the array in place and returns nothing
- `Arrays.asList` returns a fixed-size list backed by the array: `add` throws `UnsupportedOperationException`, and changes show up in the array
- `clone()` is a shallow copy: for an array of objects, both arrays share the same objects

---

## Examples

- [18-01](examples/18-01_the_arrays_class.java): The arrays class

Run one with `java examples/18-01_the_arrays_class.java`. See [Examples](examples/README.md).
