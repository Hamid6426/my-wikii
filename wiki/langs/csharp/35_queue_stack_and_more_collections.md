# 35 - Queue, Stack & More Collections

## Queue\<T\>

FIFO (first in, first out).

```csharp
var queue = new Queue<string>();

queue.Enqueue("first");
queue.Enqueue("second");
queue.Enqueue("third");

string next = queue.Dequeue();   // "first": removes it
string peek = queue.Peek();      // "second": does not remove
queue.Count;                     // 2
```

---

## Stack\<T\>

LIFO (last in, first out).

```csharp
var stack = new Stack<int>();

stack.Push(1);
stack.Push(2);
stack.Push(3);

int top  = stack.Pop();    // 3: removes it
int peek = stack.Peek();   // 2: does not remove
stack.Count;               // 2
```

---

## LinkedList\<T\>

Doubly linked list. O(1) insert/remove at any node.

```csharp
var list = new LinkedList<int>();
list.AddLast(1);
list.AddLast(2);
list.AddFirst(0);

LinkedListNode<int> node = list.Find(1);
list.AddAfter(node, 10);
list.Remove(node);
```

---

## SortedDictionary\<TKey, TValue\>

Like `Dictionary` but always sorted by key.

```csharp
var sorted = new SortedDictionary<string, int>
{
    ["banana"] = 2,
    ["apple"]  = 5,
    ["cherry"] = 1
};

foreach (var kvp in sorted)
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}
// apple: 5, banana: 2, cherry: 1
```

---

## Collection Comparison

| Collection              | Order   | Duplicates | Key Access | Complexity (add/get) |
| ----------------------- | ------- | ---------- | ---------- | -------------------- |
| `List<T>`               | Ordered | Yes        | By index   | O(1) / O(1)          |
| `Dictionary<K,V>`       | None    | Keys: No   | By key     | O(1) / O(1)          |
| `HashSet<T>`            | None    | No         | None       | O(1) / O(1)          |
| `Queue<T>`              | FIFO    | Yes        | None       | O(1) / O(1)          |
| `Stack<T>`              | LIFO    | Yes        | None       | O(1) / O(1)          |
| `SortedDictionary<K,V>` | Sorted  | Keys: No   | By key     | O(log n) / O(log n)  |
| `LinkedList<T>`         | Ordered | Yes        | By node    | O(1) / O(n)          |

---

## Gotchas

- `Queue.Dequeue()` and `Stack.Pop()` throw `InvalidOperationException` on empty collections: check `Count` first or use `TryDequeue`/`TryPop`

---

## Examples

- [35-01](examples/35-01_queue_stack_linkedlist.cs): Queue, Stack, LinkedList, SortedDictionary

Run one with `dotnet run examples/35-01_queue_stack_linkedlist.cs`. See [Examples](examples/README.md).
