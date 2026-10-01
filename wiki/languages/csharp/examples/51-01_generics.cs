// Lesson 51: Generics (../51_generics.md)
// Generic classes, methods, and constraints
// Run: dotnet run 51-01_generics.cs

var stack = new SimpleStack<string>();
stack.Push("a");
stack.Push("b");
Console.WriteLine($"{stack.Pop()} {stack.Pop()} {stack.Count}");

var numbers = new SimpleStack<int>();
numbers.Push(1);
numbers.Push(2);
Console.WriteLine(numbers.Pop() + numbers.Pop());

Console.WriteLine(Largest(3, 9, 4));
Console.WriteLine(Largest("pear", "apple", "zebra"));
Console.WriteLine(Largest(2.5, 1.5));

var (x, y) = (1, 2);
Swap(ref x, ref y);
Console.WriteLine($"{x} {y}");

var pair = new Pair<string, int>("age", 30);
Console.WriteLine(pair);

Console.WriteLine(Create<List<int>>().Count);

static T Largest<T>(params T[] items) where T : IComparable<T>
{
    T best = items[0];
    foreach (T item in items)
    {
        if (item.CompareTo(best) > 0) best = item;
    }
    return best;
}

static void Swap<T>(ref T a, ref T b) => (a, b) = (b, a);

static T Create<T>() where T : new() => new T();

class SimpleStack<T>
{
    private readonly List<T> _items = [];
    public int Count => _items.Count;

    public void Push(T item) => _items.Add(item);

    public T Pop()
    {
        if (_items.Count == 0) throw new InvalidOperationException("Stack is empty");
        T item = _items[^1];
        _items.RemoveAt(_items.Count - 1);
        return item;
    }
}

record Pair<TKey, TValue>(TKey Key, TValue Value);

// Expected output should be:
// b a 0
// 3
// 9
// zebra
// 2.5
// 2 1
// Pair { Key = age, Value = 30 }
// 0
