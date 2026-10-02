// Lesson 55: Design Patterns (../55_design_patterns.md)
// Observer, Command with undo, and Builder
// Run: dotnet run 55-02_observer_command_builder.cs

// Observer
var stock = new Stock("ACME", 100m);
stock.PriceChanged += (name, price) => Console.WriteLine($"  alert: {name} is now {price:N2}");
stock.Price = 105m;
stock.Price = 98.5m;

// Command with undo
var editor = new Editor();
var history = new Stack<ICommand>();

void Run(ICommand command)
{
    command.Do();
    history.Push(command);
    Console.WriteLine($"  text: \"{editor.Text}\"");
}

Run(new Append(editor, "Hello"));
Run(new Append(editor, ", world"));
Run(new Append(editor, "!"));

history.Pop().Undo();
Console.WriteLine($"  after undo: \"{editor.Text}\"");

// Builder
var email = new EmailBuilder()
    .To("alice@example.com")
    .Subject("Report")
    .Body("See attached.")
    .Build();
Console.WriteLine(email);

class Stock(string name, decimal price)
{
    private decimal _price = price;
    public event Action<string, decimal>? PriceChanged;

    public decimal Price
    {
        get => _price;
        set
        {
            if (value == _price) return;
            _price = value;
            PriceChanged?.Invoke(name, value);
        }
    }
}

class Editor { public string Text { get; set; } = ""; }

interface ICommand { void Do(); void Undo(); }

class Append(Editor editor, string text) : ICommand
{
    public void Do() => editor.Text += text;
    public void Undo() => editor.Text = editor.Text[..^text.Length];
}

record Email(string To, string Subject, string Body);

class EmailBuilder
{
    private string _to = "", _subject = "", _body = "";

    public EmailBuilder To(string to) { _to = to; return this; }
    public EmailBuilder Subject(string subject) { _subject = subject; return this; }
    public EmailBuilder Body(string body) { _body = body; return this; }

    public Email Build()
    {
        if (_to == "") throw new InvalidOperationException("Recipient is required");
        return new Email(_to, _subject, _body);
    }
}

// Expected output should be:
//   alert: ACME is now 105.00
//   alert: ACME is now 98.50
//   text: "Hello"
//   text: "Hello, world"
//   text: "Hello, world!"
//   after undo: "Hello, world"
// Email { To = alice@example.com, Subject = Report, Body = See attached. }
