// Lesson 35: Queue, Stack & More Collections (../35_queue_stack_and_more_collections.md)
// Queue, Stack, LinkedList, SortedDictionary
// Run: dotnet run 35-01_queue_stack_linkedlist.cs

var line = new Queue<string>();
line.Enqueue("Ann");
line.Enqueue("Ben");
line.Enqueue("Cy");
Console.WriteLine($"Serving {line.Dequeue()}, next is {line.Peek()}, waiting: {line.Count}");

var history = new Stack<string>();
history.Push("page1");
history.Push("page2");
history.Push("page3");
Console.WriteLine($"Back from {history.Pop()} to {history.Peek()}");

Console.WriteLine(IsBalanced("{[()]}"));
Console.WriteLine(IsBalanced("{[(])}"));

var list = new LinkedList<int>(new[] { 1, 2, 4 });
var node = list.Find(2)!;
list.AddAfter(node, 3);
list.AddFirst(0);
Console.WriteLine(string.Join(" -> ", list));

var sorted = new SortedDictionary<string, int> { ["pear"] = 3, ["apple"] = 7, ["fig"] = 1 };
Console.WriteLine(string.Join(", ", sorted.Select(kv => $"{kv.Key}:{kv.Value}")));

static bool IsBalanced(string text)
{
    var pairs = new Dictionary<char, char> { [')'] = '(', [']'] = '[', ['}'] = '{' };
    var stack = new Stack<char>();

    foreach (char c in text)
    {
        if (pairs.ContainsValue(c)) stack.Push(c);
        else if (pairs.TryGetValue(c, out char open))
        {
            if (stack.Count == 0 || stack.Pop() != open) return false;
        }
    }
    return stack.Count == 0;
}

// Expected output should be:
// Serving Ann, next is Ben, waiting: 2
// Back from page3 to page2
// True
// False
// 0 -> 1 -> 2 -> 3 -> 4
// apple:7, fig:1, pear:3
