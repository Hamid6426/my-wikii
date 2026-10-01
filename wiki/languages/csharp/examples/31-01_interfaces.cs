// Lesson 31: Interfaces & Abstractions (../31_interfaces_and_abstractions.md)
// Interfaces, default methods, explicit implementation
// Run: dotnet run 31-01_interfaces.cs

IPayment[] methods = [new Card("4111"), new Cash(), new Wallet(20m)];

foreach (IPayment m in methods)
{
    Console.WriteLine($"{m.Name}: {m.Pay(15m)}");
}

IReadable reader = new Document("Hello");
IWritable writer = new Document("Hello");
Console.WriteLine(reader.Read());
writer.Write("!");
Console.WriteLine(reader.Read());

IA a = new Both();
IB b = new Both();
Console.WriteLine($"{a.Show()} / {b.Show()}");

interface IPayment
{
    string Name { get; }
    string Pay(decimal amount);
    string Receipt(decimal amount) => $"Receipt: {amount:N2} paid by {Name}";   // default method
}

class Card(string last4) : IPayment
{
    public string Name => $"Card ending {last4}";
    public string Pay(decimal amount) => ((IPayment)this).Receipt(amount);
}

class Cash : IPayment
{
    public string Name => "Cash";
    public string Pay(decimal amount) => $"Handed over {amount:N2}";
}

class Wallet(decimal balance) : IPayment
{
    public string Name => "Wallet";
    public string Pay(decimal amount) => amount <= balance ? "Approved" : "Declined";
}

interface IReadable { string Read(); }
interface IWritable { void Write(string text); }

class Document(string text) : IReadable, IWritable
{
    private string _text = text;
    public string Read() => _text;
    public void Write(string more) => _text += more;
}

interface IA { string Show(); }
interface IB { string Show(); }

class Both : IA, IB
{
    string IA.Show() => "from IA";
    string IB.Show() => "from IB";
}

// Expected output should be:
// Card ending 4111: Receipt: 15.00 paid by Card ending 4111
// Cash: Handed over 15.00
// Wallet: Approved
// Hello
// Hello
// from IA / from IB
